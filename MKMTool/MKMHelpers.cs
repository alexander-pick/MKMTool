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
using System.Data.SQLite;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
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
        /// Converts the condition from string to int as.
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

        // 
        // http://stackoverflow.com/questions/1050112/how-to-read-a-csv-file-into-a-net-datatable

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            if (dt != null && dt.Rows.Count > 0)
                return dt;
            using (var sr = new StreamReader(strFilePath))
            {
                var headers = sr.ReadLine().Split(',');

                foreach (var header in headers)
                    dt.Columns.Add(header.Replace("\"", ""));

                while (!sr.EndOfStream)
                {
                    var rows = sr.ReadLine().Split(',');
                    var dr = dt.NewRow();

                    for (var i = 0; i < headers.Length; i++)
                        dr[i] = rows[i].Replace("\"", "");

                    dt.Rows.Add(dr);
                }
            }

            dt = dt.Select("[Category ID] = '1'").CopyToDataTable(); // grab only MTG Singles

            return dt;
        }

        // Reference:
        // http://stackoverflow.com/questions/665754/inner-join-of-datatables-in-c-sharp

        public static DataTable JoinDataTables(DataTable t1, DataTable t2, params Func<DataRow, DataRow, bool>[] joinOn)
        {
            var result = new DataTable();
            foreach (DataColumn col in t1.Columns)
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            foreach (DataColumn col in t2.Columns)
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            foreach (DataRow row1 in t1.Rows)
            {
                var joinRows = t2.AsEnumerable().Where(row2 =>
                {
                    foreach (var parameter in joinOn)
                        if (!parameter(row1, row2)) return false;
                    return true;
                });
                foreach (var fromRow in joinRows)
                {
                    var insertRow = result.NewRow();
                    foreach (DataColumn col1 in t1.Columns)
                        insertRow[col1.ColumnName] = row1[col1.ColumnName];
                    foreach (DataColumn col2 in t2.Columns)
                        insertRow[col2.ColumnName] = fromRow[col2.ColumnName];
                    result.Rows.Add(insertRow);
                }
            }
            return result;
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

        public static DataTable ReadSQLiteToDt(string sTableName)
        {
            var m_dbConnection = new SQLiteConnection("Data Source=mkmtool.sqlite;Version=3;");

            m_dbConnection.Open();

            var stringQuery = "SELECT * FROM " + sTableName;

            var SqliteCmd = new SQLiteCommand(stringQuery, m_dbConnection);

            SqliteCmd.CommandType = CommandType.Text;

            var da = new SQLiteDataAdapter(SqliteCmd);

            var dt = new DataTable();

            da.Fill(dt);

            return dt;
        }

        public static void BulkInsertDataTable(string tableName, DataTable table, SQLiteConnection m_dbConnection)
        {
            try
            {
                var rgx = new Regex("[^a-zA-Z0-9]");


                using (var transaction = m_dbConnection.BeginTransaction())
                {
                    var sql = new StringBuilder();

                    sql.AppendFormat("INSERT INTO {0} (", tableName);

                    var i = 0;

                    var iQm = "";

                    for (i = 0; i < table.Columns.Count; i++)
                    {
                        sql.AppendFormat(" \"{0}\"", table.Columns[i].ColumnName);

                        iQm = iQm + "@" + rgx.Replace(table.Columns[i].ColumnName, "");

                        if (i != table.Columns.Count - 1)
                        {
                            sql.Append(",");
                            iQm = iQm + ", ";
                        }
                    }

                    sql.Append(") VALUES (" + iQm + ")");

                    var sSql = sql.ToString();

                    foreach (DataRow dtRow in table.Rows)
                    {
                        var insertSQL = new SQLiteCommand(sSql, m_dbConnection);

                        foreach (DataColumn dc in table.Columns)
                            insertSQL.Parameters.Add(new SQLiteParameter("@" + rgx.Replace(dc.ColumnName, ""),
                                dtRow[dc].ToString()));

                        //Console.WriteLine(dtRow[dc].ToString());

                        insertSQL.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }

            }
            catch (Exception Ex)
            {
                MKMHelpers.LogError("bulk insert data table", Ex.Message, true);
            }
        }

        public static string GetCreateTableSql(DataTable table, string tableName)
        {
            var sql = new StringBuilder();

            sql.AppendFormat("CREATE TABLE {0} (", tableName);

            for (var i = 0; i < table.Columns.Count; i++)
            {
                sql.AppendFormat(" \"{0}\"", table.Columns[i].ColumnName);

                switch (table.Columns[i].DataType.ToString().ToUpper())
                {
                    case "SYSTEM.INT16":
                        sql.Append(" integer");
                        break;
                    case "SYSTEM.INT32":
                        sql.Append(" integer");
                        break;
                    case "SYSTEM.INT64":
                        sql.Append(" integer");
                        break;
                    case "SYSTEM.STRING":
                        sql.Append(" varchar(255)");
                        break;
                    case "SYSTEM.SINGLE":
                        sql.Append(" real");
                        break;
                    case "SYSTEM.DOUBLE":
                        sql.Append(" real");
                        break;
                    case "SYSTEM.DECIMAL":
                        sql.Append(" integer");
                        break;
                    default:
                        sql.Append(" varchar(255)");
                        break;
                }

                if (i != table.Columns.Count - 1)
                    sql.Append(",");
            }

            sql.AppendFormat(")");

            return sql.ToString();
        }


        public static void GetProductList()
        {
            if (!File.Exists(@".\\mkminventory.csv"))
            {
                try
                {
                    var doc = MKMInteract.RequestHelper.makeRequest("https://api.cardmarket.com/ws/v2.0/productlist", "GET");

                    var node = doc.GetElementsByTagName("response");

                    var zipPath = @".\\mkminventory.zip";

                    foreach (XmlNode aFile in node)
                        if (aFile["productsfile"].InnerText != null)
                        {
                            var data = Convert.FromBase64String(aFile["productsfile"].InnerText);
                            File.WriteAllBytes(zipPath, data);
                        }

                    var file = File.ReadAllBytes(zipPath);
                    var aDecompressed = gzDecompress(file);

                    File.WriteAllBytes(@".\\mkminventory.csv", aDecompressed);
                }
                catch (Exception eError)
                {
                    MKMHelpers.LogError("parsing mkm inventory, product list cannot be obtained", eError.Message, true);
                    return;
                }
            }

            //db init

            SQLiteConnection m_dbConnection;

            var dt = ConvertCSVtoDataTable(@".\\mkminventory.csv");

            var sql2 = GetCreateTableSql(dt, "inventory");

            Console.WriteLine(sql2);

            if (!File.Exists("mkmtool.sqlite"))
            {
                SQLiteConnection.CreateFile("mkmtool.sqlite");

                m_dbConnection = new SQLiteConnection("Data Source=mkmtool.sqlite;Version=3;");
                m_dbConnection.Open();

                var sql = GetCreateTableSql(dt, "inventory");

                var command = new SQLiteCommand(sql, m_dbConnection);

                command.ExecuteNonQuery();

                sql = "CREATE TABLE expansions (idExpansion, abbreviation, enName)";

                command = new SQLiteCommand(sql, m_dbConnection);

                command.ExecuteNonQuery();
            }
            else
            {
                //clean inventory table
                m_dbConnection = new SQLiteConnection("Data Source=mkmtool.sqlite;Version=3;");
                m_dbConnection.Open();

                var sql = "DELETE FROM inventory";

                var command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                sql = "DELETE FROM expansions";

                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                sql = "VACUUM";

                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }

            BulkInsertDataTable("inventory", dt, m_dbConnection);

            BuildExpansionTable(m_dbConnection);

            m_dbConnection.Close();
        }

        public static void BuildExpansionTable(SQLiteConnection m_dbConnection)
        {
            try
            {
                var doc = MKMInteract.RequestHelper.getExpansions("1"); // Only MTG at present

                var node = doc.GetElementsByTagName("expansion");

                string sSql = "INSERT INTO expansions (idExpansion, abbreviation, enName) VALUES (@idExpansion, @abbreviation, @enName)";

                using (var transaction = m_dbConnection.BeginTransaction())
                {

                    var insertSQL = new SQLiteCommand(sSql, m_dbConnection);

                    foreach (XmlNode nExpansion in node)
                    {

                        insertSQL.Parameters.Add(new SQLiteParameter("@idExpansion", nExpansion["idExpansion"].InnerText));
                        insertSQL.Parameters.Add(new SQLiteParameter("@abbreviation", nExpansion["abbreviation"].InnerText));
                        insertSQL.Parameters.Add(new SQLiteParameter("@enName", nExpansion["enName"].InnerText));

                        insertSQL.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
            catch (Exception Ex)
            {
                LogError("building expansions table", Ex.Message, true);
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
            MainView.Instance().logBox.Invoke(new MainView.logboxAppendCallback(MainView.Instance().logBoxAppend), msg + "\n");

            if (popup)
                MessageBox.Show(msg);
        }

        private static bool firstError = true; // for logging errors, see LogError()

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}