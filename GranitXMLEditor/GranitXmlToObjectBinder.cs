using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms;
using System;

namespace GranitXMLEditor
{
  internal class GranitXmlToObjectBinder
  {
    internal HUFTransaction HUFTransactions { get; private set; }
    internal HUFTransactionAdapter HUFTransactionAdapter { get; private set; }
    internal XDocument GranitXDocument { get; private set; }

    private GranitXmlToObjectBinder()
    {
      GranitXDocument = new XDocument();
    }

    public GranitXmlToObjectBinder(string xmlFilePath) : this()
    {
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
        var ser = new XmlSerializer(HUFTransactions.GetType());
        ser.Serialize(writer, HUFTransactions);
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
      HUFTransactions = (HUFTransaction)ser.Deserialize(xml.CreateReader());
      AddTransactionAttributes();
    }

    private void LoadObjectFromXElement(XElement xml)
    {
      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t = (Transaction)ser.Deserialize(xml.CreateReader());
      HUFTransactions.Transactions.Add(t);
      AddDefaultAttributes(t.TransactionId, xml);
    }

    private void AddTransactionAttributes()
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
      if (item.Attribute(Constants.TransactionActiveAttribute) == null)
      {
        XAttribute activeAttribute = new XAttribute(Constants.TransactionActiveAttribute, true);
        item.Add(activeAttribute);
      }
    }

    public void SaveToFile(string xmlFilePath)
    {
      var xDocToSave = new XDocument(new XElement(Constants.HUFTransactions));

      foreach (var item in GranitXDocument.Root.Elements().
        Where(x => x.Attribute(Constants.TransactionActiveAttribute) == null || x.Attribute(Constants.TransactionActiveAttribute).Value == "true"))
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

        if(item.Attribute(Constants.TransactionActiveAttribute) != null)
          item.Attribute(Constants.TransactionActiveAttribute).Remove();
      }
    }

    public void Sort(string columnText, SortOrder sortOrder)
    {
      switch (columnText)
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
      HUFTransactionAdapter = new HUFTransactionAdapter(HUFTransactions, GranitXDocument);
      // return with the largest TransactionId
      return HUFTransactionAdapter.Transactions.Aggregate((i, j) => i.TransactionId > j.TransactionId ? i : j); 
    }
  }
}