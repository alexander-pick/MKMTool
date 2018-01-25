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
using System.Collections.Generic;

namespace MKMTool
{
    public enum PriceSetMethod { ByAverage, ByPercentageOfLowestPrice, ByPercentageOfHighestPrice };
    public enum AcceptedCondition { OnlyMatching, // only items in matching condition will be considered similar
        SomeMatchesAbove, // accept better-condition items, but only if there is at least one more expensive matching-condition item
        Anything // items in any matching or better condition will be considered similar
    };

    /// <summary>
    /// Contains all customizable settings that are used by MKMBot
    /// All numbers expressed as percentage must be saved as a double with 0 = 0%, 1 = 100%
    /// </summary>
    public struct MKMBotSettings
    {
        /// Price Estimation Settings

        // each pair is a <max price of item; max change in % allowed for such item>
        public SortedList<double, double> priceMaxChangeLimits;
        // each pair is a <max price of item; max difference in % allowed for such the next item> to be considered as similar - used to cull outliers
        public SortedList<double, double> priceMaxDifferenceLimits;
        public double priceMinRarePrice;
        public int priceMinSimilarItems, priceMaxSimilarItems;
        public PriceSetMethod priceSetPriceBy;
        // if price computed based on average, priceFactor = 0.5 will set price as average of similar items, at 0
        //      it will be equal to the lowest price, at 1 to the highest price. Remaining values are linear interpolation between either 
        //      min price and average (0-0.5) or average and highest price (0.5-1)
        // if price computed as percentage of lowest or higest price, priceFactor is that percentage
        public double priceFactor;


        /// Card Condition Settings
        
        public AcceptedCondition condAcceptance;


        /// Log Settings

        public bool logUpdated, logLessThanMinimum, logSmallPriceChange, logHighPriceChange, logHighPriceVariance;

        /// Other Settings

        public bool testMode; // if set to true, price updates will be computed and logged, but not sent to MKM
    }

    internal class MKMBot
    {
        public delegate void logboxAppendCallback(string text, MainView frm1);

        private readonly DataTable dt = MKMHelpers.ReadSQLiteToDt("inventory");

        private MKMBotSettings settings;
        
        public MKMBot()
        {
            this.settings = GenerateDefaultSettings();
        }

        public MKMBot(MKMBotSettings settings)
        {
            this.settings = settings;
        }


        /// <summary>
        /// Generates the default settings for MKMBot.
        /// To be used when GUI is not available / relevant.
        /// </summary>
        /// <returns>Default settings for all parameters of MKMBot</returns>
        public static MKMBotSettings GenerateDefaultSettings()
        {
            MKMBotSettings s = new MKMBotSettings();

            s.priceMaxChangeLimits = new SortedList<double, double>(); // empty by default
            s.priceMaxDifferenceLimits = new SortedList<double, double>(); // empty by default

            s.priceMinRarePrice = 0.05;
            s.priceMinSimilarItems = 4; // require exactly 4 items
            s.priceMaxSimilarItems = 4;
            s.priceSetPriceBy = PriceSetMethod.ByAverage;
            s.priceFactor = 0.5;

            s.condAcceptance = AcceptedCondition.OnlyMatching;

            s.logUpdated = true;
            s.logLessThanMinimum = true;
            s.logSmallPriceChange = true;
            s.logHighPriceChange = true;
            s.logHighPriceVariance = true;

            s.testMode = false;

            return s;
        }

        public void setSettings(MKMBotSettings s)
        {
            this.settings = s;
        }

        private void logBoxAppend(string text, MainView frm1)
        {
            frm1.logBox.AppendText(text);
        }

        public DataTable buildProperWantsList(string sListId)
        {
            try
            {

                var doc = MKMInteract.RequestHelper.getWantsListByID(sListId);

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
            var doc = MKMInteract.RequestHelper.readStock();

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
                            
                            foreach (XmlNode offer in node2)
                            {
                                if (offer["seller"]["address"]["country"].InnerText == MKMHelpers.sMyOwnCountry
                                    && offer["condition"].InnerText == article["condition"].InnerText
                                    && offer["isPlayset"].InnerText == article["isPlayset"].InnerText
                                    && offer["seller"]["idUser"].InnerText != MKMHelpers.sMyId // skip items listed by myself
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

                                        var sOldPrice = article["price"].InnerText.Replace(".", ",");
                                        float dOldPrice = Convert.ToSingle(sOldPrice);
                                        var sNewPrice = dSetPrice.ToString("0.00").Replace(",", ".");

                                        if (dSetPrice > dOldPrice + MKMHelpers.fAbsoluteMinPrice || dSetPrice < dOldPrice - MKMHelpers.fAbsoluteMinPrice) // only update the price if it changed meaningfully
                                        {                                            
                                            frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                                sArticleID + ">>> " + article["product"]["enName"].InnerText + Environment.NewLine +
                                                "Current Price: " + sOldPrice + ", Calcualted Price:" + sNewPrice + Environment.NewLine, frm1);

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
                                        }
                                        else if (MKMHelpers.bLogNonUpdates)
                                        {
                                            frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                                sArticleID + ">>> " + article["product"]["enName"].InnerText + Environment.NewLine +
                                                "Current Price: " + sOldPrice + ", Calcualted Price:" + sNewPrice + " - small difference, price unchanged" + Environment.NewLine,
                                                frm1);
                                        }
                                        break;
                                    }
                                }
                            }
                            if (counter < 4 && MKMHelpers.bLogNonUpdates)
                            {
                                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                    sArticleID + ">>> " + article["product"]["enName"].InnerText + Environment.NewLine +
                                    "Current Price: " + article["price"].InnerText + ", unchanged, only " + counter + " similar items found" + Environment.NewLine,
                                    frm1);
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