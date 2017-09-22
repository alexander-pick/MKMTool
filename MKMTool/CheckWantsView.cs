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

            var bot = new MKMBot();

            var doc = bot.getExpansions("1"); // Only MTG at present

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
                var bot = new MKMBot();

                var doc = bot.getWantsLists();

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

            var bot = new MKMBot();

            var doc = bot.getWantsListByID(sListId);

            var node = doc.GetElementsByTagName("item");

            foreach (XmlNode article in node)
            {
                var sArticleID = article["product"]["idProduct"].InnerText;

                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "checking:" + sArticleID + " ...\n");

                //private void checkArticle(string sArticleID, string idLanguage, string minCondition, string isFoil, string isSigned, string isAltered, string isPlayset)
                checkArticle(sArticleID, article["idLanguage"].InnerText, article["minCondition"].InnerText,
                    article["isFoil"].InnerText, article["isSigned"].InnerText, article["isAltered"].InnerText,
                    article["isPlayset"].InnerText);
            }
        }

        private void wantListsBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //wantsListBoxReload();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bot = new MKMBot();

            bot.emptyCart();

            frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Shoping Cart emptied.\n");
        }

        private void checkEditionButton_Click(object sender, EventArgs e)
        {
            var sEdId = (editionBox.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString();

            var sT = dt.Clone();

            var result = dt.Select(string.Format("[Expansion ID] = '{0}'", sEdId));

            foreach (var row in result)
            {
                sT.ImportRow(row);
            }

            var isFoil = "";
            var isSigned = "";
            var isAltered = "";
            var isPlayset = "";
            var minCondition = conditionCombo.Text;

            if (foilBox.CheckState.ToString() == "Checked")
                isFoil = "true";

            if (signedBox.CheckState.ToString() == "Checked")
                isSigned = "true";

            if (alteredBox.CheckState.ToString() == "Checked")
                isAltered = "true";

            if (playsetBox.CheckState.ToString() == "Checked")
                isPlayset = "true";

            foreach (DataRow oRecord in sT.Rows)
            {
                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                    "Checking: " + oRecord["idProduct"] + "\n");
                checkArticle(oRecord["idProduct"].ToString(),
                    (langCombo.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString(), minCondition, isFoil, isSigned,
                    isAltered, isPlayset);
            }
        }

        private void checkArticle(string sArticleID, string idLanguage, string minCondition, string isFoil,
            string isSigned, string isAltered, string isPlayset)
        {
            var sUrl = "https://www.mkmapi.eu/ws/v2.0/articles/" + sArticleID +
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
                sUrl += "&isPlayset=" + isAltered;
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

                    // save chepest price found anywhere
                    if (noBestPrice)
                    {
                        bestPriceInternational = Convert.ToSingle(offer["price"].InnerText.Replace(".", ","));
                        noBestPrice = false;
                    }

                    if (offer["seller"]["address"]["country"].InnerText != MKMHelpers.sMyOwnCountry)
                    {
                        if (domnesticCheck.Checked)
                        {
                            continue;
                        }
                    }

                    var sXPrice = offer["price"].InnerText.Replace(".", ",");
                    aPrices[counter] = Convert.ToSingle(sXPrice);

                    if (aPrices[0] + (float) Convert.ToDouble(shipAddition.Text) >
                        (float) Convert.ToDouble(maxPrice.Text))
                    {
                        //frm1.logBox.Invoke(new Form1.logboxAppendCallback(frm1.logBoxAppend), "Price higher than Max Price\n");
                        continue;
                    }

                    if (counter == 0)
                    {
                        bestPriceArticle = offer["idArticle"].InnerText;
                    }

                    counter++;

                    if (counter == 3)
                    {
                        double factor = (float) Convert.ToDouble(percentText.Text);

                        factor = factor/100 + 1;

                        //double f1 = Math.Round(((aPrices[0] * factor) + (float)Convert.ToDouble(shipAddition.Text)), 2);

                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Price 1: " + aPrices[0] + " Price 2: " + aPrices[1] + "\n");
                        frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                            "Factor Price 1: " +
                            Math.Round(aPrices[0]*factor + (float) Convert.ToDouble(shipAddition.Text), 2)
                            + " Factor Price 2: " +
                            Math.Round(aPrices[1]*factor + (float) Convert.ToDouble(shipAddition.Text), 2) + "\n");

                        //X% under others
                        if (
                            (aPrices[0]*factor + (float) Convert.ToDouble(shipAddition.Text) < aPrices[1])
                            && (aPrices[0]*factor + (float) Convert.ToDouble(shipAddition.Text) < aPrices[2])
                            )
                        {
                            double fTrendprice = 100000; // fictive price 

                            if (checkTrend.Checked)
                            {
                                //check Trend Price
                                var doc3 =
                                    MKMInteract.RequestHelper.makeRequest(
                                        "https://www.mkmapi.eu/ws/v2.0/products/" + sArticleID, "GET");

                                fTrendprice =
                                    Convert.ToDouble(doc3.GetElementsByTagName("TREND")[0].InnerText.Replace(".", ","));

                                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                                    "Trend: " + fTrendprice + "\n");
                            }

                            //only relevant if we search domnestic
                            if (domnesticCheck.Checked)
                            {
                                // is best price international (+/-5%)?
                                if (!(aPrices[0]*0.95 <= bestPriceInternational))
                                {
                                    break;
                                }
                            }

                            // X% under TREND
                            if (aPrices[0]*factor < fTrendprice)
                            {
                                frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend),
                                    "Found cheap offer " + bestPriceArticle + "\n");
                                try
                                {
                                    var sRequestXML = MKMInteract.RequestHelper.addCartBody(bestPriceArticle);

                                    sRequestXML = MKMInteract.RequestHelper.getRequestBody(sRequestXML);

                                    MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/shoppingcart",
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

            frm1.logBox.Invoke(new MainView.logboxAppendCallback(frm1.logBoxAppend), "Done.\n");
        }

        private void CheckWants_Load(object sender, EventArgs e)
        {
        }
    }
}