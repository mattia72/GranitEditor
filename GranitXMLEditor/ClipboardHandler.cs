using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GranitEditor
{
  /// <summary>
  /// https://www.codeproject.com/Tips/208281/Copy-Paste-in-Datagridview-Control
  /// </summary>
  public class ClipboardHandler
  {
    private DataGridView dataGridView1;
    public DataGridView DataGridView { get => dataGridView1; set => dataGridView1 = value; }
    public bool ClipboardHasContent { get => dataGridView1?.GetClipboardContent() != null; }
    public bool PasteToSelectedCellsOnly { get; set; }

    public ClipboardHandler()
    {
    }

    public ClipboardHandler(DataGridView dataGridView)
    {
      this.DataGridView = dataGridView;
    }

    public void CopyToClipboard(DataGridView dataGridView)
    {
      DataGridView = dataGridView;
      //Copy to clipboard
      DataObject dataObj = DataGridView.GetClipboardContent();
      if (dataObj != null)
        Clipboard.SetDataObject(dataObj);
    }

    public void PasteClipboardValue(DataGridView dataGridView)
    {
      DataGridView = dataGridView;

      //Show Error if no cell is selected
      if (DataGridView.SelectedCells.Count == 0)
      {
        MessageBox.Show("Please select a cell", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
      }

      //Get the satring Cell
      DataGridViewCell startCell = GetStartCell(DataGridView);
      //Get the clipboard value in a dictionary
      Dictionary<int, Dictionary<int, string>> cbValue = ClipBoardValues(Clipboard.GetText());

      int iRowIndex = startCell.RowIndex;
      foreach (int rowKey in cbValue.Keys)
      {
        int iColIndex = startCell.ColumnIndex;
        foreach (int cellKey in cbValue[rowKey].Keys)
        {
          //Check if the index is with in the limit
          if (iColIndex <= DataGridView.Columns.Count - 1 && iRowIndex <= DataGridView.Rows.Count - 1)
          {
            DataGridViewCell cell = DataGridView[iColIndex, iRowIndex];

            //Copy to selected cells if 'PasteToSelectedCellsOnly' is checked
            if ((PasteToSelectedCellsOnly && cell.Selected) || (!PasteToSelectedCellsOnly))
            {
              SetCellValue(cbValue, rowKey, cellKey, cell);
            }
          }
          iColIndex++;
        }
        iRowIndex++;

        if (iRowIndex >= DataGridView.Rows.Count - 1)
        {
          if (DataGridView.Parent is GranitXMLEditorForm)
          {
            GranitXMLEditorForm form = DataGridView.Parent as GranitXMLEditorForm;
            form.ContextMenuHandler.AddNewEmptyRow();
          }
        }
      }
    }

    private void SetCellValue(Dictionary<int, Dictionary<int, string>> cbValue, int rowKey, int cellKey, DataGridViewCell cell)
    {
      switch (cell.ValueType.FullName)
      {
        case "System.Boolean":
          {
            bool parsedBoolValue = false;
            if (Boolean.TryParse(cbValue[rowKey][cellKey], out parsedBoolValue))
            {
              cell.Value = parsedBoolValue;
              DataGridView.RefreshEdit();
            }
            else
              throw new FormatException("Paste this type of value not implemented!");
            break;
          }
        case "System.Decimal":
          decimal parsedValue = 0;
          if (Decimal.TryParse(cbValue[rowKey][cellKey], out parsedValue))
            cell.Value = parsedValue;
          else
            throw new FormatException("Paste this type of value not implemented!");
          break;
        default: //string
          cell.Value = cbValue[rowKey][cellKey];
          break;
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
