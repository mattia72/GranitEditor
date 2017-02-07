using System.Xml.Linq;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using GenericUndoRedo;

namespace GranitXMLEditor
{
  public class GranitXmlToAdapterBinder : IGranitXDocumentOwner
  {
    public HUFTransactionsAdapter HUFTransactionsAdapter { get; private set; }
    public XDocument GranitXDocument { get; set; }
    public UndoRedoHistory<IGranitXDocumentOwner> History { get; set; }

    public GranitXmlToAdapterBinder()
    {
      GranitXDocument = new XDocument();
      GranitXDocument.Add(new XElement(Constants.HUFTransactions));
      XElement transactionXelem = new TransactionXElementParser().ParsedElement;
      GranitXDocument.Root.Add(transactionXelem);
      SetTransactionIdAttribute();
      ReCreateAdapter();
      History = new UndoRedoHistory<IGranitXDocumentOwner>(this);
     
    }

    public GranitXmlToAdapterBinder(string xmlFilePath) 
    {
      GranitXDocument = XDocument.Load(xmlFilePath);
      SetTransactionIdAttribute();
      ReCreateAdapter();
      History = new UndoRedoHistory<IGranitXDocumentOwner>(this);
    }

    private void HUFTransactionsAdapter_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
    {
      History?.Do(new TransactionPoolMemento(GranitXDocument));
      Debug.WriteLine("Property changing:" + e.PropertyName);
    }

    private void HUFTransactionsAdapter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      Debug.WriteLine("Property changed:" + e.PropertyName);
    }

    public TransactionAdapter AddEmptyTransactionRow()
    {
      History?.Do(new TransactionPoolMemento(GranitXDocument));

      XElement transactionXelem = new TransactionXElementParser().ParsedElement;
      GranitXDocument.Root.Add(transactionXelem);
      return ReCreateAdapter();
    }

    public void RemoveTransactionRowById( long transactionId, int rowIndex)
    {
      Debug.WriteLine("Remove transactionId: " + transactionId + " from index: " + rowIndex);

      History?.Do(new TransactionPoolMemento(GranitXDocument));

      HUFTransactionsAdapter.TransactionAdapters.RemoveAll(t => t.TransactionId == transactionId);
      GranitXDocument.Root.Elements(Constants.Transaction)
        .Where(t => t.Attribute(Constants.TransactionIdAttribute).Value == transactionId.ToString()).Remove();
    }

    public TransactionAdapter AddTransactionRow(TransactionAdapter ta)
    {

      History?.Do(new TransactionPoolMemento(GranitXDocument));

      XElement transactionXelem = new TransactionXElementParser(ta).ParsedElement;
      GranitXDocument.Root.Add(transactionXelem);
      return ReCreateAdapter();
    }

    private void SetTransactionIdAttribute()
    {
      int index = 1;
      foreach (var item in GranitXDocument.Root.Elements(Constants.Transaction).InDocumentOrder())
      {
        AddDefaultAttributes(index++, item);
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
        Where(x => x.Attribute(Constants.TransactionSelectedAttribute) == null || 
        x.Attribute(Constants.TransactionSelectedAttribute).Value == "true"))
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
      History?.Do(new TransactionPoolMemento(GranitXDocument));

      switch (columnHeaderText)
      {
        case Constants.IsSelected:
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
      HUFTransactionsAdapter = new HUFTransactionsAdapter(GranitXDocument);
      HUFTransactionsAdapter.PropertyChanged += HUFTransactionsAdapter_PropertyChanged;
      HUFTransactionsAdapter.PropertyChanging += HUFTransactionsAdapter_PropertyChanging;
      var ts = HUFTransactionsAdapter.TransactionAdapters;
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

    internal void History_Undo()
    {
      if (History.CanUndo)
      {
        History.Undo();
        ReCreateAdapter();
      }
    }

    internal void History_Redo()
    {
      if (History.CanRedo)
      {
        History.Redo();
        ReCreateAdapter();
      }
    }
  }
}