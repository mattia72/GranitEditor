using Microsoft.VisualStudio.TestTools.UnitTesting;
using GranitEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GranitEditor.Tests
{
  [TestClass()]
  public class GranitDataGridViewCellFormatterTests
  {
    [TestMethod()]
    public void FormatAccountNumberTest()
    {
      string origValue = "1234567812345678";
      var e = new DataGridViewCellFormattingEventArgs(0, 0, origValue, typeof(string), null);

      GranitDataGridViewCellFormatter.FormatAccountNumber(e);
      Assert.AreEqual("12345678-12345678-00000000", e.Value);
      Assert.IsTrue(e.FormattingApplied);

      e.Value = "12345678-12345678-12";
      GranitDataGridViewCellFormatter.FormatAccountNumber(e);
      Assert.AreEqual("12345678-12345678-12000000", e.Value);
      Assert.IsTrue(e.FormattingApplied);

      e.Value = "12345678-12345678";
      GranitDataGridViewCellFormatter.FormatAccountNumber(e);
      Assert.AreEqual("12345678-12345678-00000000", e.Value);
      Assert.IsTrue(e.FormattingApplied);

      e.Value = "1234567812345678000000000";
      GranitDataGridViewCellFormatter.FormatAccountNumber(e);
      Assert.AreEqual("12345678-12345678-00000000", e.Value);
      Assert.IsTrue(e.FormattingApplied);

      e.Value = "123456781234567812345678";
      GranitDataGridViewCellFormatter.FormatAccountNumber(e);
      Assert.AreEqual("12345678-12345678-12345678", e.Value);
      Assert.IsTrue(e.FormattingApplied);

      e.Value = "12345678-12345678-12345678";
      GranitDataGridViewCellFormatter.FormatAccountNumber(e);
      Assert.AreEqual("12345678-12345678-12345678", e.Value);
      Assert.IsTrue(e.FormattingApplied);
    }

    [TestMethod()]
    public void UnFormatAccountNumberTest()
    {
      string origValue = "12345678-12345678";
      var e = new DataGridViewCellFormattingEventArgs(0, 0, origValue, typeof(string), null);

      GranitDataGridViewCellFormatter.UnFormatAccountNumber(e);
      Assert.AreEqual("123456781234567800000000", e.Value);
      Assert.IsTrue(e.FormattingApplied);

      e.Value = "12345678-12345678-12345678";
      GranitDataGridViewCellFormatter.UnFormatAccountNumber(e);
      Assert.AreEqual("123456781234567812345678", e.Value);
      Assert.IsTrue(e.FormattingApplied);
    }

    [TestMethod()]
    public void AddNullsToTheEndTest()
    {
      string val = "12";
      string val2 = GranitDataGridViewCellFormatter.AddNullsToTheEnd(val);
      Assert.AreEqual("12000000", val2);

      val = "";
      val2 = GranitDataGridViewCellFormatter.AddNullsToTheEnd(val);
      Assert.AreEqual("00000000", val2);

      val = "12345678";
      val2 = GranitDataGridViewCellFormatter.AddNullsToTheEnd(val);
      Assert.AreEqual("12345678", val2);
    }
  }
}