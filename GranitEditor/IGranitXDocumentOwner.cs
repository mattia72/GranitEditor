using System.Xml.Linq;

namespace GranitEditor
{
  public interface IGranitXDocumentOwner
  {
    /// <summary>
    /// Current XDocument
    /// </summary>
    XDocument GranitXDocument { get; set; }
    
    /// <summary>
    /// Xml file path
    /// </summary>
    string OnDiscXmlFilePath { get; set; }

    /// <summary>
    /// XDocument representation of saved file
    /// </summary>
    XDocument OnDiscXDocument { get; set; }

    /// <summary>
    /// Compare result of OnDiscXDocument and GranitXDocument
    /// </summary>
    bool DocHasPendingChanges { get; }
  }
}
