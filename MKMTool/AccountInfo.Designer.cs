namespace MKMTool
{
  partial class AccountInfo
  {
    /// Required designer variable.
    private System.ComponentModel.IContainer components = null;

    /// Clean up any resources being used.
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

    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    private void InitializeComponent()
    {
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.SuspendLayout();
      // 
      // treeView1
      // 
      this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.treeView1.Location = new System.Drawing.Point(0, 0);
      this.treeView1.Margin = new System.Windows.Forms.Padding(6);
      this.treeView1.Name = "treeView1";
      this.treeView1.Size = new System.Drawing.Size(864, 981);
      this.treeView1.TabIndex = 0;
      // 
      // AccountInfo
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(864, 981);
      this.Controls.Add(this.treeView1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Margin = new System.Windows.Forms.Padding(6);
      this.MaximizeBox = false;
      this.Name = "AccountInfo";
      this.ShowIcon = false;
      this.Text = "Your MKM Account Info";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView treeView1;
  }
}
