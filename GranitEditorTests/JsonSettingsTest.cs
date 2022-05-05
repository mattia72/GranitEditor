using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GranitEditor.Tests
{
  [TestClass]
  public class JsonSettingsTest
  {
    [TestMethod]
    public void TestSettingsSave()
    {
      var settings = new GranitSettings();
      var s = settings.ToJson();
      Assert.IsTrue(s.Length != 0, "Settings.ToString empty");
    }

    [TestMethod]
    public void TestSettingsRestoreComparesToOrig()
    {
      var orig = CreateSettings();
      var s = orig.ToJson();

      var settings2 = GranitSettings.LoadFromText(s);
      Assert.IsTrue(0 == orig.CompareTo(settings2));
    }

    [TestMethod]
    public void TestSettingsRestoreEqualsOrig()
    {
      GranitSettings orig = CreateSettings();
      var s = orig.ToJson();

      var settings2 = GranitSettings.LoadFromText(s);
      Assert.IsTrue(orig.Equals(settings2));
    }

    [TestMethod]
    public void TestSettingsRestoreChangeNotComparesToOrig()
    {
      var orig = CreateSettings();
      var s = orig.ToJson();
      var settings2 = GranitSettings.LoadFromText(s);
      settings2.LastOpenedFilePaths.Clear();
      Assert.IsFalse(0 == orig.CompareTo(settings2));
    }

    [TestMethod]
    public void TestSettingsRestoreChangeNotEqualsOrig()
    {
      GranitSettings orig = CreateSettings();
      var s = orig.ToJson();
      var settings2 = GranitSettings.LoadFromText(s);
      settings2.LastOpenedFilePaths.Clear();
      Assert.IsFalse(orig.Equals(settings2));
    }

    private static GranitSettings CreateSettings()
    {
      var settings = new GranitSettings
      {
        SchemaFilePath = "SchemaFile.xsd"
      };
      const int LENGTH = 10;
      for (int i = 0; i < LENGTH; i++)
      {
        settings.LastOpenedFilePaths.Add("file" + i.ToString());
      }
      settings.WindowLocation = new System.Drawing.Point(10, 20);
      settings.WindowSize = new System.Drawing.Size(10, 20);
      return settings;
    }
  }
}
