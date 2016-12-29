using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;

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
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      Hide();
    }

    private bool isFirstFind = true;
    private List<DataGridViewCell> cellsToSearch;
    private int cellsToSearchNextIndex = 0;
    private Regex regexToSearch;

    private void findButton_Click(object sender, EventArgs e)
    {
      findComboBox.Items.Add(findComboBox.Text);
      Find();
    }

    private void Find()
    {
      if (isFirstFind)
        InitSearch();

      DialogResult answer = DialogResult.Yes;
      while (!FoundInCells(regexToSearch).Success && answer == DialogResult.Yes)
      {
        answer = MessageBox.Show(
          string.Format("Cannot find '{0}'.\nShell we continue from the beginning?", findComboBox.Text),
          Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

        if (answer == DialogResult.Yes)
        {
          InitSearch();
          cellsToSearchNextIndex = downRadioButton.Checked ? 0 : (upRadioButton.Checked ? cellsToSearch.Count - 1 : 0);
        }
        isFirstFind = true;
      }
    }

    private void InitSearch()
    {
      if (selectionRadioButton.Checked)
      {
        cellsToSearch = new List<DataGridViewCell>(dgv.SelectedCells.Count);
        dgv.SelectedCells.CopyTo(cellsToSearch.ToArray(), 0);
        cellsToSearch.Reverse();
      }
      else if (downRadioButton.Checked)
      {
        cellsToSearch = GetAllCells();
      }
      else if (upRadioButton.Checked)
      {
        cellsToSearch = GetAllCells();
        cellsToSearch.Reverse();
      }
      regexToSearch = CreateFindRegex();
      isFirstFind = false;

      cellsToSearchNextIndex = GetActiveCellIndex(cellsToSearch);
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

    private Match FoundInCells(Regex regex)
    {
      Match match = regex.Match("");

      for (int i = cellsToSearchNextIndex; i < cellsToSearch.Count; i++)
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
            break;
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
      isFirstFind = true;
    }

    private void replaceButton_Click(object sender, EventArgs e)
    {
      replaceComboBox.Items.Add(replaceComboBox.Text);
      string text = dgv.EditingControl.Text;
      dgv.BeginEdit(false);
      dgv.EditingControl.Text = regexToSearch.Replace(text, replaceComboBox.Text);
      dgv.EndEdit();
    }
  }
}
