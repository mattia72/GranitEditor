using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using GranitXMLEditor.Properties;
using System.Diagnostics;
using System.Linq;
using System.Xml.Schema;
using System.Collections.Generic;
using GenericUndoRedo;

namespace GranitXMLEditor
{
  public partial class GranitXMLEditorForm : Form
  {
    private GranitXmlToAdapterBinder _xmlToObjectBinder;
    private OpenFileDialog _openFileDialog;
    private SaveFileDialog _saveFileDialog;

    private string _lastOpenedFilePath;
    private bool _docHasPendingChanges=false;
    private GranitDataGridViewCellValidator _cellVallidator;
    private GranitDataGridViewContextMenuHandler _contextMenuHandler;
    private SortableBindingList<TransactionAdapter> _bindingList;

    internal UndoRedoHistory<IGranitXDocumentOwner> History => _xmlToObjectBinder?.History;

    internal DataGridView DataGrid => dataGridView1;

    public GranitXMLEditorForm(string xmlFilePath, OpenFileDialog ofDlg, SaveFileDialog sfDlg)
    {
      InitializeComponent();
      OpenNewDocument(); 
      _openFileDialog = ofDlg;
      _saveFileDialog = sfDlg;
      _cellVallidator = new GranitDataGridViewCellValidator(dataGridView1);
      _contextMenuHandler = new GranitDataGridViewContextMenuHandler(dataGridView1, contextMenuStrip1, _xmlToObjectBinder);
      SetTextResources();
      ApplySettings();
      //Drag & Drop support
      AllowDrop = true;
      dataGridView1.AllowDrop = true;
      if(File.Exists(xmlFilePath))
        LoadDocument(xmlFilePath);
      else
        LastOpenedFilePath = xmlFilePath;
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

    internal GranitDataGridViewContextMenuHandler ContextMenuHandler => _contextMenuHandler;

    public string LastOpenedFilePath
    {
      get => _lastOpenedFilePath;
      set
      {
        _lastOpenedFilePath = value;
        Text = Path.GetFileName(value);
        MainForm?.SetActiveFilePath(value);
      }
    }

    public bool DocHasPendingChanges
    {
      get => _docHasPendingChanges;
      set
      {
        _docHasPendingChanges = value;
        if(_docHasPendingChanges)
          MainForm?.SetDocsHavePendingChanges(value);
      }
    }

    private GranitEditorMainForm MainForm { get => (ParentForm as GranitEditorMainForm); }

    private void ApplySettings()
    {
      if (Settings.Default.AlignTable != 0)
      {
        dataGridView1.AutoSizeColumnsMode = Settings.Default.AlignTable;
        MainForm?.GridAlignMenu.SetCheckedByValue(dataGridView1.AutoSizeColumnsMode);
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
        Debug.WriteLine("Remove transactionId: " + transactionIdTodelete + " from index: " + e.RowIndex);
        _xmlToObjectBinder.RemoveTransactionRowById((long)transactionIdTodelete);
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
      Settings.Default.AlignTable = DataGrid.AutoSizeColumnsMode;
      MainForm.FillSettingsList(Settings.Default.LastOpenedFilePaths, new string[]{ LastOpenedFilePath });
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

    public void SaveDocument(string fileName)
    {
      string xmlFilePath = Path.GetFullPath(fileName);
      _xmlToObjectBinder.SaveToFile(xmlFilePath);
      LastOpenedFilePath = xmlFilePath;
      DocHasPendingChanges = false;
    }

    public void LoadDocument(string xmlFilePath)
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

        LastOpenedFilePath = MainForm?.GetNextNewDocumentName();
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

    public DialogResult CheckPendingChangesAndSaveIfNecessary()
    {
      DialogResult answere = DialogResult.OK; ;
      if (DocHasPendingChanges)
        answere = AskAndSaveFile(MessageBoxButtons.YesNoCancel);
      return answere;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Debug.WriteLine("OnClosing called. docHasPendingChanges: {0}", DocHasPendingChanges);

      if (DocHasPendingChanges || !File.Exists(LastOpenedFilePath))
        e.Cancel = AskAndSaveFile(MessageBoxButtons.YesNoCancel) == DialogResult.Cancel;

      if(!e.Cancel)
        SaveSettings();

      base.OnClosing(e);
    }

    private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MainForm.ShowFindAndReplaceDlg();
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
      ActualizeStatusLabelsOfAll();
    }

    public void ActualizeStatusLabelsOfAll()
    {
      MainForm?.SetStatusLabelItemText("allStatusLabel",
        Resources.StatusCountAllText + _xmlToObjectBinder.TransactionCount);
      MainForm?.SetStatusLabelItemText("allAmountStatus",
        Resources.StatusSumAllText + _xmlToObjectBinder.SumAmount + " Ft");
    }

    private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      var dlg = MainForm.CreateFindDialog(DataGrid);
      dlg.IsFirstInitNecessary = true;
    }

    public void RebindBindingList()
    {
      if(dataGridView1.IsCurrentCellInEditMode)
        dataGridView1.CancelEdit();
      _bindingList = new SortableBindingList<TransactionAdapter>(_xmlToObjectBinder.HUFTransactionsAdapter.TransactionAdapters);
      dataGridView1.DataSource = _bindingList;
      if (_bindingList.RaiseListChangedEvents)
        _bindingList.ListChanged += bindingList_ListChanged;
    }

    private void bindingList_ListChanged(object sender, ListChangedEventArgs e)
    {
      Debug.WriteLine("BindingList changed " + e.ListChangedType + " " + e.PropertyDescriptor);
    }

    private void OpenNewDocument()
    {
      _xmlToObjectBinder = new GranitXmlToAdapterBinder();
      RebindBindingList();
      DocHasPendingChanges = true;
    }

    /// <summary>
    /// Asks if parameter != null, else saves without any question
    /// </summary>
    /// <param name="buttons"></param>
    /// <returns></returns>
    public DialogResult AskAndSaveFile(MessageBoxButtons? buttons = null)
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
        if (File.Exists(LastOpenedFilePath))
        {
          SaveDocument(LastOpenedFilePath);
        }
        else
        {
          string f = MainForm.GetFileNameToSaveByOpeningSaveFileDialog();
          if (f != null && f != string.Empty)
            SaveDocument(f);
          else
            return DialogResult.Cancel;
        }
      }
      return answ;
    }

    public void SelectAll()
    {
      dataGridView1.EndEdit();
      dataGridView1.SelectAll();
      if (dataGridView1.RowCount > 0)
      {
        var lastRow = dataGridView1.Rows[dataGridView1.RowCount - 1];
        lastRow.Selected = false;
      }
    }

    private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
      GranitDataGridViewCellFormatter.Format(dataGridView1, ref e);
      if (e.ColumnIndex == dataGridView1.Columns[0].Index)
      {
        var cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        TransactionAdapter ta = (TransactionAdapter)dataGridView1.Rows[e.RowIndex].DataBoundItem;
        if (ta != null)
          cell.ToolTipText = ta.ToString();
      }
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      EnableAllcontextMenuItem(false);
      if (dataGridView1.SelectedRows.Count > 1)
      {
        deleteRowToolStripMenuItem.Text = Resources.ContextMenuDeleteSelected;
        deleteRowToolStripMenuItem.Enabled = true;
      }
      else
      {
        if(!dataGridView1.IsCurrentCellInEditMode)
          EnableAllcontextMenuItem(true);
        deleteRowToolStripMenuItem.Text = Resources.ContextMenuDeleteRow; 
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
      DocHasPendingChanges = true;
    }

    private void duplicateRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      History?.Do(new TransactionPoolMemento(_xmlToObjectBinder.GranitXDocument));

      _contextMenuHandler.grid_DuplicateRow(sender, e);
      DocHasPendingChanges = true;
    }

    private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
    {
      History?.Do(new TransactionPoolMemento(_xmlToObjectBinder.GranitXDocument));
      _contextMenuHandler.grid_DeleteSelectedRows(sender, e);
      DocHasPendingChanges = true;
    }

    private void newToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      History?.Do(new TransactionPoolMemento(_xmlToObjectBinder.GranitXDocument));
      _contextMenuHandler.grid_AddNewRow(sender, e);
      DocHasPendingChanges = true;
    }

    public void Undo()
    {
      if (_xmlToObjectBinder.History.CanUndo)
      {
        _xmlToObjectBinder.History_Undo();
        RebindBindingList();
      }
    }

    public void Redo()
    {
      if (_xmlToObjectBinder.History.CanRedo)
      {
        _xmlToObjectBinder.History_Redo();
        RebindBindingList();
      }
    }

    private void dataGridView1_SelectionChanged(object sender, EventArgs e)
    {
      ActualizeStatusLabelsOfSelected();
    }

    public void ActualizeStatusLabelsOfSelected()
    {
      decimal sum = 0;
      HashSet<int> selectedRowIndexes = new HashSet<int>();
      foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
      {
        selectedRowIndexes.Add(cell.OwningRow.Index);
      }
      foreach (int index in selectedRowIndexes)
      {
        object o = dataGridView1.Rows[index].Cells["amountDataGridViewTextBoxColumn"].Value;
        sum += o == null ? 0 : (decimal)o;
      }
      MainForm?.SetStatusLabelItemText("selectedAmountStatus",
        Resources.StatusSumSelectedText + sum.ToString() + " Ft");
      MainForm?.SetStatusLabelItemText("selectedStatusLabel",
       Resources.StatusCountSeletedText + selectedRowIndexes.Count);
    }

    private void dataGridView1_DragEnter(object sender, DragEventArgs e)
    {
      string filename = MainForm.GetDropFileName(e);
      if (filename != string.Empty)
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    private void dataGridView1_DragDrop(object sender, DragEventArgs e)
    {
      string[] files = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
      MainForm.OpenNewFormWith(Path.GetFullPath(files[0]).ToString());
    }

    private void GranitXMLEditorForm_Shown(object sender, EventArgs e)
    {
      // Work-around for error in WinForms that causes MDI children to be loaded with the default .NET icon when opened maximised.
      Icon = Icon.Clone() as System.Drawing.Icon;
      WindowState = FormWindowState.Normal;
      WindowState = FormWindowState.Maximized;
    }
  }
}
