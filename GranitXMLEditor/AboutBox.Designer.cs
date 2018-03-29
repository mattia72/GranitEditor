namespace GranitEditor
{
  partial class AboutBox
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
        scrollStop.Dispose();
        scrollThreadReady.Dispose();
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
      this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      this.labelBuildDateTimeText = new System.Windows.Forms.Label();
      this.labelCopyright = new System.Windows.Forms.Label();
      this.labelVersionText = new System.Windows.Forms.Label();
      this.labelProductNameText = new System.Windows.Forms.Label();
      this.logoPictureBox = new System.Windows.Forms.PictureBox();
      this.labelProductName = new System.Windows.Forms.Label();
      this.labelVersion = new System.Windows.Forms.Label();
      this.labelBuildDateTime = new System.Windows.Forms.Label();
      this.labelCopyrightText = new System.Windows.Forms.Label();
      this.labelHomePage = new System.Windows.Forms.Label();
      this.textBoxDescription = new System.Windows.Forms.TextBox();
      this.okButton = new System.Windows.Forms.Button();
      this.linkHomePage = new System.Windows.Forms.LinkLabel();
      this.labelLicense = new System.Windows.Forms.Label();
      this.tableLayoutPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // tableLayoutPanel
      // 
      this.tableLayoutPanel.ColumnCount = 3;
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.81042F));
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.86268F));
      this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.3269F));
      this.tableLayoutPanel.Controls.Add(this.labelBuildDateTimeText, 2, 2);
      this.tableLayoutPanel.Controls.Add(this.labelCopyright, 0, 3);
      this.tableLayoutPanel.Controls.Add(this.labelVersionText, 2, 1);
      this.tableLayoutPanel.Controls.Add(this.labelProductNameText, 2, 0);
      this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
      this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
      this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
      this.tableLayoutPanel.Controls.Add(this.labelBuildDateTime, 1, 2);
      this.tableLayoutPanel.Controls.Add(this.labelCopyrightText, 1, 3);
      this.tableLayoutPanel.Controls.Add(this.labelHomePage, 1, 4);
      this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 7);
      this.tableLayoutPanel.Controls.Add(this.okButton, 2, 8);
      this.tableLayoutPanel.Controls.Add(this.linkHomePage, 2, 4);
      this.tableLayoutPanel.Controls.Add(this.labelLicense, 1, 6);
      this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
      this.tableLayoutPanel.Name = "tableLayoutPanel";
      this.tableLayoutPanel.RowCount = 9;
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.Size = new System.Drawing.Size(459, 306);
      this.tableLayoutPanel.TabIndex = 0;
      // 
      // labelBuildDateTimeText
      // 
      this.labelBuildDateTimeText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelBuildDateTimeText.Location = new System.Drawing.Point(196, 34);
      this.labelBuildDateTimeText.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelBuildDateTimeText.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelBuildDateTimeText.Name = "labelBuildDateTimeText";
      this.labelBuildDateTimeText.Size = new System.Drawing.Size(260, 17);
      this.labelBuildDateTimeText.TabIndex = 28;
      this.labelBuildDateTimeText.Text = "2017.04.12";
      this.labelBuildDateTimeText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelCopyright
      // 
      this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCopyright.Location = new System.Drawing.Point(92, 51);
      this.labelCopyright.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelCopyright.Name = "labelCopyright";
      this.labelCopyright.Size = new System.Drawing.Size(95, 17);
      this.labelCopyright.TabIndex = 27;
      this.labelCopyright.Text = "Copyright";
      this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelVersionText
      // 
      this.labelVersionText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelVersionText.Location = new System.Drawing.Point(196, 17);
      this.labelVersionText.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelVersionText.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelVersionText.Name = "labelVersionText";
      this.labelVersionText.Size = new System.Drawing.Size(260, 17);
      this.labelVersionText.TabIndex = 26;
      this.labelVersionText.Text = "1.0.3";
      this.labelVersionText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelProductNameText
      // 
      this.labelProductNameText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelProductNameText.Location = new System.Drawing.Point(196, 0);
      this.labelProductNameText.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelProductNameText.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelProductNameText.Name = "labelProductNameText";
      this.labelProductNameText.Size = new System.Drawing.Size(260, 17);
      this.labelProductNameText.TabIndex = 25;
      this.labelProductNameText.Text = "Product Name";
      this.labelProductNameText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // logoPictureBox
      // 
      this.logoPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
      this.logoPictureBox.Image = global::GranitEditor.Properties.Resources.GranitEditorIcon;
      this.logoPictureBox.Location = new System.Drawing.Point(3, 3);
      this.logoPictureBox.Name = "logoPictureBox";
      this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 9);
      this.logoPictureBox.Size = new System.Drawing.Size(80, 78);
      this.logoPictureBox.TabIndex = 12;
      this.logoPictureBox.TabStop = false;
      this.logoPictureBox.Click += new System.EventHandler(this.logoPictureBox_Click);
      // 
      // labelProductName
      // 
      this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelProductName.Location = new System.Drawing.Point(92, 0);
      this.labelProductName.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelProductName.Name = "labelProductName";
      this.labelProductName.Size = new System.Drawing.Size(95, 17);
      this.labelProductName.TabIndex = 19;
      this.labelProductName.Text = "Product Name";
      this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelVersion
      // 
      this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelVersion.Location = new System.Drawing.Point(92, 17);
      this.labelVersion.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelVersion.Name = "labelVersion";
      this.labelVersion.Size = new System.Drawing.Size(95, 17);
      this.labelVersion.TabIndex = 0;
      this.labelVersion.Text = "Version";
      this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelBuildDateTime
      // 
      this.labelBuildDateTime.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelBuildDateTime.Location = new System.Drawing.Point(92, 34);
      this.labelBuildDateTime.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelBuildDateTime.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelBuildDateTime.Name = "labelBuildDateTime";
      this.labelBuildDateTime.Size = new System.Drawing.Size(95, 17);
      this.labelBuildDateTime.TabIndex = 22;
      this.labelBuildDateTime.Text = "Build Time";
      this.labelBuildDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelCopyrightText
      // 
      this.labelCopyrightText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCopyrightText.Location = new System.Drawing.Point(196, 51);
      this.labelCopyrightText.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelCopyrightText.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelCopyrightText.Name = "labelCopyrightText";
      this.labelCopyrightText.Size = new System.Drawing.Size(260, 17);
      this.labelCopyrightText.TabIndex = 21;
      this.labelCopyrightText.Text = "Copyright";
      this.labelCopyrightText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelHomePage
      // 
      this.labelHomePage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelHomePage.Location = new System.Drawing.Point(92, 68);
      this.labelHomePage.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelHomePage.MaximumSize = new System.Drawing.Size(0, 17);
      this.labelHomePage.Name = "labelHomePage";
      this.labelHomePage.Size = new System.Drawing.Size(95, 17);
      this.labelHomePage.TabIndex = 22;
      this.labelHomePage.Text = "Home Page";
      this.labelHomePage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxDescription
      // 
      this.tableLayoutPanel.SetColumnSpan(this.textBoxDescription, 2);
      this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBoxDescription.Location = new System.Drawing.Point(92, 104);
      this.textBoxDescription.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
      this.textBoxDescription.Multiline = true;
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.ReadOnly = true;
      this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.textBoxDescription.Size = new System.Drawing.Size(364, 170);
      this.textBoxDescription.TabIndex = 23;
      this.textBoxDescription.TabStop = false;
      this.textBoxDescription.Text = "Description";
      this.textBoxDescription.WordWrap = false;
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.okButton.Location = new System.Drawing.Point(381, 280);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 24;
      this.okButton.Text = "&OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // linkHomePage
      // 
      this.linkHomePage.AutoSize = true;
      this.linkHomePage.Location = new System.Drawing.Point(196, 68);
      this.linkHomePage.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.linkHomePage.Name = "linkHomePage";
      this.linkHomePage.Size = new System.Drawing.Size(76, 13);
      this.linkHomePage.TabIndex = 29;
      this.linkHomePage.TabStop = true;
      this.linkHomePage.Text = "linkHomePage";
      this.linkHomePage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.linkHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHomePage_LinkClicked);
      // 
      // labelLicense
      // 
      this.labelLicense.AutoSize = true;
      this.labelLicense.Location = new System.Drawing.Point(92, 85);
      this.labelLicense.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
      this.labelLicense.Name = "labelLicense";
      this.labelLicense.Size = new System.Drawing.Size(47, 13);
      this.labelLicense.TabIndex = 30;
      this.labelLicense.Text = "License:";
      this.labelLicense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // AboutBox
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(477, 324);
      this.Controls.Add(this.tableLayoutPanel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutBox";
      this.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "AboutBox";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AboutBox_FormClosing);
      this.VisibleChanged += new System.EventHandler(this.AboutBox_VisibleChanged);
      this.tableLayoutPanel.ResumeLayout(false);
      this.tableLayoutPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
    private System.Windows.Forms.PictureBox logoPictureBox;
    private System.Windows.Forms.Label labelProductName;
    private System.Windows.Forms.Label labelVersion;
    private System.Windows.Forms.Label labelCopyrightText;
    private System.Windows.Forms.Label labelBuildDateTime;
    private System.Windows.Forms.Label labelHomePage;
    private System.Windows.Forms.TextBox textBoxDescription;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Label labelVersionText;
    private System.Windows.Forms.Label labelProductNameText;
    private System.Windows.Forms.Label labelBuildDateTimeText;
    private System.Windows.Forms.Label labelCopyright;
    private System.Windows.Forms.LinkLabel linkHomePage;
    private System.Windows.Forms.Label labelLicense;
  }
}
