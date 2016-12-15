using Be.Timvw.Framework.ComponentModel;
using System;
using System.IO;
using System.Windows.Forms;

namespace GranitXMLEditor
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

            xmlToObject.LoadObjectFromFile(xmlFilePath);
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

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private DataGridViewCellValidatingEventArgs cellErrorLocation;
        private string cellErrorText;

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView1.CurrentCell.Tag == e.FormattedValue)
            {
                e.Cancel = true;    //Cancel changes of current cell
                return;
            }

            //string headerText = dataGridView1.Columns[e.ColumnIndex].HeaderText;

            //string value = "";


            //switch (headerText)
            //{
            //    case "Originator":
            //        value = (string)e.FormattedValue;
            //        if (value.Length != 16 && value.Length != 24)
            //        {
            //            dataGridView1.CurrentCell.ToolTipText = "Invalid Value";
            //            dataGridView1.BackgroundColor = System.Drawing.Color.LightPink;
            //            e.Cancel = true;
            //        }
            //        break;
            //    default:
            //        e.Cancel = false;
            //        break;
            //}
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
            cellErrorLocation = null;
            cellErrorText = null;
            //dataGridView1.BackgroundColor = BackColor;
        }

        //private void AnnotateCell(string errorMessage, DataGridViewCellValidatingEventArgs editEvent)
        //{
        //    cellErrorLocation = editEvent;
        //    cellErrorText = errorMessage;
        //}

        private void dataGridView1_CellErrorTextNeeded(object sender, DataGridViewCellErrorTextNeededEventArgs e)
        {

        }

        private void dataGridView1_RowErrorTextNeeded(object sender, DataGridViewRowErrorTextNeededEventArgs e)
        {

        }
    }
}
