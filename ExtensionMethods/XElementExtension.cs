using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace ExtensionMethods
{
  public static class XElementExtension
  {
    public static bool IsEmpty(this XElement element)
    {
      return element.Elements().Count() == 0;
    }

    public static void CommentXElmenet(this XElement xe)
    {
      xe.ReplaceWith(new XComment(xe.ToString()));
    }

    public static void UnCommentXElmenet(this XElement xe, XComment xc)
    {
      try
      {
        xe = XElement.Parse(xc.Value);
      }
      catch (XmlException e)
      {
        Debug.WriteLine(string.Format("{0} not valid xml element. Exception: {1}", xc.Value, e.Message));
      }
      finally
      {
        if (xe != null)
        {
          xc.ReplaceWith(xe);
        }
      }
    }
  }
}
