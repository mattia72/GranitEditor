using GranitXMLEditorTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GranitXMLEditor.Tests
{
  [TestClass()]
  public class TransactionTests
  {
    [TestMethod()]
    public void CompareTo_Equal_Test()
    {
      //arrange
      var xt1 = XDocument.Parse(TestConstants.TransactionXml);
      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t1 = (Transaction)ser.Deserialize(xt1.CreateReader());
      Transaction t2 = (Transaction)ser.Deserialize(xt1.CreateReader());
      //act
      //assert
      Assert.AreEqual(t1.CompareTo(t2), 0);
    }

    [TestMethod()]
    public void CompareTo_Originator_NotEqual_Test()
    {
      //arrange
      var xt1 = XDocument.Parse(TestConstants.TransactionXml);
      var transactionXml2 = TestConstants.TransactionXml.Replace("11111111", "99999999");
      var xt2 = XDocument.Parse(transactionXml2);

      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t1 = (Transaction)ser.Deserialize(xt1.CreateReader());
      Transaction t2 = (Transaction)ser.Deserialize(xt2.CreateReader());
      //act
      //assert
      Assert.AreNotEqual(t1.CompareTo(t2), 0);
    }

    [TestMethod()]
    public void CompareTo_Beneficiary_NotEqual_Test()
    {
      //arrange
      var xt1 = XDocument.Parse(TestConstants.TransactionXml);
      var transactionXml2 = TestConstants.TransactionXml.Replace("Gipsz", "Tusko");
      var xt2 = XDocument.Parse(transactionXml2);

      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t1 = (Transaction)ser.Deserialize(xt1.CreateReader());
      Transaction t2 = (Transaction)ser.Deserialize(xt2.CreateReader());
      //act
      //assert
      Assert.AreNotEqual(t1.CompareTo(t2), 0);
    }

    [TestMethod()]
    public void CompareTo_BeneficiaryAccount_NotEqual_Test()
    {
      //arrange
      var xt1 = XDocument.Parse(TestConstants.TransactionXml);
      var transactionXml2 = TestConstants.TransactionXml.Replace("33333333", "99999999");
      var xt2 = XDocument.Parse(transactionXml2);

      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t1 = (Transaction)ser.Deserialize(xt1.CreateReader());
      Transaction t2 = (Transaction)ser.Deserialize(xt2.CreateReader());
      //act
      //assert
      Assert.AreNotEqual(t1.CompareTo(t2), 0);
    }

    [TestMethod()]
    public void CompareTo_AmountCurrency_NotEqual_Test()
    {
      //arrange
      var xt1 = XDocument.Parse(TestConstants.TransactionXml);
      var transactionXml2 = TestConstants.TransactionXml.Replace("HUF", "EUR");
      var xt2 = XDocument.Parse(transactionXml2);

      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t1 = (Transaction)ser.Deserialize(xt1.CreateReader());
      Transaction t2 = (Transaction)ser.Deserialize(xt2.CreateReader());
      //act
      //assert
      Assert.AreNotEqual(t1.CompareTo(t2), 0);
    }

    [TestMethod()]
    public void CompareTo_Amount_NotEqual_Test()
    {
      //arrange
      var xt1 = XDocument.Parse(TestConstants.TransactionXml);
      var transactionXml2 = TestConstants.TransactionXml.Replace("1000.00", "1111.11");
      var xt2 = XDocument.Parse(transactionXml2);

      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t1 = (Transaction)ser.Deserialize(xt1.CreateReader());
      Transaction t2 = (Transaction)ser.Deserialize(xt2.CreateReader());
      //act
      //assert
      Assert.AreNotEqual(t1.CompareTo(t2), 0);
      Assert.AreEqual(t1.CompareTo(t2), "1000.00".CompareTo("1111.11"));
    }

  }
}