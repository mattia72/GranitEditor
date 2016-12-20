using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GranitXMLEditor
{
  public static class XDocumentExtension
  {
    public static bool IsEmpty(this XDocument element)
    {
      return element.Elements().Count() == 0;
    }

    public static void SortDescendantElementsByElementValue(this XDocument x, string elementName, string elementValue, SortOrder sortOrder)
    {
      IEnumerable<XElement> sortedElements = null;
      if (sortOrder == SortOrder.Ascending )
        sortedElements = x.Descendants(elementName).OrderBy(e => e.Element(elementValue).Value);
      else if (sortOrder == SortOrder.Descending )
        sortedElements = x.Descendants(elementName).OrderByDescending(e => e.Element(elementValue).Value);

      if (sortedElements != null)
        sortedElements.First().Parent.ReplaceNodes(sortedElements); // and now we lost comments from parent node...

      //{// this method doesn't work, but bubleshort could help
      //  int j = 0;
      //  var transactionNodes = x.Elements().Where(t => t.Attribute(Constants.TransactionAttribute) != null).ToArray();
      //  for (int i = 0; i < transactionNodes.Count(); i++)
      //  {
      //    var elem = transactionNodes[i];
      //    if (elem.GetType() == typeof(XElement))
      //    {
      //      elem.ReplaceWith(sortedElements.ToArray()[j++]);
      //    }
      //    if (j >= sortedElements.Count())                
      //      break;
      //  }
      //}
    }
  }
}
