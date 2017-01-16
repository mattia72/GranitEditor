using GranitXMLEditor.Properties;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GranitXMLEditor
{
  internal class GranitDataGridViewCellValidator
  {
    private DataGridView dataGridView1;

    public GranitDataGridViewCellValidator(DataGridView dataGridView1)
    {
      this.dataGridView1 = dataGridView1;
    }

    internal void Validate(ref DataGridViewCellValidatingEventArgs e, string headerText)
    {

      if (headerText == Resources.OriginatorHeaderText 
        || headerText == Resources.BeneficiaryAccountHeader)
      {
        string value = (string)e.FormattedValue;
        if (!IsAccountNumberValid(value))
        {
          dataGridView1.Rows[e.RowIndex].ErrorText = Resources.InvalidAccountError;
          e.Cancel = true;
        }
      }
      else if (headerText == Resources.CurrencyHeader)
      {
        string value = (string)e.FormattedValue;
        if (value != "HUF")
        {
          dataGridView1.Rows[e.RowIndex].ErrorText = Resources.InvalidCurrencyError;
          e.Cancel = true;
        }
      }
      else if (headerText == Resources.RemittanceInfoHeaderText)
      {
        string value = (string)e.FormattedValue;
        string line = string.Empty;
        if (!IsRemittanceInfoValid(value, ref line))
        {
          if (line != string.Empty)
            dataGridView1.Rows[e.RowIndex].ErrorText += string.Format(Resources.RemittanceInfoLineTooLongError, line);
          dataGridView1.Rows[e.RowIndex].ErrorText += "\n\n" + Resources.InvalidRemittanceInfoError;
          e.Cancel = true;
        }
      }
      else if (headerText == Resources.AmountHeaderText)
      {
        string value = (string)e.FormattedValue;
        decimal number;
        if ((decimal.TryParse(value, out number) && (number < 0)) || Math.Round(number) != number)
        {
          dataGridView1.Rows[e.RowIndex].ErrorText = Resources.InvalidAmountError;
          e.Cancel = true;
        }
      }
      else if (headerText == Resources.RequestedExecutionDateHeaderText)
      {
        string value = (string)e.FormattedValue;
        try
        {
          DateTime.Parse(value);
        }
        catch (System.Exception)
        {
          dataGridView1.Rows[e.RowIndex].ErrorText = Resources.InvalidDateError;
          e.Cancel = true;
        }

      }
      else
      {
        e.Cancel = false;
      }
    }

    private bool IsRemittanceInfoValid(string value, ref string lineOfError)
    {
      string[] lines = value.Split('|');

      if (lines.Length > 4)
        return false;
      else foreach (var line in lines)
        {
          if (line.Length > 32)
          {
            lineOfError = line;
            return false;
          }
        }
      return true;
    }

    private static bool IsAccountNumberValid(string value)
    {
      return value.Length == 26 && Regex.Match(value, @"(\d{8}-){2}\d{8}").Success;
    }
  }
}