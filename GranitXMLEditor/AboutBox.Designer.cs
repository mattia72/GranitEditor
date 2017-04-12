﻿namespace GranitEditor
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
      this.labelCompanyNameText = new System.Windows.Forms.Label();
      this.labelCopyright = new System.Windows.Forms.Label();
      this.labelVersionText = new System.Windows.Forms.Label();
      this.labelProductNameText = new System.Windows.Forms.Label();
      this.logoPictureBox = new System.Windows.Forms.PictureBox();
      this.labelProductName = new System.Windows.Forms.Label();
      this.labelVersion = new System.Windows.Forms.Label();
      this.labelCopyrightText = new System.Windows.Forms.Label();
      this.labelCompanyName = new System.Windows.Forms.Label();
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
      this.tableLayoutPanel.Controls.Add(this.labelCompanyNameText, 2, 3);
      this.tableLayoutPanel.Controls.Add(this.labelCopyright, 0, 2);
      this.tableLayoutPanel.Controls.Add(this.labelVersionText, 2, 1);
      this.tableLayoutPanel.Controls.Add(this.labelProductNameText, 2, 0);
      this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
      this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
      this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
      this.tableLayoutPanel.Controls.Add(this.labelCopyrightText, 1, 2);
      this.tableLayoutPanel.Controls.Add(this.labelCompanyName, 1, 3);
      this.tableLayoutPanel.Controls.Add(this.labelHomePage, 1, 4);
      this.tableLayoutPanel.Controls.Add(this.textBoxDescription, 1, 7);
      this.tableLayoutPanel.Controls.Add(this.okButton, 2, 8);
      this.tableLayoutPanel.Controls.Add(this.linkHomePage, 2, 4);
      this.tableLayoutPanel.Controls.Add(this.labelLicense, 1, 6);
      this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel.Location = new System.Drawing.Point(12, 11);
      this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
      this.tableLayoutPanel.Name = "tableLayoutPanel";
      this.tableLayoutPanel.RowCount = 9;
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel.Size = new System.Drawing.Size(612, 377);
      this.tableLayoutPanel.TabIndex = 0;
      // 
      // labelCompanyNameText
      // 
      this.labelCompanyNameText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCompanyNameText.Location = new System.Drawing.Point(262, 63);
      this.labelCompanyNameText.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelCompanyNameText.MaximumSize = new System.Drawing.Size(0, 21);
      this.labelCompanyNameText.Name = "labelCompanyNameText";
      this.labelCompanyNameText.Size = new System.Drawing.Size(346, 21);
      this.labelCompanyNameText.TabIndex = 28;
      this.labelCompanyNameText.Text = "Company (R)";
      this.labelCompanyNameText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelCopyright
      // 
      this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCopyright.Location = new System.Drawing.Point(123, 42);
      this.labelCopyright.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 21);
      this.labelCopyright.Name = "labelCopyright";
      this.labelCopyright.Size = new System.Drawing.Size(127, 21);
      this.labelCopyright.TabIndex = 27;
      this.labelCopyright.Text = "Copyright";
      this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelVersionText
      // 
      this.labelVersionText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelVersionText.Location = new System.Drawing.Point(262, 21);
      this.labelVersionText.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelVersionText.MaximumSize = new System.Drawing.Size(0, 21);
      this.labelVersionText.Name = "labelVersionText";
      this.labelVersionText.Size = new System.Drawing.Size(346, 21);
      this.labelVersionText.TabIndex = 26;
      this.labelVersionText.Text = "1.0.3";
      this.labelVersionText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelProductNameText
      // 
      this.labelProductNameText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelProductNameText.Location = new System.Drawing.Point(262, 0);
      this.labelProductNameText.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelProductNameText.MaximumSize = new System.Drawing.Size(0, 21);
      this.labelProductNameText.Name = "labelProductNameText";
      this.labelProductNameText.Size = new System.Drawing.Size(346, 21);
      this.labelProductNameText.TabIndex = 25;
      this.labelProductNameText.Text = "Product Name";
      this.labelProductNameText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // logoPictureBox
      // 
      this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.logoPictureBox.Image = global::GranitEditor.Properties.Resources.GranitEditorIcon;
      this.logoPictureBox.Location = new System.Drawing.Point(4, 4);
      this.logoPictureBox.Margin = new System.Windows.Forms.Padding(4);
      this.logoPictureBox.Name = "logoPictureBox";
      this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 9);
      this.logoPictureBox.Size = new System.Drawing.Size(107, 369);
      this.logoPictureBox.TabIndex = 12;
      this.logoPictureBox.TabStop = false;
      this.logoPictureBox.Click += new System.EventHandler(this.logoPictureBox_Click);
      // 
      // labelProductName
      // 
      this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelProductName.Location = new System.Drawing.Point(123, 0);
      this.labelProductName.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelProductName.MaximumSize = new System.Drawing.Size(0, 21);
      this.labelProductName.Name = "labelProductName";
      this.labelProductName.Size = new System.Drawing.Size(127, 21);
      this.labelProductName.TabIndex = 19;
      this.labelProductName.Text = "Product Name";
      this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelVersion
      // 
      this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelVersion.Location = new System.Drawing.Point(123, 21);
      this.labelVersion.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelVersion.MaximumSize = new System.Drawing.Size(0, 21);
      this.labelVersion.Name = "labelVersion";
      this.labelVersion.Size = new System.Drawing.Size(127, 21);
      this.labelVersion.TabIndex = 0;
      this.labelVersion.Text = "Version";
      this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelCopyrightText
      // 
      this.labelCopyrightText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCopyrightText.Location = new System.Drawing.Point(262, 42);
      this.labelCopyrightText.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelCopyrightText.MaximumSize = new System.Drawing.Size(0, 21);
      this.labelCopyrightText.Name = "labelCopyrightText";
      this.labelCopyrightText.Size = new System.Drawing.Size(346, 21);
      this.labelCopyrightText.TabIndex = 21;
      this.labelCopyrightText.Text = "Copyright";
      this.labelCopyrightText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelCompanyName
      // 
      this.labelCompanyName.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelCompanyName.Location = new System.Drawing.Point(123, 63);
      this.labelCompanyName.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelCompanyName.MaximumSize = new System.Drawing.Size(0, 21);
      this.labelCompanyName.Name = "labelCompanyName";
      this.labelCompanyName.Size = new System.Drawing.Size(127, 21);
      this.labelCompanyName.TabIndex = 22;
      this.labelCompanyName.Text = "Company Name";
      this.labelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // labelHomePage
      // 
      this.labelHomePage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.labelHomePage.Location = new System.Drawing.Point(123, 84);
      this.labelHomePage.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelHomePage.MaximumSize = new System.Drawing.Size(0, 21);
      this.labelHomePage.Name = "labelHomePage";
      this.labelHomePage.Size = new System.Drawing.Size(127, 21);
      this.labelHomePage.TabIndex = 22;
      this.labelHomePage.Text = "Home Page";
      this.labelHomePage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // textBoxDescription
      // 
      this.tableLayoutPanel.SetColumnSpan(this.textBoxDescription, 2);
      this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
      this.textBoxDescription.Location = new System.Drawing.Point(123, 157);
      this.textBoxDescription.Margin = new System.Windows.Forms.Padding(8, 4, 4, 4);
      this.textBoxDescription.Multiline = true;
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.ReadOnly = true;
      this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.textBoxDescription.Size = new System.Drawing.Size(485, 180);
      this.textBoxDescription.TabIndex = 23;
      this.textBoxDescription.TabStop = false;
      this.textBoxDescription.Text = "Description";
      this.textBoxDescription.WordWrap = false;
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.okButton.Location = new System.Drawing.Point(508, 345);
      this.okButton.Margin = new System.Windows.Forms.Padding(4);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(100, 28);
      this.okButton.TabIndex = 24;
      this.okButton.Text = "&OK";
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // linkHomePage
      // 
      this.linkHomePage.AutoSize = true;
      this.linkHomePage.Location = new System.Drawing.Point(257, 84);
      this.linkHomePage.Name = "linkHomePage";
      this.linkHomePage.Size = new System.Drawing.Size(99, 17);
      this.linkHomePage.TabIndex = 29;
      this.linkHomePage.TabStop = true;
      this.linkHomePage.Text = "linkHomePage";
      this.linkHomePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHomePage_LinkClicked);
      // 
      // labelLicense
      // 
      this.labelLicense.AutoSize = true;
      this.labelLicense.Location = new System.Drawing.Point(123, 133);
      this.labelLicense.Margin = new System.Windows.Forms.Padding(8, 0, 4, 0);
      this.labelLicense.Name = "labelLicense";
      this.labelLicense.Size = new System.Drawing.Size(61, 17);
      this.labelLicense.TabIndex = 30;
      this.labelLicense.Text = "License:";
      this.labelLicense.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // AboutBox
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(636, 399);
      this.Controls.Add(this.tableLayoutPanel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutBox";
      this.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
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
    private System.Windows.Forms.Label labelCompanyName;
    private System.Windows.Forms.Label labelHomePage;
    private System.Windows.Forms.TextBox textBoxDescription;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Label labelVersionText;
    private System.Windows.Forms.Label labelProductNameText;
    private System.Windows.Forms.Label labelCompanyNameText;
    private System.Windows.Forms.Label labelCopyright;
    private System.Windows.Forms.LinkLabel linkHomePage;
    private System.Windows.Forms.Label labelLicense;
  }
}
