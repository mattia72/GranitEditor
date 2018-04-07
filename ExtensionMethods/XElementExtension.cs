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

    public static XComment CommentXElmenet(this XElement xe)
    {
      XComment xc = new XComment(xe.ToString());
      return xc;
    }

    public static XElement UnCommentXElmenet(this XElement xe, XComment xc)
    {
      return xc.UnCommentXElmenet();
    }
  }
}
