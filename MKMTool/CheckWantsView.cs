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
    along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

    Diese Datei ist Teil von MKMTool.

    MKMTool ist Freie Software: Sie können es unter den Bedingungen
    der GNU General Public License, wie von der Free Software Foundation,
    Version 3 der Lizenz oder (nach Ihrer Wahl) jeder späteren
    veröffentlichten Version, weiterverbreiten und/oder modifizieren.

    Fubar wird in der Hoffnung, dass es nützlich sein wird, aber
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

namespace MKMTool
{
    public partial class CheckWantsView : Form
    {
        private readonly DataTable dt = MKMHelpers.ReadSQLiteToDt("inventory");

        private readonly DataTable eS = new DataTable();

        private readonly MainView frm1;

        public CheckWantsView(MainView frm)
        {
            frm1 = frm;

            InitializeComponent();

            initWantLists();
                        
            var doc = MKMInteract.RequestHelper.getExpansions("1"); // Only MTG at present

            var node = doc.GetElementsByTagName("expansion");

            eS.Columns.Add("idExpansion", typeof (string));
            eS.Columns.Add("abbreviation", typeof (string));
            eS.Columns.Add("enName", typeof (string));

            foreach (XmlNode nExpansion in node)
            {
                eS.Rows.Add(nExpansion["idExpansion"].InnerText, nExpansion["abbreviation"].InnerText,
                    nExpansion["enName"].InnerText);
            }

            foreach (XmlNode nExpansion in node)
            {
                var item = new MKMHelpers.ComboboxItem();

                item.Text = nExpansion["enName"].InnerText;
                item.Value = nExpansion["idExpansion"].InnerText;

                editionBox.Items.Add(item);
            }

            foreach (var Lang in MKMHelpers.dLanguages)
            {
                try
                {
                    var item = new MKMHelpers.ComboboxItem();

                    item.Text = Lang.Value;
                    item.Value = Lang.Key;

                    langCombo.Items.Add(item);

                    langCombo.SelectedIndex = 0;
                }
                catch (Exception eError)
                {
                }
            }

            editionBox.Sorted = true;
            editionBox.SelectedIndex = 0;

            conditionCombo.SelectedIndex = 4;
        }

        public void initWantLists()
        {
            try
            {
                var doc = MKMInteract.RequestHelper.getWantsLists();

                var node = doc.GetElementsByTagName("wantslist");

                if (node.Count > 0)
                {
                    foreach (XmlNode nWantlist in node)
                    {
                        try
                        {
                            var item = new MKMHelpers.ComboboxItem();

                            item.Text = nWantlist["name"].InnerText;
                            item.Value = nWantlist["idWantslist"].InnerText;

                            wantListsBox2.Items.Add(item);

                            wantListsBox2.SelectedIndex = 0;
                        }
                        catch (Exception eError)
                        {
                        }
                    }
                }
            }
            catch (Exception eError)
            {
                MessageBox.Show(eError.ToString());
            }
        }

        private void checkListButton_Click(object sender, EventArgs e)
        {
            var sListId = (wantListsBox2.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString();

            frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                "Starting to check your Wantslist ID:" + sListId + " ...\n");
            
            var doc = MKMInteract.RequestHelper.getWantsListByID(sListId);

            var node = doc.GetElementsByTagName("item");

            foreach (XmlNode article in node)
            {
                var sProductID = article["product"]["idProduct"].InnerText;

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "checking:" + sProductID + " ...\n");
                
                checkArticle(sProductID, article["language"]["idLanguage"].InnerText, article["minCondition"].InnerText,
                    article["isFoil"].InnerText, article["isSigned"].InnerText, article["isAltered"].InnerText,
                    article["isPlayset"].InnerText, "");

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Done.\n");
            }
        }

        private void wantListsBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //wantsListBoxReload();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MKMInteract.RequestHelper.emptyCart();

            frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Shoping Cart emptied.\n");
        }
        

        private void checkEditionButton_Click(object sender, EventArgs e)
        {
            checkEditionButton.Enabled = false;
            checkEditionButton.Text = "Checking cheap deals...";
            var isFoil = "";
            var isSigned = "";
            var isAltered = "";
            var isPlayset = "";
            var minCondition = conditionCombo.Text;

            if (foilBox.Checked)
                isFoil = "true";

            if (signedBox.Checked)
                isSigned = "true";

            if (alteredBox.Checked)
                isAltered = "true";

            if (playsetBox.Checked)
                isPlayset = "true";


            if (checkBoxUser.Checked)
            {
                if (domnesticCheck.Checked)
                    frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                        "WARNING - domestics only is checked, if the specified seller is from a foreign country, no deals will be found.\n");
                // Go through the stock of a specified user, checks for cheap deals and add them to the cart.
                int start = 0;
                double maxAllowedPrice = Convert.ToDouble(maxPrice.Text);
                while (true)
                { 
                    String sUrl = "https://api.cardmarket.com/ws/v2.0/users/" + textBoxUser.Text + "/articles?start=" + start + "&maxResults=1000";

                    try
                    {
                        // get the users stock, filtered by the selected parameters
                        var doc2 = MKMInteract.RequestHelper.makeRequest(sUrl, "GET");

                        var node2 = doc2.GetElementsByTagName("article");
                        String language = (langCombo.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString();
                        foreach (XmlNode article in node2)
                        {
                            if ( // do as much filtering here as possible to reduce the number of API calls
                                MKMHelpers.IsBetterOrSameCondition(article["condition"].InnerText, conditionCombo.Text) &&
                                (!foilBox.Checked || article["isFoil"].InnerText == "true") &&
                                (!playsetBox.Checked || article["isPlayset"].InnerText == "true") &&
                                (language == "" || article["language"]["idLanguage"].InnerText == language) &&
                                (!signedBox.Checked || article["isSigned"].InnerText == "true") &&
                                (!signedBox.Checked || article["isAltered"].InnerText == "true") &&
                                (maxAllowedPrice >= Convert.ToDouble(article["price"].InnerText, CultureInfo.InvariantCulture))
                                )
                            {

                                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                                    "Checking: " + article["idProduct"].InnerText + "\n");
                                checkArticle(article["idProduct"].InnerText,
                                    language, minCondition, isFoil, isSigned,
                                    isAltered, isPlayset, article["idArticle"].InnerText);

                                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Done.\n");
                            }
                        }
                        if (node2.Count != 1000) // there is no additional items to fetch
                            break;
                    }
                    catch (Exception eError)
                    {
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "ERR Msg : " + eError.Message + "\n");
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "ERR URL : " + sUrl + "\n");

                        using (var sw = File.AppendText(@".\\error_log.txt"))
                        {
                            sw.WriteLine("ERR Msg : " + eError.Message);
                            sw.WriteLine("ERR URL : " + sUrl);
                        }
                        break;
                    }
                    start += 1000;
                }
            }
            else
            {
                var sEdId = editionBox.SelectedItem.ToString();

                var sT = dt.Clone();

                var result = dt.Select(string.Format("[Expansion ID] = '{0}'", sEdId));

                foreach (var row in result)
                {
                    sT.ImportRow(row);
                }

                foreach (DataRow oRecord in sT.Rows)
                {
                    frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                        "Checking: " + oRecord["idProduct"] + "\n");
                    checkArticle(oRecord["idProduct"].ToString(),
                        (langCombo.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString(), minCondition, isFoil, isSigned,
                        isAltered, isPlayset, "");

                    frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Done.\n");
                }
            }

            checkEditionButton.Text = "Check now";
            checkEditionButton.Enabled = true;
        }

        // idArticle - if not empty, article will be added to shopping cart only if it is matching the specified idArticle. used for searching for cheap deals by user
        private void checkArticle(string idProduct, string idLanguage, string minCondition, string isFoil,
            string isSigned, string isAltered, string isPlayset, string matchingArticle)
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

            if (idLanguage == "")
            {
                idLanguage = "0";
            }
            else
            {
                sUrl += "&idLanguage=" + idLanguage;
            }


            try
            {
                var doc2 = MKMInteract.RequestHelper.makeRequest(sUrl, "GET");

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

                    if (offer["seller"]["address"]["country"].InnerText != MKMHelpers.sMyOwnCountry && domnesticCheck.Checked)
                        continue;
                    // save cheapest price found anywhere
                    aPrices[counter] = Convert.ToSingle(offer["price"].InnerText, CultureInfo.InvariantCulture);
                    if (noBestPrice)
                    {
                        bestPriceInternational = aPrices[counter];
                        noBestPrice = false;
                    }
                    

                    if (aPrices[0] + (float) Convert.ToDouble(shipAddition.Text) >
                        (float) Convert.ToDouble(maxPrice.Text))
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
                        double factor = (float)Convert.ToDouble(percentText.Text);

                        factor = factor / 100 + 1;

                        //double f1 = Math.Round(((aPrices[0] * factor) + (float)Convert.ToDouble(shipAddition.Text)), 2);

                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Price 1: " + aPrices[0] + " Price 2: " + aPrices[1] + "\n");
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Factor Price 1: " +
                            Math.Round(aPrices[0] * factor + (float)Convert.ToDouble(shipAddition.Text), 2)
                            + " Factor Price 2: " +
                            Math.Round(aPrices[1] * factor + (float)Convert.ToDouble(shipAddition.Text), 2) + "\n");

                        //X% under others
                        if (
                            (aPrices[0] * factor + (float)Convert.ToDouble(shipAddition.Text) < aPrices[1])
                            && (aPrices[0] * factor + (float)Convert.ToDouble(shipAddition.Text) < aPrices[2])
                            )
                        {
                            double fTrendprice = 100000; // fictive price 

                            if (checkTrend.Checked)
                            {
                                //check Trend Price
                                var doc3 =
                                    MKMInteract.RequestHelper.makeRequest(
                                        "https://api.cardmarket.com/ws/v2.0/products/" + idProduct, "GET");

                                fTrendprice =
                                    Convert.ToDouble(doc3.GetElementsByTagName("TREND")[0].InnerText.Replace(".", ","));

                                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                                    "Trend: " + fTrendprice + "\n");
                            }

                            //only relevant if we search domnestic
                            if (domnesticCheck.Checked)
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
                                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                                    "Found cheap offer " + bestPriceArticle + "\n");
                                try
                                {
                                    var sRequestXML = MKMInteract.RequestHelper.addCartBody(bestPriceArticle);

                                    sRequestXML = MKMInteract.RequestHelper.getRequestBody(sRequestXML);

                                    MKMInteract.RequestHelper.makeRequest("https://api.cardmarket.com/ws/v2.0/shoppingcart",
                                        "PUT", sRequestXML);
                                }
                                catch (Exception eError)
                                {
                                    //frm1.logBox.Invoke(new Form1.logboxAppendCallback(this.logBoxAppend), "ERR Msg : " + eError.Message + "\n", frm1);
                                    MessageBox.Show(eError.Message);
                                }
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception eError)
            {
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "ERR Msg : " + eError.Message + "\n");
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "ERR URL : " + sUrl + "\n");

                //MessageBox.Show(eError.ToString());

                using (var sw = File.AppendText(@".\\error_log.txt"))
                {
                    sw.WriteLine("ERR Msg : " + eError.Message);
                    sw.WriteLine("ERR URL : " + sUrl);
                }
            }
        }

        private void CheckWants_Load(object sender, EventArgs e)
        {
        }

        private void checkBoxUser_CheckedChanged(object sender, EventArgs e)
        {
            textBoxUser.Enabled = checkBoxUser.Checked;
            editionBox.Enabled = !checkBoxUser.Checked;
        }
    }
}