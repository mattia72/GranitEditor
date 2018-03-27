using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Globalization;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;

namespace GranitEditor
{
  public class TransactionAdapter : IComparable<XElement>,
    IComparable<Transaction>,
    IComparable<TransactionAdapter>,
    IBindable<XElement>, 
    ICloneable,
    INotifyPropertyChanged,
    INotifyPropertyChanging
  {

    public Transaction Transaction { get; set; }

    public bool IsSelected
    {
      get { return (Transaction.IsSelected); }
      set
      {
        UpdateGranitXDocument(Constants.IsSelected, value.ToString().ToLower());
        Transaction.IsSelected = value;
        Debug.WriteLine("IsActiv property set to {0} for T id:{1}", value, Transaction.TransactionId);
      }

    }
    public string Originator
    {
      get { return (Transaction.Originator.Account.AccountNumber); }
      set
      {
        UpdateGranitXDocument(Constants.Originator, value);
        Transaction.Originator.Account.AccountNumber = value;
      }
    }

    public string BeneficiaryName
    {
      get { return Transaction.Beneficiary.Name; }
      set
      {
        UpdateGranitXDocument(Constants.BeneficiaryName, value);
        Transaction.Beneficiary.Name = value;
      }
    }

    public string BeneficiaryAccount
    {
      get { return (Transaction.Beneficiary.Account.AccountNumber); }
      set
      {
        UpdateGranitXDocument(Constants.BeneficiaryAccount, value);
        Transaction.Beneficiary.Account.AccountNumber = value;
      }
    }

    public decimal Amount
    {
      get { return Transaction.Amount.Value; }
      set
      {
        UpdateGranitXDocument(Constants.Amount, value.ToString(Constants.AmountFormatString, CultureInfo.InvariantCulture));
        Transaction.Amount.Value = value;
      }
    }

    public string Currency
    {
      get { return Transaction.Amount.Currency; }
      set
      {
        UpdateGranitXDocument(Constants.Currency, value);
        Transaction.Amount.Currency = value;
      }
    }

    public DateTime ExecutionDate
    {
      get
      {
        if (string.IsNullOrEmpty(Transaction.RequestedExecutionDate))
          return DateTime.Today;
        return DateTime.Parse(Transaction.RequestedExecutionDate, new CultureInfo("HU-hu"));
      }
      set
      {
        UpdateGranitXDocument(Constants.RequestedExecutionDate, value.ToString(Constants.DateFormat));
        Transaction.RequestedExecutionDate = value.ToString(Constants.DateFormat);
      }
    }

    public string RemittanceInfo
    {
      get
      {
        if (Transaction.RemittanceInfo.Text == null)
          return string.Empty;
        return string.Join("|", Transaction.RemittanceInfo.Text);
      }
      set
      {
        UpdateGranitXDocument(Constants.RemittanceInfo, value);
        Transaction.RemittanceInfo.Text = value.Split('|').ToList();
      }
    }

    public long TransactionId { get { return Transaction.TransactionId;} }

    public XDocument GranitXDocument { get; set; }

    public TransactionAdapter()
    {
      Transaction = new Transaction();
    }

    public TransactionAdapter(Transaction t, XDocument xdoc)
    {                                
      Transaction = t;
      GranitXmlToAdapterBinder.Bind(ref xdoc, this);
      GranitXDocument = xdoc;
    }

    public event PropertyChangingEventHandler PropertyChanging;

    protected void OnPropertyChanging(string propertyName)
    {
      PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void UpdateGranitXDocument(string field, string value)
    {
      if (GranitXDocument == null)
        return;

      var xt = GranitXDocument.Root.Elements(Constants.Transaction)
          .Where(x => this.IsBindedWith(x)).ToList()
          .FirstOrDefault();

      if (xt == null)
        throw new InvalidOperationException("No XElement binded with this adapter.");

      OnPropertyChanging(field);
      switch (field)
      {
        case Constants.IsSelected:
          xt.Attribute(Constants.TransactionSelectedAttribute).Value = value;
          break;
        case Constants.Originator:
          xt.Element(Constants.Originator).Element(Constants.Account).Element(Constants.AccountNumber).Value = value;
          break;
        case Constants.BeneficiaryName:
          xt.Element(Constants.Beneficiary).Element(Constants.Name).Value = value;
          break;
        case Constants.BeneficiaryAccount:
          xt.Element(Constants.Beneficiary).Element(Constants.Account).Element(Constants.AccountNumber).Value = value;
          break;
        case Constants.Amount:
          xt.Element(Constants.Amount).Value = value;
          break;
        case Constants.Currency:
          xt.Element(Constants.Amount).Attribute(Constants.Currency).Value = value;
          break;
        case Constants.RequestedExecutionDate:
          xt.Element(Constants.RequestedExecutionDate).Value = value;
          break;
        case Constants.RemittanceInfo:
          UpdateRemittanceInfo(xt, value);
          break;
      }

      OnPropertyChanged(field);
      Debug.WriteLine(string.Format("TransactionAdapter.UpdateGranitXDocument: {0} updated with: {1}", field, value));

    }

    public int CompareTo(XElement x)
    {
      if (x == null) return 1;

      var serializer = new XmlSerializer(typeof(XDocument));
      var other = (Transaction)serializer.Deserialize(x.CreateReader());

      return CompareTo(other);
    }

    public int CompareTo(TransactionAdapter other)
    {
      if (0 != Amount.CompareTo(other.Amount)) return Amount.CompareTo(other.Amount);
      if (0 != Originator.CompareTo(other.Originator)) return Originator.CompareTo(other.Originator);
      if (0 != BeneficiaryName.CompareTo(other.BeneficiaryName)) return BeneficiaryName.CompareTo(other.BeneficiaryName);
      if (0 != BeneficiaryAccount.CompareTo(other.BeneficiaryAccount)) return BeneficiaryAccount.CompareTo(other.BeneficiaryAccount);
      if (0 != Currency.CompareTo(other.Currency)) return Currency.CompareTo(other.Currency);
      if (0 != ExecutionDate.CompareTo(other.ExecutionDate)) return ExecutionDate.CompareTo(other.ExecutionDate);
      if (0 != RemittanceInfo.CompareTo(other.RemittanceInfo)) return RemittanceInfo.CompareTo(other.RemittanceInfo);

      return Transaction.CompareTo(other.Transaction);
    }

    public int CompareTo(Transaction other)
    {
      return Transaction.CompareTo(other);
    }

    private static void UpdateRemittanceInfo(XElement xt, string value)
    {
      var texts = value.Split('|');
      var xTexts = xt.Element(Constants.RemittanceInfo).Elements(Constants.Text).ToArray();
      int i = 0;
      for (; i < texts.Length; i++)
      {
        if (i < xTexts.Length)
          xt.Element(Constants.RemittanceInfo).Elements(Constants.Text).ToArray()[i].Value = texts[i].Trim();
        else
        {
          xt.Element(Constants.RemittanceInfo).Add(new XElement(Constants.Text, texts[i].Trim()));
        }
      }
      while (i < xTexts.Length)
      {
        xt.Element(Constants.RemittanceInfo).Elements(Constants.Text).ToArray()[i++].Value = "";
      }
      xt.Element(Constants.RemittanceInfo).Elements(Constants.Text).Where(x => x.Value == "").Remove();
    }

    public bool IsBindedWith(XElement t)
    {
      return Transaction.IsBindedWith(t);
    }

    private class SortAmountAscendingHelper : IComparer<TransactionAdapter>
    {
      public int Compare(TransactionAdapter x, TransactionAdapter y)
      {
        return x.Amount.CompareTo(y.Amount);
      }
    }

    public static IComparer<TransactionAdapter> SortAmountAscending()
    {
      return (IComparer<TransactionAdapter>)new SortAmountAscendingHelper();
    }

    private class SortAmountDescendingHelper : IComparer<TransactionAdapter>
    {
      public int Compare(TransactionAdapter x, TransactionAdapter y)
      {
        return y.Amount.CompareTo(x.Amount);
      }
    }

    public static IComparer<TransactionAdapter> SortAmountDescending()
    {
      return (IComparer<TransactionAdapter>) new SortAmountDescendingHelper();
    }

    public object Clone()
    {
      TransactionAdapter clone = new TransactionAdapter((Transaction)Transaction.Clone(), GranitXDocument);
      return clone;
    }

    public override string ToString()
    {
      return string.Format("Id: {0}, From: {1}, To: {2}, Date: {3}, Amount: {4}", TransactionId, Originator, BeneficiaryName, ExecutionDate, Amount);
    }
  }
}
