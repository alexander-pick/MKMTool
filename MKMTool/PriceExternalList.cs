using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static MKMTool.MKMHelpers;

namespace MKMTool
{
    public partial class PriceExternalList : Form
    {
        UpdatePriceSettings botSettings = new UpdatePriceSettings("LastSettingsPresetExtList", "Settings of Price External List");
        MKMBot bot = new MKMBot();

        List<MKMMetaCard> importedAll = new List<MKMMetaCard>();
        List<MKMMetaCard> importedValidOnly = new List<MKMMetaCard>();
        DataColumnCollection importedColumns;

        bool priceGuidesExported = false; // flags tracking whether the user exported the computed prices, used to give warning that there are unsaved changes
        bool toolPriceExported = false;
        bool priceGuidesGenerated = false; // turns to true if at least for one article the price guide has been fetched from MKM
        bool toolPriceGenerated = false;// turns to true if at least for one article has the price computed by MKMTool

        public PriceExternalList()
        {
            InitializeComponent();

            comboBoxFoil.SelectedIndex = 0;
            comboBoxSigned.SelectedIndex = 0;
            comboBoxPlayset.SelectedIndex = 0;
            comboBoxAltered.SelectedIndex = 0;
            comboBoxCondition.SelectedIndex = 2;
            comboBoxExpansion.SelectedIndex = 0;

            foreach (var Lang in MKMHelpers.languagesNames)
            {
                var item = new MKMHelpers.ComboboxItem();

                item.Text = Lang.Value;
                item.Value = Lang.Key;

                comboBoxLanguage.Items.Add(item);
            }

            comboBoxLanguage.SelectedIndex = 0;
            comboBoxExportUploadPrice.SelectedIndex = 0;

            buttonExport.Enabled = false;
            buttonExportToMKM.Enabled = false;
            buttonAppraise.Enabled = false;

            checkBoxExportPriceGuide.Enabled = false;
            checkBoxExportToolPrices.Enabled = false;
            checkBoxExportPriceGuide.Checked = false;
            checkBoxExportToolPrices.Checked = false;
        }

        private void checkBoxToolPrices_CheckedChanged(object sender, EventArgs e)
        {
            buttonBotSettings.Enabled = checkBoxToolPrices.Enabled;
            if (!buttonBotSettings.Enabled && !botSettings.IsDisposed)
                botSettings.Close();
        }

        private async void buttonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog import = new OpenFileDialog();
            import.Multiselect = false;
            import.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (import.ShowDialog() == DialogResult.OK)
            {
                buttonImport.Enabled = false;
                buttonAppraise.Enabled = false;
                buttonExport.Enabled = false;
                buttonExportToMKM.Enabled = false;

                checkBoxExportPriceGuide.Enabled = false;
                checkBoxExportToolPrices.Enabled = false;
                checkBoxExportPriceGuide.Checked = false;
                checkBoxExportToolPrices.Checked = false;

                priceGuidesGenerated = false;
                toolPriceGenerated = false;
                priceGuidesExported = false;
                toolPriceExported = false;

                buttonImport.Text = "Importing...";

                string defaultFoil = comboBoxFoil.SelectedItem.ToString();
                string defaultPlayset = comboBoxPlayset.SelectedItem.ToString();
                string defaultSigned = comboBoxSigned.SelectedItem.ToString();
                string defaultAltered = comboBoxAltered.SelectedItem.ToString();
                string defaultCondition = comboBoxCondition.SelectedItem.ToString();
                string defaultExpansion = comboBoxExpansion.SelectedItem.ToString();
                string defaultLanguageID = ((MKMHelpers.ComboboxItem)comboBoxLanguage.SelectedItem).Value.ToString();

                await Task.Run(() => importRun(import.FileName, defaultFoil, defaultPlayset, defaultSigned, defaultAltered, 
                    defaultCondition, defaultExpansion, defaultLanguageID));

                buttonImport.Enabled = true;
                if (importedValidOnly.Count > 0)
                {
                    buttonAppraise.Enabled = true;
                    buttonExport.Enabled = true;
                    buttonExportToMKM.Enabled = true;
                }
                buttonImport.Text = "Import CSV file...";
            }
        }

        /// <summary>
        /// Imports and processes the input CSV file, i.e. fills the internal MetaCard hash tables and fetches product info from MKM if necessary.
        /// Intended to run in a separate thread.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        private void importRun(string filePath, string defaultFoil, string defaultPlayset, string defaultSigned, string defaultAltered,
            string defaultCondition, string defaultExpansion, string defaultLanguageID)
        {
            DataTable dt;
            try
            {
                 dt = MKMDatabaseManager.ConvertCSVtoDataTable(filePath);
            }
            catch (Exception eError)
            {
                LogError("importing file " + filePath, eError.Message, true);
                return;
            }
            importedColumns = dt.Columns;
            MainView.Instance.LogMainWindow("Loaded file with " + dt.Rows.Count + " articles, processing...");

            importedAll.Clear();
            importedValidOnly.Clear();
            int counter = 0, failed = 0, hasPriceGuide = 0;
            // if we search for products based on their locName, we have to make a product query for each of them - store the result to reuse in case there are more of those cards later in the list
            Dictionary<string, string> locNameProducts = new Dictionary<string, string>();
            foreach (DataRow row in dt.Rows)
            {
                MKMMetaCard mc = new MKMMetaCard(row);
                importedAll.Add(mc); // add it no matter if it can be correctly processed or not so that we can export it
                counter++;
                string productID = mc.GetAttribute(MKMMetaCardAttribute.ProductID);
                string name = mc.GetAttribute(MKMMetaCardAttribute.Name);
                string languageID = mc.GetAttribute(MKMMetaCardAttribute.LanguageID);
                if (languageID == "" && defaultLanguageID != "")
                {
                    languageID = defaultLanguageID;
                    mc.SetLanguageID(languageID);
                }
                if (name == "" && productID == "") // we have neither name or productID - we have to hope we have locName and language
                {
                    string locName = mc.GetAttribute(MKMMetaCardAttribute.LocName);
                    if (locName == "" || languageID == "")
                    {
                        LogError("importing line #" + (counter + 1) + ", article will be ignored",
                            "Neither product ID, English name, or localized name + language was found, cannot identify the card.", false);
                        failed++;
                        continue;
                    }
                    // have to use search on MKM to get the English name
                    string hash = "" + locName + languageID; // technically it is unlikely that two different languages would have the same name, but could happen
                    if (!locNameProducts.TryGetValue(hash, out name)) // we haven't had a product like this in the list yet, use MKM API to find it
                    {
                        int start = 0;
                        List<XmlNode> found = new List<XmlNode>();
                        try
                        {
                            XmlNodeList products;
                            do
                            {
                                XmlDocument doc = MKMInteract.RequestHelper.findProducts(locName, languageID, start);
                                products = doc.GetElementsByTagName("product");
                                foreach (XmlNode product in products)
                                {
                                    if (found.Count > 0 && product["enName"].InnerText != found[found.Count - 1]["enName"].InnerText)
                                    {
                                        // we still want to insert empty string in the hash table - this way we know in the future that this name is invalid
                                        locNameProducts[hash] = "";
                                        // there are different cards as a result of the search -> we cannot identify the card, do not process it
                                        throw new Exception("Multiple different products match this localized name: " +
                                            product["enName"] + " and " + found[found.Count - 1]["enName"]);
                                    }
                                    found.Add(product);
                                }
                                start += products.Count;
                            } while (products.Count == 100);

                        }
                        catch (Exception eError)
                        {
                            LogError("importing line #" + (counter + 1) + ", trying to find product by its localized name "
                                + locName + ", article will be ignored", eError.Message, false);
                            failed++;
                            continue;
                        }
                        if (found.Count < 1)
                        {
                            LogError("importing line #" + (counter + 1) + ", trying to find product by its localized name "
                                + locName + ", article will be ignored", "No article called " + locName + " in "
                                + mc.GetAttribute(MKMMetaCardAttribute.Language) + " language found on MKM.", false);
                            failed++;
                            continue;
                        }
                        locNameProducts[hash] = name = found[0]["enName"].InnerText;
                        mc.SetAttribute(MKMMetaCardAttribute.Name, name);
                    }
                    else if (name != "")
                        mc.SetAttribute(MKMMetaCardAttribute.Name, name);
                    else
                    {
                        LogError("importing line #" + (counter + 1) + ", trying to find product by its localized name "
                            + locName + ", article will be ignored", "" + locName + " is not a valid name", false);
                        failed++;
                        continue;
                    }
                }
                // process foil and condition now as it can be useful in determining expansion
                string temp = mc.GetAttribute(MKMMetaCardAttribute.Foil);
                Bool3 isFoil;
                if (temp == "")
                {
                    mc.SetBoolAttribute(MKMMetaCardAttribute.Foil, defaultFoil);
                    isFoil = ParseBool3(mc.GetAttribute(MKMMetaCardAttribute.Foil));
                }
                else
                    isFoil = ParseBool3(temp);

                string condition = mc.GetAttribute(MKMMetaCardAttribute.Condition);
                if (condition == "")
                {
                    condition = defaultCondition;
                    mc.SetCondition(condition);
                }

                if (productID == "") // we now know we have the name, but we have to find out which expansion it is from to get the productID
                {
                    string expID = mc.GetAttribute(MKMMetaCardAttribute.ExpansionID); // if the Expansion would be set, ExpansionID would be set as well in constructor of MKMMetaCard
                    if (expID == "") // we have to determine the expansion
                    {
                        DataRow[] all = MKMDatabaseManager.Instance.GetCardByName(name);
                        // options are: Latest, Oldest, Cheapest, Median Price, Most Expensive
                        if (all.Length > 0)
                        {
                            // used for prices based on price guide (cheapest, median, most expensive):
                            // for non-foil, compare the LOWEX+ for EX+ items, LOW for worse conditions, for foil compare the LOWFOIL, for "any foil" compare SELL
                            string priceGuidePrice;
                            if (isFoil == Bool3.True)
                                priceGuidePrice = "LOWFOIL";
                            else if (isFoil == Bool3.Any)
                                priceGuidePrice = "SELL";
                            else
                            {
                                if (IsBetterOrSameCondition(condition, "EX"))
                                    priceGuidePrice = "LOWEX+";
                                else
                                    priceGuidePrice = "LOW";
                            }
                            switch (defaultExpansion)
                            {
                                // for latest and oldest, we can just check local database
                                case "Latest":
                                    DateTime latestTime = new DateTime(0);
                                    foreach (DataRow dr in all)
                                    {
                                        string tempExpID = dr[MKMDatabaseManager.InventoryFields.ExpansionID].ToString();
                                        string releaseDate = MKMDatabaseManager.Instance.GetExpansionByID(
                                            tempExpID)[MKMDatabaseManager.ExpansionsFields.ReleaseDate].ToString();
                                        DateTime rel = DateTime.Parse(releaseDate, CultureInfo.InvariantCulture);
                                        if (latestTime < rel)
                                        {
                                            latestTime = rel;
                                            expID = tempExpID;
                                        }
                                    }
                                    mc.SetAttribute(MKMMetaCardAttribute.ExpansionID, expID);
                                    mc.SetAttribute(MKMMetaCardAttribute.Expansion,
                                        MKMDatabaseManager.Instance.GetExpansionByID(expID)[MKMDatabaseManager.ExpansionsFields.Name].ToString());
                                    break;
                                case "Oldest":
                                    DateTime oldestTime = DateTime.Now;
                                    foreach (DataRow dr in all)
                                    {
                                        string tempExpID = dr[MKMDatabaseManager.InventoryFields.ExpansionID].ToString();
                                        string releaseDate = MKMDatabaseManager.Instance.GetExpansionByID(
                                            tempExpID)[MKMDatabaseManager.ExpansionsFields.ReleaseDate].ToString();
                                        DateTime rel = DateTime.Parse(releaseDate, CultureInfo.InvariantCulture);
                                        if (oldestTime > rel)
                                        {
                                            latestTime = rel;
                                            expID = tempExpID;
                                        }
                                    }
                                    mc.SetAttribute(MKMMetaCardAttribute.ExpansionID, expID);
                                    mc.SetAttribute(MKMMetaCardAttribute.Expansion,
                                        MKMDatabaseManager.Instance.GetExpansionByID(expID)[MKMDatabaseManager.ExpansionsFields.Name].ToString());
                                    break;
                                // for the others we have to do product queries for each possibility
                                case "Cheapest":
                                    XmlNode cheapestProduct = null; // we know all has at least one item so this will either get assigned or an exception is thrown and caught inside the foreach cycle
                                    double cheapestPrice = double.MaxValue;
                                    foreach (DataRow dr in all)
                                    {
                                        try
                                        {
                                            // there should always be exactly one product in the list
                                            XmlNode product = MKMInteract.RequestHelper.getProduct(
                                                dr[MKMDatabaseManager.InventoryFields.ProductID].ToString()).GetElementsByTagName("product")[0];
                                            double price = double.Parse(product["priceGuide"][priceGuidePrice].InnerText);
                                            if (price < cheapestPrice)
                                            {
                                                cheapestPrice = price;
                                                cheapestProduct = product;
                                            }
                                        }
                                        catch (Exception eError)
                                        {
                                            LogError("importing line #" + (counter + 1) + ", could not identify cheapest expansion for " + name + ", article will be ignored",
                                                eError.Message, false);
                                            failed++;
                                            continue;
                                        }
                                    }
                                    mc.FillProductInfo(cheapestProduct);
                                    hasPriceGuide++;
                                    break;
                                case "Median Price":
                                    SortedList<double, XmlNode> prices = new SortedList<double, XmlNode>();
                                    foreach (DataRow dr in all)
                                    {
                                        try
                                        {
                                            // there should always be exactly one product in the list
                                            XmlNode product = MKMInteract.RequestHelper.getProduct(
                                                dr[MKMDatabaseManager.InventoryFields.ProductID].ToString()).GetElementsByTagName("product")[0];
                                            double price = double.Parse(product["priceGuide"][priceGuidePrice].InnerText);
                                            prices.Add(price, product);
                                        }
                                        catch (Exception eError)
                                        {
                                            LogError("importing line #" + (counter + 1) + ", could not identify median price expansion for " + name + ", article will be ignored",
                                                eError.Message, false);
                                            failed++;
                                            continue;
                                        }
                                    }
                                    mc.FillProductInfo(prices.Values[prices.Count / 2]);
                                    hasPriceGuide++;
                                    break;
                                case "Most Expensive":
                                    XmlNode mostExpProduct = null; // we know all has at least one item so this will either get assigned or an exception is thrown and caught inside the foreach cycle
                                    double highestPrice = double.MinValue;
                                    foreach (DataRow dr in all)
                                    {
                                        try
                                        {
                                            // there should always be exactly one product in the list
                                            XmlNode product = MKMInteract.RequestHelper.getProduct(
                                                dr[MKMDatabaseManager.InventoryFields.ProductID].ToString()).GetElementsByTagName("product")[0];
                                            double price = double.Parse(product["priceGuide"][priceGuidePrice].InnerText);
                                            if (price > highestPrice)
                                            {
                                                highestPrice = price;
                                                mostExpProduct = product;
                                            }
                                        }
                                        catch (Exception eError)
                                        {
                                            LogError("importing line #" + (counter + 1) + ", could not identify cheapest expansion for " + name + ", article will be ignored",
                                                eError.Message, false);
                                            failed++;
                                            continue;
                                        }
                                    }
                                    mc.FillProductInfo(mostExpProduct);
                                    hasPriceGuide++;
                                    break;
                            }
                        }
                        else
                        {
                            LogError("importing line #" + (counter + 1) + ", identifying expansion for " + name + ", article will be ignored",
                                "No expansion found.", false);
                            failed++;
                            continue;
                        }
                        // TODO - determine whether the expansion is foil only / cannot be foil and based on the isFoil flag of the current article choose the correct set
                    }
                    // now we have expID and English name -> we can determine the product ID
                    productID = MKMDatabaseManager.Instance.GetProductID(name, mc.GetAttribute(MKMMetaCardAttribute.ExpansionID));
                    if (productID == "")
                    {
                        LogError("importing line #" + (counter + 1) + ", article will be ignored",
                            "The specified " + name + " and expansion ID " + expID + " do not match - product cannot be identified.", false);
                        failed++;
                        continue;
                    }
                    mc.SetAttribute(MKMMetaCardAttribute.ProductID, productID);
                }

                // if the defaults are "Any", there is not point in checking whether that attribute has been set or not
                if (defaultPlayset != "")
                {
                    temp = mc.GetAttribute(MKMMetaCardAttribute.Playset);
                    if (temp == "")
                        mc.SetBoolAttribute(MKMMetaCardAttribute.Playset, defaultPlayset);
                }
                if (defaultSigned != "")
                {
                    temp = mc.GetAttribute(MKMMetaCardAttribute.Signed);
                    if (temp == "")
                        mc.SetBoolAttribute(MKMMetaCardAttribute.Signed, defaultSigned);
                }
                if (defaultAltered != "")
                {
                    temp = mc.GetAttribute(MKMMetaCardAttribute.Altered);
                    if (temp == "")
                        mc.SetBoolAttribute(MKMMetaCardAttribute.Altered, defaultAltered);
                }

                importedValidOnly.Add(mc);
                if (checkBoxImportLog.Checked)
                    MainView.Instance.LogMainWindow("Imported line #" + (counter + 1) + ", " + name);
            }

            MainView.Instance.LogMainWindow("Card list " + filePath + " imported. Successfully imported " + importedValidOnly.Count + 
                " articles, failed to import: " + failed + ", articles that include MKM Price Guide: " + hasPriceGuide);
            if (hasPriceGuide > 0)
                priceGuidesGenerated = true;
        }

        private void buttonBotSettings_Click(object sender, EventArgs e)
        {
            botSettings.Show();
        }

        private async void buttonAppraise_Click(object sender, EventArgs e)
        {
            bool MKMToolPrice = checkBoxToolPrices.Checked;
            bool PriceGuide = checkBoxPriceGuide.Checked;
            if (MKMToolPrice || PriceGuide)
            {
                MKMBotSettings s;
                if (botSettings.GenerateBotSettings(out s))
                {
                    bot.setSettings(s);
                    buttonImport.Enabled = false;
                    buttonAppraise.Enabled = false;
                    buttonExport.Enabled = false;
                    buttonExportToMKM.Enabled = false;

                    buttonAppraise.Text = "Appraising...";
                    await Task.Run(() => appraise(MKMToolPrice, PriceGuide));

                    if (PriceGuide)
                    {
                        checkBoxExportPriceGuide.Checked = true;
                        checkBoxExportPriceGuide.Enabled = true;
                    }
                    if (MKMToolPrice)
                    {
                        checkBoxExportToolPrices.Checked = true;
                        checkBoxExportToolPrices.Enabled = true;
                    }

                    buttonImport.Enabled = true;
                    buttonAppraise.Enabled = true;
                    buttonExport.Enabled = true;
                    buttonExportToMKM.Enabled = true;
                    buttonAppraise.Text = "Appraise";
                }
                else
                    MainView.Instance.LogMainWindow("Appraisal abandoned, incorrect setting parameters.");
            }
            else
                MainView.Instance.LogMainWindow("No price selected for appraisal.");
        }

        /// <summary>
        /// Appraises the imported list. Designed to work in a separate thread.
        /// </summary>
        /// <param name="MKMToolPrice">if set to <c>true</c>, MKMTool price will be computed based on the loaded settings - load the settings
        /// into the bot BEFORE calling this method.</param>
        /// <param name="MKMPriceGuide">if set to <c>true</c>, MKM price guide prices will be computed for items for which they were not computed during import.</param>
        private void appraise(bool MKMToolPrice, bool MKMPriceGuide)
        {
            if (MKMPriceGuide)
            {
                MainView.Instance.LogMainWindow("Fetching price guides for the imported list...");
                // to limit the number of API calls, we can store results from each request and re-use it if there are multiple cards with the same product ID
                Dictionary<string, XmlNode> products = new Dictionary<string, XmlNode>(); // key == product ID
                foreach (MKMMetaCard mc in importedValidOnly)
                {
                    if (!mc.HasPriceGuides)
                    {
                        string productID = mc.GetAttribute(MKMMetaCardAttribute.ProductID);
                        XmlNode product;
                        if (!products.TryGetValue(productID, out product))
                        {
                            try
                            {
                                // there should always be exactly one product in the list
                                product = MKMInteract.RequestHelper.getProduct(
                                    mc.GetAttribute(MKMMetaCardAttribute.ProductID)).GetElementsByTagName("product")[0];
                                products[productID] = product;
                            }
                            catch (Exception eError)
                            {
                                LogError("fetching MKM price guide for " + mc.GetAttribute(MKMMetaCardAttribute.Name) + ", will not have price guides",
                                    eError.Message, false);
                                continue;
                            }
                        }
                        mc.FillProductInfo(product);
                    }
                }
                priceGuidesGenerated = true;
                priceGuidesExported = false;
                MainView.Instance.LogMainWindow("Price guides fetched.");
            }
            if (MKMToolPrice)
            {
                MainView.Instance.LogMainWindow("Generating MKMTool prices for the imported list...");
                bot.generatePrices(importedValidOnly);
                toolPriceGenerated = true;
                toolPriceExported = false;
                MainView.Instance.LogMainWindow("Prices generated.");
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                MainView.Instance.LogMainWindow("Exporting external list...");
                // start with the same columns as there were in import
                DataTable export = new DataTable();
                foreach (DataColumn dc in importedColumns)
                    export.Columns.Add(dc.ColumnName);
                // now add additional columns based on what checkboxes are checked
                if (!checkBoxExportAll.Checked)
                {
                    if (checkBoxExportPriceGuide.Checked)
                    {
                        export.Columns.Add(MKMMetaCardAttribute.PriceGuideAVG);
                        export.Columns.Add(MKMMetaCardAttribute.PriceGuideLOW);
                        export.Columns.Add(MKMMetaCardAttribute.PriceGuideLOWEX);
                        export.Columns.Add(MKMMetaCardAttribute.PriceGuideLOWFOIL);
                        export.Columns.Add(MKMMetaCardAttribute.PriceGuideSELL);
                        export.Columns.Add(MKMMetaCardAttribute.PriceGuideTREND);
                        export.Columns.Add(MKMMetaCardAttribute.PriceGuideTRENDFOIL);
                        priceGuidesExported = true;
                    }
                    if (checkBoxExportToolPrices.Checked)
                    {
                        export.Columns.Add(MKMMetaCardAttribute.MKMToolPrice);
                        export.Columns.Add(MKMMetaCardAttribute.PriceCheapestSimilar);
                        toolPriceExported = true;
                    }
                }
                foreach (MKMMetaCard mc in importedAll)
                {
                    if (checkBoxExportOnlyAppraised.Checked)
                    {
                        if (toolPriceGenerated)
                        {
                            bool priceNotSet = mc.GetAttribute(MKMMetaCardAttribute.MKMToolPrice) != "" || mc.GetAttribute(MKMMetaCardAttribute.PriceCheapestSimilar) != "";
                            if (priceNotSet && (!priceGuidesGenerated || !mc.HasPriceGuides))
                                continue;
                        }
                        else if (priceGuidesGenerated && !mc.HasPriceGuides)
                            continue;
                    }
                    mc.WriteItselfIntoTable(export, checkBoxExportAll.Checked, checkBoxExportFormatDeckbox.Checked ? MCFormat.Deckbox : MCFormat.MKM);
                }

                MKMDatabaseManager.WriteTableAsCSV(sf.FileName, export);

                if (toolPriceGenerated && checkBoxExportToolPrices.Checked)
                    toolPriceExported = true;
                if (priceGuidesGenerated && checkBoxExportPriceGuide.Checked)
                    priceGuidesExported = true;
                MainView.Instance.LogMainWindow("Exporting finished.");
            }
        }

        private void PriceExternalList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((priceGuidesGenerated && !priceGuidesExported) || (toolPriceGenerated && !toolPriceExported))
            {
                if (MessageBox.Show("There are unsaved changes to the imported list, are you sure you want to close without exporting? All changes will be lost.",
                    "There are unexported changes", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void checkBoxExportAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxExportAll.Checked)
            {
                checkBoxExportPriceGuide.Enabled = false;
                checkBoxExportToolPrices.Enabled = false;
            }
            else
            {
                checkBoxExportPriceGuide.Enabled = true;
                checkBoxExportToolPrices.Enabled = true;
            }
            checkBoxExportPriceGuide.Checked = priceGuidesGenerated;
            checkBoxExportToolPrices.Checked = toolPriceGenerated;
        }

        private async void buttonExportToMKM_Click(object sender, EventArgs e)
        {
            buttonExportToMKM.Text = "Uploading...";
            string chosenPrice = comboBoxExportUploadPrice.SelectedItem.ToString();
            bool logAll = checkBoxExportLogAll.Checked;
            buttonImport.Enabled = false;
            buttonAppraise.Enabled = false;
            buttonExport.Enabled = false;
            buttonExportToMKM.Enabled = false;

            await Task.Run(() => uploadListToMKM(chosenPrice, logAll));

            buttonImport.Enabled = true;
            buttonAppraise.Enabled = true;
            buttonExport.Enabled = true;
            buttonExportToMKM.Enabled = true;
            buttonAppraise.Text = "Upload to MKM";
        }

        private void uploadListToMKM(string chosenPrice, bool logAll)
        {
            string sRequestXML = "";
            int postCounter = 0;
            foreach (MKMMetaCard mc in importedValidOnly)
            {
                if (mc.GetAttribute(MKMMetaCardAttribute.LanguageID) == "")
                {
                    MainView.Instance.LogMainWindow("Unknown language of "
                        + mc.GetAttribute(MKMMetaCardAttribute.Name) + " from " + mc.GetAttribute(MKMMetaCardAttribute.Expansion)
                        + ", cannot upload to MKM.");
                    continue;
                }
                string price = "";
                string foil = mc.GetAttribute(MKMMetaCardAttribute.Foil);
                switch (chosenPrice)
                {
                    case "Price guide - Low":
                        if (foil == "true")
                            price = mc.GetAttribute(MKMMetaCardAttribute.PriceGuideLOWFOIL);
                        else
                        {
                            string condition = mc.GetAttribute(MKMMetaCardAttribute.Condition);
                            if (IsBetterOrSameCondition(condition, "EX"))
                                price = mc.GetAttribute(MKMMetaCardAttribute.PriceGuideLOWEX);
                            else
                                price = mc.GetAttribute(MKMMetaCardAttribute.PriceGuideLOW);
                        }
                        break;
                    case "Price guide - Average":
                        if (foil == "true") // AVG is only for non-foil, SELL includes all -> choose TRENDFOIL
                            price = mc.GetAttribute(MKMMetaCardAttribute.PriceGuideTRENDFOIL);
                        else
                            price = mc.GetAttribute(MKMMetaCardAttribute.PriceGuideAVG);
                        break;
                    case "Price guide - Trend":
                        if (foil == "true") 
                            price = mc.GetAttribute(MKMMetaCardAttribute.PriceGuideTRENDFOIL);
                        else
                            price = mc.GetAttribute(MKMMetaCardAttribute.PriceGuideTREND);
                        break;
                    case "MKMTool price":
                        price = mc.GetAttribute(MKMMetaCardAttribute.MKMToolPrice);
                        break;
                    case "Cheapest matching article":
                        price = mc.GetAttribute(MKMMetaCardAttribute.PriceCheapestSimilar);
                        break;
                }
                if (price != "")
                {

                    mc.SetAttribute(MKMMetaCardAttribute.MKMPrice, price);
                    if (mc.GetAttribute(MKMMetaCardAttribute.Count) == "")
                        mc.SetAttribute(MKMMetaCardAttribute.Count, "1");
                    if (logAll)
                        MainView.Instance.LogMainWindow("Uploading " + mc.GetAttribute(MKMMetaCardAttribute.Count) + "x " +
                            mc.GetAttribute(MKMMetaCardAttribute.Name) + " from " + mc.GetAttribute(MKMMetaCardAttribute.Expansion) +
                            " for " + price + "€ to MKM.");
                    sRequestXML += MKMInteract.RequestHelper.postStockArticleBody(mc);
                    postCounter++;
                    if (postCounter > 98)
                    {
                        MKMInteract.RequestHelper.SendStockUpdate(sRequestXML, "POST");
                        postCounter = 0;
                        sRequestXML = "";
                    }
                }
                else
                    MainView.Instance.LogMainWindow("Selected price not computed for item "
                        + mc.GetAttribute(MKMMetaCardAttribute.Name) + " from " + mc.GetAttribute(MKMMetaCardAttribute.Expansion)
                        + ", not uploaded to MKM.");
            }
            if (postCounter > 0)
                MKMInteract.RequestHelper.SendStockUpdate(sRequestXML, "POST");
        }
    }
}
