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
        /// <returns>Settings for all parameters of the MKMBot as chosen by the user in the GUI.</returns>
        public MKMBotSettings GenerateBotSettings()
        {
            MKMBotSettings s = new MKMBotSettings();

            s.priceMaxChangeLimits = new SortedList<double, double>();

            string[] limits = textBoxPriceEstMaxChange.Text.Split(';');
            double threshold, allowedChange;
            for (int i = 1; i < limits.Length; i += 2)
            {
                if(double.TryParse(limits[i - 1], out threshold) && double.TryParse(limits[i], out allowedChange))
                    s.priceMaxChangeLimits.Add(threshold, allowedChange);
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
                s.priceFactor = Decimal.ToDouble(numericUpDownPriceEstLowestPrice.Value);
            }
            else
            {
                s.priceSetPriceBy = PriceSetMethod.ByPercentageOfHighestPrice;
                s.priceFactor = Decimal.ToDouble(numericUpDownPriceEstHighestPrice.Value);
            }

            s.priceOutlierLowLimit = Decimal.ToDouble(numericUpDownPriceEstOutliersLow.Value);
            s.priceOutlierUpLimit = Decimal.ToDouble(numericUpDownPriceEstOutliersHigh.Value);

            if (checkBoxCondMatchOnly.Checked)
                s.condAcceptance = AcceptedCondition.OnlyMatching;
            else if (checkBoxCondAcceptBetterAlways.Checked)
                s.condAcceptance = AcceptedCondition.Anything;
            else
            {
                s.condAcceptance = AcceptedCondition.Conditional;
                s.condAtLeastOneMatchAbove = checkBoxCondMatchesAbove.Checked;
                s.condLastMatchSimilarPrice = checkBoxCondSimilarPrice.Checked;
                s.condSimilarPriceLimit = Decimal.ToDouble(numericUpDownCondSimilarPrice.Value);
                s.condBetterOnlyBelowMinItems = checkBoxCondBetterIfBelowMinimum.Checked;
                s.condRequireAllConditions = radioButtonCondUseAND.Checked;
            }
            
            s.logUpdated = checkBoxLogUpdated.Checked;
            s.logLessThanMinimum = checkBoxLogMinItems.Checked;
            s.logSmallPriceChange =checkBoxLogSmallChange.Checked;
            s.logHighPriceChange = checkBoxLogLargeChange.Checked;
            
            s.testMode = checkBoxTestMode.Checked;

            return s;
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
                textBoxPriceEstMaxChange.Text += "" + limitPair.Key + ";" + limitPair.Value;

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
                numericUpDownPriceEstLowestPrice.Value = new decimal(settings.priceFactor);
            }
            else
            {
                radioButtonPriceEstPriceByAvg.Checked = false;
                radioButtonPriceEstByLowestPrice.Checked = false;
                radioButtonPriceEstHighestPrice.Checked = true;
                numericUpDownPriceEstHighestPrice.Value = new decimal(settings.priceFactor);
            }
            numericUpDownPriceEstOutliersLow.Value = new decimal(settings.priceOutlierLowLimit);
            numericUpDownPriceEstOutliersHigh.Value = new decimal(settings.priceOutlierUpLimit);


            if (settings.condAcceptance == AcceptedCondition.OnlyMatching)
            {
                checkBoxCondMatchOnly.Checked = true;
                checkBoxCondAcceptBetterAlways.Checked = false;
                groupBoxCondConditional.Enabled = false;
            }
            else if (settings.condAcceptance == AcceptedCondition.Anything)
            {
                checkBoxCondAcceptBetterAlways.Checked = true;
                checkBoxCondMatchOnly.Checked = false;
                groupBoxCondConditional.Enabled = false;
            }
            else
            {
                checkBoxCondAcceptBetterAlways.Checked = false;
                checkBoxCondMatchOnly.Checked = false;
                groupBoxCondConditional.Enabled = true;
                checkBoxCondMatchesAbove.Checked = settings.condAtLeastOneMatchAbove;
                checkBoxCondSimilarPrice.Checked = settings.condLastMatchSimilarPrice;
                numericUpDownCondSimilarPrice.Value = new decimal(settings.condSimilarPriceLimit);
                checkBoxCondBetterIfBelowMinimum.Checked = settings.condBetterOnlyBelowMinItems;
                radioButtonCondUseAND.Checked = settings.condRequireAllConditions;
                radioButtonCondUseOR.Checked = !settings.condRequireAllConditions;
            }

            checkBoxLogUpdated.Checked = settings.logUpdated;
            checkBoxLogMinItems.Checked = settings.logLessThanMinimum;
            checkBoxLogSmallChange.Checked = settings.logSmallPriceChange;
            checkBoxLogLargeChange.Checked = settings.logHighPriceChange;

            checkBoxTestMode.Checked = settings.testMode;
        }


        private void checkBoxCondSimilarPrice_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownCondSimilarPrice.Enabled = checkBoxCondSimilarPrice.Checked;
        }

        private void radioButtonCondUseAND_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonCondUseOR.Checked = !radioButtonCondUseAND.Checked;
        }

        private void radioButtonCondUseOR_CheckedChanged(object sender, EventArgs e)
        {
            radioButtonCondUseAND.Checked = !radioButtonCondUseOR.Checked;
        }

        private void checkBoxCondMatchOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCondMatchOnly.Checked)
            {
                if (checkBoxCondAcceptBetterAlways.Checked)
                    checkBoxCondAcceptBetterAlways.Checked = false;
                groupBoxCondConditional.Enabled = false;
            }
            else if (!checkBoxCondAcceptBetterAlways.Checked)
                groupBoxCondConditional.Enabled = true;
        }

        private void checkBoxCondAcceptBetterAlways_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCondAcceptBetterAlways.Checked)
            {
                if (checkBoxCondMatchOnly.Checked)
                    checkBoxCondMatchOnly.Checked = false;
                groupBoxCondConditional.Enabled = false;
            }
            else if (!checkBoxCondMatchOnly.Checked)
                groupBoxCondConditional.Enabled = true;
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
            }
            else
            {
                numericUpDownPriceEstLowestPrice.Enabled = false;
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
