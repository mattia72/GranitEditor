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
  [XmlRoot(ElementName = "Beneficiary")]
  public class Beneficiary : IComparable<Beneficiary>, ICloneable
  {
    [XmlElement(ElementName = "Name")]
    public string Name { get; set; }
    [XmlElement(ElementName = "Account")]
    public Account Account { get; set; }

    public Beneficiary()
    {
      Account = new Account();
      Name = "";
    }

    public int CompareTo(Beneficiary other)
    {
      if (0 != Account.CompareTo(other.Account))
        return Account.CompareTo(other.Account);
      if (0 != Name.CompareTo(other.Name))
        return Name.CompareTo(other.Name);
      return 0;
    }

    public object Clone()
    {
      return new Beneficiary { Account = (Account)Account.Clone(), Name = Name };
    }
  }

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


  [XmlRoot(ElementName = GranitXml.Constants.HUFTransactions)]
  public class HUFTransaction
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
        ElementName = GranitXml.Constants.HUFTransactions,
        IsNullable = true
      };
      var ser = new XmlSerializer(typeof(HUFTransaction), xRoot);
      return (HUFTransaction)ser.Deserialize(x.CreateReader());
    }
  }

}
