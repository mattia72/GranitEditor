/* 
 Licensed under the Apache License, Version 2.0
 Xml2CSharp.cs
 http://www.apache.org/licenses/LICENSE-2.0
*/

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GranitXml
{
  [XmlRoot(ElementName = GranitXml.Constants.HUFTransactions)]
  public class HUFTransaction : IComparable<HUFTransaction> //, ICloneable
  {
    [XmlElement(ElementName = "Transaction")]
    public List<Transaction> Transactions { get; set; }

    public HUFTransaction()
    {
      Transactions = new List<Transaction>();
    }

    public HUFTransaction(List<Transaction> transactionList)
    {
      Transactions = transactionList; 
    }

    public static HUFTransaction Load(XDocument xdoc)
    {
      return SerializeHUFTransactions(xdoc);
    }

    public static HUFTransaction Load(string xmlPath)
    {
      XDocument x = XDocument.Load(xmlPath);
      return SerializeHUFTransactions(x);
    }

    public static HUFTransaction Parse(string xml)
    {
      XDocument x = XDocument.Parse(xml);
      return SerializeHUFTransactions(x);
    }

    private static HUFTransaction SerializeHUFTransactions(XDocument x)
    {
      XmlRootAttribute xRoot = new XmlRootAttribute
      {
        ElementName = Constants.HUFTransactions,
        IsNullable = true
      };
      var ser = new XmlSerializer(typeof(HUFTransaction), xRoot);
      return x == null ? null : (HUFTransaction)ser.Deserialize(x.CreateReader());
    }

    public int CompareTo(HUFTransaction other)
    {
      if (other == null)
        return -1;

      int comp = Transactions.Count - other.Transactions.Count;
      if (comp != 0)
        return comp;

      for (int i=0;  i < Transactions.Count; i++)
      {
        comp = Transactions[i].CompareTo(other.Transactions.Count > i ? other.Transactions[i] : null);

        if (comp != 0)
          break;
      }
      return comp;
    }
  }
}
