using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms;
using System;
using System.Diagnostics;

namespace GranitXMLEditor
{
  public class GranitXmlToObjectBinder
  {
    public HUFTransaction HUFTransaction { get; private set; }
    public HUFTransactionsAdapter HUFTransactionsAdapter { get; private set; }
    public XDocument GranitXDocument { get; private set; }

    public GranitXmlToObjectBinder()
    {
      GranitXDocument = new XDocument();
      GranitXDocument.Add(new XElement(Constants.HUFTransactions));
      HUFTransaction = new HUFTransaction();
      ReCreateAdapter();
      HUFTransactionsAdapter.PropertyChanged += HUFTransactionsAdapter_PropertyChanged;
    }

    private void HUFTransactionsAdapter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      Debug.WriteLine("Property changed:" + e.PropertyName);
    }

    public GranitXmlToObjectBinder(string xmlFilePath) 
    {
      GranitXDocument = new XDocument();
      LoadXDocumentFromFile(xmlFilePath);
      LoadObjectFromXDocument(GranitXDocument);
      ReCreateAdapter();
      HUFTransactionsAdapter.PropertyChanged += HUFTransactionsAdapter_PropertyChanged;
    }

    public TransactionAdapter AddEmptyTransactionRow()
    {
      XElement transactionXelem = new TransactionXElementParser().ParsedElement;
      GranitXDocument.Root.Add(transactionXelem);
      LoadObjectFromXElement(transactionXelem);
      return ReCreateAdapter();
    }

    public void RemoveTransactionRowById( long transactionId)
    {
      HUFTransactionsAdapter.Transactions.RemoveAll(t => t.TransactionId == transactionId);
      HUFTransaction.Transactions.RemoveAll(t => t.TransactionId == transactionId);
      GranitXDocument.Root.Elements(Constants.Transaction)
        .Where(t => t.Attribute(Constants.TransactionIdAttribute).Value == transactionId.ToString()).Remove();
    }

    public TransactionAdapter AddTransactionRow(TransactionAdapter ta)
    {
      XElement transactionXelem = new TransactionXElementParser(ta).ParsedElement;
      GranitXDocument.Root.Add(transactionXelem);
      LoadObjectFromXElement(transactionXelem);
      return ReCreateAdapter();
    }

    public void UpdateGranitXDocument()
    {
      XDocument xml = new XDocument();
      using (var writer = xml.CreateWriter())
      {
        var ser = new XmlSerializer(HUFTransaction.GetType());
        ser.Serialize(writer, HUFTransaction);
      }

      //if (GranitXDocument.IsEmpty())
        GranitXDocument = xml;
      //else
      //{ // merge xmls && dataGridView1.CurrentRow != null
      //  throw new NotImplementedException();
      //}
    }

    public void LoadXDocumentFromFile(string xmlFilePath)
    {
      GranitXDocument = XDocument.Load(xmlFilePath);
    }

    private void LoadObjectFromXDocument(XDocument xml)
    {
      var ser = new XmlSerializer(typeof(HUFTransaction));
      HUFTransaction = (HUFTransaction)ser.Deserialize(xml.CreateReader());
      SetTransactionIdAttribute();
    }

    private void LoadObjectFromXElement(XElement xml)
    {
      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t = (Transaction)ser.Deserialize(xml.CreateReader());
      HUFTransaction.Transactions.Add(t);
      AddDefaultAttributes(t.TransactionId, xml);
    }

    private void SetTransactionIdAttribute()
    {
      int index = 0;
      foreach (var item in GranitXDocument.Root.Elements(Constants.Transaction).InDocumentOrder())
      {
        AddDefaultAttributes(HUFTransaction.Transactions[index].TransactionId, item);
        index++;
      }
    }

    private static void AddDefaultAttributes(long id, XElement item)
    {
      if (item.Attribute(Constants.TransactionIdAttribute) == null)
      {
        XAttribute idAttribute = new XAttribute(Constants.TransactionIdAttribute, id);
        item.Add(idAttribute);
      }
      if (item.Attribute(Constants.TransactionSelectedAttribute) == null)
      {
        XAttribute activeAttribute = new XAttribute(Constants.TransactionSelectedAttribute, true);
        item.Add(activeAttribute);
      }
    }

    public void SaveToFile(string xmlFilePath)
    {
      var xDocToSave = new XDocument(new XElement(Constants.HUFTransactions));

      foreach (var item in GranitXDocument.Root.Elements().
        Where(x => x.Attribute(Constants.TransactionSelectedAttribute) == null || x.Attribute(Constants.TransactionSelectedAttribute).Value == "true"))
      {
        RemoveTransactionAttributes(item);
        xDocToSave.Root.Add(RemoveAllNamespaces(item));
      }
      xDocToSave.Save(xmlFilePath);
    }

    private static void RemoveTransactionAttributes(XElement item)
    {
      if (item.Attribute(Constants.TransactionIdAttribute) != null)
        item.Attribute(Constants.TransactionIdAttribute).Remove();

      if (item.Attribute(Constants.TransactionSelectedAttribute) != null)
        item.Attribute(Constants.TransactionSelectedAttribute).Remove();
    }

    private static XElement RemoveAllNamespaces(XElement e)
    {
      return new XElement(e.Name.LocalName,
        (from n in e.Nodes()
         select ((n is XElement) ? RemoveAllNamespaces(n as XElement) : n)),
            (e.HasAttributes) ?
              (from a in e.Attributes()
               where (!a.IsNamespaceDeclaration)
               select new XAttribute(a.Name.LocalName, a.Value)) : null);
    }

    public void Sort(string columnHeaderText, SortOrder sortOrder)
    {
      switch (columnHeaderText)
      {
        case Constants.Active:
          GranitXDocument.SortElementsByXPathEvaluate(Constants.Transaction, "/@" + Constants.TransactionIdAttribute, 
            sortOrder);
          break;
        case Constants.Originator:
          GranitXDocument.SortElementsByXPathElementValue(Constants.Transaction, 
            string.Join("/", new string[] { Constants.Originator, Constants.Account, Constants.AccountNumber }), 
            sortOrder);
          break;
        case Constants.BeneficiaryName:
          GranitXDocument.SortElementsByXPathElementValue(Constants.Transaction, 
            string.Join("/", new string[] { Constants.Beneficiary, Constants.Name }), sortOrder);
          break;
        case Constants.BeneficiaryAccount:
          GranitXDocument.SortElementsByXPathElementValue(Constants.Transaction,
            string.Join("/", new string[] { Constants.Beneficiary, Constants.Account, Constants.AccountNumber }),
            sortOrder);
          break;
        case Constants.Amount:
          GranitXDocument.SortElementsByXPathElementValue(Constants.Transaction, Constants.Amount, sortOrder);
          break;
        case Constants.Currency:
          GranitXDocument.SortElementsByXPathEvaluate(Constants.Transaction, 
            Constants.Amount + "/@" + Constants.Currency, sortOrder);
          break;
        case Constants.RequestedExecutionDate:
          GranitXDocument.SortElementsByXPathElementValue(Constants.Transaction, 
            Constants.RequestedExecutionDate, sortOrder);
          break;
        case Constants.RemittanceInfo:
          //TODO sort by all Text field
          GranitXDocument.SortElementsByXPathElementValue(Constants.Transaction, 
            Constants.RemittanceInfo + "/" + Constants.Text, sortOrder);
          break;
      }
    }

    private TransactionAdapter ReCreateAdapter()
    {
      HUFTransactionsAdapter = new HUFTransactionsAdapter(HUFTransaction, GranitXDocument);
      var ts = HUFTransactionsAdapter.Transactions;
      if (ts.Count != 0)
      {
        // return with the largest TransactionId
        return ts.Aggregate((i, j) => i.TransactionId > j.TransactionId ? i : j);
      }
      else
      {
        return AddEmptyTransactionRow();
      }
    }
  }
}