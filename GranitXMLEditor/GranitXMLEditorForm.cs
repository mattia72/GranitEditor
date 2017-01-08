﻿using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using Be.Timvw.Framework.ComponentModel;
using GranitXMLEditor.Properties;
using System.Diagnostics;
using System.Data;
using System.Linq;

namespace GranitXMLEditor
{
  public partial class GranitXMLEditorForm : Form
  {

    private GranitXmlToObjectBinder _xmlToObject;
    private OpenFileDialog _openFileDialog1;
    private SaveFileDialog _saveFileDialog1;
    private AboutBox _aboutBox;
    private FindReplaceDlg _findReplaceDlg;
    private string _lastOpenedFilePath;
    private bool _docHasPendingChanges=false;

    public GranitXMLEditorForm()
    {
      InitializeComponent();
      OpenLastOPenedFileIfExists();
      ApplySettings();
      _docHasPendingChanges = false;
    }

    private void OpenLastOPenedFileIfExists()
    {
      LastOpenedFilePath = Settings.Default.LastOpenedFilePath;
      if (LastOpenedFilePath != string.Empty && File.Exists(LastOpenedFilePath))
        LoadXmlFile(LastOpenedFilePath);
      else
        OpenNewDocument();
    }

    public string LastOpenedFilePath
    {
      get { return _lastOpenedFilePath; }
      set
      {
        _lastOpenedFilePath = value;
        Text = Application.ProductName + " - " + Path.GetFileName(_lastOpenedFilePath);
       
      }
    }

    private void ApplySettings()
    {
      if (!string.IsNullOrEmpty(Settings.Default.SortedColumn))
      {
        DataGridViewColumn col = FindColumnByHeaderText(Settings.Default.SortedColumn);
        dataGridView1.Sort(col, Settings.Default.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
        col.HeaderCell.SortGlyphDirection = Settings.Default.SortOrder == SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
      }
      if (Settings.Default.AlignTable != DataGridViewAutoSizeColumnsMode.None)
      {
        dataGridView1.AutoSizeColumnsMode = Settings.Default.AlignTable;
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

    private void OpenGranitXmlFile()
    {
      _openFileDialog1 = _openFileDialog1 == null ? new OpenFileDialog() : _openFileDialog1;
      _openFileDialog1.InitialDirectory = Application.StartupPath;
      _openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
      _openFileDialog1.FilterIndex = 2;
      _openFileDialog1.RestoreDirectory = true;

      if (_openFileDialog1.ShowDialog() == DialogResult.OK)
      {
        LastOpenedFilePath = Path.GetFullPath(_openFileDialog1.FileName);
        LoadXmlFile(LastOpenedFilePath);
      }
    }

    private string GetFileNameToSaveByOpeningSaveFileDialog()
    {
      string filename = null;
      if (_xmlToObject != null)
      {
        _saveFileDialog1 = _saveFileDialog1 == null ? new SaveFileDialog() : _saveFileDialog1;
        _saveFileDialog1.InitialDirectory = Application.StartupPath;
        _saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
        _saveFileDialog1.FilterIndex = 2;
        _saveFileDialog1.RestoreDirectory = true;

        if (_saveFileDialog1.ShowDialog() == DialogResult.OK)
        {
          filename = _saveFileDialog1.FileName;
        }
      }
      return filename;
    }

    private void SaveToFile(string fileName)
    {
      string xmlFilePath = Path.GetFullPath(fileName);
      _xmlToObject.SaveToFile(xmlFilePath);
      LastOpenedFilePath = xmlFilePath;
      _docHasPendingChanges = false;
    }

    private void LoadXmlFile(string xmlFilePath)
    {
      _xmlToObject = new GranitXmlToObjectBinder(xmlFilePath);
      var list = new SortableBindingList<TransactionAdapter>(_xmlToObject.HUFTransactionAdapter.Transactions);
      dataGridView1.DataSource = list;
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
      alignTableToolStripMenuItem.Checked = !alignTableToolStripMenuItem.Checked;

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
      _xmlToObject.Sort(dataGridView1.SortedColumn.HeaderText, dataGridView1.SortOrder);
    }

    private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      _docHasPendingChanges = true;
      //if (dataGridView1.CurrentCell == null) return;
      //if ((bool)dataGridView1.CurrentCell.Value == true)
      //  Debug.WriteLine("Checkbox value true.");
      //else if ((bool)dataGridView1.CurrentCell.Value == false)
      //  Debug.WriteLine("Checkbox value false.");
      //else Debug.WriteLine(dataGridView1.CurrentCell.Value.ToString());
    }

    private void openToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      OpenGranitXmlFile();
    }

    private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      string f = GetFileNameToSaveByOpeningSaveFileDialog();
      SaveToFile(f);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Settings.Default.SortedColumn = dataGridView1.SortedColumn != null ? dataGridView1.SortedColumn.HeaderText : "";
      Settings.Default.SortOrder = dataGridView1.SortOrder;
      Settings.Default.AlignTable = dataGridView1.AutoSizeColumnsMode;
      if (LastOpenedFilePath == string.Empty)
        e.Cancel = AskAndSaveFile() != true;
      Settings.Default.LastOpenedFilePath = LastOpenedFilePath;
      Settings.Default.Save();

      base.OnClosing(e);
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CreateFindDialog();

      if (!_findReplaceDlg.Visible)
      {
        if (dataGridView1.SelectedCells.Count > 1)
          _findReplaceDlg.IsSelectionChecked = true;
        _findReplaceDlg.Show(this);
        _findReplaceDlg.BringToFront();
      }
      else
      {
        _findReplaceDlg.Hide();
      }
    }

    private void CreateFindDialog()
    {
      if (_findReplaceDlg == null)
        _findReplaceDlg = new FindReplaceDlg(dataGridView1);

      if (_findReplaceDlg.IsDisposed)
        _findReplaceDlg = new FindReplaceDlg(dataGridView1);
    }

    private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (_aboutBox == null)
      {
        _aboutBox = new AboutBox();
      }

      if (_aboutBox.IsDisposed)
        _aboutBox = new AboutBox();

      if (!_aboutBox.Visible)
      {
        _aboutBox.Show();
        _aboutBox.BringToFront();
      }
      else
      {
        _aboutBox.Hide();
      }
    }

    private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
    {
      var bindingList = ((SortableBindingList<TransactionAdapter>)dataGridView1.DataSource);
      //delete last (non working adapter) and create a new...
      bindingList.RemoveAt(bindingList.Count - 1);
      bindingList.Add(_xmlToObject.AddNewTransactionRow());
      dataGridView1.CurrentCell = e.Row.Cells[1]; //TODO: change index to the name of the cell
      dataGridView1.BeginEdit(false);
    }


    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      CreateFindDialog();
      _findReplaceDlg.IsFirstInitNecessary = true;
    }

    private int? _transactionIdTodelete = null;

    private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
      _transactionIdTodelete = ((TransactionAdapter)e.Row.DataBoundItem).TransactionId;

    }
    private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
    {
      if(_transactionIdTodelete != null)
        _xmlToObject.RemoveTransactionRowById((int)_transactionIdTodelete);
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_docHasPendingChanges)
        AskAndSaveFile();

      OpenNewDocument();
    }

    private void OpenNewDocument()
    {
      _xmlToObject = new GranitXmlToObjectBinder();
      var list = new SortableBindingList<TransactionAdapter>(_xmlToObject.HUFTransactionAdapter.Transactions);
      dataGridView1.DataSource = list;
      LastOpenedFilePath = string.Empty;
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (LastOpenedFilePath != string.Empty)
      {
        AskAndSaveFile();
      }
      else
      {
        LastOpenedFilePath = GetFileNameToSaveByOpeningSaveFileDialog();
        SaveToFile(LastOpenedFilePath);
      }
    }

    private bool AskAndSaveFile()
    {
      var answ = MessageBox.Show(
        string.Format(Resources.DoYouWantToSave, Path.GetFileName(LastOpenedFilePath)),
        Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

      if (answ == DialogResult.Yes)
      {
        if (LastOpenedFilePath != string.Empty)
        {
          SaveToFile(LastOpenedFilePath);
        }
        else
        {
          string f = GetFileNameToSaveByOpeningSaveFileDialog();
          if (f != null)
            SaveToFile(f);
          else
            return false;
        }
      }
      return true;
    }

    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      dataGridView1.EndEdit();
      //_xmlToObject.HUFTransactionAdapter.Transactions.Where(t => !t.IsActive).All(x => { return x.IsActive = true;});
      dataGridView1.SelectAll();
    }
  }
}
