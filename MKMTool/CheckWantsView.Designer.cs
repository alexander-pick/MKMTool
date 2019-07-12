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
            this.components = new System.ComponentModel.Container();
            this.wantListsBox2 = new System.Windows.Forms.ComboBox();
            this.checkListButton = new System.Windows.Forms.Button();
            this.percentText = new System.Windows.Forms.TextBox();
            this.labelPercent = new System.Windows.Forms.Label();
            this.shipAddition = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonEmptyCart = new System.Windows.Forms.Button();
            this.editionBox = new System.Windows.Forms.ComboBox();
            this.checkEditionButton = new System.Windows.Forms.Button();
            this.groupBoxPriceParam = new System.Windows.Forms.GroupBox();
            this.checkBestInter = new System.Windows.Forms.CheckBox();
            this.domesticCheck = new System.Windows.Forms.CheckBox();
            this.maxPrice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxTrend = new System.Windows.Forms.CheckBox();
            this.groupBoxCardParam = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.signedBox = new System.Windows.Forms.CheckBox();
            this.foilBox = new System.Windows.Forms.CheckBox();
            this.alteredBox = new System.Windows.Forms.CheckBox();
            this.conditionBox = new System.Windows.Forms.Label();
            this.conditionCombo = new System.Windows.Forms.ComboBox();
            this.playsetBox = new System.Windows.Forms.CheckBox();
            this.langCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUser = new System.Windows.Forms.TextBox();
            this.groupBoxWantlist = new System.Windows.Forms.GroupBox();
            this.groupBoxBulkCheck = new System.Windows.Forms.GroupBox();
            this.groupBoxUserCheck = new System.Windows.Forms.GroupBox();
            this.checkBoxUserExpansions = new System.Windows.Forms.CheckBox();
            this.labelUserName = new System.Windows.Forms.Label();
            this.buttonCheckUser = new System.Windows.Forms.Button();
            this.groupBoxParams = new System.Windows.Forms.GroupBox();
            this.groupBoxPerform = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxPriceParam.SuspendLayout();
            this.groupBoxCardParam.SuspendLayout();
            this.groupBoxWantlist.SuspendLayout();
            this.groupBoxBulkCheck.SuspendLayout();
            this.groupBoxUserCheck.SuspendLayout();
            this.groupBoxParams.SuspendLayout();
            this.groupBoxPerform.SuspendLayout();
            this.SuspendLayout();
            // 
            // wantListsBox2
            // 
            this.wantListsBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wantListsBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wantListsBox2.FormattingEnabled = true;
            this.wantListsBox2.Location = new System.Drawing.Point(20, 19);
            this.wantListsBox2.Name = "wantListsBox2";
            this.wantListsBox2.Size = new System.Drawing.Size(188, 21);
            this.wantListsBox2.TabIndex = 0;
            this.wantListsBox2.SelectedIndexChanged += new System.EventHandler(this.wantListsBox2_SelectedIndexChanged);
            // 
            // checkListButton
            // 
            this.checkListButton.Location = new System.Drawing.Point(20, 46);
            this.checkListButton.Name = "checkListButton";
            this.checkListButton.Size = new System.Drawing.Size(188, 51);
            this.checkListButton.TabIndex = 6;
            this.checkListButton.Text = "Check selected list";
            this.toolTip1.SetToolTip(this.checkListButton, "All cards in the wantlist will be checked - Card parameters will be ignored");
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
            // buttonEmptyCart
            // 
            this.buttonEmptyCart.Location = new System.Drawing.Point(53, 355);
            this.buttonEmptyCart.Name = "buttonEmptyCart";
            this.buttonEmptyCart.Size = new System.Drawing.Size(132, 35);
            this.buttonEmptyCart.TabIndex = 13;
            this.buttonEmptyCart.Text = "Empty cart";
            this.buttonEmptyCart.UseVisualStyleBackColor = true;
            this.buttonEmptyCart.Click += new System.EventHandler(this.button1_Click);
            // 
            // editionBox
            // 
            this.editionBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.editionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.editionBox.FormattingEnabled = true;
            this.editionBox.Location = new System.Drawing.Point(6, 154);
            this.editionBox.Name = "editionBox";
            this.editionBox.Size = new System.Drawing.Size(188, 21);
            this.editionBox.TabIndex = 27;
            // 
            // checkEditionButton
            // 
            this.checkEditionButton.Location = new System.Drawing.Point(21, 17);
            this.checkEditionButton.Name = "checkEditionButton";
            this.checkEditionButton.Size = new System.Drawing.Size(188, 51);
            this.checkEditionButton.TabIndex = 28;
            this.checkEditionButton.Text = "Check now";
            this.checkEditionButton.UseVisualStyleBackColor = true;
            this.checkEditionButton.Click += new System.EventHandler(this.checkEditionButton_Click);
            // 
            // groupBoxPriceParam
            // 
            this.groupBoxPriceParam.Controls.Add(this.checkBestInter);
            this.groupBoxPriceParam.Controls.Add(this.domesticCheck);
            this.groupBoxPriceParam.Controls.Add(this.maxPrice);
            this.groupBoxPriceParam.Controls.Add(this.label3);
            this.groupBoxPriceParam.Controls.Add(this.checkBoxTrend);
            this.groupBoxPriceParam.Controls.Add(this.percentText);
            this.groupBoxPriceParam.Controls.Add(this.labelPercent);
            this.groupBoxPriceParam.Controls.Add(this.shipAddition);
            this.groupBoxPriceParam.Controls.Add(this.label2);
            this.groupBoxPriceParam.Location = new System.Drawing.Point(6, 16);
            this.groupBoxPriceParam.Name = "groupBoxPriceParam";
            this.groupBoxPriceParam.Size = new System.Drawing.Size(220, 179);
            this.groupBoxPriceParam.TabIndex = 29;
            this.groupBoxPriceParam.TabStop = false;
            this.groupBoxPriceParam.Text = "Price parameters";
            // 
            // checkBestInter
            // 
            this.checkBestInter.AutoSize = true;
            this.checkBestInter.Checked = true;
            this.checkBestInter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBestInter.Location = new System.Drawing.Point(6, 144);
            this.checkBestInter.Name = "checkBestInter";
            this.checkBestInter.Size = new System.Drawing.Size(165, 17);
            this.checkBestInter.TabIndex = 16;
            this.checkBestInter.Text = "check best price international";
            this.checkBestInter.UseVisualStyleBackColor = true;
            // 
            // domesticCheck
            // 
            this.domesticCheck.AutoSize = true;
            this.domesticCheck.Checked = true;
            this.domesticCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.domesticCheck.Location = new System.Drawing.Point(6, 122);
            this.domesticCheck.Name = "domesticCheck";
            this.domesticCheck.Size = new System.Drawing.Size(151, 17);
            this.domesticCheck.TabIndex = 15;
            this.domesticCheck.Text = "check domestic deals only";
            this.domesticCheck.UseVisualStyleBackColor = true;
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
            // checkBoxTrend
            // 
            this.checkBoxTrend.AutoSize = true;
            this.checkBoxTrend.Checked = true;
            this.checkBoxTrend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTrend.Location = new System.Drawing.Point(6, 99);
            this.checkBoxTrend.Name = "checkBoxTrend";
            this.checkBoxTrend.Size = new System.Drawing.Size(87, 17);
            this.checkBoxTrend.TabIndex = 12;
            this.checkBoxTrend.Text = "check Trend";
            this.checkBoxTrend.UseVisualStyleBackColor = true;
            // 
            // groupBoxCardParam
            // 
            this.groupBoxCardParam.Controls.Add(this.label4);
            this.groupBoxCardParam.Controls.Add(this.signedBox);
            this.groupBoxCardParam.Controls.Add(this.editionBox);
            this.groupBoxCardParam.Controls.Add(this.foilBox);
            this.groupBoxCardParam.Controls.Add(this.alteredBox);
            this.groupBoxCardParam.Controls.Add(this.conditionBox);
            this.groupBoxCardParam.Controls.Add(this.conditionCombo);
            this.groupBoxCardParam.Controls.Add(this.playsetBox);
            this.groupBoxCardParam.Controls.Add(this.langCombo);
            this.groupBoxCardParam.Controls.Add(this.label1);
            this.groupBoxCardParam.Location = new System.Drawing.Point(6, 201);
            this.groupBoxCardParam.Name = "groupBoxCardParam";
            this.groupBoxCardParam.Size = new System.Drawing.Size(220, 189);
            this.groupBoxCardParam.TabIndex = 31;
            this.groupBoxCardParam.TabStop = false;
            this.groupBoxCardParam.Text = "Card parameters";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Expansion:";
            // 
            // signedBox
            // 
            this.signedBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.signedBox.AutoSize = true;
            this.signedBox.Location = new System.Drawing.Point(10, 107);
            this.signedBox.Margin = new System.Windows.Forms.Padding(2);
            this.signedBox.Name = "signedBox";
            this.signedBox.Size = new System.Drawing.Size(59, 17);
            this.signedBox.TabIndex = 40;
            this.signedBox.Text = "Signed";
            this.signedBox.UseVisualStyleBackColor = true;
            // 
            // foilBox
            // 
            this.foilBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.foilBox.AutoSize = true;
            this.foilBox.Location = new System.Drawing.Point(103, 85);
            this.foilBox.Margin = new System.Windows.Forms.Padding(2);
            this.foilBox.Name = "foilBox";
            this.foilBox.Size = new System.Drawing.Size(42, 17);
            this.foilBox.TabIndex = 37;
            this.foilBox.Text = "Foil";
            this.foilBox.UseVisualStyleBackColor = true;
            // 
            // alteredBox
            // 
            this.alteredBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.alteredBox.AutoSize = true;
            this.alteredBox.Location = new System.Drawing.Point(103, 106);
            this.alteredBox.Margin = new System.Windows.Forms.Padding(2);
            this.alteredBox.Name = "alteredBox";
            this.alteredBox.Size = new System.Drawing.Size(59, 17);
            this.alteredBox.TabIndex = 39;
            this.alteredBox.Text = "Altered";
            this.alteredBox.UseVisualStyleBackColor = true;
            // 
            // conditionBox
            // 
            this.conditionBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.conditionBox.AutoSize = true;
            this.conditionBox.Location = new System.Drawing.Point(100, 58);
            this.conditionBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.conditionBox.Name = "conditionBox";
            this.conditionBox.Size = new System.Drawing.Size(95, 13);
            this.conditionBox.TabIndex = 34;
            this.conditionBox.Text = "Minimum Condition";
            // 
            // conditionCombo
            // 
            this.conditionCombo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.conditionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.conditionCombo.FormattingEnabled = true;
            this.conditionCombo.Items.AddRange(new object[] {
            "MT",
            "NM",
            "EX",
            "GD",
            "LP",
            "PL",
            "PO"});
            this.conditionCombo.Location = new System.Drawing.Point(6, 56);
            this.conditionCombo.Margin = new System.Windows.Forms.Padding(2);
            this.conditionCombo.Name = "conditionCombo";
            this.conditionCombo.Size = new System.Drawing.Size(90, 21);
            this.conditionCombo.TabIndex = 33;
            // 
            // playsetBox
            // 
            this.playsetBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.playsetBox.AutoSize = true;
            this.playsetBox.Location = new System.Drawing.Point(10, 86);
            this.playsetBox.Margin = new System.Windows.Forms.Padding(2);
            this.playsetBox.Name = "playsetBox";
            this.playsetBox.Size = new System.Drawing.Size(60, 17);
            this.playsetBox.TabIndex = 38;
            this.playsetBox.Text = "Playset";
            this.playsetBox.UseVisualStyleBackColor = true;
            // 
            // langCombo
            // 
            this.langCombo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.langCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.langCombo.FormattingEnabled = true;
            this.langCombo.Location = new System.Drawing.Point(6, 29);
            this.langCombo.Margin = new System.Windows.Forms.Padding(2);
            this.langCombo.Name = "langCombo";
            this.langCombo.Size = new System.Drawing.Size(90, 21);
            this.langCombo.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 36;
            this.label1.Text = "Language";
            // 
            // textBoxUser
            // 
            this.textBoxUser.Location = new System.Drawing.Point(97, 19);
            this.textBoxUser.Name = "textBoxUser";
            this.textBoxUser.Size = new System.Drawing.Size(111, 20);
            this.textBoxUser.TabIndex = 42;
            // 
            // groupBoxWantlist
            // 
            this.groupBoxWantlist.Controls.Add(this.checkListButton);
            this.groupBoxWantlist.Controls.Add(this.wantListsBox2);
            this.groupBoxWantlist.Location = new System.Drawing.Point(7, 235);
            this.groupBoxWantlist.Name = "groupBoxWantlist";
            this.groupBoxWantlist.Size = new System.Drawing.Size(227, 110);
            this.groupBoxWantlist.TabIndex = 32;
            this.groupBoxWantlist.TabStop = false;
            this.groupBoxWantlist.Text = "Check Wantslist";
            // 
            // groupBoxBulkCheck
            // 
            this.groupBoxBulkCheck.Controls.Add(this.checkEditionButton);
            this.groupBoxBulkCheck.Location = new System.Drawing.Point(6, 19);
            this.groupBoxBulkCheck.Name = "groupBoxBulkCheck";
            this.groupBoxBulkCheck.Size = new System.Drawing.Size(228, 74);
            this.groupBoxBulkCheck.TabIndex = 33;
            this.groupBoxBulkCheck.TabStop = false;
            this.groupBoxBulkCheck.Text = "Whole expansion check";
            // 
            // groupBoxUserCheck
            // 
            this.groupBoxUserCheck.Controls.Add(this.checkBoxUserExpansions);
            this.groupBoxUserCheck.Controls.Add(this.labelUserName);
            this.groupBoxUserCheck.Controls.Add(this.textBoxUser);
            this.groupBoxUserCheck.Controls.Add(this.buttonCheckUser);
            this.groupBoxUserCheck.Location = new System.Drawing.Point(7, 99);
            this.groupBoxUserCheck.Name = "groupBoxUserCheck";
            this.groupBoxUserCheck.Size = new System.Drawing.Size(227, 130);
            this.groupBoxUserCheck.TabIndex = 33;
            this.groupBoxUserCheck.TabStop = false;
            this.groupBoxUserCheck.Text = "Check user";
            // 
            // checkBoxUserExpansions
            // 
            this.checkBoxUserExpansions.AutoSize = true;
            this.checkBoxUserExpansions.Checked = true;
            this.checkBoxUserExpansions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUserExpansions.Location = new System.Drawing.Point(20, 45);
            this.checkBoxUserExpansions.Name = "checkBoxUserExpansions";
            this.checkBoxUserExpansions.Size = new System.Drawing.Size(126, 17);
            this.checkBoxUserExpansions.TabIndex = 44;
            this.checkBoxUserExpansions.Text = "Check all expansions";
            this.toolTip1.SetToolTip(this.checkBoxUserExpansions, "If checked, all cards from the user will checked. If unchecked, only cards from t" +
        "he specified expansion will be taken into account");
            this.checkBoxUserExpansions.UseVisualStyleBackColor = true;
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(17, 22);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(69, 13);
            this.labelUserName.TabIndex = 43;
            this.labelUserName.Text = "Seller\'s name";
            // 
            // buttonCheckUser
            // 
            this.buttonCheckUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCheckUser.Location = new System.Drawing.Point(20, 68);
            this.buttonCheckUser.Name = "buttonCheckUser";
            this.buttonCheckUser.Size = new System.Drawing.Size(188, 51);
            this.buttonCheckUser.TabIndex = 6;
            this.buttonCheckUser.Text = "Check user\'s stock";
            this.buttonCheckUser.UseVisualStyleBackColor = true;
            this.buttonCheckUser.Click += new System.EventHandler(this.buttonCheckUser_Click);
            // 
            // groupBoxParams
            // 
            this.groupBoxParams.Controls.Add(this.groupBoxPriceParam);
            this.groupBoxParams.Controls.Add(this.groupBoxCardParam);
            this.groupBoxParams.Location = new System.Drawing.Point(12, 12);
            this.groupBoxParams.Name = "groupBoxParams";
            this.groupBoxParams.Size = new System.Drawing.Size(232, 398);
            this.groupBoxParams.TabIndex = 34;
            this.groupBoxParams.TabStop = false;
            this.groupBoxParams.Text = "Parameters";
            // 
            // groupBoxPerform
            // 
            this.groupBoxPerform.Controls.Add(this.groupBoxBulkCheck);
            this.groupBoxPerform.Controls.Add(this.groupBoxWantlist);
            this.groupBoxPerform.Controls.Add(this.buttonEmptyCart);
            this.groupBoxPerform.Controls.Add(this.groupBoxUserCheck);
            this.groupBoxPerform.Location = new System.Drawing.Point(250, 12);
            this.groupBoxPerform.Name = "groupBoxPerform";
            this.groupBoxPerform.Size = new System.Drawing.Size(240, 398);
            this.groupBoxPerform.TabIndex = 35;
            this.groupBoxPerform.TabStop = false;
            this.groupBoxPerform.Text = "Perform check";
            // 
            // CheckWantsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 428);
            this.Controls.Add(this.groupBoxPerform);
            this.Controls.Add(this.groupBoxParams);
            this.MinimizeBox = false;
            this.Name = "CheckWantsView";
            this.ShowIcon = false;
            this.Text = "Check for cheap deals";
            this.Shown += new System.EventHandler(this.CheckWantsView_Shown);
            this.VisibleChanged += new System.EventHandler(this.CheckWantsView_VisibleChanged);
            this.groupBoxPriceParam.ResumeLayout(false);
            this.groupBoxPriceParam.PerformLayout();
            this.groupBoxCardParam.ResumeLayout(false);
            this.groupBoxCardParam.PerformLayout();
            this.groupBoxWantlist.ResumeLayout(false);
            this.groupBoxBulkCheck.ResumeLayout(false);
            this.groupBoxUserCheck.ResumeLayout(false);
            this.groupBoxUserCheck.PerformLayout();
            this.groupBoxParams.ResumeLayout(false);
            this.groupBoxPerform.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox wantListsBox2;
        private System.Windows.Forms.Button checkListButton;
        private System.Windows.Forms.TextBox percentText;
        private System.Windows.Forms.Label labelPercent;
        private System.Windows.Forms.TextBox shipAddition;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonEmptyCart;
        private System.Windows.Forms.ComboBox editionBox;
        private System.Windows.Forms.Button checkEditionButton;
        private System.Windows.Forms.GroupBox groupBoxPriceParam;
        private System.Windows.Forms.GroupBox groupBoxCardParam;
        private System.Windows.Forms.GroupBox groupBoxWantlist;
        private System.Windows.Forms.CheckBox signedBox;
        private System.Windows.Forms.CheckBox alteredBox;
        private System.Windows.Forms.CheckBox playsetBox;
        private System.Windows.Forms.CheckBox foilBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox langCombo;
        private System.Windows.Forms.Label conditionBox;
        private System.Windows.Forms.ComboBox conditionCombo;
        private System.Windows.Forms.CheckBox checkBoxTrend;
        private System.Windows.Forms.TextBox maxPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox domesticCheck;
        private System.Windows.Forms.CheckBox checkBestInter;
        private System.Windows.Forms.TextBox textBoxUser;
        private System.Windows.Forms.GroupBox groupBoxBulkCheck;
        private System.Windows.Forms.GroupBox groupBoxUserCheck;
        private System.Windows.Forms.CheckBox checkBoxUserExpansions;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Button buttonCheckUser;
        private System.Windows.Forms.GroupBox groupBoxParams;
        private System.Windows.Forms.GroupBox groupBoxPerform;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}