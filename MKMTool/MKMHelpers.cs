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
using System.Data;
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
        // My origin country (to find domestic deals)
        public static string sMyOwnCountry = "D";

        // My userId (to disregard items listed by myself when setting a new price)
        public static string sMyId = "0";

        private static DataTable dt = new DataTable();

        /// <summary>
        /// Converts the condition from string to int so that it can be numerically compared.
        /// </summary>
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

        /// <summary>
        /// Determines whether the specified condition is better than the reference condition.
        /// Mint and Near Mint are considered to be the same.
        /// According to API: (MT for Mint > NM for Near Mint > EX for Excellent > GD for Good > LP for Light Played > PL for Played > PO for Poor) 
        /// </summary>
        /// <param name="itemCond">Card condition.</param>
        /// <param name="reference">The reference condition.</param>
        /// <returns>
        ///   <c>true</c> if <c>itemCond</c> is in better or same conditions than the <c>reference</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetterOrSameCondition(String itemCond, String reference)
        {
            return convertCondition(itemCond) >= convertCondition(reference);

        }

        public static Dictionary<string, string> dLanguages = new Dictionary<string, string>
        {
            {
                "",
                "All"
            },
            {
                "1",
                "English"
            },
            {
                "3",
                "German"
            },
            {
                "2",
                "French"
            },
            {
                "4",
                "Spanish"
            },
            {
                "5",
                "Italian"
            },
            {
                "6",
                "Simplified Chinese"
            },
            {
                "7",
                "Japanese"
            },
            {
                "8",
                "Portuguese"
            },
            {
                "9",
                "Russian"
            },
            {
                "10",
                "Korean"
            },
            {
                "11",
                "Traditional Chinese"
            }
        };

        public static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
        
        public static byte[] gzDecompress(byte[] gzip)
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

        /// <summary>
        /// Method for unified logging of exceptions and errors. Writes the error in the application's console, the error log file and
        /// if requested, shows a pop-up window with the error message.
        /// The error message will be "Error with 'subject': 'errorMessage' @ URL: 'sURL'" (URL part is optional and only logged in file).
        /// </summary>
        /// <param name="subject">The variable/item/process that caused the error. The error message will start "Error with 'subject': ...".</param>
        /// <param name="errorMessage">The error message to log.</param>
        /// <param name="popup">If set to <c>true</c>, a pop-up window with the message will be showed. In general, use this for errors
        /// that interrupt the current action completely.</param>
        /// <param name="sURL">When relevant (= in case of exceptions invoked by an API request), include the URL that triggered it. Leave empty for other errors.
        /// Since the URL can be very long, it is never outputted in the console window, only in the file.</param>
        public static void LogError(string subject, string errorMessage, bool popup, string sURL = "")
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