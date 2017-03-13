using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Globalization;

namespace GranitXMLEditor.Tests
{
  [TestClass()]
  public class GranitXmlToObjectBinderTests
  {
    static string[] good_examples = { "example.xml", "test.xml" };
    //static string[] good_examples = { "fizu_adok_1701.xml" };

    [TestMethod()]
    public void GranitXmlToObjectBinder_Test()
    {
      var x2o = new GranitXmlToAdapterBinder();
      Assert.IsNotNull(x2o.GranitXDocument);
      Assert.IsNotNull(x2o.HUFTransactionsAdapter);
    }

    [TestMethod()]
    public void Validation_Fails_On_Errorneous_XML_Test()
    {
      var x2o = new GranitXmlToAdapterBinder("bad_example.xml", true);
      Assert.AreEqual(x2o.XmlValidationErrorOccured, true);
    }

    [TestMethod()]
    public void Validation_Succeded_Test()
    {
      foreach (var xml in good_examples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);
        Assert.AreEqual(x2o.XmlValidationErrorOccured, false);
      }
    }

    [TestMethod()]
    public void TransactionId_Uniq_Test()
    {
      foreach (var xml in good_examples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        long previous_id = -1;
        foreach (var id in x2o.GranitXDocument.Root
          .Elements(Constants.Transaction).Select(x => x.Attribute(Constants.TransactionIdAttribute).Value))
        {
          Assert.AreNotEqual(previous_id, long.Parse(id));
          previous_id = long.Parse(id);
        }
      }
    }

    [TestMethod()]
    public void AddEmptyTransactionRow_Test()
    {
      foreach (var xml in good_examples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        int origCount = x2o.TransactionCount;
        x2o.AddEmptyTransactionRow();

        Assert.AreEqual(x2o.GranitXDocument.Root.Elements().ToList().Count, origCount + 1);
      }
    }

    [TestMethod()]
    public void AddEmptyTransactionRow_ReturnsWithTheLargestId_Test()
    {
      foreach (var xml in good_examples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        TransactionAdapter newTa = x2o.AddEmptyTransactionRow();
        foreach (var ta in x2o.HUFTransactionsAdapter.TransactionAdapters)
          Assert.IsTrue(newTa.TransactionId >= ta.TransactionId);

        Assert.AreEqual(x2o.GranitXDocument.Root.Elements(Constants.Transaction)
        .Where(t => t.Attribute(Constants.TransactionIdAttribute).Value == newTa.TransactionId.ToString()).Count(), 1);
      }
    }

    [TestMethod()]
    public void Sort_AmountAscending_Test()
    {
      foreach (var xml in good_examples)
      {
        Amount_Sort_and_MinMax_Test(xml, SortOrder.Ascending);
      }
    }

    [TestMethod()]
    public void Sort_AmountDescending_Test()
    {
      foreach (var xml in good_examples)
        Amount_Sort_and_MinMax_Test(xml, SortOrder.Descending);
    }

    private static void Amount_Sort_and_MinMax_Test(string xml, SortOrder order)
    {
      var x2o = new GranitXmlToAdapterBinder(xml, true);

      x2o.Sort(Constants.AmountPropertyName, order);
      decimal maxAmount = x2o.HUFTransactionsAdapter.TransactionAdapters.Max(ta => ta.Amount);
      decimal minAmount = x2o.HUFTransactionsAdapter.TransactionAdapters.Min(ta => ta.Amount);
      string firstAmount = x2o.GranitXDocument.Root.Elements(Constants.Transaction).First().Element(Constants.Amount).Value;
      string lastAmount = x2o.GranitXDocument.Root.Elements(Constants.Transaction).Last().Element(Constants.Amount).Value;

      string[] amounts = x2o.GranitXDocument.Root.Elements(Constants.Transaction).Elements(Constants.Amount).Select(t=>t.Value).ToArray();
      string s_before = amounts[0];

      if (order == SortOrder.Descending)
      {
        Assert.AreEqual(decimal.Parse(firstAmount, NumberStyles.Number, CultureInfo.InvariantCulture),
            maxAmount);
        Assert.AreEqual(decimal.Parse(lastAmount, NumberStyles.Number, CultureInfo.InvariantCulture),
            minAmount);
        foreach (var s in amounts)
        {
          Assert.IsTrue(
            decimal.Parse(s_before, NumberStyles.Number, CultureInfo.InvariantCulture) >=
            decimal.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture));
          s_before = s;
        }                                                                 
      }
      else
      {
        Assert.AreEqual(decimal.Parse(firstAmount, NumberStyles.Number, CultureInfo.InvariantCulture),
            minAmount);
        Assert.AreEqual(decimal.Parse(lastAmount, NumberStyles.Number, CultureInfo.InvariantCulture),
            maxAmount);
        foreach (var s in amounts)
        {
          Assert.IsTrue(
            decimal.Parse(s_before, NumberStyles.Number, CultureInfo.InvariantCulture) <=
            decimal.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture));
          s_before = s;
        }                                                                 


      }
    }

    [TestMethod()]
    public void Sort_ReqDateAscending_Test()
    {
      foreach (var xml in good_examples)
        Sort_ReqDate_Test(xml, SortOrder.Ascending);
    }

    [TestMethod()]
    public void Sort_ReqDateDescending_Test()
    {
      foreach (var xml in good_examples)
        Sort_ReqDate_Test(xml, SortOrder.Descending);
    }

    private static void Sort_ReqDate_Test(string xml, SortOrder order)
    {
      var x2o = new GranitXmlToAdapterBinder(xml, true);

      x2o.Sort(Constants.ExecutionDatePropertyName, order);

      DateTime maxDate = x2o.HUFTransactionsAdapter.TransactionAdapters.Max(ta => ta.ExecutionDate);
      DateTime minDate = x2o.HUFTransactionsAdapter.TransactionAdapters.Min(ta => ta.ExecutionDate);
      string firstDate = x2o.GranitXDocument.Root.Elements(Constants.Transaction).InDocumentOrder().First().Element(Constants.RequestedExecutionDate).Value;
      string lastDate = x2o.GranitXDocument.Root.Elements(Constants.Transaction).InDocumentOrder().Last().Element(Constants.RequestedExecutionDate).Value;

      if (order == SortOrder.Descending)
      {
        Assert.AreEqual(DateTime.Parse(firstDate), maxDate);
        Assert.AreEqual(DateTime.Parse(lastDate), minDate);
      }
      else
      {
        Assert.AreEqual(DateTime.Parse(firstDate), minDate);
        Assert.AreEqual(DateTime.Parse(lastDate), maxDate);
      }
    }

    [TestMethod()]
    public void AddTransactionRow_WithParameter_Test()
    {
      foreach (var xml in good_examples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        int origCount = x2o.TransactionCount;
        x2o.AddTransactionRow(new TransactionAdapter());

        Assert.AreEqual(x2o.GranitXDocument.Root.Elements().ToList().Count, origCount + 1);
      }
    }
    [TestMethod()]
    public void RemoveTransactionRowById_Test()
    {
      foreach (var xml in good_examples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        int origCount = x2o.TransactionCount;
        x2o.RemoveTransactionRowById(1);

        Assert.AreEqual(x2o.GranitXDocument.Root.Elements().ToList().Count, origCount-1);
        Assert.AreEqual(x2o.TransactionCount, origCount-1);
      }


    }
  }
}