using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using GranitEditor.Properties;
using System.Diagnostics;
using System.Linq;
using System.Xml.Schema;
using System.Collections.Generic;
using GenericUndoRedo;
using System.Drawing;

namespace GranitEditor
{
  public partial class GranitXMLEditorForm : Form
  {
    private GranitXmlToAdapterBinder _xmlToObjectBinder;
    private OpenFileDialog _openFileDialog;
    private SaveFileDialog _saveFileDialog;

    private string _lastOpenedFilePath;
    private GranitDataGridViewCellValidator _cellVallidator;
    private GranitDataGridViewContextMenuHandler _contextMenuHandler;
    private SortableBindingList<TransactionAdapter> _bindingList;
    private DataGridViewTextBoxEditingControl _editingControl;

    internal UndoRedoHistory<IGranitXDocumentOwner> History => XmlToObjectBinder?.History;
    internal DataGridView DataGrid => dataGridView1;
    internal bool HasSelectedCells => dataGridView1.SelectedCells.Count > 0;
    internal string SelectedTextInCurrentCell { get => _editingControl?.SelectedText; }

    public GranitXMLEditorForm(string xmlFilePath,
      OpenFileDialog ofDlg,
      SaveFileDialog sfDlg,
      ClipboardHandler clip)
    {
      InitializeComponent();

      if (!File.Exists(xmlFilePath))
        OpenNewDocument();

      _openFileDialog = ofDlg;
      _saveFileDialog = sfDlg;
      _cellVallidator = new GranitDataGridViewCellValidator(dataGridView1);

      dataGridView1.KeyDown += new KeyEventHandler(this.DataGridView1_KeyDown);
      ClipboardHandler = clip;

      SetTextResources();
      ApplySettings();

      //Drag & Drop support
      AllowDrop = true;
      dataGridView1.AllowDrop = true;

      if (File.Exists(xmlFilePath))
        LoadDocument(xmlFilePath);
      else
        LastOpenedFilePath = xmlFilePath;

      //This should bee the last call
      _contextMenuHandler = 
        new GranitDataGridViewContextMenuHandler(dataGridView1, contextMenuStrip1, XmlToObjectBinder);
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
      get => XmlToObjectBinder.DocHasPendingChanges;
    }

    private GranitEditorMainForm MainForm { get => (ParentForm as GranitEditorMainForm); }
    public ClipboardHandler ClipboardHandler { get; set; }
    public GranitXmlToAdapterBinder XmlToObjectBinder { get => _xmlToObjectBinder; set => _xmlToObjectBinder = value; }

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
    private void DataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
      long? transactionIdTodelete = null;
      if (dataGridView1.Rows.Count > e.RowIndex)
      {
        transactionIdTodelete = XmlToObjectBinder.HUFTransactionsAdapter.TransactionAdapters.
         Where(x => GetRow(x.TransactionId) == null).FirstOrDefault()?.TransactionId;
      }

      if (transactionIdTodelete != null)
      {
        Debug.WriteLine("Remove transactionId: " + transactionIdTodelete + " from index: " + e.RowIndex);
        XmlToObjectBinder.RemoveTransactionRowById((long)transactionIdTodelete);
        MainForm?.UpdateToolbarItems();
      }
    }

    private DataGridViewRow GetRow(long transactionId)
    {
      foreach (DataGridViewRow row in dataGridView1.Rows)
      {
        if (row.DataBoundItem != null &&
          (row.DataBoundItem as TransactionAdapter).TransactionId == transactionId)
          return row;
      }
      return null;
    }

    private void SaveSettings()
    {
      Settings.Default.AlignTable = DataGrid.AutoSizeColumnsMode;
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

      dataGridView1.EndEdit();
      XmlToObjectBinder.SaveToFile(xmlFilePath);
      LastOpenedFilePath = xmlFilePath;
      MainForm?.UpdateToolbarItems();
    }

    public void LoadDocument(string xmlFilePath)
    {
      XmlToObjectBinder = new GranitXmlToAdapterBinder(xmlFilePath, true);
      if (XmlToObjectBinder.XmlValidationErrorOccured)
      {
        XmlSchemaException e = XmlToObjectBinder.ValidationEventArgs.Exception;

        MessageBox.Show(string.Format(
          Resources.ValidationErrorMsg,
          XmlToObjectBinder.ValidationEventArgs.Severity == XmlSeverityType.Error ? Resources.ErrorText : Resources.WarningText,
          xmlFilePath + " (" + e.LineNumber + ", " + e.LinePosition + ")",
          XmlToObjectBinder.ValidationEventArgs.Message),
          Application.ProductName,
          MessageBoxButtons.OK,
          MessageBoxIcon.Error);

        XmlToObjectBinder = new GranitXmlToAdapterBinder();

        LastOpenedFilePath = MainForm?.GetNextNewDocumentName();
      }
      else
      {
        LastOpenedFilePath = xmlFilePath;
      }

      RebindBindingList();
    }

    private void DataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
      // Don't try to validate the 'new row' until finished 
      // editing since there
      // is not any point in validating its initial value.
      if (dataGridView1.Rows[e.RowIndex].IsNewRow) { return; }

      if (dataGridView1.CurrentCell.Tag == e.FormattedValue)
      {
        e.Cancel = true;    //Cancel changes of current cell
        return;
      }

      string propertyName = dataGridView1.Columns[e.ColumnIndex].DataPropertyName;

      _cellVallidator.Validate(ref e, propertyName);

    }

    //Transaction beginEditTrans=null;
    private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
      Debug.WriteLine("CellBeginEdit called on row: {0} col: {1}", e.RowIndex, e.ColumnIndex);
      //beginEditTrans = ((TransactionAdapter)dataGridView1.Rows[e.RowIndex].DataBoundItem).Transaction;
      //if ( e.RowIndex != -1 ) // not on first load 
    }

    private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      Debug.WriteLine("CellEndEdit called on row: {0} col: {1}", e.RowIndex, e.ColumnIndex);
      if (e.RowIndex != -1) // not on first load 
        dataGridView1.Rows[e.RowIndex].ErrorText = string.Empty;
      _editingControl = null;
    }

    private void DataGridView1_Sorted(object sender, EventArgs e)
    {
      XmlToObjectBinder.Sort(dataGridView1.SortedColumn.DataPropertyName, dataGridView1.SortOrder);
      MainForm?.UpdateToolbarItems();
    }

    private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      Debug.WriteLine("CellValueChanged called on row: {0} col: {1}", e.RowIndex, e.ColumnIndex);
      if (e.RowIndex != -1) // not on first load 
        MainForm?.UpdateToolbarItems();
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

      if (!e.Cancel)
        SaveSettings();

      base.OnClosing(e);
    }

    private void DataGridView1_UserAddedNewRow(object sender, DataGridViewRowEventArgs e)
    {
      var bindingList = ((SortableBindingList<TransactionAdapter>)dataGridView1.DataSource);
      //Commit the first character
      dataGridView1.EndEdit();
      //change binding list item to our Adapter
      var ta = bindingList[bindingList.Count - 1];
      bindingList[bindingList.Count - 1] = XmlToObjectBinder.AddTransactionRow(ta);
      Debug.WriteLine("User Added Adapter: " + (ta != null ? ta.ToString() : "null"));
      dataGridView1.BeginEdit(false);
    }

    private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      Debug.WriteLine(e.RowCount + " rows added at index: " + e.RowIndex);
      TransactionAdapter ta = (TransactionAdapter)dataGridView1.Rows[e.RowIndex].DataBoundItem;

      Debug.WriteLine("Adapter: " + (ta != null ? ta.ToString() : "null"));
      ActualizeStatusLabelsOfAll();
    }

    public void ActualizeStatusLabelsOfAll()
    {
      MainForm?.SetStatusLabelItemText("allStatusLabel",
        Resources.StatusCountAllText + XmlToObjectBinder.TransactionCount);
      MainForm?.SetStatusLabelItemText("allAmountStatus",
        Resources.StatusSumAllText + XmlToObjectBinder.SumAmount + " Ft");
    }

    private DateTimePicker _dateTimePicker = null;

    private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      var dlg = MainForm.CreateFindDialog(DataGrid);
      dlg.IsFirstInitNecessary = true;

      ShowDateTimePickerIfNeeded(e);

    }

    private void ShowDateTimePickerIfNeeded(DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].HeaderText == Resources.RequestedExecutionDateHeaderText)
      {
        if (_dateTimePicker != null)
          _dateTimePicker.Visible = false;
        else
          _dateTimePicker = new DateTimePicker();

        dataGridView1.Controls.Add(_dateTimePicker);
        _dateTimePicker.Format = DateTimePickerFormat.Short;
        Rectangle Rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
        _dateTimePicker.Size = new Size(Rectangle.Width, Rectangle.Height);
        _dateTimePicker.Location = new Point(Rectangle.X, Rectangle.Y);
        _dateTimePicker.CloseUp += new EventHandler(Dtp_CloseUp);
        _dateTimePicker.TextChanged += new EventHandler(Dtp_OnTextChange);
        _dateTimePicker.Value = (DateTime)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

        _dateTimePicker.Visible = true;
      }
      else if (_dateTimePicker != null && _dateTimePicker.Visible)
      {
        _dateTimePicker.Visible = false;
      }
    }

    private void Dtp_OnTextChange(object sender, EventArgs e)
    {
      dataGridView1.CurrentCell.Value = _dateTimePicker.Text.ToString();
    }

    private void Dtp_CloseUp(object sender, EventArgs e)
    {
      _dateTimePicker.Visible = false;
    }

    public void RebindBindingList()
    {
      Debug.WriteLine("RebindBindingList");

      if (dataGridView1.IsCurrentCellInEditMode)
        dataGridView1.CancelEdit();

      _bindingList = new SortableBindingList<TransactionAdapter>(XmlToObjectBinder.HUFTransactionsAdapter.TransactionAdapters);
      dataGridView1.DataSource = _bindingList;
      if (_bindingList.RaiseListChangedEvents)
        _bindingList.ListChanged += BindingList_ListChanged;
    }

    private void BindingList_ListChanged(object sender, ListChangedEventArgs e)
    {
      Debug.WriteLine("BindingList changed " + e.ListChangedType + " " + e.PropertyDescriptor);
      switch (e.ListChangedType)
      {
        case ListChangedType.ItemAdded:
        case ListChangedType.ItemDeleted:
        case ListChangedType.ItemChanged:
          MainForm?.UpdateToolbarItems();
          break;
      }
    }

    private void OpenNewDocument()
    {
      XmlToObjectBinder = new GranitXmlToAdapterBinder();
      RebindBindingList();
      MainForm?.UpdateToolbarItems();
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

    private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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

    private void DataGridView1_MouseClick(object sender, MouseEventArgs e)
    {
      _contextMenuHandler.Grid_MouseClick(sender, e);
    }

    private void DuplicateRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      History?.Do(new TransactionPoolMemento(XmlToObjectBinder.GranitXDocument));

      _contextMenuHandler.Grid_DuplicateRow(sender, e);
        //DocHasPendingChanges = true;
    }

    private void AddRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      History?.Do(new TransactionPoolMemento(XmlToObjectBinder.GranitXDocument));
      _contextMenuHandler.Grid_AddNewRow(sender, e);
        //DocHasPendingChanges = true;
    }

    public void Undo()
    {
      if (XmlToObjectBinder.History.CanUndo) 
      {
        XmlToObjectBinder.History_Undo();
        RebindBindingList();
      }
      MainForm?.UpdateSaveAndSaveAsItems();
    }

    public void Redo()
    {
      if (XmlToObjectBinder.History.CanRedo)
      {
        XmlToObjectBinder.History_Redo();
        RebindBindingList();
      }
      MainForm?.UpdateSaveAndSaveAsItems();
    }

    private void DataGridView1_SelectionChanged(object sender, EventArgs e)
    {
      ActualizeStatusLabelsOfSelected();
      MainForm.UpdateCopyPasteItems();
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

    private void DataGridView1_DragEnter(object sender, DragEventArgs e)
    {
      string filename = MainForm.GetDropFileName(e);
      if (filename != string.Empty)
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    private void DataGridView1_DragDrop(object sender, DragEventArgs e)
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

    private void DataGridView1_KeyDown(object sender, KeyEventArgs e)
    {
      try
      {
        if (e.Modifiers == Keys.Control)
        {
          switch (e.KeyCode)
          {
            case Keys.C:
            case Keys.Insert:
              ClipboardHandler.CopyToClipboard(dataGridView1);
              break;

            case Keys.V:
              ClipboardHandler.PasteClipboardValue(dataGridView1);
              break;
          }
        }
        if (e.Modifiers == Keys.Shift)
        {
          switch (e.KeyCode)
          {
            case Keys.Insert:
              ClipboardHandler.PasteClipboardValue(dataGridView1);
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show("Copy/paste operation failed. " + ex.Message,Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    public void DeleteRowToolStripMenuItem_Click(object sender, EventArgs e)
    {
      History?.Do(new TransactionPoolMemento(XmlToObjectBinder.GranitXDocument));
      _contextMenuHandler.Grid_DeleteSelectedRows(sender, e);
        //DocHasPendingChanges = true;
    }

    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MainForm.Copy();
    }

    private void CutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MainForm.Cut();
    }

    private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      MainForm.Paste();
    }

    private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      //// Don't throw an exception when we're done.
      //e.ThrowException = false;

      //// Display an error message.
      //string txt = "Error with " +
      //    dataGridView1.Columns[e.ColumnIndex].HeaderText +
      //    "\n\n" + e.Exception.Message;
      //MessageBox.Show(txt, "Error",
      //    MessageBoxButtons.OK, MessageBoxIcon.Error);

      //// If this is true, then the user is trapped in this cell.
      //e.Cancel = false;
    }

    private void DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
      if (e.Control is DataGridViewTextBoxEditingControl)
      {
        _editingControl = (DataGridViewTextBoxEditingControl)e.Control;
      }
    }
  }
}
