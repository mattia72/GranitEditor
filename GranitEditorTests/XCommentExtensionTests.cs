using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;
using GranitXMLEditorTests;

namespace ExtensionMethods.Tests
{
  [TestClass()]
  public class XCommentExtensionTests
  {
    [TestMethod()]
    public void IsNotCommentedXElement_Test()
    {
      XComment xc = new XComment("dummy comment");
      Assert.IsFalse(xc.IsCommentedXElement());
    }

    [TestMethod()]
    public void IsCommentedXElement_Test()
    {
      XComment commentedXElement = new XComment(TestConstants.TransactionXElem1);
      Assert.IsTrue(commentedXElement.IsCommentedXElement());
    }

    [TestMethod()]
    public void UnCommentXElmenet_Test()
    {
      XComment commentedXElement = new XComment(TestConstants.TransactionXElem1);
      XElement xe = commentedXElement.UnCommentXElmenet();
      
      Assert.IsTrue(TestConstants.TransactionXElem1.IgnoreWhiteSpaceEquals(xe.ToString())); 
    }
  }
}