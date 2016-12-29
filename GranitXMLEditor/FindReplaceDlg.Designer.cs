namespace GranitXMLEditor
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
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.comboBox2 = new System.Windows.Forms.ComboBox();
      this.findButton = new System.Windows.Forms.Button();
      this.replaceButton = new System.Windows.Forms.Button();
      this.replaceAllButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.matchWholeWordsCheckBox = new System.Windows.Forms.CheckBox();
      this.matchCaseCheckBox = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // comboBox1
      // 
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(111, 12);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(315, 24);
      this.comboBox1.TabIndex = 0;
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
      // comboBox2
      // 
      this.comboBox2.FormattingEnabled = true;
      this.comboBox2.Location = new System.Drawing.Point(111, 42);
      this.comboBox2.Name = "comboBox2";
      this.comboBox2.Size = new System.Drawing.Size(315, 24);
      this.comboBox2.TabIndex = 2;
      // 
      // findButton
      // 
      this.findButton.Location = new System.Drawing.Point(432, 12);
      this.findButton.Name = "findButton";
      this.findButton.Size = new System.Drawing.Size(94, 28);
      this.findButton.TabIndex = 4;
      this.findButton.Text = "&Find";
      this.findButton.UseVisualStyleBackColor = true;
      // 
      // replaceButton
      // 
      this.replaceButton.Location = new System.Drawing.Point(432, 41);
      this.replaceButton.Name = "replaceButton";
      this.replaceButton.Size = new System.Drawing.Size(94, 28);
      this.replaceButton.TabIndex = 4;
      this.replaceButton.Text = "&Replace";
      this.replaceButton.UseVisualStyleBackColor = true;
      // 
      // replaceAllButton
      // 
      this.replaceAllButton.Location = new System.Drawing.Point(432, 70);
      this.replaceAllButton.Name = "replaceAllButton";
      this.replaceAllButton.Size = new System.Drawing.Size(94, 28);
      this.replaceAllButton.TabIndex = 4;
      this.replaceAllButton.Text = "Replace &All";
      this.replaceAllButton.UseVisualStyleBackColor = true;
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(432, 113);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(94, 27);
      this.cancelButton.TabIndex = 5;
      this.cancelButton.Text = "&Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // matchWholeWordsCheckBox
      // 
      this.matchWholeWordsCheckBox.AutoSize = true;
      this.matchWholeWordsCheckBox.Location = new System.Drawing.Point(16, 88);
      this.matchWholeWordsCheckBox.Name = "matchWholeWordsCheckBox";
      this.matchWholeWordsCheckBox.Size = new System.Drawing.Size(149, 21);
      this.matchWholeWordsCheckBox.TabIndex = 6;
      this.matchWholeWordsCheckBox.Text = "Match &whole words";
      this.matchWholeWordsCheckBox.UseVisualStyleBackColor = true;
      // 
      // matchCaseCheckBox
      // 
      this.matchCaseCheckBox.AutoSize = true;
      this.matchCaseCheckBox.Location = new System.Drawing.Point(16, 115);
      this.matchCaseCheckBox.Name = "matchCaseCheckBox";
      this.matchCaseCheckBox.Size = new System.Drawing.Size(102, 21);
      this.matchCaseCheckBox.TabIndex = 6;
      this.matchCaseCheckBox.Text = "Match &case";
      this.matchCaseCheckBox.UseVisualStyleBackColor = true;
      // 
      // FindReplaceDlg
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(538, 152);
      this.Controls.Add(this.matchCaseCheckBox);
      this.Controls.Add(this.matchWholeWordsCheckBox);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.replaceAllButton);
      this.Controls.Add(this.replaceButton);
      this.Controls.Add(this.findButton);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.comboBox2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboBox1);
      this.MaximizeBox = false;
      this.Name = "FindReplaceDlg";
      this.ShowIcon = false;
      this.Text = "Find and Replace";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox comboBox2;
    private System.Windows.Forms.Button findButton;
    private System.Windows.Forms.Button replaceButton;
    private System.Windows.Forms.Button replaceAllButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.CheckBox matchWholeWordsCheckBox;
    private System.Windows.Forms.CheckBox matchCaseCheckBox;
  }
}