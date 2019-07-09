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
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using static MKMTool.MKMHelpers;

namespace MKMTool
{
    /// <summary>
    /// Class that manages the local databases with products available on MKM.
    /// Intention is to provide other classes all query methods necessary without having to expose the database code outside this class.
    /// Implemented as singleton as there should be only one, but there is no clear ownership.
    /// </summary>
    class MKMDbManager
    {
#region initialization
        private static MKMDbManager instance = new MKMDbManager();

        private DataTable inventory = new DataTable();
        private DataTable expansions = new DataTable(); // all expansions of products in inventory

        /// <summary>
        /// DataTable with all MTG singles in the MKM's inventory.
        /// Each row is a record for one card with the following entries: "idProduct","Name","Expansion ID","Metacard ID","Date Added"
        /// </summary>
        public DataTable Inventory
        {
            get { return inventory; }
        }

        /// <summary>
        /// A "string enum" for fields of the Inventory DataTable.
        /// </summary>
        public class InventoryFields
        {
            public static string ProductID { get { return "idProduct"; } }
            public static string Name { get { return "Name"; } }
            public static string ExpansionID { get { return "Expansion ID"; } }
            public static string MetaproductID { get { return "Metacard ID"; } }
            public static string DateAdded { get { return "Date Added"; } }
        }

        /// <summary>
        /// DataTable with all expansions of MtG in the MKM's inventory.
        /// Each row is a record for one card with the following entries: "idExpansion","abbreviation","enName"
        /// </summary>
        public DataTable Expansions
        {
            get { return expansions; }
        }

        /// <summary>
        /// A "string enum" for fields of the Expansions DataTable.
        /// </summary>
        public class ExpansionsFields
        {
            public static string ExpansionID { get { return "idExpansion"; } }
            public static string Abbreviation { get { return "abbreviation"; } }
            public static string Name { get { return "enName"; } }
            public static string ReleaseDate { get { return "releaseDate"; } }
        }

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit.
        // To ensure thead-safety, source: https://csharpindepth.com/articles/singleton
        static MKMDbManager()
        { }

        public static MKMDbManager Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// On construction we create the databases for inventory and expansions.
        /// The singleton instantiation is already lazy so no need to defer the creation further - it is safe to assume that if something
        /// is creating an instance it will use at least one of the databases.
        /// </summary>
        private MKMDbManager()
        {
            buildDatabase();
        }

        /// <summary>
        /// Fetches the latest database from MKM, stores it as a CSV file and re-loads the internal structures of this database manager with new data.
        /// Also creates the SQL database and stores it as mkmtool.sqlite file.
        /// </summary>
        /// <returns><c>False</c> in case the update failed either due to bad response from MKM or IO problems.</returns>
        public bool UpdateDatabaseFiles()
        {
            try
            {
                // build inventory
                var doc = MKMInteract.RequestHelper.makeRequest("https://api.cardmarket.com/ws/v2.0/productlist", "GET");

                var node = doc.GetElementsByTagName("response");
                // not sure what is the point of zipping the database when we are storing the unzipped one as well, but I will leave it as it is for now
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

                MainView.Instance.LogMainWindow("MKM inventory database updated.");

                // build expansions
                doc = MKMInteract.RequestHelper.getExpansions("1"); // Only MTG at present
                node = doc.GetElementsByTagName("expansion");

                using (StreamWriter exp = new StreamWriter(@".\\mkmexpansions.csv"))
                {
                    exp.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"",
                        ExpansionsFields.ExpansionID, ExpansionsFields.Abbreviation, ExpansionsFields.Name, ExpansionsFields.ReleaseDate));
                    foreach (XmlNode nExpansion in node)
                    {
                        exp.WriteLine("\"" + nExpansion[ExpansionsFields.ExpansionID].InnerText + "\",\"" // put commas around each, in case wizards ever decide to do set with a comma in the name
                            + nExpansion[ExpansionsFields.Abbreviation].InnerText + "\",\"" + nExpansion[ExpansionsFields.Name].InnerText + "\",\""
                            + nExpansion[ExpansionsFields.ReleaseDate].InnerText + "\"");
                    }
                }
                MainView.Instance.LogMainWindow("MKM expansion database updated.");
            }
            catch (Exception eError)
            {
                LogError("downloading MKM inventory", eError.Message, true);
                File.Delete(@".\\mkminventory.csv");
                File.Delete(@".\\mkmexpansions.csv");
                return false;
            }
            try
            {
                inventory = ConvertCSVtoDataTable(@".\\mkminventory.csv").Select("[Category ID] = '1'").CopyToDataTable(); // grab only MTG Singles
                // we know we have only category 1 (Magic Singles), no point in keeping that in memory
                inventory.Columns.Remove("Category ID");
                inventory.Columns.Remove("Category");
                expansions = ConvertCSVtoDataTable(@".\\mkmexpansions.csv"); // grab only MTG Singles
            }
            catch (Exception eError)
            {
                LogError("parsing mkm inventory, product list cannot be obtained", eError.Message, true);
                return false;
            }

            // Store the database as an SQL database. This is currently not actually used anywhere by MKMTool itself,
            // but it has existed in previous versions, might be used by other software and might have other uses in the future, so for now we keep it.
            // However, if something fails, we continue as if nothing happened.
            SQLiteConnection m_dbConnection;
            
            var sql2 = CreateTableSql(inventory, "inventory");

            Console.WriteLine(sql2);

            if (!File.Exists("mkmtool.sqlite"))
            {
                SQLiteConnection.CreateFile("mkmtool.sqlite");

                m_dbConnection = new SQLiteConnection("Data Source=mkmtool.sqlite;Version=3;");
                m_dbConnection.Open();

                var sql = CreateTableSql(inventory, "inventory");

                var command = new SQLiteCommand(sql, m_dbConnection);

                command.ExecuteNonQuery();

                sql = string.Format("CREATE TABLE expansions ({0}, {1}, {2}, {3})",
                    ExpansionsFields.ExpansionID, ExpansionsFields.Abbreviation, ExpansionsFields.Name, ExpansionsFields.ReleaseDate);

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

            BulkInsertDataTable("inventory", inventory, m_dbConnection);

            BulkInsertDataTable("expansions", expansions, m_dbConnection);

            m_dbConnection.Close();

            return true;
        }

        /// <summary>
        /// Checks if the files need to be updated, if yes, calls UpdateDatabaseFiles, otherwise just loads the files.
        /// </summary>
        private void buildDatabase()
        {
            if (!File.Exists(@".\\mkminventory.csv") || !File.Exists(@".\\mkmexpansions.csv"))
            {
                MainView.Instance.LogMainWindow("Local inventory database not found, downloading...");
                if (UpdateDatabaseFiles())
                    MainView.Instance.LogMainWindow("Database created.");
                else return;// updateDatabaseFiles reported the error
            }
            else if ((DateTime.Now - File.GetLastWriteTime(@".\\mkminventory.csv")).TotalDays > 100 ||
                (DateTime.Now - File.GetLastWriteTime(@".\\mkmexpansions.csv")).TotalDays > 100)
            {
                MainView.Instance.LogMainWindow("Local inventory database is more than 100 days old, updating...");
                if (UpdateDatabaseFiles())
                    MainView.Instance.LogMainWindow("Database updated.");
                else return;// updateDatabaseFiles reported the error
            }
            else // just read the files
            {
                try
                {
                    inventory = ConvertCSVtoDataTable(@".\\mkminventory.csv");
                    inventory = inventory.Select("[Category ID] = '1'").CopyToDataTable(); // grab only MTG Singles
                    inventory.Columns.Remove("Category ID"); // we know we have only category 1 (Magic Singles), no point in keeping that in memory
                    inventory.Columns.Remove("Category");
                    expansions = ConvertCSVtoDataTable(@".\\mkmexpansions.csv"); // grab only MTG Singles
                }
                catch (Exception eError)
                {
                    LogError("reading local database files", eError.Message, true);
                }
            }
        }

        #endregion
        #region utilities
        
        /// <summary>
        /// Parses a row in a CSV file.
        /// </summary>
        /// <param name="rowToParse">The row from a CSV file. Assumes it is comma-separated, each value can be
        /// enclosed by double quotes (i.e. the character ") and if a double quote is part of the value, it is escaped by another double quote.
        /// If a field contains an even number of double quotes and nothing else, it will be considered as not enclosed, so a field that contains """""" will
        /// be parsed as """. The only exception is when there are exactly two double quotes, in that case it will be considered an enclosed empty string.
        /// This is done as in practice, excel and similar might enclose the empty fields and the user probably wants to keep them empty.</param>
        /// <returns>A list of the individual parsed values in the order they appear on the row. All enclosing quotes are trimmed
        /// from the value and all escape characters are removed.</returns>
        private static List<string> parseCSVRow(string rowToParse)
        {
            List<string> ret = new List<string>();
            string[] split = rowToParse.Split(',');

            for (int i = 0; i < split.Length; i++)
            {
                string columnValue = split[i];
                // we need to account for entires that have commas in their own name (so the split will split them among multiple 
                // columns, even though they should be in one) and also for entries that can contain quotes.
                // MKM escapes the double quotes by another double quote, so the ending double quote is really ending only if it is not preceded by
                // something else.
                // So far worst case scenario is a card from the Force of Will game called: "I", the pilot. This in the database looks like this:
                // "304732","""I"", the Pilot","1018","Force of Will Single","1775","229401","2017-10-04 17:48:59"
                // so it has a comma, double quote precedes it, so it looks like the entry should end there, but it does not, it is in the middle of the name.
                // in general, double quote is to be considered opening/ending only if there is an odd number of them, otherwise they are escaped
                bool quoteEnclosed = false;
                for (int j = 0; j < columnValue.Length; j++)
                {
                    if (columnValue[j] == '"')
                        quoteEnclosed = !quoteEnclosed;
                    else break;
                }
                if (quoteEnclosed) // starts by a double quote -> can contain comma itself, merge until the last double quote is found
                {
                    while (true)
                    {
                        quoteEnclosed = false;
                        for (int j = columnValue.Length - 1; j >= 0; j--) // check if it ends with an odd number of double quotes
                        {
                            if (columnValue[j] == '"')
                                quoteEnclosed = !quoteEnclosed;
                            else break;
                        }
                        if (!quoteEnclosed) // the closing quote was not found yet
                            columnValue += "," + split[++i]; // append the next column value
                        else break;
                    }
                }
                if (quoteEnclosed)
                    columnValue = columnValue.Substring(1, columnValue.Length - 2);
                columnValue = columnValue.Replace("\"\"", "\""); // un-escape double quotes
                // let's handle one corner case: if somebody is exporting a list from excel and says "enclose each field in double quotes",
                // empty fields will have the value "". Since that is an even number of double quotes, our algorithm will evaluate it as 
                // not being quoteEnclosed even though it actually is an enclosed empty string. After the above replacement, we will now
                // have a string that is a single double quote. If that is the case, replace it with actual empty string.
                // Note that this still does not handle all corner cases, in general, if there is an even number X of double quotes in the field and no other text,
                // it can either be X/2 not-enclosed double quotes, or (X-1)/2 enclosed double quotes, there is no way to tell since we are allowing
                // mixed format (i.e. enclosed and unenclosed in the same file). Hopefully this never has any practical impact.
                if (!quoteEnclosed && columnValue == "\"")
                    columnValue = "";
                ret.Add(columnValue);
            }
            return ret;
        }

        /// <summary>
        /// Writes the table as CSV.
        /// </summary>
        /// <param name="filePath">Path to the file as which to write the table.</param>
        /// <param name="dt">The data table to write.</param>
        public static void WriteTableAsCSV(string filePath, DataTable dt)
        {
            try
            {
                using (StreamWriter exp = new StreamWriter(filePath))
                {
                    // we know there will be at least one column, otherwise there would be no valid imported items and therefore no export enabled
                    string row = "\"" + (dt.Columns[0].ColumnName).Replace("\"", "\"\"") + "\""; // don't forget to escape all " by doubling them
                    for (int i = 1; i < dt.Columns.Count; i++)
                        row += ",\"" + (dt.Columns[i].ColumnName).Replace("\"", "\"\"") + "\"";
                    exp.WriteLine(row);
                    foreach (DataRow card in dt.Rows)
                    {
                        row = "\"" + card[0].ToString().Replace("\"", "\"\"") + "\"";
                        for (int i = 1; i < dt.Columns.Count; i++)
                            row += ",\"" + card[i].ToString().Replace("\"", "\"\"") + "\"";
                        exp.WriteLine(row);
                    }
                }
            }
            catch (Exception eError)
            {
                LogError("writing CSV file " + filePath, eError.Message, true);
            }
        }

        /// <summary>
        /// Converts a CSV file to a DataTable.
        /// http://stackoverflow.com/questions/1050112/how-to-read-a-csv-file-into-a-net-datatable     
        /// </summary>
        /// <param name="strFilePath">Path to the CSV file. It is assumed that the file has a header on the first line with names of the columns.
        /// See parseCSVRow on details on the format of the CSV.</param>
        /// <returns>Each row of the file as a row in the returned DataTable.</returns>
        /// <exception cref="FormatException">
        /// Wrong format of the header of CSV file " + strFilePath + ": " + eError.Message
        /// or
        /// Wrong format of the CSV file on row " + (dt.Rows.Count + 1) + ": " + eError.Message
        /// </exception>
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (var sr = new StreamReader(strFilePath))
            {
                try
                {
                    List<string> headers = parseCSVRow(sr.ReadLine());
                    foreach (string header in headers)
                        dt.Columns.Add(header);
                }
                catch (Exception eError)
                {
                    throw new FormatException("Wrong format of the header of CSV file " + strFilePath + ": " + eError.Message);
                }
                while (!sr.EndOfStream)
                {
                    try
                    {
                        List<string> row = parseCSVRow(sr.ReadLine());
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < row.Count; i++)
                            dr[i] = row[i];
                        dt.Rows.Add(dr);
                    }
                    catch (Exception eError)
                    {
                        // technically it is the (dt.Rows.Count + 1)th row, but in the file the first row is the header so this should
                        // give the user the number of the row in the actual file
                        throw new FormatException("Wrong format of the CSV file on row " + (dt.Rows.Count + 2) + ": " + eError.Message);
                    }
                }
            }

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

        /// <summary>
        /// Fills the provided combobox with MKMHelpers.ComboboxItem for each expansion in the current database.
        /// The Text property of each item will be the English name of the expansion and the Value property the ID of the expansion.
        /// </summary>
        /// <param name="exp">Initialized combobox ready to have it's Item property filled. This does not empty the Item list
        /// and also does not modify anything else - only does exp.Items.Add(ComboboxItem).</param>
        public void PopulateExpansionsComboBox(ref ComboBox exp)
        {
            foreach (DataRow nExpansion in expansions.Rows)
            {
                ComboboxItem item = new ComboboxItem();

                item.Text = nExpansion["enName"].ToString();
                item.Value = nExpansion["idExpansion"].ToString();

                exp.Items.Add(item);
            }
        }

        /// <summary>
        /// Fetches a card from the product database.
        /// </summary>
        /// <param name="idProduct">Product ID of the card - it must be a single MtG cards, other products will not be found. If the card has not been
        /// found, it will attempt to update the local database if it is more than 24 hours old.</param>
        /// <returns>The card info with the entries from the Inventory (use InventoryFields to get names of columns).
        /// Returns null in case the product ID is invalid.</returns>
        public DataRow GetSingleCard(string idProduct)
        {
            DataRow[] result = inventory.Select(string.Format("[{0}] = '{1}'", InventoryFields.ProductID, idProduct));
            if (result.Length == 1) // should always be either 0 or 1 as idProduct is unique
                return result[0];
            else if ((DateTime.Now - File.GetLastWriteTime(@".\\mkminventory.csv")).TotalHours > 24)
            {
                MainView.Instance.LogMainWindow("Card id " + idProduct + " not found in local database, updating database...");
                UpdateDatabaseFiles();
                result = inventory.Select(string.Format("[{0}] = '{1}'", InventoryFields.ProductID, idProduct));
                if (result.Length == 1) // should always be either 0 or 1 as idProduct is unique
                    return result[0];
            }
            return null;
        }

        /// <summary>
        /// Gets all card entires with a given name.
        /// </summary>
        /// <param name="enName">English name of the card.</param>
        /// <returns>For each expansion the card has been printed in, one entry from the Inventory is returned (use InventoryFields to get names of columns).
        /// Empty if nothing found.</returns>
        public DataRow[] GetCardByName(string enName)
        {
            enName = enName.Replace("'", "''"); // escape apostrophes as they are understood as escape characters by SQL
            DataRow[] ret = inventory.Select(string.Format("[{0}] = '{1}'", InventoryFields.Name, enName));
            if (ret.Length > 0) // should always be either 0 or 1 as idProduct is unique
                return ret;
            else if ((DateTime.Now - File.GetLastWriteTime(@".\\mkminventory.csv")).TotalHours > 24)
            {
                MainView.Instance.LogMainWindow("Card " + enName + " not found in local database, updating database...");
                UpdateDatabaseFiles();
                return inventory.Select(string.Format("[{0}] = '{1}'", InventoryFields.Name, enName));
            }
            return new DataRow[0];
        }

        /// <summary>
        /// Returns all cards in the specified expansions.
        /// </summary>
        /// <param name="idExpansion">Expansion's ID.</param>
        /// <returns>Array of card records, each record has the entries from the Inventory (use InventoryFields to get names of columns).
        /// Empty if idExpansion is invalid.</returns>
        public DataRow[] GetCardsInExpansion(string idExpansion)
        {
            DataRow[] ret = inventory.Select(string.Format("[{0}] = '{1}'", InventoryFields.ExpansionID, idExpansion));
            if (ret.Length == 0 && (DateTime.Now - File.GetLastWriteTime(@".\\mkmexpansions.csv")).TotalHours > 24)
            {
                MainView.Instance.LogMainWindow("Expansion id " + idExpansion + " not found in local database, updating database...");
                UpdateDatabaseFiles();
                ret = inventory.Select(string.Format("[{0}] = '{1}'", InventoryFields.ExpansionID, idExpansion));
            }
            return ret;
        }

        /// <summary>
        /// Gets the expansion ID based on the name of the expansion.
        /// </summary>
        /// <param name="expansionName">Name of the expansion in English with capitalized first letters.</param>
        /// <returns>String with the ID or empty string if the expansion was not found.</returns>
        public string GetExpansionID(string expansionName)
        {
            expansionName = expansionName.Replace("'", "''"); // escape apostrophes as they are understood as escape characters by SQL
            DataRow[] ret = expansions.Select(string.Format("[{0}] = '{1}'", ExpansionsFields.Name, expansionName));
            if (ret.Length == 1)
                return ret[0][ExpansionsFields.ExpansionID].ToString();
            else if ((DateTime.Now - File.GetLastWriteTime(@".\\mkmexpansions.csv")).TotalHours > 24)
            {
                MainView.Instance.LogMainWindow("Expansion " + expansionName + " not found in local database, updating database...");
                UpdateDatabaseFiles();
                ret = expansions.Select(string.Format("[{0}] = '{1}'", ExpansionsFields.Name, expansionName));
                if (ret.Length == 1)
                    return ret[0][ExpansionsFields.ExpansionID].ToString();
            }
            return "";
        }

        /// <summary>
        /// Gets the expansion's English name based on the ID of the expansion.
        /// </summary>
        /// <param name="expansionID">ID of the expansion.</param>
        /// <returns>String with the ID or empty string if the expansion was not found.</returns>
        public string GetExpansionName(string expansionID)
        {
            DataRow[] ret = expansions.Select(string.Format("[{0}] = '{1}'", ExpansionsFields.ExpansionID, expansionID));
            if (ret.Length == 1)
                return ret[0][ExpansionsFields.Name].ToString();
            else if ((DateTime.Now - File.GetLastWriteTime(@".\\mkmexpansions.csv")).TotalHours > 24)
            {
                MainView.Instance.LogMainWindow("Expansion id " + expansionID + " not found in local database, updating database...");
                UpdateDatabaseFiles();
                ret = expansions.Select(string.Format("[{0}] = '{1}'", ExpansionsFields.ExpansionID, expansionID));
                if (ret.Length == 1)
                    return ret[0][ExpansionsFields.Name].ToString();
            }
            return "";
        }

        /// <summary>
        /// Gets the product ID based on expansionID and name.
        /// </summary>
        /// <param name="enName">English name of the card.</param>
        /// <param name="expansionID">ID of the expansion.</param>
        /// <returns>Strings of all matching products. Usually it will be exactly one, but can be empty if nothing is found,
        /// and in some cases there can be multiple products of the same name in a single expansion, e.g. basic lands.</returns>
        public string[] GetProductID(string enName, string expansionID)
        {
            enName = enName.Replace("'", "''"); // escape apostrophes as they are understood as escape characters by SQL
            DataRow[] ret = inventory.Select(string.Format("[{0}] = '{1}' AND [Name] = '{2}'", InventoryFields.ExpansionID, expansionID, enName));
            if (ret.Length == 0 && (DateTime.Now - File.GetLastWriteTime(@".\\mkmexpansions.csv")).TotalHours > 24)
            {
                MainView.Instance.LogMainWindow("Product " + enName + " from " + expansionID + " not found in local database, updating database...");
                UpdateDatabaseFiles();
                ret = inventory.Select(string.Format("[{0}] = '{1}' AND [Name] = '{2}'", InventoryFields.ExpansionID, expansionID, enName));
            }
            string[] retStrings = new string[ret.Length];
            for (int i = 0; i < ret.Length; i++)
                retStrings[i] = ret[i][InventoryFields.ProductID].ToString();
            return retStrings;
        }

        /// <summary>
        /// Gets the expansion based on its name.
        /// </summary>
        /// <param name="expansionName">Name of the expansion in English with capitalized first letters.</param>
        /// <returns>Data row from the Expansions, use ExpansionsFields to get the names of the columns. Null in case the expansion is not found.</returns>
        public DataRow GetExpansionByName(string expansionName)
        {
            expansionName = expansionName.Replace("'", "''"); // escape apostrophes as they are understood as escape characters by SQL
            DataRow[] ret = expansions.Select(string.Format("[{0}] = '{1}'", ExpansionsFields.Name, expansionName));
            if (ret.Length == 1)
                return ret[0];
            else return null;
        }

        /// <summary>
        /// Gets the expansion based on its ID.
        /// </summary>
        /// <param name="expansionID">ID of the expansion.</param>
        /// <returns>Data row from the Expansions, use ExpansionsFields to get the names of the columns. Null in case the expansion is not found.</returns>
        public DataRow GetExpansionByID(string expansionID)
        {
            DataRow[] ret = expansions.Select(string.Format("[{0}] = '{1}'", ExpansionsFields.ExpansionID, expansionID));
            if (ret.Length == 1)
                return ret[0];
            else if ((DateTime.Now - File.GetLastWriteTime(@".\\mkmexpansions.csv")).TotalHours > 24)
            {
                MainView.Instance.LogMainWindow("Expansion id " + expansionID + " not found in local database, updating database...");
                UpdateDatabaseFiles();
                ret = expansions.Select(string.Format("[{0}] = '{1}'", ExpansionsFields.ExpansionID, expansionID));
                if (ret.Length == 1)
                    return ret[0];
            }
            return null;
        }

        #endregion
        #region SQL

        // currently not used
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

        // used only to create the sql database file...which is currently not used
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
                LogError("bulk-inserting data into the SQL database (MKMTool will still function correctly)", Ex.Message, false);
            }
        }

        // used only to create the sql database file...which is currently not used
        private string CreateTableSql(DataTable table, string tableName)
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

#endregion

    }
}
