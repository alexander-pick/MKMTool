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
#undef DEBUG

using System;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace MKMTool
{
    internal class MKMBot
    {
        public delegate void logboxAppendCallback(string text, MainView frm1);

        private readonly DataTable dt = MKMHelpers.ReadSQLiteToDt("inventory");

        private void logBoxAppend(string text, MainView frm1)
        {
            frm1.logBox.AppendText(text);
        }

        public XmlDocument getExpansionsSingles(string ExpansionID)
        {
            var doc =
                MKMInteract.RequestHelper.makeRequest(
                    "https://www.mkmapi.eu/ws/v2.0/expansions/" + ExpansionID + "/singles", "GET");

            return doc;
        }

        public XmlDocument getExpansions(string sGameID)
        {
            var doc =
                MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/games/" + sGameID + "/expansions",
                    "GET");

            return doc;
        }

        public XmlDocument getAccount()
        {
            var doc = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/account", "GET");

            return doc;
        }

        public XmlDocument getWantsLists()
        {
            var doc = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/wantslist", "GET");

            return doc;
        }

        public XmlDocument getWantsListByID(string sID)
        {
            var doc = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/wantslist/" + sID, "GET");

            return doc;
        }

        public XmlDocument readStock()
        {
            var doc = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/stock", "GET");

            return doc;
        }

        public XmlDocument emptyCart()
        {
            var doc = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/shoppingcart", "DELETE");

            return doc;
        }

        public void getProductList(MainView frm1)
        {
            MKMHelpers.GetProductList();
        }

        public DataTable buildProperWantsList(string sListId)
        {
            try
            {
                var bot = new MKMBot();

                var doc = bot.getWantsListByID(sListId);

                var xmlReader = new XmlNodeReader(doc);

                var ds = new DataSet();

                ds.ReadXml(xmlReader);

                if (!ds.Tables.Contains("item"))
                {
                    return new DataTable();
                }

                DataTable eS = MKMHelpers.ReadSQLiteToDt("expansions");
                
                var dv = MKMHelpers.JoinDataTables(dt, eS,
                    (row1, row2) => row1.Field<string>("Expansion ID") == row2.Field<string>("idExpansion"));

                dv = MKMHelpers.JoinDataTables(dv, ds.Tables["item"],
                    (row1, row2) => row1.Field<string>("idProduct") == row2.Field<string>("idProduct"));

                return dv;
            }
            catch (Exception eError)
            {
                MessageBox.Show(eError.ToString());
                return new DataTable();
            }
        }

        public void updatePrices(MainView frm1)
        {

            // should fix weird float errors on foregin systems.
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");

            //frm1.logBox.Invoke(new logboxAppendCallback(this.logBoxAppend), Application.CurrentCulture.EnglishName + "\n", frm1);

            var debugCounter = 0;

            var iRequestCount = 0;
            var sRequestXML = "";

            var doc = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/stock", "GET");

            //logBox.AppendText(OutputFormat.PrettyXml(doc.OuterXml));

            var node = doc.GetElementsByTagName("article");

            foreach (XmlNode article in node)
            {
                debugCounter++;

#if (DEBUG)
                if (debugCounter > 3)
                {
                    frm1.logBox.AppendText("DEBUG MODE - EXITING AFTER 3\n");
                    break;
                }
#endif

                if (article["idArticle"].InnerText != null)
                {
                    if (article["price"].InnerText != null)
                    {

                        var sUrl = "http://not.initilaized";

                        try
                        {

                            var sArticleID = article["idProduct"].InnerText;

                            /*XmlDocument doc2 = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/products/" + sArticleID, "GET");

                            logBox.AppendText(OutputFormat.PrettyXml(doc2.OuterXml));*/

                            //TODO: Crashs/Catchs on non single cards product, should add some detection for non card products later

                            sUrl = "https://www.mkmapi.eu/ws/v2.0/articles/" + sArticleID +
                                       "?idLanguage=" + article["language"]["idLanguage"].InnerText +
                                       "&minCondition=" + article["condition"].InnerText + "&start=0&maxResults=150&isFoil="
                                       + article["isFoil"].InnerText +
                                       "&isSigned=" + article["isSigned"].InnerText +
                                       "&isAltered=" + article["isAltered"].InnerText;

                            //string sUrl = "https://www.mkmapi.eu/ws/v2.0/articles/" + sArticleID;
                            //string sUrl = "https://www.mkmapi.eu/ws/v2.0/articles/" + sArticleID + "?start=0&maxResults=250";


                            var doc2 = MKMInteract.RequestHelper.makeRequest(sUrl, "GET");

                            var node2 = doc2.GetElementsByTagName("article");

                            var counter = 0;

                            var aPrices = new float[4];

                            frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                sArticleID + ">>> " + article["product"]["enName"].InnerText + "\n", frm1);

                            foreach (XmlNode offer in node2)
                            {
                                if (offer["seller"]["address"]["country"].InnerText == MKMHelpers.sMyOwnCountry
                                    &&
                                    offer["language"]["idLanguage"].InnerText ==
                                    article["language"]["idLanguage"].InnerText
                                    && offer["isFoil"].InnerText == article["isFoil"].InnerText
                                    && offer["isSigned"].InnerText == article["isSigned"].InnerText
                                    && offer["isAltered"].InnerText == article["isAltered"].InnerText
                                    && offer["condition"].InnerText == article["condition"].InnerText
                                    && offer["isPlayset"].InnerText == article["isPlayset"].InnerText
                                    )
                                {
                                    //frm1.logBox.Invoke(new logboxAppendCallback(this.logBoxAppend), article["product"]["enName"].InnerText + "\n", frm1);
                                    //frm1.logBox.Invoke(new logboxAppendCallback(this.logBoxAppend), article["price"].InnerText + " " + offer["price"].InnerText + "\n", frm1);

                                    var sXPrice = offer["price"].InnerText.Replace(".", ",");

                                    aPrices[counter] = Convert.ToSingle(sXPrice);

                                    counter++;

                                    if (counter == 4)
                                    {
                                        var dSetPrice = (aPrices[0] + aPrices[1] + aPrices[2] + aPrices[3]) / 4;

                                        if (dSetPrice < MKMHelpers.fAbsoluteMinPrice && article["product"]["rarity"].InnerText == "Rare")
                                        {
                                            dSetPrice = MKMHelpers.fAbsoluteMinPrice;
                                        }

                                        var sNewPrice = dSetPrice.ToString("0.00").Replace(",", ".");

                                        var sOldPrice = article["price"].InnerText;

                                        frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                            "Current Price: " + sOldPrice + " Calcualted Price:" + sNewPrice + "\n",
                                            frm1);

                                        try
                                        {
                                            // if (sNewPrice != sOldPrice)
                                            //{

                                            iRequestCount++;

                                            //frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend), "UPDATE\n", frm1);


                                            var sArticleRequest =
                                                MKMInteract.RequestHelper.changeStockArticleBody(article, sNewPrice);

                                            sRequestXML += sArticleRequest;

                                            iRequestCount++;
                                            //}
                                        }
                                        catch (Exception eError)
                                        {
                                            frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend), eError.ToString(),
                                                frm1);
                                        }


                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception eError)
                        {

#if (DEBUG)
                            frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                "ERR at  : " + article["product"]["enName"].InnerText + "\n", frm1);
                            frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                "ERR Msg : " + eError.Message + "\n", frm1);
                            frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend), "ERR URL : " + sUrl + "\n", frm1);
#endif
                            using (var sw = File.AppendText(@".\\error_log.txt"))
                            {
                                sw.WriteLine("ERR at  : " + article["product"]["enName"].InnerText);
                                sw.WriteLine("ERR Msg : " + eError.Message);
                                sw.WriteLine("ERR URL : " + sUrl);
                            }

                        }
                    }
                }
            }

            if (iRequestCount > 0)
            {
                sRequestXML = MKMInteract.RequestHelper.getRequestBody(sRequestXML);

                //logBox.AppendText("final Request:\n");
                //logBox.AppendText(OutputFormat.PrettyXml(sRequestXML));

                XmlDocument rdoc = null;

                try
                {
                    rdoc = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/stock", "PUT",
                        sRequestXML);
                }
                catch (Exception eError)
                {
                    frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend), "ERR Msg : " + eError.Message + "\n",
                        frm1);
                }

                var xUpdatedArticles = rdoc.GetElementsByTagName("updatedArticles");
                var xNotUpdatedArticles = rdoc.GetElementsByTagName("notUpdatedArticles");

                var iUpdated = xUpdatedArticles.Count;
                var iFailed = xNotUpdatedArticles.Count;

                if (iFailed == 1)
                {
                    iFailed = 0;
                }

                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                    debugCounter + "/" + iUpdated + " Articles updated successfully, " + iFailed + " failed\n", frm1);

                String timeStamp = GetTimestamp(DateTime.Now);

                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                    "Last Run finsihed: " + timeStamp + "\n", frm1);

                if (iFailed > 1)
                {
                    try
                    {
                        File.WriteAllText(@".\\log" + DateTime.Now.ToString("ddMMyyyy-HHmm") + ".log", rdoc.ToString());
                    }
                    catch (Exception eError)
                    {
                        frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend), "ERR Msg : " + eError.Message + "\n",
                            frm1);
                    }

                }
            }
        }

        private string GetTimestamp(DateTime now)
        {
            return now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public string getBuys(MainView mainView, string iType)
        {
            /*
                bought or 1
                paid or 2
                sent or 4
                received or 8
                lost or 32
                cancelled or 128
            */

            int count = 0;

            int iPage = 1;

            string sFilename = ".\\mcmbuys_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".csv";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(sFilename))
            {
                do
                {
                    var doc = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v1.1/output.xml/orders/2/" + iType + "/" + iPage, "GET");

                    count = doc.SelectNodes("response/order").Count;

                    iPage = iPage + 100;

                    //Console.WriteLine(count);

                    var oNodes = doc.GetElementsByTagName("order");

                    foreach (XmlNode order in oNodes)
                    {
                        var aNodes = doc.GetElementsByTagName("article");

                        string oID = order["idOrder"].InnerText;
                        string sOdate = order["state"]["dateReceived"].InnerText;

                        foreach (XmlNode article in aNodes)
                        {
                            try
                            {
                                file.WriteLine("\"" + oID + "\";\"" + sOdate + "\";\"" + article["product"]["name"].InnerText + "\";\"" + article["product"]["expansion"].InnerText + "\";\"" + article["language"]["languageName"].InnerText + "\";\"" + article["price"].InnerText + "\"");
                            }
                            catch (Exception eError)
                            {

                            }

                        }
                    }
                }
                while (count == 100);
            }
            


            return sFilename;
        }
    }
}