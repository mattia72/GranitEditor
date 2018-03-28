/* 
 Licensed under the Apache License, Version 2.0
 Xml2CSharp.cs
 http://www.apache.org/licenses/LICENSE-2.0
*/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GranitXml
{
  [XmlRoot(ElementName = "RemittanceInfo")]
  public class RemittanceInfo : IComparable<RemittanceInfo>, ICloneable
  {
    [XmlElement(ElementName = "Text")]
    public List<string> Text { get; set; }

    public RemittanceInfo()
    {
      Text = new List<string>();
    }

    public object Clone()
    {
      var clone = new RemittanceInfo
      {
        Text = new List<string>(Text)
      };
      return clone;
    }

    public int CompareTo(RemittanceInfo other)
    {
      for (int i = 0; i < Text.Count; i++)
      {
        if (Text[i].CompareTo(other.Text[i]) != 0)
          return Text[i].CompareTo(other.Text[i]);
      }
      return 0;
    }
  }

}