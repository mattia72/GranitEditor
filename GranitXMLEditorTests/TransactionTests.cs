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
      var xt1 = XDocument.Parse(transactionXml);
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
      var xt1 = XDocument.Parse(transactionXml);
      var transactionXml2 = transactionXml.Replace("11111111", "99999999");
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
      var xt1 = XDocument.Parse(transactionXml);
      var transactionXml2 = transactionXml.Replace("Gipsz", "Tusko");
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
      var xt1 = XDocument.Parse(transactionXml);
      var transactionXml2 = transactionXml.Replace("33333333", "99999999");
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
      var xt1 = XDocument.Parse(transactionXml);
      var transactionXml2 = transactionXml.Replace("HUF", "EUR");
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
      var xt1 = XDocument.Parse(transactionXml);
      var transactionXml2 = transactionXml.Replace("1000.00", "1111.11");
      var xt2 = XDocument.Parse(transactionXml2);

      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t1 = (Transaction)ser.Deserialize(xt1.CreateReader());
      Transaction t2 = (Transaction)ser.Deserialize(xt2.CreateReader());
      //act
      //assert
      Assert.AreNotEqual(t1.CompareTo(t2), 0);
      Assert.AreEqual(t1.CompareTo(t2), "1000.00".CompareTo("1111.11"));
    }

    private const string transactionXml = @"  
      <Transaction>
       <Originator> <Account> <AccountNumber>111111112222222233333333</AccountNumber> </Account> </Originator>
       <Beneficiary> <Name>Gipsz Jakab</Name> <Account> <AccountNumber>333333334444444455555555</AccountNumber> </Account> </Beneficiary>
       <Amount Currency = ""HUF"" >1000.00</Amount>
       <RequestedExecutionDate>2016-12-02</RequestedExecutionDate>
       <RemittanceInfo> <Text>Utólagos elszámolásra</Text> </RemittanceInfo>
      </Transaction>";
  }
}