namespace MKMTool
{
    partial class CheckWantsView
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
            this.wantListsBox2 = new System.Windows.Forms.ComboBox();
            this.checkListButton = new System.Windows.Forms.Button();
            this.percentText = new System.Windows.Forms.TextBox();
            this.labelPercent = new System.Windows.Forms.Label();
            this.shipAddition = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.editionBox = new System.Windows.Forms.ComboBox();
            this.checkEditionButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBestInter = new System.Windows.Forms.CheckBox();
            this.domnesticCheck = new System.Windows.Forms.CheckBox();
            this.maxPrice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkTrend = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.checkBoxUser = new System.Windows.Forms.CheckBox();
            this.signedBox = new System.Windows.Forms.CheckBox();
            this.foilBox = new System.Windows.Forms.CheckBox();
            this.alteredBox = new System.Windows.Forms.CheckBox();
            this.conditionBox = new System.Windows.Forms.Label();
            this.conditionCombo = new System.Windows.Forms.ComboBox();
            this.playsetBox = new System.Windows.Forms.CheckBox();
            this.langCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // wantListsBox2
            // 
            this.wantListsBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wantListsBox2.FormattingEnabled = true;
            this.wantListsBox2.Location = new System.Drawing.Point(6, 147);
            this.wantListsBox2.Name = "wantListsBox2";
            this.wantListsBox2.Size = new System.Drawing.Size(188, 21);
            this.wantListsBox2.TabIndex = 0;
            this.wantListsBox2.SelectedIndexChanged += new System.EventHandler(this.wantListsBox2_SelectedIndexChanged);
            // 
            // checkListButton
            // 
            this.checkListButton.Location = new System.Drawing.Point(6, 174);
            this.checkListButton.Name = "checkListButton";
            this.checkListButton.Size = new System.Drawing.Size(188, 51);
            this.checkListButton.TabIndex = 6;
            this.checkListButton.Text = "Check selected List";
            this.checkListButton.UseVisualStyleBackColor = true;
            this.checkListButton.Click += new System.EventHandler(this.checkListButton_Click);
            // 
            // percentText
            // 
            this.percentText.Location = new System.Drawing.Point(6, 19);
            this.percentText.Name = "percentText";
            this.percentText.Size = new System.Drawing.Size(40, 20);
            this.percentText.TabIndex = 7;
            this.percentText.Text = "20";
            // 
            // labelPercent
            // 
            this.labelPercent.AutoSize = true;
            this.labelPercent.Location = new System.Drawing.Point(52, 22);
            this.labelPercent.Name = "labelPercent";
            this.labelPercent.Size = new System.Drawing.Size(78, 13);
            this.labelPercent.TabIndex = 8;
            this.labelPercent.Text = "% below others";
            // 
            // shipAddition
            // 
            this.shipAddition.Location = new System.Drawing.Point(6, 45);
            this.shipAddition.Name = "shipAddition";
            this.shipAddition.Size = new System.Drawing.Size(40, 20);
            this.shipAddition.TabIndex = 10;
            this.shipAddition.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Shipping Addition";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 174);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(190, 51);
            this.button1.TabIndex = 13;
            this.button1.Text = "Empty Cart";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // editionBox
            // 
            this.editionBox.FormattingEnabled = true;
            this.editionBox.Location = new System.Drawing.Point(6, 151);
            this.editionBox.Name = "editionBox";
            this.editionBox.Size = new System.Drawing.Size(188, 21);
            this.editionBox.TabIndex = 27;
            // 
            // checkEditionButton
            // 
            this.checkEditionButton.Location = new System.Drawing.Point(6, 174);
            this.checkEditionButton.Name = "checkEditionButton";
            this.checkEditionButton.Size = new System.Drawing.Size(188, 51);
            this.checkEditionButton.TabIndex = 28;
            this.checkEditionButton.Text = "Check now";
            this.checkEditionButton.UseVisualStyleBackColor = true;
            this.checkEditionButton.Click += new System.EventHandler(this.checkEditionButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBestInter);
            this.groupBox1.Controls.Add(this.domnesticCheck);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.maxPrice);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkTrend);
            this.groupBox1.Controls.Add(this.percentText);
            this.groupBox1.Controls.Add(this.labelPercent);
            this.groupBox1.Controls.Add(this.shipAddition);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(11, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 231);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Price Parameter";
            // 
            // checkBestInter
            // 
            this.checkBestInter.AutoSize = true;
            this.checkBestInter.Checked = true;
            this.checkBestInter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBestInter.Location = new System.Drawing.Point(6, 136);
            this.checkBestInter.Name = "checkBestInter";
            this.checkBestInter.Size = new System.Drawing.Size(165, 17);
            this.checkBestInter.TabIndex = 16;
            this.checkBestInter.Text = "check best price international";
            this.checkBestInter.UseVisualStyleBackColor = true;
            // 
            // domnesticCheck
            // 
            this.domnesticCheck.AutoSize = true;
            this.domnesticCheck.Checked = true;
            this.domnesticCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.domnesticCheck.Location = new System.Drawing.Point(6, 115);
            this.domnesticCheck.Name = "domnesticCheck";
            this.domnesticCheck.Size = new System.Drawing.Size(151, 17);
            this.domnesticCheck.TabIndex = 15;
            this.domnesticCheck.Text = "check domestic deals only";
            this.domnesticCheck.UseVisualStyleBackColor = true;
            // 
            // maxPrice
            // 
            this.maxPrice.Location = new System.Drawing.Point(6, 71);
            this.maxPrice.Name = "maxPrice";
            this.maxPrice.Size = new System.Drawing.Size(40, 20);
            this.maxPrice.TabIndex = 13;
            this.maxPrice.Text = "10";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Max Price per Card";
            // 
            // checkTrend
            // 
            this.checkTrend.AutoSize = true;
            this.checkTrend.Checked = true;
            this.checkTrend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkTrend.Location = new System.Drawing.Point(6, 94);
            this.checkTrend.Name = "checkTrend";
            this.checkTrend.Size = new System.Drawing.Size(87, 17);
            this.checkTrend.TabIndex = 12;
            this.checkTrend.Text = "check Trend";
            this.checkTrend.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxUser);
            this.groupBox3.Controls.Add(this.checkBoxUser);
            this.groupBox3.Controls.Add(this.signedBox);
            this.groupBox3.Controls.Add(this.editionBox);
            this.groupBox3.Controls.Add(this.foilBox);
            this.groupBox3.Controls.Add(this.checkEditionButton);
            this.groupBox3.Controls.Add(this.alteredBox);
            this.groupBox3.Controls.Add(this.conditionBox);
            this.groupBox3.Controls.Add(this.conditionCombo);
            this.groupBox3.Controls.Add(this.playsetBox);
            this.groupBox3.Controls.Add(this.langCombo);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(219, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 231);
            this.groupBox3.TabIndex = 31;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bulk Check";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Enabled = false;
            this.textBoxUser.Location = new System.Drawing.Point(103, 127);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(91, 20);
            this.textBoxUser.TabIndex = 42;
            // 
            // checkBoxUser
            // 
            this.checkBoxUser.AutoSize = true;
            this.checkBoxUser.Location = new System.Drawing.Point(6, 127);
            this.checkBoxUser.Name = "checkBoxUser";
            this.checkBoxUser.Size = new System.Drawing.Size(48, 17);
            this.checkBoxUser.TabIndex = 41;
            this.checkBoxUser.Text = "User";
            this.checkBoxUser.UseVisualStyleBackColor = true;
            this.checkBoxUser.CheckedChanged += new System.EventHandler(this.checkBoxUser_CheckedChanged);
            // 
            // signedBox
            // 
            this.signedBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.signedBox.AutoSize = true;
            this.signedBox.Location = new System.Drawing.Point(6, 107);
            this.signedBox.Margin = new System.Windows.Forms.Padding(2);
            this.signedBox.Name = "signedBox";
            this.signedBox.Size = new System.Drawing.Size(59, 17);
            this.signedBox.TabIndex = 40;
            this.signedBox.Text = "Signed";
            this.signedBox.UseVisualStyleBackColor = true;
            // 
            // foilBox
            // 
            this.foilBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.foilBox.AutoSize = true;
            this.foilBox.Location = new System.Drawing.Point(103, 86);
            this.foilBox.Margin = new System.Windows.Forms.Padding(2);
            this.foilBox.Name = "foilBox";
            this.foilBox.Size = new System.Drawing.Size(42, 17);
            this.foilBox.TabIndex = 37;
            this.foilBox.Text = "Foil";
            this.foilBox.UseVisualStyleBackColor = true;
            // 
            // alteredBox
            // 
            this.alteredBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.alteredBox.AutoSize = true;
            this.alteredBox.Location = new System.Drawing.Point(103, 107);
            this.alteredBox.Margin = new System.Windows.Forms.Padding(2);
            this.alteredBox.Name = "alteredBox";
            this.alteredBox.Size = new System.Drawing.Size(59, 17);
            this.alteredBox.TabIndex = 39;
            this.alteredBox.Text = "Altered";
            this.alteredBox.UseVisualStyleBackColor = true;
            // 
            // conditionBox
            // 
            this.conditionBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.conditionBox.AutoSize = true;
            this.conditionBox.Location = new System.Drawing.Point(100, 48);
            this.conditionBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.conditionBox.Name = "conditionBox";
            this.conditionBox.Size = new System.Drawing.Size(95, 13);
            this.conditionBox.TabIndex = 34;
            this.conditionBox.Text = "Minimum Condition";
            // 
            // conditionCombo
            // 
            this.conditionCombo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.conditionCombo.FormattingEnabled = true;
            this.conditionCombo.Items.AddRange(new object[] {
            "MT",
            "NM",
            "EX",
            "GD",
            "LP",
            "PL",
            "PO"});
            this.conditionCombo.Location = new System.Drawing.Point(6, 45);
            this.conditionCombo.Margin = new System.Windows.Forms.Padding(2);
            this.conditionCombo.Name = "conditionCombo";
            this.conditionCombo.Size = new System.Drawing.Size(90, 21);
            this.conditionCombo.TabIndex = 33;
            // 
            // playsetBox
            // 
            this.playsetBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.playsetBox.AutoSize = true;
            this.playsetBox.Location = new System.Drawing.Point(6, 86);
            this.playsetBox.Margin = new System.Windows.Forms.Padding(2);
            this.playsetBox.Name = "playsetBox";
            this.playsetBox.Size = new System.Drawing.Size(60, 17);
            this.playsetBox.TabIndex = 38;
            this.playsetBox.Text = "Playset";
            this.playsetBox.UseVisualStyleBackColor = true;
            // 
            // langCombo
            // 
            this.langCombo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.langCombo.FormattingEnabled = true;
            this.langCombo.Location = new System.Drawing.Point(6, 18);
            this.langCombo.Margin = new System.Windows.Forms.Padding(2);
            this.langCombo.Name = "langCombo";
            this.langCombo.Size = new System.Drawing.Size(90, 21);
            this.langCombo.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Language";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkListButton);
            this.groupBox4.Controls.Add(this.wantListsBox2);
            this.groupBox4.Location = new System.Drawing.Point(425, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 231);
            this.groupBox4.TabIndex = 32;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Check Wantslist";
            // 
            // CheckWantsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 254);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.MinimizeBox = false;
            this.Name = "CheckWantsView";
            this.ShowIcon = false;
            this.Text = "Check for cheap deals";
            this.Load += new System.EventHandler(this.CheckWants_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox wantListsBox2;
        private System.Windows.Forms.Button checkListButton;
        private System.Windows.Forms.TextBox percentText;
        private System.Windows.Forms.Label labelPercent;
        private System.Windows.Forms.TextBox shipAddition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox editionBox;
        private System.Windows.Forms.Button checkEditionButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox signedBox;
        private System.Windows.Forms.CheckBox alteredBox;
        private System.Windows.Forms.CheckBox playsetBox;
        private System.Windows.Forms.CheckBox foilBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox langCombo;
        private System.Windows.Forms.Label conditionBox;
        private System.Windows.Forms.ComboBox conditionCombo;
        private System.Windows.Forms.CheckBox checkTrend;
        private System.Windows.Forms.TextBox maxPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox domnesticCheck;
        private System.Windows.Forms.CheckBox checkBestInter;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.CheckBox checkBoxUser;
    }
}