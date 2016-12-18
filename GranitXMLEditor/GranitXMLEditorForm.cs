using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using Be.Timvw.Framework.ComponentModel;
using GranitXMLEditor.Properties;
using System.Xml.Linq;
using System.Xml;

namespace GranitXMLEditor
{
  public partial class GranitXMLEditorForm : Form
  {

    private GranitXmlToObjectBinder xmlToObject;
    private OpenFileDialog openFileDialog1;
    private SaveFileDialog saveFileDialog1;

    public GranitXMLEditorForm()
    {
      InitializeComponent();
      LoadXmlFile("default.xml");
      ApplySettings();
    }

    private void ApplySettings()
    {
      if (!string.IsNullOrEmpty(Settings.Default.SortedColumn))
      {
        DataGridViewColumn col = FindColumnByHeaderText(Settings.Default.SortedColumn);
        dataGridView1.Sort(col, Settings.Default.SortOrder == "Ascending" ? ListSortDirection.Ascending : ListSortDirection.Descending);
        col.HeaderCell.SortGlyphDirection = Settings.Default.SortOrder == "Ascending" ? SortOrder.Ascending : SortOrder.Descending;
      }
      if (!string.IsNullOrEmpty(Settings.Default.AlignTable))
      {
        dataGridView1.AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)Enum.Parse(typeof(DataGridViewAutoSizeColumnsMode),
            Settings.Default.AlignTable);
      }
    }

    private DataGridViewColumn FindColumnByHeaderText(string headerText)
    {
      foreach (DataGridViewColumn col in dataGridView1.Columns)
      {
        if (col.HeaderText == headerText)
        {
          return col;
        }
      }
      return null;
    }

    public void LoadXml_button_Click(object sender, EventArgs e)
    {
      OpenGranitXmlFile();
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SaveGranitXmlFile();
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

    private void SaveGranitXmlFile()
    {
      if (xmlToObject != null)
      {
        saveFileDialog1 = saveFileDialog1 == null ? new SaveFileDialog() : saveFileDialog1;
        saveFileDialog1.InitialDirectory = Application.StartupPath;
        saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
        saveFileDialog1.FilterIndex = 2;
        saveFileDialog1.RestoreDirectory = true;

        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
          string xmlFilePath = Path.GetFullPath(saveFileDialog1.FileName);
          xmlToObject.SaveToFile(xmlFilePath);
        }
      }
    }

    private void LoadXmlFile(string xmlFilePath)
    {
      xmlToObject = new GranitXmlToObjectBinder(xmlFilePath);
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
      {
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
      }
      else
      {
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
      }
      Settings.Default.AlignTable = dataGridView1.AutoSizeColumnsMode.ToString();
      alignTableToolStripMenuItem.Checked = !alignTableToolStripMenuItem.Checked;
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var aboutBox = new PoorMensAboutBox.AboutBox();
      aboutBox.ShowDialog();
    }

    private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {

    }

    //private DataGridViewCellValidatingEventArgs cellErrorLocation;
    //private string cellErrorText;

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
      //cellErrorLocation = null;
      //cellErrorText = null;
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

    private void dataGridView1_Sorted(object sender, EventArgs e)
    {
      Settings.Default.SortedColumn = dataGridView1.SortedColumn.HeaderText;
      Settings.Default.SortOrder = dataGridView1.SortOrder.ToString();
      Settings.Default.Save();

      xmlToObject.Sort(dataGridView1.SortedColumn.HeaderText, dataGridView1.SortOrder);
    }
  }
}
