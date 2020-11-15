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
#undef DEBUG

using System;
using System.Data;
using System.Globalization;
using System.IO;
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

        // Filtering
        public bool filterByExpansions; // if set to true, only articles from expansions included in allowedExpansions will be updated
        public List<string> allowedExpansions; // list of expansions to take into account when doing price update.
        public bool filterByCountries; // if set to true, only articles from sellers from the allowedCountryNames will be taken into account when doing worldwide search
        public List<string> allowedCountryNames; // list of countries to take into account when doing worldwide price update.
        public bool includePrivateSellers, includeProfessionalSellers, includePowersellers;

        /// Other Settings
        public bool testMode; // if set to true, price updates will be computed and logged, but not sent to MKM
        public bool searchWorldwide; // if the minimum of items is not found in the sellers country, do a search ignoring country - used only when nothing is culled!
        public string description; // overall description of what is this setting expected to do, written in the GUI

        public MKMBotSettings()
        {
            priceMaxChangeLimits = new SortedList<double, double>();
            priceMaxDifferenceLimits = new SortedList<double, double>();
            allowedExpansions = new List<string>();
            allowedCountryNames = new List<string>();

            priceMinRarePrice = 0.05;
            priceMinSimilarItems = 4; // require exactly 4 items
            priceMaxSimilarItems = 4;
            priceSetPriceBy = PriceSetMethod.ByAverage;
            priceFactor = 0.5;
            priceFactorWorldwide = 0.5;
            priceMarkup2 = 0;
            priceMarkup3 = 0;
            priceMarkup4 = 0;
            priceMarkupCap = 0;
            priceIgnorePlaysets = false;
            condAcceptance = AcceptedCondition.OnlyMatching;

            searchWorldwide = false;
            filterByExpansions = false;
            filterByCountries = false;
            includePrivateSellers = true;
            includeProfessionalSellers = true;
            includePowersellers = true;

            logUpdated = true;
            logLessThanMinimum = true;
            logSmallPriceChange = true;
            logLargePriceChangeTooLow = true;
            logLargePriceChangeTooHigh = true;
            logHighPriceVariance = true;

            testMode = false;
        }

        /// <summary>
        /// Copies all settings from the specified reference settings.
        /// </summary>
        /// <param name="refSettings">The reference settings.</param>
        public void Copy(MKMBotSettings refSettings)
        {
            priceMaxChangeLimits = new SortedList<double, double>(refSettings.priceMaxChangeLimits);
            priceMaxDifferenceLimits = new SortedList<double, double>(refSettings.priceMaxDifferenceLimits);
            allowedExpansions = new List<string>(refSettings.allowedExpansions);
            allowedCountryNames = new List<string>(refSettings.allowedCountryNames);

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

            filterByExpansions = refSettings.filterByExpansions;
            filterByCountries = refSettings.filterByCountries;
            includePrivateSellers = refSettings.includePrivateSellers;
            includeProfessionalSellers = refSettings.includeProfessionalSellers;
            includePowersellers = refSettings.includePowersellers;

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
            foreach (XmlNode child in root.ChildNodes)
            {
                switch (child.Name)
                {
                    case "priceMaxChangeLimits":
                    {
                        string[] limits = child.InnerText.Split(';');
                        for (int i = 1; i < limits.Length; i += 2)
                        {
                            threshold = double.Parse(limits[i - 1], NumberStyles.Float, CultureInfo.InvariantCulture);
                            allowedChange = double.Parse(limits[i], NumberStyles.Float, CultureInfo.InvariantCulture);
                            temp.priceMaxChangeLimits.Add(threshold, allowedChange);
                        }
                        break;
                    }
                    case "priceMaxDifferenceLimits":
                    {
                        string[] limits = child.InnerText.Split(';');
                        for (int i = 1; i < limits.Length; i += 2)
                        {
                            threshold = double.Parse(limits[i - 1], NumberStyles.Float, CultureInfo.InvariantCulture);
                            allowedChange = double.Parse(limits[i], NumberStyles.Float, CultureInfo.InvariantCulture);
                            temp.priceMaxDifferenceLimits.Add(threshold, allowedChange);
                        }
                        break;
                    }
                    case "allowedExpansions":
                    {
                        temp.allowedExpansions = new List<string>(child.InnerText.Split(';'));
                        break;
                    }
                    case "allowedCountryNames":
                    {
                        temp.allowedCountryNames = new List<string>(child.InnerText.Split(';'));
                        break;
                    }
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
                    case "searchWorldwide":
                        temp.searchWorldwide = bool.Parse(att.Value);
                        break;
                    case "filterByExpansions":
                        temp.filterByExpansions = bool.Parse(att.Value);
                        break;
                    case "filterByCountries":
                        temp.filterByCountries = bool.Parse(att.Value);
                        break;
                    case "includePrivateSellers":
                        temp.includePrivateSellers = bool.Parse(att.Value);
                        break;
                    case "includeProfessionalSellers":
                        temp.includeProfessionalSellers = bool.Parse(att.Value);
                        break;
                    case "includePowersellers":
                        temp.includePowersellers = bool.Parse(att.Value);
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
            child.InnerText = child.InnerText.Remove(child.InnerText.Length - 1); // remove the last semicolon
            root.AppendChild(child);

            child = s.CreateElement("priceMaxDifferenceLimits");
            child.InnerText = "";
            foreach (var limitPair in priceMaxDifferenceLimits)
                child.InnerText += "" + limitPair.Key + ";" + limitPair.Value.ToString("f2", CultureInfo.InvariantCulture) + ";";
            child.InnerText = child.InnerText.Remove(child.InnerText.Length - 1); // remove the last semicolon
            root.AppendChild(child);

            child = s.CreateElement("allowedExpansions");
            child.InnerText = "";
            foreach (var expansion in allowedExpansions)
                child.InnerText += "" + expansion + ";";
            child.InnerText = child.InnerText.Remove(child.InnerText.Length - 1); // remove the last semicolon
            root.AppendChild(child);

            child = s.CreateElement("allowedCountryNames");
            child.InnerText = "";
            foreach (var country in allowedCountryNames)
                child.InnerText += "" + country + ";";
            child.InnerText = child.InnerText.Remove(child.InnerText.Length - 1);
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

            root.SetAttribute("filterByExpansions", filterByExpansions.ToString());
            root.SetAttribute("filterByCountries", filterByCountries.ToString());
            root.SetAttribute("includePrivateSellers", includePrivateSellers.ToString());
            root.SetAttribute("includeProfessionalSellers", includeProfessionalSellers.ToString());
            root.SetAttribute("includePowersellers", includePowersellers.ToString());

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

        /// <summary>
        /// Determines whether the specified user type is allowed under current settings.
        /// </summary>
        /// <param name="isCommercial">Type of user obtained from the isCommercial field of the User entity.</param>
        /// <returns><c>True</c> if the specified user type is allowed; otherwise, <c>false</c>.</returns>
        public bool IsAllowedUserType(string isCommercial)
        {
            switch (isCommercial)
            {
                case "0":
                    return includePrivateSellers;
                case "1":
                    return includeProfessionalSellers;
                case "2":
                    return includePowersellers;
                default:
                    return false;
            }
        }
    }

    internal class MKMBot
    {
        private MKMBotSettings settings;
        
        public MKMBot()
        {
            settings = new MKMBotSettings();
        }

        public MKMBot(MKMBotSettings settings)
        {
            this.settings = settings;
        }

        public void SetSettings(MKMBotSettings s)
        {
            settings = s;
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

        /// <summary>
        /// Determines whether the specified expansion is allowed under current settings.
        /// </summary>
        /// <param name="expansion">The expansion to check.</param>
        /// <returns><c>True</c> if it is allowed or filtering by expansions is turned off; otherwise, <c>false</c>.
        /// </returns>
        private bool isAllowedExpansion(string expansion)
        {
            return !settings.filterByExpansions || settings.allowedExpansions.Contains(expansion);
        }

        /// <summary>
        /// Generates new prices for a provided list of cards using the current settings, except it ignores the
        /// max change price limits as the assumption is that there is not yet any user-set price.
        /// </summary>
        /// <param name="cardList">List of all the cards to appraise. Upon exiting, each card will gain two new attributes:
        /// KMTool Price and Price - Cheapest Similar. The first has the price computed, the second has the price of the item that
        /// is currently on sale and has the lowest price and is the same condition, language etc. 
        /// and also from domestic seller if worldwide search is not enabled in the settings.</param>
        /// <param name="useMyStock">If set to true, the MyStock.csv file will be used during the appraisal to 
        /// limit minimum prices of appraised cards.</param>
        public void GeneratePrices(List<MKMMetaCard> cardList, bool useMyStock)
        {
            settings.priceMaxChangeLimits.Clear();
            settings.logLargePriceChangeTooHigh = false;
            settings.logLargePriceChangeTooLow = false;

            MainView.Instance.LogMainWindow("Appraising card list...");
            Dictionary<string, List<MKMMetaCard>> myStock = useMyStock ? loadMyStock() : new Dictionary<string, List<MKMMetaCard>>();
            foreach (MKMMetaCard mc in cardList)
            {
                if (isAllowedExpansion(mc.GetAttribute(MCAttribute.Expansion)))
                {
                    XmlNodeList similarItems = getSimilarItems(mc);
                    if (similarItems != null)
                    {
                        string backupMKMPrice = mc.GetAttribute(MCAttribute.MKMPrice);
                        mc.SetAttribute(MCAttribute.MKMPrice, "-9999");
                        appraiseArticle(mc, similarItems, myStock);
                        if (backupMKMPrice != "")
                            mc.SetAttribute(MCAttribute.MKMPrice, backupMKMPrice);
                        else
                            mc.RemoveAttribute(MCAttribute.MKMPrice);
                    }
                }
            }

            MainView.Instance.LogMainWindow("List appraised.");
        }

        /// <summary>
        /// Reads the myStock.csv file used for setting minimal prices during appraisal.
        /// </summary>
        /// <returns>Dictionary of cards, key == card name or empty string for MetaCards that should ignore names.</returns>
        private Dictionary<string, List<MKMMetaCard>> loadMyStock()
        {
            Dictionary<string, List<MKMMetaCard>> myStock = new Dictionary<string, List<MKMMetaCard>>();
            if (File.Exists(@".//myStock.csv"))
            {
                MainView.Instance.LogMainWindow("Found myStock.csv, parsing minimal prices...");
                try
                {
                    DataTable stock = MKMCsvUtils.ConvertCSVtoDataTable(@".//myStock.csv");
                    if (stock.Columns.Contains(MCAttribute.MinPrice))
                    {
                        foreach (DataRow dr in stock.Rows)
                        {
                            MKMMetaCard card = new MKMMetaCard(dr);
                            if (card.GetAttribute(MCAttribute.MinPrice) != "") // if it does not have defined min price, it will be useless here
                            {
                                string name = card.GetAttribute(MCAttribute.Name);
                                if (!myStock.ContainsKey(name))
                                    myStock.Add(name, new List<MKMMetaCard>());
                                myStock[name].Add(card);
                            }
                        }
                    }
                    else
                        MainView.Instance.LogMainWindow("myStock.csv does not contain MinPrice column, will be ignored.");
                }
                catch (Exception eError)
                {
                    MKMHelpers.LogError("reading list of minimal prices, continuing price update without it", eError.Message, false);
                }
            }
            return myStock;
        }

        public void UpdatePrices()
        {
            if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice && settings.priceMaxChangeLimits.Count == 0)
            {
                MainView.Instance.LogMainWindow("Setting price according to lowest price is very risky - specify limits for maximal price change first!");
                return;
           }
            List<MKMMetaCard> articles;
            try
            {
                articles = MKMInteract.RequestHelper.getAllStockSingles(MainView.Instance.Config.UseStockGetFile);
            }
            catch (Exception error)
            {
                MKMHelpers.LogError("reading own stock, cannot continue price update", error.Message, true);
                return;
            }
            // load file with lowest prices
            Dictionary<string, List<MKMMetaCard>> myStock = loadMyStock();

            MainView.Instance.LogMainWindow("Updating Prices...");
            int putCounter = 0;
            string sRequestXML = "";
            foreach (MKMMetaCard MKMCard in articles)
            {
                if (isAllowedExpansion(MKMCard.GetAttribute(MCAttribute.Expansion)))
                {
                    XmlNodeList similarItems = getSimilarItems(MKMCard);
                    if (similarItems != null)
                    {
                        appraiseArticle(MKMCard, similarItems, myStock);
                        string newPrice = MKMCard.GetAttribute(MCAttribute.MKMToolPrice);
                        if (newPrice != "")
                        {
                            sRequestXML += MKMInteract.RequestHelper.changeStockArticleBody(MKMCard, newPrice);
                            // max 100 articles is allowed to be part of a PUT call - if we are there, call it
                            if (putCounter > 98 && !settings.testMode)
                            {
                                MKMInteract.RequestHelper.SendStockUpdate(sRequestXML, "PUT");
                                putCounter = 0;
                                sRequestXML = "";
                            }
                            else
                                putCounter++;
                        }
                    }
                }
            }
            if (settings.testMode)
            {
                MainView.Instance.LogMainWindow("Done. Prices NOT SENT to MKM - running in test mode finished.");
            }
            else if (sRequestXML.Length > 0)
            {
                MKMInteract.RequestHelper.SendStockUpdate(sRequestXML, "PUT");
            }
            else
            {
                MainView.Instance.LogMainWindow("Done. No valid/meaningful price updates created.");
            }

            string timeStamp = getTimestamp(DateTime.Now);

            MainView.Instance.LogMainWindow("Last Run finished: " + timeStamp);
        }

        /// <summary>
        /// For a specified card, makes an API request and gets articles on sale with the same product ID
        /// and if specified also the same: languageID, condition (same or better as the card), foil, signed and altered flags.
        /// </summary>
        /// <param name="card">The card template for which to get similar articles on sale on MKM.</param>
        /// <param name="maxNbItems">Maximum amount of items fetched from MKM. The larger the longer it usually takes MKM to process the request.</param>
        /// <returns>List with MKM "article" nodes, one for each similar items.</returns>
        private XmlNodeList getSimilarItems(MKMMetaCard card, int maxNbItems = 150)
        {
            string sUrl = "http://not.initilaized";
            string condition = card.GetAttribute(MCAttribute.Condition);
            if (condition == "MT") // treat mint cards as near mint for pricing purposes, but do not actually change the value in article
                condition = "NM";
            string productID = card.GetAttribute(MCAttribute.ProductID);
            string languageID = card.GetAttribute(MCAttribute.LanguageID);
            string isFoil = card.GetAttribute(MCAttribute.Foil);
            string isSigned = card.GetAttribute(MCAttribute.Signed);
            string isAltered = card.GetAttribute(MCAttribute.Altered);
            string isFirstEd = card.GetAttribute(MCAttribute.FirstEd);
            string articleName = card.GetAttribute(MCAttribute.Name);
            try
            {
                // from API documentation:
                // only articles from sellers with the specified user type are returned
                // (private for private sellers only;
                // commercial for all commercial sellers, including powersellers; 
                // powerseller for powersellers only)
                //
                // So we can't get an arbitrary combination, so let's do the best we can in a single request
                string userType = "";
                if (settings.includePrivateSellers)
                {
                    // we can use "private" only if we want neither professional nor powersellers, otherwise we have to get all
                    if (!(settings.includePowersellers || settings.includeProfessionalSellers))
                        userType = "private";
                }
                else if (settings.includeProfessionalSellers)
                    userType = "commercial"; // includes professional sellers and powersellers
                else if (settings.includePowersellers)
                    userType = "powerseller";
                else
                    return null; // if nothing is selected, return null
                sUrl = "https://api.cardmarket.com/ws/v2.0/articles/" + productID +
                            (languageID != "" ? "?idLanguage=" + card.GetAttribute(MCAttribute.LanguageID) : "") +
                            (condition != "" ? "&minCondition=" + condition : "") + (isFoil != "" ? "&isFoil=" + isFoil : "") +
                            (isSigned != "" ? "&isSigned=" + isSigned : "") + (isAltered != "" ? "&isAltered=" + isAltered : "") +
                            (isFirstEd != "" ? "&isFirstEd=" + isFirstEd : "") + (userType != "" ? "&userType=" + userType : "") +
                            "&start=0&maxResults=" + maxNbItems;

                return MKMInteract.RequestHelper.makeRequest(sUrl, "GET").GetElementsByTagName("article");
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("updating price of " + articleName, eError.Message, false, sUrl);
                return null;
            }
        }


        /// <summary>
        /// Sets a price to the specified article based on the current bot settings.
        /// </summary>
        /// <param name="article">An initialized MKMMetaCard describing the articles. Must have productID set - ideally initialize
        /// using the constructor from XMLNode. Upon exiting this method, the attributes MKMTool Price and Price - Cheapest Similar will be set
        /// for it: the first has the price computed, the second has the price of the item that
        /// is currently on sale and has the lowest price and is the same condition, language etc. 
        /// and also from domestic seller if worldwide search is not enabled in the settings. If price cannot be computed, the attribute will be empty.</param>
        /// <param name="myStock">A list of cards with minPrice set to compare with the computed price - MKMToolPrice will never be lower 
        /// then the highest minPrice among all matching cards in this list. Hashed by the card name.</param>
        private void appraiseArticle(MKMMetaCard article, XmlNodeList similarItems, Dictionary<string, List<MKMMetaCard>> myStock)
        {
            string productID = article.GetAttribute(MCAttribute.ProductID);
            string articleName = article.GetAttribute(MCAttribute.Name);
            article.SetAttribute(MCAttribute.MKMToolPrice, "");
            article.SetAttribute(MCAttribute.PriceCheapestSimilar, "");

            List<double> prices = new List<double>();
            int lastMatch = -1;
            bool ignoreSellersCountry = false;
            TraverseResult res = traverseSimilarItems(similarItems, article, ignoreSellersCountry, ref lastMatch, prices);
            if (settings.searchWorldwide && res == TraverseResult.NotEnoughSimilars // if there isn't enough similar items being sold in seller's country, check other countries as well
                || (settings.condAcceptance == AcceptedCondition.SomeMatchesAbove && lastMatch + 1 < settings.priceMinSimilarItems)
                // at least one matching item above non-matching is required -> if there wasn't, the last match might have been before min. # of items
                )
            {
                ignoreSellersCountry = true;
                prices.Clear();
                lastMatch = -1;
                res = traverseSimilarItems(similarItems, article, ignoreSellersCountry, ref lastMatch, prices);
            }
            double priceFactor = ignoreSellersCountry ? settings.priceFactorWorldwide : settings.priceFactor;
            string articleExpansion = article.GetAttribute(MCAttribute.Expansion);
            string articleLanguage = article.GetAttribute(MCAttribute.Language);
            string articlePrice = article.GetAttribute(MCAttribute.MKMPrice);
            double dArticlePrice = Convert.ToDouble(articlePrice, CultureInfo.InvariantCulture);
            double priceEstimation = dArticlePrice; // assume the price will not change -> new price is the orig price
            string logMessage = "";
            string isPlayset = article.GetAttribute(MCAttribute.Playset);
            if (res == TraverseResult.Culled)
            {
                if (settings.logLessThanMinimum)
                    logMessage += "Current Price: " + articlePrice + ", unchanged, only " +
                        (lastMatch + 1) + " similar items found (some outliers were culled)" +
                        (ignoreSellersCountry ? " - worldwide search!" : "");
            }
            else if (res == TraverseResult.HighVariance)
            {
                if (settings.logHighPriceVariance) // this signifies that prices were not updated due to too high variance
                    logMessage += "NOT UPDATED - variance of prices among cheapest similar items is too high" +
                        (ignoreSellersCountry ? " - worldwide search!" : "");
            }
            else if (res == TraverseResult.NotEnoughSimilars)
            {
                if (settings.logLessThanMinimum)
                    logMessage += "Current Price: " + articlePrice + ", unchanged, only " +
                        (lastMatch + 1) + " similar items found" +
                        (ignoreSellersCountry ? " - worldwide search!" : "");
            }
            else if (settings.condAcceptance == AcceptedCondition.SomeMatchesAbove && lastMatch + 1 < settings.priceMinSimilarItems)
            // at least one matching item above non-matching is required -> if there wasn't, the last match might have been before min. # of items

            {
                if (settings.logLessThanMinimum)
                    logMessage += "Current Price: " + articlePrice + ", unchanged, only " +
                        (lastMatch + 1) + " similar items with an item with matching condition above them were found" +
                        (ignoreSellersCountry ? " - worldwide search!" : "");
            }
            else
            {
                if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice && res == TraverseResult.SequenceFound)
                {
                    priceEstimation = prices[0] * priceFactor;
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
                        priceEstimation = 0;
                        for (int i = 0; i <= lastMatch; i++)
                            priceEstimation += prices[i]; // priceEstimation is initialized to 0 above
                        priceEstimation /= lastMatch + 1;
                        // linear interpolation between average (currently stored in priceEstimation) and highest price in the sequence
                        if (priceFactor > 0.5)
                            priceEstimation += (prices[lastMatch] - priceEstimation) * (priceFactor - 0.5) * 2;
                        else if (priceFactor < 0.5) // linear interpolation between lowest price and average
                            priceEstimation = prices[0] + (priceEstimation - prices[0]) * (priceFactor) * 2;
                    }
                }

                // increase the estimate based on how many of those articles do we have in stock
                double markupValue = 0;
                if (settings.priceIgnorePlaysets && isPlayset == "true")
                    markupValue = priceEstimation * settings.priceMarkup4;
                else if (int.TryParse(article.GetAttribute(MCAttribute.Count), NumberStyles.Any, 
                    CultureInfo.InvariantCulture, out int iCount))
                {
                    if (iCount == 2)
                        markupValue = priceEstimation * settings.priceMarkup2;
                    else if (iCount == 3)
                        markupValue = priceEstimation * settings.priceMarkup3;
                    else if (iCount > 3)
                        markupValue = priceEstimation * settings.priceMarkup4;
                }
                if (markupValue > settings.priceMarkupCap)
                    markupValue = settings.priceMarkupCap;
                priceEstimation += markupValue;

                string articleRarity = article.GetAttribute(MCAttribute.Rarity);
                if (priceEstimation < settings.priceMinRarePrice
                    && (articleRarity == "Rare" || articleRarity == "Mythic"))
                    priceEstimation = settings.priceMinRarePrice;

                // check the estimation is OK
                double dOldPrice = dArticlePrice;
                string sNewPrice;
                // just a temporary to correctly convert priceEstimation to string based on is/isn't playset; is/isn't less than minimum allowed price (0.02€)
                double priceToSet = priceEstimation;
                // if we are ignoring the playset flag -> dPrice/priceEstim are for single item, but sPrices for 4x
                if (settings.priceIgnorePlaysets && isPlayset == "true")
                {
                    dOldPrice /= 4;
                    priceToSet *= 4;
                    if (priceToSet < 0.02) // minimum price MKM accepts for a single article is 0.02€
                    {
                        priceToSet = 0.02;
                        priceEstimation = 0.005; // 0.02/4
                    }
                }
                else if (priceToSet < 0.02)
                    priceToSet = priceEstimation = 0.02;
                sNewPrice = priceToSet.ToString("f2", CultureInfo.InvariantCulture);
                // check it is not above the max price change limits
                bool changeTooLarge = false;
                foreach (var limits in settings.priceMaxChangeLimits)
                {
                    if (dOldPrice < limits.Key)
                    {
                        double priceDif = dOldPrice - priceEstimation; // positive when our price is too high, negative when our price is too low
                        if (Math.Abs(priceDif) > dOldPrice * limits.Value)
                        {
                            changeTooLarge = true;
                            priceEstimation = dArticlePrice; // restore to no price change
                            if (settings.logLargePriceChangeTooHigh && priceDif > 0 ||
                                settings.logLargePriceChangeTooLow && priceDif < 0)
                                logMessage += "NOT UPDATED - change too large: Current Price: "
                                    + articlePrice + ", Calculated Price:" + sNewPrice +
                                    (ignoreSellersCountry ? " - worldwide search!" : "");

                        }
                        break;
                    }
                }
                if (!changeTooLarge // is < 0 if change was too large
                    && Math.Abs(priceEstimation - dOldPrice) > 0.005) // don't update if it did not change - clearer log
                {
                    // log large change or small change when enabled
                    if (settings.logUpdated && (settings.logSmallPriceChange ||
                        priceEstimation > dOldPrice + settings.priceMinRarePrice || priceEstimation < dOldPrice - settings.priceMinRarePrice))
                        logMessage += "Current Price: " + articlePrice + ", Calculated Price:" + sNewPrice +
                            ", based on " + (lastMatch + 1) + " items" +
                            (ignoreSellersCountry ? " - worldwide search!" : "");

                    article.SetAttribute(MCAttribute.MKMToolPrice, sNewPrice);
                }
            }

            // finally, check against minimum price from local stock database
            // Check if the card itself has MinPrice defined - won't happen for traditional update, but can for External List Appraisal
            string sOwnMinPrice = article.GetAttribute(MCAttribute.MinPrice);
            if (double.TryParse(sOwnMinPrice, out double dOwnMinPrice))
            {
                if (isPlayset == "true")
                    dOwnMinPrice /= 4;
                if (priceEstimation < dOwnMinPrice)
                {
                    priceEstimation = dOwnMinPrice;
                }
            }
            List<MKMMetaCard> listArticles = new List<MKMMetaCard>();
            if (myStock.ContainsKey(""))
                listArticles.AddRange(myStock[""]); // special treatment for entries that are not for a specific card name
            if (myStock.ContainsKey(articleName))
                listArticles.AddRange(myStock[articleName]);
            if (listArticles.Count > 0)
            {
                string sNewPrice = article.GetAttribute(MCAttribute.MKMToolPrice);
                bool minPriceUsed = false;
                //reset the min price - card.Equals would otherwise resolve an equal card as not equal because the cards coming from MKM do not have the Min Price
                article.SetAttribute(MCAttribute.MinPrice, "");
                foreach (MKMMetaCard card in listArticles)
                {
                    if (card.Equals(article))
                    {
                        string minPrice = card.GetAttribute(MCAttribute.MinPrice);
                        double dminPrice = Convert.ToDouble(minPrice, CultureInfo.InvariantCulture);
                        if (isPlayset == "true")
                            dminPrice /= 4;
                        if (priceEstimation < dminPrice)
                        {
                            priceEstimation = dminPrice;
                            sNewPrice = minPrice;
                            minPriceUsed = true;
                        }
                    }
                }
                article.SetAttribute(MCAttribute.MinPrice, sOwnMinPrice); // restore the Min Price
                if (minPriceUsed)
                {
                    // the min price might restore the initial price, in that case don't send any update
                    if (Math.Abs(priceEstimation - dArticlePrice) > 0.009)
                    {
                        article.SetAttribute(MCAttribute.MKMToolPrice, sNewPrice);
                        if (logMessage.Length > 0)
                            logMessage += ". Too low, using minPrice " + sNewPrice;
                        else if (settings.logSmallPriceChange ||
                            priceEstimation > dArticlePrice + settings.priceMinRarePrice || priceEstimation < dArticlePrice - settings.priceMinRarePrice)
                        {
                            logMessage += "Current Price: " + articlePrice + ", using minPrice:" + sNewPrice + ".";
                        }
                    }
                    else
                        article.SetAttribute(MCAttribute.MKMToolPrice, ""); // no new price, don't set a useless update
                }
            }

            if (logMessage.Length > 0)
            {
                logMessage = productID + ">>> " + articleName + " (" + articleExpansion + ", " + 
                    (articleLanguage != "" ? articleLanguage : "unknown language") + ")"
                    + Environment.NewLine + logMessage;
                MainView.Instance.LogMainWindow(logMessage);
            }
        }

        private TraverseResult traverseSimilarItems(XmlNodeList similarItems, MKMMetaCard article, bool ignoreSellersCountry,
            ref int lastMatch, List<double> prices)
        {
            bool minNumberNotYetFound = true;
            string articleCondition = article.GetAttribute(MCAttribute.Condition);
            string isPlayset = article.GetAttribute(MCAttribute.Playset);
            bool ignorePlaysets = settings.priceIgnorePlaysets || (isPlayset == "");
            foreach (XmlNode offer in similarItems)
            {
                if (!settings.IsAllowedUserType(offer["seller"]["isCommercial"].InnerText))
                    continue;
                string sellerCountryCode = offer["seller"]["address"]["country"].InnerText;
                bool isntFromMyCountry = sellerCountryCode != MKMHelpers.sMyOwnCountry;
                if (ignoreSellersCountry)
                {
                    // if we are ignoring seller's country, check the country filter
                    if (settings.filterByCountries &&
                        !settings.allowedCountryNames.Contains(MKMHelpers.countryNames[sellerCountryCode]))
                        continue;
                }
                else if (isntFromMyCountry)
                    continue;
                if ((ignorePlaysets || ((offer["isPlayset"] != null) && (offer["isPlayset"].InnerText == isPlayset))) // isPlayset can be null for some games (not MTG)
                    && offer["seller"]["idUser"].InnerText != MKMHelpers.sMyId // skip items listed by myself
                    )
                {
                    if (offer["condition"].InnerText != articleCondition && settings.condAcceptance == AcceptedCondition.OnlyMatching)
                        continue;

                    float price = Convert.ToSingle(offer["price"].InnerText, CultureInfo.InvariantCulture);
                    if (ignorePlaysets && (offer["isPlayset"] != null)  && (offer["isPlayset"].InnerText == "true")) // if we are ignoring playsets, work with the price of a single
                        price /= 4.0f;

                    if (minNumberNotYetFound)
                    {
                        if (offer["condition"].InnerText == articleCondition)
                            lastMatch = prices.Count;
                        prices.Add(price);
                        if (article.GetAttribute(MCAttribute.PriceCheapestSimilar) == "")
                            article.SetAttribute(MCAttribute.PriceCheapestSimilar, "" + price);
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
                        if (offer["condition"].InnerText == articleCondition)
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

        private string getTimestamp(DateTime now)
        {
            return now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        public static string GetBuys(MainView mainView, string iType)
        {
            int iPage = 1;

            string sFilename = ".\\mcmbuys_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".csv";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(sFilename))
            {
                /*
                    bought or 1
                    paid or 2
                    sent or 4
                    received or 8
                    lost or 32
                    cancelled or 128
                */

                int count;
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

                    iPage += 100;

                    //Console.WriteLine(count);

                    var oNodes = doc.GetElementsByTagName("order");

                    foreach (XmlNode order in oNodes)
                    {
                        var aNodes = order.SelectNodes("article");

                        string oID = order["idOrder"].InnerText;
                        string sOdate = order["state"]["dateReceived"].InnerText;

                        foreach (XmlNode article in aNodes)
                        {
                            try
                            {
                                // MKM returns the card and expansion name in the language of the article
                                // -> for now let's output all in English with the help of our database
                                string name = article["product"]["name"].InnerText;
                                string expansion = "";
                                if (article["product"]["expansion"] != null) // ... but our database works only for singles -> so do it only for that (only singles have expansion)
                                {
                                    if (article["language"]["idLanguage"].InnerText != "1") // "1" == English
                                    {
                                        var card = MKMDbManager.Instance.GetSingleCard(article["idProduct"].InnerText);
                                        expansion = MKMDbManager.Instance.GetExpansionName(card.Field<string>(MKMDbManager.InventoryFields.ExpansionID));
                                        name = card.Field<string>(MKMDbManager.InventoryFields.Name);
                                    }
                                    else
                                        expansion = article["product"]["expansion"].InnerText;
                                }
                                // else only single cards have expansion -> leave it empty for displays etc.
                                file.WriteLine("\"" + oID + "\";\"" + sOdate + "\";\"" + name
                                    + "\";\"" + expansion
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