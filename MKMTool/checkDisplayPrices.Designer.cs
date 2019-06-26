namespace MKMTool
{
    partial class CheckDisplayPrices
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
            this.editionBox = new System.Windows.Forms.ComboBox();
            this.checkDisplayPrice = new System.Windows.Forms.Button();
            this.mythicPerRareText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelxy = new System.Windows.Forms.Label();
            this.raremythicPerPackText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.uncommonPerPackText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mythicNotInBoosterText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rareNotInBoosterText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.uncommonNotInBoosterText = new System.Windows.Forms.TextBox();
            this.boosterPerBoxText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // editionBox
            // 
            this.editionBox.FormattingEnabled = true;
            this.editionBox.Location = new System.Drawing.Point(12, 12);
            this.editionBox.Name = "editionBox";
            this.editionBox.Size = new System.Drawing.Size(260, 21);
            this.editionBox.TabIndex = 27;
            // 
            // checkDisplayPrice
            // 
            this.checkDisplayPrice.Location = new System.Drawing.Point(12, 221);
            this.checkDisplayPrice.Name = "checkDisplayPrice";
            this.checkDisplayPrice.Size = new System.Drawing.Size(260, 45);
            this.checkDisplayPrice.TabIndex = 28;
            this.checkDisplayPrice.Text = "Check expected sale value of singles in a display";
            this.checkDisplayPrice.UseVisualStyleBackColor = true;
            this.checkDisplayPrice.Click += new System.EventHandler(this.checkDisplayPrice_Click);
            // 
            // mythicPerRareText
            // 
            this.mythicPerRareText.Location = new System.Drawing.Point(12, 39);
            this.mythicPerRareText.Name = "mythicPerRareText";
            this.mythicPerRareText.Size = new System.Drawing.Size(57, 20);
            this.mythicPerRareText.TabIndex = 29;
            this.mythicPerRareText.Text = "8";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(75, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Mythic per Rare (1:x)";
            // 
            // labelxy
            // 
            this.labelxy.AutoSize = true;
            this.labelxy.Location = new System.Drawing.Point(75, 68);
            this.labelxy.Name = "labelxy";
            this.labelxy.Size = new System.Drawing.Size(133, 13);
            this.labelxy.TabIndex = 35;
            this.labelxy.Text = "Rare or Mythic per Booster";
            // 
            // raremythicPerPackText
            // 
            this.raremythicPerPackText.Location = new System.Drawing.Point(12, 65);
            this.raremythicPerPackText.Name = "raremythicPerPackText";
            this.raremythicPerPackText.Size = new System.Drawing.Size(57, 20);
            this.raremythicPerPackText.TabIndex = 34;
            this.raremythicPerPackText.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Uncommon per Booster";
            // 
            // uncommonPerPackText
            // 
            this.uncommonPerPackText.Location = new System.Drawing.Point(12, 91);
            this.uncommonPerPackText.Name = "uncommonPerPackText";
            this.uncommonPerPackText.Size = new System.Drawing.Size(57, 20);
            this.uncommonPerPackText.TabIndex = 36;
            this.uncommonPerPackText.Text = "3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Mythic not in Boosters";
            // 
            // mythicNotInBoosterText
            // 
            this.mythicNotInBoosterText.Location = new System.Drawing.Point(12, 143);
            this.mythicNotInBoosterText.Name = "mythicNotInBoosterText";
            this.mythicNotInBoosterText.Size = new System.Drawing.Size(57, 20);
            this.mythicNotInBoosterText.TabIndex = 40;
            this.mythicNotInBoosterText.Text = "2";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 39;
            this.label4.Text = "Rare not in Boosters";
            // 
            // rareNotInBoosterText
            // 
            this.rareNotInBoosterText.Location = new System.Drawing.Point(12, 117);
            this.rareNotInBoosterText.Name = "rareNotInBoosterText";
            this.rareNotInBoosterText.Size = new System.Drawing.Size(57, 20);
            this.rareNotInBoosterText.TabIndex = 38;
            this.rareNotInBoosterText.Text = "2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 13);
            this.label5.TabIndex = 43;
            this.label5.Text = "Uncommon not in Boosters";
            // 
            // uncommonNotInBoosterText
            // 
            this.uncommonNotInBoosterText.Location = new System.Drawing.Point(13, 169);
            this.uncommonNotInBoosterText.Name = "uncommonNotInBoosterText";
            this.uncommonNotInBoosterText.Size = new System.Drawing.Size(57, 20);
            this.uncommonNotInBoosterText.TabIndex = 42;
            this.uncommonNotInBoosterText.Text = "2";
            // 
            // boosterPerBoxText
            // 
            this.boosterPerBoxText.Location = new System.Drawing.Point(12, 195);
            this.boosterPerBoxText.Name = "boosterPerBoxText";
            this.boosterPerBoxText.Size = new System.Drawing.Size(57, 20);
            this.boosterPerBoxText.TabIndex = 44;
            this.boosterPerBoxText.Text = "36";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(75, 198);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 45;
            this.label6.Text = "Booster per Box";
            // 
            // CheckDisplayPrices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 275);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.boosterPerBoxText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.uncommonNotInBoosterText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mythicNotInBoosterText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.rareNotInBoosterText);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uncommonPerPackText);
            this.Controls.Add(this.labelxy);
            this.Controls.Add(this.raremythicPerPackText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mythicPerRareText);
            this.Controls.Add(this.checkDisplayPrice);
            this.Controls.Add(this.editionBox);
            this.MaximizeBox = false;
            this.Name = "CheckDisplayPrices";
            this.ShowIcon = false;
            this.Text = "Check Expected Display Value";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox editionBox;
        private System.Windows.Forms.Button checkDisplayPrice;
        private System.Windows.Forms.TextBox mythicPerRareText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelxy;
        private System.Windows.Forms.TextBox raremythicPerPackText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox uncommonPerPackText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox mythicNotInBoosterText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox rareNotInBoosterText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox uncommonNotInBoosterText;
        private System.Windows.Forms.TextBox boosterPerBoxText;
        private System.Windows.Forms.Label label6;
    }
}