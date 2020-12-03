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
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;
using System.Threading.Tasks;

namespace MKMTool
{
    public partial class CheckWantsView : Form
    {
        public CheckWantsView()
        {
            InitializeComponent();
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

        // this is done only once, when it is first shown - populate language combobox, it never changes
        private void checkWantsView_Shown(object sender, EventArgs e)
        {
            foreach (var Lang in MKMHelpers.languagesNames)
            {
                var item = new MKMHelpers.ComboboxItem
                {
                    Text = Lang.Value,
                    Value = Lang.Key
                };

                langCombo.Items.Add(item);
            }

            langCombo.SelectedIndex = 0;
        }

        // this is done whenever it is shown/hidden - in case it is made visible, reload all data in case something has changed in the meantime
        private void checkWantsView_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                MKMDbManager.Instance.PopulateExpansionsComboBox(editionBox);
                if (editionBox.Items.Count > 0)
                {
                    editionBox.Sorted = true;
                    editionBox.SelectedIndex = 0;
                }
                else
                {
                    MKMHelpers.LogError("loading expansions from local database for Check Cheap Deals", "Database empty.", false);
                }

                conditionCombo.SelectedIndex = 4;
                InitWantLists();
            }
        }

        public void InitWantLists()
        {
            XmlDocument doc;
            try
            {
                doc = MKMInteract.RequestHelper.getWantsLists();
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("fetching all want lists, disabling Check Wantlist", eError.Message, true);
                checkListButton.Enabled = false;
                return;
            }

            var node = doc.GetElementsByTagName("wantslist");

            foreach (XmlNode nWantlist in node)
            {
                var item = new MKMHelpers.ComboboxItem
                {
                    Text = nWantlist["name"].InnerText,
                    Value = nWantlist["idWantslist"].InnerText
                };

                wantListsBox2.Items.Add(item);

                wantListsBox2.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// For actually performing the check for cheap deals from wantlist, expected to be run in its separate thread
        /// </summary>
        private void checkListRun(string listID, double maxAllowedPrice, double shippingAdd, double percentBelow, bool checkTrend)
        {
            XmlDocument doc;
            try
            {
                doc = MKMInteract.RequestHelper.getWantsListByID(listID);
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("checking wantlist ID " + listID, eError.Message, true);
                return;
            }

            MainView.Instance.LogMainWindow("Starting to check your wantlist ID:" + listID + " ...");
            var node = doc.GetElementsByTagName("item");
            foreach (XmlNode article in node)
            {
                // articles in a wantlist can be either product or metaproducts (the same card from multiple sets)
                // each metaproduct needs to be expanded into a list of products that are "realized" by it
                System.Collections.Generic.List<XmlNode> products = new System.Collections.Generic.List<XmlNode>();
                if (article["type"].InnerText == "product")
                    products.Add(article["product"]);
                else // it is a metaproduct
                {
                    try
                    {
                        XmlDocument metaDoc = MKMInteract.RequestHelper.getMetaproduct(article["metaproduct"]["idMetaproduct"].InnerText);
                        XmlNodeList mProducts = metaDoc.GetElementsByTagName("product");
                        foreach (XmlNode prod in mProducts)
                            products.Add(prod);
                    }
                    catch (Exception eError)
                    {
                        MKMHelpers.LogError("checking wantlist metaproduct ID " + article["metaproduct"]["idMetaproduct"], eError.Message, false);
                        continue;
                    }
                }
                foreach (XmlNode product in products)
                {
                    // As of 25.6.2019, the countArticles and countFoils fields are not described in MKM documentation, but they seem to be there.
                    // I think this is indeed a new thing that appeared since MKM started to force promo-sets as foils only
                    // We can use this to prune lot of useless calls that will end up in empty responses from searching for non foils in promo (foil only) sets
                    int total = int.Parse(product["countArticles"].InnerText);
                    int foils = int.Parse(product["countFoils"].InnerText);
                    string artFoil = article["isFoil"] == null ? "" : article["isFoil"].InnerText;
                    if ((artFoil == "true" && foils == 0) || // there are only non-foils of this article and we want foils
                        (artFoil == "false" && foils == total)) // there are only foils and we want non-foil
                        continue;
                    MainView.Instance.LogMainWindow("checking:" + product["enName"].InnerText + " from " + product["expansionName"].InnerText + "...");

                    // a wantlist item can have more idLanguage entries, one for each wanted language
                    System.Collections.Generic.List<string> selectedLanguages = new System.Collections.Generic.List<string>();
                    foreach (XmlNode langNodes in product.ChildNodes)
                    {
                        if (langNodes.Name == "idLanguage")
                            selectedLanguages.Add(langNodes.InnerText);
                    }
                    string artSigned = article["isSigned"] == null ? "" : article["isSigned"].InnerText;
                    string artAltered = article["isAltered"] == null ? "" : article["isAltered"].InnerText;
                    checkArticle(product["idProduct"].InnerText, selectedLanguages, article["minCondition"].InnerText,
                        artFoil, artSigned, artAltered,
                        // isPlayset seems to no longer be part of the API, instead there is a count of how many times is the card wanted, let's use it
                        int.Parse(article["count"].InnerText) == 4 ? "true" : "false",
                        "", maxAllowedPrice, shippingAdd, percentBelow, checkTrend);
                }
            }
            MainView.Instance.LogMainWindow("Check finished.");
        }

        private async void checkListButton_Click(object sender, EventArgs e)
        {
            string sListId = (wantListsBox2.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString();

            // the window controls can't be accessed from a different thread -> have to parse them here and send as arguments
            double maxAllowedPrice = Convert.ToDouble(maxPrice.Text);
            double shippingAdd = Convert.ToDouble(shipAddition.Text);
            double percentBelow = Convert.ToDouble(percentText.Text);
            bool checkTrend = checkBoxTrend.Checked;

            groupBoxParams.Enabled = false;
            groupBoxPerform.Enabled = false;
            checkListButton.Text = "Checking wantlist...";
            await Task.Run(() => checkListRun(sListId, maxAllowedPrice, shippingAdd, percentBelow, checkTrend));
            checkListButton.Text = "Check selected list";
            groupBoxParams.Enabled = true;
            groupBoxPerform.Enabled = true;
        }

        private void wantListsBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //wantsListBoxReload();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MKMInteract.RequestHelper.emptyCart();

                MainView.Instance.LogMainWindow( "Shopping Cart emptied.");
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("emptying shopping cart, cart not emptied", eError.Message, true);
            }
        }

        /// <summary>
        /// For actually performing the check for cheap deals from a given user, expected to be run in its separate thread.
        /// </summary>
        /// <param name="selectedExpansionID">Leave as empty string if all expansion should be checked.</param>
        private void checkUserRun(string user, string isFoil, string isSigned, string isAltered, string isPlayset, string minCondition,
            bool domesticOnly, double maxPrice, double shippingAddition, double percentBelowOthers, bool checkTrend,
            System.Collections.Generic.List<string> selectedLanguage, string selectedExpansionID = "")
        {
            MainView.Instance.LogMainWindow("Check for cheap deals from seller '" + user + "'...");
            if (domesticOnly)
                MainView.Instance.LogMainWindow("WARNING - domestics only is checked, if the specified seller is from a foreign country, no deals will be found.");
            // Go through the stock of a specified user, checks for cheap deals and add them to the cart.
            int start = 0;
            while (true)
            {
                string sUrl = "https://api.cardmarket.com/ws/v2.0/users/" + user + "/articles?start=" + start + "&maxResults=1000";
                XmlDocument doc2;
                try
                {
                    // get the users stock, filtered by the selected parameters
                    doc2 = MKMInteract.RequestHelper.makeRequest(sUrl, "GET");
                }
                catch (Exception eError)
                {
                    MKMHelpers.LogError("looking for cheap deals from user " + user, eError.Message, false, sUrl);
                    break;
                }

                var node2 = doc2.GetElementsByTagName("article");
                foreach (XmlNode article in node2)
                {
                    if (selectedExpansionID != "") // if we want only cards from a specified set, check if this product is from that set using local database
                    {
                        DataRow card = MKMDbManager.Instance.GetSingleCard(
                            article[MKMDbManager.InventoryFields.ProductID].InnerText);

                        if (card == null || card.Field<string>(MKMDbManager.InventoryFields.ExpansionID) != selectedExpansionID)
                            continue;
                    }

                    if ( // do as much filtering here as possible to reduce the number of API calls
                        MKMHelpers.IsBetterOrSameCondition(article["condition"].InnerText, minCondition) &&
                        (!foilBox.Checked || article["isFoil"].InnerText == "true") &&
                        (!playsetBox.Checked || article["isPlayset"].InnerText == "true") &&
                        (selectedLanguage[0] == "" || article["language"]["idLanguage"].InnerText == selectedLanguage[0]) &&
                        (!signedBox.Checked || article["isSigned"].InnerText == "true") &&
                        (!signedBox.Checked || article["isAltered"].InnerText == "true") &&
                        (maxPrice >= Convert.ToDouble(article["price"].InnerText, CultureInfo.InvariantCulture))
                        )
                    {

                        MainView.Instance.LogMainWindow("Checking product ID: " + article["idProduct"].InnerText);
                        checkArticle(article["idProduct"].InnerText,
                            selectedLanguage, minCondition, isFoil, isSigned,
                            isAltered, isPlayset, article["idArticle"].InnerText, maxPrice, shippingAddition, percentBelowOthers, checkTrend);
                    }
                }
                if (node2.Count != 1000) // there is no additional items to fetch
                    break;
                start += 1000;
            }
            MainView.Instance.LogMainWindow("Check finished.");
        }

        /// <summary>
        /// For actually performing the check for cheap deals, expected to be run in its separate thread
        /// </summary>
        private void checkExpansionRun(string isFoil, string isSigned, string isAltered, string isPlayset, string minCondition,
            double maxPrice, double shippingAddition, double percentBelowOthers, bool checkTrend, string selectedExpansionID,
            System.Collections.Generic.List<string> selectedLanguage)
        {
            var sT = MKMDbManager.Instance.GetCardsInExpansion(selectedExpansionID);
            MainView.Instance.LogMainWindow("Checking for cheap deals from selected expansion...");
            foreach (DataRow oRecord in sT)
            {
                MainView.Instance.LogMainWindow("Checking: " + oRecord[MKMDbManager.InventoryFields.Name]);
                checkArticle(oRecord[MKMDbManager.InventoryFields.ProductID].ToString(), selectedLanguage, minCondition, isFoil, isSigned,
                    isAltered, isPlayset, "", maxPrice, shippingAddition, percentBelowOthers, checkTrend);
            }
            MainView.Instance.LogMainWindow("Check finished.");
        }

        private async void checkEditionButton_Click(object sender, EventArgs e)
        {
            groupBoxParams.Enabled = false;
            groupBoxPerform.Enabled = false;
            checkEditionButton.Text = "Checking cheap deals...";
            // the window controls can't be accessed from a different thread -> have to parse them here and send as arguments
            string isFoil = "";
            string isSigned = "";
            string isAltered = "";
            string isPlayset = "";
            string minCondition = conditionCombo.Text;
            double maxAllowedPrice = Convert.ToDouble(maxPrice.Text);
            double shippingAdd = Convert.ToDouble(shipAddition.Text);
            double percentBelow = Convert.ToDouble(percentText.Text);
            bool checkTrend = checkBoxTrend.Checked;
            string selectedExpansionID = ((MKMHelpers.ComboboxItem)editionBox.SelectedItem).Value.ToString();

            if (foilBox.Checked)
                isFoil = "true";

            if (signedBox.Checked)
                isSigned = "true";

            if (alteredBox.Checked)
                isAltered = "true";

            if (playsetBox.Checked)
                isPlayset = "true";

            System.Collections.Generic.List<string> selectedLanguage = new System.Collections.Generic.List<string>
            {
                (langCombo.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString()
            };

            await Task.Run(() => checkExpansionRun(isFoil, isSigned, isAltered, isPlayset, minCondition, 
                maxAllowedPrice, shippingAdd, percentBelow, checkTrend, selectedExpansionID,
                selectedLanguage));

            checkEditionButton.Text = "Check now";
            groupBoxParams.Enabled = true;
            groupBoxPerform.Enabled = true;
        }

        private async void buttonCheckUser_Click(object sender, EventArgs e)
        {
            groupBoxParams.Enabled = false;
            groupBoxPerform.Enabled = false;
            buttonCheckUser.Text = "Checking cheap deals...";

            // the window controls can't be accessed from a different thread -> have to parse them here and send as arguments
            string isFoil = "";
            string isSigned = "";
            string isAltered = "";
            string isPlayset = "";
            string minCondition = conditionCombo.Text;
            double maxAllowedPrice = Convert.ToDouble(maxPrice.Text);
            double shippingAdd = Convert.ToDouble(shipAddition.Text);
            double percentBelow = Convert.ToDouble(percentText.Text);
            bool checkTrend = checkBoxTrend.Checked;
            string selectedExpansionID = ((MKMHelpers.ComboboxItem)editionBox.SelectedItem).Value.ToString();

            if (foilBox.Checked)
                isFoil = "true";

            if (signedBox.Checked)
                isSigned = "true";

            if (alteredBox.Checked)
                isAltered = "true";

            if (playsetBox.Checked)
                isPlayset = "true";

            System.Collections.Generic.List<string> selectedLanguage = new System.Collections.Generic.List<string>
            {
                (langCombo.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString()
            };

            await Task.Run(() => checkUserRun(textBoxUser.Text, isFoil, isSigned, isAltered, isPlayset, minCondition,
                domesticCheck.Checked, maxAllowedPrice, shippingAdd, percentBelow, checkTrend,
                selectedLanguage, checkBoxUserExpansions.Checked ? "" : selectedExpansionID));

            buttonCheckUser.Text = "Check user's stock";
            groupBoxParams.Enabled = true;
            groupBoxPerform.Enabled = true;
        }

        // idArticle - if not empty, article will be added to shopping cart only if it is matching the specified idArticle. used for searching for cheap deals by user
        // idLanguages - list of languages to check for. if all are OK, pass in either an empty list or a list with exactly one entry which is an empty string
        private void checkArticle(string idProduct, System.Collections.Generic.List<string> idLanguages, string minCondition, string isFoil,
            string isSigned, string isAltered, string isPlayset, string matchingArticle, 
            double maxAllowedPrice, double shippingAddition, double percentBelowOthers, bool checkTrend)
        {
            var sUrl = "https://api.cardmarket.com/ws/v2.0/articles/" + idProduct +
                       "?minCondition=" + minCondition +
                       "&start=0&maxResults=50";

            if (isFoil != "")
            {
                sUrl += "&isFoil=" + isFoil;
            }

            if (isSigned != "")
            {
                sUrl += "&isSigned=" + isSigned;
            }

            if (isAltered != "")
            {
                sUrl += "&isAltered=" + isAltered;
            }

            if (isPlayset != "")
            {
                sUrl += "&isPlayset=" + isPlayset;
            }

            if (idLanguages.Count == 1 && idLanguages[0] != "") // if there is exactly one language specified, fetch only articles in that one language, otherwise get all
            {
                sUrl += "&idLanguage=" + idLanguages[0];
            }

            XmlDocument doc2;
            try
            {
                doc2 = MKMInteract.RequestHelper.makeRequest(sUrl, "GET");
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("checking article id " + idProduct, eError.Message, false, sUrl);
                return;
            }

            var node2 = doc2.GetElementsByTagName("article");

            var counter = 0;

            var noBestPrice = true;

            var aPrices = new float[4];

            var bestPriceArticle = "";

            float bestPriceInternational = 0;

            foreach (XmlNode offer in node2)
            {
                /*
                    * Wantstates:
                    * Empty    = n/a
                    * true     = yes
                    * false    = no
                    */

                if (offer["seller"]["address"]["country"].InnerText != MainView.Instance.Config.MyCountryCode
                    && domesticCheck.Checked)
                    continue;

                bool languageOk = true;
                if (idLanguages.Count > 1) // only some languages were specified, filter
                {
                    languageOk = false;
                    foreach (string lang in idLanguages)
                    {
                        if (lang == offer["language"]["idLanguage"].InnerText)
                        {
                            languageOk = true;
                            break;
                        }
                    }
                }
                if (!languageOk)
                    continue;

                // save cheapest price found anywhere
                aPrices[counter] = Convert.ToSingle(offer["price"].InnerText, CultureInfo.InvariantCulture);
                if (noBestPrice)
                {
                    bestPriceInternational = aPrices[counter];
                    noBestPrice = false;
                }
                    

                if (aPrices[0] + shippingAddition > maxAllowedPrice)
                {
                    //frm1.logBox.Invoke(new Form1.logboxAppendCallback(frm1.logBoxAppend), "Price higher than Max Price\n");
                    continue;
                }

                if (counter == 0)
                {
                    bestPriceArticle = offer["idArticle"].InnerText;
                    // if looking for matching article, no point to continue if it is not the cheapest - perhaps could be modified to pick things that are among matching?
                    if (matchingArticle != "" && matchingArticle != bestPriceArticle)
                        break;
                }

                counter++;

                if (counter == 3)
                {                        
                    double factor = percentBelowOthers;

                    factor = factor / 100 + 1;

                    MainView.Instance.LogMainWindow("Price 1: " + aPrices[0] + " Price 2: " + aPrices[1]);
                    MainView.Instance.LogMainWindow(
                        "Factor Price 1: " + Math.Round(aPrices[0] * factor + shippingAddition, 2)
                        + " Factor Price 2: " + Math.Round(aPrices[1] * factor + shippingAddition, 2));

                    //X% under others
                    if (
                        (aPrices[0] * factor + shippingAddition < aPrices[1])
                        && (aPrices[0] * factor + shippingAddition < aPrices[2])
                        )
                    {
                        double fTrendprice = 100000; // fictive price 

                        if (checkTrend)
                        {
                            //check Trend Price
                            try
                            {
                                var doc3 = MKMInteract.RequestHelper.makeRequest(
                                        "https://api.cardmarket.com/ws/v2.0/products/" + idProduct, "GET");

                                fTrendprice =
                                    Convert.ToDouble(doc3.GetElementsByTagName("TREND")[0].InnerText.Replace(".", ","));

                                MainView.Instance.LogMainWindow("Trend: " + fTrendprice);
                            }
                            catch (Exception eError)
                            {
                                MKMHelpers.LogError("checking trend price for " + offer["product"]["locName"].InnerText, eError.Message, false);
                            }
                        }

                        //only relevant if we search domestic
                        if (domesticCheck.Checked)
                        {
                            // is best price international (+/-5%)?
                            if (!(aPrices[0] * 0.95 <= bestPriceInternational))
                            {
                                break;
                            }
                        }

                        // X% under TREND
                        if (aPrices[0] * factor < fTrendprice)
                        {
                            MainView.Instance.LogMainWindow("Found cheap offer, ID " + bestPriceArticle);
                            try
                            {
                                var sRequestXML = MKMInteract.RequestHelper.addCartBody(bestPriceArticle);

                                sRequestXML = MKMInteract.RequestHelper.getRequestBody(sRequestXML);

                                MKMInteract.RequestHelper.makeRequest("https://api.cardmarket.com/ws/v2.0/shoppingcart",
                                    "PUT", sRequestXML);
                            }
                            catch (Exception eError)
                            {
                                MKMHelpers.LogError("adding article " + offer["product"]["locName"].InnerText + " to cart", eError.Message, false);
                            }
                        }
                    }

                    break;
                }
            }
        }
    }
}