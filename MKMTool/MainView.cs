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
#undef DEBUG

using System;
using System.IO;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using Timer = System.Timers.Timer;

namespace MKMTool
{
    public partial class MainView : Form
    {
        public delegate void logboxAppendCallback(string text);

        private static readonly Timer timer = new Timer();

        public MainView()
        {
            InitializeComponent();

#if DEBUG
            logBox.AppendText("DEBUG MODE ON!\n");
#endif

            if (!File.Exists(@".\\config.xml"))
            {
                MessageBox.Show("No config file found! Create a config.xml first.");

                Application.Exit();
            }

            if (!File.Exists(@".\\mkminventory.csv"))
            {
                var doc = MKMInteract.RequestHelper.makeRequest("https://www.mkmapi.eu/ws/v2.0/productlist", "GET");

                var node = doc.GetElementsByTagName("response");

                var zipPath = @".\\mkminventory.zip";

                foreach (XmlNode aFile in node)
                {
                    if (aFile["productsfile"].InnerText != null)
                    {
                        var data = Convert.FromBase64String(aFile["productsfile"].InnerText);
                        File.WriteAllBytes(zipPath, data);
                    }
                }

                var file = File.ReadAllBytes(zipPath);
                var aDecompressed = MKMHelpers.gzDecompress(file);

                File.WriteAllBytes(@".\\mkminventory.csv", aDecompressed);
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var ac1 = new AccountInfo();
            ac1.ShowDialog();
        }

        private void readStockButton_Click(object sender, EventArgs e)
        {
            /*           MKMBot bot = new MKMBot();
#if !DEBUG
            bot.getProductList(this);
#endif*/
            var sv1 = new StockView();
            sv1.ShowDialog();
        }

        private void updatePriceButton_Click(object sender, EventArgs e)
        {
            var bot = new MKMBot();

            bot.updatePrices(this);
        }

        private void getProductListButton_Click(object sender, EventArgs e)
        {
            var bot = new MKMBot();

            bot.getProductList(this);
        }

        private void autoUpdateCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (autoUpdateCheck.Checked)
            {
                status.Text = "Bot Mode";

                getProductListButton.Enabled = false;
                loginButton.Enabled = false;
                readStockButton.Enabled = false;
                updatePriceButton.Enabled = false;
                wantlistEditButton.Enabled = false;
                checkDisplayPriceButton.Enabled = false;
                checkWants.Enabled = false;

                runtimeIntervall.Enabled = false;

                logBox.AppendText("Timing MKM Update job every " + Convert.ToInt32(runtimeIntervall.Text) +
                                  " minutes.\n");

                timer.Interval = Convert.ToInt32(runtimeIntervall.Text)*1000*60;

                timer.Elapsed += updatePriceEvent;

                timer.Start();
            }
            else
            {
                runtimeIntervall.Enabled = true;

                logBox.AppendText("Stopping MKM Update job.\n");

                timer.Stop();

                status.Text = "Manual Mode";

                getProductListButton.Enabled = true;
                loginButton.Enabled = true;
                readStockButton.Enabled = true;
                updatePriceButton.Enabled = true;
                wantlistEditButton.Enabled = true;
                checkDisplayPriceButton.Enabled = true;
                checkWants.Enabled = true;
            }
        }

        private void updatePriceEvent(object sender, ElapsedEventArgs e)
        {
            var mainForm = Application.OpenForms["Form1"] != null ? (MainView) Application.OpenForms["Form1"] : null;

            try
            {
                logBox.Invoke(new logboxAppendCallback(logBoxAppend), "Starting scheduled MKM Update Job...\n");
            }
            catch (Exception eError)
            {
                MessageBox.Show(eError.ToString());
            }

            var bot = new MKMBot();

            bot.updatePrices(mainForm);
        }

        public void logBoxAppend(string text)
        {
            logBox.AppendText(text);
        }

        private void wantlistButton_Click(object sender, EventArgs e)
        {
            var wl1 = new WantlistEditorView();
            wl1.ShowDialog();
        }

        private void checkWants_Click(object sender, EventArgs e)
        {
            var cw = new CheckWantsView(this);
            cw.ShowDialog();
        }

        private void checkDisplayPriceButton_Click(object sender, EventArgs e)
        {
            var cw = new CheckDisplayPrices(this);
            cw.ShowDialog();
        }
    }
}