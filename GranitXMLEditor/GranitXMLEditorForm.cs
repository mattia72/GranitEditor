using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using GranitXMLEditor.Properties;
using System.Diagnostics;
using System.Linq;
using GenericUndoRedo;
using System.Collections.Generic;

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
    private MruStripMenu _mruMenu;
    private EnumStripMenu<DataGridViewAutoSizeColumnsMode> _autoSizeMenu;
    private GranitDataGridViewCellValidator _cellVallidator;
    private SortableBindingList<TransactionAdapter> _bindingList;
    private GranitDataGridViewContextMenuHandler _contextMenuHandler;
    private TransactionPool _transactionPool = new TransactionPool();
    //private UndoRedoHistory<TransactionPool> _history;
    private UndoRedoHistory<List<Transaction>> _history;

    public GranitXMLEditorForm()
    {
      InitializeComponent();
      _mruMenu = new MruStripMenu(recentFilesToolStripMenuItem, mruMenu_Clicked, 10);
      _autoSizeMenu = new EnumStripMenu<DataGridViewAutoSizeColumnsMode>(alignTableToolStripMenuItem, autoSizeMenu_Clicked);
      OpenLastOpenedFileIfExists();
      _docHasPendingChanges = false;
      //_history = new UndoRedoHistory<TransactionPool>(_transactionPool);
      _history = new UndoRedoHistory<List<Transaction>>(_xmlToObject.HUFTransaction.Transactions);
      _cellVallidator = new GranitDataGridViewCellValidator(dataGridView1);
      _contextMenuHandler = new GranitDataGridViewContextMenuHandler(dataGridView1, contextMenuStrip1, _xmlToObject);
      SetTextResources();
      ApplySettings();
    }

    private void autoSizeMenu_Clicked(DataGridViewAutoSizeColumnsMode mode)
    {
      dataGridView1.AutoSizeColumnsMode = mode == 0 ? DataGridViewAutoSizeColumnsMode.None : mode;
    }

    private void SetTextResources()
    {
      isActiveDataGridViewCheckBoxColumn.HeaderText = Resources.IsActiveHeaderText;
      originatorDataGridViewTextBoxColumn.HeaderText = Resources.OriginatorHeaderText;
      //originatorDataGridViewTextBoxColumn.
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

    private void ApplySettings()
    {
      if (!string.IsNullOrEmpty(Settings.Default.SortedColumn))
      {
        DataGridViewColumn sortedColumn = FindColumnByHeaderText(Settings.Default.SortedColumn);
        if(_bindingList.RaiseListChangedEvents)
          _bindingList.ListChanged += _bindingList_ListChanged;

        //_xmlToObject.HUFTransactionsAdapter.Sort(TransactionAdapter.SortAmountAscending());
        //_bindingList.Listanged


        dataGridView1.Sort(sortedColumn,
          Settings.Default.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
        sortedColumn.HeaderCell.SortGlyphDirection = Settings.Default.SortOrder == SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
      }
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

    private void _bindingList_ListChanged(object sender, ListChangedEventArgs e)
    {
      Debug.WriteLine("BindingList changed " + e.ListChangedType + " " + e.PropertyDescriptor);

    }

    private void SaveSettings()
    {
      Settings.Default.SortedColumn = dataGridView1.SortedColumn != null ? dataGridView1.SortedColumn.HeaderText : "";
      Settings.Default.SortOrder = dataGridView1.SortOrder;
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
      _openFileDialog1 = _openFileDialog1 == null ? new OpenFileDialog() : _openFileDialog1;
      _openFileDialog1.InitialDirectory = Application.StartupPath;
      _openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
      _openFileDialog1.FilterIndex = 2;
      _openFileDialog1.RestoreDirectory = true;

      if (_openFileDialog1.ShowDialog() == DialogResult.OK)
        LoadDocument(_openFileDialog1.FileName);
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
          filename = _saveFileDialog1.FileName;
      }
      return filename;
    }

    private void SaveDocument(string fileName)
    {
      string xmlFilePath = Path.GetFullPath(fileName);
      _xmlToObject.SaveToFile(xmlFilePath);
      LastOpenedFilePath = xmlFilePath;
      _docHasPendingChanges = false;
    }

    private void LoadDocument(string xmlFilePath)
    {
      _xmlToObject = new GranitXmlToObjectBinder(xmlFilePath);
      _bindingList = new SortableBindingList<TransactionAdapter>(_xmlToObject.HUFTransactionsAdapter.Transactions);
      dataGridView1.DataSource = _bindingList;
      LastOpenedFilePath = xmlFilePath;
      _docHasPendingChanges = false;
    }

    private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      if (dataGridView1.CurrentCell.Tag == e.FormattedValue)
      {
        e.Cancel = true;    //Cancel changes of current cell
        return;
      }

      string headerText = dataGridView1.Columns[e.ColumnIndex].HeaderText;

      _cellVallidator.Validate(ref e, headerText);
    }

    //Transaction beginEditTrans=null;
    private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      Debug.WriteLine("CellBeginEdit called on row: {0} col: {1}", e.RowIndex, e.ColumnIndex);
      //beginEditTrans = ((TransactionAdapter)dataGridView1.Rows[e.RowIndex].DataBoundItem).Transaction;

      if ( e.RowIndex != -1 ) // not on first load 
        _history.Do(new AddTransactionMemento());
    }

    private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      Debug.WriteLine("CellEndEdit called on row: {0} col: {1}", e.RowIndex, e.ColumnIndex);
      if ( e.RowIndex != -1 ) // not on first load 
        dataGridView1.Rows[e.RowIndex].ErrorText = string.Empty;
    }

    private void dataGridView1_Sorted(object sender, EventArgs e)
    {
      //_history.BeginCompoundDo();
      //foreach (Transaction t in _xmlToObject.HUFTransaction.Transactions)
      //{
      //  _history.Do(new RemoveTransactionMemento(t));
      //}
      _xmlToObject.Sort(dataGridView1.SortedColumn.HeaderText, dataGridView1.SortOrder);

      //foreach (Transaction t in _xmlToObject.HUFTransaction.Transactions)
      //{
      //  _history.Do(new AddTransactionMemento());
      //}
      //_history.EndCompoundDo();

      _docHasPendingChanges = true;
    }

    private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      Debug.WriteLine("CellValueChanged called on row: {0} col: {1}", e.RowIndex, e.ColumnIndex);
      if ( e.RowIndex != -1 ) // not on first load 
      { 
        _docHasPendingChanges = true;
      }
    }

    private void openToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      OpenGranitXmlFile();
    }

    private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      string f = GetFileNameToSaveByOpeningSaveFileDialog();
      SaveDocument(f);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Debug.WriteLine("OnClosing called. docHasPendingChanges: {0}", _docHasPendingChanges);

      if (_docHasPendingChanges || LastOpenedFilePath == string.Empty)
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

    private void dataGridView1_UserAddedNewRow(object sender, DataGridViewRowEventArgs e)
    {
      var bindingList = ((SortableBindingList<TransactionAdapter>)dataGridView1.DataSource);
      //delete last (non working) adapter and create a new...
      bindingList.RemoveAt(bindingList.Count - 1);
      bindingList.Add(_xmlToObject.AddEmptyTransactionRow());
      dataGridView1.CurrentCell = e.Row.Cells[1]; 
      dataGridView1.BeginEdit(false);
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      CreateFindDialog();
      _findReplaceDlg.IsFirstInitNecessary = true;
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogResult answere = DialogResult.OK; ;
      if (_docHasPendingChanges)
        answere = AskAndSaveFile(MessageBoxButtons.YesNoCancel);

      if(answere != DialogResult.Cancel)
        OpenNewDocument();
    }

    private void OpenNewDocument()
    {
      _xmlToObject = new GranitXmlToObjectBinder();
      _bindingList = new SortableBindingList<TransactionAdapter>(_xmlToObject.HUFTransactionsAdapter.Transactions);
      dataGridView1.DataSource = _bindingList;
      LastOpenedFilePath = string.Empty;
    }

    private DialogResult AskAndSaveFile(MessageBoxButtons buttons)
    {
      var answ = MessageBox.Show(
        string.Format(Resources.DoYouWantToSave, Path.GetFileName(LastOpenedFilePath)),
        Application.ProductName, buttons, MessageBoxIcon.Question);

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
      if (LastOpenedFilePath != string.Empty)
      {
        AskAndSaveFile(MessageBoxButtons.YesNo);
      }
      else
      {
        var file = GetFileNameToSaveByOpeningSaveFileDialog();
        SaveDocument(file);
      }
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
      if (dataGridView1.SelectedRows.Count > 1)
      {
        EnableAllcontextMenuItem(false);
        deleteRowToolStripMenuItem.Text = "Delete Selected";
        deleteRowToolStripMenuItem.Enabled = true;
      }
      else
      {
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

    private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
      long? _transactionIdTodelete = null;
      if(dataGridView1.Rows.Count > e.RowIndex)
        _transactionIdTodelete = ((TransactionAdapter)dataGridView1.Rows[e.RowIndex].DataBoundItem)?.TransactionId;

      if (_transactionIdTodelete != null)
      {
        _history.Do(new RemoveTransactionMemento(((TransactionAdapter)dataGridView1.Rows[e.RowIndex].DataBoundItem).Transaction));
        _xmlToObject.RemoveTransactionRowById((long)_transactionIdTodelete);
        _docHasPendingChanges = true;
      }
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
      if (_history.CanUndo)
      {
        _history.Undo();
        _xmlToObject.UpdateGranitXDocument();
      }
    }

    private void redoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_history.CanRedo)
      {
        _history.Redo();
        _xmlToObject.UpdateGranitXDocument();
      }
    }

    private void editToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
    {
      undoToolStripMenuItem.Enabled = _history.CanUndo;
      redoToolStripMenuItem.Enabled = _history.CanRedo;
    }

  }
}
