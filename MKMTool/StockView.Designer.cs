namespace MKMTool
{
    partial class StockView
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
            this.stockGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.stockGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // stockGridView
            // 
            this.stockGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.stockGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stockGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stockGridView.Location = new System.Drawing.Point(0, 0);
            this.stockGridView.Name = "stockGridView";
            this.stockGridView.Size = new System.Drawing.Size(920, 550);
            this.stockGridView.TabIndex = 0;
            // 
            // StockView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 550);
            this.Controls.Add(this.stockGridView);
            this.Name = "StockView";
            this.Text = "StockView";
            ((System.ComponentModel.ISupportInitialize)(this.stockGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView stockGridView;
    }
}