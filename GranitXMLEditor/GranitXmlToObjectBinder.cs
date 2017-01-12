using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms;
using System;

namespace GranitXMLEditor
{
  internal class GranitXmlToObjectBinder
  {
    internal HUFTransaction HUFTransaction { get; private set; }
    internal HUFTransactionsAdapter HUFTransactionsAdapter { get; private set; }
    internal XDocument GranitXDocument { get; private set; }

    public GranitXmlToObjectBinder()
    {
      GranitXDocument = new XDocument();
      GranitXDocument.Add(new XElement(Constants.HUFTransactions));
      HUFTransaction = new HUFTransaction();
      ReCreateAdapter();
    }

    public GranitXmlToObjectBinder(string xmlFilePath) 
    {
      GranitXDocument = new XDocument();
      LoadXDocumentFromFile(xmlFilePath);
      LoadObjectFromXDocument(GranitXDocument);
      ReCreateAdapter();
    }

    public TransactionAdapter AddNewTransactionRow()
    {
      XElement transactionXelem = new TransactionXElementParser().ParsedElement;
      GranitXDocument.Root.Add(transactionXelem);
      LoadObjectFromXElement(transactionXelem);
      return ReCreateAdapter();
    }

    public void RemoveTransactionRowById( int transactionId)
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

      if (GranitXDocument.IsEmpty())
        GranitXDocument = xml;
      else
      { // merge xmls && dataGridView1.CurrentRow != null
        throw new NotImplementedException();
      }
    }

    public void LoadXDocumentFromFile(string xmlFilePath)
    {
      GranitXDocument = XDocument.Load(xmlFilePath);
    }

    private void LoadObjectFromXDocument(XDocument xml)
    {
      var ser = new XmlSerializer(typeof(HUFTransaction));
      HUFTransaction = (HUFTransaction)ser.Deserialize(xml.CreateReader());
      InitTransactionAttributes();
    }

    private void LoadObjectFromXElement(XElement xml)
    {
      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t = (Transaction)ser.Deserialize(xml.CreateReader());
      HUFTransaction.Transactions.Add(t);
      AddDefaultAttributes(t.TransactionId, xml);
    }

    private void InitTransactionAttributes()
    {
      int id = 0;
      foreach (var item in GranitXDocument.Descendants(Constants.Transaction).InDocumentOrder())
      {
        //TODO: bind id with real Transaction objects
        AddDefaultAttributes(++id, item);
      }
    }

    private static void AddDefaultAttributes(int id, XElement item)
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
          xDocToSave.Root.Add(item);
      }

      RemoveTransactionAttributes(xDocToSave);
      xDocToSave.Save(xmlFilePath);
    }

    private void RemoveTransactionAttributes(XDocument x)
    {
      foreach (var item in x.Root.Elements())
      {
        if(item.Attribute(Constants.TransactionIdAttribute) != null)
          item.Attribute(Constants.TransactionIdAttribute).Remove();

        if(item.Attribute(Constants.TransactionSelectedAttribute) != null)
          item.Attribute(Constants.TransactionSelectedAttribute).Remove();
      }
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
      // return with the largest TransactionId
      var ts = HUFTransactionsAdapter.Transactions;
      if (ts.Count != 0)
      {
        return ts.Aggregate((i, j) => i.TransactionId > j.TransactionId ? i : j);
      }
      else
      {
        return AddNewTransactionRow();
      }
    }
  }
}