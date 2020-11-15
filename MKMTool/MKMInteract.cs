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
using System.Data;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace MKMTool
{
    public class MKMInteract
    {
        public class RequestHelper
        {
            private static bool denyAdditionalRequests = false; // this switches to true once requests limit is reached for the day
            private static System.DateTime denyTime; // to know when denyAdditionalRequests was switched, if we pass to another day, reset it 
            private static MKMAuth.OAuthHeader header = new MKMAuth.OAuthHeader();

            /// <summary>
            /// Makes a request from MKM's API. 
            /// If the daily request limit has been reached, does not send the request and instead throws an exception.
            /// </summary>
            /// <param name="url">The http URL of the API.</param>
            /// <param name="method">The name of the request method (PUT, GET, etc.).</param>
            /// <param name="body">The body containing parameters of the method if applicable.</param>
            /// <returns>Document containing the response from MKM. In some cases this can empty (when the response is "nothing matches your request").</returns>
            /// <exception cref="HttpListenerException">429 - Too many requests. Wait for 0:00 CET for request counter to reset.</exception>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument makeRequest(string url, string method, string body = null)
            {
                // throw the exception ourselves to prevent sending requests to MKM that would end with this error 
                // because MKM tends to revoke the user's app token if it gets too many requests above the limit
                // the 429 code is the same MKM uses for this error
                if (denyAdditionalRequests)
                {
                    // MKM resets the counter at 0:00 CET. CET is two hours ahead of UCT, so if it is after 22:00 of the same day
                    // the denial was triggered, that means the 0:00 CET has passed and we can reset the deny
                    if (System.DateTime.UtcNow.Date == denyTime.Date && System.DateTime.UtcNow.Hour < 22)
                        throw new HttpListenerException(429, "Too many requests. Wait for 0:00 CET for request counter to reset.");
                    else
                        denyAdditionalRequests = false;
                }
                XmlDocument doc = new XmlDocument();
                var request = WebRequest.CreateHttp(url);
                request.Method = method;

                request.Headers.Add(HttpRequestHeader.Authorization, header.getAuthorizationHeader(method, url));
                request.Method = method;

                if (body != null)
                {
                    request.ServicePoint.Expect100Continue = false;
                    request.ContentLength = body.Length;
                    request.ContentType = "text/xml";

                    var writer = new StreamWriter(request.GetRequestStream());

                    writer.Write(body);
                    writer.Close();
                }

                var response = request.GetResponse() as HttpWebResponse;

                // just for checking EoF, it is not accessible directly from the Stream object
                // Empty streams can be returned for example for article fetches that result in 0 matches (happens regularly when e.g. seeking nonfoils in foil-only promo sets). 
                // Passing empty stream to doc.Load causes exception and also sometimes seems to screw up the XML parser 
                // even when the exception is handled and it then causes problems for subsequent calls => first check if the stream is empty
                StreamReader s = new StreamReader(response.GetResponseStream());
                if (!s.EndOfStream)
                    doc.Load(s);
                s.Close();
                int requestCount = int.Parse(response.Headers.Get("X-Request-Limit-Count"));
                int requestLimit = int.Parse(response.Headers.Get("X-Request-Limit-Max"));
                if (requestCount >= requestLimit)
                {
                    denyAdditionalRequests = true;
                    denyTime = System.DateTime.UtcNow;
                }
                MainView.Instance.Invoke(new MainView.updateRequestCountCallback(MainView.Instance.updateRequestCount), requestCount, requestLimit);
    
                return doc;
            }

            public static string addWantsListBody(string idProduct, string minCondition, string idLanguage,
                string isFoil, string isAltered, string isPlayset, string isSigned)
            {
                var XMLContent = "<action>addItem</action>" +
                                 "<product>" +
                                 "<idProduct>" + idProduct + "</idProduct>" +
                                 "<count>1</count>" +
                                 "<minCondition>" + minCondition + "</minCondition>" +
                                 "<wishPrice>0.01</wishPrice>" +
                                 "<mailAlert>false</mailAlert>" +
                                 "<idLanguage>" + idLanguage + "</idLanguage>";

                if (isFoil == "Checked")
                    XMLContent += "<isFoil>true</isFoil>";

                if (isSigned == "Checked")
                    XMLContent += "<isSigned>true</isSigned>";

                if (isAltered == "Checked")
                    XMLContent += "<isAltered>true</isAltered>";

                if (isPlayset == "Checked")
                    XMLContent += "<isPlayset>true</isPlayset>";

                XMLContent += "</product>";

                return XMLContent;
            }

            public static string changeStockArticleBody(XmlNode xNode, string sNewPrice)
            {
                var XMLContent = "<article>" +
                                 "<idArticle>" + xNode["idArticle"].InnerText + "</idArticle>" +
                                 "<price>" + sNewPrice + "</price>" +
                                 "<idLanguage>" + xNode["language"]["idLanguage"].InnerText + "</idLanguage>" +
                                 "<comments>" + xNode["comments"].InnerText + " </comments>" +
                                 "<count>" + xNode["count"].InnerText + "</count>" +
                                 "<condition>" + xNode["condition"].InnerText + "</condition>" +
                                 "</article>";

                return XMLContent;
            }

            /// <summary>
            /// Generates a XML entry for PUT STOCK for a given article.
            /// </summary>
            /// <param name="card">The card. Must have idArticle, price, idLanguage, count and condition set.</param>
            /// <param name="sNewPrice">The new price to set.</param>
            /// <returns>The body of the API request.</returns>
            public static string changeStockArticleBody(MKMMetaCard card, string sNewPrice)
            {
                var XMLContent = "<article>" +
                                 "<idArticle>" + card.GetAttribute(MCAttribute.ArticleID) + "</idArticle>" +
                                 "<price>" + sNewPrice + "</price>" +
                                 "<idLanguage>" + card.GetAttribute(MCAttribute.LanguageID) + "</idLanguage>" +
                                 new XElement("comments", card.GetAttribute(MCAttribute.Comments)) + // to escape special characters (&, <, > etc.)
                                 "<count>" + card.GetAttribute(MCAttribute.Count) + "</count>" +
                                 "<condition>" + card.GetAttribute(MCAttribute.Condition) + "</condition>" +
                                 "</article>";

                return XMLContent;
            }

            /// <summary>
            /// Generates a XML entry for POST STOCK for a given article, i.e. uploading a new article to stock.
            /// </summary>
            /// <param name="card">The card. Must have idProduct, MKMPrice, LanguageID, Count and Condition set.</param>
            /// <param name="sNewPrice">The new price to set.</param>
            /// <returns>The body of the API request.</returns>
            public static string postStockArticleBody(MKMMetaCard card)
            {
                string isFoil = card.GetAttribute(MCAttribute.Foil);
                string isSigned = card.GetAttribute(MCAttribute.Signed);
                string isPlayset = card.GetAttribute(MCAttribute.Playset);
                string isAltered = card.GetAttribute(MCAttribute.Altered);
                var XMLContent = "<article>" +
                                "<idProduct>" + card.GetAttribute(MCAttribute.ProductID) + "</idProduct>" +
                                "<idLanguage>" + card.GetAttribute(MCAttribute.LanguageID) + "</idLanguage>" +
                                 new XElement("comments", card.GetAttribute(MCAttribute.Comments)) + // to escape special characters (&, <, > etc.)
                                "<count>" + card.GetAttribute(MCAttribute.Count) + "</count>" +
                                "<price>" + card.GetAttribute(MCAttribute.MKMPrice) + "</price>" +
                                "<condition>" + card.GetAttribute(MCAttribute.Condition) + "</condition>" +
                                (isFoil == "" ? "" : ("<isFoil>" + isFoil + "</isFoil>")) +
                                (isSigned == "" ? "" : ("<isSigned>" + isSigned + "</isSigned>")) +
                                (isPlayset == "" ? "" : ("<isPlayset>" + isPlayset + "</isPlayset>")) +
                                (isAltered == "" ? "" : ("<isAltered>" + isAltered + "</isAltered>")) +
                                "</article>";

                return XMLContent;
            }

            public static string deleteWantsListBody(string idWant)
            {
                var XMLContent = "<action>deleteItem</action>" +
                                 "<want>" +
                                 "<idWant>" + idWant + "</idWant>" +
                                 "</want>";

                return XMLContent;
            }

            public static string getRequestBody(string sInnerXML)
            {
                var XMLRequest = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                                 "<request>" +
                                 sInnerXML +
                                 "</request>";

                return XMLRequest;
            }

            public static string addCartBody(string sIDArticle)
            {
                var XMLContent = "<action>add</action>" +
                                 "<article>" +
                                 "<idArticle>" + sIDArticle + "</idArticle>" +
                                 "<amount>1</amount>" +
                                 "</article>";

                return XMLContent;
            }

            /// <summary>
            /// Sends stock update to MKM.
            /// </summary>
            /// <param name="sRequestXML">The raw (= not as an API request yet) XML with all article updates. Check that it is not empty before calling this.
            /// Also, it must match the used method - PUT requires to know articleID for each article, POST productID.</param>
            /// <param name="method">"PUT" to update articles already in stock, "POST" to upload new articles</param>
            public static void SendStockUpdate(string sRequestXML, string method)
            {
                sRequestXML = getRequestBody(sRequestXML);
                
                try
                {
                    XmlDocument rdoc = makeRequest("https://api.cardmarket.com/ws/v2.0/stock", method,
                        sRequestXML);
                    int iUpdated = 0, iFailed = 0;
                    string failed = "";
                    if (method == "PUT")
                    {
                        var xUpdatedArticles = rdoc.GetElementsByTagName("updatedArticles");
                        var xNotUpdatedArticles = rdoc.GetElementsByTagName("notUpdatedArticles");
                        foreach (XmlNode node in xNotUpdatedArticles)
                            failed += node.InnerText;
                        // there is always at least one element of each updated and notUpdated articles, but it can be empty if nothing succeeded/failed
                        // problem is, if exactly one failed, there will still be one element, so we need to disambiguate it
                        iUpdated = xUpdatedArticles.Count;
                        if (iUpdated == 1 && xUpdatedArticles[0].InnerText == "")
                            iUpdated = 0;
                        iFailed = failed == "" ? 0 : xNotUpdatedArticles.Count;
                    }
                    else if (method == "POST")
                    {
                        XmlNodeList inserted = rdoc.GetElementsByTagName("inserted");
                        foreach (XmlNode x in inserted)
                        {
                            if (x["success"].InnerText == "true")
                                iUpdated++;
                            else
                            {
                                iFailed++;
                                failed += x.InnerText;
                            }
                        }
                    }

                    MainView.Instance.LogMainWindow(
                        iUpdated + " articles updated successfully, " + iFailed + " failed");

                    if (iFailed > 0)
                    {
                        try
                        {
                            File.WriteAllText(@".\\log" + DateTime.Now.ToString("ddMMyyyy-HHmm") + ".log", failed);
                        }
                        catch (Exception eError)
                        {
                            MKMHelpers.LogError("logging failed stock update articles", eError.Message, false);
                        }
                        MainView.Instance.LogMainWindow(
                            "Failed articles logged in " + @".\\log" + DateTime.Now.ToString("ddMMyyyy-HHmm") + ".log");
                    }
                }
                catch (Exception eError)
                {
                    // for now this does not break the price update, i.e. the bot will attempt to update the following items - maybe it shouldn't as it is likely to fail again?
                    MKMHelpers.LogError("sending stock update to MKM", eError.Message, false, sRequestXML);
                }
            }

            /// <summary>
            /// Gets our account info.
            /// </summary>
            /// <returns>From MKM documentation: <i>Returns the Account entity of the authenticated user.</i></returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument getAccount()
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/account", "GET");
            }

            /// <summary>
            /// Gets all our wantlists.
            /// </summary>
            /// <returns>From MKM documentation: <i>Returns a list with all of the user's wantslists, their name, associated game, and item count.</i></returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument getWantsLists()
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/wantslist", "GET");
            }

            /// <summary>
            /// Gets wantlist by its ID.
            /// </summary>
            /// <param name="sID">Id of want list</param>
            /// <returns>Returns a specific wantslist designated by the provided ID</returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument getWantsListByID(string sID)
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/wantslist/" + sID, "GET");
            }

            /// <summary>
            /// Gets our stock.
            /// </summary>
            /// <param name="start">How many first items in the stock to skip.</param>
            /// <returns>Returns the users stock, starting from the specified item and grabbing the largest amount of 
            /// items allowed by the API.</returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument readStock(int start = 1)
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/stock/" + start, "GET");
            }

            /// <summary>
            /// Gets the stock file csv.
            /// Warning: the Language column in the returned csv is actually Language ID and boolean vars (foil etc. are empty for "false").
            /// Prefer using the wrapper getAllStockSingles.
            /// </summary>
            /// <returns>Decompressed data containing the stock file. Can either be written directly to an output stream.
            /// Null if the reading failed (this method logs the error).</returns>
            private static byte[] getStockFile()
            {
                var doc = makeRequest("https://api.cardmarket.com/ws/v2.0/stock/file", "GET");

                var node = doc.GetElementsByTagName("response");
                                
                if (node.Count > 0 && node.Item(0)["stock"].InnerText != null)
                {
                    var data = Convert.FromBase64String(node.Item(0)["stock"].InnerText);
                    var aDecompressed = MKMHelpers.GzDecompress(data);

                    return aDecompressed;
                }
                else
                {
                    MKMHelpers.LogError("getting stock file", "failed to get the stock file from MKM.", false);
                    return null;
                }
            }

            /// <summary>
            /// Returns all single cards in our stock as meta cards. This is just a convenience wrapper on getStockFile/readStock.
            /// </summary>
            /// <param name="useFile">This is for legacy support. If set to false, it will use the old way of getting stock
            /// by the readStock method. New way is to use getStockFile as it takes only a single API request.</param>
            /// <returns>List of all single cards in our stock</returns>
            public static List<MKMMetaCard> getAllStockSingles(bool useFile)
            {
                List<MKMMetaCard> cards = new List<MKMMetaCard>();
                if (useFile)
                {
                    byte[] stock = getStockFile();
                    if (stock != null)
                    {
                        var articleTable = MKMCsvUtils.ConvertCSVtoDataTable(stock);
                        // the GET STOCK FILE has language ID named Language, fix that
                        articleTable.Columns["Language"].ColumnName = MCAttribute.LanguageID;
                        foreach (DataRow row in articleTable.Rows)
                        {
                            MKMMetaCard mc = new MKMMetaCard(row);
                            // according to the API documentation, "The 'condition' key is only returned for single cards. "
                            // -> check if condition exists to see if this is a single card or something else
                            if (mc.GetAttribute(MCAttribute.Condition) != "" && mc.GetAttribute(MCAttribute.ArticleID) != "")
                            {
                                // sanitize the false booleans - the empty ones mean no, while in MKMMEtaCard empty means "any"
                                if (articleTable.Columns.Contains("Foil?") && mc.GetAttribute(MCAttribute.Foil) == "")
                                {
                                    mc.SetBoolAttribute(MCAttribute.Foil, "false");
                                }
                                if (articleTable.Columns.Contains("Altered?") && mc.GetAttribute(MCAttribute.Altered) == "")
                                {
                                    mc.SetBoolAttribute(MCAttribute.Altered, "false");
                                }
                                if (articleTable.Columns.Contains("Signed?") && mc.GetAttribute(MCAttribute.Signed) == "")
                                {
                                    mc.SetBoolAttribute(MCAttribute.Signed, "false");
                                }
                                if (articleTable.Columns.Contains("Playset?") && mc.GetAttribute(MCAttribute.Playset) == "")
                                {
                                    mc.SetBoolAttribute(MCAttribute.Playset, "false");
                                }
                                // this is just a guess, not sure how isFirstEd is written there, or if at all
                                if (articleTable.Columns.Contains("FirstEd?") && mc.GetAttribute(MCAttribute.FirstEd) == "")
                                {
                                    mc.SetBoolAttribute(MCAttribute.FirstEd, "false");
                                }
                                cards.Add(mc);
                            }
                        }
                    }
                }
                else
                {
                    var start = 1;
                    XmlNodeList result;
                    var count = 0;
                    do
                    {
                        var doc = readStock(start);
                        if (doc.HasChildNodes)
                        {
                            result = doc.GetElementsByTagName("article");
                            foreach (XmlNode article in result)
                            {
                                // according to the API documentation, "The 'condition' key is only returned for single cards. "
                                // -> check if condition exists to see if this is a single card or something else
                                if (article["condition"] != null && article["idArticle"].InnerText != null)
                                {
                                    cards.Add(new MKMMetaCard(article));
                                }
                            }
                            count = result.Count;
                            start += count;
                        }
                    } while (count == 100);
                }
                return cards;
            }

            /// <summary>
            /// From MKM documentation: <i>Empties the authenticated user's shopping cart.</i>
            /// </summary>
            /// <returns>Not used</returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument emptyCart()
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/shoppingcart", "DELETE");
            }

            /// <summary>
            /// Gets all cards from a given expansion.
            /// </summary>
            /// <param name="ExpansionID">ID of the expansion.</param>
            /// <returns>From MKM documentation: <i>Returns all single cards for the specified expansion.</i></returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument getExpansionsSingles(string ExpansionID)
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/expansions/" + ExpansionID + "/singles", "GET"); ;
            }

            /// <summary>
            /// Gets all expansions from a given game.
            /// </summary>
            /// <param name="ExpansionID">Game ID (use 1 for M:tG).</param>
            /// <returns>From MKM documentation: <i>Returns all expansions with single cards for the specified game.</i></returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument getExpansions(string sGameID)
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/games/" + sGameID + "/expansions", "GET");
            }

            /// <summary>
            /// Gets the specified product (detailed, i.e. including price guides).
            /// </summary>
            /// <param name="idProduct">MKM's ID of the product.</param>
            /// <returns>From MKM documentation: <i>Returns a product specified by its ID.</i></returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument getProduct(string idProduct)
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/products/" + idProduct, "GET");
            }

            /// <summary>
            /// Gets the specified metaproduct.
            /// </summary>
            /// <param name="idMetaproduct">ID stored for example in product->metaproduct->idMetaproduct of wantlist items.</param>
            /// <returns>From MKM documentation: <i>Returns the metaproduct specified by its ID.</i></returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument getMetaproduct(string idMetaproduct)
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/metaproducts/" + idMetaproduct, "GET");
            }

            /// <summary>
            /// Gets all product matching the provided name and language.
            /// </summary>
            /// <param name="cardname">Name of the card in the language specified by languageID. Can be only partial,
            /// i.e. "force of n" will return product objects for both "force of nature" and "force of negation".</param>
            /// <param name="languageID">ID of the language in which the card name is. Use MKMHelpers.languagesIds to get the correct ID.</param>
            /// <param name="start">How many first items in the stock to skip.</param>
            /// <returns>A list of products (without "details", such as price guide) matching the specified name. At most 100 products is grabbed,
            /// increase the "start" parameter to and query again for the rest.</returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument findProducts(string cardname, string languageID, int start = 0)
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/products/find?search=" 
                    + cardname + "&idGame=1&maxResults=100&idLanguage=" + languageID + "&start=" + start, "GET");
            }
        }
    }
}