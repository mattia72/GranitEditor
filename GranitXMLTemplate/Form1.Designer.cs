namespace GranitXMLTemplate
{
    partial class Form1
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
            this.LoadXml_button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.transactionAdapterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.originatorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.beneficiaryNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.beneficiaryAccountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.amountDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currencyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.executionDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionAdapterBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadXml_button
            // 
            this.LoadXml_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadXml_button.Location = new System.Drawing.Point(601, 397);
            this.LoadXml_button.Name = "LoadXml_button";
            this.LoadXml_button.Size = new System.Drawing.Size(75, 23);
            this.LoadXml_button.TabIndex = 0;
            this.LoadXml_button.Text = "Load Xml";
            this.LoadXml_button.UseVisualStyleBackColor = true;
            this.LoadXml_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 379);
            this.panel1.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.originatorDataGridViewTextBoxColumn,
            this.beneficiaryNameDataGridViewTextBoxColumn,
            this.beneficiaryAccountDataGridViewTextBoxColumn,
            this.amountDataGridViewTextBoxColumn,
            this.currencyDataGridViewTextBoxColumn,
            this.executionDateDataGridViewTextBoxColumn,
            this.commentDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.transactionAdapterBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(664, 379);
            this.dataGridView1.TabIndex = 0;
            // 
            // transactionAdapterBindingSource
            // 
            this.transactionAdapterBindingSource.DataSource = typeof(GranitXMLTemplate.TransactionAdapter);
            // 
            // originatorDataGridViewTextBoxColumn
            // 
            this.originatorDataGridViewTextBoxColumn.DataPropertyName = "Originator";
            this.originatorDataGridViewTextBoxColumn.HeaderText = "Originator";
            this.originatorDataGridViewTextBoxColumn.Name = "originatorDataGridViewTextBoxColumn";
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
            // commentDataGridViewTextBoxColumn
            // 
            this.commentDataGridViewTextBoxColumn.DataPropertyName = "Comment";
            this.commentDataGridViewTextBoxColumn.HeaderText = "Comment";
            this.commentDataGridViewTextBoxColumn.Name = "commentDataGridViewTextBoxColumn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 432);
            this.Controls.Add(this.LoadXml_button);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transactionAdapterBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button LoadXml_button;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn originatorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn beneficiaryNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn beneficiaryAccountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currencyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn executionDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource transactionAdapterBindingSource;
    }
}

