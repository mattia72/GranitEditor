using GranitXMLEditor.Properties;
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
      else if (headerText == Resources.RemittanceInfoHeader)
      {
        string value = (string)e.FormattedValue;
        if (!IsRemittanceInfoValid(value))
        {
          dataGridView1.Rows[e.RowIndex].ErrorText = Resources.InvalidRemittanceInfoError;
          e.Cancel = true;
        }
      }
      else
      {
        e.Cancel = false;
      }
    }

    private bool IsRemittanceInfoValid(string value)
    {
      string[] strs = value.Split('|');

      if (strs.Length > 4) return false;
      else foreach (var s in strs)
        {
          if (s.Length > 32) return false; 
        }
      return true;
    }

    private static bool IsAccountNumberValid(string value)
    {
      return value.Length == 16 && value.Length == 24;
    }
  }
}