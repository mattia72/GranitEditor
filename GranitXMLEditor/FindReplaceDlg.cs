using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

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

    private void cancelButton_Click(object sender, EventArgs e)
    {
      Hide();
    }

    private bool oneOccuranceFound = false;
    private List<DataGridViewCell> cellsToSearch;
    private int cellsToSearchNextIndex = 0;
    private int cellsToSearchStartIndex = 0;
    private Regex regexToSearch;

    public bool IsFirstInitNecessary { get; set; }

    private void findButton_Click(object sender, EventArgs e)
    {
      findComboBox.Items.Add(findComboBox.Text);
      FindAndSelectMatchingCell();
    }

    private Match FindAndSelectMatchingCell()
    {
      if (IsFirstInitNecessary)
      {
        InitSearch();
        IsFirstInitNecessary = false;
      }

      Match match = regexToSearch.Match("");
      bool allreadyAsked = false;
      do
      {
        match = FoundInCells(regexToSearch, allreadyAsked);

        if (!match.Success && !allreadyAsked && cellsToSearchStartIndex != 0 && !oneOccuranceFound)
        {
          var answer = MessageBox.Show(
            string.Format("Last row reached, cannot find '{0}'.\nShell we continue from the beginning?", findComboBox.Text),
            Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
          allreadyAsked = true;
          if (answer == DialogResult.Yes)
          {
            InitSearch();
            cellsToSearchNextIndex = downRadioButton.Checked ? 0 : (upRadioButton.Checked ? cellsToSearch.Count - 1 : 0);
            continue;
          }
          else
          {
            break;
          }
        }

        if(match.Success)
          oneOccuranceFound = true;

        break;
      }
      while (true);

      return match;
    }

    private void InitSearch()
    {
      if (IsFirstInitNecessary)
      {
        if (selectionRadioButton.Checked && IsFirstInitNecessary)
        {
          DataGridViewCell[] cellArrayToSearch = new DataGridViewCell[dgv.SelectedCells.Count];
          dgv.SelectedCells.CopyTo(cellArrayToSearch, 0);
          cellsToSearch = cellArrayToSearch.ToList();
          cellsToSearch.Reverse();
        }
        else if (downRadioButton.Checked && IsFirstInitNecessary)
        {
          cellsToSearch = GetAllCells();
        }
        else if (upRadioButton.Checked && IsFirstInitNecessary)
        {
          cellsToSearch = GetAllCells();
          cellsToSearch.Reverse();
        }
      }

      regexToSearch = CreateFindRegex();

      cellsToSearchNextIndex = IsFirstInitNecessary ? 0 : IsSelectedCells() ? 0 : GetActiveCellIndex(cellsToSearch);
      if (IsFirstInitNecessary)
        cellsToSearchStartIndex = cellsToSearchNextIndex;
    }

    private bool IsSelectedCells()
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

    private Match FoundInCells(Regex regex, bool againFromTheBeginning)
    {
      Match match = regex.Match("");

      for (int i = cellsToSearchNextIndex; i < (againFromTheBeginning ? cellsToSearchStartIndex - 1 : cellsToSearch.Count); i++)
      {
        var cell = cellsToSearch[i];
        if (typeof(DataGridViewTextBoxCell) == cell.GetType())
        {

          var tbCell = (DataGridViewTextBoxCell)cell;
          if (tbCell.Value == null || tbCell.Value.ToString() == string.Empty)
            continue;

          match = regex.Match(tbCell.Value.ToString());
          if (match.Success)
          {
            dgv.ClearSelection();
            dgv.CancelEdit();
            SelectTextInCell(tbCell, match);
            cellsToSearchNextIndex = i + 1;
            return match;
          }
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
      replaceComboBox.Items.Add(replaceComboBox.Text);

      if (dgv.EditingControl == null)
        FindAndSelectMatchingCell();

      if (dgv.EditingControl == null)
        return;

      ReplaceCellText();
    }

    private void ReplaceCellText()
    {
      string text = dgv.EditingControl.Text;

      dgv.BeginEdit(false);
      dgv.EditingControl.Text = regexToSearch.Replace(text, replaceComboBox.Text);
      dgv.EndEdit();
      dgv.ClearSelection();
    }

    private void FindReplaceDlg_VisibleChanged(object sender, EventArgs e)
    {
      if (Visible == true)
        IsFirstInitNecessary = true;
    }

    private void replaceAllButton_Click(object sender, EventArgs e)
    {
      replaceComboBox.Items.Add(replaceComboBox.Text);

      do
      {
        if (dgv.EditingControl == null)
          FindAndSelectMatchingCell();

        if (dgv.EditingControl == null)
        {
          IsFirstInitNecessary = true;
          return;
        }

        ReplaceCellText();
      }
      while (true);

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

    protected override void OnClosing(CancelEventArgs e)
    {
      e.Cancel = true;
      base.OnClosing(e);
      Hide();
    }
  }
}
