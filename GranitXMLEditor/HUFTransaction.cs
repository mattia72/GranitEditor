/* 
 Licensed under the Apache License, Version 2.0
 Xml2CSharp.cs
 http://www.apache.org/licenses/LICENSE-2.0
*/

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GranitEditor
{
  [XmlRoot(ElementName = "Account")]
  public class Account : IComparable<Account>, ICloneable
  {
    [XmlElement(ElementName = "AccountNumber")]
    public string AccountNumber { get; set; }
    public Account()
    {
      AccountNumber = "000000000000000000000000";
    }

    public int CompareTo(Account other)
    {
      return AccountNumber.CompareTo(other.AccountNumber);
    }

    public object Clone()
    {
      return new Account { AccountNumber = AccountNumber };
    }
  }

  [XmlRoot(ElementName = "Originator")]
  public class Originator : IComparable<Originator>, ICloneable
  {
    [XmlElement(ElementName = "Account")]
    public Account Account { get; set; }
    public Originator()
    {
      Account = new Account();
    }

    public int CompareTo(Originator other)
    {
      return Account.CompareTo(other.Account);
    }

    public object Clone()
    {
      return new Originator { Account = (Account)Account.Clone() };
    }
  }

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
      var clone = new RemittanceInfo();
      clone.Text = new List<string>(Text);
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

  [XmlRoot(ElementName = "Transaction")]
  public class Transaction : IComparable<Transaction>, IBindable<XElement>, ICloneable
  {
    [XmlIgnore()]
    public static long NextTransactionId { get; set; }

    [XmlAttribute(AttributeName = "is_selected")]
    public bool IsSelected { get; set; }

    [XmlElement(ElementName = "Originator")]
    public Originator Originator { get; set; }
    [XmlElement(ElementName = "Beneficiary")]
    public Beneficiary Beneficiary { get; set; }
    [XmlElement(ElementName = "Amount")]
    public Amount Amount { get; set; }
    [XmlElement(ElementName = "RequestedExecutionDate")]
    public string RequestedExecutionDate { get; set; }
    [XmlElement(ElementName = "RemittanceInfo")]
    public RemittanceInfo RemittanceInfo { get; set; }

    [XmlAttribute(AttributeName = "id")]
    public long TransactionId { get; set; }

    public Transaction()
    {
      TransactionId = ++NextTransactionId;
      IsSelected = true;
      Originator = new Originator();
      Beneficiary = new Beneficiary();
      Amount = new Amount();
      RequestedExecutionDate = DateTime.Today.ToString(Constants.DateFormat);
      RemittanceInfo = new RemittanceInfo();
    }

    public int CompareTo(Transaction other)
    {
      if (0 != Amount.CompareTo(other.Amount))
        return Amount.CompareTo(other.Amount);
      if (0 != Originator.CompareTo(other.Originator))
        return Originator.CompareTo(other.Originator);
      if (0 != Beneficiary.CompareTo(other.Beneficiary))
        return Beneficiary.CompareTo(other.Beneficiary);
      if (0 != RequestedExecutionDate.CompareTo(other.RequestedExecutionDate))
        return RequestedExecutionDate.CompareTo(other.RequestedExecutionDate);
      if (0 != RemittanceInfo.CompareTo(other.RemittanceInfo))
        return RemittanceInfo.CompareTo(other.RemittanceInfo);

      return 0;
    }

    public bool IsBindedWith(XElement t)
    {
      return (TransactionId == long.Parse(t.Attribute(Constants.TransactionIdAttribute)?.Value));
    }

    public object Clone()
    {
      Transaction clone = new Transaction();
      clone.IsSelected = IsSelected;
      clone.Amount = (Amount)Amount.Clone();
      clone.Beneficiary = (Beneficiary)Beneficiary.Clone();
      clone.Originator = (Originator)Originator.Clone();
      clone.RequestedExecutionDate = RequestedExecutionDate;
      clone.RemittanceInfo = (RemittanceInfo)RemittanceInfo.Clone();

      return clone;
    }
  }

  [XmlRoot(ElementName = Constants.HUFTransactions)]
  public class HUFTransaction
  {
    [XmlElement(ElementName = "Transaction")]
    public List<Transaction> Transactions { get; set; }
    public HUFTransaction()
    {
      Transactions = new List<Transaction>();
    }
  }

}
