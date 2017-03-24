using System.Windows.Forms;

namespace GranitEditor
{
    partial class GranitXMLEditorForm
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
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GranitXMLEditorForm));
      this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.duplicateRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
      this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.IsSelectedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
      this.originatorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.beneficiaryNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.beneficiaryAccountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.currencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.executionDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.remittanceInfoDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.transactionAdapterBindingSource = new System.Windows.Forms.BindingSource(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
      this.contextMenuStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.transactionAdapterBindingSource)).BeginInit();
      this.SuspendLayout();
      // 
      // errorProvider1
      // 
      this.errorProvider1.ContainerControl = this;
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRowToolStripMenuItem,
            this.deleteRowToolStripMenuItem,
            this.duplicateRowToolStripMenuItem,
            this.toolStripSeparator9,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(182, 166);
      // 
      // addRowToolStripMenuItem
      // 
      this.addRowToolStripMenuItem.Image = global::GranitEditor.Properties.Resources.add_24;
      this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
      this.addRowToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
      this.addRowToolStripMenuItem.Text = "&Add Row";
      this.addRowToolStripMenuItem.Click += new System.EventHandler(this.AddRowToolStripMenuItem_Click);
      // 
      // deleteRowToolStripMenuItem
      // 
      this.deleteRowToolStripMenuItem.Image = global::GranitEditor.Properties.Resources.subtract_24;
      this.deleteRowToolStripMenuItem.Name = "deleteRowToolStripMenuItem";
      this.deleteRowToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
      this.deleteRowToolStripMenuItem.Text = "Delete Row(s)";
      this.deleteRowToolStripMenuItem.Click += new System.EventHandler(this.DeleteRowToolStripMenuItem_Click);
      // 
      // duplicateRowToolStripMenuItem
      // 
      this.duplicateRowToolStripMenuItem.Name = "duplicateRowToolStripMenuItem";
      this.duplicateRowToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
      this.duplicateRowToolStripMenuItem.Text = "&Duplicate Row";
      this.duplicateRowToolStripMenuItem.Click += new System.EventHandler(this.duplicateRowToolStripMenuItem_Click);
      // 
      // toolStripSeparator9
      // 
      this.toolStripSeparator9.Name = "toolStripSeparator9";
      this.toolStripSeparator9.Size = new System.Drawing.Size(178, 6);
      // 
      // cutToolStripMenuItem
      // 
      this.cutToolStripMenuItem.Image = global::GranitEditor.Properties.Resources.cut;
      this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      this.cutToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
      this.cutToolStripMenuItem.Text = "Cu&t";
      this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Image = global::GranitEditor.Properties.Resources.copy;
      this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
      this.copyToolStripMenuItem.Text = "&Copy";
      this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Image = global::GranitEditor.Properties.Resources.paste;
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
      this.pasteToolStripMenuItem.Text = "&Paste";
      this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
      // 
      // dataGridView1
      // 
      this.dataGridView1.AllowUserToAddRows = false;
      this.dataGridView1.AllowUserToOrderColumns = true;
      this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridView1.AutoGenerateColumns = false;
      this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsSelectedDataGridViewTextBoxColumn,
            this.originatorDataGridViewTextBoxColumn,
            this.beneficiaryNameDataGridViewTextBoxColumn,
            this.beneficiaryAccountDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.currencyDataGridViewTextBoxColumn,
            this.executionDateDataGridViewTextBoxColumn,
            this.remittanceInfoDataGridViewTextBoxColumn});
      this.dataGridView1.DataSource = this.transactionAdapterBindingSource;
      dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
      this.dataGridView1.Location = new System.Drawing.Point(12, 12);
      this.dataGridView1.Name = "dataGridView1";
      dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
      dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
      dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
      this.dataGridView1.RowTemplate.Height = 24;
      this.dataGridView1.Size = new System.Drawing.Size(503, 190);
      this.dataGridView1.TabIndex = 0;
      this.dataGridView1.VirtualMode = true;
      this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
      this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
      this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
      this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
      this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
      this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
      this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
      this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
      this.dataGridView1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);
      this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
      this.dataGridView1.Sorted += new System.EventHandler(this.dataGridView1_Sorted);
      this.dataGridView1.UserAddedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridView1_UserAddedNewRow);
      this.dataGridView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragDrop);
      this.dataGridView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.dataGridView1_DragEnter);
      this.dataGridView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseClick);
      // 
      // IsSelectedDataGridViewTextBoxColumn
      // 
      this.IsSelectedDataGridViewTextBoxColumn.DataPropertyName = "IsSelected";
      this.IsSelectedDataGridViewTextBoxColumn.HeaderText = "Selected";
      this.IsSelectedDataGridViewTextBoxColumn.Name = "IsSelectedDataGridViewTextBoxColumn";
      this.IsSelectedDataGridViewTextBoxColumn.ToolTipText = "Selected transactions to save.";
      // 
      // originatorDataGridViewTextBoxColumn
      // 
      this.originatorDataGridViewTextBoxColumn.DataPropertyName = "Originator";
      this.originatorDataGridViewTextBoxColumn.HeaderText = "Originator";
      this.originatorDataGridViewTextBoxColumn.Name = "originatorDataGridViewTextBoxColumn";
      this.originatorDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
      // 
      // beneficiaryNameDataGridViewTextBoxColumn
      // 
      this.beneficiaryNameDataGridViewTextBoxColumn.DataPropertyName = "BeneficiaryName";
      this.beneficiaryNameDataGridViewTextBoxColumn.HeaderText = "BeneficiaryName";
      this.beneficiaryNameDataGridViewTextBoxColumn.Name = "beneficiaryNameDataGridViewTextBoxColumn";
      // 
      // beneficiaryAccountDataGridViewTextBoxColumn
      // 
      this.beneficiaryAccountDataGridViewTextBoxColumn.DataPropertyName = "BeneficiaryAccount";
      this.beneficiaryAccountDataGridViewTextBoxColumn.HeaderText = "BeneficiaryAccount";
      this.beneficiaryAccountDataGridViewTextBoxColumn.Name = "beneficiaryAccountDataGridViewTextBoxColumn";
      // 
      // amountDataGridViewTextBoxColumn
      // 
      this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
      this.amountDataGridViewTextBoxColumn.HeaderText = "Amount";
      this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
      // 
      // currencyDataGridViewTextBoxColumn
      // 
      this.currencyDataGridViewTextBoxColumn.DataPropertyName = "Currency";
      this.currencyDataGridViewTextBoxColumn.HeaderText = "Currency";
      this.currencyDataGridViewTextBoxColumn.Name = "currencyDataGridViewTextBoxColumn";
      // 
      // executionDateDataGridViewTextBoxColumn
      // 
      this.executionDateDataGridViewTextBoxColumn.DataPropertyName = "ExecutionDate";
      this.executionDateDataGridViewTextBoxColumn.HeaderText = "ExecutionDate";
      this.executionDateDataGridViewTextBoxColumn.Name = "executionDateDataGridViewTextBoxColumn";
      // 
      // remittanceInfoDataGridViewTextBoxColumn
      // 
      this.remittanceInfoDataGridViewTextBoxColumn.DataPropertyName = "RemittanceInfo";
      this.remittanceInfoDataGridViewTextBoxColumn.HeaderText = "RemittanceInfo";
      this.remittanceInfoDataGridViewTextBoxColumn.Name = "remittanceInfoDataGridViewTextBoxColumn";
      // 
      // transactionAdapterBindingSource
      // 
      this.transactionAdapterBindingSource.DataSource = typeof(GranitEditor.TransactionAdapter);
      // 
      // GranitXMLEditorForm
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(527, 214);
      this.Controls.Add(this.dataGridView1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "GranitXMLEditorForm";
      this.Text = "GranitXmlEditor";
      this.Shown += new System.EventHandler(this.GranitXMLEditorForm_Shown);
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
      this.contextMenuStrip1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.transactionAdapterBindingSource)).EndInit();
      this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ErrorProvider errorProvider1;
    private System.Windows.Forms.BindingSource transactionAdapterBindingSource;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem duplicateRowToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
    private System.Windows.Forms.ToolStripMenuItem addRowToolStripMenuItem;
    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.DataGridViewCheckBoxColumn IsSelectedDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn originatorDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn beneficiaryNameDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn beneficiaryAccountDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn currencyDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn executionDateDataGridViewTextBoxColumn;
    private System.Windows.Forms.DataGridViewTextBoxColumn remittanceInfoDataGridViewTextBoxColumn;
    private ToolStripMenuItem deleteRowToolStripMenuItem;
    private ToolStripMenuItem copyToolStripMenuItem;
    private ToolStripMenuItem cutToolStripMenuItem;
    private ToolStripMenuItem pasteToolStripMenuItem;
  }
}

