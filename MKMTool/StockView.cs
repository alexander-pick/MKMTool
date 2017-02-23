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
        private readonly DataTable dt = MKMHelpers.ConvertCSVtoDataTable(@".\\mkminventory.csv");

        public StockView()
        {
            InitializeComponent();

            stockGridView.ReadOnly = true;

            try
            {
                var bot = new MKMBot();

                var doc = bot.readStock();

                var xmlReader = new XmlNodeReader(doc);

                var ds = new DataSet();

                ds.ReadXml(xmlReader);

                var dj = MKMHelpers.JoinDataTables(ds.Tables[0], dt,
                    (row1, row2) => row1.Field<string>("idProduct") == row2.Field<string>("idProduct"));

                dj.Columns.Remove("article_Id");
                dj.Columns.Remove("Date Added");
                dj.Columns.Remove("Category ID");
                dj.Columns.Remove("Expansion ID");

                dj.Columns[dj.Columns.IndexOf("Name")].SetOrdinal(0);

                stockGridView.DataSource = dj;
                //dataGridView1.DataSource = dt;
            }
            catch (Exception eError)
            {
                MessageBox.Show(eError.ToString());
            }
        }
    }
}