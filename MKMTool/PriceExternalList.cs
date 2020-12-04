/*
	This file is part of MKMTool

    MKMTool is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    MKMTool is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with MKMTool.  If not, see <http://www.gnu.org/licenses/>.

    Diese Datei ist Teil von MKMTool.

    MKMTool ist Freie Software: Sie können es unter den Bedingungen
    der GNU General Public License, wie von der Free Software Foundation,
    Version 3 der Lizenz oder (nach Ihrer Wahl) jeder späteren
    veröffentlichten Version, weiterverbreiten und/oder modifizieren.

    MKMTool wird in der Hoffnung, dass es nützlich sein wird, aber
    OHNE JEDE GEWÄHRLEISTUNG, bereitgestellt; sogar ohne die implizite
    Gewährleistung der MARKTFÄHIGKEIT oder EIGNUNG FÜR EINEN BESTIMMTEN ZWECK.
    Siehe die GNU General Public License für weitere Details.

    Sie sollten eine Kopie der GNU General Public License zusammen mit diesem
    Programm erhalten haben. Wenn nicht, siehe <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

        bool priceGuidesGenerated = false; // turns to true if at least for one article the price guide has been fetched from MKM
        bool toolPriceGenerated = false;// turns to true if at least for one article has the price computed by MKMTool

        public PriceExternalList()
        {
            InitializeComponent();
        }

        // initialization, needs to be done only on the first show
        private void PriceExternalList_Shown(object sender, EventArgs e)
        {
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

        /// <summary>
        /// Instead of closing the window when the user presses (X) or ALT+F4, just hide it.
        /// Basically the intended behaviour is for the window to act as kind of a singleton object within the scope of its owner.
        /// </summary>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void checkBoxToolPrices_CheckedChanged(object sender, EventArgs e)
        {
            buttonBotSettings.Enabled = checkBoxToolPrices.Checked;
            if (!buttonBotSettings.Enabled && !botSettings.IsDisposed)
                botSettings.Hide();
            checkBoxMyStock.Enabled = checkBoxToolPrices.Checked;
        }

        private async void buttonImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog import = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
            };
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

                buttonImport.Text = "Importing...";

                string defaultFoil = comboBoxFoil.SelectedItem.ToString();
                string defaultPlayset = comboBoxPlayset.SelectedItem.ToString();
                string defaultSigned = comboBoxSigned.SelectedItem.ToString();
                string defaultAltered = comboBoxAltered.SelectedItem.ToString();
                string defaultCondition = comboBoxCondition.SelectedItem.ToString();
                string defaultExpansion = comboBoxExpansion.SelectedItem.ToString();
                string defaultLanguageID = ((ComboboxItem)comboBoxLanguage.SelectedItem).Value.ToString();

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
                 dt = MKMCsvUtils.ConvertCSVtoDataTable(filePath);
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
                string productID = mc.GetAttribute(MCAttribute.ProductID);
                string name = mc.GetAttribute(MCAttribute.Name);
                string languageID = mc.GetAttribute(MCAttribute.LanguageID);
                if (languageID == "" && defaultLanguageID != "")
                {
                    languageID = defaultLanguageID;
                    mc.SetLanguageID(languageID);
                }
                if (name == "" && productID == "") // we have neither name or productID - we have to hope we have locName and language
                {
                    string locName = mc.GetAttribute(MCAttribute.LocName).ToLower();
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
                                // we still want to insert empty string in the hash table - this way we know in the future that this name is invalid
                                locNameProducts[hash] = "";
                                foreach (XmlNode product in products)
                                {
                                    // only take exact matches, otherwise we get all kinds of garbage like sleeves etc. that use the name of the card
                                    if (product["locName"].InnerText.ToLower() == locName)
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
                                + mc.GetAttribute(MCAttribute.Language) + " language found on MKM.", false);
                            failed++;
                            continue;
                        }
                        locNameProducts[hash] = name = found[0]["enName"].InnerText;
                        mc.SetAttribute(MCAttribute.Name, name);
                    }
                    else if (name != "")
                        mc.SetAttribute(MCAttribute.Name, name);
                    else
                    {
                        LogError("importing line #" + (counter + 1) + ", trying to find product by its localized name "
                            + locName + ", article will be ignored", "" + locName + " is not a valid name", false);
                        failed++;
                        continue;
                    }
                }
                // process foil and condition now as it can be useful in determining expansion
                string temp = mc.GetAttribute(MCAttribute.Foil);
                Bool3 isFoil;
                if (temp == "")
                {
                    mc.SetBoolAttribute(MCAttribute.Foil, defaultFoil);
                    isFoil = ParseBool3(mc.GetAttribute(MCAttribute.Foil));
                }
                else
                    isFoil = ParseBool3(temp);

                string condition = mc.GetAttribute(MCAttribute.Condition);
                if (condition == "")
                {
                    condition = defaultCondition;
                    mc.SetCondition(condition);
                }

                if (productID == "") // we now know we have the name, but we have to find out which expansion it is from to get the productID
                {
                    string expID = mc.GetAttribute(MCAttribute.ExpansionID); // if the Expansion would be set, ExpansionID would be set as well in constructor of MKMMetaCard
                    if (expID == "") // we have to determine the expansion
                    {
                        var all = MKMDbManager.Instance.GetCardByName(name);
                        // options are: Latest, Oldest, Cheapest, Median Price, Most Expensive                        
                        if (all.GetEnumerator().MoveNext())
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
                                        string tempExpID = dr[MKMDbManager.InventoryFields.ExpansionID].ToString();
                                        string releaseDate = MKMDbManager.Instance.GetExpansionByID(
                                            tempExpID)[MKMDbManager.ExpansionsFields.ReleaseDate].ToString();
                                        DateTime rel = DateTime.Parse(releaseDate, CultureInfo.InvariantCulture);
                                        if (latestTime < rel)
                                        {
                                            latestTime = rel;
                                            expID = tempExpID;
                                        }
                                    }
                                    mc.SetAttribute(MCAttribute.ExpansionID, expID);
                                    mc.SetAttribute(MCAttribute.Expansion,
                                        MKMDbManager.Instance.GetExpansionByID(expID)[MKMDbManager.ExpansionsFields.Name].ToString());
                                    break;
                                case "Oldest":
                                    DateTime oldestTime = DateTime.Now;
                                    foreach (DataRow dr in all)
                                    {
                                        string tempExpID = dr[MKMDbManager.InventoryFields.ExpansionID].ToString();
                                        string releaseDate = MKMDbManager.Instance.GetExpansionByID(
                                            tempExpID)[MKMDbManager.ExpansionsFields.ReleaseDate].ToString();
                                        DateTime rel = DateTime.Parse(releaseDate, CultureInfo.InvariantCulture);
                                        if (oldestTime > rel)
                                        {
                                            latestTime = rel;
                                            expID = tempExpID;
                                        }
                                    }
                                    mc.SetAttribute(MCAttribute.ExpansionID, expID);
                                    mc.SetAttribute(MCAttribute.Expansion,
                                        MKMDbManager.Instance.GetExpansionByID(expID)[MKMDbManager.ExpansionsFields.Name].ToString());
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
                                                dr[MKMDbManager.InventoryFields.ProductID].ToString()).GetElementsByTagName("product")[0];
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
                                                dr[MKMDbManager.InventoryFields.ProductID].ToString()).GetElementsByTagName("product")[0];
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
                                                dr[MKMDbManager.InventoryFields.ProductID].ToString()).GetElementsByTagName("product")[0];
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
                                "No card with this name found.", false);
                            failed++;
                            continue;
                        }
                        // TODO - determine whether the expansion is foil only / cannot be foil and based on the isFoil flag of the current article choose the correct set
                    }
                    // now we have expID and English name -> we can determine the product ID
                    string[] ids = MKMDbManager.Instance.GetCardProductID(name, mc.GetAttribute(MCAttribute.ExpansionID));
                    if (ids.Length == 0)
                    {
                        LogError("importing line #" + (counter + 1) + ", article will be ignored",
                            "The specified " + name + " and expansion ID " + expID + " do not match - product cannot be identified.", false);
                        failed++;
                        continue;
                    }
                    else if (ids.Length > 1)
                    {
                        string cardNumber = mc.GetAttribute(MCAttribute.CardNumber);
                        if (cardNumber == "")
                        {
                            LogError("importing line #" + (counter + 1) + ", article will be ignored", "The specified " + name +
                                " and expansion ID " + expID + " match multiple products - please provide Card Number to identify which one it is.", false);
                            failed++;
                            continue;
                        }
                        // search for the matching item
                        int start = 0;
                        try
                        {
                            XmlNodeList products;
                            do
                            {
                                XmlDocument doc = MKMInteract.RequestHelper.findProducts(name, "1", start);
                                products = doc.GetElementsByTagName("product");
                                string expansion = mc.GetAttribute(MCAttribute.Expansion);
                                foreach (XmlNode product in products)
                                {
                                    if (product["number"].InnerText == cardNumber && product["expansionName"].InnerText == expansion)
                                    {
                                        productID = product["idProduct"].InnerText;
                                        // since we already have it, why not fill the product info in the MetaCard
                                        mc.FillProductInfo(product);
                                        break;
                                    }
                                }
                                start += products.Count;
                            } while (products.Count == 100 && productID == "");

                        }
                        catch (Exception eError)
                        {
                            LogError("importing line #" + (counter + 1) + ", trying to find product ID for "
                                + name + " based on its card number and expansion, article will be ignored", eError.Message, false);
                            failed++;
                            continue;
                        }
                        if (productID == "")
                        {
                            LogError("importing line #" + (counter + 1) + ", article will be ignored", "The specified " + name +
                                " and expansion ID " + expID 
                                + " match multiple products, Card Number was used to find the correct article, but no match was found, verify the data is correct.", false);
                            failed++;
                            continue;
                        }
                    }
                    else
                        productID = ids[0];
                    mc.SetAttribute(MCAttribute.ProductID, productID);
                }

                // if the defaults are "Any", there is not point in checking whether that attribute has been set or not
                if (defaultPlayset != "")
                {
                    temp = mc.GetAttribute(MCAttribute.Playset);
                    if (temp == "")
                        mc.SetBoolAttribute(MCAttribute.Playset, defaultPlayset);
                }
                if (defaultSigned != "")
                {
                    temp = mc.GetAttribute(MCAttribute.Signed);
                    if (temp == "")
                        mc.SetBoolAttribute(MCAttribute.Signed, defaultSigned);
                }
                if (defaultAltered != "")
                {
                    temp = mc.GetAttribute(MCAttribute.Altered);
                    if (temp == "")
                        mc.SetBoolAttribute(MCAttribute.Altered, defaultAltered);
                }
                // rarity might not be present in some cases, check it and get it from database, or worst case from MKM
                var rarity = mc.GetAttribute(MCAttribute.Rarity);
                if (rarity == "")
                {
                    var dataRow = MKMDbManager.Instance.GetSingleCard(productID);
                    rarity = dataRow[MKMDbManager.InventoryFields.Rarity].ToString();
                    if (rarity == "")
                    {
                        try
                        {
                            var productDoc = MKMInteract.RequestHelper.getProduct(productID);
                            rarity = productDoc["response"]["product"]["rarity"].InnerText;
                            dataRow[MKMDbManager.InventoryFields.Rarity] = rarity;
                        }
                        catch (Exception eError)
                        {
                            LogError("getting rarity for product " + productID, eError.Message, false);
                        }
                    }
                    mc.SetAttribute(MCAttribute.Rarity, rarity);
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
                    bot.SetSettings(s);
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
                        string productID = mc.GetAttribute(MCAttribute.ProductID);
                        XmlNode product;
                        if (!products.TryGetValue(productID, out product))
                        {
                            try
                            {
                                // there should always be exactly one product in the list
                                product = MKMInteract.RequestHelper.getProduct(
                                    mc.GetAttribute(MCAttribute.ProductID)).GetElementsByTagName("product")[0];
                                products[productID] = product;
                            }
                            catch (Exception eError)
                            {
                                LogError("fetching MKM price guide for " + mc.GetAttribute(MCAttribute.Name) + ", will not have price guides",
                                    eError.Message, false);
                                continue;
                            }
                        }
                        mc.FillProductInfo(product);
                    }
                }
                priceGuidesGenerated = true;
                MainView.Instance.LogMainWindow("Price guides fetched.");
            }
            if (MKMToolPrice)
            {
                MainView.Instance.LogMainWindow("Generating MKMTool prices for the imported list...");
                bot.GeneratePrices(importedValidOnly, checkBoxMyStock.Enabled && checkBoxMyStock.Checked); // the checkbox can be checked, but not enabled if user does not want MKMTool prices
                toolPriceGenerated = true;
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
                        export.Columns.Add(MCAttribute.PriceGuideAVG);
                        export.Columns.Add(MCAttribute.PriceGuideLOW);
                        export.Columns.Add(MCAttribute.PriceGuideLOWEX);
                        export.Columns.Add(MCAttribute.PriceGuideLOWFOIL);
                        export.Columns.Add(MCAttribute.PriceGuideSELL);
                        export.Columns.Add(MCAttribute.PriceGuideTREND);
                        export.Columns.Add(MCAttribute.PriceGuideTRENDFOIL);
                    }
                    if (checkBoxExportToolPrices.Checked)
                    {
                        export.Columns.Add(MCAttribute.MKMToolPrice);
                        export.Columns.Add(MCAttribute.PriceCheapestSimilar);
                    }
                }
                foreach (MKMMetaCard mc in importedAll)
                {
                    if (checkBoxExportOnlyAppraised.Checked)
                    {
                        // price guides are not generated or the card does not have them or the user does not want them
                        bool priceGuidesSkip = (!checkBoxExportPriceGuide.Checked || !priceGuidesGenerated || !mc.HasPriceGuides);
                        if (toolPriceGenerated)
                        {
                            if ((mc.GetAttribute(MCAttribute.MKMToolPrice) == "" && mc.GetAttribute(MCAttribute.PriceCheapestSimilar) == "") // toolPrices not present for this card
                                && priceGuidesSkip)
                                continue;
                        }
                        else if (priceGuidesSkip)
                            continue;
                    }
                    mc.WriteItselfIntoTable(export, checkBoxExportAll.Checked, 
                        checkBoxExportFormatDeckbox.Checked ? MCFormat.Deckbox : MCFormat.MKM, false);
                }

                MKMCsvUtils.WriteTableAsCSV(sf.FileName, export);

                MainView.Instance.LogMainWindow("Exporting finished.");
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
            buttonExportToMKM.Text = "Upload to MKM";
        }

        private void uploadListToMKM(string chosenPrice, bool logAll)
        {
            string sRequestXML = "";
            int postCounter = 0;
            foreach (MKMMetaCard mc in importedValidOnly)
            {
                if (mc.GetAttribute(MCAttribute.LanguageID) == "")
                {
                    MainView.Instance.LogMainWindow("Unknown language of "
                        + mc.GetAttribute(MCAttribute.Name) + " from " + mc.GetAttribute(MCAttribute.Expansion)
                        + ", cannot upload to MKM.");
                    continue;
                }
                string price = "";
                string foil = mc.GetAttribute(MCAttribute.Foil);
                switch (chosenPrice)
                {
                    case "Price guide - Low":
                        if (foil == "true")
                            price = mc.GetAttribute(MCAttribute.PriceGuideLOWFOIL);
                        else
                        {
                            string condition = mc.GetAttribute(MCAttribute.Condition);
                            if (IsBetterOrSameCondition(condition, "EX"))
                                price = mc.GetAttribute(MCAttribute.PriceGuideLOWEX);
                            else
                                price = mc.GetAttribute(MCAttribute.PriceGuideLOW);
                        }
                        break;
                    case "Price guide - Average":
                        if (foil == "true") // AVG is only for non-foil, SELL includes all -> choose TRENDFOIL
                            price = mc.GetAttribute(MCAttribute.PriceGuideTRENDFOIL);
                        else
                            price = mc.GetAttribute(MCAttribute.PriceGuideAVG);
                        break;
                    case "Price guide - Trend":
                        if (foil == "true") 
                            price = mc.GetAttribute(MCAttribute.PriceGuideTRENDFOIL);
                        else
                            price = mc.GetAttribute(MCAttribute.PriceGuideTREND);
                        break;
                    case "MKMTool price":
                        price = mc.GetAttribute(MCAttribute.MKMToolPrice);
                        break;
                    case "Cheapest matching article":
                        price = mc.GetAttribute(MCAttribute.PriceCheapestSimilar);
                        break;
                }
                if (price != "")
                {

                    mc.SetAttribute(MCAttribute.MKMPrice, price);
                    if (mc.GetAttribute(MCAttribute.Count) == "")
                        mc.SetAttribute(MCAttribute.Count, "1");
                    if (logAll)
                        MainView.Instance.LogMainWindow("Uploading " + mc.GetAttribute(MCAttribute.Count) + "x " +
                            mc.GetAttribute(MCAttribute.Name) + " from " + mc.GetAttribute(MCAttribute.Expansion) +
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
                        + mc.GetAttribute(MCAttribute.Name) + " from " + mc.GetAttribute(MCAttribute.Expansion)
                        + ", not uploaded to MKM.");
            }
            if (postCounter > 0)
                MKMInteract.RequestHelper.SendStockUpdate(sRequestXML, "POST");
        }
    }
}
