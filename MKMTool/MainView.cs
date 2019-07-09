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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace MKMTool
{
    public partial class MainView : Form
    {
        private delegate void logboxAppendCallback(string text); // use MainView.Instance.LogMainWindow(string) to log messages
        public delegate void updateRequestCountCallback(int requestsPerformed, int requestsLimit);

        private static readonly Timer timer = new Timer();

        private UpdatePriceSettings settingsWindow = new UpdatePriceSettings("LastSettingsPreset", "Settings of Update Price");

        internal MKMBot bot;

        /// <summary>
        /// The price updating bot of the application's main window. Initialized at the start of the application.
        /// </summary>
        /// <value>
        /// The bot.
        /// </value>
        internal MKMBot Bot
        {
            get
            {
                return bot;
            }
        }

        private static MainView instance = null; // singleton instance of the main app window

        /// <summary>
        /// The main application window as a singleton so that it can be easily accessed from anywhere without having to pass it around as method's argument.
        /// Not thread-safe, but we don't care because the first instance is created right at the begging by the main thread.
        /// </summary>
        /// <returns>The main application window</returns>
        public static MainView Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainView();
                    instance.Load += new EventHandler(instance.initialize);
                }
                return instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainView"/> class.
        /// Keep the constructor simple - put any initializations that might call MainView.Instance (which is anything really) in the Initialize() method.
        /// </summary>
        private MainView()
        {
            InitializeComponent();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text = "MKMTool " + fileVersionInfo.ProductVersion + " - Alexander Pick 2017 - Licensed under GPL v3";

#if DEBUG
            logBox.AppendText("DEBUG MODE ON!\n");
#endif

            if (!File.Exists(@".\\config.xml"))
            {
                MessageBox.Show("No config file found! Create a config.xml first.");

                Application.Exit();
            }
        }

        /// <summary>
        /// Initializes the instance of this MainView.
        /// Because error logging mechanism uses the MainView's console, it needs to be called only after the handle for the window
        /// has been created --> during the "Load" event or later (after the form has been created and shown).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void initialize(object sender, EventArgs e)
        {
            timer.Interval = 1440 * 1000 * 60; // set the interval to one day (1440 minutes in ms)
            try
            {
                var doc2 = MKMInteract.RequestHelper.getAccount();

                MKMHelpers.sMyOwnCountry = doc2["response"]["account"]["country"].InnerText;
                MKMHelpers.sMyId = doc2["response"]["account"]["idUser"].InnerText;
            }
            catch (Exception eError)
            {
                MKMHelpers.LogError("initializing product list and account info", eError.Message, true);
            }
            bot = new MKMBot();
        }

        /// <summary>
        /// Logs a messages in the application's main window within the main thread by using Delegate.Invoke
        /// so that it can be safely called from other threads.
        /// This is just a convenience method to shorten the syntax of something as simple as writing a message in a window.
        /// Thread safe (the whole point of it...).
        /// </summary>
        /// <param name="message">The message to log. An new line will be appended at the end of it.</param>
        public void LogMainWindow(string message)
        {
            logBox.Invoke(new logboxAppendCallback(logBoxAppend), message + Environment.NewLine);
        }

        /// <summary>
        /// Appends a given string to the main window's log. 
        /// Not thread safe -> only for internal use, use the public method LogMainWindow from the outside classes.
        /// </summary>
        /// <param name="text">The text.</param>
        private void logBoxAppend(string text)
        {
            logBox.AppendText(text);
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
            bot.updatePrices();
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
            logBoxAppend("Updating local database files..." + Environment.NewLine);
            getProductListButton.Text = "Updating...";
            getProductListButton.Enabled = false;
            if (MKMDatabaseManager.Instance.UpdateDatabaseFiles())
                logBoxAppend("Database created." + Environment.NewLine);
            getProductListButton.Enabled = true;
            getProductListButton.Text = "Update local MKM Product List";
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
            LogMainWindow("Starting scheduled MKM Update Job...");

            MKMBotSettings s;
            if (settingsWindow.GenerateBotSettings(out s))
            {
                bot.setSettings(s);
                updatePriceButton.Text = "Updating...";
                bot.updatePrices(); //mainForm
                updatePriceButton.Text = "Update Prices";
            }
            else
                LogMainWindow("Update abandoned, incorrect setting parameters.");
        }

        private void wantlistButton_Click(object sender, EventArgs e)
        {
            var wl1 = new WantlistEditorView();
            wl1.ShowDialog();
        }

        private void checkWants_Click(object sender, EventArgs e)
        {
            var cw = new CheckWantsView();
            cw.ShowDialog();
        }

        private void checkDisplayPriceButton_Click(object sender, EventArgs e)
        {
            var cw = new CheckDisplayPrices();
            cw.ShowDialog();
        }

        private void downloadBuysToExcel_Click(object sender, EventArgs e)
        {
            logBox.AppendText("Downloading Buys data." + Environment.NewLine);

            string sFilename = MKMBot.getBuys(this, "8"); //mainForm
            if (sFilename != "")
                Process.Start(sFilename);
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            if (settingsWindow.Visible)
                settingsWindow.Hide();
            else
                settingsWindow.Show(this);
        }

        // validate that it is numerical
        private void runtimeIntervall_TextChanged(object sender, EventArgs e)
        {
            int res;
            if (Int32.TryParse(runtimeIntervall.Text, out res))
                timer.Interval = res * 1000 * 60;
            else
                runtimeIntervall.Text = "" + (int)(timer.Interval / 60000);
        }

        /// <summary>
        /// Updates the label on the bottom of the main window that shows the user how many requests they did / how many they are allowed to do.
        /// </summary>
        /// <param name="requestsPerformed">The requests performed since last reset (0:00 CET).</param>
        /// <param name="requestsLimit">The requests limit (based on the account type).</param>
        public void updateRequestCount(int requestsPerformed, int requestsLimit)
        {
            labelRequestCounter.Text = "API Requests made/allowed: " + requestsPerformed + "/" + requestsLimit;
            if (requestsLimit - requestsPerformed < 50)
                labelRequestCounter.ForeColor = System.Drawing.Color.Red;
        }

        private void buttonPriceExternal_Click(object sender, EventArgs e)
        {
            PriceExternalList pxl = new PriceExternalList();
            pxl.ShowDialog();
        }
    }
}