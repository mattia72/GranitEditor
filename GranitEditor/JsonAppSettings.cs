using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace GranitEditor
{
  public class GranitSettings : JsonAppSettings<GranitSettings>, IComparable<GranitSettings>
  {
    public DataGridViewAutoSizeColumnsMode AlignTable = DataGridViewAutoSizeColumnsMode.None;
    public string SchemaFilePath = "HUFTransactions.xsd";
    public List<string> LastOpenedFilePath = new List<string>();
    public Point WindowLocation = new Point(0, 0);
    public Size WindowSize = new Size(838, 360);

    public int CompareTo(GranitSettings other)
    {
      int retVal;
      retVal = AlignTable.CompareTo(other.AlignTable);
      if (retVal == 0) retVal = SchemaFilePath.CompareTo(other.SchemaFilePath);
      for (int i = 0; i < LastOpenedFilePath.Count; i++) {
        if (retVal == 0) retVal = LastOpenedFilePath[i].CompareTo(other.LastOpenedFilePath[i]);
      }
      if (retVal == 0) retVal = WindowLocation.X.CompareTo(other.WindowLocation.X);
      if (retVal == 0) retVal = WindowLocation.Y.CompareTo(other.WindowLocation.Y);
      if (retVal == 0) retVal = WindowSize.Height.CompareTo(other.WindowSize.Height);
      if (retVal == 0) retVal = WindowSize.Height.CompareTo(other.WindowSize.Height);
      return retVal;
    }
  }

  //Todo: change user settigs to json
  public class JsonAppSettings<T> where T : new()
  {
    private const string DEFAULT_FILENAME = "settings.json";

    public void Save(string fileName = DEFAULT_FILENAME)
    {
      File.WriteAllText(fileName, new JavaScriptSerializer().Serialize(this));
    }

    public string ToJson()
    {
      return new JavaScriptSerializer().Serialize(this);
    }

    public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
    {
      File.WriteAllText(fileName, new JavaScriptSerializer().Serialize(pSettings));
    }

    public static T Load(string fileName = DEFAULT_FILENAME)
    {
      T t = new T();
      if (File.Exists(fileName))
        t = new JavaScriptSerializer().Deserialize<T>(File.ReadAllText(fileName));
      return t;
    }
  }
}