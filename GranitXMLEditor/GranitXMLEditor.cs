using Be.Timvw.Framework.ComponentModel;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace GranitXMLTemplate
{
    public partial class GranitXMLEditor : Form
    {

        private GranitXmlToObject xmlToObject;
        private OpenFileDialog openFileDialog1 ;

        public GranitXMLEditor()
        {
            InitializeComponent();
            LoadXmlFile("default.xml");
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
            if (xmlToObject == null)
                xmlToObject = new GranitXmlToObject(xmlFilePath);

            xmlToObject.ReadFromFile(xmlFilePath);
            var list = new SortableBindingList<TransactionAdapter>(xmlToObject.HUFTransactionAdapter.Transactions);
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

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (xmlToObject != null)
            {
                var saveDlg = new SaveFileDialog();
                if (saveDlg.ShowDialog() == DialogResult.OK)
                {
                    string xmlFilePath = Path.GetFullPath(saveDlg.FileName);
                    xmlToObject.SaveToFile(xmlFilePath);

                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutBox = new PoorMensAboutBox.AboutBox();
            aboutBox.ShowDialog();
        }
    }
}
