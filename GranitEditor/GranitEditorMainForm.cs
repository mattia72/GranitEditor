using GranitEditor.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using static GranitEditor.Constants;

namespace GranitEditor
{
  public partial class GranitEditorMainForm : Form
  {
    private readonly MruStripMenu _mruMenu;
    private string _activeFilePath;
    private EnumStripMenu<DataGridViewAutoSizeColumnsMode> _gridAlignMenu;

    private AboutBox _aboutBox;
    private OpenFileDialog _openFileDialog;
    private SaveFileDialog _saveFileDialog;
    private FindReplaceDlg _findReplaceDlg;
    private readonly EnumStripMenu<Constants.WindowLayout> _windowLayoutMenu;
    private WindowLayout _windowLayout;

    public GranitXMLEditorForm ActiveXmlForm => ActiveMdiChild as GranitXMLEditorForm;
    public string ActiveFilePath { get => _activeFilePath; set => _activeFilePath = value; }

    private ClipboardHandler _clipboardHandler;

    public bool ActiveDocHavePendingChanges
    {
      get => (ActiveXmlForm?.DocHasPendingChanges) != null && ActiveXmlForm.DocHasPendingChanges; 
    }

    public void UpdateToolbarItems()
    {
      UpdateSaveAndSaveAsItems();
      UpdateUndoRedoItems();
    }

    public EnumStripMenu<DataGridViewAutoSizeColumnsMode> GridAlignMenu { get => _gridAlignMenu; set => _gridAlignMenu = value; }

    public OpenFileDialog OpenFileDialog
    {
      get
      {
        if (_openFileDialog == null)
        {
          _openFileDialog = new OpenFileDialog
          {
            InitialDirectory = Application.StartupPath,
            Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
            FilterIndex = 1,
            RestoreDirectory = true,
            Multiselect = true
          };
        }
        _openFileDialog.InitialDirectory =
          ActiveFilePath == null ? Application.StartupPath : Path.GetFileName(ActiveFilePath);
        return _openFileDialog;
      }
    }

    public SaveFileDialog SaveFileDialog
    {
      get
      {
        if (_saveFileDialog == null)
        {
          _saveFileDialog = new SaveFileDialog
          {
            Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*",
            FilterIndex = 1,
            RestoreDirectory = true,
            AddExtension = true,
            DefaultExt = "xml"
          };
        }

        return _saveFileDialog;
      }
    }

    public FindReplaceDlg FindReplaceDlg { get => _findReplaceDlg; set => _findReplaceDlg = value; }

    public ClipboardHandler ClipboardHandler
    {
      get
      {
        if (_clipboardHandler == null)
          _clipboardHandler = new ClipboardHandler();
        return _clipboardHandler;
      }
      set => _clipboardHandler = value;
    }

    public GranitEditorMainForm()
    {
      InitializeComponent();
      _mruMenu = new MruStripMenu(recentFilesToolStripMenuItem, MostRecentMenu_Clicked, 10);
      _gridAlignMenu = new EnumStripMenu<DataGridViewAutoSizeColumnsMode>(alignTableToolStripMenuItem, AutoSizeMenu_Clicked);
      _windowLayoutMenu = new EnumStripMenu<WindowLayout>(layoutToolStripMenuItem, ChangeWindowLayout);
      OpenLastOpenedFilesIfExists();
      ApplySettings();
      ActualizeMenuToolBarAndStatusLabels();
    }

    public string GetNextNewDocumentName()
    {
      string newName;
      int i = 1;
      do
      {
        bool found = false;
        newName = string.Format(Resources.NewDocumentName, i++);
        foreach (var child in MdiChildren)
        {
          if (File.Exists(newName))
          {
            found = true;
            break;
          }
          if (child is GranitXMLEditorForm)
          {
            var form = child as GranitXMLEditorForm;
            if (form.LastOpenedFilePath != null && form.LastOpenedFilePath.EndsWith(newName))
            {
              found = true;
              break;
            }
          }
        }
        if (!found) break;
      } while (true);
      return newName;
    }

    public void SetActiveFilePath(string value)
    {
      ActiveFilePath = value;

      //in case of save as, change tab text too...
      if (ActiveMdiChild?.Tag != null)
        (ActiveMdiChild.Tag as TabPage).Text = Path.GetFileName(_activeFilePath);

      if (!string.IsNullOrEmpty(_activeFilePath))
        _mruMenu.AddFile(_activeFilePath);
    }

    private void ChangeWindowLayout(Constants.WindowLayout enumItem)
    {
      switch (enumItem)
      {
        case Constants.WindowLayout.Cascade:
          formsTabControl.Visible = false;
          LayoutMdi(MdiLayout.Cascade);
          break;
        case Constants.WindowLayout.TileHorizontal:
          formsTabControl.Visible = false;
          LayoutMdi(MdiLayout.TileHorizontal);
          break;
        case Constants.WindowLayout.TileVertical:
          formsTabControl.Visible = false;
          LayoutMdi(MdiLayout.TileVertical);
          break;
        case Constants.WindowLayout.Tabbed:
          if (ActiveMdiChild != null)
          {
            formsTabControl.Visible = true;
            ActiveMdiChild.WindowState = FormWindowState.Maximized;
          }
          break;
        default:
          ChangeWindowLayout(WindowLayout.Tabbed);
          MessageBox.Show(Resources.InvalidWindowLayout, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          break;
      }
      _windowLayout = enumItem;
      _windowLayoutMenu.SetCheckedByValue(enumItem);
    }

    private void OpenLastOpenedFilesIfExists()
    {
      if (Settings.Default.LastOpenedFilePaths.Count > 0)
        foreach (string file in Settings.Default.LastOpenedFilePaths)
        {
          ActiveFilePath = file;
          if (ActiveFilePath != string.Empty && File.Exists(ActiveFilePath))
          {
            OpenNewFormWith(ActiveFilePath);
          }
        }
      else
        EnableAllXmlFormContextFunction(false);

      // Childs fills this...
      Settings.Default.LastOpenedFilePaths.Clear();
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

    private void AutoSizeMenu_Clicked(DataGridViewAutoSizeColumnsMode mode)
    {
      if (ActiveMdiChild is GranitXMLEditorForm)
        ActiveXmlForm.DataGrid.AutoSizeColumnsMode = mode == 0 ? DataGridViewAutoSizeColumnsMode.None : mode;
    }

    private void EnableAllXmlFormContextFunction(bool enabled = true)
    {
      saveToolStripButton.Enabled = enabled;
      saveToolStripMenuItem.Enabled = enabled;
      saveAsToolStripMenuItem.Enabled = enabled;
      undoToolStripButton.Enabled = enabled;
      undoToolStripMenuItem.Enabled = enabled;
      redoToolStripButton.Enabled = enabled;
      redoToolStripMenuItem.Enabled = enabled;
      selectAllToolStripMenuItem.Enabled = enabled;
      deleteSelectedToolStripMenuItem.Enabled = enabled;
      findAndReplaceToolStripMenuItem.Enabled = enabled;
      findToolStripButton.Enabled = enabled;
      addRowToolStripButton.Enabled = enabled;
      deleteRowToolStripButton.Enabled = enabled;
      alignTableToolStripMenuItem.Enabled = enabled;

      copyToolStripButton.Enabled = enabled;
      copyToolStripMenuItem.Enabled = enabled;
      pasteToolStripButton.Enabled = enabled;
      pasteToolStripMenuItem.Enabled = enabled;
      cutToolStripButton.Enabled = enabled;
      cutToolStripMenuItem.Enabled = enabled;

      layoutToolStripMenuItem.Enabled = enabled;
    }

    private void MostRecentMenu_Clicked(int number, string filename)
    {
      if (File.Exists(filename))
      {
        OpenNewFormWith(filename);
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

    public string GetFileNameToSaveByOpeningSaveFileDialog()
    {
      string filename = null;
      if ((formsTabControl.SelectedTab != null) && (formsTabControl.SelectedTab.Tag != null))
      {
        SaveFileDialog.InitialDirectory =
          ActiveFilePath == null ? Application.StartupPath : Path.GetDirectoryName(ActiveFilePath);
        SaveFileDialog.FileName =
          ActiveFilePath == null ? "" : Path.GetFileName(ActiveFilePath);

        if (SaveFileDialog.ShowDialog() == DialogResult.OK)
          filename = SaveFileDialog.FileName;
      }
      return filename;
    }

    private void GranitEditorMainForm_MdiChildActivate(object sender, EventArgs e)
    {
      if (ActiveMdiChild == null)
      {
        formsTabControl.Visible = false; // If no any child form, hide tabControl
        InitStatusLabels();
        EnableAllXmlFormContextFunction(false);
      }
      else
      {
        ActiveMdiChild.WindowState = FormWindowState.Maximized; // Child form always maximized
        if (ActiveMdiChild.Tag == null)
        {
          // Add a tabPage to tabControl with child form caption
          TabPage tp = new TabPage(this.ActiveMdiChild.Text)
          {
            Tag = ActiveMdiChild,
            Parent = formsTabControl,
            ToolTipText = Path.GetFullPath(ActiveXmlForm.LastOpenedFilePath)
          };
          ActiveMdiChild.Tag = tp;
          ActiveMdiChild.FormClosed += new FormClosedEventHandler(ActiveMdiChild_FormClosed);
          formsTabControl.SelectedTab = tp;
        }

        ActualizeMenuToolBarAndStatusLabels();
        SetActiveFilePath(ActiveXmlForm.LastOpenedFilePath);
        //if (!formsTabControl.Visible) formsTabControl.Visible = true;
      }
    }

    private void ActiveMdiChild_FormClosed(object sender, FormClosedEventArgs e)
    {
      ((sender as Form).Tag as TabPage).Dispose();
    }

    private void FormsTabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((formsTabControl.SelectedTab != null) && (formsTabControl.SelectedTab.Tag != null))
      {
        // Minimize flicker when switching between tabs, by suspending layout
        SuspendLayout();
        (formsTabControl.SelectedTab.Tag as Form).SuspendLayout();
        Form activeMdiChild = this.ActiveMdiChild;
        if (activeMdiChild != null)
          activeMdiChild.SuspendLayout();

        // Minimize flicker when switching between tabs, by changing to minimized state first
        if ((formsTabControl.SelectedTab.Tag as Form).WindowState != FormWindowState.Maximized)
          (formsTabControl.SelectedTab.Tag as Form).WindowState = FormWindowState.Minimized;

        (formsTabControl.SelectedTab.Tag as Form).Select();

        // Resume layout again
        if (activeMdiChild != null && !activeMdiChild.IsDisposed)
          activeMdiChild.ResumeLayout();
        (formsTabControl.SelectedTab.Tag as Form).ResumeLayout();
        ResumeLayout();
        (formsTabControl.SelectedTab.Tag as Form).Refresh();
      }

      //ActualizeMenuToolBarAndStatusLabels();
      //SetActiveFilePath(ActiveXmlForm.LastOpenedFilePath);
    }

    private void ActualizeMenuToolBarAndStatusLabels()
    {
      UpdateMenuItemsForActiveForm();
      EnableToolbarItemsForActiveForm();

      if (ActiveXmlForm != null)
      {
        ActiveXmlForm.ActualizeStatusLabelsOfAll();
        ActiveXmlForm.ActualizeStatusLabelsOfSelected();
      }
      else
      {
        InitStatusLabels();
        EnableAllXmlFormContextFunction(false);
      }
    }

    private void InitStatusLabels()
    {
      decimal amount = 0;
      SetStatusLabelItemText("selectedAmountStatus",
        string.Format("{0}{1} Ft",
        Resources.StatusSumSelectedText,
        amount.ToString(GranitXml.Constants.AmountFormatString, CultureInfo.InvariantCulture)));
      SetStatusLabelItemText("selectedStatusLabel",
       Resources.StatusCountSeletedText + "0");
      SetStatusLabelItemText("allStatusLabel",
        Resources.StatusCountAllText + "0");
      SetStatusLabelItemText("allAmountStatus",
        string.Format("{0}{1} Ft",
        Resources.StatusSumSelectedText,
        amount.ToString(GranitXml.Constants.AmountFormatString, CultureInfo.InvariantCulture)));
    }

    private void NewToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OpenNewFormWith(GetNextNewDocumentName());
    }

    public void OpenNewFormWith(string xmlFilePath)
    {
      GranitXMLEditorForm f = new GranitXMLEditorForm(xmlFilePath, ClipboardHandler)
      {
        MdiParent = this
      };
      f.Show();
      bool succeded = f.LastOpenedFilePath != null;
      if (!succeded)
        f.LastOpenedFilePath = GetNextNewDocumentName();

      ChangeWindowLayout(_windowLayout);
    }


    private void OpenGranitXmlFile()
    {
      if (OpenFileDialog.ShowDialog() == DialogResult.OK)
      {
        foreach (string file in OpenFileDialog.FileNames)
          OpenNewFormWith(file);
      }
    }

    private void OpenToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      OpenGranitXmlFile();
    }

    private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.AskAndSaveFile();
    }

    private void SaveAsToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (ActiveMdiChild is GranitXMLEditorForm)
      {
        string f = GetFileNameToSaveByOpeningSaveFileDialog();
        if (f != null)
          ActiveXmlForm.SaveDocument(f);
      }
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.Undo();
      UpdateUndoRedoItems();
    }

    private void UpdateUndoRedoItems()
    {
      var hist = ActiveXmlForm?.History;
      undoToolStripButton.Enabled = hist != null && hist.CanUndo;
      redoToolStripButton.Enabled = hist != null && hist.CanRedo;
      //undoToolStripButton.ToolTipText = hist == null ? "" : hist.UndoCount.ToString();
      //redoToolStripMenuItem.ToolTipText =  hist == null ? "" : hist.RedoCount.ToString();
    }

    public void ShowFindAndReplaceDlg()
    {
      CreateFindDialog(ActiveXmlForm.DataGrid, ActiveXmlForm.XmlToObjectBinder.GranitXDocument);

      if (FindReplaceDlg == null)
        return;

      if (!FindReplaceDlg.Visible)
      {
        if (ActiveXmlForm.DataGrid.SelectedCells.Count > 1)
        {
          FindReplaceDlg.IsSelectionChecked = true;
        }

        FindReplaceDlg.InitialSearchText = String.Empty;

        var currCell = ActiveXmlForm.DataGrid.CurrentCell;
        if (currCell != null && currCell.ValueType != typeof(bool))
        {
          if (String.IsNullOrEmpty(ActiveXmlForm.SelectedTextInCurrentCell))
          {
            FindReplaceDlg.InitialSearchText = currCell.FormattedValue.ToString();
          }
          else
          {
            FindReplaceDlg.InitialSearchText = ActiveXmlForm.SelectedTextInCurrentCell;
          }
        }

        FindReplaceDlg.Show(this);
        FindReplaceDlg.BringToFront();
      }
      else
      {
        FindReplaceDlg.Hide();
      }
    }

    public void SetStatusLabelItemText(string name, string text)
    {
      switch (name)
      {
        case "allStatusLabel":
          allStatusLabel.Text = text;
          break;
        case "allAmountStatus":
          allAmountStatus.Text = text;
          break;
        case "selectedAmountStatus":
          selectedAmountStatus.Text = text;
          break;
        case "selectedStatusLabel":
          selectedStatusLabel.Text = text;
          break;
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Debug.WriteLine("OnClosing called on MainForm. docHasPendingChanges: {0}", ActiveDocHavePendingChanges);

      //if (DocsHavePendingChanges || ActiveFilePath == string.Empty)
      //  e.Cancel = AskAndSaveFiles(MessageBoxButtons.YesNoCancel) == DialogResult.Cancel;

      //if (!e.Cancel)
      SaveSettings();

      base.OnClosing(e);
    }

    public FindReplaceDlg CreateFindDialog(DataGridView dgv, XDocument doc)
    {
      if (ActiveMdiChild is GranitXMLEditorForm)
      {
        if (FindReplaceDlg == null)
          FindReplaceDlg = new FindReplaceDlg(ActiveXmlForm.DataGrid, ActiveXmlForm.XmlToObjectBinder.GranitXDocument);
        else if (FindReplaceDlg.IsDisposed)
          FindReplaceDlg = new FindReplaceDlg(ActiveXmlForm.DataGrid, ActiveXmlForm.XmlToObjectBinder.GranitXDocument);
        else
        {
          FindReplaceDlg.DataGrid = dgv;
          FindReplaceDlg.XmlDoc = doc;
        }

        return FindReplaceDlg;
      }
      return null;
    }

    //private DialogResult AskAndSaveFiles(MessageBoxButtons yesNoCancel)
    //{
    //  DialogResult result = DialogResult.OK;
    //  while (ActiveMdiChild != null)
    //  {
    //    if (ActiveMdiChild is GranitXMLEditorForm)
    //      result = ActiveXmlForm.AskAndSaveFile();

    //    if (result == DialogResult.Cancel) break;

    //    ActiveMdiChild.Close();
    //  }
    //  return result;
    //}

    private void SaveSettings()
    {
      string layout = _windowLayoutMenu?.CheckedMenuItem?.Tag.ToString();
      Settings.Default.WindowLayout = layout ?? "";

      Settings.Default.RecentFileList = new StringCollection();
      Settings.Default.RecentFileList.AddRange(_mruMenu.GetFiles());
      List<string> paths = new List<string>();
      foreach (var f in MdiChildren)
      {
        if (f is GranitXMLEditorForm)
          paths.Add((f as GranitXMLEditorForm).LastOpenedFilePath);
      }
      Settings.Default.LastOpenedFilePaths = new StringCollection();
      Settings.Default.LastOpenedFilePaths.AddRange(paths.ToArray());

      Settings.Default.Save();
    }

    private void ApplySettings()
    {
      _mruMenu.MaxShortenPathLength = Settings.Default.MruListItemLength;
      if (Settings.Default.RecentFileList != null)
        foreach (string item in Settings.Default.RecentFileList)
        {
          _mruMenu.AddFile(item);
        }

      _windowLayout = WindowLayout.Tabbed;
      string layout = Settings.Default.WindowLayout;
      if (layout != null && layout != string.Empty)
        _ = Enum.TryParse<WindowLayout>(layout, out _windowLayout);

    }


    private void EditToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
    {
      UpdateMenuItemsForActiveForm();
    }

    private void UpdateMenuItemsForActiveForm()
    {
      UpdateCopyPasteItems();
      UpdateSaveAndSaveAsItems();

      undoToolStripMenuItem.Enabled = ActiveXmlForm != null && ActiveXmlForm.History.CanUndo;
      redoToolStripMenuItem.Enabled = ActiveXmlForm != null && ActiveXmlForm.History.CanRedo;
      selectAllToolStripMenuItem.Enabled = ActiveXmlForm != null;
      deleteSelectedToolStripMenuItem.Enabled = ActiveXmlForm != null;
      findAndReplaceToolStripMenuItem.Enabled = ActiveXmlForm != null;

      if (ActiveXmlForm != null)
        GridAlignMenu.SetCheckedByValue(ActiveXmlForm.DataGrid.AutoSizeColumnsMode);

      layoutToolStripMenuItem.Enabled = ActiveXmlForm != null;
    }

    public void UpdateCopyPasteItems()
    {
      copyToolStripMenuItem.Enabled = ActiveXmlForm != null && ActiveXmlForm.HasSelectedCells;
      cutToolStripMenuItem.Enabled = ActiveXmlForm != null && ActiveXmlForm.HasSelectedCells;
      pasteToolStripMenuItem.Enabled = ActiveXmlForm != null && ActiveXmlForm.HasSelectedCells && ActiveXmlForm.ClipboardHandler.ClipboardHasContent;

      copyToolStripButton.Enabled = copyToolStripMenuItem.Enabled;
      cutToolStripButton.Enabled = cutToolStripMenuItem.Enabled;
      pasteToolStripButton.Enabled = pasteToolStripMenuItem.Enabled;

      ActiveXmlForm?.ContextMenuHandler.EnableMenuItem("copyToolStripMenuItem", copyToolStripMenuItem.Enabled);
      ActiveXmlForm?.ContextMenuHandler.EnableMenuItem("cutToolStripMenuItem", cutToolStripMenuItem.Enabled);
      ActiveXmlForm?.ContextMenuHandler.EnableMenuItem("pasteToolStripMenuItem", pasteToolStripMenuItem.Enabled);
    }

    public void UpdateSaveAndSaveAsItems()
    {
      saveAsToolStripMenuItem.Enabled = ActiveXmlForm != null;
      saveToolStripButton.Enabled = ActiveDocHavePendingChanges; 
      saveToolStripMenuItem.Enabled = saveToolStripButton.Enabled;
    }

    private void EnableToolbarItemsForActiveForm()
    {
      UpdateCopyPasteItems();
      UpdateUndoRedoItems();
      addRowToolStripButton.Enabled = ActiveXmlForm != null;
      deleteRowToolStripButton.Enabled = ActiveXmlForm != null;
      findToolStripButton.Enabled = ActiveXmlForm != null;
    }

    private void ToolsToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
    {
      alignTableToolStripMenuItem.Enabled = ActiveXmlForm != null;
    }

    private void FileToolStripMenuItem1_DropDownOpened(object sender, EventArgs e)
    {
      //saveToolStripMenuItem.Enabled = saveToolStripButton.Enabled;
    }

    private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.Redo();
      UpdateUndoRedoItems();
    }

    private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.SelectAll();
    }

    private void DeleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.ContextMenuHandler.Grid_DeleteSelectedRows(sender, e);
    }

    private void FindAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ShowFindAndReplaceDlg();
    }

    private void NewToolStripButton_Click(object sender, EventArgs e)
    {
      NewToolStripMenuItem_Click(sender, e);
    }

    private void OpenToolStripButton_Click(object sender, EventArgs e)
    {
      OpenToolStripMenuItem1_Click(sender, e);
    }

    private void SaveToolStripButton_Click(object sender, EventArgs e)
    {
      SaveToolStripMenuItem_Click(sender, e);
    }

    private void FindToolStripButton_Click(object sender, EventArgs e)
    {
      FindAndReplaceToolStripMenuItem_Click(sender, e);
    }

    private void UndoToolStripButton_Click(object sender, EventArgs e)
    {
      UndoToolStripMenuItem_Click(sender, e);
    }

    private void RedoToolStripButton_Click(object sender, EventArgs e)
    {
      RedoToolStripMenuItem_Click(sender, e);
    }

    private void HelpToolStripButton_Click(object sender, EventArgs e)
    {
      About();
    }

    private void AboutToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      About();
    }

    private void GranitEditorMainForm_DragDrop(object sender, DragEventArgs e)
    {
      string[] files = (string[])(e.Data.GetData(DataFormats.FileDrop, false));
      OpenNewFormWith(Path.GetFullPath(files[0]).ToString());
    }

    private void GranitEditorMainForm_DragEnter(object sender, DragEventArgs e)
    {
      Debug.WriteLine("DragEnter");
      string filename = GetDropFileName(e);
      if (filename != string.Empty)
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    public static string GetDropFileName(DragEventArgs e)
    {
      string fileName = string.Empty;

      if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
      {
        if (((IDataObject)e.Data).GetData("FileName") is Array data)
        {
          if ((data.Length == 1) && (data.GetValue(0) is string))
          {
            fileName = ((string[])data)[0];
            string ext = Path.GetExtension(fileName).ToLower();
            if ((ext == ".xml"))//|| (ext == ".png") || (ext == ".bmp"))
            {
              return fileName;
            }
          }
        }
      }
      return fileName;
    }

    private void GranitEditorMainForm_Shown(object sender, EventArgs e)
    {
      ChangeWindowLayout(_windowLayout);
    }

    private void CutToolStripButton_Click(object sender, EventArgs e)
    {
      Cut();
    }

    public void Cut()
    {
      //Copy to clipboard
      Copy();

      //Clear selected cells
      foreach (DataGridViewCell dgvCell in ActiveXmlForm.DataGrid.SelectedCells)
        dgvCell.Value = string.Empty;
    }

    private void CopyToolStripButton_Click(object sender, EventArgs e)
    {
      Copy();
    }

    public void Copy()
    {
      _clipboardHandler.CopyToClipboard(ActiveXmlForm.DataGrid);
      UpdateCopyPasteItems();
    }

    private void PasteToolStripButton_Click(object sender, EventArgs e)
    {
      Paste();
    }

    public void Paste()
    {
      ActiveXmlForm?.History.BeginCompoundDo();
      _clipboardHandler.PasteClipboardValue(ActiveXmlForm.DataGrid);
      ActiveXmlForm?.History.EndCompoundDo();
      UpdateToolbarItems();
    }

    private void CutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CutToolStripButton_Click(sender, e);
      UpdateCopyPasteItems();
    }

    private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CopyToolStripButton_Click(sender, e);
    }

    private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      PasteToolStripButton_Click(sender, e);
    }

    private void AddRowToolStripButton_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.History?.Do(new TransactionPoolMemento(ActiveXmlForm.XmlToObjectBinder.GranitXDocument));
      ActiveXmlForm?.ContextMenuHandler.AddNewEmptyRow();
    }

    private void DeleteRowToolStripButton_Click(object sender, EventArgs e)
    {
      //ActiveXmlForm?.History?.Do(new TransactionPoolMemento(ActiveXmlForm.XmlToObjectBinder.GranitXDocument));
      ActiveXmlForm?.DeleteRowToolStripMenuItem_Click(sender, e);
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
        if (_aboutBox != null)
          _aboutBox.Dispose();
        if(_openFileDialog != null)
          _openFileDialog.Dispose();
        if(_saveFileDialog != null)
        _saveFileDialog.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}
