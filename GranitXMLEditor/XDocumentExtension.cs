using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GranitXMLEditor
{
  public static class XDocumentExtension
  {
    public static bool IsEmpty(this XDocument element)
    {
      return element.Elements().Count() == 0;
    }

    public static DataTable ToDataTable(this XDocument element)
    {
      DataSet ds = new DataSet();
      string rawXml = element.ToString();
      ds.ReadXml(new StringReader(rawXml));
      return ds.Tables[0];
    }


    public static DataTable ToDataTable(this IEnumerable<XElement> elements)
    {
      return ToDataTable(new XDocument("Root", elements));
    }
  }
}
