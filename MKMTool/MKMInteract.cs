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
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace MKMTool
{

    public class MKMInteract
    {

        public class RequestHelper
        {

            public static XmlDocument makeRequest(string url, string method, string body = null)
            {

                HttpWebRequest request = WebRequest.CreateHttp(url) as HttpWebRequest;
                request.Method = method;

                MKMAuth.OAuthHeader header = new MKMAuth.OAuthHeader();
                request.Headers.Add(HttpRequestHeader.Authorization, header.getAuthorizationHeader(method, url));
                request.Method = method;

                if (body != null)
                {
                    request.ServicePoint.Expect100Continue = false;
                    request.ContentLength = body.Length;
                    request.ContentType = "text/xml";

                    StreamWriter writer = new StreamWriter(request.GetRequestStream());

                    writer.Write(body);
                    writer.Close();
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                XmlDocument doc = new XmlDocument();
                doc.Load(response.GetResponseStream());

                return doc;

            }

            public static string addWantsListBody(string idProduct, string minCondition, string idLanguage, string isFoil, string isAltered, string isPlayset, string isSigned)
            {
                string XMLContent = "<action>addItem</action>" +
                                    "<product>" +
                                    "<idProduct>" + idProduct + "</idProduct>" +
                                    "<count>1</count>" +
                                    "<minCondition>" + minCondition + "</minCondition>" +
                                    "<wishPrice>0.01</wishPrice>" +
                                    "<mailAlert>false</mailAlert>" +
                                    "<idLanguage>" + idLanguage + "</idLanguage>";

                if(isFoil == "Checked")
                    XMLContent += "<isFoil>true</isFoil>";

                if(isSigned == "Checked")
                    XMLContent += "<isSigned>true</isSigned>";

                if(isAltered == "Checked")
                    XMLContent += "<isAltered>true</isAltered>";

                if(isPlayset == "Checked")
                    XMLContent += "<isPlayset>true</isPlayset>";

                XMLContent  +=  "</product>";

                return XMLContent;
            }

            public static string changeStockArticleBody(XmlNode xNode, string sNewPrice)
            {
                string XMLContent = "<article>" +
                                    "<idArticle>" + xNode["idArticle"].InnerText + "</idArticle>" +
                                    "<price>" + sNewPrice + "</price>" +
                                    "<idLanguage>" + xNode["language"]["idLanguage"].InnerText + "</idLanguage>" +
                                    "<comments></comments>" +
                                    "<count>" + xNode["count"].InnerText + "</count>" +
                                    "<condition>" + xNode["condition"].InnerText + "</condition>" +
                                    "</article>";

                return XMLContent;
            }

            public static string deleteWantsListBody(string idWant)
            {
                string XMLContent = "<action>deleteItem</action>" +
                                    "<want>" +
                                    "<idWant>" + idWant + "</idWant>" +
                                    "</want>";

                return XMLContent;
            }

            public static string getRequestBody(string sInnerXML)
            {
                string XMLRequest = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" +
                    "<request>" +
                    sInnerXML +
                    "</request>";

                return XMLRequest;
            }

            public static string addCartBody(string sIDArticle)
            {
                string XMLContent = "<action>add</action>" +
                                    "<article>" +
                                    "<idArticle>" + sIDArticle + "</idArticle>" +
                                    "<amount>1</amount>" +
                                    "</article>";

                return XMLContent;
            }
        }
    }
}
