using GranitEditor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.IO;

namespace GranitEditor.Tests
{
  [TestClass()]
  public class GranitXmlToAdapterBinderTests
  {
    static string[] goodXmlExamples = { "example.xml", "test.xml" };
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
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);
        Assert.AreEqual(x2o.XmlValidationErrorOccured, false);
      }
    }

    [TestMethod()]
    public void TransactionId_Uniq_Test()
    {
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        long previous_id = -1;
        foreach (var id in x2o.GranitXDocument.Root
          .Elements(GranitXml.Constants.Transaction).Select(x => x.Attribute(GranitXml.Constants.TransactionIdAttribute).Value))
        {
          Assert.AreNotEqual(previous_id, long.Parse(id));
          previous_id = long.Parse(id);
        }
      }
    }

    [TestMethod()]
    public void AddEmptyTransactionRow_Test()
    {
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        int origCount = x2o.TransactionCount;

        x2o.AddEmptyTransactionRow();

        Assert.AreEqual(x2o.History.UndoCount, 1);
        Assert.AreEqual(x2o.GranitXDocument.Root.Elements().ToList().Count, origCount + 1);
      }
    }

    [TestMethod()]
    public void AddEmptyTransactionRow_IncreasesUndocountByOne()
    {
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        Assert.AreEqual(x2o.History.UndoCount, 0);
        TransactionAdapter newTa = x2o.AddEmptyTransactionRow();
        Assert.AreEqual(x2o.History.UndoCount, 1);
      }
    }

    [TestMethod()]
    public void AddEmptyTransactionRow_IncreasesTransactionCountByOne_Test()
    {
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);
        int origCount = x2o.HUFTransactionsAdapter.TransactionAdapters.Count;
        TransactionAdapter newTa = x2o.AddEmptyTransactionRow();
        int afterCount = x2o.HUFTransactionsAdapter.TransactionAdapters.Count;
        Assert.AreEqual(origCount + 1, afterCount);
      }
    }

    [TestMethod()]
    public void AddEmptyTransactionRow_ReturnsWithTheLargestId_Test()
    {
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        TransactionAdapter newTa = x2o.AddEmptyTransactionRow();

        foreach (var ta in x2o.HUFTransactionsAdapter.TransactionAdapters)
          Assert.IsTrue(newTa.TransactionId >= ta.TransactionId);

        Assert.AreEqual(x2o.GranitXDocument.Root.Elements(GranitXml.Constants.Transaction)
        .Where(t => t.Attribute(GranitXml.Constants.TransactionIdAttribute).Value == newTa.TransactionId.ToString()).Count(), 1);

      }
    }

    [TestMethod()]
    public void Sort_AmountAscending_Test()
    {
      foreach (var xml in goodXmlExamples)
      {
        Amount_Sort_and_MinMax_Test(xml, SortOrder.Ascending);
      }
    }

    [TestMethod()]
    public void Sort_AmountDescending_Test()
    {
      foreach (var xml in goodXmlExamples)
        Amount_Sort_and_MinMax_Test(xml, SortOrder.Descending);
    }

    private static void Amount_Sort_and_MinMax_Test(string xml, SortOrder order)
    {
      var x2o = new GranitXmlToAdapterBinder(xml, true);

      x2o.Sort(Constants.AmountPropertyName, order);
      decimal maxAmount = x2o.HUFTransactionsAdapter.TransactionAdapters.Max(ta => ta.Amount);
      decimal minAmount = x2o.HUFTransactionsAdapter.TransactionAdapters.Min(ta => ta.Amount);
      string firstAmount = x2o.GranitXDocument.Root.Elements(GranitXml.Constants.Transaction).First().Element(GranitXml.Constants.Amount).Value;
      string lastAmount = x2o.GranitXDocument.Root.Elements(GranitXml.Constants.Transaction).Last().Element(GranitXml.Constants.Amount).Value;

      string[] amounts = x2o.GranitXDocument.Root.Elements(GranitXml.Constants.Transaction).Elements(GranitXml.Constants.Amount).Select(t => t.Value).ToArray();
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
      Assert.AreEqual(x2o.History.UndoCount, 1);
    }


    [TestMethod()]
    public void Sort_ReqDateAscending_Test()
    {
      foreach (var xml in goodXmlExamples)
        Sort_ReqDate_Test(xml, SortOrder.Ascending);
    }

    [TestMethod()]
    public void Sort_ReqDateDescending_Test()
    {
      foreach (var xml in goodXmlExamples)
        Sort_ReqDate_Test(xml, SortOrder.Descending);
    }

    [TestMethod()]
    public void Sort_ReqDateAscending_IncreasesUndoCount_Test()
    {
      foreach (var xml in goodXmlExamples)
        Sort_UndoCount_Test(xml, SortOrder.Ascending);
    }

    [TestMethod()]
    public void Sort_ReqDateDescending_IncreasesUndoCount_Test()
    {
      foreach (var xml in goodXmlExamples)
        Sort_UndoCount_Test(xml, SortOrder.Descending);
    }

    private static void Sort_UndoCount_Test(string xml, SortOrder order)
    {
      var x2o = new GranitXmlToAdapterBinder(xml, true);
      Assert.AreEqual(x2o.History.UndoCount, 0);
      x2o.Sort(Constants.ExecutionDatePropertyName, order);
      Assert.AreEqual(x2o.History.UndoCount, 1);
    }

    private static void Sort_ReqDate_Test(string xml, SortOrder order)
    {
      var x2o = new GranitXmlToAdapterBinder(xml, true);

      x2o.Sort(Constants.ExecutionDatePropertyName, order);

      DateTime maxDate = x2o.HUFTransactionsAdapter.TransactionAdapters.Max(ta => ta.ExecutionDate);
      DateTime minDate = x2o.HUFTransactionsAdapter.TransactionAdapters.Min(ta => ta.ExecutionDate);
      string firstDate = x2o.GranitXDocument.Root.Elements(GranitXml.Constants.Transaction).InDocumentOrder().First().Element(GranitXml.Constants.RequestedExecutionDate).Value;
      string lastDate = x2o.GranitXDocument.Root.Elements(GranitXml.Constants.Transaction).InDocumentOrder().Last().Element(GranitXml.Constants.RequestedExecutionDate).Value;

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
    public void AddTransactionRow_Test()
    {
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        int origCount = x2o.TransactionCount;
        var newTa = new TransactionAdapter();

        Assert.AreEqual(x2o.History.UndoCount, 0);

        x2o.AddTransactionRow(newTa);

        Assert.AreEqual(x2o.History.UndoCount, 1);
        Assert.AreEqual(x2o.GranitXDocument.Root.Elements().ToList().Count, origCount + 1);
        foreach (var ta in x2o.HUFTransactionsAdapter.TransactionAdapters)
          Assert.IsTrue(newTa.TransactionId >= ta.TransactionId);
        Assert.AreEqual(x2o.GranitXDocument.Root.Elements(GranitXml.Constants.Transaction)
        .Where(t => t.Attribute(GranitXml.Constants.TransactionIdAttribute).Value == newTa.TransactionId.ToString()).Count(), 1);
      }
    }

    [TestMethod()]
    public void AddTransactionRow_ExistingTAdapter_Test()
    {
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        int origCount = x2o.TransactionCount;

        var newTa = x2o.HUFTransactionsAdapter.TransactionAdapters[0];

        Assert.AreEqual(x2o.History.UndoCount, 0);

        x2o.AddTransactionRow(newTa);

        Assert.AreEqual(x2o.History.UndoCount, 0);
        Assert.AreEqual(x2o.GranitXDocument.Root.Elements().ToList().Count, origCount);

        Assert.AreEqual(x2o.GranitXDocument.Root.Elements(GranitXml.Constants.Transaction)
        .Where(t => t.Attribute(GranitXml.Constants.TransactionIdAttribute).Value == newTa.TransactionId.ToString()).Count(), 1);
      }
    }

    [TestMethod()]
    public void RemoveTransactionRowById_Test()
    {
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        int origCount = x2o.TransactionCount;
        Assert.AreEqual(x2o.History.UndoCount, 0);

        long idToRemove = x2o.HUFTransactionsAdapter.TransactionAdapters.Min(x => x.TransactionId);

        x2o.RemoveTransactionRowById(idToRemove);

        Assert.AreEqual(x2o.History.UndoCount, 1);
        Assert.AreEqual(x2o.GranitXDocument.Root.Elements().ToList().Count, origCount - 1);
        Assert.AreEqual(x2o.TransactionCount, origCount - 1);
      }
    }

    [TestMethod()]
    public void SaveToFile_Test()
    {
      string tempXml = "unittest.temp.xml";
      foreach (var xml in goodXmlExamples)
      {
        var orig = new GranitXmlToAdapterBinder(xml, validate: true);

        long idToSelect1 = orig.HUFTransactionsAdapter.TransactionAdapters.Min(x => x.TransactionId);
        long idToSelect2 = orig.HUFTransactionsAdapter.TransactionAdapters.Max(x => x.TransactionId);

        orig.HUFTransactionsAdapter.TransactionAdapters[1].IsSelected = false;
        orig.HUFTransactionsAdapter.TransactionAdapters[orig.TransactionCount - 2].IsSelected = false;

        orig.SaveToFile(tempXml);
        var saved = new GranitXmlToAdapterBinder(tempXml, validate: true);
        int i = 0;
        foreach (var t in saved.HUFTransactionsAdapter.TransactionAdapters)
        {
          var origTransaction = orig.HUFTransactionsAdapter.TransactionAdapters[i++];

          if(origTransaction.IsSelected) // Selected are equal
            Assert.AreEqual(t.CompareTo(origTransaction), 0);
          else                           // else compare returns -2
            Assert.AreEqual(t.CompareTo(origTransaction), -2);
        }
      }
    }

    [TestMethod()]
    public void GranitXmlDocContains_Test()
    {
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);

        int origCount = x2o.TransactionCount;

        Assert.IsTrue(x2o.GranitXmlDocumentContains(x2o.HUFTransactionsAdapter.TransactionAdapters[0]));
        Assert.IsFalse(x2o.GranitXmlDocumentContains(new TransactionAdapter()));
        Assert.AreEqual(x2o.History.UndoCount, 0);
      }
    }

    [TestMethod()]
    public void CompareGranitXDocuments_BothNull_Test()
    {
      Assert.AreEqual(GranitXmlToAdapterBinder.CompareGranitXDocuments(null, null), 0);
    }

    [TestMethod()]
    public void CompareGranitXDocuments_OneNull_Test()
    { 
      foreach (var xml in goodXmlExamples)
      {
        var x2o = new GranitXmlToAdapterBinder(xml, true);
        Assert.AreEqual(GranitXmlToAdapterBinder.CompareGranitXDocuments(x2o.GranitXDocument, null), -1);
        Assert.AreEqual(GranitXmlToAdapterBinder.CompareGranitXDocuments(null, x2o.GranitXDocument), -1);
      }
    }

    [TestMethod()]
    public void CompareGranitXDocuments_Test()
    { 
        var x1 = new GranitXmlToAdapterBinder("example.xml", true);
        var x2 = new GranitXmlToAdapterBinder("test.xml", true);
        Assert.AreNotEqual(GranitXmlToAdapterBinder.CompareGranitXDocuments(x1.GranitXDocument, x2.GranitXDocument), 0);
    }
  }
}