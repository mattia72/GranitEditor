namespace GranitEditor
{
  partial class FindReplaceDlg
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindReplaceDlg));
      this.findComboBox = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.replaceComboBox = new System.Windows.Forms.ComboBox();
      this.findButton = new System.Windows.Forms.Button();
      this.replaceButton = new System.Windows.Forms.Button();
      this.replaceAllButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.matchWholeWordsCheckBox = new System.Windows.Forms.CheckBox();
      this.matchCaseCheckBox = new System.Windows.Forms.CheckBox();
      this.upRadioButton = new System.Windows.Forms.RadioButton();
      this.downRadioButton = new System.Windows.Forms.RadioButton();
      this.selectionRadioButton = new System.Windows.Forms.RadioButton();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.useRegexpCheckBox = new System.Windows.Forms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // findComboBox
      // 
      this.findComboBox.FormattingEnabled = true;
      this.findComboBox.Location = new System.Drawing.Point(111, 12);
      this.findComboBox.Name = "findComboBox";
      this.findComboBox.Size = new System.Drawing.Size(315, 24);
      this.findComboBox.TabIndex = 0;
      this.findComboBox.TextChanged += new System.EventHandler(this.findComboBox_TextChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(72, 17);
      this.label1.TabIndex = 1;
      this.label1.Text = "Fi&nd what:";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 45);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(92, 17);
      this.label2.TabIndex = 3;
      this.label2.Text = "R&eplace with:";
      // 
      // replaceComboBox
      // 
      this.replaceComboBox.FormattingEnabled = true;
      this.replaceComboBox.Location = new System.Drawing.Point(111, 42);
      this.replaceComboBox.Name = "replaceComboBox";
      this.replaceComboBox.Size = new System.Drawing.Size(315, 24);
      this.replaceComboBox.TabIndex = 2;
      // 
      // findButton
      // 
      this.findButton.Location = new System.Drawing.Point(432, 7);
      this.findButton.Name = "findButton";
      this.findButton.Size = new System.Drawing.Size(94, 28);
      this.findButton.TabIndex = 4;
      this.findButton.Text = "&Find";
      this.findButton.UseVisualStyleBackColor = true;
      this.findButton.Click += new System.EventHandler(this.findButton_Click);
      // 
      // replaceButton
      // 
      this.replaceButton.Location = new System.Drawing.Point(432, 41);
      this.replaceButton.Name = "replaceButton";
      this.replaceButton.Size = new System.Drawing.Size(94, 28);
      this.replaceButton.TabIndex = 4;
      this.replaceButton.Text = "&Replace";
      this.replaceButton.UseVisualStyleBackColor = true;
      this.replaceButton.Click += new System.EventHandler(this.replaceButton_Click);
      // 
      // replaceAllButton
      // 
      this.replaceAllButton.Location = new System.Drawing.Point(432, 70);
      this.replaceAllButton.Name = "replaceAllButton";
      this.replaceAllButton.Size = new System.Drawing.Size(94, 28);
      this.replaceAllButton.TabIndex = 4;
      this.replaceAllButton.Text = "Replace &All";
      this.replaceAllButton.UseVisualStyleBackColor = true;
      this.replaceAllButton.Click += new System.EventHandler(this.replaceAllButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(432, 143);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(94, 27);
      this.cancelButton.TabIndex = 5;
      this.cancelButton.Text = "&Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // matchWholeWordsCheckBox
      // 
      this.matchWholeWordsCheckBox.AutoSize = true;
      this.matchWholeWordsCheckBox.Location = new System.Drawing.Point(12, 93);
      this.matchWholeWordsCheckBox.Name = "matchWholeWordsCheckBox";
      this.matchWholeWordsCheckBox.Size = new System.Drawing.Size(149, 21);
      this.matchWholeWordsCheckBox.TabIndex = 6;
      this.matchWholeWordsCheckBox.Text = "Match &whole words";
      this.matchWholeWordsCheckBox.UseVisualStyleBackColor = true;
      this.matchWholeWordsCheckBox.CheckedChanged += new System.EventHandler(this.matchWholeWordsCheckBox_CheckedChanged);
      // 
      // matchCaseCheckBox
      // 
      this.matchCaseCheckBox.AutoSize = true;
      this.matchCaseCheckBox.Location = new System.Drawing.Point(12, 120);
      this.matchCaseCheckBox.Name = "matchCaseCheckBox";
      this.matchCaseCheckBox.Size = new System.Drawing.Size(102, 21);
      this.matchCaseCheckBox.TabIndex = 6;
      this.matchCaseCheckBox.Text = "Match &case";
      this.matchCaseCheckBox.UseVisualStyleBackColor = true;
      this.matchCaseCheckBox.CheckedChanged += new System.EventHandler(this.matchCaseCheckBox_CheckedChanged);
      // 
      // upRadioButton
      // 
      this.upRadioButton.AutoSize = true;
      this.upRadioButton.Location = new System.Drawing.Point(39, 27);
      this.upRadioButton.Name = "upRadioButton";
      this.upRadioButton.Size = new System.Drawing.Size(47, 21);
      this.upRadioButton.TabIndex = 7;
      this.upRadioButton.Text = "Up";
      this.upRadioButton.UseVisualStyleBackColor = true;
      this.upRadioButton.CheckedChanged += new System.EventHandler(this.upRadioButton_CheckedChanged);
      // 
      // downRadioButton
      // 
      this.downRadioButton.AutoSize = true;
      this.downRadioButton.Checked = true;
      this.downRadioButton.Location = new System.Drawing.Point(92, 27);
      this.downRadioButton.Name = "downRadioButton";
      this.downRadioButton.Size = new System.Drawing.Size(64, 21);
      this.downRadioButton.TabIndex = 8;
      this.downRadioButton.TabStop = true;
      this.downRadioButton.Text = "Down";
      this.downRadioButton.UseVisualStyleBackColor = true;
      // 
      // selectionRadioButton
      // 
      this.selectionRadioButton.AutoSize = true;
      this.selectionRadioButton.Location = new System.Drawing.Point(162, 27);
      this.selectionRadioButton.Name = "selectionRadioButton";
      this.selectionRadioButton.Size = new System.Drawing.Size(87, 21);
      this.selectionRadioButton.TabIndex = 8;
      this.selectionRadioButton.Text = "Selection";
      this.selectionRadioButton.UseVisualStyleBackColor = true;
      this.selectionRadioButton.CheckedChanged += new System.EventHandler(this.selectionRadioButton_CheckedChanged);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.selectionRadioButton);
      this.groupBox1.Controls.Add(this.downRadioButton);
      this.groupBox1.Controls.Add(this.upRadioButton);
      this.groupBox1.Location = new System.Drawing.Point(171, 93);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(255, 75);
      this.groupBox1.TabIndex = 9;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Direction";
      // 
      // useRegexpCheckBox
      // 
      this.useRegexpCheckBox.AutoSize = true;
      this.useRegexpCheckBox.Location = new System.Drawing.Point(12, 147);
      this.useRegexpCheckBox.Name = "useRegexpCheckBox";
      this.useRegexpCheckBox.Size = new System.Drawing.Size(102, 21);
      this.useRegexpCheckBox.TabIndex = 10;
      this.useRegexpCheckBox.Text = "Use &regexp";
      this.useRegexpCheckBox.UseVisualStyleBackColor = true;
      this.useRegexpCheckBox.CheckedChanged += new System.EventHandler(this.useRegexpCheckBox_CheckedChanged);
      // 
      // FindReplaceDlg
      // 
      this.AcceptButton = this.findButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(538, 182);
      this.Controls.Add(this.useRegexpCheckBox);
      this.Controls.Add(this.matchCaseCheckBox);
      this.Controls.Add(this.matchWholeWordsCheckBox);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.replaceAllButton);
      this.Controls.Add(this.replaceButton);
      this.Controls.Add(this.findButton);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.replaceComboBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.findComboBox);
      this.Controls.Add(this.groupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.Name = "FindReplaceDlg";
      this.ShowInTaskbar = false;
      this.Text = "Search and Replace";
      this.VisibleChanged += new System.EventHandler(this.FindReplaceDlg_VisibleChanged);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox findComboBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox replaceComboBox;
    private System.Windows.Forms.Button findButton;
    private System.Windows.Forms.Button replaceButton;
    private System.Windows.Forms.Button replaceAllButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.CheckBox matchWholeWordsCheckBox;
    private System.Windows.Forms.CheckBox matchCaseCheckBox;
    private System.Windows.Forms.RadioButton upRadioButton;
    private System.Windows.Forms.RadioButton downRadioButton;
    private System.Windows.Forms.RadioButton selectionRadioButton;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox useRegexpCheckBox;
  }
}