using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GranitEditor
{
  class ClipboardHandler
  {
    private DataGridView dataGridView1;

    public bool PasteToSelectedCellsOnly { get; private set; }

    public ClipboardHandler(DataGridView dataGridView1)
    {
      this.dataGridView1 = dataGridView1;
    }

    public void CopyToClipboard()
    {
      //Copy to clipboard
      DataObject dataObj = dataGridView1.GetClipboardContent();
      if (dataObj != null)
        Clipboard.SetDataObject(dataObj);
    }

    public void PasteClipboardValue()
    {
      //Show Error if no cell is selected
      if (dataGridView1.SelectedCells.Count == 0)
      {
        MessageBox.Show("Please select a cell", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      //Get the satring Cell
      DataGridViewCell startCell = GetStartCell(dataGridView1);
      //Get the clipboard value in a dictionary
      Dictionary<int, Dictionary<int, string>> cbValue = ClipBoardValues(Clipboard.GetText());

      int iRowIndex = startCell.RowIndex;
      foreach (int rowKey in cbValue.Keys)
      {
        int iColIndex = startCell.ColumnIndex;
        foreach (int cellKey in cbValue[rowKey].Keys)
        {
          //Check if the index is with in the limit
          if (iColIndex <= dataGridView1.Columns.Count - 1 && iRowIndex <= dataGridView1.Rows.Count - 1)
          {
            DataGridViewCell cell = dataGridView1[iColIndex, iRowIndex];

            //Copy to selected cells if 'PasteToSelectedCellsOnly' is checked
            if ((PasteToSelectedCellsOnly && cell.Selected) || (!PasteToSelectedCellsOnly))
              cell.Value = cbValue[rowKey][cellKey];
          }
          iColIndex++;
        }
        iRowIndex++;
      }
    }

    private DataGridViewCell GetStartCell(DataGridView dgView)
    {
      //get the smallest row,column index
      if (dgView.SelectedCells.Count == 0)
        return null;

      int rowIndex = dgView.Rows.Count - 1;
      int colIndex = dgView.Columns.Count - 1;

      foreach (DataGridViewCell dgvCell in dgView.SelectedCells)
      {
        if (dgvCell.RowIndex < rowIndex)
          rowIndex = dgvCell.RowIndex;
        if (dgvCell.ColumnIndex < colIndex)
          colIndex = dgvCell.ColumnIndex;
      }

      return dgView[colIndex, rowIndex];
    }

    private Dictionary<int, Dictionary<int, string>> ClipBoardValues(string clipboardValue)
    {
      Dictionary<int, Dictionary<int, string>> copyValues = new Dictionary<int, Dictionary<int, string>>();

      String[] lines = clipboardValue.Split('\n');

      for (int i = 0; i <= lines.Length - 1; i++)
      {
        copyValues[i] = new Dictionary<int, string>();
        String[] lineContent = lines[i].Split('\t');

        //if an empty cell value copied, then set the dictionay with an empty string
        //else Set value to dictionary
        if (lineContent.Length == 0)
          copyValues[i][0] = string.Empty;
        else
        {
          for (int j = 0; j <= lineContent.Length - 1; j++)
            copyValues[i][j] = lineContent[j];
        }
      }
      return copyValues;
    }
  }
}
