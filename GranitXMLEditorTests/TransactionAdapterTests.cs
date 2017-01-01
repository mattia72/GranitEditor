using GranitXMLEditor;
using GranitXMLEditorTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
      var xdoc = XDocument.Parse(TestConstants.HUFTransactionXml);
      XmlRootAttribute xRoot = new XmlRootAttribute();
      xRoot.ElementName = Constants.HUFTransactions;
      // xRoot.Namespace = "http://www.cpandl.com";
      xRoot.IsNullable = true;
      var ser = new XmlSerializer(typeof(HUFTransactions), xRoot);
      HUFTransactions t = (HUFTransactions)ser.Deserialize(xdoc.CreateReader());
      var ta = new TransactionAdapter(t.Transaction[0], xdoc);

      //Act
      ta.UpdateGranitXDocument(Constants.Active, "True");

      Assert.AreEqual(xdoc.Root.Element(Constants.Transaction)
        .Attribute(Constants.TransactionActiveAttribute).Value, 
        ta.IsActive.ToString());
    }
  }
}