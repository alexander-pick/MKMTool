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
using System.Data;
using System.Windows.Forms;

namespace MKMTool
{
    public partial class StockView : Form
    {
        public StockView()
        {
            InitializeComponent();

            stockGridView.ReadOnly = true;
        }

        // reload data each time the form is made visible in case the user's stock has changed so they can reload the stockview this way
        private void stockView_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                var articles = new DataTable();
                try
                {
                    // getAllStockSingles creates a DataTable and converts it to list of cards, so theoretically we are wasting some work
                    // but it also filters out non-singles and converting to MKMcard will make sure we use the primary column names rather than synonyms
                    var cards = MKMInteract.RequestHelper.getAllStockSingles(MainView.Instance.Config.UseStockGetFile);
                    if (cards.Count == 0)
                    {
                        MainView.Instance.LogMainWindow("Stock is empty. Did you select correct idGame in config.xml?");
                        return;
                    }
                    foreach (var card in cards)
                    {
                        card.WriteItselfIntoTable(articles, true, MCFormat.MKM, true);
                    }
                    // Remove columns we don't want showing
                    // TODO - what is and isn't shown should probably be customizable and left to the user to choose in some way
                    if (articles.Columns.Contains(MCAttribute.ArticleID))
                        articles.Columns.Remove(MCAttribute.ArticleID);
                    if (articles.Columns.Contains(MCAttribute.LanguageID))
                        articles.Columns.Remove(MCAttribute.LanguageID);
                    if (articles.Columns.Contains(MCAttribute.MetaproductID))
                        articles.Columns.Remove(MCAttribute.MetaproductID);
                    if (articles.Columns.Contains("onSale"))
                        articles.Columns.Remove("onSale"); // don't even know what this one is supposed to be, it's not even in the API documentation
                    if (articles.Columns.Contains(MCAttribute.MKMCurrencyCode))
                        articles.Columns.Remove(MCAttribute.MKMCurrencyCode);
                    if (articles.Columns.Contains(MCAttribute.MKMCurrencyId))
                        articles.Columns.Remove(MCAttribute.MKMCurrencyId);

                    var dj = MKMDbManager.JoinDataTables(articles, MKMDbManager.Instance.Expansions,
                        (row1, row2) => row1.Field<string>(MKMDbManager.InventoryFields.ExpansionID) == row2.Field<string>(MKMDbManager.ExpansionsFields.ExpansionID));

                    if (dj.Columns.Contains(MCAttribute.ExpansionID))
                        dj.Columns.Remove(MCAttribute.ExpansionID); // duplicated
                    if (dj.Columns.Contains(MKMDbManager.ExpansionsFields.ExpansionID))
                        dj.Columns.Remove(MKMDbManager.ExpansionsFields.ExpansionID); // ...and we don't want it anyway
                    if (dj.Columns.Contains(MKMDbManager.ExpansionsFields.Name))
                        dj.Columns.Remove(MKMDbManager.ExpansionsFields.Name); // duplicated
                    
                    dj.Columns[dj.Columns.IndexOf(MCAttribute.Name)].SetOrdinal(0);
                    dj.Columns[dj.Columns.IndexOf(MCAttribute.Expansion)].SetOrdinal(1);
                    dj.Columns[dj.Columns.IndexOf(MCAttribute.Language)].SetOrdinal(2);

                    stockGridView.DataSource = dj;

                    buttonExport.Enabled = true;
                }
                catch (Exception eError)
                {
                    MKMHelpers.LogError("listing stock in Stock View", eError.Message, true);
                }
            }
        }

        /// <summary>
        /// Instead of closing the window when the user presses (X) or ALT+F4, just hide it.
        /// Basically the intended behaviour is for the window to act as kind of a singleton object within the scope of its owner.
        /// </summary>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchString = searchBox.Text.Replace("'", "");
            try
            {
                (stockGridView.DataSource as DataTable).DefaultView.RowFilter =
                    string.Format("Name LIKE '%{0}%'", searchString);
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("searching for " + searchString + " in Stock View", eError.Message, true);
            }

        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog
            {
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
            };
            if (sf.ShowDialog() == DialogResult.OK)
            {
                MainView.Instance.LogMainWindow("Exporting inventory...");
                MKMCsvUtils.WriteTableAsCSV(sf.FileName, (DataTable)stockGridView.DataSource);
                MainView.Instance.LogMainWindow("Inventory exported.");
            }
        }
    }
}