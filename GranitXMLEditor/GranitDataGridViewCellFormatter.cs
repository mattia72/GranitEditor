using GranitXMLEditor.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GranitXMLEditor
{
  internal class GranitDataGridViewCellFormatter
  {
    internal static void Format(DataGridView dataGridView1, ref DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value == null) return;

      if (dataGridView1.Columns[e.ColumnIndex].HeaderText == Resources.OriginatorHeaderText)
      {
      }
      else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == Resources.AmountHeader)
      {
        decimal value = (decimal)e.Value;
        if (value < 0 || Math.Round(value) != value)
        {
          e.CellStyle.BackColor = Color.Red;
          e.CellStyle.SelectionBackColor = Color.DarkRed;
        }
      }
      else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == Resources.RequestedExecutionDateHeader)
      {
        ShortFormDateFormat(e);
      }
    }

    //Even though the date internaly stores the year as YYYY, using formatting, the
    //UI can have the format in YY.  
    private static void ShortFormDateFormat(DataGridViewCellFormattingEventArgs formatting)
    {
      if (formatting.Value != null)
      {
        try
        {
          System.Text.StringBuilder dateString = new System.Text.StringBuilder();
          DateTime theDate = DateTime.Parse(formatting.Value.ToString());

          dateString.Append(theDate.Month);
          dateString.Append("/");
          dateString.Append(theDate.Day);
          dateString.Append("/");
          dateString.Append(theDate.Year.ToString().Substring(2));
          formatting.Value = dateString.ToString();
          formatting.FormattingApplied = true;
        }
        catch (FormatException)
        {
          // Set to false in case there are other handlers interested trying to
          // format this DataGridViewCellFormattingEventArgs instance.
          formatting.FormattingApplied = false;
        }
      }
    }
  }
}