namespace MKMTool
{
    partial class PriceExternalList
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
            this.groupBoxImport = new System.Windows.Forms.GroupBox();
            this.checkBoxImportLog = new System.Windows.Forms.CheckBox();
            this.buttonImport = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxCondition = new System.Windows.Forms.ComboBox();
            this.labelCondition = new System.Windows.Forms.Label();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.comboBoxExpansion = new System.Windows.Forms.ComboBox();
            this.labelExpansion = new System.Windows.Forms.Label();
            this.comboBoxPlayset = new System.Windows.Forms.ComboBox();
            this.labelPlayset = new System.Windows.Forms.Label();
            this.comboBoxAltered = new System.Windows.Forms.ComboBox();
            this.labelAltered = new System.Windows.Forms.Label();
            this.comboBoxSigned = new System.Windows.Forms.ComboBox();
            this.labelSinged = new System.Windows.Forms.Label();
            this.comboBoxFoil = new System.Windows.Forms.ComboBox();
            this.labelFoil = new System.Windows.Forms.Label();
            this.groupBoxPrice = new System.Windows.Forms.GroupBox();
            this.buttonBotSettings = new System.Windows.Forms.Button();
            this.buttonAppraise = new System.Windows.Forms.Button();
            this.checkBoxToolPrices = new System.Windows.Forms.CheckBox();
            this.checkBoxPriceGuide = new System.Windows.Forms.CheckBox();
            this.groupBoxExport = new System.Windows.Forms.GroupBox();
            this.groupBoxExportMKM = new System.Windows.Forms.GroupBox();
            this.checkBoxExportLogAll = new System.Windows.Forms.CheckBox();
            this.buttonExportToMKM = new System.Windows.Forms.Button();
            this.comboBoxExportUploadPrice = new System.Windows.Forms.ComboBox();
            this.labelExportWhichPrice = new System.Windows.Forms.Label();
            this.groupBoxExportFile = new System.Windows.Forms.GroupBox();
            this.checkBoxExportAll = new System.Windows.Forms.CheckBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.checkBoxExportToolPrices = new System.Windows.Forms.CheckBox();
            this.checkBoxExportFormatDeckbox = new System.Windows.Forms.CheckBox();
            this.checkBoxExportPriceGuide = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxExportOnlyAppraised = new System.Windows.Forms.CheckBox();
            this.groupBoxImport.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxPrice.SuspendLayout();
            this.groupBoxExport.SuspendLayout();
            this.groupBoxExportMKM.SuspendLayout();
            this.groupBoxExportFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxImport
            // 
            this.groupBoxImport.Controls.Add(this.checkBoxImportLog);
            this.groupBoxImport.Controls.Add(this.buttonImport);
            this.groupBoxImport.Controls.Add(this.groupBox1);
            this.groupBoxImport.Location = new System.Drawing.Point(10, 12);
            this.groupBoxImport.Name = "groupBoxImport";
            this.groupBoxImport.Size = new System.Drawing.Size(496, 235);
            this.groupBoxImport.TabIndex = 0;
            this.groupBoxImport.TabStop = false;
            this.groupBoxImport.Text = "Import";
            // 
            // checkBoxImportLog
            // 
            this.checkBoxImportLog.AutoSize = true;
            this.checkBoxImportLog.Location = new System.Drawing.Point(15, 132);
            this.checkBoxImportLog.Name = "checkBoxImportLog";
            this.checkBoxImportLog.Size = new System.Drawing.Size(136, 17);
            this.checkBoxImportLog.TabIndex = 13;
            this.checkBoxImportLog.Text = "Log all imported articles";
            this.toolTip1.SetToolTip(this.checkBoxImportLog, "If turned off, only articles for which import fails will be logged.");
            this.checkBoxImportLog.UseVisualStyleBackColor = true;
            // 
            // buttonImport
            // 
            this.buttonImport.Location = new System.Drawing.Point(170, 166);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(142, 54);
            this.buttonImport.TabIndex = 12;
            this.buttonImport.Text = "Import CSV file...";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxCondition);
            this.groupBox1.Controls.Add(this.labelCondition);
            this.groupBox1.Controls.Add(this.comboBoxLanguage);
            this.groupBox1.Controls.Add(this.labelLanguage);
            this.groupBox1.Controls.Add(this.comboBoxExpansion);
            this.groupBox1.Controls.Add(this.labelExpansion);
            this.groupBox1.Controls.Add(this.comboBoxPlayset);
            this.groupBox1.Controls.Add(this.labelPlayset);
            this.groupBox1.Controls.Add(this.comboBoxAltered);
            this.groupBox1.Controls.Add(this.labelAltered);
            this.groupBox1.Controls.Add(this.comboBoxSigned);
            this.groupBox1.Controls.Add(this.labelSinged);
            this.groupBox1.Controls.Add(this.comboBoxFoil);
            this.groupBox1.Controls.Add(this.labelFoil);
            this.groupBox1.Location = new System.Drawing.Point(6, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 107);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Assume when not defined";
            // 
            // comboBoxCondition
            // 
            this.comboBoxCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCondition.FormattingEnabled = true;
            this.comboBoxCondition.Items.AddRange(new object[] {
            "MT",
            "NM",
            "EX",
            "GD",
            "LP",
            "PL",
            "PO"});
            this.comboBoxCondition.Location = new System.Drawing.Point(391, 19);
            this.comboBoxCondition.Name = "comboBoxCondition";
            this.comboBoxCondition.Size = new System.Drawing.Size(81, 21);
            this.comboBoxCondition.TabIndex = 13;
            // 
            // labelCondition
            // 
            this.labelCondition.AutoSize = true;
            this.labelCondition.Location = new System.Drawing.Point(327, 22);
            this.labelCondition.Name = "labelCondition";
            this.labelCondition.Size = new System.Drawing.Size(54, 13);
            this.labelCondition.TabIndex = 12;
            this.labelCondition.Text = "Condition:";
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Location = new System.Drawing.Point(324, 73);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(148, 21);
            this.comboBoxLanguage.TabIndex = 11;
            // 
            // labelLanguage
            // 
            this.labelLanguage.AutoSize = true;
            this.labelLanguage.Location = new System.Drawing.Point(250, 76);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(58, 13);
            this.labelLanguage.TabIndex = 10;
            this.labelLanguage.Text = "Language:";
            // 
            // comboBoxExpansion
            // 
            this.comboBoxExpansion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExpansion.FormattingEnabled = true;
            this.comboBoxExpansion.Items.AddRange(new object[] {
            "Latest",
            "Oldest",
            "Cheapest",
            "Median Price",
            "Most Expensive"});
            this.comboBoxExpansion.Location = new System.Drawing.Point(71, 73);
            this.comboBoxExpansion.Name = "comboBoxExpansion";
            this.comboBoxExpansion.Size = new System.Drawing.Size(148, 21);
            this.comboBoxExpansion.TabIndex = 9;
            // 
            // labelExpansion
            // 
            this.labelExpansion.AutoSize = true;
            this.labelExpansion.Location = new System.Drawing.Point(6, 76);
            this.labelExpansion.Name = "labelExpansion";
            this.labelExpansion.Size = new System.Drawing.Size(59, 13);
            this.labelExpansion.TabIndex = 8;
            this.labelExpansion.Text = "Expansion:";
            // 
            // comboBoxPlayset
            // 
            this.comboBoxPlayset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlayset.FormattingEnabled = true;
            this.comboBoxPlayset.Items.AddRange(new object[] {
            "Any",
            "Yes",
            "No"});
            this.comboBoxPlayset.Location = new System.Drawing.Point(225, 46);
            this.comboBoxPlayset.Name = "comboBoxPlayset";
            this.comboBoxPlayset.Size = new System.Drawing.Size(81, 21);
            this.comboBoxPlayset.TabIndex = 7;
            // 
            // labelPlayset
            // 
            this.labelPlayset.AutoSize = true;
            this.labelPlayset.Location = new System.Drawing.Point(161, 49);
            this.labelPlayset.Name = "labelPlayset";
            this.labelPlayset.Size = new System.Drawing.Size(44, 13);
            this.labelPlayset.TabIndex = 6;
            this.labelPlayset.Text = "Playset:";
            // 
            // comboBoxAltered
            // 
            this.comboBoxAltered.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAltered.FormattingEnabled = true;
            this.comboBoxAltered.Items.AddRange(new object[] {
            "Any",
            "Yes",
            "No"});
            this.comboBoxAltered.Location = new System.Drawing.Point(71, 46);
            this.comboBoxAltered.Name = "comboBoxAltered";
            this.comboBoxAltered.Size = new System.Drawing.Size(81, 21);
            this.comboBoxAltered.TabIndex = 5;
            // 
            // labelAltered
            // 
            this.labelAltered.AutoSize = true;
            this.labelAltered.Location = new System.Drawing.Point(7, 49);
            this.labelAltered.Name = "labelAltered";
            this.labelAltered.Size = new System.Drawing.Size(43, 13);
            this.labelAltered.TabIndex = 4;
            this.labelAltered.Text = "Altered:";
            // 
            // comboBoxSigned
            // 
            this.comboBoxSigned.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSigned.FormattingEnabled = true;
            this.comboBoxSigned.Items.AddRange(new object[] {
            "Any",
            "Yes",
            "No"});
            this.comboBoxSigned.Location = new System.Drawing.Point(225, 19);
            this.comboBoxSigned.Name = "comboBoxSigned";
            this.comboBoxSigned.Size = new System.Drawing.Size(81, 21);
            this.comboBoxSigned.TabIndex = 3;
            // 
            // labelSinged
            // 
            this.labelSinged.AutoSize = true;
            this.labelSinged.Location = new System.Drawing.Point(161, 22);
            this.labelSinged.Name = "labelSinged";
            this.labelSinged.Size = new System.Drawing.Size(43, 13);
            this.labelSinged.TabIndex = 2;
            this.labelSinged.Text = "Signed:";
            // 
            // comboBoxFoil
            // 
            this.comboBoxFoil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFoil.FormattingEnabled = true;
            this.comboBoxFoil.Items.AddRange(new object[] {
            "Any",
            "Yes",
            "No"});
            this.comboBoxFoil.Location = new System.Drawing.Point(71, 19);
            this.comboBoxFoil.Name = "comboBoxFoil";
            this.comboBoxFoil.Size = new System.Drawing.Size(81, 21);
            this.comboBoxFoil.TabIndex = 1;
            // 
            // labelFoil
            // 
            this.labelFoil.AutoSize = true;
            this.labelFoil.Location = new System.Drawing.Point(7, 22);
            this.labelFoil.Name = "labelFoil";
            this.labelFoil.Size = new System.Drawing.Size(26, 13);
            this.labelFoil.TabIndex = 0;
            this.labelFoil.Text = "Foil:";
            // 
            // groupBoxPrice
            // 
            this.groupBoxPrice.Controls.Add(this.buttonBotSettings);
            this.groupBoxPrice.Controls.Add(this.buttonAppraise);
            this.groupBoxPrice.Controls.Add(this.checkBoxToolPrices);
            this.groupBoxPrice.Controls.Add(this.checkBoxPriceGuide);
            this.groupBoxPrice.Location = new System.Drawing.Point(10, 253);
            this.groupBoxPrice.Name = "groupBoxPrice";
            this.groupBoxPrice.Size = new System.Drawing.Size(496, 130);
            this.groupBoxPrice.TabIndex = 1;
            this.groupBoxPrice.TabStop = false;
            this.groupBoxPrice.Text = "Appraise";
            // 
            // buttonBotSettings
            // 
            this.buttonBotSettings.Location = new System.Drawing.Point(393, 12);
            this.buttonBotSettings.Name = "buttonBotSettings";
            this.buttonBotSettings.Size = new System.Drawing.Size(91, 28);
            this.buttonBotSettings.TabIndex = 14;
            this.buttonBotSettings.Text = "Settings";
            this.buttonBotSettings.UseVisualStyleBackColor = true;
            this.buttonBotSettings.Click += new System.EventHandler(this.buttonBotSettings_Click);
            // 
            // buttonAppraise
            // 
            this.buttonAppraise.Location = new System.Drawing.Point(170, 56);
            this.buttonAppraise.Name = "buttonAppraise";
            this.buttonAppraise.Size = new System.Drawing.Size(142, 54);
            this.buttonAppraise.TabIndex = 13;
            this.buttonAppraise.Text = "Appraise";
            this.buttonAppraise.UseVisualStyleBackColor = true;
            this.buttonAppraise.Click += new System.EventHandler(this.buttonAppraise_Click);
            // 
            // checkBoxToolPrices
            // 
            this.checkBoxToolPrices.AutoSize = true;
            this.checkBoxToolPrices.Checked = true;
            this.checkBoxToolPrices.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxToolPrices.Location = new System.Drawing.Point(239, 19);
            this.checkBoxToolPrices.Name = "checkBoxToolPrices";
            this.checkBoxToolPrices.Size = new System.Drawing.Size(148, 17);
            this.checkBoxToolPrices.TabIndex = 1;
            this.checkBoxToolPrices.Text = "Compute MKMTool prices";
            this.checkBoxToolPrices.UseVisualStyleBackColor = true;
            this.checkBoxToolPrices.CheckedChanged += new System.EventHandler(this.checkBoxToolPrices_CheckedChanged);
            // 
            // checkBoxPriceGuide
            // 
            this.checkBoxPriceGuide.AutoSize = true;
            this.checkBoxPriceGuide.Location = new System.Drawing.Point(6, 19);
            this.checkBoxPriceGuide.Name = "checkBoxPriceGuide";
            this.checkBoxPriceGuide.Size = new System.Drawing.Size(167, 17);
            this.checkBoxPriceGuide.TabIndex = 0;
            this.checkBoxPriceGuide.Text = "Fetch MKM price guide prices";
            this.checkBoxPriceGuide.UseVisualStyleBackColor = true;
            // 
            // groupBoxExport
            // 
            this.groupBoxExport.Controls.Add(this.groupBoxExportMKM);
            this.groupBoxExport.Controls.Add(this.groupBoxExportFile);
            this.groupBoxExport.Location = new System.Drawing.Point(10, 389);
            this.groupBoxExport.Name = "groupBoxExport";
            this.groupBoxExport.Size = new System.Drawing.Size(496, 225);
            this.groupBoxExport.TabIndex = 1;
            this.groupBoxExport.TabStop = false;
            this.groupBoxExport.Text = "Export";
            // 
            // groupBoxExportMKM
            // 
            this.groupBoxExportMKM.Controls.Add(this.checkBoxExportLogAll);
            this.groupBoxExportMKM.Controls.Add(this.buttonExportToMKM);
            this.groupBoxExportMKM.Controls.Add(this.comboBoxExportUploadPrice);
            this.groupBoxExportMKM.Controls.Add(this.labelExportWhichPrice);
            this.groupBoxExportMKM.Location = new System.Drawing.Point(248, 19);
            this.groupBoxExportMKM.Name = "groupBoxExportMKM";
            this.groupBoxExportMKM.Size = new System.Drawing.Size(242, 195);
            this.groupBoxExportMKM.TabIndex = 15;
            this.groupBoxExportMKM.TabStop = false;
            this.groupBoxExportMKM.Text = "To MKM";
            // 
            // checkBoxExportLogAll
            // 
            this.checkBoxExportLogAll.AutoSize = true;
            this.checkBoxExportLogAll.Location = new System.Drawing.Point(11, 65);
            this.checkBoxExportLogAll.Name = "checkBoxExportLogAll";
            this.checkBoxExportLogAll.Size = new System.Drawing.Size(140, 17);
            this.checkBoxExportLogAll.TabIndex = 14;
            this.checkBoxExportLogAll.Text = "Log all uploaded articles";
            this.toolTip1.SetToolTip(this.checkBoxExportLogAll, "If turned off, only articles for which upload fails will be logged.");
            this.checkBoxExportLogAll.UseVisualStyleBackColor = true;
            // 
            // buttonExportToMKM
            // 
            this.buttonExportToMKM.Location = new System.Drawing.Point(55, 135);
            this.buttonExportToMKM.Name = "buttonExportToMKM";
            this.buttonExportToMKM.Size = new System.Drawing.Size(142, 54);
            this.buttonExportToMKM.TabIndex = 22;
            this.buttonExportToMKM.Text = "Upload to MKM";
            this.buttonExportToMKM.UseVisualStyleBackColor = true;
            this.buttonExportToMKM.Click += new System.EventHandler(this.buttonExportToMKM_Click);
            // 
            // comboBoxExportUploadPrice
            // 
            this.comboBoxExportUploadPrice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExportUploadPrice.FormattingEnabled = true;
            this.comboBoxExportUploadPrice.Items.AddRange(new object[] {
            "MKMTool price",
            "Cheapest matching article",
            "Price guide - Low",
            "Price guide - Average",
            "Price guide - Trend"});
            this.comboBoxExportUploadPrice.Location = new System.Drawing.Point(77, 17);
            this.comboBoxExportUploadPrice.Name = "comboBoxExportUploadPrice";
            this.comboBoxExportUploadPrice.Size = new System.Drawing.Size(159, 21);
            this.comboBoxExportUploadPrice.TabIndex = 13;
            // 
            // labelExportWhichPrice
            // 
            this.labelExportWhichPrice.AutoSize = true;
            this.labelExportWhichPrice.Location = new System.Drawing.Point(7, 21);
            this.labelExportWhichPrice.Name = "labelExportWhichPrice";
            this.labelExportWhichPrice.Size = new System.Drawing.Size(70, 13);
            this.labelExportWhichPrice.TabIndex = 12;
            this.labelExportWhichPrice.Text = "Upload price:";
            // 
            // groupBoxExportFile
            // 
            this.groupBoxExportFile.Controls.Add(this.checkBoxExportOnlyAppraised);
            this.groupBoxExportFile.Controls.Add(this.checkBoxExportAll);
            this.groupBoxExportFile.Controls.Add(this.buttonExport);
            this.groupBoxExportFile.Controls.Add(this.checkBoxExportToolPrices);
            this.groupBoxExportFile.Controls.Add(this.checkBoxExportFormatDeckbox);
            this.groupBoxExportFile.Controls.Add(this.checkBoxExportPriceGuide);
            this.groupBoxExportFile.Location = new System.Drawing.Point(9, 19);
            this.groupBoxExportFile.Name = "groupBoxExportFile";
            this.groupBoxExportFile.Size = new System.Drawing.Size(233, 195);
            this.groupBoxExportFile.TabIndex = 23;
            this.groupBoxExportFile.TabStop = false;
            this.groupBoxExportFile.Text = "To file";
            // 
            // checkBoxExportAll
            // 
            this.checkBoxExportAll.AutoSize = true;
            this.checkBoxExportAll.Location = new System.Drawing.Point(6, 19);
            this.checkBoxExportAll.Name = "checkBoxExportAll";
            this.checkBoxExportAll.Size = new System.Drawing.Size(129, 17);
            this.checkBoxExportAll.TabIndex = 18;
            this.checkBoxExportAll.Text = "Include all known info";
            this.checkBoxExportAll.UseVisualStyleBackColor = true;
            this.checkBoxExportAll.CheckedChanged += new System.EventHandler(this.checkBoxExportAll_CheckedChanged);
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(33, 135);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(142, 54);
            this.buttonExport.TabIndex = 13;
            this.buttonExport.Text = "Export CSV file...";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // checkBoxExportToolPrices
            // 
            this.checkBoxExportToolPrices.AutoSize = true;
            this.checkBoxExportToolPrices.Checked = true;
            this.checkBoxExportToolPrices.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExportToolPrices.Location = new System.Drawing.Point(6, 65);
            this.checkBoxExportToolPrices.Name = "checkBoxExportToolPrices";
            this.checkBoxExportToolPrices.Size = new System.Drawing.Size(103, 17);
            this.checkBoxExportToolPrices.TabIndex = 20;
            this.checkBoxExportToolPrices.Text = "MKMTool prices";
            this.checkBoxExportToolPrices.UseVisualStyleBackColor = true;
            // 
            // checkBoxExportFormatDeckbox
            // 
            this.checkBoxExportFormatDeckbox.AutoSize = true;
            this.checkBoxExportFormatDeckbox.Location = new System.Drawing.Point(6, 88);
            this.checkBoxExportFormatDeckbox.Name = "checkBoxExportFormatDeckbox";
            this.checkBoxExportFormatDeckbox.Size = new System.Drawing.Size(147, 17);
            this.checkBoxExportFormatDeckbox.TabIndex = 21;
            this.checkBoxExportFormatDeckbox.Text = "Force deckbox.org format";
            this.toolTip1.SetToolTip(this.checkBoxExportFormatDeckbox, "Affects format of condition, foil, signed and altered columns");
            this.checkBoxExportFormatDeckbox.UseVisualStyleBackColor = true;
            // 
            // checkBoxExportPriceGuide
            // 
            this.checkBoxExportPriceGuide.AutoSize = true;
            this.checkBoxExportPriceGuide.Location = new System.Drawing.Point(6, 42);
            this.checkBoxExportPriceGuide.Name = "checkBoxExportPriceGuide";
            this.checkBoxExportPriceGuide.Size = new System.Drawing.Size(137, 17);
            this.checkBoxExportPriceGuide.TabIndex = 19;
            this.checkBoxExportPriceGuide.Text = "MKM price guide prices";
            this.checkBoxExportPriceGuide.UseVisualStyleBackColor = true;
            // 
            // checkBoxExportOnlyAppraised
            // 
            this.checkBoxExportOnlyAppraised.AutoSize = true;
            this.checkBoxExportOnlyAppraised.Location = new System.Drawing.Point(6, 112);
            this.checkBoxExportOnlyAppraised.Name = "checkBoxExportOnlyAppraised";
            this.checkBoxExportOnlyAppraised.Size = new System.Drawing.Size(96, 17);
            this.checkBoxExportOnlyAppraised.TabIndex = 22;
            this.checkBoxExportOnlyAppraised.Text = "Only appraised";
            this.toolTip1.SetToolTip(this.checkBoxExportOnlyAppraised, "If checked, only items that had a price generated for them  (at least one of the " +
        "selected prices) will be exported");
            this.checkBoxExportOnlyAppraised.UseVisualStyleBackColor = true;
            // 
            // PriceExternalList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 620);
            this.Controls.Add(this.groupBoxPrice);
            this.Controls.Add(this.groupBoxExport);
            this.Controls.Add(this.groupBoxImport);
            this.Name = "PriceExternalList";
            this.Text = "PriceExternalList";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PriceExternalList_FormClosing);
            this.groupBoxImport.ResumeLayout(false);
            this.groupBoxImport.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxPrice.ResumeLayout(false);
            this.groupBoxPrice.PerformLayout();
            this.groupBoxExport.ResumeLayout(false);
            this.groupBoxExportMKM.ResumeLayout(false);
            this.groupBoxExportMKM.PerformLayout();
            this.groupBoxExportFile.ResumeLayout(false);
            this.groupBoxExportFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxImport;
        private System.Windows.Forms.GroupBox groupBoxPrice;
        private System.Windows.Forms.GroupBox groupBoxExport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxSigned;
        private System.Windows.Forms.Label labelSinged;
        private System.Windows.Forms.ComboBox comboBoxFoil;
        private System.Windows.Forms.Label labelFoil;
        private System.Windows.Forms.ComboBox comboBoxExpansion;
        private System.Windows.Forms.Label labelExpansion;
        private System.Windows.Forms.ComboBox comboBoxPlayset;
        private System.Windows.Forms.Label labelPlayset;
        private System.Windows.Forms.ComboBox comboBoxAltered;
        private System.Windows.Forms.Label labelAltered;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.Button buttonBotSettings;
        private System.Windows.Forms.Button buttonAppraise;
        private System.Windows.Forms.CheckBox checkBoxToolPrices;
        private System.Windows.Forms.CheckBox checkBoxPriceGuide;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.CheckBox checkBoxExportFormatDeckbox;
        private System.Windows.Forms.CheckBox checkBoxExportPriceGuide;
        private System.Windows.Forms.CheckBox checkBoxExportToolPrices;
        private System.Windows.Forms.CheckBox checkBoxExportAll;
        private System.Windows.Forms.ComboBox comboBoxExportUploadPrice;
        private System.Windows.Forms.Label labelExportWhichPrice;
        private System.Windows.Forms.Button buttonExportToMKM;
        private System.Windows.Forms.CheckBox checkBoxImportLog;
        private System.Windows.Forms.ComboBox comboBoxCondition;
        private System.Windows.Forms.Label labelCondition;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBoxExportMKM;
        private System.Windows.Forms.GroupBox groupBoxExportFile;
        private System.Windows.Forms.CheckBox checkBoxExportLogAll;
        private System.Windows.Forms.CheckBox checkBoxExportOnlyAppraised;
    }
}