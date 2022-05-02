using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GranitEditor.Tests
{
  [TestClass]
  public class SettingsTest
  {
    [TestMethod]
    public void TestSettingsSave()
    {
      var settings = new GranitSettings();
      var s = settings.ToJson(); 
      Assert.IsTrue(s.Length != 0, "Settings.ToString empty" );
    }
  }
}
