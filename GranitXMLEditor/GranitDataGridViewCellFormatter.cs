using GranitXMLEditor.Properties;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GranitXMLEditor
{
  internal class GranitDataGridViewCellFormatter
  {
    internal static void Format(DataGridView dataGridView1, ref DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value == null) return;

      if (dataGridView1.Columns[e.ColumnIndex].HeaderText == Resources.OriginatorHeaderText
        || dataGridView1.Columns[e.ColumnIndex].HeaderText == Resources.BeneficiaryAccountHeader)
      {
        FormatAccountNumber(e);
      }
      else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == Resources.AmountHeaderText)
      {
        decimal value = (decimal)e.Value;
        if (value < 0 || Math.Round(value) != value)
        {
          e.CellStyle.BackColor = Color.Red;
          e.CellStyle.SelectionBackColor = Color.DarkRed;
        }
      }
      else if (dataGridView1.Columns[e.ColumnIndex].HeaderText == Resources.RequestedExecutionDateHeaderText)
      {
        //ShortFormDateFormat(e);
      }
    }

    private static void FormatAccountNumber(DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value != null)
      {
        try
        {
          StringBuilder accountString = new StringBuilder();
          string value = (string)e.Value;
          string fragment = Constants.NullAccountFragment;

          if(value.Length > 7)
            fragment = value.Substring(0, 8);

          accountString.Append(fragment);
          accountString.Append("-");

          if(value.Length > 15)
            fragment = value.Substring(8, 8);
          else
            fragment = Constants.NullAccountFragment;

          accountString.Append(fragment);
          accountString.Append("-");

          if (value.Length >= 23)
            fragment = value.Substring(16, 8);
          else
            fragment = Constants.NullAccountFragment;

          accountString.Append(fragment);

          e.Value = accountString.ToString();
          e.FormattingApplied = true;
        }
        catch (Exception)
        {
          // Set to false in case there are other handlers interested trying to
          // format this DataGridViewCellFormattingEventArgs instance.
          e.FormattingApplied = false;
        }

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

          dateString.Append(theDate.Year.ToString().Substring(2));
          dateString.Append("/");
          dateString.Append(theDate.Month);
          dateString.Append("/");
          dateString.Append(theDate.Day);
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