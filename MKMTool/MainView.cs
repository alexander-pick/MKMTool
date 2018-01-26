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
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Timer = System.Timers.Timer;

namespace MKMTool
{
    public partial class MainView : Form
    {
        public delegate void logboxAppendCallback(string text);

        private static readonly Timer timer = new Timer();

        private UpdatePriceSettings settingsWindow = new UpdatePriceSettings();
        private MKMBot bot = new MKMBot();
        
        public MainView()
        {
            InitializeComponent();

#if DEBUG
            logBox.AppendText("DEBUG MODE ON!\n");
#endif
            try
            {


                if (!File.Exists(@".\\config.xml"))
                {
                    MessageBox.Show("No config file found! Create a config.xml first.");

                    Application.Exit();
                }

                MKMHelpers.GetProductList();
                                
                var doc2 = MKMInteract.RequestHelper.getAccount();

                MKMHelpers.sMyOwnCountry = doc2["response"]["account"]["country"].InnerText;
                MKMHelpers.sMyId = doc2["response"]["account"]["idUser"].InnerText;
            }
            catch (Exception eError)
            {
                MessageBox.Show(eError.Message);
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

        private void updatePriceRun()
        {
            bot.updatePrices(this);
        }

        private async void updatePriceButton_Click(object sender, EventArgs e)
        {
            MKMBotSettings s;
            if (settingsWindow.GenerateBotSettings(out s))
            {
                bot.setSettings(s);
                updatePriceButton.Enabled = false;
                updatePriceButton.Text = "Updating...";
                await Task.Run(() => updatePriceRun());
                updatePriceButton.Text = "Update Prices";
                updatePriceButton.Enabled = true;
            }
            else
                logBox.AppendText("Update abandoned, incorrect setting parameters." + Environment.NewLine);
        }

        private void getProductListButton_Click(object sender, EventArgs e)
        {
            MKMHelpers.GetProductList();
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
                                  " minutes." + Environment.NewLine);

                timer.Interval = Convert.ToInt32(runtimeIntervall.Text) * 1000 * 60;

                timer.Elapsed += updatePriceEvent;

                timer.Start();
            }
            else
            {
                runtimeIntervall.Enabled = true;

                logBox.AppendText("Stopping MKM Update job." + Environment.NewLine);

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
            //var mainForm = Application.OpenForms["Form1"] != null ? (MainView) Application.OpenForms["Form1"] : null;

            try
            {
                logBox.Invoke(new logboxAppendCallback(logBoxAppend), "Starting scheduled MKM Update Job..." + Environment.NewLine);
            }
            catch (Exception eError)
            {
                MessageBox.Show(eError.ToString());
            }

            MKMBotSettings s;
            if (settingsWindow.GenerateBotSettings(out s))
            {
                bot.setSettings(s);
                updatePriceButton.Text = "Updating...";
                bot.updatePrices(this); //mainForm
                updatePriceButton.Text = "Update Prices";
            }
            else
                logBox.AppendText("Update abandoned, incorrect setting parameters." + Environment.NewLine);
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

        private void downloadBuysToExcel_Click(object sender, EventArgs e)
        {
            MKMBotSettings s;
            if (settingsWindow.GenerateBotSettings(out s))
            {
                logBox.AppendText("Downloading Buys data." + Environment.NewLine);
                bot.setSettings(s);

                string sFilename = bot.getBuys(this, "8"); //mainForm

                Process.Start(sFilename);
            }
            else
                logBox.AppendText("Bud data download abandoned, incorrect setting parameters." + Environment.NewLine);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            if (settingsWindow.Visible)
                settingsWindow.Hide();
            else
                settingsWindow.Show(this);
        }
    }
}