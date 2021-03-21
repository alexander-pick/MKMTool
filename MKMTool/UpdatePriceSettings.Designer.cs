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
    along with MKMTool.  If not, see <http://www.gnu.org/licenses/>.

    Diese Datei ist Teil von MKMTool.

    MKMTool ist Freie Software: Sie können es unter den Bedingungen
    der GNU General Public License, wie von der Free Software Foundation,
    Version 3 der Lizenz oder (nach Ihrer Wahl) jeder späteren
    veröffentlichten Version, weiterverbreiten und/oder modifizieren.

    MKMTool wird in der Hoffnung, dass es nützlich sein wird, aber
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdatePriceSettings));
      this.groupBoxLogSettings = new System.Windows.Forms.GroupBox();
      this.checkBoxLogLargeChangeHigh = new System.Windows.Forms.CheckBox();
      this.checkBoxLogHighVariance = new System.Windows.Forms.CheckBox();
      this.checkBoxLogLargeChangeLow = new System.Windows.Forms.CheckBox();
      this.checkBoxLogSmallChange = new System.Windows.Forms.CheckBox();
      this.checkBoxLogMinItems = new System.Windows.Forms.CheckBox();
      this.checkBoxLogUpdated = new System.Windows.Forms.CheckBox();
      this.groupBoxPriceGuides = new System.Windows.Forms.GroupBox();
      this.labelPriceGuidesModifiersFoils = new System.Windows.Forms.Label();
      this.labelPriceGuidesModifiersNonfoils = new System.Windows.Forms.Label();
      this.checkBoxPriceGuidesLogNotFound = new System.Windows.Forms.CheckBox();
      this.labelPriceGuidesNotFound = new System.Windows.Forms.Label();
      this.checkBoxPriceGuidesTraverseNotFound = new System.Windows.Forms.CheckBox();
      this.comboBoxPriceGuidesFoils = new System.Windows.Forms.ComboBox();
      this.labelPriceGuidesFoils = new System.Windows.Forms.Label();
      this.textBoxPriceGuidesFoils = new System.Windows.Forms.TextBox();
      this.comboBoxPriceGuidesNonFoils = new System.Windows.Forms.ComboBox();
      this.labelPriceGuidesNonFoils = new System.Windows.Forms.Label();
      this.textBoxPriceGuidesNonFoil = new System.Windows.Forms.TextBox();
      this.groupBoxConditionSettings = new System.Windows.Forms.GroupBox();
      this.radioButtonCondMatchesAbove = new System.Windows.Forms.RadioButton();
      this.radioButtonCondAcceptBetterAlways = new System.Windows.Forms.RadioButton();
      this.labelMatchExplanation = new System.Windows.Forms.Label();
      this.radioButtonCondMatchOnly = new System.Windows.Forms.RadioButton();
      this.groupBoxPriceEstim = new System.Windows.Forms.GroupBox();
      this.comboBoxPriceEstMinPriceMatch = new System.Windows.Forms.ComboBox();
      this.labelPriceEstMinPriceMatch = new System.Windows.Forms.Label();
      this.comboBoxPriceEstUpdateMode = new System.Windows.Forms.ComboBox();
      this.labelPriceEstUpdateMode = new System.Windows.Forms.Label();
      this.labelMultCopiesCap = new System.Windows.Forms.Label();
      this.numericUpDownPriceMultCopiesCap = new System.Windows.Forms.NumericUpDown();
      this.labelMultCopies4 = new System.Windows.Forms.Label();
      this.numericUpDownPriceMultCopies4 = new System.Windows.Forms.NumericUpDown();
      this.labelMultCopiesThree = new System.Windows.Forms.Label();
      this.numericUpDownPriceMultCopies3 = new System.Windows.Forms.NumericUpDown();
      this.labelMultiplesTwo = new System.Windows.Forms.Label();
      this.numericUpDownPriceMultCopies2 = new System.Windows.Forms.NumericUpDown();
      this.labelMultiples = new System.Windows.Forms.Label();
      this.labelPriceEstMaximumPrice = new System.Windows.Forms.Label();
      this.textBoxPriceEstMaxChange = new System.Windows.Forms.TextBox();
      this.numericUpDownPriceEstMinPrice = new System.Windows.Forms.NumericUpDown();
      this.labelPriceEstMinPrice = new System.Windows.Forms.Label();
      this.numericUpDownPriceEstMaxN = new System.Windows.Forms.NumericUpDown();
      this.numericUpDownPriceEstMinN = new System.Windows.Forms.NumericUpDown();
      this.labelPriceEstMaxN = new System.Windows.Forms.Label();
      this.labelPriceEstMinN = new System.Windows.Forms.Label();
      this.checkBoxPricePlaysetIgnore = new System.Windows.Forms.CheckBox();
      this.checkBoxPriceEstWorldwide = new System.Windows.Forms.CheckBox();
      this.textBoxPriceEstMaxDiff = new System.Windows.Forms.TextBox();
      this.labelPriceEstAvgOutliers1 = new System.Windows.Forms.Label();
      this.labelWorlwideAvg = new System.Windows.Forms.Label();
      this.panelPriceEstWorldForSliderLabel = new System.Windows.Forms.Panel();
      this.labelPriceEstSliderValueWorld = new System.Windows.Forms.Label();
      this.panelPriceEstForSliderLabel = new System.Windows.Forms.Panel();
      this.labelPriceEstSliderValue = new System.Windows.Forms.Label();
      this.trackBarPriceEstAvgWorld = new System.Windows.Forms.TrackBar();
      this.trackBarPriceEstAvg = new System.Windows.Forms.TrackBar();
      this.labelPriceEstHighestPrice = new System.Windows.Forms.Label();
      this.numericUpDownPriceEstHighestPrice = new System.Windows.Forms.NumericUpDown();
      this.radioButtonPriceEstHighestPrice = new System.Windows.Forms.RadioButton();
      this.labelPriceEstLowestPrice = new System.Windows.Forms.Label();
      this.numericUpDownPriceEstLowestPrice = new System.Windows.Forms.NumericUpDown();
      this.radioButtonPriceEstByLowestPrice = new System.Windows.Forms.RadioButton();
      this.radioButtonPriceEstPriceByAvg = new System.Windows.Forms.RadioButton();
      this.checkBoxTestMode = new System.Windows.Forms.CheckBox();
      this.groupBoxPresets = new System.Windows.Forms.GroupBox();
      this.panelPresetsDescr = new System.Windows.Forms.Panel();
      this.labelPresetsDescr = new System.Windows.Forms.Label();
      this.buttonPresetsDelete = new System.Windows.Forms.Button();
      this.buttonPresetsStore = new System.Windows.Forms.Button();
      this.buttonPresetsLoad = new System.Windows.Forms.Button();
      this.comboBoxPresets = new System.Windows.Forms.ComboBox();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.checkBoxFilterCountries = new System.Windows.Forms.CheckBox();
      this.checkBoxFilterExpansions = new System.Windows.Forms.CheckBox();
      this.checkBoxFilterPowerseller = new System.Windows.Forms.CheckBox();
      this.checkBoxFilterProfSeller = new System.Windows.Forms.CheckBox();
      this.checkBoxFilterPrivSeller = new System.Windows.Forms.CheckBox();
      this.buttonFilterCountries = new System.Windows.Forms.Button();
      this.buttonFilterExpansions = new System.Windows.Forms.Button();
      this.groupBoxTraversal = new System.Windows.Forms.GroupBox();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.groupBoxLogSettings.SuspendLayout();
      this.groupBoxPriceGuides.SuspendLayout();
      this.groupBoxConditionSettings.SuspendLayout();
      this.groupBoxPriceEstim.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceMultCopiesCap)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceMultCopies4)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceMultCopies3)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceMultCopies2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinPrice)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMaxN)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinN)).BeginInit();
      this.panelPriceEstWorldForSliderLabel.SuspendLayout();
      this.panelPriceEstForSliderLabel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarPriceEstAvgWorld)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarPriceEstAvg)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstHighestPrice)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstLowestPrice)).BeginInit();
      this.groupBoxPresets.SuspendLayout();
      this.panelPresetsDescr.SuspendLayout();
      this.groupBoxTraversal.SuspendLayout();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxLogSettings
      // 
      this.groupBoxLogSettings.Controls.Add(this.checkBoxLogLargeChangeHigh);
      this.groupBoxLogSettings.Controls.Add(this.checkBoxLogHighVariance);
      this.groupBoxLogSettings.Controls.Add(this.checkBoxLogLargeChangeLow);
      this.groupBoxLogSettings.Controls.Add(this.checkBoxLogSmallChange);
      this.groupBoxLogSettings.Controls.Add(this.checkBoxLogMinItems);
      this.groupBoxLogSettings.Controls.Add(this.checkBoxLogUpdated);
      this.groupBoxLogSettings.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxLogSettings.Location = new System.Drawing.Point(3, 610);
      this.groupBoxLogSettings.Name = "groupBoxLogSettings";
      this.groupBoxLogSettings.Size = new System.Drawing.Size(806, 99);
      this.groupBoxLogSettings.TabIndex = 14;
      this.groupBoxLogSettings.TabStop = false;
      this.groupBoxLogSettings.Text = "Log settings";
      // 
      // checkBoxLogLargeChangeHigh
      // 
      this.checkBoxLogLargeChangeHigh.AutoSize = true;
      this.checkBoxLogLargeChangeHigh.Checked = true;
      this.checkBoxLogLargeChangeHigh.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxLogLargeChangeHigh.Location = new System.Drawing.Point(11, 66);
      this.checkBoxLogLargeChangeHigh.Name = "checkBoxLogLargeChangeHigh";
      this.checkBoxLogLargeChangeHigh.Size = new System.Drawing.Size(315, 17);
      this.checkBoxLogLargeChangeHigh.TabIndex = 5;
      this.checkBoxLogLargeChangeHigh.Text = "Log non-updates due to large price change (yours is too high)";
      this.checkBoxLogLargeChangeHigh.UseVisualStyleBackColor = true;
      // 
      // checkBoxLogHighVariance
      // 
      this.checkBoxLogHighVariance.AutoSize = true;
      this.checkBoxLogHighVariance.Checked = true;
      this.checkBoxLogHighVariance.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxLogHighVariance.Location = new System.Drawing.Point(405, 66);
      this.checkBoxLogHighVariance.Name = "checkBoxLogHighVariance";
      this.checkBoxLogHighVariance.Size = new System.Drawing.Size(315, 17);
      this.checkBoxLogHighVariance.TabIndex = 4;
      this.checkBoxLogHighVariance.Text = "Log non-updates due to high variance among cheapest items";
      this.checkBoxLogHighVariance.UseVisualStyleBackColor = true;
      // 
      // checkBoxLogLargeChangeLow
      // 
      this.checkBoxLogLargeChangeLow.AutoSize = true;
      this.checkBoxLogLargeChangeLow.Checked = true;
      this.checkBoxLogLargeChangeLow.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxLogLargeChangeLow.Location = new System.Drawing.Point(10, 43);
      this.checkBoxLogLargeChangeLow.Name = "checkBoxLogLargeChangeLow";
      this.checkBoxLogLargeChangeLow.Size = new System.Drawing.Size(311, 17);
      this.checkBoxLogLargeChangeLow.TabIndex = 3;
      this.checkBoxLogLargeChangeLow.Text = "Log non-updates due to large price change (yours is too low)";
      this.checkBoxLogLargeChangeLow.UseVisualStyleBackColor = true;
      // 
      // checkBoxLogSmallChange
      // 
      this.checkBoxLogSmallChange.AutoSize = true;
      this.checkBoxLogSmallChange.Checked = true;
      this.checkBoxLogSmallChange.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxLogSmallChange.Location = new System.Drawing.Point(405, 19);
      this.checkBoxLogSmallChange.Name = "checkBoxLogSmallChange";
      this.checkBoxLogSmallChange.Size = new System.Drawing.Size(225, 17);
      this.checkBoxLogSmallChange.TabIndex = 2;
      this.checkBoxLogSmallChange.Text = "Log updates even with small price change";
      this.checkBoxLogSmallChange.UseVisualStyleBackColor = true;
      // 
      // checkBoxLogMinItems
      // 
      this.checkBoxLogMinItems.AutoSize = true;
      this.checkBoxLogMinItems.Checked = true;
      this.checkBoxLogMinItems.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxLogMinItems.Location = new System.Drawing.Point(405, 43);
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
      this.checkBoxLogUpdated.Size = new System.Drawing.Size(222, 17);
      this.checkBoxLogUpdated.TabIndex = 0;
      this.checkBoxLogUpdated.Text = "Log updates with significant price change";
      this.checkBoxLogUpdated.UseVisualStyleBackColor = true;
      // 
      // groupBoxPriceGuides
      // 
      this.groupBoxPriceGuides.Controls.Add(this.labelPriceGuidesModifiersFoils);
      this.groupBoxPriceGuides.Controls.Add(this.labelPriceGuidesModifiersNonfoils);
      this.groupBoxPriceGuides.Controls.Add(this.checkBoxPriceGuidesLogNotFound);
      this.groupBoxPriceGuides.Controls.Add(this.labelPriceGuidesNotFound);
      this.groupBoxPriceGuides.Controls.Add(this.checkBoxPriceGuidesTraverseNotFound);
      this.groupBoxPriceGuides.Controls.Add(this.comboBoxPriceGuidesFoils);
      this.groupBoxPriceGuides.Controls.Add(this.labelPriceGuidesFoils);
      this.groupBoxPriceGuides.Controls.Add(this.textBoxPriceGuidesFoils);
      this.groupBoxPriceGuides.Controls.Add(this.comboBoxPriceGuidesNonFoils);
      this.groupBoxPriceGuides.Controls.Add(this.labelPriceGuidesNonFoils);
      this.groupBoxPriceGuides.Controls.Add(this.textBoxPriceGuidesNonFoil);
      this.groupBoxPriceGuides.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxPriceGuides.Location = new System.Drawing.Point(3, 120);
      this.groupBoxPriceGuides.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
      this.groupBoxPriceGuides.Name = "groupBoxPriceGuides";
      this.groupBoxPriceGuides.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
      this.groupBoxPriceGuides.Size = new System.Drawing.Size(806, 93);
      this.groupBoxPriceGuides.TabIndex = 68;
      this.groupBoxPriceGuides.TabStop = false;
      this.groupBoxPriceGuides.Text = "Pricing by price guides";
      this.groupBoxPriceGuides.Visible = false;
      // 
      // labelPriceGuidesModifiersFoils
      // 
      this.labelPriceGuidesModifiersFoils.AutoSize = true;
      this.labelPriceGuidesModifiersFoils.Location = new System.Drawing.Point(631, 14);
      this.labelPriceGuidesModifiersFoils.Name = "labelPriceGuidesModifiersFoils";
      this.labelPriceGuidesModifiersFoils.Size = new System.Drawing.Size(52, 13);
      this.labelPriceGuidesModifiersFoils.TabIndex = 77;
      this.labelPriceGuidesModifiersFoils.Text = "Modifiers:";
      this.toolTip1.SetToolTip(this.labelPriceGuidesModifiersFoils, "Use only *, + and - as operators. No parenthesis: operations are evaluated in the" +
        " written order! You may use price guide codes or numbers as operands.");
      // 
      // labelPriceGuidesModifiersNonfoils
      // 
      this.labelPriceGuidesModifiersNonfoils.AutoSize = true;
      this.labelPriceGuidesModifiersNonfoils.Location = new System.Drawing.Point(261, 16);
      this.labelPriceGuidesModifiersNonfoils.Name = "labelPriceGuidesModifiersNonfoils";
      this.labelPriceGuidesModifiersNonfoils.Size = new System.Drawing.Size(52, 13);
      this.labelPriceGuidesModifiersNonfoils.TabIndex = 76;
      this.labelPriceGuidesModifiersNonfoils.Text = "Modifiers:";
      this.toolTip1.SetToolTip(this.labelPriceGuidesModifiersNonfoils, "Use only *, + and - as operators. No parenthesis: operations are evaluated in the" +
        " written order! You may use price guide codes or numbers as operands.");
      // 
      // checkBoxPriceGuidesLogNotFound
      // 
      this.checkBoxPriceGuidesLogNotFound.AutoSize = true;
      this.checkBoxPriceGuidesLogNotFound.Checked = true;
      this.checkBoxPriceGuidesLogNotFound.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxPriceGuidesLogNotFound.Location = new System.Drawing.Point(300, 60);
      this.checkBoxPriceGuidesLogNotFound.Name = "checkBoxPriceGuidesLogNotFound";
      this.checkBoxPriceGuidesLogNotFound.Size = new System.Drawing.Size(80, 17);
      this.checkBoxPriceGuidesLogNotFound.TabIndex = 75;
      this.checkBoxPriceGuidesLogNotFound.Text = "Write to log";
      this.checkBoxPriceGuidesLogNotFound.UseVisualStyleBackColor = true;
      // 
      // labelPriceGuidesNotFound
      // 
      this.labelPriceGuidesNotFound.AutoSize = true;
      this.labelPriceGuidesNotFound.Location = new System.Drawing.Point(6, 61);
      this.labelPriceGuidesNotFound.Name = "labelPriceGuidesNotFound";
      this.labelPriceGuidesNotFound.Size = new System.Drawing.Size(190, 13);
      this.labelPriceGuidesNotFound.TabIndex = 74;
      this.labelPriceGuidesNotFound.Text = "When price guide not found for a card:";
      this.toolTip1.SetToolTip(this.labelPriceGuidesNotFound, "insert sequence \"T1;C1;T2;C2;\" etc., where Cx is max allowed price change in % fo" +
        "r items that cost (old price) Tx or less");
      // 
      // checkBoxPriceGuidesTraverseNotFound
      // 
      this.checkBoxPriceGuidesTraverseNotFound.AutoSize = true;
      this.checkBoxPriceGuidesTraverseNotFound.Location = new System.Drawing.Point(205, 60);
      this.checkBoxPriceGuidesTraverseNotFound.Name = "checkBoxPriceGuidesTraverseNotFound";
      this.checkBoxPriceGuidesTraverseNotFound.Size = new System.Drawing.Size(77, 17);
      this.checkBoxPriceGuidesTraverseNotFound.TabIndex = 6;
      this.checkBoxPriceGuidesTraverseNotFound.Text = "Use TOSS";
      this.checkBoxPriceGuidesTraverseNotFound.UseVisualStyleBackColor = true;
      this.checkBoxPriceGuidesTraverseNotFound.CheckedChanged += new System.EventHandler(this.checkBoxPriceGuidesTraversalNotFound_CheckedChanged);
      // 
      // comboBoxPriceGuidesFoils
      // 
      this.comboBoxPriceGuidesFoils.CausesValidation = false;
      this.comboBoxPriceGuidesFoils.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPriceGuidesFoils.FormattingEnabled = true;
      this.comboBoxPriceGuidesFoils.Location = new System.Drawing.Point(438, 30);
      this.comboBoxPriceGuidesFoils.Name = "comboBoxPriceGuidesFoils";
      this.comboBoxPriceGuidesFoils.Size = new System.Drawing.Size(141, 21);
      this.comboBoxPriceGuidesFoils.TabIndex = 71;
      // 
      // labelPriceGuidesFoils
      // 
      this.labelPriceGuidesFoils.AutoSize = true;
      this.labelPriceGuidesFoils.Location = new System.Drawing.Point(402, 33);
      this.labelPriceGuidesFoils.Name = "labelPriceGuidesFoils";
      this.labelPriceGuidesFoils.Size = new System.Drawing.Size(31, 13);
      this.labelPriceGuidesFoils.TabIndex = 72;
      this.labelPriceGuidesFoils.Text = "Foils:";
      this.toolTip1.SetToolTip(this.labelPriceGuidesFoils, "insert sequence \"T1;C1;T2;C2;\" etc., where Cx is max allowed price change in % fo" +
        "r items that cost (old price) Tx or less");
      // 
      // textBoxPriceGuidesFoils
      // 
      this.textBoxPriceGuidesFoils.Location = new System.Drawing.Point(580, 30);
      this.textBoxPriceGuidesFoils.Name = "textBoxPriceGuidesFoils";
      this.textBoxPriceGuidesFoils.Size = new System.Drawing.Size(167, 20);
      this.textBoxPriceGuidesFoils.TabIndex = 73;
      // 
      // comboBoxPriceGuidesNonFoils
      // 
      this.comboBoxPriceGuidesNonFoils.CausesValidation = false;
      this.comboBoxPriceGuidesNonFoils.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPriceGuidesNonFoils.FormattingEnabled = true;
      this.comboBoxPriceGuidesNonFoils.Location = new System.Drawing.Point(63, 30);
      this.comboBoxPriceGuidesNonFoils.Name = "comboBoxPriceGuidesNonFoils";
      this.comboBoxPriceGuidesNonFoils.Size = new System.Drawing.Size(141, 21);
      this.comboBoxPriceGuidesNonFoils.TabIndex = 69;
      // 
      // labelPriceGuidesNonFoils
      // 
      this.labelPriceGuidesNonFoils.AutoSize = true;
      this.labelPriceGuidesNonFoils.Location = new System.Drawing.Point(6, 33);
      this.labelPriceGuidesNonFoils.Name = "labelPriceGuidesNonFoils";
      this.labelPriceGuidesNonFoils.Size = new System.Drawing.Size(51, 13);
      this.labelPriceGuidesNonFoils.TabIndex = 69;
      this.labelPriceGuidesNonFoils.Text = "Non-foils:";
      this.toolTip1.SetToolTip(this.labelPriceGuidesNonFoils, "insert sequence \"T1;C1;T2;C2;\" etc., where Cx is max allowed price change in % fo" +
        "r items that cost (old price) Tx or less");
      // 
      // textBoxPriceGuidesNonFoil
      // 
      this.textBoxPriceGuidesNonFoil.Location = new System.Drawing.Point(205, 30);
      this.textBoxPriceGuidesNonFoil.Name = "textBoxPriceGuidesNonFoil";
      this.textBoxPriceGuidesNonFoil.Size = new System.Drawing.Size(175, 20);
      this.textBoxPriceGuidesNonFoil.TabIndex = 70;
      // 
      // groupBoxConditionSettings
      // 
      this.groupBoxConditionSettings.Controls.Add(this.radioButtonCondMatchesAbove);
      this.groupBoxConditionSettings.Controls.Add(this.radioButtonCondAcceptBetterAlways);
      this.groupBoxConditionSettings.Controls.Add(this.labelMatchExplanation);
      this.groupBoxConditionSettings.Controls.Add(this.radioButtonCondMatchOnly);
      this.groupBoxConditionSettings.Location = new System.Drawing.Point(10, 297);
      this.groupBoxConditionSettings.Name = "groupBoxConditionSettings";
      this.groupBoxConditionSettings.Size = new System.Drawing.Size(779, 72);
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
      this.groupBoxPriceEstim.Controls.Add(this.comboBoxPriceEstMinPriceMatch);
      this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMinPriceMatch);
      this.groupBoxPriceEstim.Controls.Add(this.comboBoxPriceEstUpdateMode);
      this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstUpdateMode);
      this.groupBoxPriceEstim.Controls.Add(this.labelMultCopiesCap);
      this.groupBoxPriceEstim.Controls.Add(this.buttonFilterExpansions);
      this.groupBoxPriceEstim.Controls.Add(this.checkBoxFilterExpansions);
      this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceMultCopiesCap);
      this.groupBoxPriceEstim.Controls.Add(this.labelMultCopies4);
      this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceMultCopies4);
      this.groupBoxPriceEstim.Controls.Add(this.labelMultCopiesThree);
      this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceMultCopies3);
      this.groupBoxPriceEstim.Controls.Add(this.labelMultiplesTwo);
      this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceMultCopies2);
      this.groupBoxPriceEstim.Controls.Add(this.labelMultiples);
      this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMaximumPrice);
      this.groupBoxPriceEstim.Controls.Add(this.textBoxPriceEstMaxChange);
      this.groupBoxPriceEstim.Controls.Add(this.numericUpDownPriceEstMinPrice);
      this.groupBoxPriceEstim.Controls.Add(this.labelPriceEstMinPrice);
      this.groupBoxPriceEstim.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxPriceEstim.Location = new System.Drawing.Point(3, 3);
      this.groupBoxPriceEstim.Name = "groupBoxPriceEstim";
      this.groupBoxPriceEstim.Size = new System.Drawing.Size(806, 111);
      this.groupBoxPriceEstim.TabIndex = 12;
      this.groupBoxPriceEstim.TabStop = false;
      this.groupBoxPriceEstim.Text = "Pricing rules";
      // 
      // comboBoxPriceEstMinPriceMatch
      // 
      this.comboBoxPriceEstMinPriceMatch.CausesValidation = false;
      this.comboBoxPriceEstMinPriceMatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPriceEstMinPriceMatch.FormattingEnabled = true;
      this.comboBoxPriceEstMinPriceMatch.Items.AddRange(new object[] {
            "Highest",
            "Best"});
      this.comboBoxPriceEstMinPriceMatch.Location = new System.Drawing.Point(307, 16);
      this.comboBoxPriceEstMinPriceMatch.Name = "comboBoxPriceEstMinPriceMatch";
      this.comboBoxPriceEstMinPriceMatch.Size = new System.Drawing.Size(141, 21);
      this.comboBoxPriceEstMinPriceMatch.TabIndex = 67;
      this.toolTip1.SetToolTip(this.comboBoxPriceEstMinPriceMatch, resources.GetString("comboBoxPriceEstMinPriceMatch.ToolTip"));
      // 
      // labelPriceEstMinPriceMatch
      // 
      this.labelPriceEstMinPriceMatch.AutoSize = true;
      this.labelPriceEstMinPriceMatch.Location = new System.Drawing.Point(225, 21);
      this.labelPriceEstMinPriceMatch.Name = "labelPriceEstMinPriceMatch";
      this.labelPriceEstMinPriceMatch.Size = new System.Drawing.Size(83, 13);
      this.labelPriceEstMinPriceMatch.TabIndex = 66;
      this.labelPriceEstMinPriceMatch.Text = "MinPrice match:";
      this.toolTip1.SetToolTip(this.labelPriceEstMinPriceMatch, resources.GetString("labelPriceEstMinPriceMatch.ToolTip"));
      // 
      // comboBoxPriceEstUpdateMode
      // 
      this.comboBoxPriceEstUpdateMode.CausesValidation = false;
      this.comboBoxPriceEstUpdateMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPriceEstUpdateMode.FormattingEnabled = true;
      this.comboBoxPriceEstUpdateMode.Items.AddRange(new object[] {
            "Full update - TOSS",
            "TOSS only below minPrice",
            "Only ensure minPrice"});
      this.comboBoxPriceEstUpdateMode.Location = new System.Drawing.Point(529, 16);
      this.comboBoxPriceEstUpdateMode.Name = "comboBoxPriceEstUpdateMode";
      this.comboBoxPriceEstUpdateMode.Size = new System.Drawing.Size(144, 21);
      this.comboBoxPriceEstUpdateMode.TabIndex = 65;
      this.toolTip1.SetToolTip(this.comboBoxPriceEstUpdateMode, "This is mainly related to how myStock.csv is used, see documentation for details");
      this.comboBoxPriceEstUpdateMode.SelectedIndexChanged += new System.EventHandler(this.comboBoxPriceEstUpdateMode_SelectedIndexChanged);
      // 
      // labelPriceEstUpdateMode
      // 
      this.labelPriceEstUpdateMode.AutoSize = true;
      this.labelPriceEstUpdateMode.Location = new System.Drawing.Point(456, 21);
      this.labelPriceEstUpdateMode.Name = "labelPriceEstUpdateMode";
      this.labelPriceEstUpdateMode.Size = new System.Drawing.Size(74, 13);
      this.labelPriceEstUpdateMode.TabIndex = 7;
      this.labelPriceEstUpdateMode.Text = "Update mode:";
      this.toolTip1.SetToolTip(this.labelPriceEstUpdateMode, "This is mainly related to how myStock.csv is used, see documentation for details");
      // 
      // labelMultCopiesCap
      // 
      this.labelMultCopiesCap.AutoSize = true;
      this.labelMultCopiesCap.Location = new System.Drawing.Point(483, 49);
      this.labelMultCopiesCap.Name = "labelMultCopiesCap";
      this.labelMultCopiesCap.Size = new System.Drawing.Size(93, 13);
      this.labelMultCopiesCap.TabIndex = 62;
      this.labelMultCopiesCap.Text = "Markup cap (€/£):";
      this.toolTip1.SetToolTip(this.labelMultCopiesCap, "If the estimated price would increased by more than the specified cap by the adde" +
        "d markup, it is increased only by the cap value");
      // 
      // numericUpDownPriceMultCopiesCap
      // 
      this.numericUpDownPriceMultCopiesCap.DecimalPlaces = 2;
      this.numericUpDownPriceMultCopiesCap.Location = new System.Drawing.Point(577, 46);
      this.numericUpDownPriceMultCopiesCap.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
      this.numericUpDownPriceMultCopiesCap.Name = "numericUpDownPriceMultCopiesCap";
      this.numericUpDownPriceMultCopiesCap.Size = new System.Drawing.Size(70, 20);
      this.numericUpDownPriceMultCopiesCap.TabIndex = 61;
      this.toolTip1.SetToolTip(this.numericUpDownPriceMultCopiesCap, "If the estimated price would increase by more than the specified cap by the added" +
        " markup, it is increased only by the cap value");
      // 
      // labelMultCopies4
      // 
      this.labelMultCopies4.AutoSize = true;
      this.labelMultCopies4.Location = new System.Drawing.Point(371, 48);
      this.labelMultCopies4.Name = "labelMultCopies4";
      this.labelMultCopies4.Size = new System.Drawing.Size(54, 13);
      this.labelMultCopies4.TabIndex = 60;
      this.labelMultCopies4.Text = "4 or more:";
      this.toolTip1.SetToolTip(this.labelMultCopies4, "Warning: playsets are not affected by this markup (but they are also priced only " +
        "based on other playsets on sale)");
      // 
      // numericUpDownPriceMultCopies4
      // 
      this.numericUpDownPriceMultCopies4.DecimalPlaces = 1;
      this.numericUpDownPriceMultCopies4.Location = new System.Drawing.Point(426, 46);
      this.numericUpDownPriceMultCopies4.Name = "numericUpDownPriceMultCopies4";
      this.numericUpDownPriceMultCopies4.Size = new System.Drawing.Size(50, 20);
      this.numericUpDownPriceMultCopies4.TabIndex = 59;
      this.toolTip1.SetToolTip(this.numericUpDownPriceMultCopies4, "Warning: playsets are not affected by this markup (but they are also priced only " +
        "based on other playsets on sale).");
      // 
      // labelMultCopiesThree
      // 
      this.labelMultCopiesThree.AutoSize = true;
      this.labelMultCopiesThree.Location = new System.Drawing.Point(267, 48);
      this.labelMultCopiesThree.Name = "labelMultCopiesThree";
      this.labelMultCopiesThree.Size = new System.Drawing.Size(50, 13);
      this.labelMultCopiesThree.TabIndex = 58;
      this.labelMultCopiesThree.Text = "3 copies:";
      // 
      // numericUpDownPriceMultCopies3
      // 
      this.numericUpDownPriceMultCopies3.DecimalPlaces = 1;
      this.numericUpDownPriceMultCopies3.Location = new System.Drawing.Point(317, 46);
      this.numericUpDownPriceMultCopies3.Name = "numericUpDownPriceMultCopies3";
      this.numericUpDownPriceMultCopies3.Size = new System.Drawing.Size(50, 20);
      this.numericUpDownPriceMultCopies3.TabIndex = 57;
      // 
      // labelMultiplesTwo
      // 
      this.labelMultiplesTwo.AutoSize = true;
      this.labelMultiplesTwo.Location = new System.Drawing.Point(166, 48);
      this.labelMultiplesTwo.Name = "labelMultiplesTwo";
      this.labelMultiplesTwo.Size = new System.Drawing.Size(50, 13);
      this.labelMultiplesTwo.TabIndex = 56;
      this.labelMultiplesTwo.Text = "2 copies:";
      // 
      // numericUpDownPriceMultCopies2
      // 
      this.numericUpDownPriceMultCopies2.DecimalPlaces = 1;
      this.numericUpDownPriceMultCopies2.Location = new System.Drawing.Point(216, 46);
      this.numericUpDownPriceMultCopies2.Name = "numericUpDownPriceMultCopies2";
      this.numericUpDownPriceMultCopies2.Size = new System.Drawing.Size(50, 20);
      this.numericUpDownPriceMultCopies2.TabIndex = 55;
      // 
      // labelMultiples
      // 
      this.labelMultiples.AutoSize = true;
      this.labelMultiples.Location = new System.Drawing.Point(8, 48);
      this.labelMultiples.Name = "labelMultiples";
      this.labelMultiples.Size = new System.Drawing.Size(161, 13);
      this.labelMultiples.TabIndex = 54;
      this.labelMultiples.Text = "Markup for multiple copies (in %):";
      this.toolTip1.SetToolTip(this.labelMultiples, "If you have multiple copies of a given card, its estimated price will be increase" +
        "d by the specified percentage of the estimate for 2, 3 and 4 or more copies, up " +
        "to the specified cap");
      // 
      // labelPriceEstMaximumPrice
      // 
      this.labelPriceEstMaximumPrice.AutoSize = true;
      this.labelPriceEstMaximumPrice.Location = new System.Drawing.Point(8, 75);
      this.labelPriceEstMaximumPrice.Name = "labelPriceEstMaximumPrice";
      this.labelPriceEstMaximumPrice.Size = new System.Drawing.Size(95, 13);
      this.labelPriceEstMaximumPrice.TabIndex = 17;
      this.labelPriceEstMaximumPrice.Text = "Max price change:";
      this.toolTip1.SetToolTip(this.labelPriceEstMaximumPrice, "insert sequence \"T1;C1;T2;C2;\" etc., where Cx is max allowed price change in % fo" +
        "r items that cost (old price) Tx or less");
      // 
      // textBoxPriceEstMaxChange
      // 
      this.textBoxPriceEstMaxChange.Location = new System.Drawing.Point(168, 72);
      this.textBoxPriceEstMaxChange.Name = "textBoxPriceEstMaxChange";
      this.textBoxPriceEstMaxChange.Size = new System.Drawing.Size(637, 20);
      this.textBoxPriceEstMaxChange.TabIndex = 18;
      this.toolTip1.SetToolTip(this.textBoxPriceEstMaxChange, "insert sequence \"T1;C1;T2;C2;\" etc., where Cx is max allowed price change in % fo" +
        "r items that cost (old price) Tx or less");
      // 
      // numericUpDownPriceEstMinPrice
      // 
      this.numericUpDownPriceEstMinPrice.DecimalPlaces = 2;
      this.numericUpDownPriceEstMinPrice.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
      this.numericUpDownPriceEstMinPrice.Location = new System.Drawing.Point(168, 19);
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
      this.numericUpDownPriceEstMinPrice.Size = new System.Drawing.Size(49, 20);
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
      this.labelPriceEstMinPrice.Location = new System.Drawing.Point(8, 21);
      this.labelPriceEstMinPrice.Name = "labelPriceEstMinPrice";
      this.labelPriceEstMinPrice.Size = new System.Drawing.Size(141, 13);
      this.labelPriceEstMinPrice.TabIndex = 15;
      this.labelPriceEstMinPrice.Text = "Minimum price of rares [€/£]:";
      // 
      // numericUpDownPriceEstMaxN
      // 
      this.numericUpDownPriceEstMaxN.Location = new System.Drawing.Point(378, 22);
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
      this.numericUpDownPriceEstMinN.Location = new System.Drawing.Point(144, 22);
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
      this.labelPriceEstMaxN.Location = new System.Drawing.Point(238, 24);
      this.labelPriceEstMaxN.Name = "labelPriceEstMaxN";
      this.labelPriceEstMaxN.Size = new System.Drawing.Size(134, 13);
      this.labelPriceEstMaxN.TabIndex = 32;
      this.labelPriceEstMaxN.Text = "Maximum # of similar items:";
      // 
      // labelPriceEstMinN
      // 
      this.labelPriceEstMinN.AutoSize = true;
      this.labelPriceEstMinN.Location = new System.Drawing.Point(7, 24);
      this.labelPriceEstMinN.Name = "labelPriceEstMinN";
      this.labelPriceEstMinN.Size = new System.Drawing.Size(131, 13);
      this.labelPriceEstMinN.TabIndex = 31;
      this.labelPriceEstMinN.Text = "Minimum # of similar items:";
      // 
      // checkBoxPricePlaysetIgnore
      // 
      this.checkBoxPricePlaysetIgnore.AutoSize = true;
      this.checkBoxPricePlaysetIgnore.Location = new System.Drawing.Point(479, 23);
      this.checkBoxPricePlaysetIgnore.Name = "checkBoxPricePlaysetIgnore";
      this.checkBoxPricePlaysetIgnore.Size = new System.Drawing.Size(150, 17);
      this.checkBoxPricePlaysetIgnore.TabIndex = 63;
      this.checkBoxPricePlaysetIgnore.Text = "Treat playsets as 4 singles";
      this.toolTip1.SetToolTip(this.checkBoxPricePlaysetIgnore, resources.GetString("checkBoxPricePlaysetIgnore.ToolTip"));
      this.checkBoxPricePlaysetIgnore.UseVisualStyleBackColor = true;
      // 
      // checkBoxPriceEstWorldwide
      // 
      this.checkBoxPriceEstWorldwide.AutoSize = true;
      this.checkBoxPriceEstWorldwide.Location = new System.Drawing.Point(661, 23);
      this.checkBoxPriceEstWorldwide.Name = "checkBoxPriceEstWorldwide";
      this.checkBoxPriceEstWorldwide.Size = new System.Drawing.Size(136, 17);
      this.checkBoxPriceEstWorldwide.TabIndex = 50;
      this.checkBoxPriceEstWorldwide.Text = "Allow worldwide search";
      this.toolTip1.SetToolTip(this.checkBoxPriceEstWorldwide, "If minimum number of items (before any culling) is not found in your country, ite" +
        "ms from all sellers will be considered");
      this.checkBoxPriceEstWorldwide.UseVisualStyleBackColor = true;
      this.checkBoxPriceEstWorldwide.CheckedChanged += new System.EventHandler(this.checkBoxPriceEstWorldwide_CheckedChanged);
      // 
      // textBoxPriceEstMaxDiff
      // 
      this.textBoxPriceEstMaxDiff.Location = new System.Drawing.Point(225, 47);
      this.textBoxPriceEstMaxDiff.Name = "textBoxPriceEstMaxDiff";
      this.textBoxPriceEstMaxDiff.Size = new System.Drawing.Size(572, 20);
      this.textBoxPriceEstMaxDiff.TabIndex = 49;
      this.toolTip1.SetToolTip(this.textBoxPriceEstMaxDiff, "insert sequence \"T1;C1;T2;C2;\" etc., where Cx = max difference in % for items tha" +
        "t cost Tx or less");
      // 
      // labelPriceEstAvgOutliers1
      // 
      this.labelPriceEstAvgOutliers1.AutoSize = true;
      this.labelPriceEstAvgOutliers1.Location = new System.Drawing.Point(8, 50);
      this.labelPriceEstAvgOutliers1.Name = "labelPriceEstAvgOutliers1";
      this.labelPriceEstAvgOutliers1.Size = new System.Drawing.Size(217, 13);
      this.labelPriceEstAvgOutliers1.TabIndex = 48;
      this.labelPriceEstAvgOutliers1.Text = "Max differences between consecutive items:";
      this.toolTip1.SetToolTip(this.labelPriceEstAvgOutliers1, "insert sequence \"T1;C1;T2;C2;\" etc., where Cx = max difference in % for items tha" +
        "t cost Tx or less");
      // 
      // labelWorlwideAvg
      // 
      this.labelWorlwideAvg.AutoSize = true;
      this.labelWorlwideAvg.Location = new System.Drawing.Point(21, 233);
      this.labelWorlwideAvg.Name = "labelWorlwideAvg";
      this.labelWorlwideAvg.Size = new System.Drawing.Size(179, 13);
      this.labelWorlwideAvg.TabIndex = 64;
      this.labelWorlwideAvg.Text = "Price based on average - worldwide:";
      // 
      // panelPriceEstWorldForSliderLabel
      // 
      this.panelPriceEstWorldForSliderLabel.Controls.Add(this.labelPriceEstSliderValueWorld);
      this.panelPriceEstWorldForSliderLabel.Location = new System.Drawing.Point(255, 262);
      this.panelPriceEstWorldForSliderLabel.Name = "panelPriceEstWorldForSliderLabel";
      this.panelPriceEstWorldForSliderLabel.Size = new System.Drawing.Size(522, 29);
      this.panelPriceEstWorldForSliderLabel.TabIndex = 53;
      // 
      // labelPriceEstSliderValueWorld
      // 
      this.labelPriceEstSliderValueWorld.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelPriceEstSliderValueWorld.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
      this.labelPriceEstSliderValueWorld.Location = new System.Drawing.Point(0, 0);
      this.labelPriceEstSliderValueWorld.Name = "labelPriceEstSliderValueWorld";
      this.labelPriceEstSliderValueWorld.Size = new System.Drawing.Size(522, 29);
      this.labelPriceEstSliderValueWorld.TabIndex = 0;
      this.labelPriceEstSliderValueWorld.Text = "AVG";
      this.labelPriceEstSliderValueWorld.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // panelPriceEstForSliderLabel
      // 
      this.panelPriceEstForSliderLabel.Controls.Add(this.labelPriceEstSliderValue);
      this.panelPriceEstForSliderLabel.Location = new System.Drawing.Point(255, 197);
      this.panelPriceEstForSliderLabel.Name = "panelPriceEstForSliderLabel";
      this.panelPriceEstForSliderLabel.Size = new System.Drawing.Size(522, 29);
      this.panelPriceEstForSliderLabel.TabIndex = 52;
      // 
      // labelPriceEstSliderValue
      // 
      this.labelPriceEstSliderValue.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelPriceEstSliderValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
      this.labelPriceEstSliderValue.Location = new System.Drawing.Point(0, 0);
      this.labelPriceEstSliderValue.Name = "labelPriceEstSliderValue";
      this.labelPriceEstSliderValue.Size = new System.Drawing.Size(522, 29);
      this.labelPriceEstSliderValue.TabIndex = 0;
      this.labelPriceEstSliderValue.Text = "AVG";
      this.labelPriceEstSliderValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // trackBarPriceEstAvgWorld
      // 
      this.trackBarPriceEstAvgWorld.Location = new System.Drawing.Point(247, 233);
      this.trackBarPriceEstAvgWorld.Maximum = 50;
      this.trackBarPriceEstAvgWorld.Name = "trackBarPriceEstAvgWorld";
      this.trackBarPriceEstAvgWorld.Size = new System.Drawing.Size(534, 45);
      this.trackBarPriceEstAvgWorld.TabIndex = 51;
      this.trackBarPriceEstAvgWorld.Value = 25;
      this.trackBarPriceEstAvgWorld.ValueChanged += new System.EventHandler(this.trackBarPriceEstAvgWorld_ValueChanged);
      // 
      // trackBarPriceEstAvg
      // 
      this.trackBarPriceEstAvg.Location = new System.Drawing.Point(247, 172);
      this.trackBarPriceEstAvg.Maximum = 50;
      this.trackBarPriceEstAvg.Name = "trackBarPriceEstAvg";
      this.trackBarPriceEstAvg.Size = new System.Drawing.Size(535, 45);
      this.trackBarPriceEstAvg.TabIndex = 36;
      this.trackBarPriceEstAvg.Value = 25;
      this.trackBarPriceEstAvg.ValueChanged += new System.EventHandler(this.trackBarPriceEstAvg_ValueChanged);
      // 
      // labelPriceEstHighestPrice
      // 
      this.labelPriceEstHighestPrice.AutoSize = true;
      this.labelPriceEstHighestPrice.Location = new System.Drawing.Point(302, 144);
      this.labelPriceEstHighestPrice.Name = "labelPriceEstHighestPrice";
      this.labelPriceEstHighestPrice.Size = new System.Drawing.Size(325, 13);
      this.labelPriceEstHighestPrice.TabIndex = 32;
      this.labelPriceEstHighestPrice.Text = "% of highest price (among the up to max # of cheapest similar items)";
      // 
      // numericUpDownPriceEstHighestPrice
      // 
      this.numericUpDownPriceEstHighestPrice.DecimalPlaces = 2;
      this.numericUpDownPriceEstHighestPrice.Enabled = false;
      this.numericUpDownPriceEstHighestPrice.Location = new System.Drawing.Point(226, 142);
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
      this.radioButtonPriceEstHighestPrice.Location = new System.Drawing.Point(11, 142);
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
      this.labelPriceEstLowestPrice.Location = new System.Drawing.Point(301, 112);
      this.labelPriceEstLowestPrice.Name = "labelPriceEstLowestPrice";
      this.labelPriceEstLowestPrice.Size = new System.Drawing.Size(408, 13);
      this.labelPriceEstLowestPrice.TabIndex = 29;
      this.labelPriceEstLowestPrice.Text = "% of lowest price (WARNING - this will not cut off outliers based on price differ" +
    "ences!)";
      // 
      // numericUpDownPriceEstLowestPrice
      // 
      this.numericUpDownPriceEstLowestPrice.DecimalPlaces = 2;
      this.numericUpDownPriceEstLowestPrice.Enabled = false;
      this.numericUpDownPriceEstLowestPrice.Location = new System.Drawing.Point(225, 110);
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
      this.radioButtonPriceEstByLowestPrice.Location = new System.Drawing.Point(11, 110);
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
      this.radioButtonPriceEstPriceByAvg.Location = new System.Drawing.Point(10, 172);
      this.radioButtonPriceEstPriceByAvg.Name = "radioButtonPriceEstPriceByAvg";
      this.radioButtonPriceEstPriceByAvg.Size = new System.Drawing.Size(192, 17);
      this.radioButtonPriceEstPriceByAvg.TabIndex = 25;
      this.radioButtonPriceEstPriceByAvg.TabStop = true;
      this.radioButtonPriceEstPriceByAvg.Text = "Price based on average - domestic:";
      this.radioButtonPriceEstPriceByAvg.UseVisualStyleBackColor = true;
      this.radioButtonPriceEstPriceByAvg.CheckedChanged += new System.EventHandler(this.radioButtonPriceEstPriceByAvg_CheckedChanged);
      // 
      // checkBoxTestMode
      // 
      this.checkBoxTestMode.AutoSize = true;
      this.checkBoxTestMode.Dock = System.Windows.Forms.DockStyle.Fill;
      this.checkBoxTestMode.Location = new System.Drawing.Point(3, 820);
      this.checkBoxTestMode.Name = "checkBoxTestMode";
      this.checkBoxTestMode.Size = new System.Drawing.Size(806, 92);
      this.checkBoxTestMode.TabIndex = 15;
      this.checkBoxTestMode.Text = "Test mode - do not send price updates to MKM";
      this.checkBoxTestMode.UseVisualStyleBackColor = true;
      // 
      // groupBoxPresets
      // 
      this.groupBoxPresets.Controls.Add(this.panelPresetsDescr);
      this.groupBoxPresets.Controls.Add(this.buttonPresetsDelete);
      this.groupBoxPresets.Controls.Add(this.buttonPresetsStore);
      this.groupBoxPresets.Controls.Add(this.buttonPresetsLoad);
      this.groupBoxPresets.Controls.Add(this.comboBoxPresets);
      this.groupBoxPresets.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxPresets.Location = new System.Drawing.Point(3, 715);
      this.groupBoxPresets.Name = "groupBoxPresets";
      this.groupBoxPresets.Size = new System.Drawing.Size(806, 99);
      this.groupBoxPresets.TabIndex = 16;
      this.groupBoxPresets.TabStop = false;
      this.groupBoxPresets.Text = "Presets";
      // 
      // panelPresetsDescr
      // 
      this.panelPresetsDescr.Controls.Add(this.labelPresetsDescr);
      this.panelPresetsDescr.Location = new System.Drawing.Point(375, 19);
      this.panelPresetsDescr.Name = "panelPresetsDescr";
      this.panelPresetsDescr.Size = new System.Drawing.Size(403, 74);
      this.panelPresetsDescr.TabIndex = 4;
      // 
      // labelPresetsDescr
      // 
      this.labelPresetsDescr.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelPresetsDescr.Location = new System.Drawing.Point(0, 0);
      this.labelPresetsDescr.Name = "labelPresetsDescr";
      this.labelPresetsDescr.Size = new System.Drawing.Size(403, 74);
      this.labelPresetsDescr.TabIndex = 1;
      // 
      // buttonPresetsDelete
      // 
      this.buttonPresetsDelete.Enabled = false;
      this.buttonPresetsDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonPresetsDelete.Location = new System.Drawing.Point(248, 59);
      this.buttonPresetsDelete.Name = "buttonPresetsDelete";
      this.buttonPresetsDelete.Size = new System.Drawing.Size(113, 34);
      this.buttonPresetsDelete.TabIndex = 3;
      this.buttonPresetsDelete.Text = "Delete Preset";
      this.buttonPresetsDelete.UseVisualStyleBackColor = true;
      this.buttonPresetsDelete.Click += new System.EventHandler(this.buttonPresetsDelete_Click);
      // 
      // buttonPresetsStore
      // 
      this.buttonPresetsStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonPresetsStore.Location = new System.Drawing.Point(129, 59);
      this.buttonPresetsStore.Name = "buttonPresetsStore";
      this.buttonPresetsStore.Size = new System.Drawing.Size(113, 34);
      this.buttonPresetsStore.TabIndex = 2;
      this.buttonPresetsStore.Text = "Store Preset...";
      this.buttonPresetsStore.UseVisualStyleBackColor = true;
      this.buttonPresetsStore.Click += new System.EventHandler(this.buttonPresetsStore_Click);
      // 
      // buttonPresetsLoad
      // 
      this.buttonPresetsLoad.Enabled = false;
      this.buttonPresetsLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonPresetsLoad.Location = new System.Drawing.Point(10, 59);
      this.buttonPresetsLoad.Name = "buttonPresetsLoad";
      this.buttonPresetsLoad.Size = new System.Drawing.Size(113, 34);
      this.buttonPresetsLoad.TabIndex = 1;
      this.buttonPresetsLoad.Text = "Load Preset";
      this.buttonPresetsLoad.UseVisualStyleBackColor = true;
      this.buttonPresetsLoad.Click += new System.EventHandler(this.buttonPresetsLoad_Click);
      // 
      // comboBoxPresets
      // 
      this.comboBoxPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxPresets.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.comboBoxPresets.FormattingEnabled = true;
      this.comboBoxPresets.Location = new System.Drawing.Point(10, 19);
      this.comboBoxPresets.Name = "comboBoxPresets";
      this.comboBoxPresets.Size = new System.Drawing.Size(351, 24);
      this.comboBoxPresets.TabIndex = 0;
      this.comboBoxPresets.DropDown += new System.EventHandler(this.comboBoxPresets_DropDown);
      this.comboBoxPresets.SelectedIndexChanged += new System.EventHandler(this.comboBoxPresets_SelectedIndexChanged);
      // 
      // checkBoxFilterCountries
      // 
      this.checkBoxFilterCountries.AutoSize = true;
      this.checkBoxFilterCountries.Location = new System.Drawing.Point(529, 79);
      this.checkBoxFilterCountries.Name = "checkBoxFilterCountries";
      this.checkBoxFilterCountries.Size = new System.Drawing.Size(121, 17);
      this.checkBoxFilterCountries.TabIndex = 2;
      this.checkBoxFilterCountries.Text = "Filter seller countries";
      this.toolTip1.SetToolTip(this.checkBoxFilterCountries, "Use the button below to choose countries. Only sellers from countries will be use" +
        "d to compute price. Worlwide search must be activated.");
      this.checkBoxFilterCountries.UseVisualStyleBackColor = true;
      this.checkBoxFilterCountries.CheckedChanged += new System.EventHandler(this.checkBoxFilterCountries_CheckedChanged);
      // 
      // checkBoxFilterExpansions
      // 
      this.checkBoxFilterExpansions.AutoSize = true;
      this.checkBoxFilterExpansions.Location = new System.Drawing.Point(693, 22);
      this.checkBoxFilterExpansions.Name = "checkBoxFilterExpansions";
      this.checkBoxFilterExpansions.Size = new System.Drawing.Size(104, 17);
      this.checkBoxFilterExpansions.TabIndex = 0;
      this.checkBoxFilterExpansions.Text = "Filter expansions";
      this.toolTip1.SetToolTip(this.checkBoxFilterExpansions, "Use the button below to select expansions. Only cards from those expansions will " +
        "be updated.");
      this.checkBoxFilterExpansions.UseVisualStyleBackColor = true;
      this.checkBoxFilterExpansions.CheckedChanged += new System.EventHandler(this.checkBoxFilterExpansions_CheckedChanged);
      // 
      // checkBoxFilterPowerseller
      // 
      this.checkBoxFilterPowerseller.AutoSize = true;
      this.checkBoxFilterPowerseller.Checked = true;
      this.checkBoxFilterPowerseller.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxFilterPowerseller.Location = new System.Drawing.Point(309, 79);
      this.checkBoxFilterPowerseller.Name = "checkBoxFilterPowerseller";
      this.checkBoxFilterPowerseller.Size = new System.Drawing.Size(122, 17);
      this.checkBoxFilterPowerseller.TabIndex = 6;
      this.checkBoxFilterPowerseller.Text = "Include powersellers";
      this.checkBoxFilterPowerseller.UseVisualStyleBackColor = true;
      // 
      // checkBoxFilterProfSeller
      // 
      this.checkBoxFilterProfSeller.AutoSize = true;
      this.checkBoxFilterProfSeller.Checked = true;
      this.checkBoxFilterProfSeller.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxFilterProfSeller.Location = new System.Drawing.Point(149, 79);
      this.checkBoxFilterProfSeller.Name = "checkBoxFilterProfSeller";
      this.checkBoxFilterProfSeller.Size = new System.Drawing.Size(152, 17);
      this.checkBoxFilterProfSeller.TabIndex = 5;
      this.checkBoxFilterProfSeller.Text = "Include professional sellers";
      this.checkBoxFilterProfSeller.UseVisualStyleBackColor = true;
      // 
      // checkBoxFilterPrivSeller
      // 
      this.checkBoxFilterPrivSeller.AutoSize = true;
      this.checkBoxFilterPrivSeller.Checked = true;
      this.checkBoxFilterPrivSeller.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxFilterPrivSeller.Location = new System.Drawing.Point(11, 79);
      this.checkBoxFilterPrivSeller.Name = "checkBoxFilterPrivSeller";
      this.checkBoxFilterPrivSeller.Size = new System.Drawing.Size(128, 17);
      this.checkBoxFilterPrivSeller.TabIndex = 4;
      this.checkBoxFilterPrivSeller.Text = "Include private sellers";
      this.checkBoxFilterPrivSeller.UseVisualStyleBackColor = true;
      // 
      // buttonFilterCountries
      // 
      this.buttonFilterCountries.Enabled = false;
      this.buttonFilterCountries.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
      this.buttonFilterCountries.Location = new System.Drawing.Point(645, 75);
      this.buttonFilterCountries.Name = "buttonFilterCountries";
      this.buttonFilterCountries.Size = new System.Drawing.Size(144, 23);
      this.buttonFilterCountries.TabIndex = 3;
      this.buttonFilterCountries.Text = "Select Countries...";
      this.buttonFilterCountries.UseVisualStyleBackColor = true;
      this.buttonFilterCountries.Click += new System.EventHandler(this.buttonFilterCountries_Click);
      // 
      // buttonFilterExpansions
      // 
      this.buttonFilterExpansions.Enabled = false;
      this.buttonFilterExpansions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonFilterExpansions.Location = new System.Drawing.Point(679, 40);
      this.buttonFilterExpansions.Name = "buttonFilterExpansions";
      this.buttonFilterExpansions.Size = new System.Drawing.Size(126, 29);
      this.buttonFilterExpansions.TabIndex = 1;
      this.buttonFilterExpansions.Text = "Select Expansions...";
      this.buttonFilterExpansions.UseVisualStyleBackColor = true;
      this.buttonFilterExpansions.Click += new System.EventHandler(this.buttonFilterExpansions_Click);
      // 
      // groupBoxTraversal
      // 
      this.groupBoxTraversal.AutoSize = true;
      this.groupBoxTraversal.Controls.Add(this.checkBoxFilterPowerseller);
      this.groupBoxTraversal.Controls.Add(this.radioButtonPriceEstHighestPrice);
      this.groupBoxTraversal.Controls.Add(this.checkBoxFilterProfSeller);
      this.groupBoxTraversal.Controls.Add(this.checkBoxFilterPrivSeller);
      this.groupBoxTraversal.Controls.Add(this.numericUpDownPriceEstHighestPrice);
      this.groupBoxTraversal.Controls.Add(this.buttonFilterCountries);
      this.groupBoxTraversal.Controls.Add(this.labelPriceEstHighestPrice);
      this.groupBoxTraversal.Controls.Add(this.checkBoxFilterCountries);
      this.groupBoxTraversal.Controls.Add(this.checkBoxPricePlaysetIgnore);
      this.groupBoxTraversal.Controls.Add(this.groupBoxConditionSettings);
      this.groupBoxTraversal.Controls.Add(this.radioButtonPriceEstByLowestPrice);
      this.groupBoxTraversal.Controls.Add(this.labelWorlwideAvg);
      this.groupBoxTraversal.Controls.Add(this.numericUpDownPriceEstLowestPrice);
      this.groupBoxTraversal.Controls.Add(this.labelPriceEstLowestPrice);
      this.groupBoxTraversal.Controls.Add(this.radioButtonPriceEstPriceByAvg);
      this.groupBoxTraversal.Controls.Add(this.panelPriceEstForSliderLabel);
      this.groupBoxTraversal.Controls.Add(this.panelPriceEstWorldForSliderLabel);
      this.groupBoxTraversal.Controls.Add(this.trackBarPriceEstAvgWorld);
      this.groupBoxTraversal.Controls.Add(this.trackBarPriceEstAvg);
      this.groupBoxTraversal.Controls.Add(this.numericUpDownPriceEstMaxN);
      this.groupBoxTraversal.Controls.Add(this.checkBoxPriceEstWorldwide);
      this.groupBoxTraversal.Controls.Add(this.numericUpDownPriceEstMinN);
      this.groupBoxTraversal.Controls.Add(this.labelPriceEstAvgOutliers1);
      this.groupBoxTraversal.Controls.Add(this.textBoxPriceEstMaxDiff);
      this.groupBoxTraversal.Controls.Add(this.labelPriceEstMaxN);
      this.groupBoxTraversal.Controls.Add(this.labelPriceEstMinN);
      this.groupBoxTraversal.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBoxTraversal.Location = new System.Drawing.Point(3, 216);
      this.groupBoxTraversal.Name = "groupBoxTraversal";
      this.groupBoxTraversal.Size = new System.Drawing.Size(806, 388);
      this.groupBoxTraversal.TabIndex = 68;
      this.groupBoxTraversal.TabStop = false;
      this.groupBoxTraversal.Text = "Pricing by Traversal of Other Seller\'s Stock (TOSS)";
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.AutoSize = true;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.Controls.Add(this.groupBoxPriceEstim, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.groupBoxPriceGuides, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.groupBoxTraversal, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.groupBoxLogSettings, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.groupBoxPresets, 0, 4);
      this.tableLayoutPanel1.Controls.Add(this.checkBoxTestMode, 0, 5);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 6;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(812, 915);
      this.tableLayoutPanel1.TabIndex = 68;
      // 
      // UpdatePriceSettings
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ClientSize = new System.Drawing.Size(812, 915);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "UpdatePriceSettings";
      this.Text = "Settings of Update Price";
      this.groupBoxLogSettings.ResumeLayout(false);
      this.groupBoxLogSettings.PerformLayout();
      this.groupBoxPriceGuides.ResumeLayout(false);
      this.groupBoxPriceGuides.PerformLayout();
      this.groupBoxConditionSettings.ResumeLayout(false);
      this.groupBoxConditionSettings.PerformLayout();
      this.groupBoxPriceEstim.ResumeLayout(false);
      this.groupBoxPriceEstim.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceMultCopiesCap)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceMultCopies4)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceMultCopies3)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceMultCopies2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinPrice)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMaxN)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstMinN)).EndInit();
      this.panelPriceEstWorldForSliderLabel.ResumeLayout(false);
      this.panelPriceEstForSliderLabel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.trackBarPriceEstAvgWorld)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarPriceEstAvg)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstHighestPrice)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPriceEstLowestPrice)).EndInit();
      this.groupBoxPresets.ResumeLayout(false);
      this.panelPresetsDescr.ResumeLayout(false);
      this.groupBoxTraversal.ResumeLayout(false);
      this.groupBoxTraversal.PerformLayout();
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBoxLogSettings;
    private System.Windows.Forms.CheckBox checkBoxLogLargeChangeLow;
    private System.Windows.Forms.CheckBox checkBoxLogSmallChange;
    private System.Windows.Forms.CheckBox checkBoxLogMinItems;
    private System.Windows.Forms.CheckBox checkBoxLogUpdated;
    private System.Windows.Forms.GroupBox groupBoxConditionSettings;
    private System.Windows.Forms.RadioButton radioButtonCondMatchesAbove;
    private System.Windows.Forms.RadioButton radioButtonCondAcceptBetterAlways;
    private System.Windows.Forms.Label labelMatchExplanation;
    private System.Windows.Forms.RadioButton radioButtonCondMatchOnly;
    private System.Windows.Forms.GroupBox groupBoxPriceEstim;
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
    private System.Windows.Forms.Label labelPriceEstAvgOutliers1;
    private System.Windows.Forms.CheckBox checkBoxTestMode;
    private System.Windows.Forms.TextBox textBoxPriceEstMaxDiff;
    private System.Windows.Forms.CheckBox checkBoxLogHighVariance;
    private System.Windows.Forms.GroupBox groupBoxPresets;
    private System.Windows.Forms.Panel panelPresetsDescr;
    private System.Windows.Forms.Button buttonPresetsDelete;
    private System.Windows.Forms.Button buttonPresetsStore;
    private System.Windows.Forms.Button buttonPresetsLoad;
    private System.Windows.Forms.ComboBox comboBoxPresets;
    private System.Windows.Forms.Label labelPresetsDescr;
    private System.Windows.Forms.CheckBox checkBoxPriceEstWorldwide;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Panel panelPriceEstWorldForSliderLabel;
    private System.Windows.Forms.Panel panelPriceEstForSliderLabel;
    private System.Windows.Forms.Label labelPriceEstSliderValue;
    private System.Windows.Forms.TrackBar trackBarPriceEstAvgWorld;
    private System.Windows.Forms.Label labelPriceEstSliderValueWorld;
    private System.Windows.Forms.Label labelMultCopiesCap;
    private System.Windows.Forms.NumericUpDown numericUpDownPriceMultCopiesCap;
    private System.Windows.Forms.Label labelMultCopies4;
    private System.Windows.Forms.NumericUpDown numericUpDownPriceMultCopies4;
    private System.Windows.Forms.Label labelMultCopiesThree;
    private System.Windows.Forms.NumericUpDown numericUpDownPriceMultCopies3;
    private System.Windows.Forms.Label labelMultiplesTwo;
    private System.Windows.Forms.NumericUpDown numericUpDownPriceMultCopies2;
    private System.Windows.Forms.Label labelMultiples;
    private System.Windows.Forms.Label labelWorlwideAvg;
    private System.Windows.Forms.CheckBox checkBoxPricePlaysetIgnore;
    private System.Windows.Forms.CheckBox checkBoxLogLargeChangeHigh;
    private System.Windows.Forms.Button buttonFilterExpansions;
    private System.Windows.Forms.CheckBox checkBoxFilterExpansions;
    private System.Windows.Forms.Button buttonFilterCountries;
    private System.Windows.Forms.CheckBox checkBoxFilterCountries;
    private System.Windows.Forms.CheckBox checkBoxFilterPowerseller;
    private System.Windows.Forms.CheckBox checkBoxFilterProfSeller;
    private System.Windows.Forms.CheckBox checkBoxFilterPrivSeller;
    private System.Windows.Forms.ComboBox comboBoxPriceEstUpdateMode;
    private System.Windows.Forms.Label labelPriceEstUpdateMode;
    private System.Windows.Forms.ComboBox comboBoxPriceEstMinPriceMatch;
    private System.Windows.Forms.Label labelPriceEstMinPriceMatch;
    private System.Windows.Forms.GroupBox groupBoxTraversal;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.GroupBox groupBoxPriceGuides;
    private System.Windows.Forms.CheckBox checkBoxPriceGuidesLogNotFound;
    private System.Windows.Forms.Label labelPriceGuidesNotFound;
    private System.Windows.Forms.CheckBox checkBoxPriceGuidesTraverseNotFound;
    private System.Windows.Forms.ComboBox comboBoxPriceGuidesFoils;
    private System.Windows.Forms.Label labelPriceGuidesFoils;
    private System.Windows.Forms.TextBox textBoxPriceGuidesFoils;
    private System.Windows.Forms.ComboBox comboBoxPriceGuidesNonFoils;
    private System.Windows.Forms.Label labelPriceGuidesNonFoils;
    private System.Windows.Forms.TextBox textBoxPriceGuidesNonFoil;
    private System.Windows.Forms.Label labelPriceGuidesModifiersFoils;
    private System.Windows.Forms.Label labelPriceGuidesModifiersNonfoils;
  }
}
