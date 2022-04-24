using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GranitEditor
{
  /// <summary>
  /// https://www.codeproject.com/Tips/208281/Copy-Paste-in-Datagridview-Control
  /// </summary>
  public class ClipboardHandler
  {
    public DataGridView DataGridView { get; set; }
    public bool ClipboardHasContent { get => DataGridView?.GetClipboardContent() != null; }
    public bool PasteToSelectedCellsOnly { get; set; }

    public ClipboardHandler()
    {
    }

    public ClipboardHandler(DataGridView dataGridView)
    {
      this.DataGridView = dataGridView;
      this.DataGridView.CellValueChanged += DataGridView_CellValueChanged;
    }

    private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      //Todo: handle History.EndCompoundChange.
      //throw new NotImplementedException();
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
      if (DataGridView != null && DataGridView.SelectedCells.Count == 0)
      {
        _ = MessageBox.Show("Please select a cell", "Paste", MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
        return;
      }

      //Get the satring Cell
      DataGridViewCell startCell = GetStartCell(DataGridView);
      //Get the clipboard value in a dictionary
      Dictionary<int, Dictionary<int, string>> cbValue = PutClipboardToDictionary();

      int gridRowIndex = startCell.RowIndex;
      for (int rowKey = 0; rowKey < cbValue.Keys.Count; rowKey++)
      {
        int gridColIndex = startCell.ColumnIndex;
        foreach (int cellKey in cbValue[rowKey].Keys)
        {
          // add new row if necessary
          if (gridRowIndex == DataGridView.Rows.Count)
          {
            AddRow();
          }

          //Check if the index is with in the limit
          if (gridColIndex < DataGridView.Columns.Count && gridRowIndex < DataGridView.Rows.Count)
          {
            DataGridViewCell cell = DataGridView[gridColIndex, gridRowIndex];

            //Copy to selected cells if 'PasteToSelectedCellsOnly' is checked
            if ((PasteToSelectedCellsOnly && cell.Selected) || (!PasteToSelectedCellsOnly))
            {
              //Todo: History.BeginCompoundDo()
              SetCellValue(cbValue, rowKey, cellKey, cell);
            }
          }
          gridColIndex++;
        }
        gridRowIndex++;

      }
    }

    private Dictionary<int, Dictionary<int, string>> PutClipboardToDictionary()
    {
      string clipBoardContent = Clipboard.GetText();
      List<string> lineList = new List<string>();

      var lines = clipBoardContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
      foreach (string line in lines)
      {
        //Skip empty lines
        if (IsWholeLine(line) && Regex.IsMatch(line, "^\t+$"))
          continue;

        if (IsWholeLine(line))
          lineList.Add(line.Remove(0, 1));
        else
          lineList.Add(line);
      }
      Dictionary<int, Dictionary<int, string>> cbValue = ClipBoardValuesToDictionary(lineList);
      return cbValue;
    }

    private bool IsWholeLine(string line)
    {
      return line.StartsWith("\t", StringComparison.Ordinal) && line.Count(c => c == '\t') == DataGridView.ColumnCount;
    }

    private void AddRow()
    {
      if (DataGridView.Parent is GranitXMLEditorForm)
      {
        GranitXMLEditorForm form = DataGridView.Parent as GranitXMLEditorForm;
        form.ContextMenuHandler.AddNewEmptyRow();
      }
    }

    private void SetCellValue(Dictionary<int, Dictionary<int, string>> cbValue, int rowKey, int cellKey, DataGridViewCell cell)
    {
      DataGridViewCellFormattingEventArgs e =
        new DataGridViewCellFormattingEventArgs(cell.ColumnIndex, cell.RowIndex, cbValue[rowKey][cellKey], cell.ValueType, cell.Style);

      GranitDataGridViewCellFormatter.UnFormat(DataGridView, ref e);
      if (e.FormattingApplied)
      {
        //TODO call validator
        DataGridView.BeginEdit(false);  
        cell.Value = e.Value;
        DataGridView.EndEdit();
      }
      else
      {
        switch (cell.ValueType.FullName)
        {
          case "System.Boolean":
            {
              if (Boolean.TryParse(cbValue[rowKey][cellKey], out bool parsedBoolValue))
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

    private Dictionary<int, Dictionary<int, string>> ClipBoardValuesToDictionary(List<string> lines)
    {
      Dictionary<int, Dictionary<int, string>> copyValues = new Dictionary<int, Dictionary<int, string>>();

      for (int i = 0; i <= lines.Count - 1; i++)
      {
        String[] lineContent = lines[i].Split('\t');

        copyValues[i] = new Dictionary<int, string>();

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
