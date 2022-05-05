using System.IO;
using System.Web.Script.Serialization;

namespace GranitEditor
{
  // Todo: change user settigs to json
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

    public static T LoadFromText(string json)
    {
      return new JavaScriptSerializer().Deserialize<T>(json);
    }

    public static T LoadFromFile(string fileName = DEFAULT_FILENAME)
    {
      return (File.Exists(fileName)) ? 
        new JavaScriptSerializer().Deserialize<T>(File.ReadAllText(fileName)) : new T();
    }
  }
}