using GranitEditor.Properties;
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GranitEditor
{
  public class GranitDataGridViewCellFormatter
  {
    public static void Format(DataGridView dataGridView1, ref DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value == null) return;

      switch (dataGridView1.Columns[e.ColumnIndex].DataPropertyName)
      {
        case Constants.OriginatorPropertyName:
          FormatAccountNumber(e);
          break;
        case Constants.BeneficiaryAccountPropertyName:
          FormatAccountNumber(e);
          break;
        case Constants.AmountPropertyName:
          FormatAmount(dataGridView1, e);
          break;
        case Constants.ExecutionDatePropertyName:
          FormatDateField(dataGridView1, e);
          break;
        default:
          e.FormattingApplied = false;
          break;
      }
    }

    public static void FormatDateField(DataGridView dgv, DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value != null)
      {
        try
        {
          DateTime value = DateTime.Parse(e.Value.ToString());
          if (value < DateTime.Today)
          {
            SetErrorBackground(dgv, e, Resources.DateInThePastError);
          }
          else
          {
            SetErrorBackground(dgv, e);
          }

          e.Value = FormatDateTime(value);
          e.FormattingApplied = true;
        }
        catch (Exception)
        {
          e.FormattingApplied = false;
        }
      }
    }

    private static void SetErrorBackground(DataGridView dgv, DataGridViewCellFormattingEventArgs e, string errorText = null)
    {
      if (errorText != null)
      {
        e.CellStyle.BackColor = Color.LightPink;
        e.CellStyle.SelectionBackColor = Color.HotPink;
        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = Resources.DateInThePastError;
      }
      else
      {
        e.CellStyle.BackColor = SystemColors.Window;
        e.CellStyle.SelectionBackColor = SystemColors.Highlight;
        dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = "";
      }
    }

    public static void FormatAmount(DataGridView dgv, DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value != null)
      {
        try
        {
          decimal value = (decimal)e.Value;
          if (value < 0 || Math.Round(value) != value)
          {
            SetErrorBackground(dgv, e, Resources.InvalidAmountError);
          }
          else
          {
            SetErrorBackground(dgv, e); 
          }
          e.Value = value.ToString("N2");
          e.FormattingApplied = true;
        }
        catch (Exception)
        {
          e.FormattingApplied = false;
        }
      }
    }

    public static void FormatAccountNumber(DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value != null)
      {
        try
        {
          StringBuilder accountString = new StringBuilder();
          string value = (string)e.Value;
          value = Regex.Replace(value, "-", "");
          string fragment = Constants.NullAccountFragment;

          if (value.Length > 7)
            fragment = value.Substring(0, 8);
          else
            fragment = SafeAddNulls(value, 0);

          accountString.Append(fragment);
          accountString.Append("-");

          if (value.Length > 15)
            fragment = value.Substring(8, 8);
          else
            fragment = SafeAddNulls(value, 8);

          accountString.Append(fragment);

          if (value.Length > 16)
          {
            accountString.Append("-");

            if (value.Length >= 23)
              fragment = value.Substring(16, 8);
            else
              fragment = SafeAddNulls(value, 16);

            accountString.Append(fragment);
          }

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

    private static string SafeAddNulls(string value, int index)
    {
      return AddNullsToTheEnd(value.Length >= index + 1 ? value.Substring(index, value.Length - index) : String.Empty);
    }

    public static string AddNullsToTheEnd(string value)
    {
      StringBuilder valueWithNulls = new StringBuilder();
      StringBuilder nulls = new StringBuilder();

      while (value.Length + nulls.Length != 8)
        nulls.Append("0");

      valueWithNulls.Append(value).Append(nulls);

      return valueWithNulls.ToString();
    }

    public static void UnFormat(DataGridView dataGridView, ref DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value == null) return;

      switch (dataGridView.Columns[e.ColumnIndex].DataPropertyName)
      {
        case Constants.OriginatorPropertyName:
          UnFormatAccountNumber(e);
          break;
        case Constants.BeneficiaryAccountPropertyName:
          UnFormatAccountNumber(e);
          break;
        case Constants.AmountPropertyName:
          UnFormatAmount(dataGridView, e);
          break;
        case Constants.ExecutionDatePropertyName:
          UnFormatDateField(dataGridView, e);
          break;
        default:
          e.FormattingApplied = false;
          break;
      }
    }

    public static void UnFormatDateField(DataGridView dataGridView, DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value != null)
      {
        DateTime d = DateTime.Parse((string)e.Value);
        e.Value = d;
        e.FormattingApplied = true;
      }
    }

    public static void UnFormatAmount(DataGridView dataGridView, DataGridViewCellFormattingEventArgs e)
    {

      if (e.Value != null)
      {
        decimal parsedValue = 0;
        if (Decimal.TryParse((string)e.Value, out parsedValue))
        {
          e.Value = parsedValue;
          e.FormattingApplied = true;
        }
      }
    }

    public static void UnFormatAccountNumber(DataGridViewCellFormattingEventArgs e)
    {
      if (e.Value != null)
      {
        try
        {
          string value = (string)e.Value;
          value = Regex.Replace(value, "[ -]", "");
          StringBuilder accountString = new StringBuilder(value);
          while (accountString.Length < 24)
            accountString.Append("0");

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

    public static string FormatDateTime(DateTime date)
    {
      StringBuilder dateString = new StringBuilder();

      dateString.Append(date.Year.ToString().Substring(2));
      dateString.Append(".");
      dateString.Append(date.Month.ToString("00"));
      dateString.Append(".");
      dateString.Append(date.Day.ToString("00"));
      dateString.Append(".");
      return dateString.ToString();
    }
  }
}