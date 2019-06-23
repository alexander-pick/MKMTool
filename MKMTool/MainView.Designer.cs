namespace MKMTool
{
    partial class MainView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            this.loginButton = new System.Windows.Forms.Button();
            this.readStockButton = new System.Windows.Forms.Button();
            this.logBox = new System.Windows.Forms.TextBox();
            this.updatePriceButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.getProductListButton = new System.Windows.Forms.Button();
            this.runtimeIntervall = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.autoUpdateCheck = new System.Windows.Forms.CheckBox();
            this.status = new System.Windows.Forms.Label();
            this.checkWants = new System.Windows.Forms.Button();
            this.wantlistEditButton = new System.Windows.Forms.Button();
            this.checkDisplayPriceButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.downloadBuysToExcel = new System.Windows.Forms.Button();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.labelRequestCounter = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(532, 347);
            this.loginButton.Margin = new System.Windows.Forms.Padding(2);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(102, 22);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Account Info";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // readStockButton
            // 
            this.readStockButton.Location = new System.Drawing.Point(638, 347);
            this.readStockButton.Margin = new System.Windows.Forms.Padding(2);
            this.readStockButton.Name = "readStockButton";
            this.readStockButton.Size = new System.Drawing.Size(102, 22);
            this.readStockButton.TabIndex = 3;
            this.readStockButton.Text = "View Inventory Stock";
            this.readStockButton.UseVisualStyleBackColor = true;
            this.readStockButton.Click += new System.EventHandler(this.readStockButton_Click);
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(109, 11);
            this.logBox.Margin = new System.Windows.Forms.Padding(2);
            this.logBox.Multiline = true;
            this.logBox.Name = "logBox";
            this.logBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logBox.Size = new System.Drawing.Size(631, 324);
            this.logBox.TabIndex = 4;
            // 
            // updatePriceButton
            // 
            this.updatePriceButton.Location = new System.Drawing.Point(3, 69);
            this.updatePriceButton.Margin = new System.Windows.Forms.Padding(2);
            this.updatePriceButton.Name = "updatePriceButton";
            this.updatePriceButton.Size = new System.Drawing.Size(102, 41);
            this.updatePriceButton.TabIndex = 5;
            this.updatePriceButton.Text = "Update Prices";
            this.updatePriceButton.UseVisualStyleBackColor = true;
            this.updatePriceButton.Click += new System.EventHandler(this.updatePriceButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(10, 482);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 13);
            this.statusLabel.TabIndex = 8;
            // 
            // getProductListButton
            // 
            this.getProductListButton.Location = new System.Drawing.Point(3, 249);
            this.getProductListButton.Margin = new System.Windows.Forms.Padding(2);
            this.getProductListButton.Name = "getProductListButton";
            this.getProductListButton.Size = new System.Drawing.Size(102, 41);
            this.getProductListButton.TabIndex = 9;
            this.getProductListButton.Text = "Update local MKM Product List";
            this.getProductListButton.UseVisualStyleBackColor = true;
            this.getProductListButton.Click += new System.EventHandler(this.getProductListButton_Click);
            // 
            // runtimeIntervall
            // 
            this.runtimeIntervall.Location = new System.Drawing.Point(3, 347);
            this.runtimeIntervall.Name = "runtimeIntervall";
            this.runtimeIntervall.Size = new System.Drawing.Size(34, 20);
            this.runtimeIntervall.TabIndex = 10;
            this.runtimeIntervall.Text = "1440";
            this.runtimeIntervall.TextChanged += new System.EventHandler(this.runtimeIntervall_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 350);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Minutes";
            // 
            // autoUpdateCheck
            // 
            this.autoUpdateCheck.AutoSize = true;
            this.autoUpdateCheck.Location = new System.Drawing.Point(85, 349);
            this.autoUpdateCheck.Name = "autoUpdateCheck";
            this.autoUpdateCheck.Size = new System.Drawing.Size(87, 17);
            this.autoUpdateCheck.TabIndex = 12;
            this.autoUpdateCheck.Text = "Bot Mode on";
            this.autoUpdateCheck.UseVisualStyleBackColor = true;
            this.autoUpdateCheck.CheckedChanged += new System.EventHandler(this.autoUpdateCheck_CheckedChanged);
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(10, 26);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(72, 13);
            this.status.TabIndex = 13;
            this.status.Text = "Manual Mode";
            // 
            // checkWants
            // 
            this.checkWants.Location = new System.Drawing.Point(3, 114);
            this.checkWants.Margin = new System.Windows.Forms.Padding(2);
            this.checkWants.Name = "checkWants";
            this.checkWants.Size = new System.Drawing.Size(102, 41);
            this.checkWants.TabIndex = 14;
            this.checkWants.Text = "Check for cheap deals";
            this.checkWants.UseVisualStyleBackColor = true;
            this.checkWants.Click += new System.EventHandler(this.checkWants_Click);
            // 
            // wantlistEditButton
            // 
            this.wantlistEditButton.Location = new System.Drawing.Point(3, 204);
            this.wantlistEditButton.Margin = new System.Windows.Forms.Padding(2);
            this.wantlistEditButton.Name = "wantlistEditButton";
            this.wantlistEditButton.Size = new System.Drawing.Size(102, 41);
            this.wantlistEditButton.TabIndex = 15;
            this.wantlistEditButton.Text = "Wantlist Editor";
            this.wantlistEditButton.UseVisualStyleBackColor = true;
            this.wantlistEditButton.Click += new System.EventHandler(this.wantlistButton_Click);
            // 
            // checkDisplayPriceButton
            // 
            this.checkDisplayPriceButton.Location = new System.Drawing.Point(3, 159);
            this.checkDisplayPriceButton.Margin = new System.Windows.Forms.Padding(2);
            this.checkDisplayPriceButton.Name = "checkDisplayPriceButton";
            this.checkDisplayPriceButton.Size = new System.Drawing.Size(102, 41);
            this.checkDisplayPriceButton.TabIndex = 16;
            this.checkDisplayPriceButton.Text = "Check Display Value";
            this.checkDisplayPriceButton.UseVisualStyleBackColor = true;
            this.checkDisplayPriceButton.Click += new System.EventHandler(this.checkDisplayPriceButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.status);
            this.groupBox1.Location = new System.Drawing.Point(3, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(102, 53);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Bot Status";
            // 
            // downloadBuysToExcel
            // 
            this.downloadBuysToExcel.Location = new System.Drawing.Point(3, 294);
            this.downloadBuysToExcel.Margin = new System.Windows.Forms.Padding(2);
            this.downloadBuysToExcel.Name = "downloadBuysToExcel";
            this.downloadBuysToExcel.Size = new System.Drawing.Size(102, 41);
            this.downloadBuysToExcel.TabIndex = 18;
            this.downloadBuysToExcel.Text = "Download Buys to Excel";
            this.downloadBuysToExcel.UseVisualStyleBackColor = true;
            this.downloadBuysToExcel.Click += new System.EventHandler(this.downloadBuysToExcel_Click);
            // 
            // buttonSettings
            // 
            this.buttonSettings.Location = new System.Drawing.Point(426, 347);
            this.buttonSettings.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(102, 22);
            this.buttonSettings.TabIndex = 19;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click);
            // 
            // labelRequestCounter
            // 
            this.labelRequestCounter.AutoSize = true;
            this.labelRequestCounter.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelRequestCounter.Location = new System.Drawing.Point(178, 350);
            this.labelRequestCounter.Name = "labelRequestCounter";
            this.labelRequestCounter.Size = new System.Drawing.Size(243, 13);
            this.labelRequestCounter.TabIndex = 20;
            this.labelRequestCounter.Text = "API Requests made/allowed: ??????? / ???????";
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 379);
            this.Controls.Add(this.labelRequestCounter);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.downloadBuysToExcel);
            this.Controls.Add(this.checkDisplayPriceButton);
            this.Controls.Add(this.wantlistEditButton);
            this.Controls.Add(this.checkWants);
            this.Controls.Add(this.autoUpdateCheck);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.runtimeIntervall);
            this.Controls.Add(this.getProductListButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.updatePriceButton);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.readStockButton);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MainView";
            this.Text = "MKMTool 0.6 - Alexander Pick 2017 - Licensed under GPL v3 ";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Button readStockButton;
        private System.Windows.Forms.Button updatePriceButton;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button getProductListButton;
        public System.Windows.Forms.TextBox logBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label status;
        public System.Windows.Forms.TextBox runtimeIntervall;
        public System.Windows.Forms.CheckBox autoUpdateCheck;
        private System.Windows.Forms.Button checkWants;
        private System.Windows.Forms.Button wantlistEditButton;
        private System.Windows.Forms.Button checkDisplayPriceButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button downloadBuysToExcel;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.Label labelRequestCounter;
    }
}

