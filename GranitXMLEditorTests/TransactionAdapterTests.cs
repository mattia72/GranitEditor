using GranitEditor;
using GranitXMLEditorTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GranitEditor.Tests
{
  [TestClass()]
  public class TransactionAdapterTests
  {
    TransactionAdapter TestAdapter { get; set; }
    XDocument TestXDoc { get; set; }

    [TestMethod()]
    public void UpdateGranitXDocument_Test()
    {
      //Arange
      FillTransactionAdapter();

      //Act
      TestAdapter.IsSelected = true;
      TestAdapter.Amount = 999.99m;
      TestAdapter.BeneficiaryAccount = "999999998888888877777777";
      TestAdapter.BeneficiaryName = "James Bond";
      TestAdapter.Currency = "EUR";
      TestAdapter.ExecutionDate = System.DateTime.Now;
      TestAdapter.Originator = "555555556666666677777777";
      TestAdapter.RemittanceInfo = "szöveg|szöveg|megint szöveg";

      Assert.AreEqual(TestXDoc.Root.Element(Constants.Transaction).Attribute(Constants.TransactionSelectedAttribute).Value.ToLower(),
        TestAdapter.IsSelected.ToString().ToLower());
      Assert.AreEqual(TestXDoc.Root.Element(Constants.Transaction).Element(Constants.Amount).Value,
        TestAdapter.Amount.ToString(Constants.AmountFormatString, CultureInfo.InvariantCulture));
      Assert.AreEqual(TestXDoc.Root.Element(Constants.Transaction).Element(Constants.Beneficiary).Element(Constants.Account).Element(Constants.AccountNumber).Value,
        TestAdapter.BeneficiaryAccount);
      Assert.AreEqual(TestXDoc.Root.Element(Constants.Transaction).Element(Constants.Beneficiary).Element(Constants.Name).Value,
        TestAdapter.BeneficiaryName);
      Assert.AreEqual(TestXDoc.Root.Element(Constants.Transaction).Element(Constants.Amount).Attribute(Constants.Currency).Value,
        TestAdapter.Currency);
      Assert.AreEqual(TestXDoc.Root.Element(Constants.Transaction).Element(Constants.RequestedExecutionDate).Value,
        TestAdapter.ExecutionDate.ToString(Constants.DateFormat));
      Assert.AreEqual(TestXDoc.Root.Element(Constants.Transaction).Element(Constants.Originator).Element(Constants.Account).Element(Constants.AccountNumber).Value,
        TestAdapter.Originator);

      string rInfo = string.Join("|", TestXDoc.Root.Element(Constants.Transaction).Element(Constants.RemittanceInfo).Elements(Constants.Text).Select(x => x.Value.Trim()));
      Assert.AreEqual(rInfo, TestAdapter.RemittanceInfo);
    }

    public void FillTransactionAdapter()
    {
      TestXDoc = System.Xml.Linq.XDocument.Parse(TestConstants.HUFTransactionXml);
      XmlRootAttribute xRoot = new XmlRootAttribute();
      xRoot.ElementName = Constants.HUFTransactions;
      // xRoot.Namespace = "http://www.cpandl.com";
      xRoot.IsNullable = true;
      var ser = new XmlSerializer(typeof(HUFTransaction), xRoot);
      HUFTransaction testHUFTransaction = (HUFTransaction)ser.Deserialize(TestXDoc.CreateReader());
      TestAdapter = new TransactionAdapter(testHUFTransaction.Transactions[0], TestXDoc);
    }

    [TestMethod()]
    public void ToString_Test()
    {
      FillTransactionAdapter();

      XElement xt = TestXDoc.Root.Elements(Constants.Transaction)
          .Where(x => TestAdapter.IsBindedWith(x)).ToList()
          .FirstOrDefault();

      Assert.IsTrue(TestAdapter.ToString().StartsWith("Id: " + xt.Attribute(Constants.TransactionIdAttribute).Value));
    }

    [TestMethod()]
    public void Clone_Test()
    {
      FillTransactionAdapter();

      TransactionAdapter clone = (TransactionAdapter)TestAdapter.Clone();

      Assert.IsTrue(clone.CompareTo(TestAdapter) == 0);
      Assert.IsTrue(TestAdapter.CompareTo(clone) == 0);
    }

    [TestMethod()]
    public void CompareTo_Test()
    {
      FillTransactionAdapter();

      TransactionAdapter clone = (TransactionAdapter)TestAdapter.Clone();

      Assert.IsTrue(clone.CompareTo(TestAdapter) == 0);
      clone.Amount = TestAdapter.Amount + 1;
      Assert.IsFalse(clone.CompareTo(TestAdapter) == 0);

    }
  }
}