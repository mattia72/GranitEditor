using GranitXMLEditor;
using GranitXMLEditorTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GranitXMLEditor.Tests
{
  [TestClass()]
  public class TransactionAdapterTests
  {
    [TestMethod()]
    public void UpdateGranitXDocumentTest()
    {
      System.Xml.Linq.XDocument xt;
      TransactionAdapter ta;
      FillTransactionAdapter(out xt, out ta);

      //Act
      ta.IsSelected = true;
      ta.Amount = 999.99m;
      ta.BeneficiaryAccount = "999999998888888877777777";
      ta.BeneficiaryName = "James Bond";
      ta.Currency = "EUR";
      ta.ExecutionDate = System.DateTime.Now;
      ta.Originator = "555555556666666677777777";
      ta.RemittanceInfo = "szöveg|szöveg|megint szöveg";

      Assert.AreEqual(xt.Root.Element(Constants.Transaction).Attribute(Constants.TransactionSelectedAttribute).Value.ToLower(),
        ta.IsSelected.ToString().ToLower());
      Assert.AreEqual(xt.Root.Element(Constants.Transaction).Element(Constants.Amount).Value,
        ta.Amount.ToString(Constants.AmountFormatString, CultureInfo.InvariantCulture));
      Assert.AreEqual(xt.Root.Element(Constants.Transaction).Element(Constants.Beneficiary).Element(Constants.Account).Element(Constants.AccountNumber).Value,
        ta.BeneficiaryAccount);
      Assert.AreEqual(xt.Root.Element(Constants.Transaction).Element(Constants.Beneficiary).Element(Constants.Name).Value,
        ta.BeneficiaryName);
      Assert.AreEqual(xt.Root.Element(Constants.Transaction).Element(Constants.Amount).Attribute(Constants.Currency).Value,
        ta.Currency);
      Assert.AreEqual(xt.Root.Element(Constants.Transaction).Element(Constants.RequestedExecutionDate).Value,
        ta.ExecutionDate.ToString(Constants.DateFormat));
      Assert.AreEqual(xt.Root.Element(Constants.Transaction).Element(Constants.Originator).Element(Constants.Account).Element(Constants.AccountNumber).Value,
        ta.Originator);

      string rInfo = string.Join("|", xt.Root.Element(Constants.Transaction).Element(Constants.RemittanceInfo).Elements(Constants.Text).Select(x => x.Value.Trim()));
      Assert.AreEqual(rInfo, ta.RemittanceInfo);
    }

    public static void FillTransactionAdapter(out System.Xml.Linq.XDocument xdoc, out TransactionAdapter ta)
    {
      xdoc = System.Xml.Linq.XDocument.Parse(TestConstants.HUFTransactionXml);
      XmlRootAttribute xRoot = new XmlRootAttribute();
      xRoot.ElementName = Constants.HUFTransactions;
      // xRoot.Namespace = "http://www.cpandl.com";
      xRoot.IsNullable = true;
      var ser = new XmlSerializer(typeof(HUFTransaction), xRoot);
      HUFTransaction t = (HUFTransaction)ser.Deserialize(xdoc.CreateReader());
      ta = new TransactionAdapter(t.Transactions[0], xdoc);
    }
  }
}