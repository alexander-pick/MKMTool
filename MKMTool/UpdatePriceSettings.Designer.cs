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

namespace MKMTool
{
    partial class UpdatePriceSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxLogSettings = new System.Windows.Forms.GroupBox();
            this.checkBoxLogLargeChange = new System.Windows.Forms.CheckBox();
            this.checkBoxLogSmallChange = new System.Windows.Forms.CheckBox();
            this.checkBoxLogMinItems = new System.Windows.Forms.CheckBox();
            this.checkBoxLogUpdated = new System.Windows.Forms.CheckBox();
            this.groupBoxConditionSettings = new System.Windows.Forms.GroupBox();
            this.groupBoxCondConditional = new System.Windows.Forms.GroupBox();
            this.radioButtonCondUseOR = new System.Windows.Forms.RadioButton();
            this.radioButtonCondUseAND = new System.Windows.Forms.RadioButton();
            this.labelCondBetterConSimilarPrice = new System.Windows.Forms.Label();
            this.checkBoxCondBetterIfBelowMinimum = new System.Windows.Forms.CheckBox();
            this.numericUpDownCondSimilarPrice = new System.Windows.Forms.NumericUpDown();
            this.checkBoxCondSimilarPrice = new System.Windows.Forms.CheckBox();
            this.checkBoxCondMatchesAbove = new System.Windows.Forms.CheckBox();
            this.checkBoxCondAcceptBetterAlways = new System.Windows.Forms.CheckBox();
            this.labelMatchExplanation = new System.Windows.Forms.Label();
            this.checkBoxCondMatchOnly = new System.Windows.Forms.CheckBox();
            this.groupBoxPriceEstim = new System.Windows.Forms.GroupBox();
            this.panelPriceEstForSliderLabel = new System.Windows.Forms.Panel();
            this.labelPriceEstSliderValue = new System.Windows.Forms.Label();
            this.trackBarPriceEstAvg = new System.Windows.Forms.TrackBar();
            this.numericUpDownPriceEstMaxN = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPriceEstMinN = new System.Windows.Forms.NumericUpDown();
            this.labelPriceEstMaxN = new System.Windows.Forms.Label();
            this.labelPriceEstMinN = new System.Windows.Forms.Label();
            this.labelPriceEstHighestPrice = new System.Windows.Forms.Label();
            this.numericUpDownPriceEstHighestPrice = new System.Windows.Forms.NumericUpDown();
            this.radioButtonPriceEstHighestPrice = new System.Windows.Forms.RadioButton();
            this.labelPriceEstLowestPrice = new System.Windows.Forms.Label();
            this.numericUpDownPriceEstLowestPrice = new System.Windows.Forms.NumericUpDown();
            this.radioButtonPriceEstByLowestPrice = new System.Windows.Forms.RadioButton();
            this.radioButtonPriceEstPriceByAvg = new System.Windows.Forms.RadioButton();
            this.textBoxPriceEstMaxChange = new System.Windows.Forms.TextBox();
            this.labelPriceEstMaximumPrice = new System.Windows.Forms.Label();
            this.numericUpDownPriceEstMinPrice = new System.Windows.Forms.NumericUpDown();
            this.labelPriceEstMinPrice = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.labelPriceEstOutliers3 = new System.Windows.Forms.Label();
            this.labelPriceEstOutliers2 = new System.Windows.Forms.Label();
            this.numericUpDownPriceEstOutliersHigh = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPriceEstOutliersLow = new System.Windows.Forms.NumericUpDown();
            this.labelPriceEstAvgOutliers1 = new System.Windows.Forms.Label();
            this.checkBoxTestMode = new System.Windows.Forms.CheckBox();
            this.groupBoxLogSettings.SuspendLayout();
            this.groupBoxConditionSettings.SuspendLayout();
            this.groupBoxCondConditional.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCondSimilarPrice)).BeginInit();
            this.groupBoxPriceEstim.SuspendLayout();
            this.panelPriceEstForSliderLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPriceEstAvg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMaxN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstHighestPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstLowestPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstOutliersHigh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstOutliersLow)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxLogSettings
            // 
            this.groupBoxLogSettings.Controls.Add(this.checkBoxLogLargeChange);
            this.groupBoxLogSettings.Controls.Add(this.checkBoxLogSmallChange);
            this.groupBoxLogSettings.Controls.Add(this.checkBoxLogMinItems);
            this.groupBoxLogSettings.Controls.Add(this.checkBoxLogUpdated);
            this.groupBoxLogSettings.Location = new System.Drawing.Point(13, 413);
            this.groupBoxLogSettings.Name = "groupBoxLogSettings";
            this.groupBoxLogSettings.Size = new System.Drawing.Size(727, 66);
            this.groupBoxLogSettings.TabIndex = 14;
            this.groupBoxLogSettings.TabStop = false;
            this.groupBoxLogSettings.Text = "Log settings";
            // 
            // checkBoxLogLargeChange
            // 
            this.checkBoxLogLargeChange.AutoSize = true;
            this.checkBoxLogLargeChange.Checked = true;
            this.checkBoxLogLargeChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogLargeChange.Location = new System.Drawing.Point(264, 43);
            this.checkBoxLogLargeChange.Name = "checkBoxLogLargeChange";
            this.checkBoxLogLargeChange.Size = new System.Drawing.Size(248, 17);
            this.checkBoxLogLargeChange.TabIndex = 3;
            this.checkBoxLogLargeChange.Text = "Log non-updates due to too large price change";
            this.checkBoxLogLargeChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogSmallChange
            // 
            this.checkBoxLogSmallChange.AutoSize = true;
            this.checkBoxLogSmallChange.Checked = true;
            this.checkBoxLogSmallChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogSmallChange.Location = new System.Drawing.Point(10, 43);
            this.checkBoxLogSmallChange.Name = "checkBoxLogSmallChange";
            this.checkBoxLogSmallChange.Size = new System.Drawing.Size(231, 17);
            this.checkBoxLogSmallChange.TabIndex = 2;
            this.checkBoxLogSmallChange.Text = "Log non-updated due to small price change";
            this.checkBoxLogSmallChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogMinItems
            // 
            this.checkBoxLogMinItems.AutoSize = true;
            this.checkBoxLogMinItems.Checked = true;
            this.checkBoxLogMinItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogMinItems.Location = new System.Drawing.Point(264, 19);
            this.checkBoxLogMinItems.Name = "checkBoxLogMinItems";
            this.checkBoxLogMinItems.Size = new System.Drawing.Size(307, 17);
            this.checkBoxLogMinItems.TabIndex = 1;
            this.checkBoxLogMinItems.Text = "Log non-updates due to less than minimum # of similar items";
            this.checkBoxLogMinItems.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogUpdated
            // 
            this.checkBoxLogUpdated.AutoSize = true;
            this.checkBoxLogUpdated.Checked = true;
            this.checkBoxLogUpdated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogUpdated.Location = new System.Drawing.Point(10, 19);
            this.checkBoxLogUpdated.Name = "checkBoxLogUpdated";
            this.checkBoxLogUpdated.Size = new System.Drawing.Size(85, 17);
            this.checkBoxLogUpdated.TabIndex = 0;
            this.checkBoxLogUpdated.Text = "Log updates";
            this.checkBoxLogUpdated.UseVisualStyleBackColor = true;
            // 
            // groupBoxConditionSettings
            // 
            this.groupBoxConditionSettings.Controls.Add(this.groupBoxCondConditional);
            this.groupBoxConditionSettings.Controls.Add(this.checkBoxCondAcceptBetterAlways);
            this.groupBoxConditionSettings.Controls.Add(this.labelMatchExplanation);
            this.groupBoxConditionSettings.Controls.Add(this.checkBoxCondMatchOnly);
            this.groupBoxConditionSettings.Location = new System.Drawing.Point(13, 253);
            this.groupBoxConditionSettings.Name = "groupBoxConditionSettings";
            this.groupBoxConditionSettings.Size = new System.Drawing.Size(727, 154);
            this.groupBoxConditionSettings.TabIndex = 13;
            this.groupBoxConditionSettings.TabStop = false;
            this.groupBoxConditionSettings.Text = "Accepted condition of similar items";
            // 
            // groupBoxCondConditional
            // 
            this.groupBoxCondConditional.Controls.Add(this.radioButtonCondUseOR);
            this.groupBoxCondConditional.Controls.Add(this.radioButtonCondUseAND);
            this.groupBoxCondConditional.Controls.Add(this.labelCondBetterConSimilarPrice);
            this.groupBoxCondConditional.Controls.Add(this.checkBoxCondBetterIfBelowMinimum);
            this.groupBoxCondConditional.Controls.Add(this.numericUpDownCondSimilarPrice);
            this.groupBoxCondConditional.Controls.Add(this.checkBoxCondSimilarPrice);
            this.groupBoxCondConditional.Controls.Add(this.checkBoxCondMatchesAbove);
            this.groupBoxCondConditional.Enabled = false;
            this.groupBoxCondConditional.Location = new System.Drawing.Point(253, 19);
            this.groupBoxCondConditional.Name = "groupBoxCondConditional";
            this.groupBoxCondConditional.Size = new System.Drawing.Size(468, 115);
            this.groupBoxCondConditional.TabIndex = 8;
            this.groupBoxCondConditional.TabStop = false;
            this.groupBoxCondConditional.Text = "Accept better items conditionally";
            // 
            // radioButtonCondUseOR
            // 
            this.radioButtonCondUseOR.AutoSize = true;
            this.radioButtonCondUseOR.Checked = true;
            this.radioButtonCondUseOR.Location = new System.Drawing.Point(226, 91);
            this.radioButtonCondUseOR.Name = "radioButtonCondUseOR";
            this.radioButtonCondUseOR.Size = new System.Drawing.Size(204, 17);
            this.radioButtonCondUseOR.TabIndex = 13;
            this.radioButtonCondUseOR.TabStop = true;
            this.radioButtonCondUseOR.Text = "Any one checked condition is enough";
            this.radioButtonCondUseOR.UseVisualStyleBackColor = true;
            this.radioButtonCondUseOR.CheckedChanged += new System.EventHandler(this.radioButtonCondUseOR_CheckedChanged);
            // 
            // radioButtonCondUseAND
            // 
            this.radioButtonCondUseAND.AutoSize = true;
            this.radioButtonCondUseAND.Location = new System.Drawing.Point(11, 91);
            this.radioButtonCondUseAND.Name = "radioButtonCondUseAND";
            this.radioButtonCondUseAND.Size = new System.Drawing.Size(209, 17);
            this.radioButtonCondUseAND.TabIndex = 12;
            this.radioButtonCondUseAND.Text = "All checked conditions must be fullfilled";
            this.radioButtonCondUseAND.UseVisualStyleBackColor = true;
            this.radioButtonCondUseAND.CheckedChanged += new System.EventHandler(this.radioButtonCondUseAND_CheckedChanged);
            // 
            // labelCondBetterConSimilarPrice
            // 
            this.labelCondBetterConSimilarPrice.AutoSize = true;
            this.labelCondBetterConSimilarPrice.Location = new System.Drawing.Point(442, 46);
            this.labelCondBetterConSimilarPrice.Name = "labelCondBetterConSimilarPrice";
            this.labelCondBetterConSimilarPrice.Size = new System.Drawing.Size(15, 13);
            this.labelCondBetterConSimilarPrice.TabIndex = 11;
            this.labelCondBetterConSimilarPrice.Text = "%";
            // 
            // checkBoxCondBetterIfBelowMinimum
            // 
            this.checkBoxCondBetterIfBelowMinimum.AutoSize = true;
            this.checkBoxCondBetterIfBelowMinimum.Location = new System.Drawing.Point(11, 68);
            this.checkBoxCondBetterIfBelowMinimum.Name = "checkBoxCondBetterIfBelowMinimum";
            this.checkBoxCondBetterIfBelowMinimum.Size = new System.Drawing.Size(324, 17);
            this.checkBoxCondBetterIfBelowMinimum.TabIndex = 10;
            this.checkBoxCondBetterIfBelowMinimum.Text = "Accept better condition only until minimum # of items is reached";
            this.checkBoxCondBetterIfBelowMinimum.UseVisualStyleBackColor = true;
            // 
            // numericUpDownCondSimilarPrice
            // 
            this.numericUpDownCondSimilarPrice.DecimalPlaces = 2;
            this.numericUpDownCondSimilarPrice.Enabled = false;
            this.numericUpDownCondSimilarPrice.Location = new System.Drawing.Point(370, 44);
            this.numericUpDownCondSimilarPrice.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDownCondSimilarPrice.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCondSimilarPrice.Name = "numericUpDownCondSimilarPrice";
            this.numericUpDownCondSimilarPrice.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownCondSimilarPrice.TabIndex = 9;
            this.numericUpDownCondSimilarPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownCondSimilarPrice.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // checkBoxCondSimilarPrice
            // 
            this.checkBoxCondSimilarPrice.AutoSize = true;
            this.checkBoxCondSimilarPrice.Location = new System.Drawing.Point(11, 45);
            this.checkBoxCondSimilarPrice.Name = "checkBoxCondSimilarPrice";
            this.checkBoxCondSimilarPrice.Size = new System.Drawing.Size(362, 17);
            this.checkBoxCondSimilarPrice.TabIndex = 8;
            this.checkBoxCondSimilarPrice.Text = "Accept better condition if the last cheaper match* is cheaper by at most";
            this.checkBoxCondSimilarPrice.UseVisualStyleBackColor = true;
            this.checkBoxCondSimilarPrice.CheckedChanged += new System.EventHandler(this.checkBoxCondSimilarPrice_CheckedChanged);
            // 
            // checkBoxCondMatchesAbove
            // 
            this.checkBoxCondMatchesAbove.AutoSize = true;
            this.checkBoxCondMatchesAbove.Location = new System.Drawing.Point(11, 22);
            this.checkBoxCondMatchesAbove.Name = "checkBoxCondMatchesAbove";
            this.checkBoxCondMatchesAbove.Size = new System.Drawing.Size(352, 17);
            this.checkBoxCondMatchesAbove.TabIndex = 7;
            this.checkBoxCondMatchesAbove.Text = "Accept better condition if there is at least one more expensive match*";
            this.checkBoxCondMatchesAbove.UseVisualStyleBackColor = true;
            // 
            // checkBoxCondAcceptBetterAlways
            // 
            this.checkBoxCondAcceptBetterAlways.AutoSize = true;
            this.checkBoxCondAcceptBetterAlways.Location = new System.Drawing.Point(10, 64);
            this.checkBoxCondAcceptBetterAlways.Name = "checkBoxCondAcceptBetterAlways";
            this.checkBoxCondAcceptBetterAlways.Size = new System.Drawing.Size(186, 17);
            this.checkBoxCondAcceptBetterAlways.TabIndex = 7;
            this.checkBoxCondAcceptBetterAlways.Text = "Accept better condition whenever";
            this.checkBoxCondAcceptBetterAlways.UseVisualStyleBackColor = true;
            this.checkBoxCondAcceptBetterAlways.CheckedChanged += new System.EventHandler(this.checkBoxCondAcceptBetterAlways_CheckedChanged);
            // 
            // labelMatchExplanation
            // 
            this.labelMatchExplanation.AutoSize = true;
            this.labelMatchExplanation.Location = new System.Drawing.Point(3, 137);
            this.labelMatchExplanation.Name = "labelMatchExplanation";
            this.labelMatchExplanation.Size = new System.Drawing.Size(343, 13);
            this.labelMatchExplanation.TabIndex = 2;
            this.labelMatchExplanation.Text = "*Match = item in exactly the same condition as the one being evaluated";
            // 
            // checkBoxCondMatchOnly
            // 
            this.checkBoxCondMatchOnly.AutoSize = true;
            this.checkBoxCondMatchOnly.Checked = true;
            this.checkBoxCondMatchOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCondMatchOnly.Location = new System.Drawing.Point(10, 41);
            this.checkBoxCondMatchOnly.Name = "checkBoxCondMatchOnly";
            this.checkBoxCondMatchOnly.Size = new System.Drawing.Size(129, 17);
            this.checkBoxCondMatchOnly.TabIndex = 0;
            this.checkBoxCondMatchOnly.Text = "Accept only matches*";
            this.checkBoxCondMatchOnly.UseVisualStyleBackColor = true;
            this.checkBoxCondMatchOnly.CheckedChanged += new System.EventHandler(this.checkBoxCondMatchOnly_CheckedChanged);
            // 
            // groupBoxPriceEstim
            // 
            this.groupBoxPriceEstim.Controls.Add(this.panelPriceEstForSliderLabel);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstOutliers3);
            this.groupBoxPriceEstim.Controls.Add(this.trackBarPriceEstAvg);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstOutliers2);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstOutliersHigh);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstOutliersLow);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstAvgOutliers1);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstHighestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstHighestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.radioButtonPriceEstHighestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstLowestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstLowestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.radioButtonPriceEstByLowestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.radioButtonPriceEstPriceByAvg);
            this.groupBoxPriceEstim.Controls.Add(this.textBoxPriceEstMaxChange);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstMaxN);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMaximumPrice);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstMinN);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstMinPrice);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMaxN);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMinPrice);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMinN);
            this.groupBoxPriceEstim.Controls.Add(this.statusLabel);
            this.groupBoxPriceEstim.Location = new System.Drawing.Point(12, 12);
            this.groupBoxPriceEstim.Name = "groupBoxPriceEstim";
            this.groupBoxPriceEstim.Size = new System.Drawing.Size(728, 235);
            this.groupBoxPriceEstim.TabIndex = 12;
            this.groupBoxPriceEstim.TabStop = false;
            this.groupBoxPriceEstim.Text = "Price estimation";
            // 
            // panelPriceEstForSliderLabel
            // 
            this.panelPriceEstForSliderLabel.Controls.Add(this.labelPriceEstSliderValue);
            this.panelPriceEstForSliderLabel.Location = new System.Drawing.Point(175, 152);
            this.panelPriceEstForSliderLabel.Name = "panelPriceEstForSliderLabel";
            this.panelPriceEstForSliderLabel.Size = new System.Drawing.Size(546, 23);
            this.panelPriceEstForSliderLabel.TabIndex = 37;
            // 
            // labelPriceEstSliderValue
            // 
            this.labelPriceEstSliderValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPriceEstSliderValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPriceEstSliderValue.Location = new System.Drawing.Point(0, 0);
            this.labelPriceEstSliderValue.Name = "labelPriceEstSliderValue";
            this.labelPriceEstSliderValue.Size = new System.Drawing.Size(546, 23);
            this.labelPriceEstSliderValue.TabIndex = 24;
            this.labelPriceEstSliderValue.Text = "AVG";
            this.labelPriceEstSliderValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarPriceEstAvg
            // 
            this.trackBarPriceEstAvg.Location = new System.Drawing.Point(177, 107);
            this.trackBarPriceEstAvg.Maximum = 50;
            this.trackBarPriceEstAvg.Name = "trackBarPriceEstAvg";
            this.trackBarPriceEstAvg.Size = new System.Drawing.Size(546, 45);
            this.trackBarPriceEstAvg.TabIndex = 36;
            this.trackBarPriceEstAvg.Value = 25;
            this.trackBarPriceEstAvg.Scroll += new System.EventHandler(this.trackBarPriceEstAvg_Scroll);
            // 
            // numericUpDownPriceEstMaxN
            // 
            this.numericUpDownPriceEstMaxN.Location = new System.Drawing.Point(651, 47);
            this.numericUpDownPriceEstMaxN.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownPriceEstMaxN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPriceEstMaxN.Name = "numericUpDownPriceEstMaxN";
            this.numericUpDownPriceEstMaxN.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownPriceEstMaxN.TabIndex = 35;
            this.numericUpDownPriceEstMaxN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownPriceEstMaxN.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownPriceEstMaxN.ValueChanged += new System.EventHandler(this.numericUpDownPriceEstMaxN_ValueChanged);
            // 
            // numericUpDownPriceEstMinN
            // 
            this.numericUpDownPriceEstMinN.Location = new System.Drawing.Point(417, 47);
            this.numericUpDownPriceEstMinN.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownPriceEstMinN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPriceEstMinN.Name = "numericUpDownPriceEstMinN";
            this.numericUpDownPriceEstMinN.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownPriceEstMinN.TabIndex = 33;
            this.numericUpDownPriceEstMinN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownPriceEstMinN.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownPriceEstMinN.ValueChanged += new System.EventHandler(this.numericUpDownPriceEstMinN_ValueChanged);
            // 
            // labelPriceEstMaxN
            // 
            this.labelPriceEstMaxN.AutoSize = true;
            this.labelPriceEstMaxN.Location = new System.Drawing.Point(511, 49);
            this.labelPriceEstMaxN.Name = "labelPriceEstMaxN";
            this.labelPriceEstMaxN.Size = new System.Drawing.Size(134, 13);
            this.labelPriceEstMaxN.TabIndex = 32;
            this.labelPriceEstMaxN.Text = "Maximum # of similar items:";
            // 
            // labelPriceEstMinN
            // 
            this.labelPriceEstMinN.AutoSize = true;
            this.labelPriceEstMinN.Location = new System.Drawing.Point(271, 49);
            this.labelPriceEstMinN.Name = "labelPriceEstMinN";
            this.labelPriceEstMinN.Size = new System.Drawing.Size(131, 13);
            this.labelPriceEstMinN.TabIndex = 31;
            this.labelPriceEstMinN.Text = "Minimum # of similar items:";
            // 
            // labelPriceEstHighestPrice
            // 
            this.labelPriceEstHighestPrice.AutoSize = true;
            this.labelPriceEstHighestPrice.Location = new System.Drawing.Point(271, 210);
            this.labelPriceEstHighestPrice.Name = "labelPriceEstHighestPrice";
            this.labelPriceEstHighestPrice.Size = new System.Drawing.Size(325, 13);
            this.labelPriceEstHighestPrice.TabIndex = 32;
            this.labelPriceEstHighestPrice.Text = "% of highest price (among the up to max # of cheapest similar items)";
            // 
            // numericUpDownPriceEstHighestPrice
            // 
            this.numericUpDownPriceEstHighestPrice.DecimalPlaces = 2;
            this.numericUpDownPriceEstHighestPrice.Location = new System.Drawing.Point(195, 208);
            this.numericUpDownPriceEstHighestPrice.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownPriceEstHighestPrice.Name = "numericUpDownPriceEstHighestPrice";
            this.numericUpDownPriceEstHighestPrice.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownPriceEstHighestPrice.TabIndex = 31;
            this.numericUpDownPriceEstHighestPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownPriceEstHighestPrice.Value = new decimal(new int[] {
            105,
            0,
            0,
            0});
            // 
            // radioButtonPriceEstHighestPrice
            // 
            this.radioButtonPriceEstHighestPrice.AutoSize = true;
            this.radioButtonPriceEstHighestPrice.Location = new System.Drawing.Point(9, 208);
            this.radioButtonPriceEstHighestPrice.Name = "radioButtonPriceEstHighestPrice";
            this.radioButtonPriceEstHighestPrice.Size = new System.Drawing.Size(180, 17);
            this.radioButtonPriceEstHighestPrice.TabIndex = 30;
            this.radioButtonPriceEstHighestPrice.TabStop = true;
            this.radioButtonPriceEstHighestPrice.Text = "Set price based on highest price:";
            this.radioButtonPriceEstHighestPrice.UseVisualStyleBackColor = true;
            this.radioButtonPriceEstHighestPrice.CheckedChanged += new System.EventHandler(this.radioButtonPriceEstHighestPrice_CheckedChanged);
            // 
            // labelPriceEstLowestPrice
            // 
            this.labelPriceEstLowestPrice.AutoSize = true;
            this.labelPriceEstLowestPrice.Location = new System.Drawing.Point(271, 180);
            this.labelPriceEstLowestPrice.Name = "labelPriceEstLowestPrice";
            this.labelPriceEstLowestPrice.Size = new System.Drawing.Size(86, 13);
            this.labelPriceEstLowestPrice.TabIndex = 29;
            this.labelPriceEstLowestPrice.Text = "% of lowest price";
            // 
            // numericUpDownPriceEstLowestPrice
            // 
            this.numericUpDownPriceEstLowestPrice.DecimalPlaces = 2;
            this.numericUpDownPriceEstLowestPrice.Location = new System.Drawing.Point(195, 178);
            this.numericUpDownPriceEstLowestPrice.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownPriceEstLowestPrice.Name = "numericUpDownPriceEstLowestPrice";
            this.numericUpDownPriceEstLowestPrice.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownPriceEstLowestPrice.TabIndex = 28;
            this.numericUpDownPriceEstLowestPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownPriceEstLowestPrice.Value = new decimal(new int[] {
            95,
            0,
            0,
            0});
            // 
            // radioButtonPriceEstByLowestPrice
            // 
            this.radioButtonPriceEstByLowestPrice.AutoSize = true;
            this.radioButtonPriceEstByLowestPrice.Location = new System.Drawing.Point(9, 178);
            this.radioButtonPriceEstByLowestPrice.Name = "radioButtonPriceEstByLowestPrice";
            this.radioButtonPriceEstByLowestPrice.Size = new System.Drawing.Size(176, 17);
            this.radioButtonPriceEstByLowestPrice.TabIndex = 26;
            this.radioButtonPriceEstByLowestPrice.TabStop = true;
            this.radioButtonPriceEstByLowestPrice.Text = "Set price based on lowest price:";
            this.radioButtonPriceEstByLowestPrice.UseVisualStyleBackColor = true;
            this.radioButtonPriceEstByLowestPrice.CheckedChanged += new System.EventHandler(this.radioButtonPriceByLowestPrice_CheckedChanged);
            // 
            // radioButtonPriceEstPriceByAvg
            // 
            this.radioButtonPriceEstPriceByAvg.AutoSize = true;
            this.radioButtonPriceEstPriceByAvg.Checked = true;
            this.radioButtonPriceEstPriceByAvg.Location = new System.Drawing.Point(9, 109);
            this.radioButtonPriceEstPriceByAvg.Name = "radioButtonPriceEstPriceByAvg";
            this.radioButtonPriceEstPriceByAvg.Size = new System.Drawing.Size(159, 17);
            this.radioButtonPriceEstPriceByAvg.TabIndex = 25;
            this.radioButtonPriceEstPriceByAvg.TabStop = true;
            this.radioButtonPriceEstPriceByAvg.Text = "Set price based on average:";
            this.radioButtonPriceEstPriceByAvg.UseVisualStyleBackColor = true;
            this.radioButtonPriceEstPriceByAvg.CheckedChanged += new System.EventHandler(this.radioButtonPriceEstPriceByAvg_CheckedChanged);
            // 
            // textBoxPriceEstMaxChange
            // 
            this.textBoxPriceEstMaxChange.Location = new System.Drawing.Point(530, 18);
            this.textBoxPriceEstMaxChange.Name = "textBoxPriceEstMaxChange";
            this.textBoxPriceEstMaxChange.Size = new System.Drawing.Size(191, 20);
            this.textBoxPriceEstMaxChange.TabIndex = 18;
            // 
            // labelPriceEstMaximumPrice
            // 
            this.labelPriceEstMaximumPrice.AutoSize = true;
            this.labelPriceEstMaximumPrice.Location = new System.Drawing.Point(6, 21);
            this.labelPriceEstMaximumPrice.Name = "labelPriceEstMaximumPrice";
            this.labelPriceEstMaximumPrice.Size = new System.Drawing.Size(518, 13);
            this.labelPriceEstMaximumPrice.TabIndex = 17;
            this.labelPriceEstMaximumPrice.Text = "Max price change (format: \"T1;C1;T2;C2;\" etc., Cx is max allowed change in % for " +
    "items that cost Tx or less):";
            // 
            // numericUpDownPriceEstMinPrice
            // 
            this.numericUpDownPriceEstMinPrice.DecimalPlaces = 2;
            this.numericUpDownPriceEstMinPrice.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDownPriceEstMinPrice.Location = new System.Drawing.Point(195, 47);
            this.numericUpDownPriceEstMinPrice.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownPriceEstMinPrice.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownPriceEstMinPrice.Name = "numericUpDownPriceEstMinPrice";
            this.numericUpDownPriceEstMinPrice.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownPriceEstMinPrice.TabIndex = 16;
            this.numericUpDownPriceEstMinPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownPriceEstMinPrice.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // labelPriceEstMinPrice
            // 
            this.labelPriceEstMinPrice.AutoSize = true;
            this.labelPriceEstMinPrice.Location = new System.Drawing.Point(6, 49);
            this.labelPriceEstMinPrice.Name = "labelPriceEstMinPrice";
            this.labelPriceEstMinPrice.Size = new System.Drawing.Size(147, 13);
            this.labelPriceEstMinPrice.TabIndex = 15;
            this.labelPriceEstMinPrice.Text = "Minimum price of rares [EUR]:";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(2, 182);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 13);
            this.statusLabel.TabIndex = 8;
            // 
            // labelPriceEstOutliers3
            // 
            this.labelPriceEstOutliers3.AutoSize = true;
            this.labelPriceEstOutliers3.Location = new System.Drawing.Point(493, 79);
            this.labelPriceEstOutliers3.Name = "labelPriceEstOutliers3";
            this.labelPriceEstOutliers3.Size = new System.Drawing.Size(160, 13);
            this.labelPriceEstOutliers3.TabIndex = 54;
            this.labelPriceEstOutliers3.Text = "% of median price of similar items";
            // 
            // labelPriceEstOutliers2
            // 
            this.labelPriceEstOutliers2.AutoSize = true;
            this.labelPriceEstOutliers2.Location = new System.Drawing.Point(271, 79);
            this.labelPriceEstOutliers2.Name = "labelPriceEstOutliers2";
            this.labelPriceEstOutliers2.Size = new System.Drawing.Size(140, 13);
            this.labelPriceEstOutliers2.TabIndex = 53;
            this.labelPriceEstOutliers2.Text = "% and more expensive than ";
            // 
            // numericUpDownPriceEstOutliersHigh
            // 
            this.numericUpDownPriceEstOutliersHigh.DecimalPlaces = 2;
            this.numericUpDownPriceEstOutliersHigh.Location = new System.Drawing.Point(417, 77);
            this.numericUpDownPriceEstOutliersHigh.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDownPriceEstOutliersHigh.Name = "numericUpDownPriceEstOutliersHigh";
            this.numericUpDownPriceEstOutliersHigh.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownPriceEstOutliersHigh.TabIndex = 52;
            this.numericUpDownPriceEstOutliersHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownPriceEstOutliersHigh.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // numericUpDownPriceEstOutliersLow
            // 
            this.numericUpDownPriceEstOutliersLow.DecimalPlaces = 2;
            this.numericUpDownPriceEstOutliersLow.Location = new System.Drawing.Point(195, 77);
            this.numericUpDownPriceEstOutliersLow.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numericUpDownPriceEstOutliersLow.Name = "numericUpDownPriceEstOutliersLow";
            this.numericUpDownPriceEstOutliersLow.Size = new System.Drawing.Size(70, 20);
            this.numericUpDownPriceEstOutliersLow.TabIndex = 51;
            this.numericUpDownPriceEstOutliersLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownPriceEstOutliersLow.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // labelPriceEstAvgOutliers1
            // 
            this.labelPriceEstAvgOutliers1.AutoSize = true;
            this.labelPriceEstAvgOutliers1.Location = new System.Drawing.Point(6, 79);
            this.labelPriceEstAvgOutliers1.Name = "labelPriceEstAvgOutliers1";
            this.labelPriceEstAvgOutliers1.Size = new System.Drawing.Size(188, 13);
            this.labelPriceEstAvgOutliers1.TabIndex = 48;
            this.labelPriceEstAvgOutliers1.Text = "Remove outliers that are cheaper than";
            // 
            // checkBoxTestMode
            // 
            this.checkBoxTestMode.AutoSize = true;
            this.checkBoxTestMode.Location = new System.Drawing.Point(23, 486);
            this.checkBoxTestMode.Name = "checkBoxTestMode";
            this.checkBoxTestMode.Size = new System.Drawing.Size(248, 17);
            this.checkBoxTestMode.TabIndex = 15;
            this.checkBoxTestMode.Text = "Test mode - do not send price updates to MKM";
            this.checkBoxTestMode.UseVisualStyleBackColor = true;
            // 
            // UpdatePriceSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 515);
            this.Controls.Add(this.checkBoxTestMode);
            this.Controls.Add(this.groupBoxLogSettings);
            this.Controls.Add(this.groupBoxConditionSettings);
            this.Controls.Add(this.groupBoxPriceEstim);
            this.Name = "UpdatePriceSettings";
            this.Text = "Settings of Update Price";
            this.groupBoxLogSettings.ResumeLayout(false);
            this.groupBoxLogSettings.PerformLayout();
            this.groupBoxConditionSettings.ResumeLayout(false);
            this.groupBoxConditionSettings.PerformLayout();
            this.groupBoxCondConditional.ResumeLayout(false);
            this.groupBoxCondConditional.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCondSimilarPrice)).EndInit();
            this.groupBoxPriceEstim.ResumeLayout(false);
            this.groupBoxPriceEstim.PerformLayout();
            this.panelPriceEstForSliderLabel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPriceEstAvg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMaxN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstHighestPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstLowestPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstOutliersHigh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstOutliersLow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLogSettings;
        private System.Windows.Forms.CheckBox checkBoxLogLargeChange;
        private System.Windows.Forms.CheckBox checkBoxLogSmallChange;
        private System.Windows.Forms.CheckBox checkBoxLogMinItems;
        private System.Windows.Forms.CheckBox checkBoxLogUpdated;
        private System.Windows.Forms.GroupBox groupBoxConditionSettings;
        private System.Windows.Forms.GroupBox groupBoxCondConditional;
        private System.Windows.Forms.RadioButton radioButtonCondUseOR;
        private System.Windows.Forms.RadioButton radioButtonCondUseAND;
        private System.Windows.Forms.Label labelCondBetterConSimilarPrice;
        private System.Windows.Forms.CheckBox checkBoxCondBetterIfBelowMinimum;
        private System.Windows.Forms.NumericUpDown numericUpDownCondSimilarPrice;
        private System.Windows.Forms.CheckBox checkBoxCondSimilarPrice;
        private System.Windows.Forms.CheckBox checkBoxCondMatchesAbove;
        private System.Windows.Forms.CheckBox checkBoxCondAcceptBetterAlways;
        private System.Windows.Forms.Label labelMatchExplanation;
        private System.Windows.Forms.CheckBox checkBoxCondMatchOnly;
        private System.Windows.Forms.GroupBox groupBoxPriceEstim;
        private System.Windows.Forms.Panel panelPriceEstForSliderLabel;
        private System.Windows.Forms.Label labelPriceEstSliderValue;
        private System.Windows.Forms.TrackBar trackBarPriceEstAvg;
        private System.Windows.Forms.NumericUpDown numericUpDownPriceEstMaxN;
        private System.Windows.Forms.NumericUpDown numericUpDownPriceEstMinN;
        private System.Windows.Forms.Label labelPriceEstMaxN;
        private System.Windows.Forms.Label labelPriceEstMinN;
        private System.Windows.Forms.Label labelPriceEstHighestPrice;
        private System.Windows.Forms.NumericUpDown numericUpDownPriceEstHighestPrice;
        private System.Windows.Forms.RadioButton radioButtonPriceEstHighestPrice;
        private System.Windows.Forms.Label labelPriceEstLowestPrice;
        private System.Windows.Forms.NumericUpDown numericUpDownPriceEstLowestPrice;
        private System.Windows.Forms.RadioButton radioButtonPriceEstByLowestPrice;
        private System.Windows.Forms.RadioButton radioButtonPriceEstPriceByAvg;
        private System.Windows.Forms.TextBox textBoxPriceEstMaxChange;
        private System.Windows.Forms.Label labelPriceEstMaximumPrice;
        private System.Windows.Forms.NumericUpDown numericUpDownPriceEstMinPrice;
        private System.Windows.Forms.Label labelPriceEstMinPrice;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label labelPriceEstOutliers3;
        private System.Windows.Forms.Label labelPriceEstOutliers2;
        private System.Windows.Forms.NumericUpDown numericUpDownPriceEstOutliersHigh;
        private System.Windows.Forms.NumericUpDown numericUpDownPriceEstOutliersLow;
        private System.Windows.Forms.Label labelPriceEstAvgOutliers1;
        private System.Windows.Forms.CheckBox checkBoxTestMode;
    }
}