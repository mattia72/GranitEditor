using GranitEditor.Properties;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;

namespace GranitEditor
{
  public static class XDocumentExtension
  {
    public static bool IsEmpty(this XDocument element)
    {
      return element.Elements().Count() == 0;
    }

    /// <summary>
    /// Sorts immediate child elements by XPath to a string value
    /// </summary>
    public static void SortElementsByXPathToStringValue(this XDocument x, string nameOfElementToSort, string xPathToValueSortBy, SortOrder sortOrder)
    {
      IEnumerable<XElement> sortedElements = null;
      sortedElements = SortElementsByXPathElementStringValue(
        x, nameOfElementToSort, xPathToValueSortBy, sortOrder);

      if (sortedElements != null)
        sortedElements.First().Parent.ReplaceNodes(sortedElements); // and now we lost comments from parent node... BUG 15
    }

    /// <summary>
    /// Sorts immediate child elements by XPath to a string value
    /// </summary>
    public static void SortElementsByXPathToDecimalValue(this XDocument x, string nameOfElementToSort, string xPathToValueSortBy, SortOrder sortOrder)
    {
      IEnumerable<XElement> sortedElements = null;
      sortedElements = SortElementsByXPathElementDecimalValue(
        x, nameOfElementToSort, xPathToValueSortBy, sortOrder);

      if (sortedElements != null)
        sortedElements.First().Parent.ReplaceNodes(sortedElements); // and now we lost comments from parent node... BUG 15
    }

    /// <summary>
    /// Sorts immediate child elements by XPath to an attribute or a value
    /// </summary>
    public static void SortElementsByXPathEvaluate(this XDocument x, string nameOfElementToSort, string xPathToValueSortBy, SortOrder sortOrder)
    {
      IEnumerable<XElement> sortedElements = null;
      sortedElements = SortElementsDescendingByXPathEvaluateString(
        x, nameOfElementToSort, xPathToValueSortBy, sortOrder);

      if (sortedElements != null)
        sortedElements.First().Parent.ReplaceNodes(sortedElements); // and now we lost comments from parent node... BUG 15
    }

    public static XDocument ValidateAndLoad(this XDocument x, string xmlPath, string schemaPath, ref ValidationEventArgs validationEventArgs)
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

    private static IEnumerable<XElement> SortElementsByXPathElementStringValue(
      XDocument x, string nameOfElementToSort, string xPathToValueSortBy, SortOrder sortOrder)
    {
      if (sortOrder == SortOrder.Descending)
        return x.Root.Elements(nameOfElementToSort)
          .OrderByDescending(
          elem => elem.XPathSelectElement(xPathToValueSortBy).Value);
      else
        return from elems in x.Root.Elements(nameOfElementToSort)
               orderby elems.XPathSelectElement(xPathToValueSortBy).Value
               select elems;
    }

    private static IEnumerable<XElement> SortElementsByXPathElementDecimalValue(
      XDocument x, string nameOfElementToSort, string xPathToValueSortBy, SortOrder sortOrder)
    {
      if (sortOrder == SortOrder.Descending)
        return x.Root.Elements(nameOfElementToSort)
          .OrderByDescending(
            elem => decimal.Parse(elem.XPathSelectElement(xPathToValueSortBy).Value,
          NumberStyles.Number, CultureInfo.InvariantCulture));
      else
        return from elems in x.Root.Elements(nameOfElementToSort)
               orderby decimal.Parse(elems.XPathSelectElement(xPathToValueSortBy).Value,
                         NumberStyles.Number, CultureInfo.InvariantCulture)
               select elems;
    }

    private static IEnumerable<XElement> SortElementsDescendingByXPathEvaluateString(
      XDocument x, string nameOfElementToSort, string xPathToValueSortBy, SortOrder sortOrder)
    {
      if (sortOrder == SortOrder.Descending)
        return x.Root.Elements(nameOfElementToSort)
          .OrderByDescending(
          elem => elem.XPathEvaluate("string(" + xPathToValueSortBy + ")") as string);
      else
        return from elem in x.Root.Elements(nameOfElementToSort)
               orderby elem.XPathEvaluate("string(" + xPathToValueSortBy + ")") as string
               select elem;
    }
  }
}
