namespace MKMTool
{
    partial class SettingPresetStore
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
            this.buttonStore = new System.Windows.Forms.Button();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelChooseName = new System.Windows.Forms.Label();
            this.panelDescription = new System.Windows.Forms.Panel();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStore
            // 
            this.buttonStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStore.Location = new System.Drawing.Point(153, 169);
            this.buttonStore.Name = "buttonStore";
            this.buttonStore.Size = new System.Drawing.Size(79, 34);
            this.buttonStore.TabIndex = 0;
            this.buttonStore.Text = "Store";
            this.buttonStore.UseVisualStyleBackColor = true;
            this.buttonStore.Click += new System.EventHandler(this.buttonStore_Click);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFileName.Location = new System.Drawing.Point(12, 46);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(260, 26);
            this.textBoxFileName.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelChooseName);
            this.panel1.Location = new System.Drawing.Point(12, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 34);
            this.panel1.TabIndex = 3;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(56, 169);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(79, 34);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelChooseName
            // 
            this.labelChooseName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelChooseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelChooseName.Location = new System.Drawing.Point(0, 0);
            this.labelChooseName.Name = "labelChooseName";
            this.labelChooseName.Size = new System.Drawing.Size(260, 34);
            this.labelChooseName.TabIndex = 3;
            this.labelChooseName.Text = "Preset Name";
            this.labelChooseName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelDescription
            // 
            this.panelDescription.Controls.Add(this.labelDescription);
            this.panelDescription.Location = new System.Drawing.Point(12, 78);
            this.panelDescription.Name = "panelDescription";
            this.panelDescription.Size = new System.Drawing.Size(260, 34);
            this.panelDescription.TabIndex = 5;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDescription.Location = new System.Drawing.Point(9, 115);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(260, 48);
            this.textBoxDescription.TabIndex = 4;
            this.textBoxDescription.Text = "1\r\n2\r\n3";
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDescription.Location = new System.Drawing.Point(0, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(260, 34);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Preset Description";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingPresetStore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 210);
            this.Controls.Add(this.panelDescription);
            this.Controls.Add(this.textBoxDescription);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.buttonStore);
            this.Name = "SettingPresetStore";
            this.Text = "Store Settings Preset";
            this.panel1.ResumeLayout(false);
            this.panelDescription.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStore;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelChooseName;
        private System.Windows.Forms.Panel panelDescription;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxDescription;
    }
}