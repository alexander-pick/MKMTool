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
  public enum AcceptedCondition
  {
    OnlyMatching, // only items in matching condition will be considered similar
    SomeMatchesAbove, // accept better-condition items, but only if there is at least one more expensive matching-condition item
    Anything // items in any matching or better condition will be considered similar
  };

  // result of traversing the similar items listed on MKM
  public enum TraverseResult
  {
    SequenceFound, // successfully found long enough sequence of similar items
    Culled, // not enough items were found and some  outliers were culled
    HighVariance, // not enough items were found because of high difference between few cheap items and many expensive ones
    NotEnoughSimilars // not enough items were found - even without any culling
  }

  public struct TOSSResult
  {
    public double EstimatedPrice;
    public bool UsedWorldWideSearch;
    public int NumberOfItemsInSequence; // how many items is the estimate based on
    public bool ForceLog;
  }

  /// Contains all customizable settings that are used by MKMBot.
  /// All numbers expressed as percentage must be saved as a double with 0 = 0%, 1 = 100%.
  /// Can be serialized into a XML, otherwise it is basically a struct -> all members are public
  public class MKMBotSettings
  {
    /// How is min price used during price estimation.
    public enum UpdateMode
    {
      FullUpdate, // original behaviour - the price estimation algorithm runs in full and in the end compares
                  // computed value with minPrice, if it is lower, sets price to min price. if no price is computed, at least ensures min price
      UpdateOnlyBelowMinPrice, // runs full update only for articles with current price below minPrice
      OnlyEnsureMinPrice // does not run update, only checks if current price is below minPrice, if yes, sets minPrice as current
    }

    /// How is the min price from myStock matched 
    public enum MinPriceMatch
    {
      Highest, // of all matching metacards in mystock, the highest minPrice is chosen as the one to use
      Best // of all matching metacards in mystock, the one with the most non-null columns is chosen (highest in case of a tie)
    }

    // each pair is a <max price of item; max change in % allowed for such item>
    public SortedList<double, double> PriceMaxChangeLimits;
    // each pair is a <max price of item; max difference in % allowed for such the next item> to be considered as similar - used to cull outliers
    public SortedList<double, double> PriceMaxDifferenceLimits;
    public double PriceMinRarePrice;
    public int PriceMinSimilarItems, PriceMaxSimilarItems;
    public PriceSetMethod PriceSetPriceBy;
    // if price computed based on average, priceFactor = 0.5 will set price as average of similar items, at 0
    //      it will be equal to the lowest price, at 1 to the highest price. Remaining values are linear interpolation between either 
    //      min price and average (0-0.5) or average and highest price (0.5-1)
    // if price computed as percentage of lowest or highest price, priceFactor is that percentage
    public double PriceFactor;
    public double PriceFactorWorldwide; // the same, but for worldwide search

    public double PriceMarkup2, PriceMarkup3, PriceMarkup4; // in percent, markup to use when we have 2, 3 or more copies of the given card
    public double PriceMarkupCap; // in euro, max amount of money allowed to be added on top of the estimated price by the markup
    public bool PriceIgnorePlaysets; // if set to true, articles with isPlayset=true will be treated as four single cards - both for our stock and other sellers

    /// Card Condition Settings        
    public AcceptedCondition CondAcceptance;

    /// Log Settings
    public bool LogUpdated, LogLessThanMinimum, LogSmallPriceChange, LogLargePriceChangeTooLow,
  LogLargePriceChangeTooHigh, LogHighPriceVariance;

    // Filtering
    public bool FilterByExpansions; // if set to true, only articles from expansions included in allowedExpansions will be updated
    public List<string> AllowedExpansions; // list of expansions to take into account when doing price update.
    public bool FilterByCountries; // if set to true, only articles from sellers from the allowedCountryNames will be taken into account when doing worldwide search
    public List<string> AllowedCountryNames; // list of countries to take into account when doing worldwide price update.
    public bool IncludePrivateSellers, IncludeProfessionalSellers, IncludePowersellers;

    /// Other Settings
    public bool TestMode; // if set to true, price updates will be computed and logged, but not sent to MKM
    public bool SearchWorldwide; // if the minimum of items is not found in the sellers country, do a search ignoring country - used only when nothing is culled!
    public string Description; // overall description of what is this setting expected to do, written in the GUI
    public UpdateMode MinPriceUpdateMode;
    public MinPriceMatch MyStockMinPriceMatch;

    public MKMBotSettings()
    {
      PriceMaxChangeLimits = new SortedList<double, double>();
      PriceMaxDifferenceLimits = new SortedList<double, double>();
      AllowedExpansions = new List<string>();
      AllowedCountryNames = new List<string>();

      PriceMinRarePrice = 0.05;
      PriceMinSimilarItems = 4; // require exactly 4 items
      PriceMaxSimilarItems = 4;
      PriceSetPriceBy = PriceSetMethod.ByAverage;
      PriceFactor = 0.5;
      PriceFactorWorldwide = 0.5;
      PriceMarkup2 = 0;
      PriceMarkup3 = 0;
      PriceMarkup4 = 0;
      PriceMarkupCap = 0;
      PriceIgnorePlaysets = false;
      CondAcceptance = AcceptedCondition.OnlyMatching;

      SearchWorldwide = false;
      FilterByExpansions = false;
      FilterByCountries = false;
      IncludePrivateSellers = true;
      IncludeProfessionalSellers = true;
      IncludePowersellers = true;

      LogUpdated = true;
      LogLessThanMinimum = true;
      LogSmallPriceChange = true;
      LogLargePriceChangeTooLow = true;
      LogLargePriceChangeTooHigh = true;
      LogHighPriceVariance = true;

      TestMode = false;
      MinPriceUpdateMode = UpdateMode.FullUpdate;
      MyStockMinPriceMatch = MinPriceMatch.Highest;
    }

    /// Copies all settings from the specified reference settings.
    /// <param name="refSettings">The reference settings.</param>
    public void Copy(MKMBotSettings refSettings)
    {
      PriceMaxChangeLimits = new SortedList<double, double>(refSettings.PriceMaxChangeLimits);
      PriceMaxDifferenceLimits = new SortedList<double, double>(refSettings.PriceMaxDifferenceLimits);
      AllowedExpansions = new List<string>(refSettings.AllowedExpansions);
      AllowedCountryNames = new List<string>(refSettings.AllowedCountryNames);

      PriceMinRarePrice = refSettings.PriceMinRarePrice;
      PriceMinSimilarItems = refSettings.PriceMinSimilarItems;
      PriceMaxSimilarItems = refSettings.PriceMaxSimilarItems;
      PriceSetPriceBy = refSettings.PriceSetPriceBy;
      PriceFactor = refSettings.PriceFactor;
      PriceFactorWorldwide = refSettings.PriceFactorWorldwide;
      PriceMarkup2 = refSettings.PriceMarkup2;
      PriceMarkup3 = refSettings.PriceMarkup3;
      PriceMarkup4 = refSettings.PriceMarkup4;
      PriceMarkupCap = refSettings.PriceMarkupCap;
      PriceIgnorePlaysets = refSettings.PriceIgnorePlaysets;

      CondAcceptance = refSettings.CondAcceptance;

      FilterByExpansions = refSettings.FilterByExpansions;
      FilterByCountries = refSettings.FilterByCountries;
      IncludePrivateSellers = refSettings.IncludePrivateSellers;
      IncludeProfessionalSellers = refSettings.IncludeProfessionalSellers;
      IncludePowersellers = refSettings.IncludePowersellers;

      LogUpdated = refSettings.LogUpdated;
      LogLessThanMinimum = refSettings.LogLessThanMinimum;
      LogSmallPriceChange = refSettings.LogSmallPriceChange;
      LogLargePriceChangeTooLow = refSettings.LogLargePriceChangeTooLow;
      LogLargePriceChangeTooHigh = refSettings.LogLargePriceChangeTooHigh;
      LogHighPriceVariance = refSettings.LogHighPriceVariance;
      TestMode = refSettings.TestMode;
      Description = refSettings.Description;
      SearchWorldwide = refSettings.SearchWorldwide;
      MinPriceUpdateMode = refSettings.MinPriceUpdateMode;
      MyStockMinPriceMatch = refSettings.MyStockMinPriceMatch;
    }

    /// Fills this instance from data stored in XML.
    /// In case there is a failure, exception is thrown and the data previously contained in this instance will not be changed at all.
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
                temp.PriceMaxChangeLimits.Add(threshold, allowedChange);
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
                temp.PriceMaxDifferenceLimits.Add(threshold, allowedChange);
              }
              break;
            }
          case "allowedExpansions":
            {
              temp.AllowedExpansions = new List<string>(child.InnerText.Split(';'));
              break;
            }
          case "allowedCountryNames":
            {
              temp.AllowedCountryNames = new List<string>(child.InnerText.Split(';'));
              break;
            }
        }
      }
      temp.PriceFactorWorldwide = -1;
      foreach (XmlNode att in root.Attributes)
      {
        switch (att.Name)
        {
          case "priceMinRarePrice":
            temp.PriceMinRarePrice = double.Parse(att.Value, CultureInfo.InvariantCulture);
            break;
          case "priceMinSimilarItems":
            temp.PriceMinSimilarItems = int.Parse(att.Value, CultureInfo.InvariantCulture);
            break;
          case "priceMaxSimilarItems":
            temp.PriceMaxSimilarItems = int.Parse(att.Value, CultureInfo.InvariantCulture);
            break;
          case "priceSetPriceBy":
            temp.PriceSetPriceBy = (PriceSetMethod)Enum.Parse(typeof(PriceSetMethod), att.Value);
            break;
          case "priceFactor":
            temp.PriceFactor = double.Parse(att.Value, CultureInfo.InvariantCulture);
            break;
          case "priceFactorWorldwide":
            temp.PriceFactorWorldwide = double.Parse(att.Value, CultureInfo.InvariantCulture);
            break;
          case "priceMarkup2":
            temp.PriceMarkup2 = double.Parse(att.Value, CultureInfo.InvariantCulture);
            break;
          case "priceMarkup3":
            temp.PriceMarkup3 = double.Parse(att.Value, CultureInfo.InvariantCulture);
            break;
          case "priceMarkup4":
            temp.PriceMarkup4 = double.Parse(att.Value, CultureInfo.InvariantCulture);
            break;
          case "priceMarkupCap":
            temp.PriceMarkupCap = double.Parse(att.Value, CultureInfo.InvariantCulture);
            break;
          case "priceIgnorePlaysets":
            temp.PriceIgnorePlaysets = bool.Parse(att.Value);
            break;
          case "searchWorldwide":
            temp.SearchWorldwide = bool.Parse(att.Value);
            break;
          case "filterByExpansions":
            temp.FilterByExpansions = bool.Parse(att.Value);
            break;
          case "filterByCountries":
            temp.FilterByCountries = bool.Parse(att.Value);
            break;
          case "includePrivateSellers":
            temp.IncludePrivateSellers = bool.Parse(att.Value);
            break;
          case "includeProfessionalSellers":
            temp.IncludeProfessionalSellers = bool.Parse(att.Value);
            break;
          case "includePowersellers":
            temp.IncludePowersellers = bool.Parse(att.Value);
            break;
          case "condAcceptance":
            temp.CondAcceptance = (AcceptedCondition)Enum.Parse(typeof(AcceptedCondition), att.Value);
            break;
          case "logUpdated":
            temp.LogUpdated = bool.Parse(att.Value);
            break;
          case "logLessThanMinimum":
            temp.LogLessThanMinimum = bool.Parse(att.Value);
            break;
          case "logSmallPriceChange":
            temp.LogSmallPriceChange = bool.Parse(att.Value);
            break;
          case "logLargePriceChangeTooLow":
            temp.LogLargePriceChangeTooLow = bool.Parse(att.Value);
            break;
          case "logLargePriceChangeTooHigh":
            temp.LogLargePriceChangeTooHigh = bool.Parse(att.Value);
            break;
          case "logHighPriceVariance":
            temp.LogHighPriceVariance = bool.Parse(att.Value);
            break;
          case "testMode":
            temp.TestMode = bool.Parse(att.Value);
            break;
          case "updateMode":
            {
              switch (att.Value)
              {
                case "FullUpdate":
                  temp.MinPriceUpdateMode = UpdateMode.FullUpdate;
                  break;
                case "UpdateOnlyBelowMinPrice":
                  temp.MinPriceUpdateMode = UpdateMode.UpdateOnlyBelowMinPrice;
                  break;
                case "OnlyEnsureMinPrice":
                  temp.MinPriceUpdateMode = UpdateMode.OnlyEnsureMinPrice;
                  break;
              }
              break;
            }
          case "minPriceMatch":
            {
              switch (att.Value)
              {
                case "Highest":
                  temp.MyStockMinPriceMatch = MinPriceMatch.Highest;
                  break;
                case "Best":
                  temp.MyStockMinPriceMatch = MinPriceMatch.Best;
                  break;
              }
              break;
            }
          case "description":
            temp.Description = att.Value;
            break;
        }
      }
      if (temp.PriceFactorWorldwide == -1)
        temp.PriceFactorWorldwide = temp.PriceFactor; // for backwards compatibility - in 0.6.1 and older version, priceFactorWorldwide was always the same as priceFactor
      this.Copy(temp); // nothing failed, let's keep the settings
    }

    /// Serializes this instance.
    /// <returns>A XML representation of this object.</returns>
    public XmlDocument Serialize()
    {
      XmlDocument s = new XmlDocument();
      s.CreateXmlDeclaration("1.0", System.Text.Encoding.UTF8.ToString(), "no");
      XmlElement root = s.CreateElement("MKMBotSettings");

      if (PriceMaxChangeLimits.Count > 0)
      {
        XmlElement child = s.CreateElement("priceMaxChangeLimits");
        child.InnerText = "";
        foreach (var limitPair in PriceMaxChangeLimits)
          child.InnerText += "" + limitPair.Key + ";" + limitPair.Value.ToString("f2", CultureInfo.InvariantCulture) + ";";
        child.InnerText = child.InnerText.Remove(child.InnerText.Length - 1); // remove the last semicolon
        root.AppendChild(child);
      }
      if (PriceMaxDifferenceLimits.Count > 0)
      {
        XmlElement child = s.CreateElement("priceMaxDifferenceLimits");
        child.InnerText = "";
        foreach (var limitPair in PriceMaxDifferenceLimits)
          child.InnerText += "" + limitPair.Key + ";" + limitPair.Value.ToString("f2", CultureInfo.InvariantCulture) + ";";
        child.InnerText = child.InnerText.Remove(child.InnerText.Length - 1); // remove the last semicolon
        root.AppendChild(child);
      }
      if (AllowedExpansions.Count > 0)
      {
        XmlElement child = s.CreateElement("allowedExpansions");
        child.InnerText = "";
        foreach (var expansion in AllowedExpansions)
          child.InnerText += "" + expansion + ";";
        child.InnerText = child.InnerText.Remove(child.InnerText.Length - 1); // remove the last semicolon
        root.AppendChild(child);
      }
      if (AllowedCountryNames.Count > 0)
      {
        XmlElement child = s.CreateElement("allowedCountryNames");
        child.InnerText = "";
        foreach (var country in AllowedCountryNames)
          child.InnerText += "" + country + ";";
        child.InnerText = child.InnerText.Remove(child.InnerText.Length - 1);
        root.AppendChild(child);
      }
      root.SetAttribute("priceMinRarePrice", PriceMinRarePrice.ToString("f2", CultureInfo.InvariantCulture));
      root.SetAttribute("priceMinSimilarItems", PriceMinSimilarItems.ToString(CultureInfo.InvariantCulture));
      root.SetAttribute("priceMaxSimilarItems", PriceMaxSimilarItems.ToString(CultureInfo.InvariantCulture));
      root.SetAttribute("priceSetPriceBy", PriceSetPriceBy.ToString());
      root.SetAttribute("priceFactor", PriceFactor.ToString("f2", CultureInfo.InvariantCulture));
      root.SetAttribute("priceFactorWorldwide", PriceFactorWorldwide.ToString("f2", CultureInfo.InvariantCulture));
      root.SetAttribute("priceMarkup2", PriceMarkup2.ToString(CultureInfo.InvariantCulture));
      root.SetAttribute("priceMarkup3", PriceMarkup3.ToString(CultureInfo.InvariantCulture));
      root.SetAttribute("priceMarkup4", PriceMarkup4.ToString(CultureInfo.InvariantCulture));
      root.SetAttribute("priceMarkupCap", PriceMarkupCap.ToString(CultureInfo.InvariantCulture));
      root.SetAttribute("priceIgnorePlaysets", PriceIgnorePlaysets.ToString());

      root.SetAttribute("searchWorldwide", SearchWorldwide.ToString());
      root.SetAttribute("filterByExpansions", FilterByExpansions.ToString());
      root.SetAttribute("filterByCountries", FilterByCountries.ToString());
      root.SetAttribute("includePrivateSellers", IncludePrivateSellers.ToString());
      root.SetAttribute("includeProfessionalSellers", IncludeProfessionalSellers.ToString());
      root.SetAttribute("includePowersellers", IncludePowersellers.ToString());

      root.SetAttribute("condAcceptance", CondAcceptance.ToString());

      root.SetAttribute("logUpdated", LogUpdated.ToString());
      root.SetAttribute("logLessThanMinimum", LogLessThanMinimum.ToString());
      root.SetAttribute("logSmallPriceChange", LogSmallPriceChange.ToString());
      root.SetAttribute("logLargePriceChangeTooLow", LogLargePriceChangeTooLow.ToString());
      root.SetAttribute("logLargePriceChangeTooHigh", LogLargePriceChangeTooHigh.ToString());
      root.SetAttribute("logHighPriceVariance", LogHighPriceVariance.ToString());

      root.SetAttribute("testMode", TestMode.ToString());
      switch (MinPriceUpdateMode)
      {
        case UpdateMode.FullUpdate:
          root.SetAttribute("updateMode", "FullUpdate");
          break;
        case UpdateMode.UpdateOnlyBelowMinPrice:
          root.SetAttribute("updateMode", "UpdateOnlyBelowMinPrice");
          break;
        case UpdateMode.OnlyEnsureMinPrice:
          root.SetAttribute("updateMode", "OnlyEnsureMinPrice");
          break;
      }

      switch (MyStockMinPriceMatch)
      {
        case MinPriceMatch.Highest:
          root.SetAttribute("minPriceMatch", "Highest");
          break;
        case MinPriceMatch.Best:
          root.SetAttribute("minPriceMatch", "Best");
          break;
      }

      root.SetAttribute("description", Description);

      s.AppendChild(root);
      return s;
    }

    /// Determines whether the specified user type is allowed under current settings.
    /// <param name="isCommercial">Type of user obtained from the isCommercial field of the User entity.</param>
    /// <returns><c>True</c> if the specified user type is allowed; otherwise, <c>false</c>.</returns>
    public bool IsAllowedUserType(string isCommercial)
    {
      switch (isCommercial)
      {
        case "0":
          return IncludePrivateSellers;
        case "1":
          return IncludeProfessionalSellers;
        case "2":
          return IncludePowersellers;
        default:
          return false;
      }
    }

    /// Determines whether sellers from the specified country are allowed or not under these settings.
    /// Logs an error if the country code is not recognized.
    /// <param name="countryCode">The country code.</param>
    /// <returns>True if filtering by countries is off or the country is on the allowed list.
    /// False otherwise or if filtering is on and the code is not in our table (need to update the table).
    /// </returns>
    public bool IsAllowedSellerCountry(string countryCode)
    {
      if (FilterByCountries)
      {
        if (MKMHelpers.CountryNames.ContainsKey(countryCode))
          return AllowedCountryNames.Contains(MKMHelpers.CountryNames[countryCode]);
        else
        {
          MKMHelpers.LogError("filtering sellers by countries",
              "country code " + countryCode + " not recognized. Seller will be ignored.", false);
          return false;
        }
      }
      return true;
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

    /// Gets the maximum allowed price difference between the specified price and its neighbour.
    /// <param name="refPrice">The reference price.</param>
    /// <returns>Maximal allowed difference according to the <c>settings.priceMaxDifferenceLimits</c>.</returns>
    private double getMaxPriceDifference(double refPrice)
    {
      // find the right limit
      foreach (var limit in settings.PriceMaxDifferenceLimits)
      {
        if (limit.Key > refPrice)
          return refPrice * limit.Value;
      }
      return double.MaxValue;
    }

    /// Determines whether the specified expansion is allowed under current settings.
    /// <param name="expansion">The expansion to check.</param>
    /// <returns><c>True</c> if it is allowed or filtering by expansions is turned off; otherwise, <c>false</c>.
    /// </returns>
    private bool isAllowedExpansion(string expansion)
    {
      return !settings.FilterByExpansions || settings.AllowedExpansions.Contains(expansion);
    }

    /// Generates new prices for a provided list of cards using the current settings, except it ignores the
    /// max change price limits as the assumption is that there is not yet any user-set price.
    /// <param name="cardList">List of all the cards to appraise. Upon exiting, each card will gain two new attributes:
    /// KMTool Price and Price - Cheapest Similar. The first has the price computed, the second has the price of the item that
    /// is currently on sale and has the lowest price and is the same condition, language etc. 
    /// and also from domestic seller if worldwide search is not enabled in the settings.</param>
    /// <param name="useMyStock">If set to true, the MyStock.csv file will be used during the appraisal to 
    /// limit minimum prices of appraised cards.</param>
    public void GeneratePrices(List<MKMMetaCard> cardList, bool useMyStock)
    {
      if (!settings.IncludePrivateSellers && !settings.IncludeProfessionalSellers && !settings.IncludePowersellers)
      {
        MainView.Instance.LogMainWindow("All seller types excluded, need to include at least one type to be able to estimate prices.");
        return;
      }
      settings.PriceMaxChangeLimits.Clear();
      settings.LogLargePriceChangeTooHigh = false;
      settings.LogLargePriceChangeTooLow = false;

      MainView.Instance.LogMainWindow("Appraising card list...");
      Dictionary<string, List<MKMMetaCard>> myStock = useMyStock ? loadMyStock() : new Dictionary<string, List<MKMMetaCard>>();
      foreach (MKMMetaCard mc in cardList)
      {
        if (isAllowedExpansion(mc.GetAttribute(MCAttribute.Expansion)))
        {
          string backupMKMPrice = mc.GetAttribute(MCAttribute.MKMPrice);
          mc.SetAttribute(MCAttribute.MKMPrice, "-9999");
          appraiseArticle(mc, myStock);
          if (backupMKMPrice != "")
            mc.SetAttribute(MCAttribute.MKMPrice, backupMKMPrice);
          else
            mc.RemoveAttribute(MCAttribute.MKMPrice);
        }
      }

      MainView.Instance.LogMainWindow("List appraised.");
    }

    /// Reads the myStock.csv file used for setting minimal prices during appraisal.
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
      if (!settings.IncludePrivateSellers && !settings.IncludeProfessionalSellers && !settings.IncludePowersellers)
      {
        MainView.Instance.LogMainWindow("All seller types excluded, need to include at least one type to be able to estimate prices.");
        return;
      }
      if (settings.PriceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice && settings.PriceMaxChangeLimits.Count == 0)
      {
        MainView.Instance.LogMainWindow("Setting price according to lowest price is very risky - specify limits for maximal price change first!");
        return;
      }
      List<MKMMetaCard> articles;
      try
      {
        articles = MKMInteract.RequestHelper.GetAllStockSingles(MainView.Instance.Config.UseStockGetFile);
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
      // to process only a limited amount of articles (to save on API requests) change the "false" on next line to "true"
#if false
            int startFrom = 0; // first item that will be processed - you only need to change this
            int batchSize = 3000; // size of the batch to process at one go
            int end = startFrom + batchSize;
            if (end > articles.Count)
                end = articles.Count;
            for (; startFrom < end; startFrom++)
            {
                MKMMetaCard MKMCard = articles[startFrom];
#else
      foreach (MKMMetaCard MKMCard in articles)
      {
#endif
        if (isAllowedExpansion(MKMCard.GetAttribute(MCAttribute.Expansion)))
        {
          appraiseArticle(MKMCard, myStock);
          string newPrice = MKMCard.GetAttribute(MCAttribute.MKMToolPrice);
          if (newPrice != "")
          {
            sRequestXML += MKMInteract.RequestHelper.ChangeStockArticleBody(MKMCard, newPrice);
            // max 100 articles is allowed to be part of a PUT call - if we are there, call it
            if (putCounter > 98 && !settings.TestMode)
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
      if (settings.TestMode)
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

    /// For a specified card, makes an API request and gets articles on sale with the same product ID
    /// and if specified also the same: languageID, condition (same or better as the card), foil, signed and altered flags.
    /// <param name="card">The card template for which to get similar articles on sale on MKM.</param>
    /// <param name="maxNbItems">Maximum amount of items fetched from MKM. The larger the longer it usually takes MKM to process the request.</param>
    /// <returns>List with MKM "article" nodes, one for each similar items. Null in case fetching similar items failed.</returns>
    private XmlNodeList getSimilarItems(MKMMetaCard card, int maxNbItems)
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
        if (settings.IncludePrivateSellers)
        {
          // we can use "private" only if we want neither professional nor powersellers, otherwise we have to get all
          if (!(settings.IncludePowersellers || settings.IncludeProfessionalSellers))
            userType = "private";
        }
        else if (settings.IncludeProfessionalSellers)
          userType = "commercial"; // includes professional sellers and powersellers
        else if (settings.IncludePowersellers)
          userType = "powerseller";
        sUrl = "https://api.cardmarket.com/ws/v2.0/articles/" + productID +
                    (languageID != "" ? "?idLanguage=" + card.GetAttribute(MCAttribute.LanguageID) : "") +
                    (condition != "" ? "&minCondition=" + condition : "") + (isFoil != "" ? "&isFoil=" + isFoil : "") +
                    (isSigned != "" ? "&isSigned=" + isSigned : "") + (isAltered != "" ? "&isAltered=" + isAltered : "") +
                    (isFirstEd != "" ? "&isFirstEd=" + isFirstEd : "") + (userType != "" ? "&userType=" + userType : "") +
                    "&start=0&maxResults=" + maxNbItems;

        return MKMInteract.RequestHelper.MakeRequest(sUrl, "GET").GetElementsByTagName("article");
      }
      catch (Exception eError)
      {
        MKMHelpers.LogError("updating price of " + articleName, eError.Message, false, sUrl);
        return null;
      }
    }


    /// Sets a price to the specified article based on the current bot settings.
    /// <param name="article">An initialized MKMMetaCard describing the articles. Must have productID set - ideally initialize
    /// using the constructor from XMLNode. Upon exiting this method, the attributes MKMTool Price and Price - Cheapest Similar will be set
    /// for it: the first has the price computed, the second has the price of the item that
    /// is currently on sale and has the lowest price and is the same condition, language etc. 
    /// and also from domestic seller if worldwide search is not enabled in the settings. If price cannot be computed, the attribute will be empty.</param>
    /// <param name="myStock">A list of cards with minPrice set to compare with the computed price - MKMToolPrice will never be lower 
    /// then the highest minPrice among all matching cards in this list. Hashed by the card name.</param>
    private void appraiseArticle(MKMMetaCard article, Dictionary<string, List<MKMMetaCard>> myStock)
    {
      string productID = article.GetAttribute(MCAttribute.ProductID);
      string articleName = article.GetAttribute(MCAttribute.Name);
      article.SetAttribute(MCAttribute.MKMToolPrice, "");
      article.SetAttribute(MCAttribute.PriceCheapestSimilar, "");

      string articleExpansion = article.GetAttribute(MCAttribute.Expansion);
      string articleLanguage = article.GetAttribute(MCAttribute.Language);
      string articlePrice = article.GetAttribute(MCAttribute.MKMPrice);
      string isPlayset = article.GetAttribute(MCAttribute.Playset);
      // **playset are equal to **single in non-playset case, all string prices are always prices for playset when applicable
      double dArticlePlayset = Convert.ToDouble(articlePrice, CultureInfo.InvariantCulture);
      double dArticleSingle = dArticlePlayset;
      double minPricePlayset = 0.005;// minimum price MKM accepts for a single article is 0.02€
      string articleRarity = article.GetAttribute(MCAttribute.Rarity);
      int bestMatchCount = 0; // used for MKMBotSettings.MinPriceMatch.Best, treat it as 1 if we matched by rarity from
      if (articleRarity == "Rare" || articleRarity == "Mythic")
      {
        minPricePlayset = settings.PriceMinRarePrice;
        bestMatchCount = 1;
      }
      double minPriceSingle = minPricePlayset;
      if (isPlayset == "true")
      {
        dArticleSingle /= 4;
        minPricePlayset *= 4;
      }
      string logMessage = productID + ">> " + articleName + " (" + articleExpansion + ", " +
              (articleLanguage != "" ? articleLanguage : "unknown language") + ")" + Environment.NewLine;

      // compute min price first
      // Check if the card itself has MinPrice defined - won't happen for traditional update, but can for External List Appraisal
      string sOwnMinPrice = article.GetAttribute(MCAttribute.MinPrice);
      if (double.TryParse(sOwnMinPrice, out double dOwnMinPrice))
      {
        if (minPricePlayset < dOwnMinPrice)
        {
          minPricePlayset = minPriceSingle = dOwnMinPrice;
          if (isPlayset == "true")
            minPriceSingle /= 4;
        }
      }
      List<MKMMetaCard> listArticles = new List<MKMMetaCard>();
      if (myStock.ContainsKey(""))
        listArticles.AddRange(myStock[""]); // special treatment for entries that are not for a specific card name
      if (myStock.ContainsKey(articleName))
        listArticles.AddRange(myStock[articleName]);
      if (listArticles.Count > 0)
      {
        //reset the min price - card.Equals would otherwise resolve an equal card as not equal because the cards coming from MKM do not have the Min Price
        article.SetAttribute(MCAttribute.MinPrice, "");
        foreach (MKMMetaCard card in listArticles)
        {
          if (card.Equals(article))
          {
            if (settings.MyStockMinPriceMatch == MKMBotSettings.MinPriceMatch.Best)
            {
              int noAtts = card.GetNumberOfAttributes();
              if (noAtts < bestMatchCount)
                continue;
              else if (noAtts > bestMatchCount)// erase previous minPrice, we want this one
              {
                minPricePlayset = -9999;
                minPriceSingle = -9999;
              }
              bestMatchCount = noAtts; // if its bigger or the same
            }
            string sMinPriceTemp = card.GetAttribute(MCAttribute.MinPrice);
            double dMinPriceTemp = Convert.ToDouble(sMinPriceTemp, CultureInfo.InvariantCulture);
            if (minPricePlayset < dMinPriceTemp)
            {
              minPricePlayset = minPriceSingle = dMinPriceTemp;
              if (isPlayset == "true")
                minPriceSingle /= 4;
            }
          }
        }
        article.SetAttribute(MCAttribute.MinPrice, sOwnMinPrice); // restore the Min Price
      }

      string sNewPrice = "";
      if (settings.MinPriceUpdateMode == MKMBotSettings.UpdateMode.OnlyEnsureMinPrice)
      {
        if (minPricePlayset - dArticlePlayset > 0.009)
        {
          sNewPrice = minPricePlayset.ToString("f2", CultureInfo.InvariantCulture);
          article.SetAttribute(MCAttribute.MKMToolPrice, sNewPrice);
          if (settings.LogSmallPriceChange || minPricePlayset > dArticlePlayset + settings.PriceMinRarePrice)
          {
            logMessage += "Current Price: " + articlePrice + ", using minPrice:" + sNewPrice + ".";
            MainView.Instance.LogMainWindow(logMessage);
          }
        }
        return;
      }
      if (settings.MinPriceUpdateMode == MKMBotSettings.UpdateMode.UpdateOnlyBelowMinPrice && minPricePlayset <= dArticlePlayset)
        return;
      
      // run the TOSS
      double priceEstimationSingle = dArticleSingle; // assume the price will not change -> new price is the orig price
      var tossResult = performTOSS(article, ref logMessage);
      if (tossResult.EstimatedPrice > 0)
        priceEstimationSingle = tossResult.EstimatedPrice;
      bool forceLog = tossResult.ForceLog;
      // postprocess the estimated price - add markup for multiple copies and ensure it does not cross the max change
      if (priceEstimationSingle > 0)
      {
        // increase the estimate based on how many of those articles do we have in stock
        double markupValue = 0;
        if (settings.PriceIgnorePlaysets && isPlayset == "true")
          markupValue = priceEstimationSingle * settings.PriceMarkup4;
        else if (int.TryParse(article.GetAttribute(MCAttribute.Count), NumberStyles.Any,
            CultureInfo.InvariantCulture, out int iCount))
        {
          if (iCount == 2)
            markupValue = priceEstimationSingle * settings.PriceMarkup2;
          else if (iCount == 3)
            markupValue = priceEstimationSingle * settings.PriceMarkup3;
          else if (iCount > 3)
            markupValue = priceEstimationSingle * settings.PriceMarkup4;
        }
        if (markupValue > settings.PriceMarkupCap)
          markupValue = settings.PriceMarkupCap;
        priceEstimationSingle += markupValue;

        // just a temporary to correctly convert priceEstimation to string based on is/isn't playset; is/isn't less than minimum allowed price (0.02€)
        double priceToSet = priceEstimationSingle;
        // if we are ignoring the playset flag -> dPrice/priceEstim are for single item, but sPrices for 4x
        if (settings.PriceIgnorePlaysets && isPlayset == "true")
          priceToSet *= 4;
        sNewPrice = priceToSet.ToString("f2", CultureInfo.InvariantCulture);
        // check it is not above the max price change limits
        double priceDif = dArticleSingle - priceEstimationSingle; // positive when our price is too high, negative when our price is too low
        foreach (var limits in settings.PriceMaxChangeLimits)
        {
          if (dArticleSingle < limits.Key)
          {
            if (Math.Abs(priceDif) > dArticleSingle * limits.Value)
            {
              priceEstimationSingle = dArticleSingle; // restore to no price change
              if (settings.LogLargePriceChangeTooHigh && priceDif > 0 ||
                  settings.LogLargePriceChangeTooLow && priceDif < 0)
              {
                logMessage += "NOT UPDATED - change too large: Current Price: "
                    + articlePrice + ", Calculated Price:" + sNewPrice +
                    ((tossResult.UsedWorldWideSearch && tossResult.EstimatedPrice > 0) ? " - worldwide search! " : ". ");
                forceLog = true;
              }
            }
            break;
          }
        }
      }
      bool calculatedPrice = true;
      if (priceEstimationSingle < minPriceSingle)
      {
        double priceEstimPlayset = priceEstimationSingle; // just for log
        if (isPlayset == "true")
          priceEstimPlayset *= 4;
        priceEstimationSingle = minPriceSingle;
        sNewPrice = minPricePlayset.ToString("f2", CultureInfo.InvariantCulture);
        string sEstim = priceEstimPlayset.ToString("f2", CultureInfo.InvariantCulture);
        logMessage += "Calculated price too low (" + sEstim + "), using minPrice " + sNewPrice;
        calculatedPrice = false;
      }

      if (Math.Abs(priceEstimationSingle - dArticleSingle) > 0.009) // don't send useless update
      {
        // finally write the new price
        article.SetAttribute(MCAttribute.MKMToolPrice, sNewPrice);

        if (settings.LogUpdated && (settings.LogSmallPriceChange ||
            Math.Abs(priceEstimationSingle - dArticleSingle) > settings.PriceMinRarePrice))
        {
          if (calculatedPrice)
          {
            string basedOn = tossResult.EstimatedPrice > 0 ? tossResult.NumberOfItemsInSequence + " items" +
                (tossResult.UsedWorldWideSearch ? " - worldwide search! " : ". ") : "previous price being under MinPrice.";
            logMessage += "Current Price: " + articlePrice + ", Calculated Price:" + sNewPrice +
                ", based on " + basedOn;
          }
          MainView.Instance.LogMainWindow(logMessage);
        }
      }
      else if (forceLog)
        MainView.Instance.LogMainWindow(logMessage);
    }

    private TraverseResult traverseSimilarItems(XmlNodeList similarItems, MKMMetaCard article, bool ignoreSellersCountry,
        ref int lastMatch, List<double> prices)
    {
      bool minNumberNotYetFound = true;
      string articleCondition = article.GetAttribute(MCAttribute.Condition);
      string isPlayset = article.GetAttribute(MCAttribute.Playset);
      bool ignorePlaysets = settings.PriceIgnorePlaysets || (isPlayset == "");
      foreach (XmlNode offer in similarItems)
      {
        if (!settings.IsAllowedUserType(offer["seller"]["isCommercial"].InnerText))
          continue;
        string sellerCountryCode = offer["seller"]["address"]["country"].InnerText;
        bool isntFromMyCountry = sellerCountryCode != MainView.Instance.Config.MyCountryCode;
        if (ignoreSellersCountry)
        {
          // if we are ignoring seller's country, check the country filter
          if (!settings.IsAllowedSellerCountry(sellerCountryCode))
            continue;
        }
        else if (isntFromMyCountry)
          continue;
        if ((ignorePlaysets || ((offer["isPlayset"] != null) && (offer["isPlayset"].InnerText == isPlayset))) // isPlayset can be null for some games (not MTG)
            && offer["seller"]["idUser"].InnerText != MKMHelpers.SMyId // skip items listed by myself
            )
        {
          if (offer["condition"].InnerText != articleCondition && settings.CondAcceptance == AcceptedCondition.OnlyMatching)
            continue;

          float price = MKMHelpers.GetPriceFromXml(offer);
          if (price < 0) // error, price not found
            continue;
          if (ignorePlaysets && (offer["isPlayset"] != null) && (offer["isPlayset"].InnerText == "true")) // if we are ignoring playsets, work with the price of a single
            price /= 4.0f;

          if (minNumberNotYetFound)
          {
            if (offer["condition"].InnerText == articleCondition)
              lastMatch = prices.Count;
            prices.Add(price);
            if (article.GetAttribute(MCAttribute.PriceCheapestSimilar) == "")
              article.SetAttribute(MCAttribute.PriceCheapestSimilar, "" + price);
            if (settings.PriceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice)
            {
              lastMatch = 0; // so that it is correctly counted that 1 item was used to estimate the price
              return TraverseResult.SequenceFound;
            }
            // we can now check for the outliers in the first part of the sequence
            // If there are outliers on the right side (too expensive), we can directly end now - it will not get better
            if (prices.Count == settings.PriceMinSimilarItems)
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
                if (prices.Count < settings.PriceMinSimilarItems)
                  return TraverseResult.Culled;
                else return TraverseResult.SequenceFound;
              }
            }
            if (offer["condition"].InnerText == articleCondition)
            {
              // this can be true only if settings.condAcceptance == AcceptedCondition.SomeMatchesAbove
              // and we still have not found a match above the maxItems
              // now we have -> we can use the entire priceMaxSimilarItems item sequence for appraisal
              if (prices.Count >= settings.PriceMaxSimilarItems)
              {
                lastMatch = settings.PriceMaxSimilarItems - 1;
                return TraverseResult.SequenceFound;
              }
              lastMatch = prices.Count;
            }
            prices.Add(price);
          }
          if (prices.Count >= settings.PriceMaxSimilarItems &&
              (settings.CondAcceptance != AcceptedCondition.SomeMatchesAbove || lastMatch == prices.Count - 1))
          {
            // in case of SomeMatchesAbove, we want to look even above maxSimilarItems
            // to find out if there is some matching
            return TraverseResult.SequenceFound;
          }
        }
      }
      if (minNumberNotYetFound)
        return TraverseResult.NotEnoughSimilars;
      else if (prices.Count < settings.PriceMinSimilarItems)
        return TraverseResult.Culled;
      return TraverseResult.SequenceFound;
    }

    /// Performs the algorithm for estimating price based on other seller's stock.
    /// <param name="article">The article that is being appraised.</param>
    /// <param name="logMessage">Messages that should be written to log are appended to this, but not printed in the console.</param>
    /// <returns>The estimated price, or -9999 in case price can't be estimated</returns>
    private TOSSResult performTOSS(MKMMetaCard article, ref string logMessage)
    {
      TOSSResult res;
      res.EstimatedPrice = -9999;
      res.NumberOfItemsInSequence = 0;
      res.UsedWorldWideSearch = false;
      res.ForceLog = false;
      string articlePrice = article.GetAttribute(MCAttribute.MKMPrice);
      List<double> prices = new List<double>();
      int lastMatch = -1;
      XmlNodeList similarItems = getSimilarItems(article, MainView.Instance.Config.MaxArticlesFetched);
      if (similarItems == null)
        return res;
      TraverseResult traverseRes = traverseSimilarItems(similarItems, article, res.UsedWorldWideSearch, ref lastMatch, prices);
      if (settings.SearchWorldwide &&
          (traverseRes == TraverseResult.NotEnoughSimilars // if there isn't enough similar items being sold in seller's country, check other countries as well
          || (settings.CondAcceptance == AcceptedCondition.SomeMatchesAbove && lastMatch + 1 < settings.PriceMinSimilarItems)))
      // at least one matching item above non-matching is required -> if there wasn't, the last match might have been before min. # of items
      {
        res.UsedWorldWideSearch = true;
        prices.Clear();
        lastMatch = -1;
        traverseRes = traverseSimilarItems(similarItems, article, res.UsedWorldWideSearch, ref lastMatch, prices);
      }
      res.NumberOfItemsInSequence = lastMatch + 1;
      double priceFactor = res.UsedWorldWideSearch ? settings.PriceFactorWorldwide : settings.PriceFactor;
      if (traverseRes == TraverseResult.Culled)
      {
        if (settings.LogLessThanMinimum)
        {
          logMessage += "Current Price: " + articlePrice + ", unchanged, only " +
              res.NumberOfItemsInSequence + " similar items found (some outliers culled)" +
              (res.UsedWorldWideSearch ? " - worldwide search! " : ". ");
          res.ForceLog = true;
        }
      }
      else if (traverseRes == TraverseResult.HighVariance)
      {
        if (settings.LogHighPriceVariance) // this signifies that prices were not updated due to too high variance
        {
          logMessage += "NOT UPDATED - variance among cheapest similar items too high" +
              (res.UsedWorldWideSearch ? " - worldwide search! " : ". ");
          res.ForceLog = true;
        }
      }
      else if (traverseRes == TraverseResult.NotEnoughSimilars)
      {
        if (settings.LogLessThanMinimum)
        {
          logMessage += "Current Price: " + articlePrice + ", unchanged, only " +
              res.NumberOfItemsInSequence + " similar items found" +
              (res.UsedWorldWideSearch ? " - worldwide search! " : ". ");
          res.ForceLog = true;
        }
      }
      else if (settings.CondAcceptance == AcceptedCondition.SomeMatchesAbove 
        && res.NumberOfItemsInSequence < settings.PriceMinSimilarItems)
      // at least one matching item above non-matching is required -> if there wasn't, the last match might have been before min. # of items
      {
        if (settings.LogLessThanMinimum)
        {
          logMessage += "Current Price: " + articlePrice + ", unchanged, only " +
              res.NumberOfItemsInSequence + " similar items with a condition-matching item above them found" +
              (res.UsedWorldWideSearch ? " - worldwide search! " : ". ");
          res.ForceLog = true;
        }
      }
      else
      {
        if (settings.PriceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice && traverseRes == TraverseResult.SequenceFound)
        {
          res.EstimatedPrice = prices[0] * priceFactor;
        }
        else
        {
          // if any condition is allowed, use the whole sequence
          // if only matching is allowed, use whole sequence as well because it is only matching items
          if (settings.CondAcceptance != AcceptedCondition.SomeMatchesAbove)
          {
            lastMatch = prices.Count - 1;
            res.NumberOfItemsInSequence = lastMatch + 1;
          }
          if (settings.PriceSetPriceBy == PriceSetMethod.ByPercentageOfHighestPrice)
            res.EstimatedPrice = prices[lastMatch] * priceFactor;
          else // estimation by average
          {
            res.EstimatedPrice = 0;
            for (int i = 0; i < res.NumberOfItemsInSequence; i++)
              res.EstimatedPrice += prices[i];
            res.EstimatedPrice /= res.NumberOfItemsInSequence;
            // linear interpolation between average (currently stored in priceEstimation) and highest price in the sequence
            if (priceFactor > 0.5)
              res.EstimatedPrice += (prices[lastMatch] - res.EstimatedPrice) * (priceFactor - 0.5) * 2;
            else if (priceFactor < 0.5) // linear interpolation between lowest price and average
              res.EstimatedPrice = prices[0] + (res.EstimatedPrice - prices[0]) * (priceFactor) * 2;
          }
        }
      }
      return res;
    }

    private string getTimestamp(DateTime now)
    {
      return now.ToString("dd.MM.yyyy HH:mm:ss");
    }

    public static string GetBuys(string iType)
    {
      int iPage = 1;

      string sFilename = ".\\mcmbuys_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + ".csv";

      using (StreamWriter file = new StreamWriter(sFilename))
      {
        /*
            bought or 1
            paid or 2
            sent or 4
            received or 8
            lost or 32
            canceled or 128
        */

        int count;
        do
        {
          string sUrl = "https://api.cardmarket.com/ws/v1.1/output.xml/orders/2/" + iType + "/" + iPage;
          XmlDocument doc = null;
          try
          {
            doc = MKMInteract.RequestHelper.MakeRequest(sUrl, "GET");
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
          string separ = "\"" + MainView.Instance.Config.CSVExportSeparator + "\"";
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
                file.WriteLine("\"" + oID + separ + sOdate + separ + name
                    + separ + expansion
                    + separ + article["language"]["languageName"].InnerText
                    + separ + article["price"].InnerText + "\"");
                // note: this article contains price in only one currency, likely the one used to make the purchase
                // so do not use MKMHelpers.GetPriceFromXml. Also, for accounts that switched between currencies,
                // there will be just one price column but it will have prices in different currencies...
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
