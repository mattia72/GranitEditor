using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using GranitXMLEditor.Properties;
using System.Diagnostics;
using System.Linq;
using System.Xml.Schema;

namespace GranitXMLEditor
{
  public partial class GranitXMLEditorForm : Form
  {
    private GranitXmlToAdapterBinder _xmlToObjectBinder;
    private OpenFileDialog _openFileDialog;
    private SaveFileDialog _saveFileDialog;
    private AboutBox _aboutBox;
    private FindReplaceDlg _findReplaceDlg;
    private string _lastOpenedFilePath;
    private bool _docHasPendingChanges=false;
    private MruStripMenu _mruMenu;
    private EnumStripMenu<DataGridViewAutoSizeColumnsMode> _autoSizeMenu;
    private GranitDataGridViewCellValidator _cellVallidator;
    private GranitDataGridViewContextMenuHandler _contextMenuHandler;
    private SortableBindingList<TransactionAdapter> _bindingList;

    //private UndoRedoHistory<TransactionPool> _history;

    public GranitXMLEditorForm()
    {
      InitializeComponent();
      _mruMenu = new MruStripMenu(recentFilesToolStripMenuItem, mruMenu_Clicked, 10);
      _autoSizeMenu = new EnumStripMenu<DataGridViewAutoSizeColumnsMode>(alignTableToolStripMenuItem, autoSizeMenu_Clicked);
      OpenLastOpenedFileIfExists();
      //_history = new UndoRedoHistory<TransactionPool>(_transactionPool);
      _cellVallidator = new GranitDataGridViewCellValidator(dataGridView1);
      _contextMenuHandler = new GranitDataGridViewContextMenuHandler(dataGridView1, contextMenuStrip1, _xmlToObjectBinder);
      SetTextResources();
      ApplySettings();
      //after sorting has to be reset...
      DocHasPendingChanges = false;
    }

    private void autoSizeMenu_Clicked(DataGridViewAutoSizeColumnsMode mode)
    {
      dataGridView1.AutoSizeColumnsMode = mode == 0 ? DataGridViewAutoSizeColumnsMode.None : mode;
    }

    private void SetTextResources()
    {
      IsSelectedDataGridViewTextBoxColumn.HeaderText = Resources.IsActiveHeaderText;
      originatorDataGridViewTextBoxColumn.HeaderText = Resources.OriginatorHeaderText;
      beneficiaryNameDataGridViewTextBoxColumn.HeaderText = Resources.BeneficiaryNameHeader;
      beneficiaryAccountDataGridViewTextBoxColumn.HeaderText = Resources.BeneficiaryAccountHeader;
      amountDataGridViewTextBoxColumn.HeaderText = Resources.AmountHeaderText;
      currencyDataGridViewTextBoxColumn.HeaderText = Resources.CurrencyHeader;
      executionDateDataGridViewTextBoxColumn.HeaderText = Resources.RequestedExecutionDateHeaderText;
      remittanceInfoDataGridViewTextBoxColumn.HeaderText = Resources.RemittanceInfoHeaderText;
    }

    private void mruMenu_Clicked(int number, string filename)
    {
      if (File.Exists(filename))
      {
        LoadDocument(filename);
        _mruMenu.SetFirstFile(number);
      }
      else
      {
        MessageBox.Show(
        string.Format(Resources.FileDoesntExists, Path.GetFileName(filename)),
        Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        _mruMenu.RemoveFile(number);
      }
    }

    private void OpenLastOpenedFileIfExists()
    {
      LastOpenedFilePath = Settings.Default.LastOpenedFilePath;
      if (LastOpenedFilePath != string.Empty && File.Exists(LastOpenedFilePath))
        LoadDocument(LastOpenedFilePath);
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
        if(!string.IsNullOrEmpty(_lastOpenedFilePath))
          _mruMenu.AddFile(_lastOpenedFilePath);
      }
    }

    public bool DocHasPendingChanges
    {
      get
      {
        return _docHasPendingChanges;
      }

      set
      {
        _docHasPendingChanges = value;
        saveToolStripButton.Enabled = _docHasPendingChanges;
        saveToolStripMenuItem.Enabled = _docHasPendingChanges;
        if (_docHasPendingChanges)
        {
          undoToolStripButton.Enabled = _xmlToObjectBinder.History.CanUndo;
          redoToolStripButton.Enabled = _xmlToObjectBinder.History.CanRedo;
        }
        else
        {
          undoToolStripButton.Enabled = redoToolStripButton.Enabled = false;
        }
      }
    }

    private void ApplySettings()
    {
      //if (!string.IsNullOrEmpty(Settings.Default.SortedColumn))
      //{
      //  DataGridViewColumn sortedColumn = FindColumnByHeaderText(Settings.Default.SortedColumn);
      //  dataGridView1.Sort(sortedColumn,
      //    Settings.Default.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
      //  sortedColumn.HeaderCell.SortGlyphDirection = 
      //    Settings.Default.SortOrder == SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
      //}
      if (Settings.Default.AlignTable != 0)
      {
        dataGridView1.AutoSizeColumnsMode = Settings.Default.AlignTable;
        _autoSizeMenu.SetCheckedByMode(dataGridView1.AutoSizeColumnsMode);
      }
      _mruMenu.MaxShortenPathLength = Settings.Default.MruListItemLength;
      if (Settings.Default.RecentFileList != null)
        foreach (string item in Settings.Default.RecentFileList)
        {
          _mruMenu.AddFile(item);
        }
    }

    /// <summary>
    /// If the user press delete on a row we should delete the DataBoundItem
    /// </summary>
    private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
      long? transactionIdTodelete = null;
      if (dataGridView1.Rows.Count > e.RowIndex)
      {
         transactionIdTodelete = _xmlToObjectBinder.HUFTransactionsAdapter.TransactionAdapters.
          Where(x => GetRow(x.TransactionId) == null).FirstOrDefault()?.TransactionId;
      }

      if (transactionIdTodelete != null)
      {
        _xmlToObjectBinder.RemoveTransactionRowById((long)transactionIdTodelete, e.RowIndex);
        DocHasPendingChanges = true;
      }
    }

    private DataGridViewRow GetRow(long transactionId)
    {
      foreach (DataGridViewRow row in dataGridView1.Rows)
      {
        if(row.DataBoundItem != null && 
          (row.DataBoundItem as TransactionAdapter).TransactionId == transactionId)
          return row;
      }
      return null;
    }

    private void SaveSettings()
    {
      //Settings.Default.SortedColumn = dataGridView1.SortedColumn != null ? dataGridView1.SortedColumn.HeaderText : "";
      //Settings.Default.SortOrder = dataGridView1.SortOrder;
      Settings.Default.AlignTable = dataGridView1.AutoSizeColumnsMode;
      Settings.Default.LastOpenedFilePath = LastOpenedFilePath;
      if (Settings.Default.RecentFileList != null)
        Settings.Default.RecentFileList.Clear();
      else
        Settings.Default.RecentFileList = new System.Collections.Specialized.StringCollection();
      var files = _mruMenu.GetFiles();
      Settings.Default.RecentFileList.AddRange(files);
      Settings.Default.Save();
    }

    private DataGridViewColumn FindColumnByHeaderText(string headerText)
    {
      foreach (DataGridViewColumn col in dataGridView1.Columns)
      {
        if (col.HeaderText == headerText)
          return col;
      }
      return null;
    }

    private void OpenGranitXmlFile()
    {
      _openFileDialog = _openFileDialog == null ? new OpenFileDialog() : _openFileDialog;
      _openFileDialog.InitialDirectory = Application.StartupPath;
      _openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
      _openFileDialog.FilterIndex = 2;
      _openFileDialog.RestoreDirectory = true;

      if (_openFileDialog.ShowDialog() == DialogResult.OK)
        LoadDocument(_openFileDialog.FileName);
    }

    private string GetFileNameToSaveByOpeningSaveFileDialog()
    {
      string filename = null;
      if (_xmlToObjectBinder != null)
      {
        _saveFileDialog = _saveFileDialog == null ? new SaveFileDialog() : _saveFileDialog;
        _saveFileDialog.InitialDirectory = Application.StartupPath;
        _saveFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
        _saveFileDialog.FilterIndex = 2;
        _saveFileDialog.RestoreDirectory = true;

        if (_saveFileDialog.ShowDialog() == DialogResult.OK)
          filename = _saveFileDialog.FileName;
      }
      return filename;
    }

    private void SaveDocument(string fileName)
    {
      string xmlFilePath = Path.GetFullPath(fileName);
      _xmlToObjectBinder.SaveToFile(xmlFilePath);
      LastOpenedFilePath = xmlFilePath;
      DocHasPendingChanges = false;
    }

    private void LoadDocument(string xmlFilePath)
    {
      _xmlToObjectBinder = new GranitXmlToAdapterBinder(xmlFilePath, true);
      if (_xmlToObjectBinder.XmlValidationErrorOccured)
      {
        XmlSchemaException e = _xmlToObjectBinder.ValidationEventArgs.Exception;

        MessageBox.Show(string.Format(
          Resources.ValidationErrorMsg,
          _xmlToObjectBinder.ValidationEventArgs.Severity == XmlSeverityType.Error ? Resources.ErrorText : Resources.WarningText,
          xmlFilePath + " (" + e.LineNumber + ", " + e.LinePosition + ")",
          _xmlToObjectBinder.ValidationEventArgs.Message),
          Application.ProductName,
          MessageBoxButtons.OK,
          MessageBoxIcon.Error);

        _xmlToObjectBinder = new GranitXmlToAdapterBinder();
        LastOpenedFilePath = string.Empty;
      }
      else
      {
        LastOpenedFilePath = xmlFilePath;
      }

      RebindBindingList();
      DocHasPendingChanges = false;
    }

    private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (dataGridView1.CurrentCell.Tag == e.FormattedValue)
      {
        e.Cancel = true;    //Cancel changes of current cell
        return;
      }

      string propertyName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

      _cellVallidator.Validate(ref e, propertyName);

    }

    //Transaction beginEditTrans=null;
    private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      Debug.WriteLine("CellBeginEdit called on row: {0} col: {1}", e.RowIndex, e.ColumnIndex);
      //beginEditTrans = ((TransactionAdapter)dataGridView1.Rows[e.RowIndex].DataBoundItem).Transaction;
      //if ( e.RowIndex != -1 ) // not on first load 
    }

    private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      Debug.WriteLine("CellEndEdit called on row: {0} col: {1}", e.RowIndex, e.ColumnIndex);
      if ( e.RowIndex != -1 ) // not on first load 
        dataGridView1.Rows[e.RowIndex].ErrorText = string.Empty;
    }

    private void dataGridView1_Sorted(object sender, EventArgs e)
    {
      _xmlToObjectBinder.Sort(dataGridView1.SortedColumn.DataPropertyName, dataGridView1.SortOrder);
      DocHasPendingChanges = true;
    }

    private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      Debug.WriteLine("CellValueChanged called on row: {0} col: {1}", e.RowIndex, e.ColumnIndex);
      if ( e.RowIndex != -1 ) // not on first load 
        DocHasPendingChanges = true;
    }

    private void openToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      OpenGranitXmlFile();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Debug.WriteLine("OnClosing called. docHasPendingChanges: {0}", DocHasPendingChanges);

      if (DocHasPendingChanges || LastOpenedFilePath == string.Empty)
        e.Cancel = AskAndSaveFile(MessageBoxButtons.YesNoCancel) == DialogResult.Cancel;

      if(!e.Cancel)
        SaveSettings();

      base.OnClosing(e);
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ShowFindAndReplaceDlg();
    }

    private void ShowFindAndReplaceDlg()
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
      About();
    }

    private void About()
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

    private void dataGridView1_UserAddedNewRow(object sender, DataGridViewRowEventArgs e)
    {
      var bindingList = ((SortableBindingList<TransactionAdapter>)dataGridView1.DataSource);
      //Commit the first character
      dataGridView1.EndEdit();
      //change binding list item to our Adapter
      var ta = bindingList[bindingList.Count - 1];
      bindingList[bindingList.Count - 1] = _xmlToObjectBinder.AddTransactionRow(ta);
      Debug.WriteLine("User Added Adapter: " + (ta != null ? ta.ToString() : "null"));
      dataGridView1.BeginEdit(false);
    }

    private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      Debug.WriteLine(e.RowCount + " rows added at index: " + e.RowIndex);
      TransactionAdapter ta = (TransactionAdapter)dataGridView1.Rows[e.RowIndex].DataBoundItem;
      Debug.WriteLine("Adapter: " + (ta != null ? ta.ToString() : "null"));
      allStatusLabel.Text = "Count: " + _xmlToObjectBinder.TransactionCount; 
      allAmountStatus.Text = "Sum: " + _xmlToObjectBinder.SumAmount;
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      CreateFindDialog();
      _findReplaceDlg.IsFirstInitNecessary = true;
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      New();
    }

    private void New()
    {
      DialogResult answere = DialogResult.OK; ;
      if (DocHasPendingChanges)
        answere = AskAndSaveFile(MessageBoxButtons.YesNoCancel);

      if (answere != DialogResult.Cancel)
        OpenNewDocument();
    }

    public void RebindBindingList()
    {
      if(dataGridView1.IsCurrentCellInEditMode)
        dataGridView1.CancelEdit();
      _bindingList = new SortableBindingList<TransactionAdapter>(_xmlToObjectBinder.HUFTransactionsAdapter.TransactionAdapters);
      dataGridView1.DataSource = _bindingList;
      if (_bindingList.RaiseListChangedEvents)
        _bindingList.ListChanged += _bindingList_ListChanged;
    }

    private void _bindingList_ListChanged(object sender, ListChangedEventArgs e)
    {
      Debug.WriteLine("BindingList changed " + e.ListChangedType + " " + e.PropertyDescriptor);
    }

    private void OpenNewDocument()
    {
      _xmlToObjectBinder = new GranitXmlToAdapterBinder();
      LastOpenedFilePath = string.Empty;
      RebindBindingList();
      DocHasPendingChanges = true;
    }

    /// <summary>
    /// Asks if parameter != null, else saves without any question
    /// </summary>
    /// <param name="buttons"></param>
    /// <returns></returns>
    private DialogResult AskAndSaveFile(MessageBoxButtons? buttons = null)
    {
      DialogResult answ = DialogResult.Yes;
      if (buttons != null)
      {
        answ = MessageBox.Show(
          string.Format(Resources.DoYouWantToSave, Path.GetFileName(LastOpenedFilePath)),
          Application.ProductName, (MessageBoxButtons)buttons, MessageBoxIcon.Question);
      }

      if (answ == DialogResult.Yes)
      {
        if (LastOpenedFilePath != string.Empty)
        {
          SaveDocument(LastOpenedFilePath);
        }
        else
        {
          string f = GetFileNameToSaveByOpeningSaveFileDialog();
          if (f != null)
            SaveDocument(f);
          else
            return DialogResult.Cancel;
        }
      }
      return answ;
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      AskAndSaveFile();
    }

    private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      string f = GetFileNameToSaveByOpeningSaveFileDialog();
      SaveDocument(f);
    }


    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      dataGridView1.EndEdit();
      //_xmlToObject.HUFTransactionAdapter.Transactions.Where(t => !t.IsActive).All(x => { return x.IsActive = true;});
      dataGridView1.SelectAll();
    }

    private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      GranitDataGridViewCellFormatter.Format(dataGridView1, ref e);
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      //TODO text resource
      EnableAllcontextMenuItem(false);
      if (dataGridView1.SelectedRows.Count > 1)
      {
        deleteRowToolStripMenuItem.Text = "Delete Selected";
        deleteRowToolStripMenuItem.Enabled = true;
      }
      else
      {
        if(!dataGridView1.IsCurrentCellInEditMode)
          EnableAllcontextMenuItem(true);
        deleteRowToolStripMenuItem.Text = "Delete";
      }
    }

    private void EnableAllcontextMenuItem(bool v)
    {
      foreach (ToolStripItem item in contextMenuStrip1.Items)
      {
        if (item is ToolStripMenuItem)
          item.Enabled = v;
      }
    }

    private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
    {
      _contextMenuHandler.grid_MouseClick(sender, e);
    }

    private void deleteRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      _contextMenuHandler.grid_DeleteSelectedRows(sender, e);
    }

    private void duplicateRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      _contextMenuHandler.grid_DuplicateRow(sender, e);
    }

    private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
    {
      _contextMenuHandler.grid_DeleteSelectedRows(sender, e);
    }

    private void newToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      _contextMenuHandler.grid_AddNewRow(sender, e);
    }

    private void undoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Undo();
    }

    private void Undo()
    {
      if (_xmlToObjectBinder.History.CanUndo)
      {
        _xmlToObjectBinder.History_Undo();
        RebindBindingList();
      }
      undoToolStripButton.Enabled = _xmlToObjectBinder.History.CanUndo;
      redoToolStripButton.Enabled = _xmlToObjectBinder.History.CanRedo;
    }

    private void redoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Redo();
    }

    private void Redo()
    {
      if (_xmlToObjectBinder.History.CanRedo)
      {
        _xmlToObjectBinder.History_Redo();
        RebindBindingList();
      }
      undoToolStripButton.Enabled = _xmlToObjectBinder.History.CanUndo;
      redoToolStripButton.Enabled = _xmlToObjectBinder.History.CanRedo;
    }

    private void editToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
    {
      undoToolStripMenuItem.Enabled = _xmlToObjectBinder.History.CanUndo;
      redoToolStripMenuItem.Enabled = _xmlToObjectBinder.History.CanRedo;
    }

    private void findToolStripButton_Click(object sender, EventArgs e)
    {
      ShowFindAndReplaceDlg();
    }

    private void undoToolStripButton_Click(object sender, EventArgs e)
    {
      Undo();
    }

    private void redoToolStripButton_Click(object sender, EventArgs e)
    {
      Redo();
    }

    private void openToolStripButton_Click(object sender, EventArgs e)
    {
      OpenGranitXmlFile();
    }

    private void newToolStripButton_Click(object sender, EventArgs e)
    {
      New();
    }

    private void helpToolStripButton_Click(object sender, EventArgs e)
    {
      About();
    }

    private void saveToolStripButton_Click(object sender, EventArgs e)
    {
      AskAndSaveFile();
    }

    private void dataGridView1_SelectionChanged(object sender, EventArgs e)
    {
      decimal sum = 0;
      int count = 0;
      foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
      {
        if(cell.Value is decimal)
        {
          sum += (decimal)cell.Value;
          count++;
        }
      }
      selectedAmountStatus.Text = "Sum of Selected: " + sum.ToString();
      selectedStatusLabel.Text = "Selected: " + count;
    }
  }
}
