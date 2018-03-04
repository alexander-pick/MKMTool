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
    public enum TraverseResult { SequenceFound, // succesfully found long enough sequence of similar items
        Culled, // not enough items were found and some  outliers were culled
        HighVariance, // not enough items were found because of high difference between few cheap items and many expensive ones
        NotEnoughSimilars // not enough items were found - even without any culling
    }

    /// <summary>
    /// Contains all customizable settings that are used by MKMBot.
    /// All numbers expressed as percentage must be saved as a double with 0 = 0%, 1 = 100%.
    /// Can be serialized into a xml, otherwise it is basically a struct -> all members are public
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
        // if price computed as percentage of lowest or higest price, priceFactor is that percentage
        public double priceFactor;


        /// Card Condition Settings
        
        public AcceptedCondition condAcceptance;


        /// Log Settings

        public bool logUpdated, logLessThanMinimum, logSmallPriceChange, logHighPriceChange, logHighPriceVariance;

        /// Other Settings

        public bool testMode; // if set to true, price updates will be computed and logged, but not sent to MKM
        public bool searchWorldwide; // if the minimum of items is not found in the sellers country, do a search ignoring country - used only when nothing is culled!
        public string description; // overal description of what is this setting expected to do, written in the GUI

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
            condAcceptance = refSettings.condAcceptance;
            logUpdated = refSettings.logUpdated;
            logLessThanMinimum = refSettings.logLessThanMinimum;
            logSmallPriceChange = refSettings.logSmallPriceChange;
            logHighPriceChange = refSettings.logHighPriceChange;
            logHighPriceVariance = refSettings.logHighPriceVariance;
            testMode = refSettings.testMode;
            description = refSettings.description;
            searchWorldwide = refSettings.searchWorldwide;
        }

        /// <summary>
        /// Fills this instance from data stored in XML.
        /// </summary>
        /// <param name="s">XML document representing an instance of MKMBotSettings. <seealso cref="Serialize()" /></param>
        /// <returns>True if parsing was succesful, false if it was not (in that case, the structure will remain unchanged).</returns>
        public bool Parse(XmlDocument s)
        {
            MKMBotSettings temp = new MKMBotSettings();
            try
            {
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
                                if (double.TryParse(limits[i - 1], NumberStyles.Float, CultureInfo.InvariantCulture, out threshold)
                                    && double.TryParse(limits[i], NumberStyles.Float, CultureInfo.InvariantCulture, out allowedChange))
                                    temp.priceMaxChangeLimits.Add(threshold, allowedChange);
                                else
                                    return false;
                            }
                            break;
                        case "priceMaxDifferenceLimits":
                            limits = child.InnerText.Split(';');
                            for (int i = 1; i < limits.Length; i += 2)
                            {
                                if (double.TryParse(limits[i - 1], NumberStyles.Float, CultureInfo.InvariantCulture, out threshold)
                                    && double.TryParse(limits[i], NumberStyles.Float, CultureInfo.InvariantCulture, out allowedChange))
                                    temp.priceMaxDifferenceLimits.Add(threshold, allowedChange);
                                else
                                    return false;
                            }
                            break;
                    }
                }
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
                        case "logHighPriceChange":
                            temp.logHighPriceChange = bool.Parse(att.Value);
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
            }
            catch (Exception)
            {
                return false;
            }
            this.Copy(temp); // nothing failed, let's keep the settings
            return true;
        }

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns>A xml representation of this object.</returns>
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

            root.SetAttribute("condAcceptance", condAcceptance.ToString());

            root.SetAttribute("logUpdated", logUpdated.ToString());
            root.SetAttribute("logLessThanMinimum", logLessThanMinimum.ToString());
            root.SetAttribute("logSmallPriceChange", logSmallPriceChange.ToString());
            root.SetAttribute("logHighPriceChange", logHighPriceChange.ToString());
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

            s.priceMaxChangeLimits.Clear(); // empty by default
            s.priceMaxDifferenceLimits.Clear(); // empty by default

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
            s.searchWorldwide = false;

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
                MessageBox.Show(eError.ToString());
                return new DataTable();
            }
        }

        public void updatePrices(MainView frm1)
        {
            if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice && settings.priceMaxChangeLimits.Count == 0)
            {
                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                    "Setting price according to lowest price is very risky - specify limits for maximal price change first!" + Environment.NewLine, frm1);
                return;
            }
            frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                "Updating Prices..." + Environment.NewLine, frm1);

            // should fix weird float errors on foregin systems.

            // TJ - this does not look like a good idea to me. MKM is sending data formated in a locale where '.' is used as decimal separator
            // it makes no sense to force switch to german locale here and then later start replacing all '.' by ','
            //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");

            //frm1.logBox.Invoke(new logboxAppendCallback(this.logBoxAppend), Application.CurrentCulture.EnglishName + "\n", frm1);

            var debugCounter = 0;
            
            string sRequestXML = "";
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
                // according to the API documentation, "The 'condition' key is only returned for single cards. "
                // -> check if condition exists to see if this is a single card or something else
                if (article["condition"] != null && article["idArticle"].InnerText != null && article["price"].InnerText != null)
                {
                    var sUrl = "http://not.initilaized";
                    if (article["condition"].InnerText == "MT") // treat mint cards as near mint for pricing purposes
                        article["condition"].InnerText = "NM";
                    try
                    {
                        var sArticleID = article["idProduct"].InnerText;

                        /*XmlDocument doc2 = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/products/" + sArticleID, "GET");

                        logBox.AppendText(OutputFormat.PrettyXml(doc2.OuterXml));*/
                        
                        sUrl = "https://www.mkmapi.eu/ws/v2.0/articles/" + sArticleID +
                                    "?idLanguage=" + article["language"]["idLanguage"].InnerText +
                                    "&minCondition=" + article["condition"].InnerText + "&start=0&maxResults=150&isFoil="
                                    + article["isFoil"].InnerText +
                                    "&isSigned=" + article["isSigned"].InnerText +
                                    "&isAltered=" + article["isAltered"].InnerText;

                        //string sUrl = "https://www.mkmapi.eu/ws/v2.0/articles/" + sArticleID;
                        //string sUrl = "https://www.mkmapi.eu/ws/v2.0/articles/" + sArticleID + "?start=0&maxResults=250";


                        var doc2 = MKMInteract.RequestHelper.makeRequest(sUrl, "GET");

                        XmlNodeList similarItems = doc2.GetElementsByTagName("article");
                        
                        List<double> prices = new List<double>();
                        string sOldPrice = article["price"].InnerText;
                        double dOldPrice = Convert.ToDouble(sOldPrice, CultureInfo.InvariantCulture);
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
                        if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice && res == TraverseResult.SequenceFound)
                        {
                            priceEstimation = prices[0] * settings.priceFactor;
                        }
                        else if (res == TraverseResult.Culled)
                        {
                            if (settings.logLessThanMinimum)
                                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                        sArticleID + ">>> " + article["product"]["enName"].InnerText +
                                        " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                                        "Current Price: " + article["price"].InnerText + ", unchanged, only " +
                                        (lastMatch + 1) + " similar items found (but some outliers were culled)" +
                                        (ignoreSellersCountry ? " - worldwide search!" : "")+ Environment.NewLine, frm1);
                            continue;
                        }
                        else if (res == TraverseResult.HighVariance)
                        {
                            if (settings.logHighPriceVariance) // this signifies that prices were not updated due to too high variance
                                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                    sArticleID + ">>> " + article["product"]["enName"].InnerText +
                                    " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                                    "NOT UPDATED - variance of prices among cheapest similar items is too high" +
                                    (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine, frm1);
                            continue;
                        }
                        else if (res == TraverseResult.NotEnoughSimilars)
                        {
                            if (settings.logLessThanMinimum)
                                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                    sArticleID + ">>> " + article["product"]["enName"].InnerText +
                                    " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                                    "Current Price: " + article["price"].InnerText + ", unchanged, only " +
                                    (lastMatch + 1) + " similar items found" +
                                    (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine, frm1);
                            continue;
                        }
                        else if (settings.condAcceptance == AcceptedCondition.SomeMatchesAbove && lastMatch + 1 < settings.priceMinSimilarItems)
                            // at least one matching item above non-matching is required -> if there wasn't, the last match might have been before min. # of items

                        {
                            if (settings.logLessThanMinimum)
                                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                    sArticleID + ">>> " + article["product"]["enName"].InnerText +
                                    " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                                    "Current Price: " + article["price"].InnerText + ", unchanged, only " +
                                    (lastMatch + 1) + " similar items with an item with matching condition above them were found" +
                                    (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine, frm1);
                            continue;
                        }
                        else
                        {
                            // if any condition is allowed, use the whole sequence
                            // if only matching is allowed, use whole sequence as well because it is only matching items
                            if (settings.condAcceptance != AcceptedCondition.SomeMatchesAbove)
                                lastMatch = prices.Count - 1;
                            if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfHighestPrice)
                                priceEstimation = prices[lastMatch] * settings.priceFactor;
                            else // estimation by average
                            {
                                for (int i = 0; i <= lastMatch; i++)
                                    priceEstimation += prices[i]; // priceEstimation is initialized to 0 above
                                priceEstimation /= (lastMatch + 1);
                                // linear interpolation between average (currently stored in priceEstimation) and highest price in the sequence
                                if (settings.priceFactor > 0.5)
                                    priceEstimation += (prices[lastMatch] - priceEstimation) * (settings.priceFactor - 0.5) * 2;
                                else if (settings.priceFactor < 0.5) // linear interpolation between lowest price and average
                                    priceEstimation = prices[0] + (priceEstimation - prices[0]) * (settings.priceFactor) * 2;
                            }
                        }
                        if (priceEstimation < settings.priceMinRarePrice && 
                            (article["product"]["rarity"].InnerText == "Rare" || article["product"]["rarity"].InnerText == "Mythic"))
                            priceEstimation = settings.priceMinRarePrice;

                        // check the estimation is OK
                        string sNewPrice = priceEstimation.ToString("f2", CultureInfo.InvariantCulture);

                        // check it is not above the max price change limits
                        foreach (var limits in settings.priceMaxChangeLimits)
                        {
                            if (dOldPrice < limits.Key)
                            {
                                if (Math.Abs(dOldPrice - priceEstimation) > dOldPrice * limits.Value)
                                {
                                    priceEstimation = -1;
                                    if (settings.logHighPriceVariance)
                                        frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                            sArticleID + ">>> " + article["product"]["enName"].InnerText +
                                            " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                                            "NOT UPDATED - change too large: Current Price: "
                                            + sOldPrice + ", Calcualted Price:" + sNewPrice +
                                            (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine, frm1);

                                }
                                break;
                            }
                        }
                        if (priceEstimation > 0 // is < 0 if change was too large
                            && Math.Abs(priceEstimation - dOldPrice) != Double.Epsilon // don't update if it did not change - clearer log
                            )
                        {
                            if (settings.logUpdated && (settings.logSmallPriceChange ||
                                (priceEstimation > dOldPrice + settings.priceMinRarePrice || priceEstimation < dOldPrice - settings.priceMinRarePrice)))
                                    frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                                        sArticleID + ">>> " + article["product"]["enName"].InnerText +
                                        " (" + article["product"]["expansion"].InnerText + ", " + article["language"]["languageName"].InnerText + ")" + Environment.NewLine +
                                        "Current Price: " + sOldPrice + ", Calcualted Price:" + sNewPrice +
                                        ", based on " + (lastMatch + 1) + " items" +
                                        (ignoreSellersCountry ? " - worldwide search!" : "") + Environment.NewLine, frm1);

                            sRequestXML += MKMInteract.RequestHelper.changeStockArticleBody(article, sNewPrice);
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

            if (settings.testMode)
            {
                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                    "Done. Prices NOT SENT to MKM - running in test mode finished." + Environment.NewLine,
                    frm1);
            }
            else if (sRequestXML.Length > 0)
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
                    debugCounter + "/" + iUpdated + " Articles updated successfully, " + iFailed + " failed" + Environment.NewLine, frm1);
                
                if (iFailed > 1)
                {
                    try
                    {
                        File.WriteAllText(@".\\log" + DateTime.Now.ToString("ddMMyyyy-HHmm") + ".log", rdoc.ToString());
                    }
                    catch (Exception eError)
                    {
                        frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend), "ERR Msg : " + eError.Message + Environment.NewLine,
                            frm1);
                    }
                }
            }
            else
            {
                frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                    "Done. No valid/meaningful price updates created." + Environment.NewLine, frm1);
            }

            String timeStamp = GetTimestamp(DateTime.Now);

            frm1.logBox.Invoke(new logboxAppendCallback(logBoxAppend),
                "Last Run finsihed: " + timeStamp + Environment.NewLine, frm1);
        }

        private TraverseResult traverseSimilarItems(XmlNodeList similarItems, XmlNode article, bool ignoreSellersCountry,
            ref int lastMatch, ref List<double> prices)
        {
            bool minNumberNotYetFound = true;
            foreach (XmlNode offer in similarItems)
            {
                if ((ignoreSellersCountry || offer["seller"]["address"]["country"].InnerText == MKMHelpers.sMyOwnCountry)
                    && offer["isPlayset"].InnerText == article["isPlayset"].InnerText
                    && offer["seller"]["idUser"].InnerText != MKMHelpers.sMyId // skip items listed by myself
                    )
                {
                    //frm1.logBox.Invoke(new logboxAppendCallback(this.logBoxAppend), article["product"]["enName"].InnerText + "\n", frm1);
                    //frm1.logBox.Invoke(new logboxAppendCallback(this.logBoxAppend), article["price"].InnerText + " " + offer["price"].InnerText + "\n", frm1);

                    if (offer["condition"].InnerText != article["condition"].InnerText && settings.condAcceptance == AcceptedCondition.OnlyMatching)
                        continue;

                    float price = Convert.ToSingle(offer["price"].InnerText, CultureInfo.InvariantCulture);

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