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
using System.Xml;

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
        private void StockView_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                int start = 1;
                var articles = new DataTable();
                try
                {
                    while (true)
                    {
                        var doc = MKMInteract.RequestHelper.readStock(start);
                        if (doc.HasChildNodes)
                        {
                            var result = doc.GetElementsByTagName("article");
                            int elementCount = 0;
                            foreach (XmlNode article in result)
                            {
                                if (article["condition"] != null) // is null for articles that are not cards (boosters etc.) - don't process those
                                {
                                    MKMMetaCard m = new MKMMetaCard(article);
                                    m.WriteItselfIntoTable(articles, true, MCFormat.MKM);
                                    elementCount++;
                                }
                            }
                            if (elementCount != 100)
                            {
                                break;
                            }
                            start += elementCount;
                        }
                        else break; // document is empty -> end*/
                    }

                    // Remove columns we don't want showing
                    // TODO - what is and isn't shown should probably be customizable and left to the user to choose in some way
                    articles.Columns.Remove(MCAttribute.ArticleID);
                    articles.Columns.Remove(MCAttribute.ProductID);
                    articles.Columns.Remove(MCAttribute.LanguageID);
                    articles.Columns.Remove(MCAttribute.CardNumber);

                    var dj = MKMDbManager.JoinDataTables(articles, MKMDbManager.Instance.Expansions,
                        (row1, row2) => row1.Field<string>(MKMDbManager.InventoryFields.ExpansionID) == row2.Field<string>(MKMDbManager.ExpansionsFields.ExpansionID));

                    dj.Columns.Remove(MCAttribute.ExpansionID); // duplicated
                    dj.Columns.Remove(MKMDbManager.ExpansionsFields.ExpansionID); // ...and we don't want it anyway
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
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                MainView.Instance.LogMainWindow("Exporting inventory...");
                MKMDbManager.WriteTableAsCSV(sf.FileName, (DataTable)stockGridView.DataSource);
                MainView.Instance.LogMainWindow("Inventory exported.");
            }
        }
    }
}