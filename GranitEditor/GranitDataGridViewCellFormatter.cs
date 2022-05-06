using GranitEditor.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GranitEditor
{
  public static class GranitDataGridViewCellFormatter
  {
    private static Color defaultBackColor = SystemColors.Window;
    private static Color defaultHighlightedBackColor = SystemColors.Highlight;
    private static Color defaultNotSelectedBackColor = Color.LightGray;
    private static Color defaultNotSelectedHighlightedBackColor = Color.DarkGray;
    private static Color defaultErrorBackColor = Color.LightPink;
    private static Color defaultErrorHighlightedBackColor = Color.HotPink;

    public static Color DefaultBackColor { get => defaultBackColor; set => defaultBackColor = value; }
    public static Color DefaultHighlightedBackColor { get => defaultHighlightedBackColor; set => defaultHighlightedBackColor = value; }
    public static Color DefaultNotSelectedBackColor { get => defaultNotSelectedBackColor; set => defaultNotSelectedBackColor = value; }
    public static Color DefaultNotSelectedHighlightedBackColor { get => defaultNotSelectedHighlightedBackColor; set => defaultNotSelectedHighlightedBackColor = value; }
    public static Color DefaultErrorBackColor { get => defaultErrorBackColor; set => defaultErrorBackColor = value; }
    public static Color DefaultErrorHighlightedBackColor { get => defaultErrorHighlightedBackColor; set => defaultErrorHighlightedBackColor = value; }

    //List<Tuple<int, int>> notSelectedCells;

    //public List<Tuple<int, int>> NotSelectedCells {
    //  get => notSelectedCells ?? new List<Tuple<int, int>>();
    //  }

    public static void CellFormat(DataGridView dataGridView1, ref DataGridViewCellFormattingEventArgs e)
    {
      if (dataGridView1 == null || e == null || e.Value == null) 
        return;

      //Debug.WriteLine($"CellFormat cell: {e.RowIndex} {e.ColumnIndex}");

      switch (dataGridView1.Columns[e.ColumnIndex].DataPropertyName)
      {
        //case Constants.IsSelectedPropertyName:
        //  SetNotSelectedBackground(dataGridView1, e);
        //  break;
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

      //if (!(bool)dataGridView1.Rows[e.RowIndex].Cells[0].Value)
      //  SetNotSelectedBackground(dataGridView1, e);
    }

    public static void FormatDateField(DataGridView dgv, DataGridViewCellFormattingEventArgs e)
    {
      if (dgv != null && e != null && e.Value != null)
      {
        try
        {
          DateTime value = DateTime.Parse(e.Value.ToString(), new CultureInfo("HU-hu"));
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

    //private static void SetNotSelectedBackground(DataGridView dgv, DataGridViewCellFormattingEventArgs e, string errorText = null)
    //{
    //  foreach (DataGridViewCell cell in dgv.Rows[e.RowIndex].Cells)
    //  {
    //    //bool isSelected = (bool)dgv.Rows[e.RowIndex].Cells[0].Value;
    //    cell.Style.BackColor = /*isSelected ? DefaultBackColor : */DefaultNotSelectedBackColor;
    //    cell.Style.SelectionBackColor = /*isSelected ? DefaultHighlightedBackColor : */DefaultNotSelectedHighlightedBackColor;
    //  }
    //}

    private static void SetErrorBackground(DataGridView dgv, DataGridViewCellFormattingEventArgs e, string errorText = null)
    {
       e.CellStyle.BackColor = (errorText != null) ? DefaultErrorBackColor : DefaultBackColor;
       e.CellStyle.SelectionBackColor = (errorText != null) ? DefaultErrorHighlightedBackColor : DefaultHighlightedBackColor;
       dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = errorText ?? string.Empty; 
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
          e.Value = FormatToDisplayAccountNumber((string)e.Value);
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

    public static string FormatToDisplayAccountNumber(string value)
    {
      StringBuilder accountString = new StringBuilder();
      value = Regex.Replace(value, "-", "");
      string fragment;

      if (value.Length > 7)
        fragment = value.Substring(0, 8);
      else
        fragment = SafeAddNulls(value, 0);

      accountString.Append(fragment);
      accountString.Append('-');

      if (value.Length > 15)
        fragment = value.Substring(8, 8);
      else
        fragment = SafeAddNulls(value, 8);

      accountString.Append(fragment);

      if (value.Length > 16)
      {
        accountString.Append('-');

        if (value.Length >= 23)
          fragment = value.Substring(16, 8);
        else
          fragment = SafeAddNulls(value, 16);

        accountString.Append(fragment);
      }

      return accountString.ToString();
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
        nulls.Append('0');

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
            UnFormatAmount(e);
            break;
          case Constants.ExecutionDatePropertyName:
            UnFormatDateField(e);
            break;
          default:
            e.FormattingApplied = false;
            break;
        }
      }

      public static void UnFormatDateField(DataGridViewCellFormattingEventArgs e)
      {
        if (e.Value != null)
        {
          DateTime d = DateTime.Parse((string)e.Value, new CultureInfo("HU-hu"));
          e.Value = d;
          e.FormattingApplied = true;
        }
      }

      public static void UnFormatAmount(DataGridViewCellFormattingEventArgs e)
    {

      if (e.Value != null)
      {
        if (Decimal.TryParse((string)e.Value, out decimal parsedValue))
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
          e.Value = FormatToXmlValidAccountNumber((string)e.Value);
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

    public static string FormatToXmlValidAccountNumber(string value)
    {
      value = Regex.Replace(value, "[ -]", "");
      StringBuilder accountString = new StringBuilder(value);
      if (accountString.Length != 16)
      {
        while (accountString.Length < 24)
          accountString.Append('0');
      }

      return accountString.ToString();
    }

    public static string FormatDateTime(DateTime date)
    {
      StringBuilder dateString = new StringBuilder();

      dateString.Append(date.Year.ToString().Substring(2));
      dateString.Append('.');
      dateString.Append(date.Month.ToString("00"));
      dateString.Append('.');
      dateString.Append(date.Day.ToString("00"));
      dateString.Append('.');
      return dateString.ToString();
    }
  }
}