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
using System.Net;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

public class MKMAuth
{


    /// <summary>
    /// Class encapsulates tokens and secret to create OAuth signatures and return Authorization headers for web requests.
    /// </summary>
    public class OAuthHeader
    {


        /// <summary>App Token</summary>
        protected String appToken = "";
        /// <summary>App Secret</summary>
        protected String appSecret = "";
        /// <summary>Access Token (Class should also implement an AccessToken property to set the value)</summary>
        protected String accessToken = "";
        /// <summary>Access Token Secret (Class should also implement an AccessToken property to set the value)</summary>
        protected String accessSecret = "";
        /// <summary>OAuth Signature Method</summary>
        protected String signatureMethod = "HMAC-SHA1";
        /// <summary>OAuth Version</summary>
        protected String version = "1.0";
        /// <summary>All Header params compiled into a Dictionary</summary>
        protected IDictionary<String, String> headerParams;

        /// <summary>
        /// Constructor
        /// </summary>
        public OAuthHeader()
        {
            XmlDocument xConfigFile = new XmlDocument();

            xConfigFile.Load(@".//config.xml");

            this.appToken = xConfigFile["config" ]["appToken"].InnerText;
            this.appSecret = xConfigFile["config"]["appSecret"].InnerText;
            this.accessToken = xConfigFile["config"]["accessToken"].InnerText;
            this.accessSecret = xConfigFile["config"]["accessSecret"].InnerText;

            // String nonce = Guid.NewGuid().ToString("n");
            String nonce = "53eb1f44909d6";
            // String timestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
            String timestamp = "1407917892";
            /// Initialize all class members
            this.headerParams = new Dictionary<String, String>();
            this.headerParams.Add("oauth_consumer_key", this.appToken);
            this.headerParams.Add("oauth_token", this.accessToken);
            this.headerParams.Add("oauth_nonce", nonce);
            this.headerParams.Add("oauth_timestamp", timestamp);
            this.headerParams.Add("oauth_signature_method", this.signatureMethod);
            this.headerParams.Add("oauth_version", this.version);
        }

        public static SortedDictionary<string, string> ParseQueryString(String query)
        {
            SortedDictionary<string, string> queryParameters = new SortedDictionary<string, string>();
            string[] querySegments = query.Split('&');
            foreach (string segment in querySegments)
            {
                string[] parts = segment.Split('=');
                if (parts.Length > 0)
                {
                    string key = parts[0].Trim(new char[] { '?', ' ' });
                    string val = parts[1].Trim();

                    queryParameters.Add(key, val);
                }
            }

            return queryParameters;
        }

        /// <summary>
        /// Pass request method and URI parameters to get the Authorization header value
        /// </summary>
        /// <param name="method">Request Method</param>
        /// <param name="url">Request URI</param>
        /// <returns>Authorization header value</returns>
        public String getAuthorizationHeader(String method, String url)
        {
            Uri uri = new Uri(url);
            string baseUri = uri.GetLeftPart(System.UriPartial.Path);

            //MessageBox.Show(baseUri);

            /// Add the realm parameter to the header params
            this.headerParams.Add("realm", baseUri);
           
            /// Start composing the base string from the method and request URI
            String baseString = method.ToUpper()
                              + "&"
                              + Uri.EscapeDataString(baseUri)
                              + "&";

            int index = url.IndexOf("?");

            if (index > 0)
            {
                string urlParams = url.Substring(index).Remove(0, 1);

                SortedDictionary<string, string> args = ParseQueryString(urlParams);

                foreach (KeyValuePair< string, string> k in args)
                {
                    this.headerParams.Add(k.Key, k.Value);
                }

            }

            /// Gather, encode, and sort the base string parameters
            SortedDictionary<String, String> encodedParams = new SortedDictionary<String, String>();
            foreach (KeyValuePair<String, String> parameter in this.headerParams)
            {
                if (false == parameter.Key.Equals("realm"))
                {
                    encodedParams.Add(Uri.EscapeDataString(parameter.Key), Uri.EscapeDataString(parameter.Value));
                }
            }

            /// Expand the base string by the encoded parameter=value pairs
            List<String> paramStrings = new List<String>();
            foreach (KeyValuePair<String, String> parameter in encodedParams)
            {
                paramStrings.Add(parameter.Key + "=" + parameter.Value);
            }
            String paramString = Uri.EscapeDataString(String.Join<String>("&", paramStrings));
            baseString += paramString;

            /// Create the OAuth signature
            String signatureKey = Uri.EscapeDataString(this.appSecret) + "&" + Uri.EscapeDataString(this.accessSecret);
            HMAC hasher = HMACSHA1.Create();
            hasher.Key = Encoding.UTF8.GetBytes(signatureKey);
            Byte[] rawSignature = hasher.ComputeHash(Encoding.UTF8.GetBytes(baseString));
            String oAuthSignature = Convert.ToBase64String(rawSignature);

            /// Include the OAuth signature parameter in the header parameters array
            this.headerParams.Add("oauth_signature", oAuthSignature);

            /// Construct the header string
            List<String> headerParamStrings = new List<String>();
            foreach (KeyValuePair<String, String> parameter in this.headerParams)
            {
                headerParamStrings.Add(parameter.Key + "=\"" + parameter.Value + "\"");
            }
            String authHeader = "OAuth " + String.Join<String>(", ", headerParamStrings);

            return authHeader;
        }
    }
}
