using System.Xml.Linq;

namespace GranitEditor
{
  public interface IGranitXDocumentOwner
  {
    XDocument GranitXDocument { get; set; }
  }
}
