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
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace MKMTool
{
    public partial class WantlistEditorView : Form
    {
        private DataTable dj;
        // Column Sort for listView Elements:
        // https://msdn.microsoft.com/en-us/library/ms996467.aspx?f=255&MSPPError=-2147217396

        private int sortColumn = -1;

        public WantlistEditorView()
        {
            InitializeComponent();

            foreach (var Lang in MKMHelpers.dLanguages)
            {
                var item = new MKMHelpers.ComboboxItem();

                item.Text = Lang.Value;
                item.Value = Lang.Key;

                langCombo.Items.Add(item);

                langCombo.SelectedIndex = 0;
            }

            MKMDatabaseManager.Instance.PopulateExpansionsComboBox(ref editionBox);
            editionBox.Sorted = true;

            initWantLists();

            initCardView();

            conditionCombo.SelectedIndex = 3;
        }

        public void initWantLists()
        {
            XmlDocument doc = null;

            try
            {
                doc = MKMInteract.RequestHelper.getWantsLists();
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("initializing want list, editor will be disabled", eError.Message, true);
                addButton.Enabled = false;
                deleteItemButton.Enabled = false;
                return;
            }

            var node = doc.GetElementsByTagName("wantslist");

            if (node.Count > 0)
            {
                foreach (XmlNode nWantlist in node)
                {
                    var item = new MKMHelpers.ComboboxItem();

                    item.Text = nWantlist["name"].InnerText;
                    item.Value = nWantlist["idWantslist"].InnerText;

                    wantListsBox.Items.Add(item);

                    wantListsBox.SelectedIndex = 0;
                }
            }
        }

        public void initCardView()
        {
            cardView.View = View.Details;
            cardView.GridLines = true;
            cardView.FullRowSelect = true;

            cardView.BackColor = Color.WhiteSmoke;

            cardView.Columns.Add("ProduktID", 60);
            cardView.Columns.Add("Card Name", 220);
            cardView.Columns.Add("Edition", 120);

            dj = new DataTable();

            dj = MKMDatabaseManager.JoinDataTables(MKMDatabaseManager.Instance.Inventory, MKMDatabaseManager.Instance.Expansions,
                (row1, row2) => row1.Field<string>("Expansion ID") == row2.Field<string>("idExpansion"));

            foreach (DataRow row in dj.Rows)
            {
                var item = new ListViewItem(row["idProduct"].ToString());

                item.SubItems.Add(row["Name"].ToString());
                item.SubItems.Add(row["enName"].ToString());

                cardView.Items.Add(item);
            }
        }

        private void WantlistEditor_Load(object sender, EventArgs e)
        {
        }

        public void processSearchBox()
        {
            if ((searchBox.Text.Length > 3) || (editionBox.Text != ""))
            {
                cardView.Items.Clear();

                foreach (DataRow row in dj.Rows)
                {
                    var item = new ListViewItem(row["idProduct"].ToString());

                    if (row["Name"].ToString().Contains(searchBox.Text))
                    {
                        //check editon Dropdown
                        if (editionBox.Text != "")
                        {
                            if (row["enName"].ToString() != editionBox.Text)
                            {
                                continue;
                            }
                        }

                        item.SubItems.Add(row["Name"].ToString());
                        item.SubItems.Add(row["enName"].ToString());

                        cardView.Items.Add(item);
                    }
                }
            }
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            processSearchBox();
        }


        private void wantsListBoxReload()
        {
            var sListId = (wantListsBox.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString();

            wantsView.Columns.Clear();

            var ds = MainView.Instance.Bot.buildProperWantsList(sListId);

            if (ds.Select().Length > 0)
            {
                wantsView.AutoGenerateColumns = true;
                wantsView.DataSource = ds;

                wantsView.Refresh();

                wantsView.Columns["idProduct"].Visible = false;
                wantsView.Columns["Expansion ID"].Visible = false;
                wantsView.Columns["Date Added"].Visible = false;
                wantsView.Columns["idExpansion"].Visible = false;
                wantsView.Columns["abbreviation"].Visible = false;
                wantsView.Columns["idWant"].Visible = false;
                wantsView.Columns["item_Id"].Visible = false;
                wantsView.Columns["wantslist_Id"].Visible = false;
                wantsView.Columns["type"].Visible = false;
                wantsView.Columns["wishPrice"].Visible = false;
                wantsView.Columns["count"].Visible = false;

                wantsView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                wantsView.ReadOnly = true;
            }
        }

        private void wantListsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            wantsListBoxReload();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in cardView.SelectedItems)
            {
                var idProduct = item.Text;

                //MessageBox.Show(idProduct);

                //addWantsListBody(string idProduct, string minCondition, string idLanguage, string isFoil, string isAltered, string isPlayset, string isSigned)
                var sRequestXML = MKMInteract.RequestHelper.addWantsListBody(idProduct,
                    conditionCombo.Text,
                    (langCombo.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString(),
                    foilBox.CheckState.ToString(),
                    alteredBox.CheckState.ToString(),
                    playsetBox.CheckState.ToString(),
                    signedBox.CheckState.ToString());

                sRequestXML = MKMInteract.RequestHelper.getRequestBody(sRequestXML);

                try
                {
                    var sListId = (wantListsBox.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString();

                    MKMInteract.RequestHelper.makeRequest("https://api.cardmarket.com/ws/v2.0/wantslist/" + sListId, "PUT",
                        sRequestXML);
                }
                catch (Exception eError)
                {
                    MKMHelpers.LogError("adding to wantlist, product id " + idProduct, eError.Message, true, sRequestXML);
                    return;
                }
            }
            wantsListBoxReload();
        }

        private void deleteItemButton_Click(object sender, EventArgs e)
        {
            var iCellIndex = wantsView.Columns["idWant"].Index;

            if (wantsView.SelectedRows[0].Cells[iCellIndex].Value.ToString() == "")
            {
                MessageBox.Show("Please select the row you want to delete!");
                return;
            }
            var idWant = wantsView.SelectedRows[0].Cells[iCellIndex].Value.ToString();

            var sRequestXML = MKMInteract.RequestHelper.deleteWantsListBody(idWant);
            sRequestXML = MKMInteract.RequestHelper.getRequestBody(sRequestXML);
            var sListId = (wantListsBox.SelectedItem as MKMHelpers.ComboboxItem).Value.ToString();

            try
            {
                MKMInteract.RequestHelper.makeRequest("https://api.cardmarket.com/ws/v2.0/wantslist/" + sListId, "PUT",
                    sRequestXML);
                wantsListBoxReload();
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("deleting from wantlist item #" + idWant, eError.Message, true, sRequestXML);
            }
        }

        private void cardView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //prevent long loading clicks
            if (searchBox.Text.Length < 4)
            {
                return;
            }

            // Determine whether the column is the same as the last column clicked.
            if (e.Column != sortColumn)
            {
                // Set the sort column to the new column.
                sortColumn = e.Column;
                // Set the sort order to ascending by default.
                cardView.Sorting = SortOrder.Ascending;
            }
            else
            {
                // Determine what the last sort order was and change it.
                if (cardView.Sorting == SortOrder.Ascending)
                    cardView.Sorting = SortOrder.Descending;
                else
                    cardView.Sorting = SortOrder.Ascending;
            }

            // Call the sort method to manually sort.
            cardView.Sort();
            // Set the ListViewItemSorter property to a new ListViewItemComparer
            // object.
            cardView.ListViewItemSorter = new ListViewItemComparer(e.Column, cardView.Sorting);
        }

        private void editionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            processSearchBox();
        }

        // Implements the manual sorting of items by columns.
        private class ListViewItemComparer : IComparer
        {
            private readonly int col;
            private readonly SortOrder order;

            public ListViewItemComparer()
            {
                col = 0;
                order = SortOrder.Ascending;
            }

            public ListViewItemComparer(int column, SortOrder order)
            {
                col = column;
                this.order = order;
            }

            public int Compare(object x, object y)
            {
                var returnVal = -1;
                returnVal = string.Compare(((ListViewItem) x).SubItems[col].Text,
                    ((ListViewItem) y).SubItems[col].Text);
                // Determine whether the sort order is descending.
                if (order == SortOrder.Descending)
                    // Invert the value returned by String.Compare.
                    returnVal *= -1;
                return returnVal;
            }
        }
    }
}