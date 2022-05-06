using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace GranitEditor
{
  // Todo: change user settigs to json
  public class JsonAppSettings<T> where T : new()
  {
    public void Save(string filePath)
    {
      File.WriteAllText(filePath, new JavaScriptSerializer().Serialize(this));
    }

    public string ToJson()
    {
      return new JavaScriptSerializer().Serialize(this);
    }

    public static void Save(T settings, string filePath)
    {
      File.WriteAllText(filePath, new JavaScriptSerializer().Serialize(settings));
    }

    public static T LoadFromText(string json)
    {
      return new JavaScriptSerializer().Deserialize<T>(json);
    }

    public static T LoadFromFile(string filPath)
    {
      return (File.Exists(filPath)) ? 
        new JavaScriptSerializer().Deserialize<T>(File.ReadAllText(filPath)) : new T();
    }
  }
}