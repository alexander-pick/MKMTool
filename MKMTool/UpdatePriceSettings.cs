﻿/*
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MKMTool
{
    public partial class UpdatePriceSettings : Form
    {
        // price computation based on average - at 0.5, price will be = to average of similar items, at 0
        // it will be equal to the lowest price, at 1 to the highest price. Remaining values are linear interpolation between either 
        // min price and average (0-0.5) or average and highest price (0.5-1)
        private double priceByAvg = 0.5;
        
        public UpdatePriceSettings()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Instead of closing the settings window when the user presses (X) or ALT+F4, just hide it.
        /// Basically the intended behaviour is for the settings window to act as kind of a singleton object (owned by the main form)
        /// that holds the settings and other parts of the app can read from it.
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

        /// <summary>
        /// Gathers data from the gui and creates their appropriate representation as the MKMBotSettings object.
        /// </summary>
        /// <param name="s">The settings object after the method succesfully executes.</param>
        /// <returns>True if all settings were read, false in case of parsing error.</returns>
        public bool GenerateBotSettings(out MKMBotSettings s)
        {
            s = new MKMBotSettings();
            s.priceMaxChangeLimits = new SortedList<double, double>();

            string[] limits = textBoxPriceEstMaxChange.Text.Split(';');
            double threshold, allowedChange;
            for (int i = 1; i < limits.Length; i += 2)
            {
                if(double.TryParse(limits[i - 1], out threshold) && double.TryParse(limits[i], out allowedChange))
                    s.priceMaxChangeLimits.Add(threshold, allowedChange / 100); // convert to percent
                else
                {
                    MessageBox.Show("The max price change limit pair " + limits[i - 1] + ";" + limits[i]
                        + " could not be parsed as a number.", "Incorrect format of max price change format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            s.priceMaxDifferenceLimits = new SortedList<double, double>();
            limits = textBoxPriceEstMaxDiff.Text.Split(';');
            for (int i = 1; i < limits.Length; i += 2)
            {
                if (double.TryParse(limits[i - 1], out threshold) && double.TryParse(limits[i], out allowedChange))
                    s.priceMaxDifferenceLimits.Add(threshold, allowedChange / 100); // convert to percent
                else
                {
                    MessageBox.Show("The max difference limit pair " + limits[i - 1] + ";" + limits[i]
                        + " could not be parsed as a number.", "Incorrect format max difference between items", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            s.priceMinRarePrice = Decimal.ToDouble(numericUpDownPriceEstMinPrice.Value);
            s.priceMinSimilarItems = Decimal.ToInt32(numericUpDownPriceEstMinN.Value);
            s.priceMaxSimilarItems = Decimal.ToInt32(numericUpDownPriceEstMaxN.Value);
            if (radioButtonPriceEstPriceByAvg.Checked)
            {
                s.priceSetPriceBy = PriceSetMethod.ByAverage;
                s.priceFactor = (double)trackBarPriceEstAvg.Value / (trackBarPriceEstAvg.Maximum - trackBarPriceEstAvg.Minimum);
            }
            else if (radioButtonPriceEstByLowestPrice.Checked)
            {
                s.priceSetPriceBy = PriceSetMethod.ByPercentageOfLowestPrice;
                s.priceFactor = Decimal.ToDouble(numericUpDownPriceEstLowestPrice.Value) / 100;
            }
            else
            {
                s.priceSetPriceBy = PriceSetMethod.ByPercentageOfHighestPrice;
                s.priceFactor = Decimal.ToDouble(numericUpDownPriceEstHighestPrice.Value) / 100;
            }
            
            if (radioButtonCondMatchOnly.Checked)
                s.condAcceptance = AcceptedCondition.OnlyMatching;
            else if (radioButtonCondAcceptBetterAlways.Checked)
                s.condAcceptance = AcceptedCondition.Anything;
            else
                s.condAcceptance = AcceptedCondition.SomeMatchesAbove;
            
            s.logUpdated = checkBoxLogUpdated.Checked;
            s.logLessThanMinimum = checkBoxLogMinItems.Checked;
            s.logSmallPriceChange = checkBoxLogSmallChange.Checked;
            s.logHighPriceChange = checkBoxLogLargeChange.Checked;
            s.logHighPriceVariance = checkBoxLogHighVariance.Checked;

            s.logHighPriceVariance = true;
            
            s.testMode = checkBoxTestMode.Checked;

            return true;
        }

        /// <summary>
        /// Resets all GUI controls to default values.
        /// </summary>
        public void ResetToDefault()
        {
            UpdateSettingsGUI(MKMBot.GenerateDefaultSettings());
        }

        /// <summary>
        /// Updates the GUI controls according to the provided settings.
        /// </summary>
        /// <param name="settings">The settings to which to set the GUI.</param>
        public void UpdateSettingsGUI(MKMBotSettings settings)
        {            
            textBoxPriceEstMaxChange.Text = "";
            foreach (var limitPair in settings.priceMaxChangeLimits)
                textBoxPriceEstMaxChange.Text += "" + limitPair.Key + ";" + (limitPair.Value * 100).ToString("f2");

            textBoxPriceEstMaxDiff.Text = "";
            foreach (var limitPair in settings.priceMaxDifferenceLimits)
                textBoxPriceEstMaxDiff.Text += "" + limitPair.Key + ";" + (limitPair.Value * 100).ToString("f2");

            numericUpDownPriceEstMinPrice.Value = new decimal(settings.priceMinRarePrice);
            numericUpDownPriceEstMinN.Value = new decimal(settings.priceMinSimilarItems);
            numericUpDownPriceEstMaxN.Value = new decimal(settings.priceMaxSimilarItems);
            if (settings.priceSetPriceBy == PriceSetMethod.ByAverage)
            {
                radioButtonPriceEstPriceByAvg.Checked = true;
                radioButtonPriceEstByLowestPrice.Checked = false;
                radioButtonPriceEstHighestPrice.Checked = false;
                trackBarPriceEstAvg.Value = (int)(settings.priceFactor * (trackBarPriceEstAvg.Maximum - trackBarPriceEstAvg.Minimum) + trackBarPriceEstAvg.Minimum);
            }
            else if (settings.priceSetPriceBy == PriceSetMethod.ByPercentageOfLowestPrice)
            {
                radioButtonPriceEstPriceByAvg.Checked = false;
                radioButtonPriceEstByLowestPrice.Checked = true;
                radioButtonPriceEstHighestPrice.Checked = false;
                numericUpDownPriceEstLowestPrice.Value = new decimal(settings.priceFactor * 100);
            }
            else
            {
                radioButtonPriceEstPriceByAvg.Checked = false;
                radioButtonPriceEstByLowestPrice.Checked = false;
                radioButtonPriceEstHighestPrice.Checked = true;
                numericUpDownPriceEstHighestPrice.Value = new decimal(settings.priceFactor * 100);
            }

            if (settings.condAcceptance == AcceptedCondition.OnlyMatching)
            {
                radioButtonCondMatchOnly.Checked = true;
                radioButtonCondAcceptBetterAlways.Checked = false;
                radioButtonCondMatchesAbove.Checked = false;
            }
            else if (settings.condAcceptance == AcceptedCondition.Anything)
            {
                radioButtonCondMatchOnly.Checked = false;
                radioButtonCondAcceptBetterAlways.Checked = true;
                radioButtonCondMatchesAbove.Checked = false;
            }
            else
            {
                radioButtonCondAcceptBetterAlways.Checked = false;
                radioButtonCondMatchOnly.Checked = false;
                radioButtonCondMatchesAbove.Checked = true;
            }

            checkBoxLogUpdated.Checked = settings.logUpdated;
            checkBoxLogMinItems.Checked = settings.logLessThanMinimum;
            checkBoxLogSmallChange.Checked = settings.logSmallPriceChange;
            checkBoxLogLargeChange.Checked = settings.logHighPriceChange;
            checkBoxLogHighVariance.Checked = settings.logHighPriceVariance;

            checkBoxTestMode.Checked = settings.testMode;
        }
                
        private void checkBoxCondMatchOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCondMatchOnly.Checked)
            {
                if (radioButtonCondAcceptBetterAlways.Checked)
                    radioButtonCondAcceptBetterAlways.Checked = false;
            }
        }

        private void checkBoxCondAcceptBetterAlways_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCondAcceptBetterAlways.Checked)
            {
                if (radioButtonCondMatchOnly.Checked)
                    radioButtonCondMatchOnly.Checked = false;
            }
        }

        private void trackBarPriceEstAvg_Scroll(object sender, EventArgs e)
        {
            priceByAvg = (double)trackBarPriceEstAvg.Value / (trackBarPriceEstAvg.Maximum - trackBarPriceEstAvg.Minimum);
            if (priceByAvg == 1)
                labelPriceEstSliderValue.Text = "Max Price";
            else if (priceByAvg > 0.5)
                labelPriceEstSliderValue.Text = "AVG + " + ((priceByAvg - 0.5) * 2).ToString("f2") + " * (Max Price - AVG)";
            else if (priceByAvg == 0)
                labelPriceEstSliderValue.Text = "Min Price";
            else if (priceByAvg < 0.5)
                labelPriceEstSliderValue.Text = "Min Price + " + ((priceByAvg) * 2).ToString("f2") + " * (AVG - Min Price)";
            else
                labelPriceEstSliderValue.Text = "AVG";
        }

        private void radioButtonPriceEstPriceByAvg_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonPriceEstPriceByAvg.Checked)
            {
                trackBarPriceEstAvg.Enabled = true;
                radioButtonPriceEstHighestPrice.Checked = false;
                radioButtonPriceEstByLowestPrice.Checked = false;
            }
            else
            {
                trackBarPriceEstAvg.Enabled = false;
            }
        }

        private void radioButtonPriceByLowestPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonPriceEstByLowestPrice.Checked)
            {
                numericUpDownPriceEstLowestPrice.Enabled = true;
                radioButtonPriceEstHighestPrice.Checked = false;
                radioButtonPriceEstPriceByAvg.Checked = false;
                // if assigning by lowest price, no sequence of prices is collected, only the first lowest one is used
                numericUpDownPriceEstMinN.Enabled = false;
                numericUpDownPriceEstMaxN.Enabled = false;
            }
            else
            {
                numericUpDownPriceEstLowestPrice.Enabled = false;
                numericUpDownPriceEstMinN.Enabled = true;
                numericUpDownPriceEstMaxN.Enabled = true;
            }
        }

        private void radioButtonPriceEstHighestPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonPriceEstHighestPrice.Checked)
            {
                numericUpDownPriceEstHighestPrice.Enabled = true;
                radioButtonPriceEstPriceByAvg.Checked = false;
                radioButtonPriceEstByLowestPrice.Checked = false;
            }
            else
            {
                numericUpDownPriceEstHighestPrice.Enabled = false;
            }
        }

        private void numericUpDownPriceEstMinN_ValueChanged(object sender, EventArgs e)
        {
            // make sure maximum items is not lower than minimum
            if (numericUpDownPriceEstMinN.Value > numericUpDownPriceEstMaxN.Value)
                numericUpDownPriceEstMaxN.Value = numericUpDownPriceEstMinN.Value;
        }

        private void numericUpDownPriceEstMaxN_ValueChanged(object sender, EventArgs e)
        {
            // make sure maximum items is not lower than minimum
            if (numericUpDownPriceEstMaxN.Value < numericUpDownPriceEstMinN.Value)
                numericUpDownPriceEstMinN.Value = numericUpDownPriceEstMaxN.Value;
        }
    }
}
