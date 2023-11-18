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
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace MKMTool
{
  public static class MKMHelpers
  {
    // My userId (to disregard items listed by myself when setting a new price)
    public static string SMyId = "0";
    public static string SMyCurrencyId = "1";
    public static bool   SAmCommercial = false; // true if my account is professional or powerseller

    private static readonly object errorLogLock = new object();

    // key = language ID, value = language name
    public static Dictionary<string, string> LanguagesNames = new Dictionary<string, string>
        {
            { "", "All" }, { "1", "English" }, { "2", "French" }, { "3", "German" },
            { "4", "Spanish" }, { "5", "Italian" }, { "6", "Simplified Chinese" },
            { "7", "Japanese" }, { "8", "Portuguese" }, { "9", "Russian" },
            { "10", "Korean" }, { "11", "Traditional Chinese" }
        };

    // key = language name, value = language ID
    public static Dictionary<string, string> LanguagesIds = new Dictionary<string, string>
        {
            { "All", "" }, { "English", "1" }, { "French", "2" }, { "German", "3" },
            { "Spanish", "4" }, { "Italian", "5" }, { "Simplified Chinese", "6" },
            { "Japanese", "7" }, { "Portuguese", "8" }, { "Russian", "9" },
            { "Korean", "10" }, { "Traditional Chinese", "11" }
        };

    // only countries currently supported by MKM are uncommented. 
    // sorted by alphabet by country name manually - when adding a country, put it at the right spot
    /*    public static Dictionary<string, string> CountryCodes = new Dictionary<string, string>
        {
            //{ "Afghanistan", "AF" },   { "Albania", "AL" },{ "Algeria", "DZ" }, 
            //{ "U.A.E.", "AE" },   { "Argentina", "AR" },    { "Armenia", "AM" }, { "Australia", "AU" },
            { "Austria", "AT" },
            //{ "Azerbaijan", "AZ" },
            { "Belgium", "BE" }, { "Bulgaria", "BG" },
            // { "Bangladesh", "BD" }, { "Bahrain", "BH" },  { "Bosnia and Herzegovina", "BA" }, { "Belarus", "BY" },
            // { "Belize", "BZ" }, { "Bolivia", "BO" },  { "Brazil", "BR" }, { "Brunei Darussalam", "BN" }, 
            { "Canada", "CA" }, { "Croatia", "HR" }, { "Cyprus", "CY" },{ "Czech Republic", "CZ" },
            //{ "Chile", "CL" }, { "People's Republic of China", "CN" }, { "Colombia", "CO" }, { "Costa Rica", "CR" }, 
            { "Denmark", "DK" }, { "Estonia", "EE" },
            //{ "Dominican Republic", "DO" }, { "Ecuador", "EC" }, { "Egypt", "EG" }, { "Ethiopia", "ET" }, { "Faroe Islands", "FO" }, 
            { "Finland", "FI" }, { "France", "FR" },
            { "Germany", "D" },  { "Greece", "GR" },   // MKM's exception, ISO code for Germany is DE
            { "Hungary", "HU" }, 
            //{ "Hong Kong S.A.R.", "HK" }, { "Honduras", "HN" }, 
            //{ "Georgia", "GE" }, { "Greenland", "GL" }, { "Guatemala", "GT" },            
            { "Iceland", "IS" }, { "Ireland", "IE" }, { "Italy", "IT" }, 
            //{ "Indonesia", "ID" }, { "India", "IN" }, { "Iran", "IR" }, { "Iraq", "IQ" }, { "Israel", "IL" },             
            //{ "Jamaica", "JM" }, { "Jordan", "JO" }, 
            { "Japan", "JP" },
            { "Latvia", "LV" }, { "Liechtenstein", "LI" }, { "Lithuania", "LT" }, { "Luxembourg", "LU" },
            //{ "Kazakhstan", "KZ" },  { "Kenya", "KE" },  { "Kyrgyzstan", "KG" },  { "Cambodia", "KH" }, { "Korea", "KR" }, 
            //{ "Kuwait", "KW" }, { "Lao P.D.R.", "LA" }, { "Lebanon", "LB" }, { "Libya", "LY" },             
            //{ "Macao S.A.R.", "MO" }, { "Morocco", "MA" }, { "Principality of Monaco", "MC" }, { "Maldives", "MV" }, { "Mexico", "MX" }, 
            //{ "Macedonia (FYROM)", "MK" }, 
            { "Malta", "MT" }, { "Netherlands", "NL" }, { "Norway", "NO" }, 
            //{ "Montenegro", "ME" }, { "Mongolia", "MN" }, { "Malaysia", "MY" }, { "Nigeria", "NG" }, { "Nicaragua", "NI" },             
            //{ "Nepal", "NP" }, { "New Zealand", "NZ" }, { "Oman", "OM" }, { "Islamic Republic of Pakistan", "PK" }, { "Panama", "PA" }, 
            //{ "Peru", "PE" }, { "Republic of the Philippines", "PH" }, 
            { "Poland", "PL" },  { "Portugal", "PT" }, { "Romania", "RO" }, 
            //{ "Puerto Rico", "PR" }, { "Paraguay", "PY" }, { "Qatar", "QA" },             
            //{ "Russia", "RU" }, { "Rwanda", "RW" }, { "Saudi Arabia", "SA" },  { "Senegal", "SN" }, 
            //{ "El Salvador", "SV" }, { "Serbia", "RS" },{ "Sri Lanka", "LK" }, 
            { "Singapore", "SG" }, { "Slovakia", "SK" }, { "Slovenia", "SI" },{ "Spain", "ES" }, 
            { "Sweden", "SE" },{ "Switzerland", "CH" }, { "United Kingdom", "GB" },
            //{ "Syria", "SY" }, { "Tajikistan", "TJ" }, { "Thailand", "TH" }, { "Turkmenistan", "TM" }, { "Trinidad and Tobago", "TT" },
            //{ "Tunisia", "TN" }, { "Turkey", "TR" }, { "Taiwan", "TW" }, { "Ukraine", "UA" }, { "Uruguay", "UY" },
            //{ "United States of America", "US" },  { "Uzbekistan", "UZ" }, { "Bolivarian Republic of Venezuela", "VE" },
            //{ "Vietnam", "VN" }, { "Yemen", "YE" },  { "South Africa", "ZA" }, { "Zimbabwe", "ZW" } 
        };*/
    public static Dictionary<string, string> CountryNames = new Dictionary<string, string>
        {
            { "AT", "Austria"  },
            { "BE", "Belgium" }, { "BG", "Bulgaria" },
            { "CA", "Canada"}, { "HR", "Croatia" }, { "CY", "Cyprus" },{ "CZ", "Czech Republic" },
            { "DK", "Denmark" }, { "EE", "Estonia" },
            { "FI", "Finland" }, { "FR", "France" },
            { "D", "Germany" },  { "GR", "Greece"  },   // MKM's exception, ISO code for Germany is DE
            { "HK", "Hong Kong" }, { "HU", "Hungary" },
            { "IS", "Iceland" }, { "IE", "Ireland" }, {"IT", "Italy" },
            { "JP", "Japan" },
            { "LV", "Latvia" }, { "LI", "Liechtenstein" }, {"LT", "Lithuania" }, { "LU", "Luxembourg" },
            { "MT", "Malta" }, {"NL", "Netherlands" }, { "NO", "Norway" },
            { "PL", "Poland"},  {"PT", "Portugal" }, {"RO", "Romania" },
            { "SG", "Singapore"}, { "SK", "Slovakia" }, {"SI", "Slovenia" },{"ES", "Spain" },
            { "SE", "Sweden" }, {"CH", "Switzerland" }, { "GB", "United Kingdom" }
        };

    public struct GameDesc
    {
      public GameDesc(string gameId, string categoryId)
      {
        GameID = gameId;
        SinglesCategoryID = categoryId;
      }
      public string GameID { get; set; }// as obtained from MKMInteract.ReadGames()
      public string SinglesCategoryID { get; set; }// id for the singles of this game in the Inventory database
    }

    // as obtained from MKMInteract.ReadGames(), key == name
    public static Dictionary<string, GameDesc> GameIDsByName = new Dictionary<string, GameDesc>
    {
        { "Magic the Gathering", new GameDesc("1", "1") }, { "World of Warcraft TCG", new GameDesc("2", "3") },
        { "Yugioh", new GameDesc("3", "5") }, { "The Spoils", new GameDesc("5", "22") },
        { "Pokémon", new GameDesc("6", "51") }, {"Force of Will",new GameDesc("7", "1018") },
        { "Cardfight!! Vanguard", new GameDesc("8", "1019") }, { "Final Fantasy", new GameDesc("9", "1022") },
        { "Weiß Schwarz",new GameDesc("10", "1040") }, { "Dragoborne", new GameDesc("11", "1039") },
        { "My Little Pony", new GameDesc("12", "1041") }, {"Dragon Ball Super", new GameDesc("13", "1049") },
        { "Star Wars: Destiny", new GameDesc("15", "1072") }, { "Flesh and Blood", new GameDesc("16", "1601") },
        { "Digimon", new GameDesc("17", "1611") }
    };

    public const int MaxNbItemsPerRequest = 100; // limit by MKM

    public struct PriceGuideDesc
    {
      public PriceGuideDesc(string code, string doc)
      {
        Code = code;
        Documentation = doc;
      }
      public string Code { get; }
      public string Documentation { get; }
    }

    // key == name MKM uses as column name in the PriceGuides request
    public static Dictionary<string, PriceGuideDesc> PriceGuides = new Dictionary<string, PriceGuideDesc>
    {
      {"Avg. Sell Price", new PriceGuideDesc("SELL",
        "The average sell price as shown in the chart at the website for non-foils") },
      {"Low Price", new PriceGuideDesc("LOW",
        "The lowest price at the market for non-foils") },
      {"Trend Price", new PriceGuideDesc("TREND",
        "The trend price as shown at the website (and in the chart) for non-foils") },
      {"German Pro Low", new PriceGuideDesc("LOWGERMAN",
        "The lowest sell price from German professional sellers") },
      {"Suggested Price", new PriceGuideDesc("SUGGESTED",
        "A suggested sell price for professional users, determined by an internal algorithm; this algorithm will not be made public") },
      {"Foil Sell", new PriceGuideDesc("SELLFOIL",
        "The average sell price as shown in the chart at the website for foils") },
      {"Foil Low", new PriceGuideDesc("LOWFOIL",
        "The lowest price at the market as shown at the website(for condition EX+) for foils") },
      {"Foil Trend", new PriceGuideDesc("TRENDFOIL",
        "The trend price as shown at the website(and in the chart) for foils") },
      {"Low Price Ex+", new PriceGuideDesc("LOWEX",
        "The lowest price at the market for non-foils with condition EX or better") },
      {"AVG1", new PriceGuideDesc("AVG1",
        "The average sale price over the last day") },
      {"AVG7", new PriceGuideDesc("AVG7",
        "The average sale price over the last 7 days") },
      {"AVG30", new PriceGuideDesc("AVG30",
        "The average sale price over the last 30 days") },
      {"Foil AVG1", new PriceGuideDesc("AVG1FOIL",
        "The average sale price over the last day for foils") },
      {"Foil AVG7", new PriceGuideDesc("AVG7FOIL",
        "The average sale price over the last 7 days for foils") },
      {"Foil AVG30", new PriceGuideDesc("AVG30FOIL",
        "The average sale price over the last 30 days for foils") },
      {"Current", new PriceGuideDesc("CURRENT",
        "Not really a price guide, this is your current price of the card") }
    };

    /// A three-state boolean allowing "any" as the third state.
    public enum Bool3 { False = 0, True = 1, Any = 2 }

    /// Parses the given string as a 3-state bool value.
    /// <param name="val">The value. Will be transformed to lowercase.</param>
    /// <returns>True if the value of the string is "true", False if it is "false" and Any if it is anything else.</returns>
    public static Bool3 ParseBool3(string val)
    {
      val = val.ToLower();
      switch (val)
      {
        case "true":
          return Bool3.True;
        case "false":
          return Bool3.False;
        default:
          return Bool3.Any;
      }
    }

    /// Converts a specified Bool3 value to string.
    /// <param name="val">The value.</param>
    /// <returns>"false" for False, "true" for True, "" for Any.</returns>
    public static string Bool3ToString(Bool3 val)
    {
      switch (val)
      {
        case Bool3.False:
          return "false";
        case Bool3.True:
          return "true";
        default:
          return "";
      }
    }

    /// Converts the condition from string to int so that it can be numerically compared.
    /// <param name="cond">The condition as two letter code.</param>
    /// <returns>5 for MT or NM, 4 for EX, 3 for GD, 2 for LP, 1 for PL, 0 for PO.</returns>
    private static int convertCondition(String cond)
    {
      if (cond == "EX")
        return 4;
      else if (cond == "GD")
        return 3;
      else if (cond == "LP")
        return 2;
      else if (cond == "PL")
        return 1;
      else if (cond == "PO")
        return 0;
      return 5;
    }

    /// Determines whether the specified condition is better than the reference condition.
    /// Mint and Near Mint are considered to be the same.
    /// According to API: (MT for Mint > NM for Near Mint > EX for Excellent > GD for Good > LP for Light Played > PL for Played > PO for Poor) 
    /// <param name="itemCond">Card condition.</param>
    /// <param name="reference">The reference condition.</param>
    /// <returns>
    ///   <c>true</c> if <c>itemCond</c> is in better or same conditions than the <c>reference</c>; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBetterOrSameCondition(string itemCond, string reference)
    {
      return convertCondition(itemCond) >= convertCondition(reference);
    }

    /// Tries to convert a double first by InvariantCulture (. as separator), if it fails,
    /// tries current culture.
    /// <param name="toConvert">String to convert to double.</param>
    /// <returns>Converted number or NaN if conversion failed.</returns>
    public static double ConvertDoubleAnySep(string toConvert)
    {
      if (double.TryParse(toConvert, NumberStyles.Float, CultureInfo.InvariantCulture, out double resInv))
        return resInv;
      if (double.TryParse(toConvert, NumberStyles.Float, CultureInfo.CurrentCulture, out double resCur))
        return resCur;
      return double.NaN;
    }

    /// From a xml response object with prices extracts the price in the currency of our account.
    /// <param name="prices">Node that is expected to include one or more "prices" node.
    /// This is e.g. the Article response nodes.</param>
    /// <returns>Price in the sMyCurrencyId currency. -1 in case the price is not found (invalid object, logged as error).</returns>
    public static float GetPriceFromXml(XmlNode prices)
    {
      foreach (XmlNode child in prices.SelectNodes("prices"))
      {
        if (child["idCurrency"].InnerText == SMyCurrencyId)
        {
          return Convert.ToSingle(child["price"].InnerText, System.Globalization.CultureInfo.InvariantCulture);
        }
      }
      LogError("getting price", "price not found in the specified xml object", false, prices.InnerXml);
      return -1;
    }

    public static string PrettyXml(string xml)
    {
      var stringBuilder = new StringBuilder();

      var element = XElement.Parse(xml);

      var settings = new XmlWriterSettings
      {
        OmitXmlDeclaration = true,
        Indent = true,
        NewLineOnAttributes = true
      };

      using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
      {
        element.Save(xmlWriter);
      }

      return stringBuilder.ToString();
    }

    public static byte[] GzDecompress(byte[] gzip)
    {
      // Create a GZIP stream with decompression mode.
      // ... Then create a buffer and write into while reading from the GZIP stream.
      using (var stream = new GZipStream(new MemoryStream(gzip),
          CompressionMode.Decompress))
      {
        const int size = 4096;
        var buffer = new byte[size];
        using (var memory = new MemoryStream())
        {
          var count = 0;
          do
          {
            count = stream.Read(buffer, 0, size);
            if (count > 0)
              memory.Write(buffer, 0, count);
          } while (count > 0);
          return memory.ToArray();
        }
      }
    }

    /// Method for unified logging of exceptions and errors. Writes the error in the application's console, the error log file and
    /// if requested, shows a pop-up window with the error message.
    /// The error message will be "Error with 'subject': 'errorMessage' @ URL: 'sURL'" (URL part is optional and only logged in file).
    /// <param name="subject">The variable/item/process that caused the error. The error message will start "Error with 'subject': ...".</param>
    /// <param name="errorMessage">The error message to log.</param>
    /// <param name="popup">If set to <c>true</c>, a pop-up window with the message will be showed. In general, use this for errors
    /// that interrupt the current action completely.</param>
    /// <param name="sURL">When relevant (= in case of exceptions invoked by an API request), include the URL that triggered it. Leave empty for other errors.
    /// Since the URL can be very long, it is never outputted in the console window, only in the file.</param>
    public static void LogError(string subject, string errorMessage, bool popup, string sURL = "")
    {
      lock (errorLogLock)
      {
        // if this the first error of this run, write a header with current date and time in the error log file to know which errors are old and which new
        // monitoring when (if) first error happens helps limit the size of the log in runs when no error happens
        // TODO - maybe clean the log once in a while?
        if (firstError)
        {
          System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
          System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
          using (var sw = File.AppendText(@".\\error_log.txt"))
          {
            sw.WriteLine(DateTime.Now.ToString() + ", version: " + fileVersionInfo.ProductVersion);
            firstError = false;
          }
        }
        string msg = "Error with " + subject + ": " + errorMessage;
        using (var sw = File.AppendText(@".\\error_log.txt"))
        {
          if (sURL.Length > 0)
            sw.WriteLine(msg + " @ " + sURL);
          else
            sw.WriteLine(msg);
        }
        MainView.Instance.LogMainWindow(msg);

        if (popup)
          MessageBox.Show(msg, "MKMTool encountered error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    public class ComboboxItem
    {
      public string Text { get; set; }
      public object Value { get; set; }

      public override string ToString()
      {
        return Text;
      }
    }

    private static bool firstError = true; // for logging errors, see LogError()
  }
}
