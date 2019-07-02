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
            int start = 1;
            var articles = new DataTable();
            Boolean first = true;
            try
            {
                while (true)
                {
                    var doc = MKMInteract.RequestHelper.readStock(start);
                    if (doc.HasChildNodes)
                    {
                        var xmlReader = new XmlNodeReader(doc);
                        var ds = new DataSet();
                        ds.ReadXml(xmlReader);
                        var articleTable = ds.Tables[0];
                        int elementCount = articleTable.Rows.Count;
                        if (first)
                        {
                            articles = articleTable;
                            first = false;
                        }
                        else
                        {
                            foreach (DataRow dr in articleTable.Rows)
                            {
                                dr["article_Id"] = articles.Rows.Count;
                                articles.ImportRow(dr);
                            }
                        }
                        if (elementCount != 100)
                        {
                            break;
                        }
                        start += elementCount;
                    }
                    else break; // document is empty -> end
                }

                var dj = MKMDatabaseManager.JoinDataTables(articles, MKMDatabaseManager.Instance.Inventory,
                    (row1, row2) => row1.Field<string>("idProduct") == row2.Field<string>("idProduct"));

                dj = MKMDatabaseManager.JoinDataTables(dj, MKMDatabaseManager.Instance.Expansions,
                    (row1, row2) => row1.Field<string>("Expansion ID") == row2.Field<string>("idExpansion"));

                dj.Columns.Remove("article_Id");
                dj.Columns.Remove("Date Added");
                dj.Columns.Remove("Metacard ID");
                dj.Columns.Remove("idArticle");
                dj.Columns.Remove("idProduct");
                dj.Columns.Remove("Expansion ID");
                dj.Columns.Remove("idExpansion");

                dj.Columns[dj.Columns.IndexOf("Name")].SetOrdinal(0);

                stockGridView.DataSource = dj;
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("listing stock in Stock View", eError.Message, true);
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
    }
}