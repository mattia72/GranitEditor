using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GranitEditor
{
  // Threadsafe Singleton
  public sealed class UserSettings
  {
    private static readonly Lazy<GranitSettings> lazy =
          new Lazy<GranitSettings>(() => GranitSettings.LoadFromFile());

    public static GranitSettings Instance { get { return lazy.Value; } }

    private UserSettings()
    {
    }
  }

  public class GranitSettings : JsonAppSettings<GranitSettings>, IComparable<GranitSettings>, IEqualityComparer<GranitSettings>
  {
    public DataGridViewAutoSizeColumnsMode AlignTable;
    public string SchemaFilePath;
    public List<string> LastOpenedFilePaths;
    public List<string> RecentFileList;
    public Point WindowLocation;
    public Size WindowSize;
    public Constants.WindowLayout WindowLayout;
    public int MruListItemLength;

    public GranitSettings()
    {
      SetDefault();
    }

    public void SetDefault()
    {
      AlignTable = DataGridViewAutoSizeColumnsMode.None;
      SchemaFilePath = "HUFTransactions.xsd";
      LastOpenedFilePaths = new List<string>
      {
        "example.xls"
      };
      RecentFileList = new List<string>
      {
        "example.xls"
      };
      WindowLocation = new Point(0, 0);
      WindowSize = new Size(838, 360);
      WindowLayout = Constants.WindowLayout.Tabbed;
      MruListItemLength = 10;
    }

    public int CompareTo(GranitSettings other)
    {
      int retVal;
      retVal = AlignTable.CompareTo(other.AlignTable);
      if (retVal == 0) retVal = SchemaFilePath.CompareTo(other.SchemaFilePath);

      if (retVal == 0) retVal = ComapareList(LastOpenedFilePaths, other.LastOpenedFilePaths);
      if (retVal == 0) retVal = ComapareList(RecentFileList, other.RecentFileList);

      if (retVal == 0) retVal = WindowLocation.X.CompareTo(other.WindowLocation.X);
      if (retVal == 0) retVal = WindowLocation.Y.CompareTo(other.WindowLocation.Y);
      if (retVal == 0) retVal = WindowSize.Height.CompareTo(other.WindowSize.Height);
      if (retVal == 0) retVal = WindowSize.Height.CompareTo(other.WindowSize.Height);
      if (retVal == 0) retVal = WindowLayout.CompareTo(other.WindowLayout);
      if (retVal == 0) retVal = MruListItemLength.CompareTo(other.MruListItemLength);
      return retVal;
    }

    private static int ComapareList(List<string> one, List<string> other)
    {
      int retVal, i = 0;
      retVal = one.Count.CompareTo(other.Count);
      if (retVal == 0) 
        retVal = one.Sum(o => o.CompareTo(other[i++]));
      return retVal;
    }

    public int GetHashCode(GranitSettings obj)
    {
      int hash = obj.AlignTable.GetHashCode();
      hash += obj.SchemaFilePath.GetHashCode();
      hash += obj.LastOpenedFilePaths.GetSequenceHashCode();
      hash += obj.RecentFileList.GetSequenceHashCode();
      hash += obj.WindowLocation.GetHashCode();
      hash += obj.WindowSize.GetHashCode();
      hash += obj.WindowLayout.GetHashCode();
      hash += obj.MruListItemLength.GetHashCode();
      return hash;
    }

    public override bool Equals(object obj)
    {
      return 0 == CompareTo(obj as GranitSettings);    }

    public bool Equals(GranitSettings x, GranitSettings y)
    {
      return x.CompareTo(y) == 0;
    }

    public override int GetHashCode()
    {
      return GetHashCode(this);
    }
  }
}