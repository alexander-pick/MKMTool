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
using System.Security.Cryptography;
using System.Text;
using System.Xml;

public class MKMAuth
{
    /// <summary>
    ///     Class encapsulates tokens and secret to create OAuth signatures and return Authorization headers for web requests.
    /// </summary>
    public class OAuthHeader
    {
        /// <summary>Access Token Secret (Class should also implement an AccessToken property to set the value)</summary>
        protected string accessSecret = "";

        /// <summary>Access Token (Class should also implement an AccessToken property to set the value)</summary>
        protected string accessToken = "";

        /// <summary>App Secret</summary>
        protected string appSecret = "";


        /// <summary>App Token</summary>
        protected string appToken = "";

        /// <summary>All Header params compiled into a Dictionary</summary>
        protected IDictionary<string, string> headerParams;

        /// <summary>OAuth Signature Method</summary>
        protected string signatureMethod = "HMAC-SHA1";

        /// <summary>OAuth Version</summary>
        protected string version = "1.0";

        /// <summary>
        ///     Constructor
        /// </summary>
        public OAuthHeader()
        {
            var xConfigFile = new XmlDocument();

            xConfigFile.Load(@".//config.xml");

            appToken = xConfigFile["config"]["appToken"].InnerText;
            appSecret = xConfigFile["config"]["appSecret"].InnerText;
            accessToken = xConfigFile["config"]["accessToken"].InnerText;
            accessSecret = xConfigFile["config"]["accessSecret"].InnerText;

            // String nonce = Guid.NewGuid().ToString("n");
            var nonce = "53eb1f44909d6";
            // String timestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
            var timestamp = "1407917892";
            /// Initialize all class members
            headerParams = new Dictionary<string, string>();
            headerParams.Add("oauth_consumer_key", appToken);
            headerParams.Add("oauth_token", accessToken);
            headerParams.Add("oauth_nonce", nonce);
            headerParams.Add("oauth_timestamp", timestamp);
            headerParams.Add("oauth_signature_method", signatureMethod);
            headerParams.Add("oauth_version", version);
        }

        public static SortedDictionary<string, string> ParseQueryString(string query)
        {
            var queryParameters = new SortedDictionary<string, string>();
            var querySegments = query.Split('&');
            foreach (var segment in querySegments)
            {
                var parts = segment.Split('=');
                if (parts.Length > 0)
                {
                    var key = parts[0].Trim('?', ' ');
                    var val = parts[1].Trim();

                    queryParameters.Add(key, val);
                }
            }

            return queryParameters;
        }

        /// <summary>
        ///     Pass request method and URI parameters to get the Authorization header value
        /// </summary>
        /// <param name="method">Request Method</param>
        /// <param name="url">Request URI</param>
        /// <returns>Authorization header value</returns>
        public string getAuthorizationHeader(string method, string url)
        {
            var uri = new Uri(url);
            var baseUri = uri.GetLeftPart(UriPartial.Path);

            //MessageBox.Show(baseUri);

            /// Add the realm parameter to the header params
            headerParams.Add("realm", baseUri);

            /// Start composing the base string from the method and request URI
            var baseString = method.ToUpper()
                             + "&"
                             + Uri.EscapeDataString(baseUri)
                             + "&";

            var index = url.IndexOf("?");

            if (index > 0)
            {
                var urlParams = url.Substring(index).Remove(0, 1);

                var args = ParseQueryString(urlParams);

                foreach (var k in args)
                {
                    headerParams.Add(k.Key, k.Value);
                }
            }

            /// Gather, encode, and sort the base string parameters
            var encodedParams = new SortedDictionary<string, string>();
            foreach (var parameter in headerParams)
            {
                if (false == parameter.Key.Equals("realm"))
                {
                    encodedParams.Add(Uri.EscapeDataString(parameter.Key), Uri.EscapeDataString(parameter.Value));
                }
            }

            /// Expand the base string by the encoded parameter=value pairs
            var paramStrings = new List<string>();
            foreach (var parameter in encodedParams)
            {
                paramStrings.Add(parameter.Key + "=" + parameter.Value);
            }
            var paramString = Uri.EscapeDataString(string.Join<string>("&", paramStrings));
            baseString += paramString;

            /// Create the OAuth signature
            var signatureKey = Uri.EscapeDataString(appSecret) + "&" + Uri.EscapeDataString(accessSecret);
            var hasher = HMAC.Create();
            hasher.Key = Encoding.UTF8.GetBytes(signatureKey);
            var rawSignature = hasher.ComputeHash(Encoding.UTF8.GetBytes(baseString));
            var oAuthSignature = Convert.ToBase64String(rawSignature);

            /// Include the OAuth signature parameter in the header parameters array
            headerParams.Add("oauth_signature", oAuthSignature);

            /// Construct the header string
            var headerParamStrings = new List<string>();
            foreach (var parameter in headerParams)
            {
                headerParamStrings.Add(parameter.Key + "=\"" + parameter.Value + "\"");
            }
            var authHeader = "OAuth " + string.Join<string>(", ", headerParamStrings);

            return authHeader;
        }
    }
}