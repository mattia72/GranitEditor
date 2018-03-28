/* 
 Licensed under the Apache License, Version 2.0
 Xml2CSharp.cs
 http://www.apache.org/licenses/LICENSE-2.0
*/

using System;
using System.Xml.Serialization;

namespace GranitXml
{
  [XmlRoot(ElementName = "Amount")]
  public class Amount : IComparable<Amount>, ICloneable
  {
    [XmlAttribute(AttributeName = "Currency")]
    public string Currency { get; set; }
    [XmlText]
    public decimal Value { get; set; }

    public Amount()
    {
      Value = 1.00m;
      Currency = "HUF";
    }

    public Amount(decimal amount, string currency = "HUF")
    {
      Value = amount;
      Currency = currency;
    }

    public int CompareTo(Amount other)
    {
      if (Value != other.Value)
        return Value.CompareTo(other.Value);
      if (Currency != other.Currency)
        return Currency.CompareTo(other.Currency);
      return 0;
    }

    public object Clone()
    {
      return new Amount { Value = Value, Currency = Currency };
    }
  }
}