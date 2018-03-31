using GenericUndoRedo;
using GranitEditor.Properties;
using GranitXml;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GranitEditor
{
  public class GranitXmlToAdapterBinder : IGranitXDocumentOwner
  {
    public HUFTransactionsAdapter HUFTransactionsAdapter { get; private set; }

    //IGranitXDocumentOwner
    public XDocument GranitXDocument { get; set; }
    public string OnDiscXmlFilePath { get; set; }
    public XDocument OnDiscXDocument { get; set; }

    public UndoRedoHistory<IGranitXDocumentOwner> History { get; set; }

    // TODO: DeepEquals doesn't do the job... 
    public bool DocHasPendingChanges => 
      string.IsNullOrEmpty(OnDiscXmlFilePath) ? true :  0 != CompareGranitXDocuments(GranitXDocument, OnDiscXDocument);

    public decimal SumAmount => HUFTransactionsAdapter.TransactionAdapters.Aggregate(0m, (total, next) => total + next.Amount);
    public int TransactionCount => GranitXDocument.Root.Elements(GranitXml.Constants.Transaction).Count();

    public bool XmlValidationErrorOccured { get; private set; }

    private ValidationEventArgs _validationEventArgs = null;

    public ValidationEventArgs ValidationEventArgs { get => _validationEventArgs; set => _validationEventArgs = value; }


    public GranitXmlToAdapterBinder()
    {
      GranitXDocument = new XDocument();
      GranitXDocument.Add(new XElement(GranitXml.Constants.HUFTransactions));
      AddEmptyTransactionRow();
      //XElement transactionXelem = new TransactionXElementParser().ParsedElement;
      //GranitXDocument.Root.Add(transactionXelem);
      //SetTransactionIdAttribute();
      //ReCreateAdapter();
      History = new UndoRedoHistory<IGranitXDocumentOwner>(this);
      OnDiscXmlFilePath = "";
    }

    public GranitXmlToAdapterBinder(string xmlFilePath, bool validate = false)
    {
      XmlValidationErrorOccured = false;
      if (File.Exists(xmlFilePath))
      {
        if (validate)
        {
          GranitXDocument = new XDocument();
          GranitXDocument = GranitXDocument.ValidateAndLoad(xmlFilePath, Settings.Default.SchemaFile, ref _validationEventArgs);
          XmlValidationErrorOccured = _validationEventArgs != null;
        }
        else
          GranitXDocument = XDocument.Load(xmlFilePath);

        OnDiscXmlFilePath = xmlFilePath;
        OnDiscXDocument = XDocument.Load(xmlFilePath);
      }

      if (!XmlValidationErrorOccured)
      {
        _validationEventArgs = null;
        SetTransactionIdAttribute();
        ReCreateAdapter();
        History = new UndoRedoHistory<IGranitXDocumentOwner>(this);
      }
    }

    public static int CompareGranitXDocuments(XDocument x1, XDocument x2)
    {
      HUFTransaction h1 = HUFTransaction.Load(x1);
      HUFTransaction h2 = HUFTransaction.Load(x2);
      return h1 != null ? h1.CompareTo(h2) : h2 == null ? 0 : -1;
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
      var ta = new TransactionAdapter();
      return AddTransactionRow(ta);
    }

    public bool GranitXmlDocumentContains(TransactionAdapter ta)
    {
      return GranitXmlToAdapterBinder.GranitXmlDocumentContains(GranitXDocument, ta);
    }

    public static bool GranitXmlDocumentContains(XDocument xd, TransactionAdapter ta)
    {
      return xd.Root.Elements(GranitXml.Constants.Transaction)
        .Where(t => t.Attribute(GranitXml.Constants.TransactionIdAttribute).Value == ta.TransactionId.ToString()).
        FirstOrDefault() != null;
    }

    public static void Bind(ref XDocument xd, TransactionAdapter ta)
    {
      if(!GranitXmlDocumentContains(xd, ta))
      {
        XElement transactionXelem = new TransactionXElementParser(ta).ParsedElement;
        xd.Root.Add(transactionXelem);
      }
    }

    public void RemoveTransactionRowById(long transactionId)
    {
      History?.Do(new TransactionPoolMemento(GranitXDocument));

      HUFTransactionsAdapter.TransactionAdapters.RemoveAll(t => t.TransactionId == transactionId);
      GranitXDocument.Root.Elements(GranitXml.Constants.Transaction)
        .Where(t => t.Attribute(GranitXml.Constants.TransactionIdAttribute).Value == transactionId.ToString()).Remove();
    }

    public TransactionAdapter AddTransactionRow(TransactionAdapter ta)
    {
      if (!GranitXmlDocumentContains(ta))
      {
        History?.Do(new TransactionPoolMemento(GranitXDocument));
        XElement transactionXelem = new TransactionXElementParser(ta).ParsedElement;
        GranitXDocument.Root.Add(transactionXelem);
      }

      TransactionAdapter taRetVal = ReCreateAdapter();
      return taRetVal;
    }

    private void SetTransactionIdAttribute()
    {
      int index = 1;
      foreach (var item in GranitXDocument.Root.Elements(GranitXml.Constants.Transaction).InDocumentOrder())
      {
        AddDefaultAttributes(index++, item);
      }
    }

    private static void AddDefaultAttributes(long id, XElement item)
    {
      if (item.Attribute(GranitXml.Constants.TransactionIdAttribute) == null)
      {
        XAttribute idAttribute = new XAttribute(GranitXml.Constants.TransactionIdAttribute, id);
        item.Add(idAttribute);
      }
      if (item.Attribute(GranitXml.Constants.TransactionSelectedAttribute) == null)
      {
        XAttribute activeAttribute = new XAttribute(GranitXml.Constants.TransactionSelectedAttribute, true);
        item.Add(activeAttribute);
      }
    }

    public void SaveToFile(string xmlFilePath)
    {
      OnDiscXDocument = new XDocument(new XElement(GranitXml.Constants.HUFTransactions));

      Debug.WriteLine("SaveToFile: {0}", HUFTransactionsAdapter.ToString());
      
      foreach (var item in GranitXDocument.Root.Elements().InDocumentOrder().
        Where(x => x.Attribute(GranitXml.Constants.TransactionSelectedAttribute) == null ||
        x.Attribute(GranitXml.Constants.TransactionSelectedAttribute).Value == "true"))
      {
        XElement copy = RemoveTransactionAttributes(item);
        OnDiscXDocument.Root.Add(RemoveAllNamespaces(copy));
      }
      try
      {
        OnDiscXDocument.Save(xmlFilePath);
        OnDiscXmlFilePath = xmlFilePath;
      }
      catch(UnauthorizedAccessException ex)
      {
        MessageBox.Show(Resources.FileCouldntBeSaved + "\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        OnDiscXDocument = null;
      }
      catch(Exception ex)
      {
        MessageBox.Show(Resources.FileCouldntBeSaved + "\n" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        OnDiscXDocument = null;
      }
    }

    private static XElement RemoveTransactionAttributes( XElement item)
    {
      var returnItem = new XElement(item);
      if (returnItem.Attribute(GranitXml.Constants.TransactionIdAttribute) != null)
      {
        returnItem.Attribute(GranitXml.Constants.TransactionIdAttribute).Remove();
      }

      if (returnItem.Attribute(GranitXml.Constants.TransactionSelectedAttribute) != null)
      {
        returnItem.Attribute(GranitXml.Constants.TransactionSelectedAttribute).Remove();
      }
      return returnItem;
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

    public void Sort(string propertyName, SortOrder sortOrder)
    {
      History?.Do(new TransactionPoolMemento(GranitXDocument));

      switch (propertyName)
      {
        case Constants.IsSelectedPropertyName:
          GranitXDocument.SortElementsByXPathEvaluate(GranitXml.Constants.Transaction, "/@" + GranitXml.Constants.TransactionIdAttribute,
            sortOrder);
          break;
        case Constants.OriginatorPropertyName:
          GranitXDocument.SortElementsByXPathToStringValue(GranitXml.Constants.Transaction,
            string.Join("/", new string[] { GranitXml.Constants.Originator, GranitXml.Constants.Account, GranitXml.Constants.AccountNumber }),
            sortOrder);
          break;
        case Constants.BeneficiaryNamePropertyName:
          GranitXDocument.SortElementsByXPathToStringValue(GranitXml.Constants.Transaction,
            string.Join("/", new string[] { GranitXml.Constants.Beneficiary, GranitXml.Constants.Name }), sortOrder);
          break;
        case Constants.BeneficiaryAccountPropertyName:
          GranitXDocument.SortElementsByXPathToStringValue(GranitXml.Constants.Transaction,
            string.Join("/", new string[] { GranitXml.Constants.Beneficiary, GranitXml.Constants.Account, GranitXml.Constants.AccountNumber }),
            sortOrder);
          break;
        case Constants.AmountPropertyName:
          GranitXDocument.SortElementsByXPathToDecimalValue(GranitXml.Constants.Transaction, GranitXml.Constants.Amount, sortOrder);
          break;
        case Constants.CurrencyPropertyName:
          GranitXDocument.SortElementsByXPathEvaluate(GranitXml.Constants.Transaction,
            GranitXml.Constants.Amount + "/@" + GranitXml.Constants.Currency, sortOrder);
          break;
        case Constants.ExecutionDatePropertyName:
          GranitXDocument.SortElementsByXPathToStringValue(GranitXml.Constants.Transaction,
            GranitXml.Constants.RequestedExecutionDate, sortOrder);
          break;
        case Constants.RemittanceInfoPropertyName:
          //TODO sort by all Text field
          GranitXDocument.SortElementsByXPathToStringValue(GranitXml.Constants.Transaction,
            GranitXml.Constants.RemittanceInfo + "/" + GranitXml.Constants.Text, sortOrder);
          break;
      }
    }

    private TransactionAdapter ReCreateAdapter()
    {
      var ts = HUFTransactionsAdapter?.TransactionAdapters;

      if(ts!=null) Debug.WriteLine("ReCreateAdapter begin TransactionCount: {0}", ts.Count);

      HUFTransactionsAdapter = new HUFTransactionsAdapter(GranitXDocument);
      HUFTransactionsAdapter.PropertyChanged += HUFTransactionsAdapter_PropertyChanged;
      HUFTransactionsAdapter.PropertyChanging += HUFTransactionsAdapter_PropertyChanging;
      ts = HUFTransactionsAdapter.TransactionAdapters;

      Debug.WriteLine("ReCreateAdapter end TransactionCount: {0}", ts.Count);

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