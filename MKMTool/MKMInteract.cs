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
using System.IO;
using System.Net;
using System.Xml;

namespace MKMTool
{
    public class MKMInteract
    {
        public class RequestHelper
        {
            private static bool denyAdditionalRequests = false; // this switches to true once requests limit is reached for the day
            private static System.DateTime denyTime; // to know when denyAdditionalRequests was switched, if we pass to another day, reset it 

            /// <summary>
            /// Makes a request from MKM's API. 
            /// If the daily request limit has been reached, does not send the request and instead throws an exception.
            /// </summary>
            /// <param name="url">The http URL of the API.</param>
            /// <param name="method">The name of the request method (PUT, GET, etc.).</param>
            /// <param name="body">The body containing parameters of the method if applicable.</param>
            /// <returns>Document containing the response from MKM.</returns>
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
                var request = WebRequest.CreateHttp(url);
                request.Method = method;

                var header = new MKMAuth.OAuthHeader();
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
                var doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                int requestCount = int.Parse(response.Headers.Get("X-Request-Limit-Count"));
                int requestLimit = int.Parse(response.Headers.Get("X-Request-Limit-Max"));
                if (requestCount >= requestLimit)
                {
                    denyAdditionalRequests = true;
                    denyTime = System.DateTime.UtcNow;
                }
                MainView.Instance().Invoke(new MainView.updateRequestCountCallback(MainView.Instance().updateRequestCount), requestCount, requestLimit);
    
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
            /// <returns>From MKM documentation: <i>Returns all expansions with single cards for the specified game..</i></returns>
            /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
            public static XmlDocument getExpansions(string sGameID)
            {
                return makeRequest("https://api.cardmarket.com/ws/v2.0/games/" + sGameID + "/expansions", "GET");
            }
        }
    }
}