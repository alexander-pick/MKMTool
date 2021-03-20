﻿/*
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
      private static DateTime denyTime; // to know when denyAdditionalRequests was switched, if we pass to another day, reset it 
                                        // As of 25.11.2020, MKM allows maximum 600 requests per 60 seconds - use this queue to make sure we don't overstep it
      private static readonly int maxRequestsPerMinute = 600;
      private static readonly Queue<DateTime> requestTimes = new Queue<DateTime>(maxRequestsPerMinute);
      private static readonly MKMAuth.OAuthHeader header = new MKMAuth.OAuthHeader();

      /// Makes a request from MKM's API. 
      /// If the daily request limit has been reached, does not send the request and instead throws an exception.
      /// <param name="url">The http URL of the API.</param>
      /// <param name="method">The name of the request method (PUT, GET, etc.).</param>
      /// <param name="body">The body containing parameters of the method if applicable.</param>
      /// <returns>Document containing the response from MKM. In some cases this can empty (when the response is "nothing matches your request").</returns>
      /// <exception cref="HttpListenerException">429 - Too many requests. Wait for 0:00 CET for request counter to reset.</exception>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument MakeRequest(string url, string method, string body = null)
      {
        // throw the exception ourselves to prevent sending requests to MKM that would end with this error 
        // because MKM tends to revoke the user's app token if it gets too many requests above the limit
        // the 429 code is the same MKM uses for this error
        if (denyAdditionalRequests)
        {
          // MKM resets the counter at 0:00 CET. CET is two hours ahead of UCT, so if it is after 22:00 of the same day
          // the denial was triggered, that means the 0:00 CET has passed and we can reset the deny
          if (DateTime.UtcNow.Date == denyTime.Date && DateTime.UtcNow.Hour < 22)
            throw new HttpListenerException(429, "Too many requests. Wait for 0:00 CET for request counter to reset.");
          else
            denyAdditionalRequests = false;
        }
        // enforce the maxRequestsPerMinute limit - technically it's just an approximation as the requests
        // can arrive to MKM with some delay, but it should be close enough
        var now = DateTime.Now;
        while (requestTimes.Count > 0 && (now - requestTimes.Peek()).TotalSeconds > 60)
        {
          requestTimes.Dequeue();// keep only times of requests in the past 60 seconds
        }
        if (requestTimes.Count >= maxRequestsPerMinute)
        {
          // wait until 60.01 seconds passed since the oldest request
          // we know (now - peek) is <= 60, otherwise it would get dequeued above,
          // so we are passing a positive number to sleep
          System.Threading.Thread.Sleep(
              60010 - (int)(now - requestTimes.Peek()).TotalMilliseconds);
          requestTimes.Dequeue();
        }

        requestTimes.Enqueue(DateTime.Now);
        XmlDocument doc = new XmlDocument();
        for (int numAttempts = 0; numAttempts < MainView.Instance.Config.MaxTimeoutRepeat; numAttempts++)
        {
          try
          {
            var request = WebRequest.CreateHttp(url);
            request.Method = method;

            request.Headers.Add(HttpRequestHeader.Authorization, header.GetAuthorizationHeader(method, url));
            request.Method = method;

            if (body != null)
            {
              request.ServicePoint.Expect100Continue = false;
              request.ContentLength = System.Text.Encoding.UTF8.GetByteCount(body);
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
            MainView.Instance.Invoke(new MainView.UpdateRequestCountCallback(MainView.Instance.UpdateRequestCount), requestCount, requestLimit);
            break;
          }
          catch (WebException webEx)
          {
            // timeout can be either on our side (Timeout) or on server
            bool isTimeout = webEx.Status == WebExceptionStatus.Timeout;
            if (webEx.Status == WebExceptionStatus.ProtocolError)
            {
              if (webEx.Response is HttpWebResponse response)
              {
                isTimeout = response.StatusCode == HttpStatusCode.GatewayTimeout
                    || response.StatusCode == HttpStatusCode.ServiceUnavailable;
              }
            }
            // handle only timeouts, client handles other exceptions
            if (isTimeout && numAttempts + 1 < MainView.Instance.Config.MaxTimeoutRepeat)
              System.Threading.Thread.Sleep(1500); // wait and try again
            else
              throw webEx;
          }
        }
        return doc;
      }

      public static string AddWantsListBody(string idProduct, string minCondition, string idLanguage,
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

      /// Generates a XML entry for PUT STOCK for a given article.
      /// <param name="card">The card. Must have idArticle, price, idLanguage, count and condition set.</param>
      /// <param name="sNewPrice">The new price to set.</param>
      /// <returns>The body of the API request.</returns>
      public static string ChangeStockArticleBody(MKMMetaCard card, string sNewPrice)
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

      /// Generates a XML entry for POST STOCK for a given article, i.e. uploading a new article to stock.
      /// <param name="card">The card. Must have idProduct, MKMPrice, LanguageID, Count and Condition set.</param>
      /// <param name="sNewPrice">The new price to set.</param>
      /// <returns>The body of the API request.</returns>
      public static string PostStockArticleBody(MKMMetaCard card)
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

      public static string DeleteWantsListBody(string idWant)
      {
        var XMLContent = "<action>deleteItem</action>" +
                         "<want>" +
                         "<idWant>" + idWant + "</idWant>" +
                         "</want>";

        return XMLContent;
      }

      public static string GetRequestBody(string sInnerXML)
      {
        var XMLRequest = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                         "<request>" +
                         sInnerXML +
                         "</request>";

        return XMLRequest;
      }

      public static string AddCartBody(string sIDArticle)
      {
        var XMLContent = "<action>add</action>" +
                         "<article>" +
                         "<idArticle>" + sIDArticle + "</idArticle>" +
                         "<amount>1</amount>" +
                         "</article>";

        return XMLContent;
      }

      /// Sends stock update to MKM.
      /// <param name="sRequestXML">The raw (= not as an API request yet) XML with all article updates. Check that it is not empty before calling this.
      /// Also, it must match the used method - PUT requires to know articleID for each article, POST productID.</param>
      /// <param name="method">"PUT" to update articles already in stock, "POST" to upload new articles</param>
      public static void SendStockUpdate(string sRequestXML, string method)
      {
        sRequestXML = GetRequestBody(sRequestXML);

        try
        {
          XmlDocument rdoc = MakeRequest("https://api.cardmarket.com/ws/v2.0/stock", method,
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

      /// Gets our account info.
      /// <returns>From MKM documentation: <i>Returns the Account entity of the authenticated user.</i></returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument GetAccount()
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/account", "GET");
      }

      /// Gets all our wantlists.
      /// <returns>From MKM documentation: <i>Returns a list with all of the user's wantslists, their name, associated game, and item count.</i></returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument GetWantsLists()
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/wantslist", "GET");
      }

      /// Gets wantlist by its ID.
      /// <param name="sID">Id of want list</param>
      /// <returns>Returns a specific wantslist designated by the provided ID</returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument GetWantsListByID(string sID)
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/wantslist/" + sID, "GET");
      }

      public static byte[] GetPriceGuideFile(string gameId)
      {
        var doc = MakeRequest("https://api.cardmarket.com/ws/v2.0/priceguide?idGame="
            + gameId, "GET");

        var node = doc.GetElementsByTagName("response");

        if (node.Count > 0 && node.Item(0)["priceguidefile"].InnerText != null)
        {
          var data = Convert.FromBase64String(node.Item(0)["priceguidefile"].InnerText);
          var aDecompressed = MKMHelpers.GzDecompress(data);
          return aDecompressed;
        }
        else
        {
          MKMHelpers.LogError("getting price guide file", "failed to get the price guide file from MKM.", false);
          return null;
        }
      }

      /// Gets our stock.
      /// <param name="start">How many first items in the stock to skip.</param>
      /// <returns>Returns the users stock, starting from the specified item and grabbing the largest amount of 
      /// items allowed by the API.</returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument ReadStock(int start = 1)
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/stock/" + start, "GET");
      }

      /// Gets the stock file csv.
      /// Warning: the Language column in the returned csv is actually Language ID and boolean vars 
      /// (foil etc. are empty for "false"); the Local Name column is not the local name based on card language,
      /// but based on our request (so always English).
      /// Prefer using the wrapper getAllStockSingles.
      /// <param name="gameId">Game for which to get the stock.</param>
      /// <returns>Decompressed data containing the stock file. Can either be written directly to an output stream.
      /// Null if the reading failed (this method logs the error). Empty array if no items are in the stock.</returns>
      private static byte[] getStockFile(string gameId)
      {
        var doc = MakeRequest("https://api.cardmarket.com/ws/v2.0/stock/file?idGame="
            + gameId, "GET");

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

      /// Returns all single cards in our stock as meta cards. This is just a convenience wrapper on getStockFile/readStock.
      /// <param name="useFile">This is for legacy support. If set to false, it will use the old way of getting stock
      /// by the readStock method. New way is to use getStockFile as it takes only a single API request.</param>
      /// <returns>List of all single cards in our stock</returns>
      public static List<MKMMetaCard> GetAllStockSingles(bool useFile)
      {
        MainView.Instance.LogMainWindow("Fetching stock...");
        List<MKMMetaCard> cards = new List<MKMMetaCard>();
        if (useFile)
        {
          foreach (var game in MainView.Instance.Config.Games)
          {
            byte[] stock = getStockFile(game.GameID);
            if (stock != null && stock.Length > 0)
            {
              var articleTable = MKMCsvUtils.ConvertCSVtoDataTable(stock);
              // the GET STOCK FILE has language ID named Language, fix that
              articleTable.Columns["Language"].ColumnName = MCAttribute.LanguageID;
              articleTable.Columns.Remove("Local Name"); // this is in the language of the request...which is always English
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
        }
        else
        {
          var start = 1;
          XmlNodeList result;
          var count = 0;
          do
          {
            var doc = ReadStock(start);
            if (doc.HasChildNodes)
            {
              result = doc.GetElementsByTagName("article");
              foreach (XmlNode article in result)
              {
                // according to the API documentation, "The 'condition' key is only returned for single cards. "
                // -> check if condition exists to see if this is a single card or something else
                if (article["condition"] != null && article["idArticle"].InnerText != null)
                {
                  var card = new MKMMetaCard(article);
                  cards.Add(card);
                }
              }
              count = result.Count;
              start += count;
            }
          } while (count == 100);
        }
        MainView.Instance.LogMainWindow("Finished fetching stock.");
        return cards;
      }

      /// From MKM documentation: <i>Empties the authenticated user's shopping cart.</i>
      /// <returns>Not used</returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument EmptyCart()
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/shoppingcart", "DELETE");
      }

      /// Gets all cards from a given expansion.
      /// <param name="ExpansionID">ID of the expansion.</param>
      /// <returns>From MKM documentation: <i>Returns all single cards for the specified expansion.</i></returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument GetExpansionsSingles(string ExpansionID)
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/expansions/" + ExpansionID + "/singles", "GET"); ;
      }

      /// Gets all expansions from a given game.
      /// <param name="ExpansionID">Game ID (use 1 for M:tG).</param>
      /// <returns>From MKM documentation: <i>Returns all expansions with single cards for the specified game.</i></returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument GetExpansions(string sGameID)
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/games/" + sGameID + "/expansions", "GET");
      }

      /// Gets the specified product (detailed, i.e. including price guides).
      /// <param name="idProduct">MKM's ID of the product.</param>
      /// <returns>From MKM documentation: <i>Returns a product specified by its ID.</i></returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument GetProduct(string idProduct)
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/products/" + idProduct, "GET");
      }

      /// Gets the specified metaproduct.
      /// <param name="idMetaproduct">ID stored for example in product->metaproduct->idMetaproduct of wantlist items.</param>
      /// <returns>From MKM documentation: <i>Returns the metaproduct specified by its ID.</i></returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument GetMetaproduct(string idMetaproduct)
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/metaproducts/" + idMetaproduct, "GET");
      }

      /// Gets all product matching the provided name and language.
      /// <param name="cardname">Name of the card in the language specified by languageID. Can be only partial,
      /// i.e. "force of n" will return product objects for both "force of nature" and "force of negation".</param>
      /// <param name="languageID">ID of the language in which the card name is. Use MKMHelpers.languagesIds to get the correct ID.</param>
      /// <param name="start">How many first items in the stock to skip.</param>
      /// <returns>A list of products (without "details", such as price guide) matching the specified name. At most 100 products is grabbed,
      /// increase the "start" parameter to and query again for the rest.</returns>
      /// <exception cref="APIProcessingExceptions">Many different network-based exceptions.</exception>
      public static XmlDocument FindProducts(string cardname, string languageID, int start = 0)
      {
        return MakeRequest("https://api.cardmarket.com/ws/v2.0/products/find?search="
            + cardname + "&idGame=1&maxResults=100&idLanguage=" + languageID + "&start=" + start, "GET");
      }
    }
  }
}
