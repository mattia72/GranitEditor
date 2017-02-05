using System.Xml.Linq;

namespace GranitXMLEditor
{
  public interface IGranitXDocumentOwner
  {
    XDocument GranitXDocument { get; set; }
  }
}
