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
  }
}
