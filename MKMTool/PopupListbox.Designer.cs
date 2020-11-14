namespace MKMTool
{
    partial class PopupListbox
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
            this.listBoxContent = new System.Windows.Forms.ListBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.panelForLabel = new System.Windows.Forms.Panel();
            this.labelHint = new System.Windows.Forms.Label();
            this.panelHintText = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelForLabel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxContent
            // 
            this.listBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxContent.FormattingEnabled = true;
            this.listBoxContent.ItemHeight = 18;
            this.listBoxContent.Location = new System.Drawing.Point(12, 38);
            this.listBoxContent.Name = "listBoxContent";
            this.listBoxContent.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxContent.Size = new System.Drawing.Size(258, 580);
            this.listBoxContent.TabIndex = 0;
            this.listBoxContent.SelectedValueChanged += new System.EventHandler(this.listBoxContent_SelectedValueChanged);
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(258, 33);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "List title";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonConfirm.Location = new System.Drawing.Point(12, 634);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(258, 34);
            this.buttonConfirm.TabIndex = 2;
            this.buttonConfirm.Text = "OK";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.ButtonConfirm_Click);
            // 
            // panelForLabel
            // 
            this.panelForLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelForLabel.Controls.Add(this.labelTitle);
            this.panelForLabel.Location = new System.Drawing.Point(12, 0);
            this.panelForLabel.Name = "panelForLabel";
            this.panelForLabel.Size = new System.Drawing.Size(258, 33);
            this.panelForLabel.TabIndex = 3;
            // 
            // labelHint
            // 
            this.labelHint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHint.Location = new System.Drawing.Point(0, 0);
            this.labelHint.Name = "labelHint";
            this.labelHint.Size = new System.Drawing.Size(258, 13);
            this.labelHint.TabIndex = 4;
            this.labelHint.Text = "Hold CTRL to select multiple";
            this.labelHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelHintText
            // 
            this.panelHintText.Location = new System.Drawing.Point(0, 0);
            this.panelHintText.Name = "panelHintText";
            this.panelHintText.Size = new System.Drawing.Size(200, 100);
            this.panelHintText.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.labelHint);
            this.panel1.Location = new System.Drawing.Point(12, 620);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 13);
            this.panel1.TabIndex = 5;
            // 
            // PopupListbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 669);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonConfirm);
            this.Controls.Add(this.listBoxContent);
            this.Controls.Add(this.panelForLabel);
            this.Name = "PopupListbox";
            this.Text = "PopupListbox";
            this.panelForLabel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxContent;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.Panel panelForLabel;
        private System.Windows.Forms.Panel panelHintText;
        private System.Windows.Forms.Label labelHint;
        private System.Windows.Forms.Panel panel1;
    }
}