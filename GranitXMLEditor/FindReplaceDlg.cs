using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using GranitXMLEditor.Properties;

namespace GranitXMLEditor
{
  public partial class FindReplaceDlg : Form
  {
    DataGridView dgv;

    public FindReplaceDlg(DataGridView dataGrdView)
      : this()
    {
      dgv = dataGrdView;
    }

    public FindReplaceDlg()
    {
      InitializeComponent();
      IsFirstInitNecessary = true;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      e.Cancel = true;
      base.OnClosing(e);
      Hide();
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      Hide();
    }

    private bool _oneOccuranceFound = false;
    private bool _allreadyAsked = false;
    private List<DataGridViewCell> _cellsToSearch;
    private int _cellsToSearchNextIndex = 0;
    private int _cellsToSearchEndIndex = 0;
    private Regex _regexToSearch;

    public bool IsFirstInitNecessary { get; set; }
    public bool IsSelectionChecked {
      get { return selectionRadioButton.Checked; }
      set { selectionRadioButton.Checked = value; }
    }

    internal DataGridView DataGrid
    {
      get
      {
        return dgv;
      }

      set
      {
        dgv = value;
      }
    }

    private void findButton_Click(object sender, EventArgs e)
    {
      AddToRecentSearches();
      FindAndSelectMatchingCell(false);
    }

    private void AddToRecentSearches()
    {
      if(!findComboBox.Items.Contains(findComboBox.Text.Trim()))
        findComboBox.Items.Add(findComboBox.Text.Trim());
      if(!replaceComboBox.Items.Contains(replaceComboBox.Text.Trim()))
        replaceComboBox.Items.Add(replaceComboBox.Text.Trim());
    }

    private Match FindAndSelectMatchingCell(bool calledFromReplace)
    {
      FirstInitializationIfNeeded();

      Match match = _regexToSearch.Match("");
      do
      {
        match = SelectNextMatchingCell(_regexToSearch, _allreadyAsked, ref _cellsToSearchNextIndex);

        bool notFound = !match.Success;
        bool endReached = _cellsToSearchNextIndex < 0;

        if (!calledFromReplace && ( endReached || notFound) || 
            (calledFromReplace && ( endReached || notFound) && !_oneOccuranceFound && !_allreadyAsked) // Don't ask in case of replace all, if something replaced
          //&& _cellsToSearchEndIndex == (downRadioButton.Checked ? 0 : (upRadioButton.Checked ? _cellsToSearch.Count - 1 : 0))
          )
        {
          DialogResult answer = ShowMessageBox(endReached && !_oneOccuranceFound);

          if (calledFromReplace)
            _allreadyAsked = true;

          if (answer == DialogResult.Yes)
          {
            _cellsToSearch = GetCellsToSearchList();
            _regexToSearch = CreateFindRegex();
            _cellsToSearchNextIndex = 0; //downRadioButton.Checked ? 0 : (upRadioButton.Checked ? _cellsToSearch.Count - 1 : 0);
            _cellsToSearchEndIndex = _cellsToSearchNextIndex;
            continue;
          }
          else
          {
            ResetOriginalSelection();
            IsFirstInitNecessary = true;
            break;
          }
        }

        if (match.Success)
          _oneOccuranceFound = true;

        break;
      }
      while (true);

      return match;
    }

    private DialogResult ShowMessageBox(bool nothingFoundAtAll)
    {
      string text = nothingFoundAtAll ? string.Format(Resources.NotFoundMsgBoxText, findComboBox.Text)
        : upRadioButton.Checked ? Resources.FirstRowMsgBoxText : Resources.LastRowMsgBoxText;

      var answer = MessageBox.Show(text, Application.ProductName,
        nothingFoundAtAll ? MessageBoxButtons.OK : MessageBoxButtons.YesNo,
        nothingFoundAtAll ? MessageBoxIcon.Information : MessageBoxIcon.Question);
      return answer;
    }

    private void ResetOriginalSelection()
    {
      if (selectionRadioButton.Checked)
      {
        ResetDgvState();
        var selectedCells = GetCellsToSearchList();
        foreach (var item in selectedCells)
        {
          if (item.IsInEditMode)
          {
            if(dgv.CancelEdit())
              dgv.EndEdit();
          }
          item.Selected = true;
        }
      }
    }

    private void FirstInitializationIfNeeded()
    {
      if (IsFirstInitNecessary)
      {
        _cellsToSearch = GetCellsToSearchList();
        _regexToSearch = CreateFindRegex();
        _cellsToSearchNextIndex = HasSelectedCells() ? 0 : GetActiveCellIndex(_cellsToSearch);
        _cellsToSearchEndIndex = _cellsToSearchNextIndex;
        _oneOccuranceFound = false;
      }

      IsFirstInitNecessary = false;
    }

    private List<DataGridViewCell> GetCellsToSearchList()
    {
      List<DataGridViewCell> cells = null;
      if (IsFirstInitNecessary)
      {
        if (selectionRadioButton.Checked)
        {
          DataGridViewCell[] cellArrayToSearch = new DataGridViewCell[dgv.SelectedCells.Count];
          dgv.SelectedCells.CopyTo(cellArrayToSearch, 0);
          cells = cellArrayToSearch.ToList();
          cells.Reverse();
        }
        else if (downRadioButton.Checked)
        {
          cells = GetAllCells();
        }
        else if (upRadioButton.Checked)
        {
          cells = GetAllCells();
          cells.Reverse();
        }
      }

      return cells == null ? _cellsToSearch : cells;
    }

    private bool HasSelectedCells()
    {
      return selectionRadioButton.Checked;
    }

    private int GetActiveCellIndex(List<DataGridViewCell> cellsToSearch)
    {
      int i = 0;
      foreach (DataGridViewCell cell in cellsToSearch)
      {
        if (dgv.CurrentCell == cell)
          break;
        i++;
      }
      return i;
    }

    private List<DataGridViewCell> GetAllCells()
    {
      var cells = new List<DataGridViewCell>();
      foreach (DataGridViewRow row in dgv.Rows)
      {
        foreach (DataGridViewCell cell in row.Cells)
        {
          cells.Add(cell);
        }
      }
      return cells;
    }

    private Match SelectNextMatchingCell(Regex regex, bool againFromTheBeginning, ref int nextIndex)
    {

      Match match = regex.Match("");
      int startIndex = againFromTheBeginning ? 0 : nextIndex; 
      int endIndex = againFromTheBeginning ? 
        (startIndex == 0 && _cellsToSearchEndIndex == 0 ? _cellsToSearch.Count : _cellsToSearchEndIndex) : _cellsToSearch.Count;

      if (startIndex >= 0)
      {
        for (int i = startIndex; i < endIndex; i++)
        {
          var cell = _cellsToSearch[i];
          match = SelectMatchedTextInCell(cell, regex);
          if (match.Success)
          {
            nextIndex = i + 1;
            return match;
          }
        }
      }
      nextIndex = -1;
      return match;
    }

    private Match SelectMatchedTextInCell(DataGridViewCell cell, Regex regex)
    {
      Match match = regex.Match("");
      if (typeof(DataGridViewTextBoxCell) == cell.GetType())
      {
        var tbCell = (DataGridViewTextBoxCell)cell;

        if (tbCell.Value == null 
          || tbCell.Value.ToString() == string.Empty)
          return match;

        match = regex.Match(tbCell.Value.ToString());
        if (match.Success)
        {
          ResetDgvState();
          SelectTextInCell(tbCell, match);
        }
      }
      return match;
    }

    private void SelectTextInCell(DataGridViewTextBoxCell tbCell, Match match)
    {
      dgv.CurrentCell = tbCell;
      dgv.BeginEdit(false);
      ((TextBox)dgv.EditingControl).SelectionStart = match.Index;
      ((TextBox)dgv.EditingControl).SelectionLength = match.Length;
      dgv.Parent.Parent.Select(); //Activate parent, to show the selection in the grid
    }

    private Regex CreateFindRegex()
    {
      Regex result;
      String regExString;

      // Get what the user entered
      regExString = findComboBox.Text;

      if (useRegexpCheckBox.Checked)
      {
        // If regular expressions checkbox is selected,
        // our job is easy. Just do nothing
      }
      //else if (useWildcardsCheckBox.Checked) // wild cards checkbox checked
      //{
      //  regExString = regExString.Replace("*", @"\w*");     // multiple characters wildcard (*)
      //  regExString = regExString.Replace("?", @"\w");      // single character wildcard (?)
      //  // if wild cards selected, find whole words only
      //  regExString = String.Format("{0}{1}{0}", @"\b", regExString);
      //}
      else
      {
        // replace escape characters
        regExString = Regex.Escape(regExString);
      }

      // Is whole word check box checked?
      if (matchWholeWordsCheckBox.Checked)
      {
        regExString = String.Format("{0}{1}{0}", @"\b", regExString);
      }

      // Is match case checkbox checked or not?
      if (matchCaseCheckBox.Checked)
      {
        result = new Regex(regExString);
      }
      else
      {
        result = new Regex(regExString, RegexOptions.IgnoreCase);
      }

      return result;
    }

    private void findComboBox_TextChanged(object sender, EventArgs e)
    {
      IsFirstInitNecessary = true;
    }

    private void replaceButton_Click(object sender, EventArgs e)
    {
      AddToRecentSearches();
      FindNextAndReplace();
    }

    private void FindNextAndReplace()
    {
        Match m = null; 
        if (dgv.EditingControl == null)
          m = FindAndSelectMatchingCell(true);

        if (dgv.EditingControl == null)
        {
          IsFirstInitNecessary = true;
          return;
        }
        ReplaceCellText(m);
    }

    private void ReplaceCellText(Match match)
    {
      dgv.BeginEdit(false);
      string text = dgv.EditingControl.Text;
      dgv.EditingControl.Text = _regexToSearch.Replace(text, replaceComboBox.Text);
      dgv.EndEdit();
      ResetDgvState();
    }

    private void FindReplaceDlg_VisibleChanged(object sender, EventArgs e)
    {
      if (Visible == true)
        IsFirstInitNecessary = true;
    }

    private void replaceAllButton_Click(object sender, EventArgs e)
    {

      AddToRecentSearches();
      FirstInitializationIfNeeded();

      ResetDgvState();
      var matchingCells = _cellsToSearch
        .Where(c => c.Value != null && _regexToSearch.Match(c.Value.ToString()).Success).ToList();

      if (matchingCells.Count == 0)
        ShowMessageBox(true);
      else
        for (int i = 0; i < matchingCells.Count; i++)
        {
          Match match = SelectMatchedTextInCell(matchingCells[i], _regexToSearch);
          if (match.Success)
          {
            ReplaceCellText(match);
          }
        }
    }

    private void ResetDgvState()
    {
      dgv.CancelEdit();
      dgv.EndEdit();
      dgv.ClearSelection();
    }

    private void matchWholeWordsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      IsFirstInitNecessary = true;
    }

    private void matchCaseCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      IsFirstInitNecessary = true;
    }

    private void useRegexpCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      IsFirstInitNecessary = true;
    }

    private void upRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      IsFirstInitNecessary = true;
    }

    private void selectionRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      IsFirstInitNecessary = true;
    }
  }
}
