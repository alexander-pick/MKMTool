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
            this.radioButtonCondMatchesAbove = new System.Windows.Forms.RadioButton();
            this.radioButtonCondAcceptBetterAlways = new System.Windows.Forms.RadioButton();
            this.labelMatchExplanation = new System.Windows.Forms.Label();
            this.radioButtonCondMatchOnly = new System.Windows.Forms.RadioButton();
            this.groupBoxPriceEstim = new System.Windows.Forms.GroupBox();
            this.textBoxPriceEstMaxDiff = new System.Windows.Forms.TextBox();
            this.panelPriceEstForSliderLabel = new System.Windows.Forms.Panel();
            this.labelPriceEstSliderValue = new System.Windows.Forms.Label();
            this.trackBarPriceEstAvg = new System.Windows.Forms.TrackBar();
            this.labelPriceEstMaximumPrice = new System.Windows.Forms.Label();
            this.textBoxPriceEstMaxChange = new System.Windows.Forms.TextBox();
            this.labelPriceEstAvgOutliers1 = new System.Windows.Forms.Label();
            this.labelPriceEstHighestPrice = new System.Windows.Forms.Label();
            this.numericUpDownPriceEstHighestPrice = new System.Windows.Forms.NumericUpDown();
            this.radioButtonPriceEstHighestPrice = new System.Windows.Forms.RadioButton();
            this.labelPriceEstLowestPrice = new System.Windows.Forms.Label();
            this.numericUpDownPriceEstLowestPrice = new System.Windows.Forms.NumericUpDown();
            this.radioButtonPriceEstByLowestPrice = new System.Windows.Forms.RadioButton();
            this.radioButtonPriceEstPriceByAvg = new System.Windows.Forms.RadioButton();
            this.numericUpDownPriceEstMaxN = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPriceEstMinN = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownPriceEstMinPrice = new System.Windows.Forms.NumericUpDown();
            this.labelPriceEstMaxN = new System.Windows.Forms.Label();
            this.labelPriceEstMinPrice = new System.Windows.Forms.Label();
            this.labelPriceEstMinN = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.checkBoxTestMode = new System.Windows.Forms.CheckBox();
            this.checkBoxLogHighVariance = new System.Windows.Forms.CheckBox();
            this.groupBoxLogSettings.SuspendLayout();
            this.groupBoxConditionSettings.SuspendLayout();
            this.groupBoxPriceEstim.SuspendLayout();
            this.panelPriceEstForSliderLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPriceEstAvg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstHighestPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstLowestPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMaxN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxLogSettings
            // 
            this.groupBoxLogSettings.Controls.Add(this.checkBoxLogHighVariance);
            this.groupBoxLogSettings.Controls.Add(this.checkBoxLogLargeChange);
            this.groupBoxLogSettings.Controls.Add(this.checkBoxLogSmallChange);
            this.groupBoxLogSettings.Controls.Add(this.checkBoxLogMinItems);
            this.groupBoxLogSettings.Controls.Add(this.checkBoxLogUpdated);
            this.groupBoxLogSettings.Location = new System.Drawing.Point(13, 331);
            this.groupBoxLogSettings.Name = "groupBoxLogSettings";
            this.groupBoxLogSettings.Size = new System.Drawing.Size(788, 99);
            this.groupBoxLogSettings.TabIndex = 14;
            this.groupBoxLogSettings.TabStop = false;
            this.groupBoxLogSettings.Text = "Log settings";
            // 
            // checkBoxLogLargeChange
            // 
            this.checkBoxLogLargeChange.AutoSize = true;
            this.checkBoxLogLargeChange.Checked = true;
            this.checkBoxLogLargeChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogLargeChange.Location = new System.Drawing.Point(10, 66);
            this.checkBoxLogLargeChange.Name = "checkBoxLogLargeChange";
            this.checkBoxLogLargeChange.Size = new System.Drawing.Size(230, 17);
            this.checkBoxLogLargeChange.TabIndex = 3;
            this.checkBoxLogLargeChange.Text = "Log non-updates due to large price change";
            this.checkBoxLogLargeChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogSmallChange
            // 
            this.checkBoxLogSmallChange.AutoSize = true;
            this.checkBoxLogSmallChange.Checked = true;
            this.checkBoxLogSmallChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogSmallChange.Location = new System.Drawing.Point(10, 43);
            this.checkBoxLogSmallChange.Name = "checkBoxLogSmallChange";
            this.checkBoxLogSmallChange.Size = new System.Drawing.Size(242, 17);
            this.checkBoxLogSmallChange.TabIndex = 2;
            this.checkBoxLogSmallChange.Text = "Log non-updated due to minimal price change";
            this.checkBoxLogSmallChange.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogMinItems
            // 
            this.checkBoxLogMinItems.AutoSize = true;
            this.checkBoxLogMinItems.Checked = true;
            this.checkBoxLogMinItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogMinItems.Location = new System.Drawing.Point(253, 43);
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
            this.groupBoxConditionSettings.Controls.Add(this.radioButtonCondMatchesAbove);
            this.groupBoxConditionSettings.Controls.Add(this.radioButtonCondAcceptBetterAlways);
            this.groupBoxConditionSettings.Controls.Add(this.labelMatchExplanation);
            this.groupBoxConditionSettings.Controls.Add(this.radioButtonCondMatchOnly);
            this.groupBoxConditionSettings.Location = new System.Drawing.Point(13, 253);
            this.groupBoxConditionSettings.Name = "groupBoxConditionSettings";
            this.groupBoxConditionSettings.Size = new System.Drawing.Size(788, 72);
            this.groupBoxConditionSettings.TabIndex = 13;
            this.groupBoxConditionSettings.TabStop = false;
            this.groupBoxConditionSettings.Text = "Accepted items in better condition as similar items";
            // 
            // radioButtonCondMatchesAbove
            // 
            this.radioButtonCondMatchesAbove.AutoSize = true;
            this.radioButtonCondMatchesAbove.Location = new System.Drawing.Point(253, 19);
            this.radioButtonCondMatchesAbove.Name = "radioButtonCondMatchesAbove";
            this.radioButtonCondMatchesAbove.Size = new System.Drawing.Size(327, 17);
            this.radioButtonCondMatchesAbove.TabIndex = 7;
            this.radioButtonCondMatchesAbove.Text = "Accept better only if there is at least one more expensive match*";
            this.radioButtonCondMatchesAbove.UseVisualStyleBackColor = true;
            // 
            // radioButtonCondAcceptBetterAlways
            // 
            this.radioButtonCondAcceptBetterAlways.AutoSize = true;
            this.radioButtonCondAcceptBetterAlways.Location = new System.Drawing.Point(611, 19);
            this.radioButtonCondAcceptBetterAlways.Name = "radioButtonCondAcceptBetterAlways";
            this.radioButtonCondAcceptBetterAlways.Size = new System.Drawing.Size(139, 17);
            this.radioButtonCondAcceptBetterAlways.TabIndex = 7;
            this.radioButtonCondAcceptBetterAlways.Text = "Accept better whenever";
            this.radioButtonCondAcceptBetterAlways.UseVisualStyleBackColor = true;
            this.radioButtonCondAcceptBetterAlways.CheckedChanged += new System.EventHandler(this.checkBoxCondAcceptBetterAlways_CheckedChanged);
            // 
            // labelMatchExplanation
            // 
            this.labelMatchExplanation.AutoSize = true;
            this.labelMatchExplanation.Location = new System.Drawing.Point(7, 50);
            this.labelMatchExplanation.Name = "labelMatchExplanation";
            this.labelMatchExplanation.Size = new System.Drawing.Size(343, 13);
            this.labelMatchExplanation.TabIndex = 2;
            this.labelMatchExplanation.Text = "*Match = item in exactly the same condition as the one being evaluated";
            // 
            // radioButtonCondMatchOnly
            // 
            this.radioButtonCondMatchOnly.AutoSize = true;
            this.radioButtonCondMatchOnly.Checked = true;
            this.radioButtonCondMatchOnly.Location = new System.Drawing.Point(10, 19);
            this.radioButtonCondMatchOnly.Name = "radioButtonCondMatchOnly";
            this.radioButtonCondMatchOnly.Size = new System.Drawing.Size(128, 17);
            this.radioButtonCondMatchOnly.TabIndex = 0;
            this.radioButtonCondMatchOnly.TabStop = true;
            this.radioButtonCondMatchOnly.Text = "Accept only matches*";
            this.radioButtonCondMatchOnly.UseVisualStyleBackColor = true;
            this.radioButtonCondMatchOnly.CheckedChanged += new System.EventHandler(this.checkBoxCondMatchOnly_CheckedChanged);
            // 
            // groupBoxPriceEstim
            // 
            this.groupBoxPriceEstim.Controls.Add(this.textBoxPriceEstMaxDiff);
            this.groupBoxPriceEstim.Controls.Add(this.panelPriceEstForSliderLabel);
            this.groupBoxPriceEstim.Controls.Add(this.trackBarPriceEstAvg);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMaximumPrice);
            this.groupBoxPriceEstim.Controls.Add(this.textBoxPriceEstMaxChange);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstAvgOutliers1);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstHighestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstHighestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.radioButtonPriceEstHighestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstLowestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstLowestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.radioButtonPriceEstByLowestPrice);
            this.groupBoxPriceEstim.Controls.Add(this.radioButtonPriceEstPriceByAvg);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstMaxN);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstMinN);
            this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstMinPrice);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMaxN);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMinPrice);
            this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMinN);
            this.groupBoxPriceEstim.Controls.Add(this.statusLabel);
            this.groupBoxPriceEstim.Location = new System.Drawing.Point(12, 12);
            this.groupBoxPriceEstim.Name = "groupBoxPriceEstim";
            this.groupBoxPriceEstim.Size = new System.Drawing.Size(789, 235);
            this.groupBoxPriceEstim.TabIndex = 12;
            this.groupBoxPriceEstim.TabStop = false;
            this.groupBoxPriceEstim.Text = "Price estimation";
            // 
            // textBoxPriceEstMaxDiff
            // 
            this.textBoxPriceEstMaxDiff.Location = new System.Drawing.Point(589, 66);
            this.textBoxPriceEstMaxDiff.Name = "textBoxPriceEstMaxDiff";
            this.textBoxPriceEstMaxDiff.Size = new System.Drawing.Size(191, 20);
            this.textBoxPriceEstMaxDiff.TabIndex = 49;
            this.textBoxPriceEstMaxDiff.Text = "1;300;10;20;20;15;50;8;500;3";
            // 
            // panelPriceEstForSliderLabel
            // 
            this.panelPriceEstForSliderLabel.Controls.Add(this.labelPriceEstSliderValue);
            this.panelPriceEstForSliderLabel.Location = new System.Drawing.Point(254, 152);
            this.panelPriceEstForSliderLabel.Name = "panelPriceEstForSliderLabel";
            this.panelPriceEstForSliderLabel.Size = new System.Drawing.Size(526, 23);
            this.panelPriceEstForSliderLabel.TabIndex = 37;
            // 
            // labelPriceEstSliderValue
            // 
            this.labelPriceEstSliderValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPriceEstSliderValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPriceEstSliderValue.Location = new System.Drawing.Point(0, 0);
            this.labelPriceEstSliderValue.Name = "labelPriceEstSliderValue";
            this.labelPriceEstSliderValue.Size = new System.Drawing.Size(526, 23);
            this.labelPriceEstSliderValue.TabIndex = 24;
            this.labelPriceEstSliderValue.Text = "AVG";
            this.labelPriceEstSliderValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarPriceEstAvg
            // 
            this.trackBarPriceEstAvg.Location = new System.Drawing.Point(256, 107);
            this.trackBarPriceEstAvg.Maximum = 50;
            this.trackBarPriceEstAvg.Name = "trackBarPriceEstAvg";
            this.trackBarPriceEstAvg.Size = new System.Drawing.Size(526, 45);
            this.trackBarPriceEstAvg.TabIndex = 36;
            this.trackBarPriceEstAvg.Value = 25;
            this.trackBarPriceEstAvg.Scroll += new System.EventHandler(this.trackBarPriceEstAvg_Scroll);
            // 
            // labelPriceEstMaximumPrice
            // 
            this.labelPriceEstMaximumPrice.AutoSize = true;
            this.labelPriceEstMaximumPrice.Location = new System.Drawing.Point(6, 43);
            this.labelPriceEstMaximumPrice.Name = "labelPriceEstMaximumPrice";
            this.labelPriceEstMaximumPrice.Size = new System.Drawing.Size(518, 13);
            this.labelPriceEstMaximumPrice.TabIndex = 17;
            this.labelPriceEstMaximumPrice.Text = "Max price change (format: \"T1;C1;T2;C2;\" etc., Cx is max allowed change in % for " +
    "items that cost Tx or less):";
            // 
            // textBoxPriceEstMaxChange
            // 
            this.textBoxPriceEstMaxChange.Location = new System.Drawing.Point(589, 40);
            this.textBoxPriceEstMaxChange.Name = "textBoxPriceEstMaxChange";
            this.textBoxPriceEstMaxChange.Size = new System.Drawing.Size(191, 20);
            this.textBoxPriceEstMaxChange.TabIndex = 18;
            // 
            // labelPriceEstAvgOutliers1
            // 
            this.labelPriceEstAvgOutliers1.AutoSize = true;
            this.labelPriceEstAvgOutliers1.Location = new System.Drawing.Point(6, 69);
            this.labelPriceEstAvgOutliers1.Name = "labelPriceEstAvgOutliers1";
            this.labelPriceEstAvgOutliers1.Size = new System.Drawing.Size(579, 13);
            this.labelPriceEstAvgOutliers1.TabIndex = 48;
            this.labelPriceEstAvgOutliers1.Text = "Max differences between consecutive items (\"T1;C1;T2;C2;\" etc., Cx = max differen" +
    "ce in % for items that cost Tx or less): ";
            // 
            // labelPriceEstHighestPrice
            // 
            this.labelPriceEstHighestPrice.AutoSize = true;
            this.labelPriceEstHighestPrice.Location = new System.Drawing.Point(330, 210);
            this.labelPriceEstHighestPrice.Name = "labelPriceEstHighestPrice";
            this.labelPriceEstHighestPrice.Size = new System.Drawing.Size(325, 13);
            this.labelPriceEstHighestPrice.TabIndex = 32;
            this.labelPriceEstHighestPrice.Text = "% of highest price (among the up to max # of cheapest similar items)";
            // 
            // numericUpDownPriceEstHighestPrice
            // 
            this.numericUpDownPriceEstHighestPrice.DecimalPlaces = 2;
            this.numericUpDownPriceEstHighestPrice.Location = new System.Drawing.Point(254, 208);
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
            this.labelPriceEstLowestPrice.Location = new System.Drawing.Point(330, 180);
            this.labelPriceEstLowestPrice.Name = "labelPriceEstLowestPrice";
            this.labelPriceEstLowestPrice.Size = new System.Drawing.Size(383, 13);
            this.labelPriceEstLowestPrice.TabIndex = 29;
            this.labelPriceEstLowestPrice.Text = "% of lowest price (WARNING - this will not cut off outliers based on price jumps!" +
    ")";
            // 
            // numericUpDownPriceEstLowestPrice
            // 
            this.numericUpDownPriceEstLowestPrice.DecimalPlaces = 2;
            this.numericUpDownPriceEstLowestPrice.Location = new System.Drawing.Point(254, 178);
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
            // numericUpDownPriceEstMaxN
            // 
            this.numericUpDownPriceEstMaxN.Location = new System.Drawing.Point(710, 14);
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
            this.numericUpDownPriceEstMinN.Location = new System.Drawing.Point(476, 14);
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
            // numericUpDownPriceEstMinPrice
            // 
            this.numericUpDownPriceEstMinPrice.DecimalPlaces = 2;
            this.numericUpDownPriceEstMinPrice.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numericUpDownPriceEstMinPrice.Location = new System.Drawing.Point(254, 14);
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
            // labelPriceEstMaxN
            // 
            this.labelPriceEstMaxN.AutoSize = true;
            this.labelPriceEstMaxN.Location = new System.Drawing.Point(570, 16);
            this.labelPriceEstMaxN.Name = "labelPriceEstMaxN";
            this.labelPriceEstMaxN.Size = new System.Drawing.Size(134, 13);
            this.labelPriceEstMaxN.TabIndex = 32;
            this.labelPriceEstMaxN.Text = "Maximum # of similar items:";
            // 
            // labelPriceEstMinPrice
            // 
            this.labelPriceEstMinPrice.AutoSize = true;
            this.labelPriceEstMinPrice.Location = new System.Drawing.Point(6, 16);
            this.labelPriceEstMinPrice.Name = "labelPriceEstMinPrice";
            this.labelPriceEstMinPrice.Size = new System.Drawing.Size(147, 13);
            this.labelPriceEstMinPrice.TabIndex = 15;
            this.labelPriceEstMinPrice.Text = "Minimum price of rares [EUR]:";
            // 
            // labelPriceEstMinN
            // 
            this.labelPriceEstMinN.AutoSize = true;
            this.labelPriceEstMinN.Location = new System.Drawing.Point(330, 16);
            this.labelPriceEstMinN.Name = "labelPriceEstMinN";
            this.labelPriceEstMinN.Size = new System.Drawing.Size(131, 13);
            this.labelPriceEstMinN.TabIndex = 31;
            this.labelPriceEstMinN.Text = "Minimum # of similar items:";
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
            // checkBoxTestMode
            // 
            this.checkBoxTestMode.AutoSize = true;
            this.checkBoxTestMode.Location = new System.Drawing.Point(21, 445);
            this.checkBoxTestMode.Name = "checkBoxTestMode";
            this.checkBoxTestMode.Size = new System.Drawing.Size(248, 17);
            this.checkBoxTestMode.TabIndex = 15;
            this.checkBoxTestMode.Text = "Test mode - do not send price updates to MKM";
            this.checkBoxTestMode.UseVisualStyleBackColor = true;
            // 
            // checkBoxLogHighVariance
            // 
            this.checkBoxLogHighVariance.AutoSize = true;
            this.checkBoxLogHighVariance.Checked = true;
            this.checkBoxLogHighVariance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLogHighVariance.Location = new System.Drawing.Point(253, 66);
            this.checkBoxLogHighVariance.Name = "checkBoxLogHighVariance";
            this.checkBoxLogHighVariance.Size = new System.Drawing.Size(315, 17);
            this.checkBoxLogHighVariance.TabIndex = 4;
            this.checkBoxLogHighVariance.Text = "Log non-updates due to high variance among cheapest items";
            this.checkBoxLogHighVariance.UseVisualStyleBackColor = true;
            // 
            // UpdatePriceSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 474);
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
            this.groupBoxPriceEstim.ResumeLayout(false);
            this.groupBoxPriceEstim.PerformLayout();
            this.panelPriceEstForSliderLabel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPriceEstAvg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstHighestPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstLowestPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMaxN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinPrice)).EndInit();
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
        private System.Windows.Forms.RadioButton radioButtonCondMatchesAbove;
        private System.Windows.Forms.RadioButton radioButtonCondAcceptBetterAlways;
        private System.Windows.Forms.Label labelMatchExplanation;
        private System.Windows.Forms.RadioButton radioButtonCondMatchOnly;
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
        private System.Windows.Forms.Label labelPriceEstAvgOutliers1;
        private System.Windows.Forms.CheckBox checkBoxTestMode;
        private System.Windows.Forms.TextBox textBoxPriceEstMaxDiff;
        private System.Windows.Forms.CheckBox checkBoxLogHighVariance;
    }
}