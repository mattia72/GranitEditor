/* 
 Licensed under the Apache License, Version 2.0
 Xml2CSharp.cs
 http://www.apache.org/licenses/LICENSE-2.0
*/

using System;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using ExtensionMethods;

namespace GranitXml
{
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
      RequestedExecutionDate = DateTime.Today.ToString(GranitXml.Constants.DateFormat);
      RemittanceInfo = new RemittanceInfo();
    }

    public int CompareTo(Transaction other)
    {
      if (other == null)
        return -1;

      if (IsSelected != other.IsSelected)
        return -2;

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
      Transaction clone = new Transaction
      {
        IsSelected = IsSelected,
        Amount = (Amount)Amount.Clone(),
        Beneficiary = (Beneficiary)Beneficiary.Clone(),
        Originator = (Originator)Originator.Clone(),
        RequestedExecutionDate = RequestedExecutionDate,
        RemittanceInfo = (RemittanceInfo)RemittanceInfo.Clone()
      };

      return clone;
    }

    public static Transaction CreateTransactionFromXElement(XElement xml)
    {
      var ser = new XmlSerializer(typeof(Transaction));
      return (Transaction)ser.Deserialize(xml.CreateReader());
    }

    public static void AddDefaultAttributes(XElement item, long id = 0, bool selected = true, bool overrideExisting = true)
    {
      if (item.Attribute(Constants.TransactionIdAttribute) == null)
      {
        XAttribute idAttribute = new XAttribute(Constants.TransactionIdAttribute, id == 0 ? ++NextTransactionId : id);
        item.Add(idAttribute);
      }
      else 
      {
        if(overrideExisting)
          item.Attribute(Constants.TransactionIdAttribute).Value = (id == 0 ? ++NextTransactionId : id).ToString();
      }

      if (item.Attribute(Constants.TransactionSelectedAttribute) == null)
      {
        XAttribute activeAttribute = new XAttribute(Constants.TransactionSelectedAttribute, selected);
        item.Add(activeAttribute);
      }
      else
      {
        if(overrideExisting)
          item.Attribute(Constants.TransactionSelectedAttribute).Value = selected.ToString().ToLower();
      }
    }

    public static long ConvertCommentsToTransactions(XDocument xd)
    {
      if (xd == null)
        return 0;

      long maxid = xd.Root.Elements(Constants.Transaction).
        Select(xe => xe.HasAttributes ? long.Parse(xe.Attribute(Constants.TransactionIdAttribute).Value) : 0).Max();

      foreach (var item in xd.Root.DescendantNodes().OfType<XComment>().InDocumentOrder())
      {
        XElement xe = item.UnCommentXElmenet();
        if (xe != null)
        {
          Transaction.AddDefaultAttributes(xe, ++maxid, false);
          item.ReplaceWith(xe);
        }
      }
      return maxid;
    }
  }
}
