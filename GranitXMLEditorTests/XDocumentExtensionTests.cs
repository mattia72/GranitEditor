using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml.Linq;
using GranitXMLEditorTests;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Diagnostics;

namespace GranitXMLEditor.Tests
{
  [TestClass()]
  public class XDocumentExtensionTests
  {
    [TestMethod()]
    public void SortElementsByXPathElementValue_Test()
    {
      string[] HUFTransactionXmls = {
        @" 
        <HUFTransactions> " +
          TestConstants.TransactionXElem2 +
          TestConstants.TransactionXml +
          TestConstants.TransactionXElem4 +
          TestConstants.TransactionXElem1 +
          TestConstants.TransactionXElem3 + @"
        </HUFTransactions> ",
        File.ReadAllText("example.xml"),
        File.ReadAllText("test.xml") };

      foreach (string HUFTransactionXml in HUFTransactionXmls)
      {
        Sort_Test(HUFTransactionXml, Constants.Transaction, Constants.AmountPropertyName, SortOrder.Ascending);
        Sort_Test(HUFTransactionXml, Constants.Transaction, Constants.AmountPropertyName, SortOrder.Descending);
      }
    }

    private static void Sort_Test(string xml, string elements, string byElement, SortOrder sortOrder)
    {
      var xdoc = XDocument.Parse(xml);

      string[] elementArray = xdoc.Root.Elements(elements)
        .Elements(byElement).Select(t => t.Value).ToArray();

      xdoc.SortElementsByXPathToStringValue(elements, byElement, sortOrder);

      string[] sortedElementArray = xdoc.Root.Elements(elements)
        .Elements(byElement).Select(t => t.Value).ToArray();

      var ordered = sortOrder == SortOrder.Descending ?
        elementArray.OrderByDescending(x => x) : elementArray.OrderBy(x => x);

      Debug.WriteLine("elements=" + string.Join(", ", elementArray));
      Debug.WriteLine("sorted  =" + string.Join(", ", sortedElementArray));
      Debug.WriteLine("ordered =" + string.Join(", ", ordered));

      string s1 = string.Join(",", ordered);
      string s2 = string.Join(",", sortedElementArray);

      Assert.AreEqual(s1, s2);
    }
  }
}