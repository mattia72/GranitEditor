using System.Xml.Serialization;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms;
using System;

namespace GranitXMLEditor
{
  internal class GranitXmlToObjectBinder
  {
    internal HUFTransactions HUFTransactions { get; private set; }
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
      CreateAdapter();
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
      {
        // merge xmls
        throw new NotImplementedException();
      }
    }

    public void LoadXDocumentFromFile(string xmlFilePath)
    {
      GranitXDocument = XDocument.Load(xmlFilePath);
    }

    private void LoadObjectFromXDocument(XDocument xml)
    {
      var ser = new XmlSerializer(typeof(HUFTransactions));
      HUFTransactions = (HUFTransactions)ser.Deserialize(xml.CreateReader());
      AddTransactionAttributes();
    }

    private void AddTransactionAttributes()
    {
      int id = 0;
      foreach (var item in GranitXDocument.Descendants(Constants.Transaction).InDocumentOrder())
      {
        XAttribute a = new XAttribute(Constants.TransactionIdAttribute, ++id);
        XAttribute a2 = new XAttribute(Constants.TransactionActiveAttribute, true);
        item.Add(a);
        item.Add(a2);
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
          GranitXDocument.SortDescendantElementsByElementValue(Constants.Transaction, Constants.Active, sortOrder);
          break;
        case Constants.Originator:
          GranitXDocument.SortDescendantElementsByElementValue(Constants.Transaction, Constants.Originator, sortOrder);
          break;
        case Constants.BeneficiaryName:
          GranitXDocument.SortDescendantElementsByElementValue(Constants.Transaction, Constants.BeneficiaryName, sortOrder);
          break;
        case Constants.BeneficiaryAccount:
          GranitXDocument.SortDescendantElementsByElementValue(Constants.Transaction, Constants.BeneficiaryAccount, sortOrder);
          break;
        case Constants.Amount:
          GranitXDocument.SortDescendantElementsByElementValue(Constants.Transaction, Constants.Amount, sortOrder);
          break;
        case Constants.Currency:
          GranitXDocument.SortDescendantElementsByElementValue(Constants.Transaction, Constants.Currency, sortOrder);
          break;
        case Constants.RequestedExecutionDate:
          GranitXDocument.SortDescendantElementsByElementValue(Constants.Transaction, Constants.RequestedExecutionDate, sortOrder);
          break;
        case Constants.RemittanceInfo:
          GranitXDocument.SortDescendantElementsByElementValue(Constants.Transaction, Constants.RemittanceInfo, sortOrder);
          break;
      }
    }

    private void CreateAdapter()
    {
      HUFTransactionAdapter = new HUFTransactionAdapter(HUFTransactions, GranitXDocument);
    }
  }
}