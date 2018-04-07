using ExtensionMethods;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Xml.Linq;
using GranitXMLEditorTests;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Xml;

namespace GranitEditor.Tests
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
        SortDecimal_Test(HUFTransactionXml, GranitXml.Constants.Transaction, Constants.AmountPropertyName, SortOrder.Ascending);
        SortDecimal_Test(HUFTransactionXml, GranitXml.Constants.Transaction, Constants.AmountPropertyName, SortOrder.Descending);
      }

      foreach (string HUFTransactionXml in HUFTransactionXmls)
      {
        SortString_Test(HUFTransactionXml, GranitXml.Constants.Transaction, GranitXml.Constants.RequestedExecutionDate, SortOrder.Ascending);
        SortString_Test(HUFTransactionXml, GranitXml.Constants.Transaction, GranitXml.Constants.RequestedExecutionDate, SortOrder.Descending);
      }
    }

    private static void SortString_Test(string xml, string elements, string byElement, SortOrder sortOrder)
    {
      var xdoc = XDocument.Parse(xml);

      string[] elementArray = xdoc.Root.Elements(elements)
        .Elements(byElement).Select(t => t.Value).ToArray();

      xdoc.SortElementsByXPathToStringValue(elements, byElement, sortOrder);

      string[] sortedElementArray = xdoc.Root.Elements(elements)
        .Elements(byElement).Select(t => t.Value).ToArray();

      var ordered = sortOrder == SortOrder.Descending ?
        elementArray.OrderByDescending(x => x) : elementArray.OrderBy(x => x);

      //Debug.WriteLine("------\nxml=" + xml);
      //Debug.WriteLine("elements=" + string.Join(", ", elementArray));
      //Debug.WriteLine("sorted  =" + string.Join(", ", sortedElementArray));
      //Debug.WriteLine("ordered =" + string.Join(", ", ordered));

      string s1 = string.Join(",", ordered);
      string s2 = string.Join(",", sortedElementArray);

      Assert.AreEqual(s1, s2);
    }

    private static void SortDecimal_Test(string xml, string elements, string byElement, SortOrder sortOrder)
    {
      var xdoc = XDocument.Parse(xml);

      decimal[] elementArray = xdoc.Root.Elements(elements)
        .Elements(byElement).Select(t =>
        decimal.Parse(t.Value, NumberStyles.Number, CultureInfo.InvariantCulture)).ToArray();

      xdoc.SortElementsByXPathToDecimalValue(elements, byElement, sortOrder);

      decimal[] sortedElementArray = xdoc.Root.Elements(elements)
        .Elements(byElement).Select(t =>
        decimal.Parse(t.Value, NumberStyles.Number, CultureInfo.InvariantCulture)).ToArray();

      var ordered = sortOrder == SortOrder.Descending ?
        elementArray.OrderByDescending(x => x) : elementArray.OrderBy(x => x);

      //Debug.WriteLine("------\nxml=" + xml);
      //Debug.WriteLine("elements=" + string.Join(", ", elementArray));
      //Debug.WriteLine("sorted  =" + string.Join(", ", sortedElementArray));
      //Debug.WriteLine("ordered =" + string.Join(", ", ordered));

      string s1 = string.Join(",", ordered);
      string s2 = string.Join(",", sortedElementArray);

      Assert.AreEqual(s1, s2);
    }

    [TestMethod()]
    public void IsEmpty_Test()
    {
      XDocument x = new XDocument();
      Assert.IsTrue(x.IsEmpty());
    }

    [TestMethod()]
    public void CommentXElmenet_Test()
    {
      CommentFirstXElement(out XDocument xdoc, out int origTransCount, out int origCommentCount);

      CountTransactionsAndComments(xdoc, out int actTransCount, out int actCommentCount);

      Assert.AreEqual(origTransCount - 1, actTransCount);
      Assert.AreEqual(origCommentCount + 1, actCommentCount);
    }

    private static void CountTransactionsAndComments(XDocument xdoc, out int actTransCount, out int actCommentCount)
    {
      actTransCount = xdoc.Root.Elements(GranitXml.Constants.Transaction).Count();
      actCommentCount = xdoc.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Count();
    }

    [TestMethod()]
    public void UnCommentXElmenet_Test()
    {
      CommentFirstXElement(out XDocument xdoc, out int origTransCount, out int origCommentCount);
      CountTransactionsAndComments(xdoc, out origTransCount, out origCommentCount);

      var comList = xdoc.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).ToList();

      foreach(XComment xc in comList)
        xdoc.UnCommentXElmenet(xc);

      CountTransactionsAndComments(xdoc, out int actTransCount, out int actCommentCount);

      Assert.AreEqual(origTransCount + 1, actTransCount);
      Assert.AreEqual(origCommentCount - 1, actCommentCount);
    }

    private void CommentFirstXElement(out XDocument xdoc, out int origTransCount, out int origCommentCount)
    {
      TransactionAdapterTests.FillTransactionAdapter();
      xdoc = TransactionAdapterTests.TestXDoc;

      origTransCount = xdoc.Root.Elements(GranitXml.Constants.Transaction).Count();
      origCommentCount = xdoc.DescendantNodes().Where(x => x.NodeType == XmlNodeType.Comment).Count();

      XElement xe = xdoc.Root.Elements(GranitXml.Constants.Transaction)
          .Where(x => TransactionAdapterTests.TestAdapter.IsBindedWith(x)).ToList()
          .FirstOrDefault();

      xe.CommentXElmenet();
    }
  }
}