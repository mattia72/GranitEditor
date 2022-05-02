using GranitEditor.Properties;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GranitEditor
{
  internal class GranitDataGridViewCellValidator
  {
    private readonly DataGridView dataGridView1;

    public GranitDataGridViewCellValidator(DataGridView dataGridView1)
    {
      this.dataGridView1 = dataGridView1;
    }

    internal void ValidateCell(ref DataGridViewCellValidatingEventArgs e, DataGridViewCell cell)
    {
      Validate(ref e, cell.OwningColumn.DataPropertyName);
    }

    internal void Validate(ref DataGridViewCellValidatingEventArgs e, string propertyName)
    {
      switch (propertyName)
      {
        case Constants.OriginatorPropertyName:
          ValidateAccountNum(e);
          break;
        case Constants.BeneficiaryAccountPropertyName:
          ValidateAccountNum(e);
          break;
        case Constants.CurrencyPropertyName:
          ValidateCurrency(e);
          break;
        case Constants.RemittanceInfoPropertyName:
          ValidateRemittanceInfo(e);
          break;
        case Constants.AmountPropertyName:
          ValidateAmount(e);
          break;
        case Constants.ExecutionDatePropertyName:
          ValidateRequestedExecutionDate(e);
          break;
        default:
          e.Cancel = false;
          break;
      }
    }

    private void ValidateRequestedExecutionDate(DataGridViewCellValidatingEventArgs e)
    {
      try
      {
        DateTime.Parse((string)e.FormattedValue, new CultureInfo("HU-hu"));
      }
      catch (System.Exception)
      {
        dataGridView1.Rows[e.RowIndex].ErrorText = Resources.InvalidDateError;
        e.Cancel = true;
      }
    }

    private void ValidateAmount(DataGridViewCellValidatingEventArgs e)
    {
      decimal number;
      string value = (string)e.FormattedValue;
      if ((decimal.TryParse(value, out number) && (number < 0))) //|| Math.Round(number) != number)
      {
        dataGridView1.Rows[e.RowIndex].ErrorText = Resources.InvalidAmountError;
        e.Cancel = true;
      }
    }

    private void ValidateCurrency(DataGridViewCellValidatingEventArgs e)
    {
      if ((string)e.FormattedValue != "HUF")
      {
        dataGridView1.Rows[e.RowIndex].ErrorText = Resources.InvalidCurrencyError;
        e.Cancel = true;
      }
    }

    private void ValidateRemittanceInfo(DataGridViewCellValidatingEventArgs e) 
    {
      string value = (string)e.FormattedValue;
      string line = string.Empty;
      if (!IsRemittanceInfoValid(value, ref line) &&
        dataGridView1.Rows[e.RowIndex].ErrorText == string.Empty)
      {
        if (line != string.Empty)
          dataGridView1.Rows[e.RowIndex].ErrorText += string.Format(Resources.RemittanceInfoLineTooLongError, line);
        dataGridView1.Rows[e.RowIndex].ErrorText += "\n\n" + Resources.InvalidRemittanceInfoError;
        e.Cancel = true;
      }
    }

    private void ValidateAccountNum(DataGridViewCellValidatingEventArgs e)
    {
      string value = (string)e.FormattedValue;
      if (!IsAccountNumberValid(value))
      {
        dataGridView1.Rows[e.RowIndex].ErrorText = Resources.InvalidAccountError;
        e.Cancel = true;
      }
    }

    private bool IsRemittanceInfoValid(string value, ref string lineOfError)
    {
      string[] lines = value.Split('|');
      return (lines.Length <= 4);
    }

    private static bool IsAccountNumberValid(string value)
    {
      return (Regex.Match(value, @"^\d{8}-?\d{8}-?(\d{8})?$").Success);
    }
  }
}