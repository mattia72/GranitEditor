using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GranitEditor
{
  // Threadsafe Singleton
  public sealed class UserSettings
  {
    public const string DEFAULT_FILENAME = "settings_default.json";
    public const string FILENAME = "settings.json";

    private static GranitSettings settingsInstance;
    private static readonly Func<GranitSettings> objectFactory = () =>
    {
      var filePath = GetSettingsFilePath(FILENAME);
      if (!File.Exists(filePath))
      {
        filePath = GetSettingsFilePath(DEFAULT_FILENAME);
      }
      GranitSettings obj;
      try
      {
        obj = GranitSettings.LoadFromFile(filePath);
      }
      catch (Exception)
      {
        obj = GranitSettings.LoadFromFile(DEFAULT_FILENAME);
      }
      return obj;
    };
    private static readonly Lazy<GranitSettings> lazy = new Lazy<GranitSettings>(objectFactory);

    public static GranitSettings Instance
    {
      get
      {
        if (null == settingsInstance)
        {
          settingsInstance = lazy.Value;
        }
        return settingsInstance;
      }
    }

    private UserSettings()
    {
      settingsInstance = null;
    }

    private static string GetSettingsFilePath(string fileName)
    {
      return Path.Combine(Application.StartupPath, fileName);
    }

    public static void Save()
    {
      Instance.Save(GetSettingsFilePath(FILENAME));
    }

  }
  public class GranitXMLFormSettings : IComparable<GranitXMLFormSettings>, IEqualityComparer<GranitXMLFormSettings>, IComparable
  {
    public string FilePath;
    public DataGridViewAutoSizeColumnsMode AlignTable;

    public GranitXMLFormSettings()
    { }

    public GranitXMLFormSettings(string filePath, DataGridViewAutoSizeColumnsMode alignTable)
    {
      FilePath = filePath;
      AlignTable = alignTable;
    }

    public int CompareTo(GranitXMLFormSettings other)
    {
      int retVal = AlignTable.CompareTo(other.AlignTable);
      if (0 == retVal) retVal = FilePath.CompareTo(other.FilePath);
      return retVal;
    }

    public int CompareTo(object obj) => CompareTo(obj as GranitXMLFormSettings);

    public override bool Equals(object obj) => 0 == CompareTo(obj as GranitXMLFormSettings);
    public bool Equals(GranitXMLFormSettings x, GranitXMLFormSettings y) => 0 == x.CompareTo(y);

    public int GetHashCode(GranitXMLFormSettings obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
      int hash = AlignTable.GetHashCode();
      hash += FilePath.GetHashCode();
      return hash;
    }
  }

  public class GranitSettings : JsonAppSettings<GranitSettings>, IComparable<GranitSettings>, IEqualityComparer<GranitSettings>
  {
    public string SchemaFilePath;
    public List<GranitXMLFormSettings> LastOpenedFilePaths;
    public List<string> RecentFileList;
    public Point WindowLocation;
    public Size WindowSize;
    public Constants.WindowLayout WindowLayout;
    public int MruListItemLength;

    public GranitSettings()
    {
      SetDefaultValues();
    }

    public void SetDefaultValues()
    {
      SchemaFilePath = "HUFTransactions.xsd";
      LastOpenedFilePaths = new List<GranitXMLFormSettings>
      {
        new GranitXMLFormSettings("example.xml", DataGridViewAutoSizeColumnsMode.None)
      };

      RecentFileList = new List<string>
      {
        "example.xml"
      };
      WindowLocation = new Point(0, 0);
      WindowSize = new Size(838, 360);
      WindowLayout = Constants.WindowLayout.Tabbed;
      MruListItemLength = 10;
    }

    public int CompareTo(GranitSettings other)
    {
      int retVal = SchemaFilePath.CompareTo(other.SchemaFilePath);

      if (retVal == 0) retVal = ComapareList<GranitXMLFormSettings>(LastOpenedFilePaths, other.LastOpenedFilePaths);
      if (retVal == 0) retVal = ComapareList<string>(RecentFileList, other.RecentFileList);

      if (retVal == 0) retVal = WindowLocation.X.CompareTo(other.WindowLocation.X);
      if (retVal == 0) retVal = WindowLocation.Y.CompareTo(other.WindowLocation.Y);
      if (retVal == 0) retVal = WindowSize.Height.CompareTo(other.WindowSize.Height);
      if (retVal == 0) retVal = WindowSize.Height.CompareTo(other.WindowSize.Height);
      if (retVal == 0) retVal = WindowLayout.CompareTo(other.WindowLayout);
      if (retVal == 0) retVal = MruListItemLength.CompareTo(other.MruListItemLength);
      return retVal;
    }

    private static int ComapareList<T>(List<T> one, List<T> other) where T : IComparable
    {
      int retVal, i = 0;
      retVal = one.Count.CompareTo(other.Count);
      if (retVal == 0)
        retVal = one.Sum(o => o.CompareTo(other[i++]));
      return retVal;
    }

    public int GetHashCode(GranitSettings obj)
    {
      var hash = obj.SchemaFilePath.GetHashCode();
      hash += obj.LastOpenedFilePaths.GetSequenceHashCode();
      hash += obj.RecentFileList.GetSequenceHashCode();
      hash += obj.WindowLocation.GetHashCode();
      hash += obj.WindowSize.GetHashCode();
      hash += obj.WindowLayout.GetHashCode();
      hash += obj.MruListItemLength.GetHashCode();
      return hash;
    }

    public override bool Equals(object obj) => 0 == CompareTo(obj as GranitSettings);

    public bool Equals(GranitSettings x, GranitSettings y) => x.CompareTo(y) == 0;

    public override int GetHashCode()
    {
      return GetHashCode(this);
    }
  }
}