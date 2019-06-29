using System;
using System.Data;
using System.Collections.Generic;
using System.Xml;

namespace MKMTool
{
    /// <summary>
    /// A three-state boolean allowing "any" as the third state.
    /// </summary>
    public enum Bool3 { False = 0, True = 1, Any = 2}

    /// <summary>
    /// A "string enum" of all attributes MKMMetaCard uses for comparisons, exports etc.
    /// If you are extending functionality of MKMMetaCard by working with some new attribute, don't forget to add it here.
    /// </summary>
    public class MKMMetaCardAttribute
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
        /// English name of the language, first letters capital, i.e. one of the following strings:
        /// (English;French; German; Spanish; Italian; Simplified Chinese; Japanese; Portuguese; Russian; Korean; Traditional Chinese) 
        /// </summary>
        public static string Language { get { return "Language"; } }

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
        /// The MKM's product ID.
        /// </summary>
        public static string ProductID { get { return "idProduct"; } }

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
        public static string MKMToolPrice { get { return "MKMTool price"; } }

        /// <summary>
        /// The price of the card on MKM (relevant only for exact card article).
        /// </summary>
        public static string MKMPrice { get { return "Price"; } }

        /// <summary>
        /// For MKM articles, this is what is inside the "comments" field.
        /// </summary>
        public static string Comments { get { return "Comments"; } }
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
        // A map of all recognized attributes and the one name we internally use for each. Essentially some attributes (=keys to this dictionary)
        // are synonyms and the dictionary says which is the one name to use for these synonyms
        private static Dictionary<string, string> recognizedAttributes = new Dictionary<string, string>();

        // list of attribute names that are comparable directly, e.g. Name, Expansion etc. 
        // attributes not in this list require custom implementation of comparison
        private static HashSet<string> comparableAttributes = new HashSet<string>();

        private static HashSet<string> Conditions = new HashSet<string>();

        /// <summary>
        /// Used to set-up the dictionary of recognized attributes. If you are extending the class to internally handle some additional attribute,
        /// make sure to include it in this method and in the MKMMetaCardAttribute class above.
        /// </summary>
        static MKMMetaCard()
        {
            recognizedAttributes["Name"] = MKMMetaCardAttribute.Name;
            recognizedAttributes["enName"] = MKMMetaCardAttribute.Name;

            recognizedAttributes["LocName"] = MKMMetaCardAttribute.LocName;

            recognizedAttributes["Expansion"] = MKMMetaCardAttribute.Expansion;
            recognizedAttributes["Edition"] = MKMMetaCardAttribute.Expansion;

            recognizedAttributes["Expansion ID"] = MKMMetaCardAttribute.ExpansionID; // MKM's ID

            recognizedAttributes["Language"] = MKMMetaCardAttribute.Language;

            recognizedAttributes["Condition"] = MKMMetaCardAttribute.Condition;

            recognizedAttributes["MinCondition"] = MKMMetaCardAttribute.MinCondition;

            recognizedAttributes["Foil"] = MKMMetaCardAttribute.Foil;

            recognizedAttributes["Signed"] = MKMMetaCardAttribute.Signed;

            recognizedAttributes["Altered"] = MKMMetaCardAttribute.Altered;
            recognizedAttributes["Altered Art"] = MKMMetaCardAttribute.Altered;

            recognizedAttributes["Playset"] = MKMMetaCardAttribute.Playset;

            recognizedAttributes["idProduct"] = MKMMetaCardAttribute.ProductID; // MKM's ID


            recognizedAttributes["Count"] = MKMMetaCardAttribute.Count;

            recognizedAttributes["Rarity"] = MKMMetaCardAttribute.Rarity;

            recognizedAttributes["MinPrice"] = MKMMetaCardAttribute.MinPrice;

            recognizedAttributes["MKMTool price"] = MKMMetaCardAttribute.MKMToolPrice;

            recognizedAttributes["Price"] = MKMMetaCardAttribute.MKMPrice;

            recognizedAttributes["Comments"] = MKMMetaCardAttribute.Comments;

            Conditions.Add("MT");
            Conditions.Add("NM");
            Conditions.Add("EX");
            Conditions.Add("GD");
            Conditions.Add("LP");
            Conditions.Add("PL");
            Conditions.Add("PO");

            comparableAttributes.Add(MKMMetaCardAttribute.Name);
            comparableAttributes.Add(MKMMetaCardAttribute.LocName);
            comparableAttributes.Add(MKMMetaCardAttribute.Expansion);
            comparableAttributes.Add(MKMMetaCardAttribute.ExpansionID);
            comparableAttributes.Add(MKMMetaCardAttribute.Language);
            comparableAttributes.Add(MKMMetaCardAttribute.Condition);
            comparableAttributes.Add(MKMMetaCardAttribute.Foil);
            comparableAttributes.Add(MKMMetaCardAttribute.Signed);
            comparableAttributes.Add(MKMMetaCardAttribute.Altered);
            comparableAttributes.Add(MKMMetaCardAttribute.Playset);
            comparableAttributes.Add(MKMMetaCardAttribute.ProductID);
            comparableAttributes.Add(MKMMetaCardAttribute.Count);
            comparableAttributes.Add(MKMMetaCardAttribute.Rarity);
            comparableAttributes.Add(MKMMetaCardAttribute.MinPrice);
            comparableAttributes.Add(MKMMetaCardAttribute.MKMToolPrice);
            comparableAttributes.Add(MKMMetaCardAttribute.MKMPrice);
            comparableAttributes.Add(MKMMetaCardAttribute.Comments);

            // not comparable: MinCondition
        }

        private Dictionary<string, string> data = new Dictionary<string, string>();
        private List<string> importedColumns = new List<string>(); // stores the unmodified names of columns used to initialize this card in the order they were in the input DataRow


        /// <summary>
        /// Initializes a new instance of the <see cref="MKMMetaCard"/> class based on a database entry.
        /// </summary>
        /// <param name="card">The card.</param>
        public MKMMetaCard(DataRow card)
        {
            for (int i = 0; i < card.Table.Columns.Count; i++)
            {
                string columnVal = card[i].ToString();
                importedColumns.Add(card.Table.Columns[i].ColumnName);
                string attName;
                if (!recognizedAttributes.TryGetValue(card.Table.Columns[i].ColumnName, out attName))
                    attName = card.Table.Columns[i].ColumnName;
                string attValue;
                if (data.TryGetValue(attName, out attValue) && attValue != columnVal) // it's already in, but is not the same
                {
                    // if this attribute has already been set, but to a different value, it means a synonym for this column has been processed
                    // in that case, store each of them as a separate variable so that it can be exported unchanged
                    if (attName != card.Table.Columns[i].ColumnName)
                        attName = card.Table.Columns[i].ColumnName; // we just write this column using the original name without the translation
                   else // this is the "main" name -> we should use this column -> rename the previously written one to its original name
                    {
                        // find the original name - it will be the first column that translates as the "main" name
                        foreach (string colName in importedColumns)
                        {
                            string temp;
                            if (recognizedAttributes.TryGetValue(colName, out temp) && temp == attName)
                            {
                                data[colName] = temp;
                                break;
                            }
                        }
                    }
                }
                // do translations of some attributes
                if (attName == MKMMetaCardAttribute.Condition)
                {
                    columnVal = columnVal.ToUpper(); // MKM API is returning the conditions always as capital letters
                    if (!Conditions.Contains(columnVal))
                    {
                        switch (columnVal)
                        {
                            case "MINT":
                                columnVal = "MT";
                                break;
                            case "NEAR MINT":
                                columnVal = "NM";
                                break;
                            case "EXCELLENT":
                                columnVal = "EX";
                                break;
                            case "GOOD":
                                columnVal = "GD";
                                break;
                            case "LIGHTLY PLAYED":
                                columnVal = "LP";
                                break;
                            case "POOR":
                                columnVal = "PO";
                                break;
                            // for the following the European (MKM) and American (deckbox.org) grades are the same word but different meaning
                            // we will assume that if it is the entire word, it is exported from deckbox.org and therefore uses the American grading
                            case "GOOD (LIGHTLY PLAYED)":
                                columnVal = "EX";
                                break;
                            case "PLAYED":
                                columnVal = "GD";
                                break;
                            case "HEAVILY PLAYED": // this could be either LP or PL, we will never know...let's set it to LP
                                columnVal = "LP";
                                break;
                        }
                    }
                }
                else if (attName == MKMMetaCardAttribute.Foil || attName == MKMMetaCardAttribute.Signed
                    || attName == MKMMetaCardAttribute.Altered || attName == MKMMetaCardAttribute.Playset)
                {
                    columnVal = columnVal.ToLower(); // MKM API is returning boolean values always as "false/true/null"
                    switch (columnVal)
                    {
                        case "null": // means "any"
                            columnVal = "";
                            break;
                        // deckbox.org says the word if it is true and leaves the field blank if it is false
                        // note that we are deviating for the empty field case from their system because it is wrong:
                        // for example, they leave blank foil field even for cards that are available only in foil like all kinds of promos
                        // therefore we understand empty string or "null" as meaning "Any"
                        case "foil":
                            columnVal = "true";
                            break;
                        case "signed":
                            columnVal = "true";
                            break;
                        case "altered":
                            columnVal = "true";
                            break;
                    }
                }
                if (columnVal != "") // having an attribute at "any" is the same as not having it at all
                    data[attName] = columnVal;
            }

            // fill in missing data that we know from our local database (and have them be "any" would make no sense)
            string val = GetAttribute(MKMMetaCardAttribute.ProductID);
            if (val != "") // if we know product ID, we can use the inventory database to get expansion ID and Name
            {
                DataRow row = MKMDatabaseManager.Instance.GetSingleCard(val);
                if (row != null)
                {
                    data[MKMMetaCardAttribute.ExpansionID] = row[MKMMetaCardAttribute.ExpansionID].ToString();
                    data[MKMMetaCardAttribute.Name] = row[MKMMetaCardAttribute.Name].ToString();
                }
            }
            val = GetAttribute(MKMMetaCardAttribute.Expansion);
            if (val != "")
                data[MKMMetaCardAttribute.ExpansionID] = MKMDatabaseManager.Instance.GetExpansionID(val);
            else
            {
                val = GetAttribute(MKMMetaCardAttribute.ExpansionID);
                if (val != "")
                    data[MKMMetaCardAttribute.Expansion] = MKMDatabaseManager.Instance.GetExpansionName(val);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MKMMetaCard"/> class based on a MKM API's "article".
        /// Only attributes that are among the "recognized" ones (have an entry in MKMMetaCardAttribute) will be stored.
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
                inShoppingCart:                     // Flag, if that article is currently in a shopping cart
                product: {                          // Short Product entity
                    enName:                         // English name
                    locName:                        // Localized name (according to the language of the article)
                    image:                          // path to the product's image
                    expansion:                      // expansion name in English, if applicable
                    nr:                             // Number of single within the expansion, if applicable
                    expIcon:                        // index to the expansion's icon, if applicable
                    rarity:                         // product's rarity if applicable
                }
                seller: { }                          // Seller's user entity
                lastEdited:                         // Date, the article was last updated
                condition:                          // Product's condition, if applicable
                isFoil:                             // Foil flag, if applicable
                isSigned:                           // Signed flag, if applicable
                isAltered:                          // Altered flag, if applicable
                isPlayset:                          // Playset flag, if applicable
                links: { }                           // HATEOAS links
            }
            */
            data[MKMMetaCardAttribute.ProductID] = MKMArticle["idProduct"].InnerText;
            data[MKMMetaCardAttribute.Language] = MKMArticle["language"]["languageName"].InnerText;
            data[MKMMetaCardAttribute.Comments] = MKMArticle["comments"].InnerText;
            data[MKMMetaCardAttribute.MKMPrice] = MKMArticle["price"].InnerText;
            if (MKMArticle["product"] != null) // based on which API call was used, this can be null
            {
                data[MKMMetaCardAttribute.Name] = MKMArticle["product"]["enName"].InnerText;
                data[MKMMetaCardAttribute.LocName] = MKMArticle["product"]["locName"].InnerText;
                data[MKMMetaCardAttribute.Expansion] = MKMArticle["product"]["expansion"].InnerText;
                data[MKMMetaCardAttribute.Rarity] = MKMArticle["product"]["rarity"].InnerText;
            }
            data[MKMMetaCardAttribute.Count] = MKMArticle["count"].InnerText;
            data[MKMMetaCardAttribute.Condition] = MKMArticle["condition"].InnerText;
            data[MKMMetaCardAttribute.Foil] = MKMArticle["isFoil"].InnerText;
            data[MKMMetaCardAttribute.Signed] = MKMArticle["isSigned"].InnerText;
            data[MKMMetaCardAttribute.Altered] = MKMArticle["isAltered"].InnerText;
            data[MKMMetaCardAttribute.Playset] = MKMArticle["isPlayset"].InnerText;
        }

        /// <summary>
        /// Generic method for obtaining values from the data object of this card. Performs a check if the value is assigned.
        /// </summary>
        /// <param name="key">The name of the card attribute to fetch. Note that some attributes might be internally re-named,
        /// so if there is a MKMMetaCardAttribute accessor that matches the attribute you want, use that.</param>
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
            string minC = GetAttribute(MKMMetaCardAttribute.MinCondition);
            if (minC != "")
            {
                string otherC = otherCard.GetAttribute(MKMMetaCardAttribute.Condition);
                if (otherC != "")
                {
                    bool res = MKMHelpers.IsBetterOrSameCondition(otherC, minC);
                    return res ? Bool3.True : Bool3.False;
                }
            }
            return Bool3.Any;
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
                string att1, att2;
                foreach (string att in comparableAttributes)
                {
                    att1 = GetAttribute(att);
                    if (att1 != "")
                    {
                        att2 = ((MKMMetaCard)card2).GetAttribute(att);
                        if (att2 != "" && att1 != att2)
                            return false;
                    }
                }
                // now non-directly comparable
                if (IsOfMinCondition((MKMMetaCard)card2) == Bool3.False)
                    return false;
                if (((MKMMetaCard)card2).IsOfMinCondition(this) == Bool3.False)
                    return false;
                return true;
            }
            return false;
        }

        // Automatically generated by Visual Studio, just to shut up a warning that we are overriding Equals but not this
        public override int GetHashCode()
        {
            var hashCode = -373259831;
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, string>>.Default.GetHashCode(data);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(importedColumns);
            return hashCode;
        }
    }
}
