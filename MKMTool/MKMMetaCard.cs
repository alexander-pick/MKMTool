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

using System.Data;
using System.Collections.Generic;
using System.Xml;
using static MKMTool.MKMHelpers;

namespace MKMTool
{

    /// <summary>
    /// Different formats that can be used to export the meta card.
    /// </summary>
    public enum MCFormat { MKM, Deckbox}

    /// <summary>
    /// A "string enum" of all attributes MKMMetaCard uses for comparisons, exports etc.
    /// If you are extending functionality of MKMMetaCard by working with some new attribute, don't forget to add it here.
    /// </summary>
    public class MCAttribute
    {
        /// <summary>
        /// The English name of the card.
        /// </summary>
        public static string Name { get { return "Name"; } }

        /// <summary>
        /// The name of the card in the language it is printed in.
        /// </summary>
        public static string LocName { get { return "LocName"; } }

        /// <summary>
        /// The name of the expansion in English as used by MKM's API, i.e. with first letters capitalized.
        /// </summary>
        public static string Expansion { get { return "Expansion"; } }

        /// <summary>
        /// The MKM's expansion ID.
        /// </summary>
        public static string ExpansionID { get { return "Expansion ID"; } }

        /// <summary>
        /// The MKM's expansion abbreviation.
        /// </summary>
        public static string ExpansionAbb { get { return "abbreviation"; } }

        /// <summary>
        /// English name of the language, first letters capital, i.e. one of the following strings:
        /// (English;French; German; Spanish; Italian; Simplified Chinese; Japanese; Portuguese; Russian; Korean; Traditional Chinese) 
        /// </summary>
        public static string Language { get { return "Language"; } }

        /// <summary>
        /// MKM's ID of the language (based on MKMHelpers.languagesNames)
        /// </summary>
        public static string LanguageID { get { return "LanguageID"; } }

        /// <summary>
        /// Condition of the card as a two-letter abbreviation used by MKM:
        /// MT for Mint, NM for Near Mint, EX for Excellent, GD for Good, LP for Light Played, PL for Played, PO for Poor.
        /// </summary>
        public static string Condition { get { return "Condition"; } }
        
        /// <summary>
        /// The worst condition this card can be in, see Condition for possible values.
        /// </summary>
        public static string MinCondition { get { return "MinCondition"; } }

        /// <summary>
        /// The string "true" (lowercase) if the card is foil, "false" if it isn't and an empty string if either is accepted.
        /// </summary>
        public static string Foil { get { return "Foil"; } }

        /// <summary>
        /// The string "true" (lowercase) if the card is signed, "false" if it isn't and an empty string if either is accepted.
        /// </summary>
        public static string Signed { get { return "Signed"; } }

        /// <summary>
        /// The string "true" (lowercase) if the card has altered art, "false" if it hasn't and an empty string if either is accepted.
        /// </summary>
        public static string Altered { get { return "Altered"; } }

        /// <summary>
        /// The string "true" (lowercase) if the card is treated as a playset, "false" if it isn't and an empty string if either is accepted.
        /// </summary>
        public static string Playset { get { return "Playset"; } }

        /// <summary>
        /// The string "true" (lowercase) if the card is treated as first edition, "false" if it isn't and an empty string if either is accepted.
        /// Only relevant for Yu-Gi-Oh
        /// </summary>
        public static string FirstEd { get { return "FirstEd"; } }

        /// <summary>
        /// The MKM's product ID.
        /// </summary>
        public static string ProductID { get { return "idProduct"; } }

        /// <summary>
        /// The MKM's article ID.
        /// </summary>
        public static string ArticleID { get { return "idArticle"; } }

        /// <summary>
        /// The MKM's metaproduct ID.
        /// </summary>
        public static string MetaproductID { get { return "idMetaproduct"; } }

        /// <summary>
        /// An integer number indicating number of the given card in stock.
        /// </summary>
        public static string Count { get { return "Count"; } }

        /// <summary>
        /// Rarity as a word with capitalized letters:
        /// Masterpiece, Mythic, Rare, Special, Time Shifted, Uncommon, Common, Land, Token, Arena Code Card, Tip Card.
        /// </summary>
        public static string Rarity { get { return "Rarity"; } }

        /// <summary>
        /// The minimum price the user wants for this card.
        /// </summary>
        public static string MinPrice { get { return "MinPrice"; } }

        /// <summary>
        /// Price generated for this card by MKMTool (relevant only for exact card article).
        /// </summary>
        public static string MKMToolPrice { get { return "MKMTool Price"; } }

        /// <summary>
        /// Price of the cheapest card with the same attributes as this card.
        /// </summary>
        public static string PriceCheapestSimilar { get { return "Price - Cheapest Similar"; } }

        /// <summary>
        /// The price of the card on MKM (relevant only for exact card article).
        /// </summary>
        public static string MKMPrice { get { return "Price"; } }

        /// <summary>
        /// The string EUR or GBP.
        /// </summary>
        public static string MKMCurrencyCode { get { return "Currency Code"; } }

        /// <summary>
        /// The currency id, 1 for EUR, 2 for GBP.
        /// </summary>
        public static string MKMCurrencyId { get { return "idCurrency"; } }

        /// <summary>
        /// For MKM articles, this is what is inside the "comments" field.
        /// </summary>
        public static string Comments { get { return "Comments"; } }

        /// <summary>
        /// Number of product within the expansion.
        /// </summary>
        public static string CardNumber { get { return "Card Number"; } }

        /// <summary>
        /// True or false whether the card is currently in some shopping cart on MKM.
        /// </summary>
        public static string IsInCart { get { return "inShoppingCart"; } }

        /// <summary>
        /// When was the card last edited on MKM.
        /// </summary>
        public static string LastEdited { get { return "lastEdited"; } }

        /// <summary>
        /// The MKM's price guide. Average price of articles ever sold of this product.
        /// </summary>
        public static string PriceGuideSELL { get { return "SELL - MKM Price Guide"; } }

        /// <summary>
        /// The MKM's price guide. Current lowest non-foil price (all conditions).
        /// </summary>
        public static string PriceGuideLOW { get { return "LOW - MKM Price Guide"; } }

        /// <summary>
        /// The MKM's price guide. Current lowest non-foil price (condition EX and better).
        /// </summary>
        public static string PriceGuideLOWEX { get { return "LOWEX - MKM Price Guide"; } }

        /// <summary>
        /// The MKM's price guide. Current lowest foil price.
        /// </summary>
        public static string PriceGuideLOWFOIL { get { return "LOWFOIL - MKM Price Guide"; } }

        /// <summary>
        /// The MKM's price guide. Current average non-foil price of all available articles of this product.
        /// </summary>
        public static string PriceGuideAVG { get { return "AVG - MKM Price Guide"; } }

        /// <summary>
        /// The MKM's price guide. Current trend price.
        /// </summary>
        public static string PriceGuideTREND { get { return "TREND - MKM Price Guide"; } }

        /// <summary>
        /// The MKM's price guide. Current foil trend price.
        /// </summary>
        public static string PriceGuideTRENDFOIL { get { return "TRENDFOIL - MKM Price Guide"; } }
    }

    /// <summary>
    /// Class to represent individual cards, encapsulating all possible attributes and descriptions of it.
    /// Each instance of this class represents a card as a metaproduct, i.e. it can represent multiple cards that share the filled in attributes:
    /// for example all cards from a given set in; or all cards with a given name from a given set in any language etc.
    /// The aim is to have a very robust representation that allows filling only partial information about the card and the rest can be either
    /// assumed to have some specified default value or be of any value.
    /// The class also provides methods for conversion of different formats of attribute names, construction based on different input formats
    /// and an operator for comparison of two metacards
    /// Everything is stored as strings as most often it will be used for comparisons to strings returned by the MKM API so it is usually not needed to parse the
    /// numerical values when applicable.
    /// DO NOT expose the "data" variable as public in any way - all modifications need to be done within the class as different attributes
    /// can be re-named internally. For example, "Name" and "enName" are both acceptable names for the same attribute, but it will be stored only once in the class.
    /// </summary>
    public class MKMMetaCard
    {
        // If a recognized attribute has a synonym, add it to this dictionary, key == synonym, value == the recognized attribute to which this synonym maps.
        private static Dictionary<string, string> synonyms = new Dictionary<string, string>
        {
            { "enName", MCAttribute.Name }, { "Edition", MCAttribute.Expansion }, { "Altered Art", MCAttribute.Altered },
            // the following 10 are used by GET STOCK FILE
            { "Foil?", MCAttribute.Foil }, { "Signed?", MCAttribute.Signed }, { "Playset?", MCAttribute.Playset },
            { "Altered?", MCAttribute.Altered }, { "FirstEd?", MCAttribute.FirstEd }, { "English Name", MCAttribute.Name },
            { "Exp. Name", MCAttribute.Expansion }, { "Amount", MCAttribute.Count }, { "Exp.", MCAttribute.ExpansionAbb },
            { "Local Name", MCAttribute.LocName }
        };

        // literal dictionary of conditions - translates any supported condition denomination to its equivalent in the two-letter format MKM uses
        // all capitals! transform your string toUpper before checking against this dictionary
        private static Dictionary<string, string> conditionsDictionary = new Dictionary<string, string>
        {
            { "MT", "MT" }, { "NM", "NM" }, { "EX", "EX" }, { "GD", "GD" }, { "LP", "LP" }, { "PL", "PL" }, { "PO", "PO" },
            { "MINT", "MT" }, { "NEAR MINT", "NM" }, { "EXCELLENT", "EX" }, { "GOOD", "GD" }, 
            { "LIGHTLY PLAYED", "LP" }, { "POOR", "PO" }, { "GOOD (LIGHTLY PLAYED)", "EX" },
            // for the following the European (MKM) and American (deckbox.org) grades are the same word but different meaning
            // we will assume that if it is the entire word, it is exported from deckbox.org and therefore uses the American grading
            { "PLAYED", "GD" }, 
            { "HEAVILY PLAYED", "LP" }// this could be either LP or PL, we will never know...let's set it to LP
        };
        // key == any recognized condition in upper case, value == case sensitive deckbox format
        private static Dictionary<string, string> conditionsDeckboxDictionary = new Dictionary<string, string>
        {
            { "MT", "Mint" }, { "NM", "Near Mint" }, { "EX", "Good (Lightly Played)" }, { "GD", "Played" },
            { "LP", "Heavily Played" }, { "PL", "Heavily Played" }, { "PO", "Poor" },
            { "MINT", "Mint" }, { "NEAR MINT", "Near Mint" }, { "EXCELLENT", "Good (Lightly Played)" }, { "GOOD", "Played" },
            { "LIGHTLY PLAYED", "Heavily Played" }, { "POOR", "Poor" }, { "GOOD (LIGHTLY PLAYED)", "Good (Lightly Played)" },
            { "PLAYED", "Played" }, { "HEAVILY PLAYED", "Heavily Played" }// this could be either LP or PL, we will never know...let's set it to LP
        };

        /// <summary>
        /// Used to set-up the dictionary of recognized attributes. If you are extending the class to internally handle some additional attribute,
        /// make sure to include it in this method and in the MCAttribute class above.
        /// </summary>
        static MKMMetaCard()
        {
        }

        private Dictionary<string, string> data = new Dictionary<string, string>();

        private bool hasPriceGuides = false; // is set to true 

        /// <summary>
        /// Gets a value indicating whether this instance has recent MKM's price guides,
        /// i.e. if a product entry with price guides is used to fill this card, see FillProductInfo.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has price guides; otherwise, <c>false</c>.
        /// </value>
        public bool HasPriceGuides
        {
            get { return hasPriceGuides; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MKMMetaCard"/> class based on a database entry.
        /// It will use the local database to fill in name, expansion and expansion ID if the input contains product id
        /// and vice versa will get the product id if the input contains name and expansion/expansion ID.
        /// </summary>
        /// <param name="card">The card.</param>
        public MKMMetaCard(DataRow card)
        {
            for (int i = 0; i < card.Table.Columns.Count; i++)
            {
                string columnVal = card[i].ToString();
                if (columnVal != "") // having an attribute at "any" is the same as not having it at all - no need to process it
                {
                    // if this is a synonym, store the original input value under the synonym, 
                    // but continue the rest of the processing with the "main" name
                    string attName;
                    if (synonyms.TryGetValue(card.Table.Columns[i].ColumnName, out attName))
                        data[card.Table.Columns[i].ColumnName] = columnVal;
                    else
                        attName = card.Table.Columns[i].ColumnName;
                    // for attributes for which we have custom setters, use them to perform all kinds of checks
                    if (attName == MCAttribute.Condition)
                        SetCondition(columnVal);
                    else if (attName == MCAttribute.MinCondition)
                        SetMinCondition(columnVal);
                    else if (attName == MCAttribute.Language)
                        SetLanguage(columnVal);
                    else if (attName == MCAttribute.LanguageID)
                        SetLanguageID(columnVal);
                    else if (attName == MCAttribute.Foil || attName == MCAttribute.Signed
                        || attName == MCAttribute.Altered || attName == MCAttribute.Playset || attName == MCAttribute.FirstEd)
                        SetBoolAttribute(attName, columnVal);
                    // all other attributes
                    else
                        data[attName] = columnVal;
                }
            }

            // fill in missing data that we know from our local database (and have them be "any" would make no sense)
            string productId = GetAttribute(MCAttribute.ProductID);
            if (productId != "") // if we know product ID, we can use the inventory database to get expansion ID and Name
            {
                DataRow row = MKMDbManager.Instance.GetSingleCard(productId);
                if (row != null)
                {
                    data[MCAttribute.ExpansionID] = row[MKMDbManager.InventoryFields.ExpansionID].ToString();
                    data[MCAttribute.Name] = row[MKMDbManager.InventoryFields.Name].ToString();
                    data[MCAttribute.MetaproductID] = row[MKMDbManager.InventoryFields.MetaproductID].ToString();
                }
                else
                {
                    data[MCAttribute.ProductID] = "";
                    LogError("creating metacard", "Unrecognized product ID " + productId + ", will be ignored", false);
                }
            }
            // make sure the chosen expansion is valid 
            string expID = GetAttribute(MCAttribute.ExpansionID);
            string expansion = GetAttribute(MCAttribute.Expansion);
            if (expID != "")
            {
                string temp = MKMDbManager.Instance.GetExpansionName(expID);
                if (temp == "")
                {
                    LogError("creating metacard", "Unrecognized expansion ID " + expID + ", will be ignored", false);
                    data[MCAttribute.ExpansionID] = expID = "";                    
                }
                else
                    data[MCAttribute.Expansion] = expansion = temp;
            }
            if (expansion != "" && expID == "") // if expID is set, expansion has already been set too
            {
                string temp = MKMDbManager.Instance.GetExpansionID(expansion);
                if (temp == "")
                {
                    LogError("creating metacard", "Unrecognized expansion " + expansion + ", will be ignored", false);
                    data[MCAttribute.Expansion] = expID = "";
                }
                else
                    data[MCAttribute.ExpansionID] = expID = temp;
            }
            // if we don't know product ID, but we know expansion and name, we can get it
            if (productId == "" && expID != "")
            {
                string name = GetAttribute(MCAttribute.Name);
                if (name != "")
                {
                    string[] ids = MKMDbManager.Instance.GetProductID(name, expID);
                    if (ids.Length == 1)
                        data[MCAttribute.ProductID] = ids[0];
                    // else we are unable to determine the product ID
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MKMMetaCard"/> class based on a MKM API's "article".
        /// Only attributes that are among the "recognized" ones (have an entry in MCAttribute) will be stored.
        /// </summary>
        /// <param name="MKMArticle">A XML node with the article.</param>
        public MKMMetaCard(XmlNode MKMArticle)
        {
            /* From MKM API documentation as of 29.6.2019:
            article:
            {
                idArticle:                          // Article ID
                idProduct:                          // Product ID
                language:
                {                         // Language entity
                    idLanguage:                     // Language ID
                    languageName:                   // Language's name in English
                }
                comments:                           // Comments
                price:                              // Price of the article
                count:                              // Count (see notes)
NOT STORED      inShoppingCart:                     // Flag, if that article is currently in a shopping cart
                product: {                          // Short Product entity
                    enName:                         // English name
                    locName:                        // Localized name (according to the language of the article)
NOT STORED          image:                          // path to the product's image
                    expansion:                      // expansion name in English, if applicable
                    nr:                             // Number of single within the expansion, if applicable
NOT STORED          expIcon:                        // index to the expansion's icon, if applicable
                    rarity:                         // product's rarity if applicable
                }
NOT STORED      seller: { }                          // Seller's user entity
NOT STORED      lastEdited:                         // Date, the article was last updated
                condition:                          // Product's condition, if applicable
                isFoil:                             // Foil flag, if applicable
                isSigned:                           // Signed flag, if applicable
                isAltered:                          // Altered flag, if applicable
                isPlayset:                          // Playset flag, if applicable
NOT STORED      links: { }                           // HATEOAS links
            }
            */
            data[MCAttribute.ArticleID] = MKMArticle["idArticle"].InnerText;
            data[MCAttribute.ProductID] = MKMArticle["idProduct"].InnerText;
            data[MCAttribute.LanguageID] = MKMArticle["language"]["idLanguage"].InnerText;
            data[MCAttribute.Language] = MKMArticle["language"]["languageName"].InnerText;
            data[MCAttribute.Comments] = MKMArticle["comments"].InnerText;
            data[MCAttribute.MKMPrice] = MKMArticle["price"].InnerText;
            data[MCAttribute.Count] = MKMArticle["count"].InnerText;
            data[MCAttribute.Condition] = MKMArticle["condition"].InnerText;
            data[MCAttribute.IsInCart] = MKMArticle["inShoppingCart"].InnerText;
            data[MCAttribute.LastEdited] = MKMArticle["lastEdited"].InnerText;

            // the flags can be null based on what game this is
            if (MKMArticle["isFoil"] != null)
                data[MCAttribute.Foil] = MKMArticle["isFoil"].InnerText;
            if (MKMArticle["isSigned"] != null)
                data[MCAttribute.Signed] = MKMArticle["isSigned"].InnerText;
            if (MKMArticle["isAltered"] != null)
                data[MCAttribute.Altered] = MKMArticle["isAltered"].InnerText;
            if (MKMArticle["isPlayset"] != null)
                data[MCAttribute.Playset] = MKMArticle["isPlayset"].InnerText;
            if (MKMArticle["isFirstEd"] != null)
                data[MCAttribute.FirstEd] = MKMArticle["isFirstEd"].InnerText;
            if (MKMArticle["product"] != null) // based on which API call was used, this can be null
            {
                data[MCAttribute.Name] = MKMArticle["product"]["enName"].InnerText;
                data[MCAttribute.LocName] = MKMArticle["product"]["locName"].InnerText;
                data[MCAttribute.Expansion] = MKMArticle["product"]["expansion"].InnerText;
                data[MCAttribute.ExpansionID] = MKMDbManager.Instance.GetExpansionID(MKMArticle["product"]["expansion"].InnerText);
                data[MCAttribute.CardNumber] = MKMArticle["product"]["nr"].InnerText;
                data[MCAttribute.Rarity] = MKMArticle["product"]["rarity"].InnerText;
            }
        }

        /// <summary>
        /// Fills the attributes of this metacard based on a provided MKM product entry. All entries set in this
        /// will be used - if previous values existed, they will be overwritten.
        /// Only attributes that are among the "recognized" ones (have an entry in MCAttribute) will be stored.
        /// If the entry has price guides, the HasPriceGuides will be set to true and all price guides will be stored - even
        /// foil ones for non-foil cards and vice versa.
        /// </summary>
        /// <param name="MKMProduct">A XML node with the product entry. Can be either the detailed or non-detailed version.
        /// No validity checks are performed - it is assumed that this is a correct XMLNode obtained from a MKM API call.</param>
        public void FillProductInfo(XmlNode MKMProduct)
        {
            /*From MKM documentation 6.7.2019
             * product: {
                idProduct:                  // Product ID
                idMetaproduct:              // Metaproduct ID
NOT STORED      countReprints:              // Number of total products bundled by the metaproduct
                enName:                     // Product's English name
NOT STORED      localization: {}            // localization entities for the product's name
NOT STORED      category: {                 // Category entity the product belongs to
                    idCategory:             // Category ID
                    categoryName:           // Category's name
                }
NOT STORED      website:                    // URL to the product (relative to MKM's base URL)
NOT STORED      image:                      // Path to the product's image
NOT STORED      gameName:                   // the game's name
NOT STORED      categoryName:               // the category's name
                number:                     // Number of product within the expansion (where applicable)
                rarity:                     // Rarity of product (where applicable)
IT IS NOT THERE IF expansion IS       expansionName:              // Expansion's name 
NOT STORED      links: {}                   // HATEOAS links
                // The following information is only returned for responses that return the detailed product entity 
                    expansion:
                    {                // detailed expansion information (where applicable)
                        idExpansion:
                        enName:
 NOT STORED             expansionIcon:
                }
                    priceGuide:
                    {               // Price guide entity '''(ATTN: not returned for expansion requests)'''
                        SELL:                   // Average price of articles ever sold of this product
                        LOW:                    // Current lowest non-foil price (all conditions)
THE + is not there!     LOWEX+:                 // Current lowest non-foil price (condition EX and better)
                        LOWFOIL:                // Current lowest foil price
                        AVG:                    // Current average non-foil price of all available articles of this product
                        TREND:                  // Current trend price
                        TRENDFOIL:              // Current foil trend price
                }
NOT STORED          reprint: [                  // Reprint entities for each similar product bundled by the metaproduct
                    {
                        idProduct:
                        expansion:
                        expIcon:
                    }
                ]
            }*/
            data[MCAttribute.ProductID] = MKMProduct["idProduct"].InnerText;
            data[MCAttribute.MetaproductID] = MKMProduct["idMetaproduct"].InnerText;
            data[MCAttribute.Name] = MKMProduct["enName"].InnerText;
            data[MCAttribute.CardNumber] = MKMProduct["number"].InnerText;
            data[MCAttribute.Rarity] = MKMProduct["rarity"].InnerText;
            if (MKMProduct["expansionName"] != null)
            {
                data[MCAttribute.ExpansionID] = MKMDbManager.Instance.GetExpansionID(MKMProduct["expansionName"].InnerText);
                data[MCAttribute.Expansion] = MKMProduct["expansionName"].InnerText;
            }
            else
            {
                if (MKMProduct["expansion"] != null)
                    data[MCAttribute.ExpansionID] = MKMProduct["expansion"]["idExpansion"].InnerText;
                else
                    data[MCAttribute.ExpansionID] = MKMDbManager.Instance.GetSingleCard(
                        data[MCAttribute.ProductID])[MKMDbManager.InventoryFields.ExpansionID].ToString();
                data[MCAttribute.Expansion] = MKMDbManager.Instance.GetExpansionName(data[MCAttribute.ExpansionID]);
            }

            if (MKMProduct["priceGuide"] != null)
            {
                hasPriceGuides = true;
                data[MCAttribute.PriceGuideSELL] = MKMProduct["priceGuide"]["SELL"].InnerText;
                data[MCAttribute.PriceGuideLOW] = MKMProduct["priceGuide"]["LOW"].InnerText;
                data[MCAttribute.PriceGuideLOWEX] = MKMProduct["priceGuide"]["LOWEX"].InnerText;
                data[MCAttribute.PriceGuideLOWFOIL] = MKMProduct["priceGuide"]["LOWFOIL"].InnerText;
                data[MCAttribute.PriceGuideAVG] = MKMProduct["priceGuide"]["AVG"].InnerText;
                data[MCAttribute.PriceGuideTREND] = MKMProduct["priceGuide"]["TREND"].InnerText;
                data[MCAttribute.PriceGuideTRENDFOIL] = MKMProduct["priceGuide"]["TRENDFOIL"].InnerText;
            }
        }

        /// <summary>
        /// Performs checks whether the specified language is valid and if so, sets it and also sets the appropriate languageID.
        /// </summary>
        /// <param name="language">The language.
        /// Allowed values (case sensitive): English; French; German; Spanish; Italian; Simplified Chinese; Japanese; Portuguese; Russian; Korean; Traditional Chinese.</param>
        public void SetLanguage(string language)
        {
            string languageID;
            if (languagesIds.TryGetValue(language, out languageID))
            {
                data[MCAttribute.LanguageID] = languageID;
                data[MCAttribute.Language] = language;
            }
            else// if it is an unknown language, report it and ignore it
            {
                LogError("setting card language", "Unknown language \"" + language +
                    "\". Allowed values (case sensitive): English; French; German; Spanish; Italian; Simplified Chinese; Japanese; Portuguese; Russian; Korean; Traditional Chinese."
                    + " Language of the current item will be ignored.", false);
                data[MCAttribute.Language] = "";
                data[MCAttribute.LanguageID] = "";
            }
        }

        /// <summary>
        /// Performs checks whether the language ID is valid (number 1-11) and if so, sets it and also appropriate Language.
        /// </summary>
        /// <param name="languageID">The language identifier. Must be an integer 1-11</param>
        public void SetLanguageID(string languageID)
        {
            string language;
            if (languagesNames.TryGetValue(languageID, out language))
            {
                data[MCAttribute.LanguageID] = languageID;
                data[MCAttribute.Language] = language;
            }
            else// if it is an unknown language ID, report it and ignore it
            {
                LogError("setting card language ID", "Unknown language ID \"" + languageID +
                    "\", Allowed values are integer numbers from 1 to 12. Language of the current item will be ignored.", false);
                data[MCAttribute.Language] = "";
                data[MCAttribute.LanguageID] = "";
            }
        }

        /// <summary>
        /// Transform the condition to two-letter abbreviation style used by MKM and sets it as the Condition.
        /// If the condition is unrecognized, it will not be stored.
        /// </summary>
        /// <param name="condition">The condition in any supported format.</param>
        public void SetCondition(string condition)
        {
            condition = condition.ToUpper(); // MKM API is returning the conditions always as capital letters
            string conditionOut;
            if (!conditionsDictionary.TryGetValue(condition, out conditionOut))
            {
                LogError("setting card condition", "Unrecognized condition \"" + condition +
                    "\", condition of the current item will be ignored.", false);
                return;
            }
            data[MCAttribute.Condition] = conditionOut;
        }

        /// <summary>
        /// Transform the condition to two-letter abbreviation style used by MKM and sets it as the minimal condition of the card.
        /// If the condition is unrecognized, it will not be stored.
        /// </summary>
        /// <param name="condition">The condition in any supported format.</param>
        public void SetMinCondition(string condition)
        {
            condition = condition.ToUpper(); // MKM API is returning the conditions always as capital letters
            string conditionOut;
            if (!conditionsDictionary.TryGetValue(condition, out conditionOut))
            {
                LogError("setting card minCondition", "Unrecognized condition \"" + condition +
                    "\", condition of the current item will be ignored.", false);
                return;
            }
            data[MCAttribute.MinCondition] = conditionOut;
        }

        /// <summary>
        /// Sets an attribute that has boolean value. Also sets all existing synonyms to that value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value. Can be in any recognized format, any capitalization, it will be transformed to lowercase true/false/"".
        /// Recognized values: empty string always == "any"; true/false/null; yes/no/any; foil/signed/altered all equal true.</param>
        public void SetBoolAttribute(string name, string value)
        {
            value = value.ToLower(); // MKM API is returning boolean values always as "false/true/null"
            string toWrite = value;
            switch (value)
            {
                case "null": // means "any"
                    toWrite = "";
                    break;
                case "yes":
                case "x": // this is used by GET STOCK FILE
                    toWrite = "true";
                    break;
                case "no":
                    toWrite = "false";
                    break;
                case "any":
                    toWrite = "";
                    break;
                // deckbox.org says the word if it is true and leaves the field blank if it is false
                // note that we are deviating for the empty field case from their system because it is wrong:
                // for example, they leave blank foil field even for cards that are available only in foil like all kinds of promos
                // therefore we understand empty string or "null" as meaning "Any"
                case "foil":
                case "signed":
                case "altered":
                    toWrite = "true";
                    break;
            }
            SetAttribute(name, toWrite);
        }

        /// <summary>
        /// Sets the specified attribute to the specified value. Also sets all existing synonyms to that value.
        /// DOES NOT PERFORM ANY CHECKS ON VALIDITY - if there is a Set*** method available for a particular attribute (like language, condition),
        /// use that instead of this method to perform a check that the value is valid and that all related attributes will be set to correct values.
        /// Use SetBoolAttribute where applicable: foil, playset, signed, altered...
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value.</param>
        public void SetAttribute(string name, string value)
        {
            data[name] = value;
            // get the main name
            string mainName;
            if (synonyms.TryGetValue(name, out mainName)) // check if this is a registered synonym
            {
                data[mainName] = value; // don't forget to write it in the main name, not just other synonyms
            }
            else  // if it fails, then this is a main name
            {
                mainName = name;
            }
            // now look for all synonyms for the main name
            foreach (var entry in synonyms)
            {
                if (entry.Value == mainName)
                {
                    data[entry.Key] = value; // key are synonyms, values are main names
                }
            }
        }

        /// <summary>
        /// Removes the specified attribute entirely.
        /// </summary>
        /// <param name="name">The name of the attribute.</param>
        public void RemoveAttribute(string name)
        {
            data.Remove(name);
        }

        /// <summary>
        /// Generic method for obtaining values from the data object of this card. Performs a check if the value is assigned.
        /// </summary>
        /// <param name="key">The name of the card attribute to fetch. Note that some attributes might be internally re-named,
        /// so if there is a MCAttribute accessor that matches the attribute you want, use that.</param>
        /// <returns>The value of the attribute or empty string if it has not been set.</returns>
        public string GetAttribute(string key)
        {
            string ret;
            if (data.TryGetValue(key, out ret))
                return ret;
            return "";
        }

        /// <summary>
        /// Determines whether the specified other card has the minimum required condition by this card (attribute MinCondition).
        /// </summary>
        /// <param name="otherCard">The other card.</param>
        /// <returns>
        ///   <c>True</c> if the specified other card has Condition at least == this.MinCondition,
        ///   <c>False</c> if it does not and <c>Any</c> if either Condition is not assigned for the other card or MinCondition is not assigned for this card.
        /// </returns>
        public Bool3 IsOfMinCondition(MKMMetaCard otherCard)
        {
            string minC = GetAttribute(MCAttribute.MinCondition);
            if (minC != "")
            {
                string otherC = otherCard.GetAttribute(MCAttribute.Condition);
                if (otherC != "")
                {
                    bool res = IsBetterOrSameCondition(otherC, minC);
                    return res ? Bool3.True : Bool3.False;
                }
            }
            return Bool3.Any;
        }

        /// <summary>
        /// Writes all attributes of this card into the provided table.
        /// </summary>
        /// <param name="table">The table to which a new row will be appended with the attributes of this card.</param>
        /// <param name="writeAllAttributes">If set to false, only attributes that already have their respective columns (attributeName == columnName)
        /// in the table will be written. If this card does not have a value for that attribute, an empty string will be set as the value.
        /// If set to true, all attributes will be written - new columns will be added if they are missing.</param>
        /// <param name="format">Specific format - if not set to MKM, some fields will have reformatted output.</param>
        /// <param name="noSynonyms">If a given attribute is a synonym of an already existing one, do not write it if noSynonyms is true.
        /// However, if the column with the synonym attribute is already in the table, it will be written, i.e. this has effect 
        /// only if writeAllAttributes is set to true</param>
        public void WriteItselfIntoTable(DataTable table, bool writeAllAttributes, MCFormat format, bool noSynonyms)
        {
            List<string> attributes = new List<string>();
            foreach (DataColumn dc in table.Columns) // first we collect all attributes that are already in the table
            {
                attributes.Add(GetAttribute(dc.ColumnName));
            }
            if (writeAllAttributes)
            {
                // if we are supposed to write even attributes that are not in the table yet,
                // for each such attribute create a new column and add the current value
                foreach (var att in data)
                {
                    if (att.Value != "" && !table.Columns.Contains(att.Key))
                    {
                        if (noSynonyms)
                        {
                            // check if this attribute is a synonym, if it is, write it only if the "parent" (the main name) is non-empty
                            string synonymRoot;
                            if (synonyms.TryGetValue(att.Key, out synonymRoot))
                            {
                                if (GetAttribute(synonymRoot) != "")
                                    continue;
                            }
                        }
                        DataColumn newColumn = new DataColumn(att.Key, typeof(string));
                        newColumn.DefaultValue = "";
                        table.Columns.Add(newColumn);
                        attributes.Add(att.Value);
                    }
                }
            }
            DataRow dr = table.Rows.Add(attributes.ToArray());

            // do format-specific conversions
            if (format == MCFormat.Deckbox)
            {
                if (table.Columns.Contains(MCAttribute.Condition))
                {
                    string temp = dr[MCAttribute.Condition].ToString();
                    if (temp != "")
                        dr[MCAttribute.Condition] = conditionsDeckboxDictionary[temp];
                }
                if (table.Columns.Contains(MCAttribute.Foil))
                {
                    string temp = dr[MCAttribute.Foil].ToString();
                    if (temp != "")
                        dr[MCAttribute.Foil] = (temp == "true" ? "Foil" : "");
                }
                if (table.Columns.Contains(MCAttribute.Signed))
                {
                    string temp = dr[MCAttribute.Signed].ToString();
                    if (temp != "")
                        dr[MCAttribute.Signed] = (temp == "true" ? "Signed" : "");
                }
                if (table.Columns.Contains(MCAttribute.Altered))
                {
                    string temp = dr[MCAttribute.Altered].ToString();
                    if (temp != "")
                        dr[MCAttribute.Altered] = (temp == "true" ? "Altered" : "");
                }
            }
        }


        /// <summary>
        /// Compares all attributes defined for each card with attributes of the other card.
        /// </summary>
        /// <param name="card1">The card1.</param>
        /// <param name="card2">The card2.</param>
        /// <returns>
        /// True only if all attributes of each of the cards are compatible with all attributes of the other card.
        /// For most attributes, exact match is required, but for "range-like" attributes like MinCondition the 
        /// comparison is done such as to ensure that they are compatible with each other.
        /// </returns>
        public override bool Equals(object card2)
        {
            if (card2 is MKMMetaCard)
            {
                foreach (var att in data)
                {
                    if (att.Key == MCAttribute.MinCondition && att.Value != "")
                    {
                        // comparing minCondition requires custom handling
                        if (IsOfMinCondition((MKMMetaCard)card2) == Bool3.False)
                            return false;
                        if (((MKMMetaCard)card2).IsOfMinCondition(this) == Bool3.False)
                            return false;
                    }
                    else
                    {
                        // all other attributes are compared directly
                        if (att.Value != "")
                        {
                            string att2 = ((MKMMetaCard)card2).GetAttribute(att.Key);
                            if (att2 != "" && att.Value != att2)
                                return false;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        // Automatically generated by Visual Studio, just to shut up a warning that we are overriding Equals but not this
        public override int GetHashCode()
        {
            var hashCode = -373259831;
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(data);
            return hashCode;
        }
    }
}
