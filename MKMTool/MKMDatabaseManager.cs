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
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using static MKMTool.MKMCsvUtils;
using static MKMTool.MKMHelpers;
using System.Collections.Generic;

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

        /// <summary>
        /// DataTable with all singles in the MKM's inventory.
        /// Each row is a record for one card with the following entries: "idProduct","Name","Expansion ID","Metacard ID","Date Added"
        /// Avoid using this as working with the entire inventory is slow, use InventorySinglesOnly whenever possible.
        /// </summary>
        private DataTable Inventory { get; set; }
        /// <summary>
        /// Subset of Inventory, only single cards from games selected in config.
        /// </summary>
        public DataRow[] InventorySinglesOnly { get; private set; } = { };

        /// <summary>
        /// Hashes the indices of rows in InventorySinglesOnly by the product ID.
        /// This significantly speeds up queries in which we know product id.
        /// </summary>
        private Dictionary<string, int> SinglesByProductId { get; set; }

        /// <summary>
        /// A "string enum" for fields of the Inventory DataTable.
        /// </summary>
        public class InventoryFields
        {
            public static string ProductID { get { return "idProduct"; } }
            public static string Name { get { return "Name"; } }
            public static string ExpansionID { get { return "Expansion ID"; } }
            public static string MetaproductID { get { return "Metacard ID"; } }
            public static string Category { get { return "Category"; } }
            public static string CategoryID { get { return "Category ID"; } }
            public static string DateAdded { get { return "Date Added"; } }
            public static string Rarity { get { return MCAttribute.Rarity; } }
        }

        /// <summary>
        /// DataTable with all expansions of games selected in the config file.
        /// If multiple games are selected, all their expansions are mangled together in this table.
        /// Each row is a record for one card with the following entries: "idExpansion","abbreviation","enName"
        /// </summary>
        public DataTable Expansions { get; private set; } = new DataTable();

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
        // To ensure thread-safety, source: https://csharpindepth.com/articles/singleton
        static MKMDbManager()
        { }

        public static MKMDbManager Instance { get; } = new MKMDbManager();

        /// <summary>
        /// On construction we create the databases for inventory and expansions.
        /// The singleton instantiation is already lazy so no need to defer the creation further - it is safe to assume that if something
        /// is creating an instance it will use at least one of the databases.
        /// </summary>
        private MKMDbManager()
        {
            buildDatabase();
        }

        ~MKMDbManager()
        {
            if (Inventory.GetChanges() != null)
            {
                WriteTableAsCSV(@".\\mkminventory.csv", Inventory);
            }
        }

        private bool isSelectedGame(string categoryID)
        {
            foreach (var gameDesc in MainView.Instance.Config.Games)
            {
                if (categoryID == gameDesc.SinglesCategoryID)
                    return true;
            }
            return false;
        }

        // initializes helper structures for operating only over singles of the selected games
        private void buildSinglesDatabase()
        {
            InventorySinglesOnly = Inventory.AsEnumerable().Where(
                   r => isSelectedGame(r.Field<string>(InventoryFields.CategoryID))).ToArray();
            SinglesByProductId = new Dictionary<string, int>(InventorySinglesOnly.Length);
            for (int i = 0; i < InventorySinglesOnly.Length; i++)
                SinglesByProductId[InventorySinglesOnly[i].Field<string>(InventoryFields.ProductID)] = i;
        }

        /// Fetches the latest database from MKM, stores it as a CSV file and re-loads the internal structures of this database manager with new data.
        /// <returns><c>False</c> in case the update failed either due to bad response from MKM or IO problems.</returns>
        private bool updateInventoryDatabaseFile()
        {
            try
            {
                MainView.Instance.LogMainWindow("Updating MKM inventory database...");
                // build inventory
                var doc = MKMInteract.RequestHelper.makeRequest("https://api.cardmarket.com/ws/v2.0/productlist", "GET");

                var node = doc.GetElementsByTagName("response");
                var data = Convert.FromBase64String(node.Item(0)["productsfile"].InnerText);
                var aDecompressed = GzDecompress(data);
                var downloadedProducts = ConvertCSVtoDataTable(aDecompressed);

                // only join the downloaded with the current version, so that we don't overwrite other
                // data we cache into the Inventory (Rarity) that is not present in the productlist
                // the productList is sorted by productid, so just read the last few that we don't have yet
                // this assumes the previous rows never change, which is the case only if there is no error in them...
                for (int i = Inventory.Rows.Count; i < downloadedProducts.Rows.Count; i++)
                {
                    var insertRow = Inventory.NewRow();
                    foreach (DataColumn col2 in downloadedProducts.Columns)
                    {
                        insertRow[col2.ColumnName] = downloadedProducts.Rows[i][col2.ColumnName];
                    }
                    Inventory.Rows.Add(insertRow);
                }
                WriteTableAsCSV(@".\\mkminventory.csv", Inventory);
                Inventory.AcceptChanges();
                buildSinglesDatabase();
                MainView.Instance.LogMainWindow("MKM inventory database updated.");
            }
            catch (Exception eError)
            {
                LogError("downloading MKM inventory", eError.Message, true);
                return false;
            }
            return true;
        }

        /// Forces the update of all database files and reload of the internal database manager structures.
        /// <returns>False in case of any failure (exceptions are reported inside).</returns>
        public bool UpdateDatabaseFiles()
        {
          if (updateInventoryDatabaseFile())
          {
            foreach (var game in MainView.Instance.Config.Games)
            {
              if (!updateExpansionsDatabaseFiles(game))
                return false;
            }
            loadExpansionDatabase();
            return true;
          }
          return false;
        }

        /// Fetches the latest expansions database for a given game from MKM and stores it as a CSV file.
        /// Does NOT re-load the internal database manager structures - loadExpansionDatabase has to be called once all games are up to date. 
        /// <param name="game">The game for which to load the database.</param>
        /// <returns><c>False</c> in case the update failed either due to bad response from MKM or IO problems.</returns>
        private bool updateExpansionsDatabaseFiles(GameDesc game)
        {
          if (MainView.Instance.Config.Games.Count == 0)
          {
            LogError("updating expansion database", "No games specified in config.xml.", true);
            return false;
          }
          try
          {
            MainView.Instance.LogMainWindow("Updating MKM expansions database...");
            // build expansions
            var doc = MKMInteract.RequestHelper.getExpansions(game.GameID);
            var node = doc.GetElementsByTagName("expansion");

            using (StreamWriter exp = new StreamWriter(@".\\ExpansionsDatabase\\mkmexpansions_" + game.GameID + ".csv"))
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
            MainView.Instance.LogMainWindow("MKM expansions database updated.");
          }
          catch (Exception eError)
          {
            LogError("downloading MKM expansions", eError.Message, true);
            return false;
          }
          return true;
        }

        // ! Already assumes all files are loaded, shows an error otherwise.
        private void loadExpansionDatabase()
        {
          if (MainView.Instance.Config.Games.Count > 0)
          {
            try
            {
              // Expansions will have all expansions of all games merged - this could theoretically make issues if there are
              // two expansions with the same name (each from a different game)
              Expansions = ConvertCSVtoDataTable(@".\\ExpansionsDatabase\\mkmexpansions_"
                + MainView.Instance.Config.Games[0].GameID + ".csv"); // initialize the database by the first game
              for (int i = 1; i < MainView.Instance.Config.Games.Count; i++)
              {
                Expansions.Merge(ConvertCSVtoDataTable(@".\\ExpansionsDatabase\\mkmexpansions_"
                  + MainView.Instance.Config.Games[i].GameID + ".csv"));
              }
            }
            catch (Exception eError)
            {
              LogError("parsing expansion database", eError.Message, true);
            }
          }
        }

        /// <summary>
        /// Checks if the files need to be updated, if yes, calls UpdateDatabaseFiles, otherwise just loads the files.
        /// </summary>
        private void buildDatabase()
        {
            Inventory = new DataTable();
            Inventory.Columns.Add(InventoryFields.ProductID);
            Inventory.Columns.Add(InventoryFields.Name);
            Inventory.Columns.Add(InventoryFields.CategoryID);
            Inventory.Columns.Add(InventoryFields.Category);
            Inventory.Columns.Add(InventoryFields.ExpansionID);
            Inventory.Columns.Add(InventoryFields.MetaproductID);
            Inventory.Columns.Add(InventoryFields.DateAdded);
            Inventory.Columns.Add(InventoryFields.Rarity);
            if (!File.Exists(@".\\mkminventory.csv"))
            {
                MainView.Instance.LogMainWindow("Local inventory database not found, downloading...");
                if (updateInventoryDatabaseFile())
                    MainView.Instance.LogMainWindow("Database created.");
                else return;// updateDatabaseFiles reported the error
            }
            else if ((DateTime.Now - File.GetLastWriteTime(@".\\mkminventory.csv")).TotalDays > 100)
            {
                MainView.Instance.LogMainWindow("Local inventory database is more than 100 days old, updating...");
                if (updateInventoryDatabaseFile())
                    MainView.Instance.LogMainWindow("Database updated.");
                else return;// updateDatabaseFiles reported the error
            }
            else // just read the files
            {
                try
                {
                    Inventory = ConvertCSVtoDataTable(@".\\mkminventory.csv");
                    buildSinglesDatabase();
                    Inventory.AcceptChanges();
                }
                catch (Exception eError)
                {
                    LogError("reading local database files", eError.Message, true);
                }
            }
            // load expansions
            if (!Directory.Exists(@".\\ExpansionsDatabase)"))
              Directory.CreateDirectory(@".\\ExpansionsDatabase");
            foreach (var game in MainView.Instance.Config.Games)
            {
              string filename = @".\\ExpansionsDatabase\\mkmexpansions_" + game.GameID + ".csv";
              if (!File.Exists(filename))
              {
                MainView.Instance.LogMainWindow("Local expansion database for game ID " + game.GameID + " not found, downloading...");
                if (updateExpansionsDatabaseFiles(game))
                  MainView.Instance.LogMainWindow("Database created.");
                else return;// updateExpansionsDatabaseFiles reported the error
              }
              else if ((DateTime.Now - File.GetLastWriteTime(filename)).TotalDays > 100)
              {
                MainView.Instance.LogMainWindow("Local expansion database for game ID " + game.GameID + 
                  " is more than 100 days old, downloading...");
                if (updateExpansionsDatabaseFiles(game))
                  MainView.Instance.LogMainWindow("Database updated.");
                else return;// updateExpansionsDatabaseFiles reported the error
              }
            }
            loadExpansionDatabase();
        }

#endregion
#region utilities

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
        public void PopulateExpansionsComboBox(ComboBox exp)
        {
            foreach (DataRow nExpansion in Expansions.Rows)
            {
                ComboboxItem item = new ComboboxItem
                {
                    Text = nExpansion[ExpansionsFields.Name].ToString(),
                    Value = nExpansion[ExpansionsFields.ExpansionID].ToString()
                };

                exp.Items.Add(item);
            }
        }

        /// <summary>
        /// Writes the specified value for a specified item in the inventory.
        /// If the value is different from current value, marks the inventory as modified 
        /// -> will save it to file before closing the application.
        /// </summary>
        /// <param name="productID">The product identifier.</param>
        /// <param name="inventoryField">The field (column) to which to write the value.</param>
        /// <param name="value">The value to enter in the database.</param>
        public void WriteValueToInventory(string idProduct,
            string inventoryField, string value)
        {
            if (!SinglesByProductId.ContainsKey(idProduct))
            {
                if ((DateTime.Now - File.GetLastWriteTime(@".\\mkminventory.csv")).TotalHours > 24)
                {
                    MainView.Instance.LogMainWindow("Card id " + idProduct + " not found in local database, updating database...");
                    updateInventoryDatabaseFile();
                }
                // check again...in case we did not update the database we will call it twice in a row, but that's ok
                if (!SinglesByProductId.ContainsKey(idProduct))
                {
                    LogError("writing " + inventoryField + " " + value + " for product id " + idProduct,
                        "Specified product ID does not exist.", false);
                    return;
                }
            }
            int index = SinglesByProductId[idProduct];
            if (InventorySinglesOnly[index][inventoryField].ToString() != value)
            {
                InventorySinglesOnly[index][inventoryField] = value;
            }
        }

        /// <summary>
        /// Gets all expansion names.
        /// </summary>
        /// <param name="sortByName">If set to <c>true</c>, the result will be sorted by name.</param>
        /// <returns>List of all expansions in the database.</returns>
        public List<string> GetAllExpansionNames(bool sortByName)
        {
            List<string> exp = new List<string>(Expansions.Rows.Count);
            foreach (DataRow nExpansion in Expansions.Rows)
            {
                exp.Add(nExpansion[ExpansionsFields.Name].ToString());
            }
            if (sortByName)
                exp.Sort();
            return exp;
        }

        /// <summary>
        /// Fetches a card from the product database.
        /// </summary>
        /// <param name="idProduct">Product ID of the card - it must be a single card, other products will not be found.
        /// If the card has not been found, it will attempt to update the local database if it is more than 24 hours old.</param>
        /// <param name="idProduct">Product ID of the card - it must be a single card, other products will not be found.
        /// If the card has not been found, it will attempt to update the local database if it is more than 24 hours old.</param>
        /// <returns>The card info with the entries from the Inventory (use InventoryFields to get names of columns).
        /// Returns null in case the product ID is invalid.</returns>
        public DataRow GetSingleCard(string idProduct)
        {
            if (SinglesByProductId.ContainsKey(idProduct))
                return InventorySinglesOnly[SinglesByProductId[idProduct]];
            else if ((DateTime.Now - File.GetLastWriteTime(@".\\mkminventory.csv")).TotalHours > 24)
            {
                MainView.Instance.LogMainWindow("Card id " + idProduct + " not found in local database, updating database...");
                updateInventoryDatabaseFile();
                if (SinglesByProductId.ContainsKey(idProduct))
                    return InventorySinglesOnly[SinglesByProductId[idProduct]];
            }
            return null;
        }

        /// <summary>
        /// Gets all card entires with a given name.
        /// </summary>
        /// <param name="enName">English name of the card.</param>
        /// <returns>For each expansion the card has been printed in, one entry from the Inventory is returned (use InventoryFields to get names of columns).
        /// Empty if nothing found.</returns>
        public IEnumerable<DataRow> GetCardByName(string enName)
        {
            var ret = InventorySinglesOnly.AsEnumerable().Where(
                r => r.Field<string>(InventoryFields.Name) == enName);
            if (ret.Count() > 0)
                return ret;
            else if ((DateTime.Now - File.GetLastWriteTime(@".\\mkminventory.csv")).TotalHours > 24)
            {
                MainView.Instance.LogMainWindow("Card " + enName + " not found in local database, updating database...");
                updateInventoryDatabaseFile();
                return InventorySinglesOnly.AsEnumerable().Where(r => r.Field<string>(InventoryFields.Name) == enName);
            }
            return new DataRow[0];
        }

        /// <summary>
        /// Updates all the expansions databases if they are more than 24 hours old.
        /// </summary>
        /// <returns>True if at least one database was updated</returns>
        private bool updateExpansionDatabase24hours()
        {
          bool wasUpdated = false;
          foreach (var game in MainView.Instance.Config.Games)
          {
              string filename = @".\\ExpansionsDatabase\\mkmexpansions_" + game.GameID + ".csv";
              if ((DateTime.Now - File.GetLastWriteTime(filename)).TotalHours > 24)
              {
                if (!updateExpansionsDatabaseFiles(game))
                  return false; // there was an error, do not continue
                wasUpdated = true;
              }
          }
          if (wasUpdated)
            loadExpansionDatabase();
          return wasUpdated;
        }

        /// <summary>
        /// Returns all cards in the specified expansions.
        /// </summary>
        /// <param name="idExpansion">Expansion's ID.</param>
        /// <returns>Array of card records, each record has the entries from the Inventory (use InventoryFields to get names of columns).
        /// Empty if idExpansion is invalid.</returns>
        public IEnumerable<DataRow> GetCardsInExpansion(string idExpansion)
        {
            var ret = InventorySinglesOnly.AsEnumerable()
                .Where(r => r.Field<string>(InventoryFields.ExpansionID).Equals(idExpansion));
            if (ret.Count() == 0 && updateExpansionDatabase24hours()) // try again with up to date database
            {
                ret = InventorySinglesOnly.AsEnumerable()
                    .Where(r => r.Field<string>(InventoryFields.ExpansionID).Equals(idExpansion));
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
            DataRow ret = Expansions.AsEnumerable().FirstOrDefault(
                r => r.Field<string>(ExpansionsFields.Name) == expansionName);
            if (ret != default)
                return ret[ExpansionsFields.ExpansionID].ToString();
            else if (updateExpansionDatabase24hours()) // try again with up to date database
      {
                ret = Expansions.AsEnumerable().FirstOrDefault(
                    r => r.Field<string>(ExpansionsFields.Name) == expansionName);
                if (ret != default)
                    return ret[ExpansionsFields.ExpansionID].ToString();
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
            DataRow ret = Expansions.AsEnumerable().FirstOrDefault(
                r => r.Field<string>(ExpansionsFields.ExpansionID) == expansionID);
            if (ret != default)
                return ret[ExpansionsFields.Name].ToString();
            else if (updateExpansionDatabase24hours()) // try again with up to date database
            {
                ret = Expansions.AsEnumerable().FirstOrDefault(
                    r => r.Field<string>(ExpansionsFields.ExpansionID) == expansionID);
                if (ret != default)
                    return ret[ExpansionsFields.Name].ToString();
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
        public string[] GetCardProductID(string enName, string expansionID)
        {
            var ret = InventorySinglesOnly.AsEnumerable().Where(
                r => r.Field<string>(InventoryFields.ExpansionID) == expansionID &&
                     r.Field<string>(InventoryFields.Name) == enName);
            if (ret.Count() == 0 && updateExpansionDatabase24hours()) // try again with up to date database
            {
                ret = InventorySinglesOnly.AsEnumerable().Where(
                    r => r.Field<string>(InventoryFields.ExpansionID) == expansionID &&
                         r.Field<string>(InventoryFields.Name) == enName);
            }
            string[] retStrings = new string[ret.Count()];
            for (int i = 0; i < ret.Count(); i++)
                retStrings[i] = ret.ElementAt(i)[InventoryFields.ProductID].ToString();
            return retStrings;
        }

        /// <summary>
        /// Gets the expansion based on its name.
        /// </summary>
        /// <param name="expansionName">Name of the expansion in English with capitalized first letters.</param>
        /// <returns>Data row from the Expansions, use ExpansionsFields to get the names of the columns. Null in case the expansion is not found.</returns>
        public DataRow GetExpansionByName(string expansionName)
        {
            DataRow ret = Expansions.AsEnumerable().FirstOrDefault(
                r => r.Field<string>(ExpansionsFields.Name) == expansionName);
            if (ret != default)
                return ret;
            else return null;
        }

        /// <summary>
        /// Gets the expansion based on its ID.
        /// </summary>
        /// <param name="expansionID">ID of the expansion.</param>
        /// <returns>Data row from the Expansions, use ExpansionsFields to get the names of the columns. Null in case the expansion is not found.</returns>
        public DataRow GetExpansionByID(string expansionID)
        {
            DataRow ret = Expansions.AsEnumerable().FirstOrDefault(
                r => r.Field<string>(ExpansionsFields.ExpansionID) == expansionID);
            if (ret != default)
                return ret;
            else if (updateExpansionDatabase24hours()) // try again with up to date database
            {
                ret = Expansions.AsEnumerable().FirstOrDefault(
                    r => r.Field<string>(ExpansionsFields.ExpansionID) == expansionID);
                if (ret != default)
                    return ret;
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
