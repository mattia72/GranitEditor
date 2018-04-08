using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;

namespace ExtensionMethods
{
  public static class XCommentExtension
  {
    public static bool IsCommentedXElement(this XComment xc)
    {
      XElement xe = GetCommentedXElement(xc);
      return (xe != null);
    }

    public static XElement UnCommentXElmenet(this XComment xc)
    {
      return GetCommentedXElement(xc);
    }

    public static XElement GetCommentedXElement(XComment xc)
    {
      XElement xe = null;
      try
      {
        if(xc.Value.Contains("<"))
          xe = XElement.Parse(xc.Value);
      }
      catch (XmlException e)
      {
        Debug.WriteLine(string.Format("{0} not valid xml element. Exception: {1}", xc.Value, e.Message));
      }

      return xe;
    }
  }
}
