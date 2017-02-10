using GranitXMLEditor.Properties;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;

namespace GranitXMLEditor
{
  public static class XDocumentExtension
  {
    public static bool IsEmpty(this System.Xml.Linq.XDocument element)
    {
      return element.Elements().Count() == 0;
    }
    /// <summary>
    /// Sorts immediate child elements by XPath to a value
    /// </summary>
    public static void SortElementsByXPathElementValue(this System.Xml.Linq.XDocument x, string nameOfElementToSort, string xPathToValueSortBy, SortOrder sortOrder)
    {
      IEnumerable<XElement> sortedElements = null;
      if (sortOrder == SortOrder.Ascending)
      {
        sortedElements = SortElementsAscendingByXPathElementValue(x, nameOfElementToSort, xPathToValueSortBy);
      }
      else if (sortOrder == SortOrder.Descending)
      {
        sortedElements = SortElementsDescendingByXPathElementValue(x, nameOfElementToSort, xPathToValueSortBy);
      }

      if (sortedElements != null)
        sortedElements.First().Parent.ReplaceNodes(sortedElements); // and now we lost comments from parent node... BUG 15
    }

    /// <summary>
    /// Sorts immediate child elements by XPath to an attribute or a value
    /// </summary>
    public static void SortElementsByXPathEvaluate(this System.Xml.Linq.XDocument x, string nameOfElementToSort, string xPathToValueSortBy, SortOrder sortOrder)
    {
      IEnumerable<XElement> sortedElements = null;
      if (sortOrder == SortOrder.Ascending)
      {
        sortedElements = SortElementsAscendingByXPathEvaluate(x, nameOfElementToSort, xPathToValueSortBy);
      }
      else if (sortOrder == SortOrder.Descending)
      {
        sortedElements = SortElementsDescendingByXPathEvaluate(x, nameOfElementToSort, xPathToValueSortBy);
      }

      if (sortedElements != null)
        sortedElements.First().Parent.ReplaceNodes(sortedElements); // and now we lost comments from parent node... BUG 15
    }

    public static XDocument ValidateAndLoad( this XDocument x, string xmlPath, string schemaPath, ref ValidationEventArgs validationEventArgs)
    {
      validationEventArgs = Validate(schemaPath, xmlPath);
      return XDocument.Load(xmlPath);
    }

    private static ValidationEventArgs Validate(string schemaPath, string xmlPath)
    {
      XDocument doc = XDocument.Load(xmlPath, LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo);

      XmlSchemaSet schemaSet = new XmlSchemaSet();
      schemaSet.Add("", schemaPath);

      XmlReaderSettings settings = new XmlReaderSettings();
      settings.ValidationType = ValidationType.Schema;
      settings.ValidationFlags =
              XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessSchemaLocation;
      settings.CloseInput = true;
      settings.Schemas = schemaSet;

      ValidationEventArgs eventArgs = null;
      settings.ValidationEventHandler += (o, e) =>
      {
        eventArgs = e;
        string text = $"[Line: {e.Exception?.LineNumber}, Column: {e.Exception?.LinePosition}]: {e.Message}";
        Debug.WriteLine(text);
      };

      using (XmlReader xrv = XmlReader.Create(doc.CreateReader(), settings))
      {
        while (xrv.Read()) { }
      }

      return eventArgs;
    }

    private static IEnumerable<XElement> SortElementsDescendingByXPathElementValue(System.Xml.Linq.XDocument x, string nameOfElementToSort, string xPathToValueSortBy)
    {
      return x.Root.Elements(nameOfElementToSort)
        .OrderByDescending(
        elem => elem.XPathSelectElement(xPathToValueSortBy).Value);
    }

    private static IEnumerable<XElement> SortElementsAscendingByXPathElementValue(System.Xml.Linq.XDocument x, string nameOfElementToSort, string xPathToValueSortBy)
    {
      return from elem in x.Root.Elements(nameOfElementToSort)
             orderby elem.XPathSelectElement(xPathToValueSortBy).Value
             select elem;
    }

    private static IEnumerable<XElement> SortElementsDescendingByXPathEvaluate(System.Xml.Linq.XDocument x, string nameOfElementToSort, string xPathToValueSortBy)
    {
      return x.Root.Elements(nameOfElementToSort)
        .OrderByDescending(
        elem => elem.XPathEvaluate("string(" + xPathToValueSortBy +")") as string );
    }

    private static IEnumerable<XElement> SortElementsAscendingByXPathEvaluate(System.Xml.Linq.XDocument x, string nameOfElementToSort, string xPathToValueSortBy)
    {
      return from elem in x.Root.Elements(nameOfElementToSort)
             orderby elem.XPathEvaluate("string(" + xPathToValueSortBy +")") as string
             select elem;
    }


  }
}
