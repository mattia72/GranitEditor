using GranitXMLEditor.Properties;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GranitXMLEditor
{
  public partial class GranitEditorMainForm : Form
  {
    private AboutBox _aboutBox;
    private MruStripMenu _mruMenu;
    private string _lastOpenedFilePath;
    private FindReplaceDlg _findReplaceDlg;
    private EnumStripMenu<DataGridViewAutoSizeColumnsMode> _gridAlignMenu;
    private bool _docsHavePendingChanges = false;
    private OpenFileDialog _openFileDialog;
    private SaveFileDialog _saveFileDialog;

    public GranitXMLEditorForm ActiveXmlForm
    {
      get
      {
        return ActiveMdiChild as GranitXMLEditorForm;
      }
    }

    public string LastOpenedFilePath
    {
      get { return _lastOpenedFilePath; }
      set
      {
        _lastOpenedFilePath = value;
        var filePath = _lastOpenedFilePath == string.Empty ? GetNextNewDocumentName() : _lastOpenedFilePath;

        Text = Path.GetFullPath(filePath) + " - " + Application.ProductName;

        if (!string.IsNullOrEmpty(_lastOpenedFilePath))
          _mruMenu.AddFile(_lastOpenedFilePath);
      }
    }

    public string GetNextNewDocumentName()
    {
      string newName;
      int i = 1;
      bool found = false;
      do
      {
        found = false;
        newName = string.Format(Resources.NewDocumentName, i++);
        foreach (var f in MdiChildren)
        {
          if (f is GranitXMLEditorForm)
          {
            if ((f as GranitXMLEditorForm).LastOpenedFilePath.EndsWith(newName))
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

    public void SetLastOpenedFilePath(string value)
    {
      LastOpenedFilePath = value;
      (ActiveXmlForm?.Tag as TabPage).Text = Path.GetFileName(value);
    }

    public bool DocsHavePendingChanges
    {
      get
      {
        return _docsHavePendingChanges;
      }
      set
      {
        _docsHavePendingChanges = value;
        saveToolStripButton.Enabled = _docsHavePendingChanges;
        saveToolStripMenuItem.Enabled = _docsHavePendingChanges;
        EnableUndoRedoItems();
      }
    }

    public void SetDocsHavePendingChanges(bool value)
    {
      DocsHavePendingChanges = value;
    }

    public EnumStripMenu<DataGridViewAutoSizeColumnsMode> GridAlignMenu
    {
      get
      {
        return _gridAlignMenu;
      }

      set
      {
        _gridAlignMenu = value;
      }
    }

    public GranitEditorMainForm()
    {
      InitializeComponent();
      _mruMenu = new MruStripMenu(recentFilesToolStripMenuItem, MostRecentMenu_Clicked, 10);
      _gridAlignMenu = new EnumStripMenu<DataGridViewAutoSizeColumnsMode>(alignTableToolStripMenuItem, autoSizeMenu_Clicked);
      ApplySettings();
    }

    private void OpenLastOpenedFilesIfExists()
    {
      if (Settings.Default.LastOpenedFilePaths.Count > 0)
        foreach (string file in Settings.Default.LastOpenedFilePaths)
        {
          LastOpenedFilePath = file;
          if (LastOpenedFilePath != string.Empty && File.Exists(LastOpenedFilePath))
            CreateNewForm(LastOpenedFilePath);
        }
      else
        EnableAllXmlFormContextFunction(false);
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

    private void autoSizeMenu_Clicked(DataGridViewAutoSizeColumnsMode mode)
    {
      if (ActiveMdiChild is GranitXMLEditorForm)
        ActiveXmlForm.DataGrid.AutoSizeColumnsMode = mode == 0 ? DataGridViewAutoSizeColumnsMode.None : mode;
    }

    private void EnableAllXmlFormContextFunction(bool enabled = true)
    {
      saveToolStripButton.Enabled = enabled;
      saveToolStripMenuItem.Enabled = enabled;
      saveAsToolStripMenuItem1.Enabled = enabled;
      undoToolStripButton.Enabled = enabled;
      undoToolStripMenuItem.Enabled = enabled;
      redoToolStripButton.Enabled = enabled;
      redoToolStripMenuItem.Enabled = enabled;
      selectAllToolStripMenuItem.Enabled = enabled;
      deleteSelectedToolStripMenuItem.Enabled = enabled;
      findAndReplaceToolStripMenuItem.Enabled = enabled;
      findToolStripButton.Enabled = enabled;
      alignTableToolStripMenuItem.Enabled = enabled;

      copyToolStripButton.Enabled = enabled;
      copyToolStripMenuItem.Enabled = enabled;
      pasteToolStripButton.Enabled = enabled;
      pasteToolStripMenuItem.Enabled = enabled;
      cutToolStripButton.Enabled = enabled;
      cutToolStripMenuItem.Enabled = enabled;

      cascadeToolStripMenuItem.Enabled = enabled;
      tileHorizontalyToolStripMenuItem.Enabled = enabled;
      tileVerticallyToolStripMenuItem.Enabled = enabled; 
    }

    private void ApplySettings()
    {
      OpenLastOpenedFilesIfExists();
      _mruMenu.MaxShortenPathLength = Settings.Default.MruListItemLength;
      if (Settings.Default.RecentFileList != null)
        foreach (string item in Settings.Default.RecentFileList)
        {
          _mruMenu.AddFile(item);
        }
    }

    private void MostRecentMenu_Clicked(int number, string filename)
    {
      if (File.Exists(filename))
      {
        CreateNewForm(filename);
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

    private string GetFileNameToSaveByOpeningSaveFileDialog()
    {
      string filename = null;
      if ((tabForms.SelectedTab != null) && (tabForms.SelectedTab.Tag != null))
      {
        _saveFileDialog = _saveFileDialog == null ? new SaveFileDialog() : _saveFileDialog;
        _saveFileDialog.InitialDirectory =
          LastOpenedFilePath == null ? Application.StartupPath : Path.GetDirectoryName(LastOpenedFilePath);
        _saveFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
        _saveFileDialog.FilterIndex = 1;
        _saveFileDialog.RestoreDirectory = true;

        if (_saveFileDialog.ShowDialog() == DialogResult.OK)
          filename = _saveFileDialog.FileName;
      }
      return filename;
    }

    private void GranitEditorMainForm_MdiChildActivate(object sender, EventArgs e)
    {
      if (this.ActiveMdiChild == null)
      {
        tabForms.Visible = false; // If no any child form, hide tabControl
        InitStatusLabels();
        EnableAllXmlFormContextFunction(false);
      }
      else
      {
        this.ActiveMdiChild.WindowState = FormWindowState.Maximized; // Child form always maximized

        // If child form is new and no has tabPage, create new tabPage
        if (this.ActiveMdiChild.Tag == null)
        {
          // Add a tabPage to tabControl with child form caption
          TabPage tp = new TabPage(this.ActiveMdiChild.Text);
          tp.Tag = this.ActiveMdiChild;
          tp.Parent = tabForms;
          tabForms.SelectedTab = tp;

          this.ActiveMdiChild.Tag = tp;
          this.ActiveMdiChild.FormClosed += new FormClosedEventHandler(ActiveMdiChild_FormClosed);
        }

        if (!tabForms.Visible) tabForms.Visible = true;
      }
    }

    private void ActiveMdiChild_FormClosed(object sender, FormClosedEventArgs e)
    {
      ((sender as Form).Tag as TabPage).Dispose();
    }

    private void tabForms_SelectedIndexChanged(object sender, EventArgs e)
    {
      if ((tabForms.SelectedTab != null) && (tabForms.SelectedTab.Tag != null))
      {
        // Minimize flicker when switching between tabs, by suspending layout
        SuspendLayout();
        (tabForms.SelectedTab.Tag as Form).SuspendLayout();
        Form activeMdiChild = this.ActiveMdiChild;
        if (activeMdiChild != null)
          activeMdiChild.SuspendLayout();

        // Minimize flicker when switching between tabs, by changing to minimized state first
        if ((tabForms.SelectedTab.Tag as Form).WindowState != FormWindowState.Maximized)
          (tabForms.SelectedTab.Tag as Form).WindowState = FormWindowState.Minimized;

        (tabForms.SelectedTab.Tag as Form).Select();

        // Resume layout again
        if (activeMdiChild != null && !activeMdiChild.IsDisposed)
          activeMdiChild.ResumeLayout();
        (tabForms.SelectedTab.Tag as Form).ResumeLayout();
        ResumeLayout();
        (tabForms.SelectedTab.Tag as Form).Refresh();
      }

      ActualizeMenuToolBarAndStatusLabels();

    }

    private void ActualizeMenuToolBarAndStatusLabels()
    {
      EnableMenuItemsForActiveForm();
      EnableToolBoxItemsForActiveForm();

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
      SetStatusLabelItemText("selectedAmountStatus",
        Resources.StatusSumSelectedText + "0.00 Ft");
      SetStatusLabelItemText("selectedStatusLabel",
       Resources.StatusCountSeletedText + "0");
      SetStatusLabelItemText("allStatusLabel",
        Resources.StatusCountAllText + "0");
      SetStatusLabelItemText("allAmountStatus",
        Resources.StatusSumAllText + "0.00 Ft");
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CreateNewForm(GetNextNewDocumentName());
    }

    private void CreateNewForm(string xmlFilePath)
    {
      GranitXMLEditorForm f = new GranitXMLEditorForm(xmlFilePath);
      f.MdiParent = this;
      f.Show();
    }


    private void OpenGranitXmlFile()
    {
      _openFileDialog = _openFileDialog == null ? new OpenFileDialog() : _openFileDialog;
      _openFileDialog.InitialDirectory = Application.StartupPath;
      _openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
      _openFileDialog.FilterIndex = 1;
      _openFileDialog.RestoreDirectory = true;

      if (_openFileDialog.ShowDialog() == DialogResult.OK)
      {
        GranitXMLEditorForm f = new GranitXMLEditorForm(_openFileDialog.FileName);
        f.MdiParent = this;
        f.Show();
      }
    }
    private void openToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      OpenGranitXmlFile();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.AskAndSaveFile();
    }

    private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      if (ActiveMdiChild is GranitXMLEditorForm)
      {
        string f = GetFileNameToSaveByOpeningSaveFileDialog();
        if (f != null)
          ActiveXmlForm.SaveDocument(f);
      }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void undoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.Undo();
      EnableUndoRedoItems();
    }

    private void EnableUndoRedoItems()
    {
      var hist = ActiveXmlForm?.History;
      undoToolStripButton.Enabled = hist == null ? false : hist.CanUndo;
      redoToolStripButton.Enabled = hist == null ? false : hist.CanRedo;
    }

    public void ShowFindAndReplaceDlg()
    {
      CreateFindDialog();

      if (_findReplaceDlg == null)
        return;

      if (!_findReplaceDlg.Visible)
      {
        if (ActiveXmlForm.DataGrid.SelectedCells.Count > 1)
          _findReplaceDlg.IsSelectionChecked = true;

        _findReplaceDlg.Show(this);
        _findReplaceDlg.BringToFront();
      }
      else
      {
        _findReplaceDlg.Hide();
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

    public void SetToolBarItemEnabled(string name, bool enabled)
    {
      switch (name)
      {
        case "saveToolStripButton":
          saveToolStripButton.Enabled = enabled;
          break;
        case "cutToolStripButton":
          cutToolStripButton.Enabled = enabled;
          break;
        case "copyToolStripButton":
          copyToolStripButton.Enabled = enabled;
          break;
        case "pasteToolStripButton":
          pasteToolStripButton.Enabled = enabled;
          break;
        case "findToolStripButton":
          findToolStripButton.Enabled = enabled;
          break;
        case "undoToolStripButton":
          undoToolStripButton.Enabled = enabled;
          break;
        case "redoToolStripButton":
          redoToolStripButton.Enabled = enabled;
          break;
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      Debug.WriteLine("OnClosing called. docHasPendingChanges: {0}", DocsHavePendingChanges);

      if (DocsHavePendingChanges || LastOpenedFilePath == string.Empty)
        e.Cancel = AskAndSaveFiles(MessageBoxButtons.YesNoCancel) == DialogResult.Cancel;

      if (!e.Cancel)
        SaveSettings();

      base.OnClosing(e);
    }

    private void CreateFindDialog()
    {
      if (ActiveMdiChild is GranitXMLEditorForm)
      {
        if (_findReplaceDlg == null)
          _findReplaceDlg = new FindReplaceDlg(ActiveXmlForm.DataGrid);

        if (_findReplaceDlg.IsDisposed)
          _findReplaceDlg = new FindReplaceDlg(ActiveXmlForm.DataGrid);
      }
    }

    private DialogResult AskAndSaveFiles(MessageBoxButtons yesNoCancel)
    {
      DialogResult result = DialogResult.OK;
      while (ActiveMdiChild != null)
      {
        if (ActiveMdiChild is GranitXMLEditorForm)
          result = ActiveXmlForm.AskAndSaveFile();

        if (result == DialogResult.Cancel) break;

        ActiveMdiChild.Close();
      }
      return result;
    }

    private void SaveSettings()
    {
      if (ActiveXmlForm != null)
        Settings.Default.AlignTable = ActiveXmlForm.DataGrid.AutoSizeColumnsMode;

      var lastOpenedPaths = 
        MdiChildren.Select(f => f is GranitXMLEditorForm ? (f as GranitXMLEditorForm).LastOpenedFilePath : "").ToArray();

      FillSettingsList(Settings.Default.LastOpenedFilePaths, lastOpenedPaths);
      FillSettingsList(Settings.Default.RecentFileList, _mruMenu.GetFiles());

      Settings.Default.Save();
    }

    private void FillSettingsList(StringCollection settingList, string[] values)
    {
      if (settingList != null)
        settingList.Clear();
      else
        settingList = new StringCollection();

      settingList.AddRange(values);
    }

    private void editToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
    {
      EnableMenuItemsForActiveForm();
    }

    private void EnableMenuItemsForActiveForm()
    {
      saveToolStripMenuItem.Enabled = ActiveXmlForm == null ? false : ActiveXmlForm.DocHasPendingChanges;
      undoToolStripMenuItem.Enabled = ActiveXmlForm == null ? false : ActiveXmlForm.History.CanUndo;
      redoToolStripMenuItem.Enabled = ActiveXmlForm == null ? false : ActiveXmlForm.History.CanRedo;
      selectAllToolStripMenuItem.Enabled = ActiveXmlForm != null;
      deleteSelectedToolStripMenuItem.Enabled = ActiveXmlForm != null;
      findAndReplaceToolStripMenuItem.Enabled = ActiveXmlForm != null;

      cascadeToolStripMenuItem.Enabled = ActiveXmlForm != null;
      tileHorizontalyToolStripMenuItem.Enabled =ActiveXmlForm != null;
      tileVerticallyToolStripMenuItem.Enabled = ActiveXmlForm != null;
    }

    private void EnableToolBoxItemsForActiveForm()
    {
      saveToolStripButton.Enabled = ActiveXmlForm == null ? false : ActiveXmlForm.DocHasPendingChanges;
      EnableUndoRedoItems();
      findToolStripButton.Enabled = ActiveXmlForm != null;
    }

    private void toolsToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
    {
      alignTableToolStripMenuItem.Enabled = ActiveXmlForm == null;
    }

    private void fileToolStripMenuItem1_DropDownOpened(object sender, EventArgs e)
    {
      saveToolStripMenuItem.Enabled = ActiveXmlForm == null ? false : ActiveXmlForm.DocHasPendingChanges;
    }

    private void redoToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.Redo();
      EnableUndoRedoItems();
    }

    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.SelectAll();
    }

    private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.ContextMenuHandler.grid_DeleteSelectedRows(sender, e);
    }

    private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ActiveXmlForm?.ShowFindAndReplaceDlg();
    }

    private void newToolStripButton_Click(object sender, EventArgs e)
    {
      newToolStripMenuItem_Click(sender, e);
    }

    private void openToolStripButton_Click(object sender, EventArgs e)
    {
      openToolStripMenuItem1_Click(sender, e);
    }

    private void saveToolStripButton_Click(object sender, EventArgs e)
    {
      saveAsToolStripMenuItem1_Click(sender, e);
    }

    private void findToolStripButton_Click(object sender, EventArgs e)
    {
      findAndReplaceToolStripMenuItem_Click(sender, e);
    }

    private void undoToolStripButton_Click(object sender, EventArgs e)
    {
      undoToolStripMenuItem_Click(sender, e);
    }

    private void redoToolStripButton_Click(object sender, EventArgs e)
    {
      redoToolStripMenuItem_Click(sender, e);
    }

    private void helpToolStripButton_Click(object sender, EventArgs e)
    {
      About();
    }

    private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
    {
      About();
    }

    private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LayoutMdi(MdiLayout.Cascade);
    }

    private void tileHorizontalyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LayoutMdi(MdiLayout.TileHorizontal);
    }

    private void tileVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LayoutMdi(MdiLayout.TileVertical);
    }
  }
}
