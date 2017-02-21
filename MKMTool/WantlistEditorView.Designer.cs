namespace MKMTool
{
    partial class WantlistEditorView
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
            this.searchBox = new System.Windows.Forms.TextBox();
            this.wantListsBox = new System.Windows.Forms.ComboBox();
            this.cardView = new System.Windows.Forms.ListView();
            this.addButton = new System.Windows.Forms.Button();
            this.wantsView = new System.Windows.Forms.DataGridView();
            this.deleteItemButton = new System.Windows.Forms.Button();
            this.conditionCombo = new System.Windows.Forms.ComboBox();
            this.conditionBox = new System.Windows.Forms.Label();
            this.langCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.foilBox = new System.Windows.Forms.CheckBox();
            this.playsetBox = new System.Windows.Forms.CheckBox();
            this.alteredBox = new System.Windows.Forms.CheckBox();
            this.signedBox = new System.Windows.Forms.CheckBox();
            this.editionBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.wantsView)).BeginInit();
            this.SuspendLayout();
            // 
            // searchBox
            // 
            this.searchBox.Location = new System.Drawing.Point(2, 10);
            this.searchBox.Margin = new System.Windows.Forms.Padding(2);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(279, 20);
            this.searchBox.TabIndex = 0;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // wantListsBox
            // 
            this.wantListsBox.FormattingEnabled = true;
            this.wantListsBox.Location = new System.Drawing.Point(460, 9);
            this.wantListsBox.Margin = new System.Windows.Forms.Padding(2);
            this.wantListsBox.Name = "wantListsBox";
            this.wantListsBox.Size = new System.Drawing.Size(695, 21);
            this.wantListsBox.TabIndex = 3;
            this.wantListsBox.SelectedIndexChanged += new System.EventHandler(this.wantListsBox_SelectedIndexChanged);
            // 
            // cardView
            // 
            this.cardView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.cardView.Location = new System.Drawing.Point(2, 36);
            this.cardView.Name = "cardView";
            this.cardView.Size = new System.Drawing.Size(454, 503);
            this.cardView.TabIndex = 13;
            this.cardView.UseCompatibleStateImageBehavior = false;
            this.cardView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.cardView_ColumnClick);
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(2, 594);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(454, 23);
            this.addButton.TabIndex = 15;
            this.addButton.Text = "Add selected cards to MKM Wantlist";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // wantsView
            // 
            this.wantsView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wantsView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.wantsView.Location = new System.Drawing.Point(462, 36);
            this.wantsView.Name = "wantsView";
            this.wantsView.Size = new System.Drawing.Size(693, 550);
            this.wantsView.TabIndex = 16;
            // 
            // deleteItemButton
            // 
            this.deleteItemButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteItemButton.Location = new System.Drawing.Point(460, 594);
            this.deleteItemButton.Margin = new System.Windows.Forms.Padding(2);
            this.deleteItemButton.Name = "deleteItemButton";
            this.deleteItemButton.Size = new System.Drawing.Size(179, 23);
            this.deleteItemButton.TabIndex = 17;
            this.deleteItemButton.Text = "Delete Item";
            this.deleteItemButton.UseVisualStyleBackColor = true;
            this.deleteItemButton.Click += new System.EventHandler(this.deleteItemButton_Click);
            // 
            // conditionCombo
            // 
            this.conditionCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.conditionCombo.FormattingEnabled = true;
            this.conditionCombo.Items.AddRange(new object[] {
            "MT",
            "NM ",
            "EX",
            "GD",
            "LP",
            "PL",
            "PO"});
            this.conditionCombo.Location = new System.Drawing.Point(2, 544);
            this.conditionCombo.Margin = new System.Windows.Forms.Padding(2);
            this.conditionCombo.Name = "conditionCombo";
            this.conditionCombo.Size = new System.Drawing.Size(90, 21);
            this.conditionCombo.TabIndex = 18;
            // 
            // conditionBox
            // 
            this.conditionBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.conditionBox.AutoSize = true;
            this.conditionBox.Location = new System.Drawing.Point(93, 547);
            this.conditionBox.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.conditionBox.Name = "conditionBox";
            this.conditionBox.Size = new System.Drawing.Size(95, 13);
            this.conditionBox.TabIndex = 19;
            this.conditionBox.Text = "Minimum Condition";
            // 
            // langCombo
            // 
            this.langCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.langCombo.FormattingEnabled = true;
            this.langCombo.Location = new System.Drawing.Point(2, 570);
            this.langCombo.Margin = new System.Windows.Forms.Padding(2);
            this.langCombo.Name = "langCombo";
            this.langCombo.Size = new System.Drawing.Size(90, 21);
            this.langCombo.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 573);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Language";
            // 
            // foilBox
            // 
            this.foilBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.foilBox.AutoSize = true;
            this.foilBox.Location = new System.Drawing.Point(323, 549);
            this.foilBox.Margin = new System.Windows.Forms.Padding(2);
            this.foilBox.Name = "foilBox";
            this.foilBox.Size = new System.Drawing.Size(42, 17);
            this.foilBox.TabIndex = 22;
            this.foilBox.Text = "Foil";
            this.foilBox.UseVisualStyleBackColor = true;
            // 
            // playsetBox
            // 
            this.playsetBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playsetBox.AutoSize = true;
            this.playsetBox.Location = new System.Drawing.Point(323, 570);
            this.playsetBox.Margin = new System.Windows.Forms.Padding(2);
            this.playsetBox.Name = "playsetBox";
            this.playsetBox.Size = new System.Drawing.Size(60, 17);
            this.playsetBox.TabIndex = 23;
            this.playsetBox.Text = "Playset";
            this.playsetBox.UseVisualStyleBackColor = true;
            // 
            // alteredBox
            // 
            this.alteredBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.alteredBox.AutoSize = true;
            this.alteredBox.Location = new System.Drawing.Point(396, 549);
            this.alteredBox.Margin = new System.Windows.Forms.Padding(2);
            this.alteredBox.Name = "alteredBox";
            this.alteredBox.Size = new System.Drawing.Size(59, 17);
            this.alteredBox.TabIndex = 24;
            this.alteredBox.Text = "Altered";
            this.alteredBox.UseVisualStyleBackColor = true;
            // 
            // signedBox
            // 
            this.signedBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.signedBox.AutoSize = true;
            this.signedBox.Location = new System.Drawing.Point(396, 570);
            this.signedBox.Margin = new System.Windows.Forms.Padding(2);
            this.signedBox.Name = "signedBox";
            this.signedBox.Size = new System.Drawing.Size(59, 17);
            this.signedBox.TabIndex = 25;
            this.signedBox.Text = "Signed";
            this.signedBox.UseVisualStyleBackColor = true;
            // 
            // editionBox
            // 
            this.editionBox.FormattingEnabled = true;
            this.editionBox.Location = new System.Drawing.Point(286, 9);
            this.editionBox.Name = "editionBox";
            this.editionBox.Size = new System.Drawing.Size(170, 21);
            this.editionBox.TabIndex = 26;
            this.editionBox.SelectedIndexChanged += new System.EventHandler(this.editionBox_SelectedIndexChanged);
            // 
            // WantlistEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 623);
            this.Controls.Add(this.editionBox);
            this.Controls.Add(this.signedBox);
            this.Controls.Add(this.alteredBox);
            this.Controls.Add(this.playsetBox);
            this.Controls.Add(this.foilBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.langCombo);
            this.Controls.Add(this.conditionBox);
            this.Controls.Add(this.conditionCombo);
            this.Controls.Add(this.deleteItemButton);
            this.Controls.Add(this.wantsView);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.wantListsBox);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.cardView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WantlistEditor";
            this.ShowIcon = false;
            this.Text = "Wantlist Editor";
            this.Load += new System.EventHandler(this.WantlistEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.wantsView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.ComboBox wantListsBox;
        private System.Windows.Forms.ListView cardView;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.DataGridView wantsView;
        private System.Windows.Forms.Button deleteItemButton;
        private System.Windows.Forms.ComboBox conditionCombo;
        private System.Windows.Forms.Label conditionBox;
        private System.Windows.Forms.ComboBox langCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox foilBox;
        private System.Windows.Forms.CheckBox playsetBox;
        private System.Windows.Forms.CheckBox alteredBox;
        private System.Windows.Forms.CheckBox signedBox;
        private System.Windows.Forms.ComboBox editionBox;
    }
}