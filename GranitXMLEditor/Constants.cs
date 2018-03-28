using System.Windows.Forms;

namespace GranitEditor
{
  public class Constants
  {

    //DataGridView
    public const string BeneficiaryAccount = "BeneficiaryAccount";
    public const string BeneficiaryName = "BeneficiaryName";
    public const string NullAccountFragment = "00000000";
    //PropertyNames
    public const string ExecutionDatePropertyName = "ExecutionDate";
    public const string OriginatorPropertyName = "Originator";
    public const string BeneficiaryNamePropertyName = "BeneficiaryName";
    public const string BeneficiaryAccountPropertyName = "BeneficiaryAccount";
    public const string AmountPropertyName  = "Amount";
    public const string CurrencyPropertyName = "Currency"; 
    public const string RemittanceInfoPropertyName  = "RemittanceInfo";      
    public const string IsSelectedPropertyName = "IsSelected";

    public enum WindowLayout
    {
      Cascade = MdiLayout.Cascade,
      TileHorizontal = MdiLayout.TileHorizontal,
      TileVertical = MdiLayout.TileVertical,
      Tabbed,
    }
      
  }
}
