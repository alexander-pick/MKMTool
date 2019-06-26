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
using System.Globalization;
using System.IO;
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

    // result of traversing the similar items listed on MKM
    public enum TraverseResult { SequenceFound, // successfully found long enough sequence of similar items
        Culled, // not enough items were found and some  outliers were culled
        HighVariance, // not enough items were found because of high difference between few cheap items and many expensive ones
        NotEnoughSimilars // not enough items were found - even without any culling
    }

    /// <summary>
    /// Contains all customizable settings that are used by MKMBot.
    /// All numbers expressed as percentage must be saved as a double with 0 = 0%, 1 = 100%.
    /// Can be serialized into a XML, otherwise it is basically a struct -> all members are public
    /// </summary>
    public class MKMBotSettings
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
        // if price computed as percentage of lowest or highest price, priceFactor is that percentage
        public double priceFactor;
        public double priceFactorWorldwide; // the same, but for worldwide search

        public double priceMarkup2, priceMarkup3, priceMarkup4; // in percent, markup to use when we have 2, 3 or more copies of the given card
        public double priceMarkupCap; // in euro, max amount of money allowed to be added on top of the estimated price by the markup
        public bool priceIgnorePlaysets; // if set to true, articles with isPlayset=true will be treated as four single cards - both for our stock and other sellers

        /// Card Condition Settings
        
        public AcceptedCondition condAcceptance;


        /// Log Settings

        public bool logUpdated, logLessThanMinimum, logSmallPriceChange, logLargePriceChangeTooLow, logLargePriceChangeTooHigh, logHighPriceVariance;

        /// Other Settings

        public bool testMode; // if set to true, price updates will be computed and logged, but not sent to MKM
        public bool searchWorldwide; // if the minimum of items is not found in the sellers country, do a search ignoring country - used only when nothing is culled!
        public string description; // overall description of what is this setting expected to do, written in the GUI

        public MKMBotSettings()
        {
            priceMaxChangeLimits = new SortedList<double, double>();
            priceMaxDifferenceLimits = new SortedList<double, double>();
        }

        /// <summary>
        /// Copies all settings from the specified reference settings.
        /// </summary>
        /// <param name="refSettings">The reference settings.</param>
        public void Copy(MKMBotSettings refSettings)
        {
            priceMaxChangeLimits.Clear();
            foreach (var limit in refSettings.priceMaxChangeLimits)
                priceMaxChangeLimits.Add(limit.Key, limit.Value);

            priceMaxDifferenceLimits.Clear();
            foreach (var limit in refSettings.priceMaxDifferenceLimits)
                priceMaxDifferenceLimits.Add(limit.Key, limit.Value);

            priceMinRarePrice = refSettings.priceMinRarePrice;
            priceMinSimilarItems = refSettings.priceMinSimilarItems;
            priceMaxSimilarItems = refSettings.priceMaxSimilarItems;
            priceSetPriceBy = refSettings.priceSetPriceBy;
            priceFactor = refSettings.priceFactor;
            priceFactorWorldwide = refSettings.priceFactorWorldwide;
            priceMarkup2 = refSettings.priceMarkup2;
            priceMarkup3 = refSettings.priceMarkup3;
            priceMarkup4 = refSettings.priceMarkup4;
            priceMarkupCap = refSettings.priceMarkupCap;
            priceIgnorePlaysets = refSettings.priceIgnorePlaysets;

            condAcceptance = refSettings.condAcceptance;
            logUpdated = refSettings.logUpdated;
            logLessThanMinimum = refSettings.logLessThanMinimum;
            logSmallPriceChange = refSettings.logSmallPriceChange;
            logLargePriceChangeTooLow = refSettings.logLargePriceChangeTooLow;
            logLargePriceChangeTooHigh = refSettings.logLargePriceChangeTooHigh;
            logHighPriceVariance = refSettings.logHighPriceVariance;
            testMode = refSettings.testMode;
            description = refSettings.description;
            searchWorldwide = refSettings.searchWorldwide;
        }

        /// <summary>
        /// Fills this instance from data stored in XML.
        /// In case there is a failure, exception is thrown and the data previously contained in this instance will not be changed at all.
        /// </summary>
        /// <param name="s">XML document representing an instance of MKMBotSettings. <seealso cref="Serialize()" /></param>
        /// <exception cref="FormatException">When parsing numbers.</exception>
        /// <exception cref="ArgumentException">When parsing numbers.</exception>
        /// <exception cref="OverflowException">When parsing numbers.</exception>
        public void Parse(XmlDocument s)
        {
            MKMBotSettings temp = new MKMBotSettings();
            XmlNode root = s.GetElementsByTagName("MKMBotSettings").Item(0);

            double threshold, allowedChange;
            string[] limits;
            foreach (XmlNode child in root.ChildNodes)
            {
                switch (child.Name)
                {
                    case "priceMaxChangeLimits":
                        limits = child.InnerText.Split(';');
                        for (int i = 1; i < limits.Length; i += 2)
                        {
                            threshold = double.Parse(limits[i - 1], NumberStyles.Float, CultureInfo.InvariantCulture);
                            allowedChange = double.Parse(limits[i], NumberStyles.Float, CultureInfo.InvariantCulture);
                            temp.priceMaxChangeLimits.Add(threshold, allowedChange);
                        }
                        break;
                    case "priceMaxDifferenceLimits":
                        limits = child.InnerText.Split(';');
                        for (int i = 1; i < limits.Length; i += 2)
                        {
                            threshold = double.Parse(limits[i - 1], NumberStyles.Float, CultureInfo.InvariantCulture);
                            allowedChange = double.Parse(limits[i], NumberStyles.Float, CultureInfo.InvariantCulture);
                            temp.priceMaxDifferenceLimits.Add(threshold, allowedChange);
                        }
                        break;
                }
            }
            temp.priceFactorWorldwide = -1;
            foreach (XmlNode att in root.Attributes)
            {
                switch (att.Name)
                {
                    case "priceMinRarePrice":
                        temp.priceMinRarePrice = double.Parse(att.Value, CultureInfo.InvariantCulture);
                        break;
                    case "priceMinSimilarItems":
                        temp.priceMinSimilarItems = int.Parse(att.Value, CultureInfo.InvariantCulture);
                        break;
                    case "priceMaxSimilarItems":
                        temp.priceMaxSimilarItems = int.Parse(att.Value, CultureInfo.InvariantCulture);
                        break;
                    case "priceSetPriceBy":
                        temp.priceSetPriceBy = (PriceSetMethod)Enum.Parse(typeof(PriceSetMethod), att.Value);
                        break;
                    case "priceFactor":
                        temp.priceFactor = double.Parse(att.Value, CultureInfo.InvariantCulture);
                        break;
                    case "priceFactorWorldwide":
                        temp.priceFactorWorldwide = double.Parse(att.Value, CultureInfo.InvariantCulture);
                        break;
                    case "priceMarkup2":
                        temp.priceMarkup2 = double.Parse(att.Value, CultureInfo.InvariantCulture);
                        break;
                    case "priceMarkup3":
                        temp.priceMarkup3 = double.Parse(att.Value, CultureInfo.InvariantCulture);
                        break;
                    case "priceMarkup4":
                        temp.priceMarkup4 = double.Parse(att.Value, CultureInfo.InvariantCulture);
                        break;
                    case "priceMarkupCap":
                        temp.priceMarkupCap = double.Parse(att.Value, CultureInfo.InvariantCulture);
                        break;
                    case "priceIgnorePlaysets":
                        temp.priceIgnorePlaysets = bool.Parse(att.Value);
                        break;
                    case "condAcceptance":
                        temp.condAcceptance = (AcceptedCondition)Enum.Parse(typeof(AcceptedCondition), att.Value);
                        break;
                    case "logUpdated":
                        temp.logUpdated = bool.Parse(att.Value);
                        break;
                    case "logLessThanMinimum":
                        temp.logLessThanMinimum = bool.Parse(att.Value);
                        break;
                    case "logSmallPriceChange":
                        temp.logSmallPriceChange = bool.Parse(att.Value);
                        break;
                    case "logLargePriceChangeTooLow":
                        temp.logLargePriceChangeTooLow = bool.Parse(att.Value);
                        break;
                    case "logLargePriceChangeTooHigh":
                        temp.logLargePriceChangeTooHigh = bool.Parse(att.Value);
                        break;
                    case "logHighPriceVariance":
                        temp.logHighPriceVariance = bool.Parse(att.Value);
                        break;
                    case "testMode":
                        temp.testMode = bool.Parse(att.Value);
                        break;
                    case "searchWorldwide":
                        temp.searchWorldwide = bool.Parse(att.Value);
                        break;
                    case "description":
                        temp.description = att.Value;
                        break;
                }
            }
            if (temp.priceFactorWorldwide == -1)
                temp.priceFactorWorldwide = temp.priceFactor; // for backwards compatibility - in 0.6.1 and older version, priceFactorWorldwide was always the same as priceFactor
            this.Copy(temp); // nothing failed, let's keep the settings
        }

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns>A XML representation of this object.</returns>
        public XmlDocument Serialize()
        {
            XmlDocument s = new XmlDocument();
            s.CreateXmlDeclaration("1.0", System.Text.Encoding.UTF8.ToString(), "no");
            XmlElement root = s.CreateElement("MKMBotSettings");

            XmlElement child = s.CreateElement("priceMaxChangeLimits");
            child.InnerText = "";
            foreach (var limitPair in priceMaxChangeLimits)
                child.InnerText += "" + limitPair.Key + ";" + limitPair.Value.ToString("f2", CultureInfo.InvariantCulture) + ";";
            root.AppendChild(child);

            child = s.CreateElement("priceMaxDifferenceLimits");
            child.InnerText = "";
            foreach (var limitPair in priceMaxDifferenceLimits)
                child.InnerText += "" + limitPair.Key + ";" + limitPair.Value.ToString("f2", CultureInfo.InvariantCulture) + ";";
            root.AppendChild(child);

            root.SetAttribute("priceMinRarePrice", priceMinRarePrice.ToString("f2", CultureInfo.InvariantCulture));
            root.SetAttribute("priceMinSimilarItems", priceMinSimilarItems.ToString(CultureInfo.InvariantCulture));
            root.SetAttribute("priceMaxSimilarItems", priceMaxSimilarItems.ToString(CultureInfo.InvariantCulture));
            root.SetAttribute("priceSetPriceBy", priceSetPriceBy.ToString());
            root.SetAttribute("priceFactor", priceFactor.ToString("f2", CultureInfo.InvariantCulture));
            root.SetAttribute("priceFactorWorldwide", priceFactorWorldwide.ToString("f2", CultureInfo.InvariantCulture));
            root.SetAttribute("priceMarkup2", priceMarkup2.ToString(CultureInfo.InvariantCulture));
            root.SetAttribute("priceMarkup3", priceMarkup3.ToString(CultureInfo.InvariantCulture));
            root.SetAttribute("priceMarkup4", priceMarkup4.ToString(CultureInfo.InvariantCulture));
            root.SetAttribute("priceMarkupCap", priceMarkupCap.ToString(CultureInfo.InvariantCulture));
            root.SetAttribute("priceIgnorePlaysets", priceIgnorePlaysets.ToString());

            root.SetAttribute("condAcceptance", condAcceptance.ToString());

            root.SetAttribute("logUpdated", logUpdated.ToString());
            root.SetAttribute("logLessThanMinimum", logLessThanMinimum.ToString());
            root.SetAttribute("logSmallPriceChange", logSmallPriceChange.ToString());
            root.SetAttribute("logLargePriceChangeTooLow", logLargePriceChangeTooLow.ToString());
            root.SetAttribute("logLargePriceChangeTooHigh", logLargePriceChangeTooHigh.ToString());
            root.SetAttribute("logHighPriceVariance", logHighPriceVariance.ToString());

            root.SetAttribute("testMode", testMode.ToString());
            root.SetAttribute("searchWorldwide", searchWorldwide.ToString());
            root.SetAttribute("description", description);

            s.AppendChild(root);
            return s;
        }
    }

    internal class MKMBot
    {
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

            s.priceMaxChangeLimits.Clear(); // empty by default
            s.priceMaxDifferenceLimits.Clear(); // empty by default

            s.priceMinRarePrice = 0.05;
            s.priceMinSimilarItems = 4; // require exactly 4 items
            s.priceMaxSimilarItems = 4;
            s.priceSetPriceBy = PriceSetMethod.ByAverage;
            s.priceFactor = 0.5;
            s.priceFactorWorldwide = 0.5;
            s.priceMarkup2 = s.priceMarkup3 = s.priceMarkup4 = 0;
            s.priceMarkupCap = 0;
            s.priceIgnorePlaysets = false;

            s.condAcceptance = AcceptedCondition.OnlyMatching;

            s.logUpdated = true;
            s.logLessThanMinimum = true;
            s.logSmallPriceChange = true;
            s.logLargePriceChangeTooLow = true;
            s.logLargePriceChangeTooHigh = true;
            s.logHighPriceVariance = true;

            s.testMode = false;
            s.searchWorldwide = false;


            return s;
        }

        public void setSettings(MKMBotSettings s)
        {
            this.settings = s;
        }


        /// <summary>
        /// Gets the maximum allowed price difference between the specified price and its neighbour.
        /// </summary>
        /// <param name="refPrice">The reference price.</param>
        /// <returns>Maximal allowed difference according to the <c>settings.priceMaxDifferenceLimits</c>.</returns>
        private double getMaxPriceDifference(double refPrice)
        {
            // find the right limit
            foreach (var limit in settings.priceMaxDifferenceLimits)
            {
                if (limit.Key > refPrice)
                    return refPrice * limit.Value;
            }
            return double.MaxValue;
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
                MKMHelpers.LogError("processing wantlist " + sListId + ", it will be ignored", eError.Message, true);
                return new DataTable();
            }
        }

        public void updatePrices()
        {
            if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice && settings.priceMaxChangeLimits.Count == 0)
            {
                MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                    "Setting price according to lowest price is very risky - specify limits for maximal price change first!" + Environment.NewLine);
                return;
            }
            // should fix weird float errors on foreign systems.

            // TJ - this does not look like a good idea to me. MKM is sending data formated in a locale where '.' is used as decimal separator
            // it makes no sense to force switch to German locale here and then later start replacing all '.' by ','
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");

            //MainView.Instance().logBox.Invoke(new logboxAppendCallback(this.logBoxAppend), Application.CurrentCulture.EnglishName + "\n");
#if (DEBUG)
            var debugCounter = 0;
#endif
            List<XmlNode> articles = new List<XmlNode>();
            string sRequestXML = "";
            XmlNodeList result;
            var start = 1;
            // load file with lowest prices
            Dictionary<string, List<XmlNode>> myStock = new Dictionary<string, List<XmlNode>>();
            try
            {
                XmlDocument st = new XmlDocument();
                st.Load(@".//myStock.xml");
                MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                    "Found list of minimal prices..." + Environment.NewLine);
                foreach (XmlNode n in st["stock"].ChildNodes)
                {
                    string nameLower = n.Attributes["name"].InnerText.ToLower();
                    if (!myStock.ContainsKey(nameLower))
                        myStock.Add(nameLower, new List<XmlNode>());
                    myStock[nameLower].Add(n);
                }
            }
            catch (Exception)
            {
                myStock = null;
            }

            try
            {
                do
                {
                    var doc = MKMInteract.RequestHelper.readStock(start);

                    //logBox.AppendText(OutputFormat.PrettyXml(doc.OuterXml));
                    result = doc.GetElementsByTagName("article");
                    foreach (XmlNode article in result)
                    {
                        articles.Add(article);
                    }
                    start += result.Count;
                } while (result.Count == 100);
            }
            catch (Exception error)
            {
                MKMHelpers.LogError("reading own stock, cannot continue price update", error.Message, true);
                return;
            }

            MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                "Updating Prices..." + Environment.NewLine);
            int putCounter = 0; 
            foreach (XmlNode article in articles)
            {
#if (DEBUG)
                debugCounter++;
                if (debugCounter > 3)
                {
                    MainView.Instance().logBox.AppendText("DEBUG MODE - EXITING AFTER 3\n");
                    break;
                }
#endif
                // according to the API documentation, "The 'condition' key is only returned for single cards. "
                // -> check if condition exists to see if this is a single card or something else
                if (article["condition"] != null && article["idArticle"].InnerText != null && article["price"].InnerText != null)
                {
                    string update = checkArticle(article, ref myStock);
                    if (update != null)
                    {
                        sRequestXML += update;
                        // max 100 articles is allowed to be part of a PUT call - if we are there, call it
                        if (putCounter > 98 && !settings.testMode)
                        {
                            sendPriceUpdate(sRequestXML);
                            putCounter = 0;
                            sRequestXML = "";
                        }
                        else
                            putCounter++;
                    }
                }
            }

            if (settings.testMode)
            {
                MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                    "Done. Prices NOT SENT to MKM - running in test mode finished." + Environment.NewLine);
            }
            else if (sRequestXML.Length > 0)
            {
                sendPriceUpdate(sRequestXML);
            }
            else
            {
                MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                    "Done. No valid/meaningful price updates created." + Environment.NewLine);
            }

            String timeStamp = GetTimestamp(DateTime.Now);

            MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                "Last Run finished: " + timeStamp + Environment.NewLine);
        }

        /// <summary>
        /// Sends the price update to MKM.
        /// </summary>
        /// <param name="sRequestXML">The raw (= not as an API request yet) XML with all article updates. Check that it is not empty before calling this.</param>
        private void sendPriceUpdate(string sRequestXML)
        {
            sRequestXML = MKMInteract.RequestHelper.getRequestBody(sRequestXML);

            //logBox.AppendText("final Request:\n");
            //logBox.AppendText(OutputFormat.PrettyXml(sRequestXML));

            XmlDocument rdoc = null;

            try
            {
                rdoc = MKMInteract.RequestHelper.makeRequest("https://api.cardmarket.com/ws/v2.0/stock", "PUT",
                    sRequestXML);
                var xUpdatedArticles = rdoc.GetElementsByTagName("updatedArticles");
                var xNotUpdatedArticles = rdoc.GetElementsByTagName("notUpdatedArticles");

                var iUpdated = xUpdatedArticles.Count;
                var iFailed = xNotUpdatedArticles.Count;

                if (iFailed == 1)
                {
                    iFailed = 0;
                }

                MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                    iUpdated + " articles updated successfully, " + iFailed + " failed" + Environment.NewLine);

                if (iFailed > 1)
                {
                    try
                    {
                        File.WriteAllText(@".\\log" + DateTime.Now.ToString("ddMMyyyy-HHmm") + ".log", rdoc.ToString());
                    }
                    catch (Exception eError)
                    {
                        MKMHelpers.LogError("logging failed price update articles", eError.Message, false);
                    }
                    MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                        "Failed articles logged in " + @".\\log" + DateTime.Now.ToString("ddMMyyyy-HHmm") + ".log" + Environment.NewLine);
                }
            }
            catch (Exception eError)
            {
                // for now this does not break the price update, i.e. the bot will attempt to update the following items - maybe it shouldn't as it is likely to fail again?
                MKMHelpers.LogError("sending price update to MKM", eError.Message, false, sRequestXML);
                return;
            }

        }

        private string checkArticle(XmlNode article, ref Dictionary<string, List<XmlNode>> myStock)
        {
            var sUrl = "http://not.initilaized";
            bool changeMT = false;
            if (article["condition"].InnerText == "MT") // treat mint cards as near mint for pricing purposes
            {
                article["condition"].InnerText = "NM";
                changeMT = true;
            }
            XmlDocument doc2 = null;
            var sArticleID = article["idProduct"].InnerText;
            try
            {
                /*XmlDocument doc2 = MKMInteract.RequestHelper.makeRequest("https://api.cardmarket.com/ws/v2.0/products/" + sArticleID, "GET");

                logBox.AppendText(OutputFormat.PrettyXml(doc2.OuterXml));*/

                sUrl = "https://api.cardmarket.com/ws/v2.0/articles/" + sArticleID +
                            "?idLanguage=" + article["language"]["idLanguage"].InnerText +
                            "&minCondition=" + article["condition"].InnerText + "&start=0&maxResults=150&isFoil="
                            + article["isFoil"].InnerText +
                            "&isSigned=" + article["isSigned"].InnerText +
                            "&isAltered=" + article["isAltered"].InnerText;

                doc2 = MKMInteract.RequestHelper.makeRequest(sUrl, "GET");
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("updating price of " + article["product"]["enName"].InnerText, eError.Message, false, sUrl);
                return null;
            }

            XmlNodeList similarItems = doc2.GetElementsByTagName("article");

            List<double> prices = new List<double>();
            int lastMatch = -1;
            bool ignoreSellersCountry = false;
            TraverseResult res = traverseSimilarItems(similarItems, article, ignoreSellersCountry, ref lastMatch, ref prices);
            if (settings.searchWorldwide && res == TraverseResult.NotEnoughSimilars) // if there isn't enough similar items being sold in seller's country, check other countries as well
            {
                ignoreSellersCountry = true;
                prices.Clear();
                lastMatch = -1;
                res = traverseSimilarItems(similarItems, article, ignoreSellersCountry, ref lastMatch, ref prices);
            }
            double priceEstimation = 0;
            double priceFactor = ignoreSellersCountry ? settings.priceFactorWorldwide : settings.priceFactor;
            if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice && res == TraverseResult.SequenceFound)
            {
                priceEstimation = prices[0] * priceFactor;
            }
            else if (res == TraverseResult.Culled)
            {
                if (settings.logLessThanMinimum)
                    MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                            sArticleID + ">>> " + article["product"]["enName"].InnerText +
                            " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                            "Current Price: " + article["price"].InnerText + ", unchanged, only " +
                            (lastMatch + 1) + " similar items found (but some outliers were culled)" +
                            (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine);
                return null;
            }
            else if (res == TraverseResult.HighVariance)
            {
                if (settings.logHighPriceVariance) // this signifies that prices were not updated due to too high variance
                    MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                        sArticleID + ">>> " + article["product"]["enName"].InnerText +
                        " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                        "NOT UPDATED - variance of prices among cheapest similar items is too high" +
                        (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine);
                return null;
            }
            else if (res == TraverseResult.NotEnoughSimilars)
            {
                if (settings.logLessThanMinimum)
                    MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                        sArticleID + ">>> " + article["product"]["enName"].InnerText +
                        " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                        "Current Price: " + article["price"].InnerText + ", unchanged, only " +
                        (lastMatch + 1) + " similar items found" +
                        (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine);
                return null;
            }
            else if (settings.condAcceptance == AcceptedCondition.SomeMatchesAbove && lastMatch + 1 < settings.priceMinSimilarItems)
            // at least one matching item above non-matching is required -> if there wasn't, the last match might have been before min. # of items

            {
                if (settings.logLessThanMinimum)
                    MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                        sArticleID + ">>> " + article["product"]["enName"].InnerText +
                        " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                        "Current Price: " + article["price"].InnerText + ", unchanged, only " +
                        (lastMatch + 1) + " similar items with an item with matching condition above them were found" +
                        (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine);
                return null;
            }
            else
            {
                // if any condition is allowed, use the whole sequence
                // if only matching is allowed, use whole sequence as well because it is only matching items
                if (settings.condAcceptance != AcceptedCondition.SomeMatchesAbove)
                    lastMatch = prices.Count - 1;
                if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfHighestPrice)
                    priceEstimation = prices[lastMatch] * priceFactor;
                else // estimation by average
                {
                    for (int i = 0; i <= lastMatch; i++)
                        priceEstimation += prices[i]; // priceEstimation is initialized to 0 above
                    priceEstimation /= (lastMatch + 1);
                    // linear interpolation between average (currently stored in priceEstimation) and highest price in the sequence
                    if (priceFactor > 0.5)
                        priceEstimation += (prices[lastMatch] - priceEstimation) * (priceFactor - 0.5) * 2;
                    else if (priceFactor < 0.5) // linear interpolation between lowest price and average
                        priceEstimation = prices[0] + (priceEstimation - prices[0]) * (priceFactor) * 2;
                }
            }

            // increase the estimate based on how many of those articles do we have in stock
            double markupValue = 0;
            if (settings.priceIgnorePlaysets && article["isPlayset"].InnerText == "true")
                markupValue = priceEstimation * settings.priceMarkup4;
            else
            {
                int count = Convert.ToInt32(article["count"].InnerText, CultureInfo.InvariantCulture);
                if (count == 2)
                    markupValue = priceEstimation * settings.priceMarkup2;
                else if (count == 3)
                    markupValue = priceEstimation * settings.priceMarkup3;
                else if (count > 3)
                    markupValue = priceEstimation * settings.priceMarkup4;
            }
            if (markupValue > settings.priceMarkupCap)
                markupValue = settings.priceMarkupCap;
            priceEstimation += markupValue;

            if (priceEstimation < settings.priceMinRarePrice
                && (article["product"]["rarity"].InnerText == "Rare" || article["product"]["rarity"].InnerText == "Mythic"))
                priceEstimation = settings.priceMinRarePrice;

            // check the estimation is OK
            string sOldPrice = article["price"].InnerText;
            double dOldPrice = Convert.ToDouble(sOldPrice, CultureInfo.InvariantCulture);
            string sNewPrice = priceEstimation.ToString("f2", CultureInfo.InvariantCulture);
            // if we are ignoring the playset flag -> dPrice/priceEstim are for single item, but sPrices for 4x
            if (settings.priceIgnorePlaysets && article["isPlayset"].InnerText == "true")
            {
                dOldPrice /= 4;
                sNewPrice = (priceEstimation * 4).ToString("f2", CultureInfo.InvariantCulture);
            }
            // check it is not above the max price change limits
            foreach (var limits in settings.priceMaxChangeLimits)
            {
                if (dOldPrice < limits.Key)
                {
                    double priceDif = dOldPrice - priceEstimation; // positive when our price is too high, negative when our price is too low
                    if (Math.Abs(priceDif) > dOldPrice * limits.Value)
                    {
                        priceEstimation = -1;
                        if (settings.logLargePriceChangeTooHigh && priceDif > 0 ||
                            settings.logLargePriceChangeTooLow && priceDif < 0)
                            MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                                sArticleID + ">>> " + article["product"]["enName"].InnerText +
                                " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                                "NOT UPDATED - change too large: Current Price: "
                                + sOldPrice + ", Calculated Price:" + sNewPrice +
                                (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine);

                    }
                    break;
                }
            }
            if (priceEstimation > 0 // is < 0 if change was too large
                && Math.Abs(priceEstimation - dOldPrice) != Double.Epsilon // don't update if it did not change - clearer log
                )
            {
                    // check against minimum price from local stock database
                    if (myStock != null && myStock.ContainsKey(article["product"]["enName"].InnerText.ToLower()))
                    {
                        List<XmlNode> listArticles = myStock[article["product"]["enName"].InnerText.ToLower()];
                        foreach (XmlNode n in listArticles)
                        {
                            if (n.Attributes["set"].InnerText.ToLower() == article["product"]["expansion"].InnerText.ToLower()
                                && n.Attributes["language"].InnerText == article["language"]["languageName"].InnerText
                                && n.Attributes["condition"].InnerText == article["condition"].InnerText
                                && n.Attributes["isFoil"].InnerText == article["isFoil"].InnerText
                                && n.Attributes["isPlayset"].InnerText == article["isPlayset"].InnerText
                                )
                            {
                                string minPrice = n.Attributes["minPrice"].InnerText;
                                double dminPrice = Convert.ToDouble(minPrice, CultureInfo.InvariantCulture);
                                if (priceEstimation < dminPrice)
                                {
                                    priceEstimation = dminPrice;
                                    sNewPrice = minPrice;
                                }
                                break;
                            }
                        }
                    }
                // log large change or small change when enabled
                if (settings.logUpdated && (settings.logSmallPriceChange ||
                    (priceEstimation > dOldPrice + settings.priceMinRarePrice || priceEstimation < dOldPrice - settings.priceMinRarePrice)))
                    MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend),
                        sArticleID + ">>> " + article["product"]["enName"].InnerText +
                        " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                        "Current Price: " + sOldPrice + ", Calculated Price:" + sNewPrice +
                        ", based on " + (lastMatch + 1) + " items" +
                        (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine);

                if (changeMT)
                    article["condition"].InnerText = "MT";

                return MKMInteract.RequestHelper.changeStockArticleBody(article, sNewPrice);
            }
            return null;
        }

        private TraverseResult traverseSimilarItems(XmlNodeList similarItems, XmlNode article, bool ignoreSellersCountry,
            ref int lastMatch, ref List<double> prices)
        {
            bool minNumberNotYetFound = true;
            foreach (XmlNode offer in similarItems)
            {
                if ((ignoreSellersCountry || offer["seller"]["address"]["country"].InnerText == MKMHelpers.sMyOwnCountry)
                    && (settings.priceIgnorePlaysets || (offer["isPlayset"].InnerText == article["isPlayset"].InnerText))
                    && offer["seller"]["idUser"].InnerText != MKMHelpers.sMyId // skip items listed by myself
                    )
                {
                    //MainView.Instance().logBox.Invoke(new logboxAppendCallback(this.logBoxAppend), article["product"]["enName"].InnerText + "\n", MainView.Instance());
                    //MainView.Instance().logBox.Invoke(new logboxAppendCallback(this.logBoxAppend), article["price"].InnerText + " " + offer["price"].InnerText + "\n", MainView.Instance());

                    if (offer["condition"].InnerText != article["condition"].InnerText && settings.condAcceptance == AcceptedCondition.OnlyMatching)
                        continue;

                    float price = Convert.ToSingle(offer["price"].InnerText, CultureInfo.InvariantCulture);
                    if (settings.priceIgnorePlaysets && offer["isPlayset"].InnerText == "true") // if we are ignoring playsets, work with the price of a single
                        price /= 4;

                    if (minNumberNotYetFound)
                    {
                        if (offer["condition"].InnerText == article["condition"].InnerText)
                            lastMatch = prices.Count;
                        prices.Add(price);
                        if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice)
                        {
                            lastMatch = 0; // so that it is correctly counted that 1 item was used to estimate the price
                            return TraverseResult.SequenceFound;
                        }
                        // we can now check for the outliers in the first part of the sequence
                        // If there are outliers on the right side (too expensive), we can directly end now - it will not get better
                        if (prices.Count == settings.priceMinSimilarItems)
                        {
                            // start from the median and go both ways to cut off significantly cheap items as well
                            int median = prices.Count / 2;
                            for (int i = median + 1; i < prices.Count; i++) // first the expensive ones to see if we can end immediately
                            {
                                if (prices[i] - prices[i - 1] > getMaxPriceDifference(prices[i - 1]))
                                    return TraverseResult.HighVariance;
                            }
                            for (int i = median - 1; i >= 0; i--)
                            {
                                if (prices[i + 1] - prices[i] > getMaxPriceDifference(prices[i + 1]))
                                {
                                    prices.RemoveRange(0, i + 1); // remove the first items until item i to get rid of all the outliers
                                    break;
                                }
                            }
                            // even if some cheapest outliers were culled, consider MIN number of items found and from now on
                            // cull only from the top - this way is ensured we stay on the cheaper side,
                            // after all, there can't be too many outliers - if there are, they are not outliers anymore
                            minNumberNotYetFound = false;
                        }
                    }
                    else
                    {
                        // check if it's not significantly more expensive than previous item
                        if (prices.Count > 1)
                        {
                            if (price - prices[prices.Count - 1] > getMaxPriceDifference(prices[prices.Count - 1]))
                            {
                                if (prices.Count < settings.priceMinSimilarItems)
                                    return TraverseResult.Culled;
                                else return TraverseResult.SequenceFound;
                            }
                        }
                        if (offer["condition"].InnerText == article["condition"].InnerText)
                            lastMatch = prices.Count;
                        prices.Add(price);
                    }
                    if (prices.Count >= settings.priceMaxSimilarItems)
                        return TraverseResult.SequenceFound;
                }
            }
            if (minNumberNotYetFound)
                return TraverseResult.NotEnoughSimilars;
            else if (prices.Count < settings.priceMinSimilarItems)
                return TraverseResult.Culled;
            else
                return TraverseResult.SequenceFound;
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
                    string sUrl = "https://api.cardmarket.com/ws/v1.1/output.xml/orders/2/" + iType + "/" + iPage;
                    XmlDocument doc = null;
                    try
                    {
                        doc = MKMInteract.RequestHelper.makeRequest(sUrl, "GET");
                    }
                    catch (Exception eError)
                    {
                        MKMHelpers.LogError("getting buys, no data downloaded", eError.Message, true, sUrl);
                        return "";
                    }

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
                                file.WriteLine("\"" + oID + "\";\"" + sOdate + "\";\"" + article["product"]["name"].InnerText
                                    // only single cards have expansion -> leave it empty for displays etc.
                                    + "\";\"" + (article["product"]["expansion"] == null ? "" : article["product"]["expansion"].InnerText)
                                    + "\";\"" + article["language"]["languageName"].InnerText
                                    + "\";\"" + article["price"].InnerText + "\"");
                            }
                            catch (Exception eError)
                            {
                                MKMHelpers.LogError("writing buy entry of " + article["product"]["name"].InnerText + ", product skipped", eError.Message, false);
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