using Microsoft.VisualStudio.TestTools.UnitTesting;
using GranitXMLEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GranitXMLEditor.Tests;

namespace GranitXMLEditor.Tests
{
  [TestClass()]
  public class GranitXmlToObjectBinderTests
  {
    [TestMethod()]
    public void GranitXmlToObjectBinderTest()
    {
      var x2o = new GranitXmlToAdapterBinder();
      Assert.IsNotNull(x2o.GranitXDocument);
      //Assert.IsNotNull(x2o.HUFTransaction);
      Assert.IsNotNull(x2o.HUFTransactionsAdapter);
    }

    [TestMethod()]
    public void TransactionId_Uniq_Test()
    {
      var x2o = new GranitXmlToAdapterBinder("example.xml");

      long previous_id = -1;
      foreach ( var id in x2o.GranitXDocument.Root.Elements(Constants.Transaction).Select(x=>x.Attribute(Constants.TransactionIdAttribute).Value))
      {
        Assert.AreNotEqual(previous_id, long.Parse(id));
        previous_id = long.Parse(id);
      }

    }
  }
}