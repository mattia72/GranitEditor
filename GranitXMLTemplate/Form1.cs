using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace GranitXMLTemplate
{
    public partial class Form1 : Form
    {

        private OpenFileDialog openFileDialog1 ;

        public Form1()
        {
            InitializeComponent();
            LoadXmlFile("fizu_adok_11.xml");
        }

        public void LoadXml_button_Click(object sender, EventArgs e)
        {
            OpenGranitXmlFile();
        }

        private void OpenGranitXmlFile()
        {
            openFileDialog1 = openFileDialog1 == null ? new OpenFileDialog() : openFileDialog1;
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string xmlFilePath = Path.GetFullPath(openFileDialog1.FileName);
                LoadXmlFile(xmlFilePath);
            }
        }

        private void LoadXmlFile(string xmlFilePath)
        {
            GranitXmlToObject xmlGen = new GranitXmlToObject(xmlFilePath);
            var list = new BindingList<TransactionAdapter>(xmlGen.HUFTransactionAdapter.Transactions);
            dataGridView1.DataSource = list;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGranitXmlFile();
        }

        private void alignTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (alignTableToolStripMenuItem.Checked)
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            else
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;

            alignTableToolStripMenuItem.Checked = !alignTableToolStripMenuItem.Checked;
        }
    }
}
