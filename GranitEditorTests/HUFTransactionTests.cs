using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml.Linq;
using GranitXMLEditorTests;
using ExtensionMethods;

namespace GranitXml.Tests
{
  [TestClass()]
  public class HUFTransactionTests
  {
    XDocument TestXDoc { get; set; }

    [TestMethod()]
    public void Load_Test()
    {
      TestXDoc = XDocument.Parse(TestConstants.HUFTransactionXml);
      var hufTrans = HUFTransaction.Load(TestXDoc);

      Assert.AreEqual(hufTrans.Transactions.Count, 
        TestXDoc.Root.Elements(GranitXml.Constants.Transaction).Count() + 
        TestXDoc.Root.DescendantNodes().OfType<XComment>().Where( xc => xc.IsCommentedXElement()).Count());

    }
  }
}