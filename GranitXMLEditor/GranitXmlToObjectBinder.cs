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
      AddTransactionIdAttribute();
    }

    private void AddTransactionIdAttribute()
    {
      int id = 0;
      foreach (var item in GranitXDocument.Descendants(Constants.Transaction).InDocumentOrder())
      {
        XAttribute a = new XAttribute(Constants.TransactionAttribute, ++id);
        item.Add(a);
      }
    }

    public void SaveToFile(string xmlFilePath)
    {
      RemoveTransactionIdAttribute();
      GranitXDocument.Save(xmlFilePath);
    }

    private void RemoveTransactionIdAttribute()
    {
      foreach (var item in GranitXDocument.Descendants(Constants.HUFTransactions).Elements())
      {
        if(item.Attribute(Constants.TransactionAttribute) != null)
          item.Attribute(Constants.TransactionAttribute).Remove();
      }
    }

    public void Sort(string columnText, SortOrder sortOrder)
    {
      switch (columnText)
      {
        case Constants.Amount:
          GranitXDocument.SortDescendantElementsByElementValue(Constants.Transaction, Constants.Amount, sortOrder);
          break;
        default:
          break;
      }
    }

    private void CreateAdapter()
    {
      HUFTransactionAdapter = new HUFTransactionAdapter(HUFTransactions, GranitXDocument);
    }

  }
}