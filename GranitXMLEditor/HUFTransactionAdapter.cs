using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GranitXMLEditor
{
  public class HUFTransactionsAdapter : INotifyPropertyChanged, INotifyPropertyChanging
  {
    private HUFTransaction HUFTransactions { get; set; }
    public List<TransactionAdapter> TransactionAdapters { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangingEventHandler PropertyChanging;

    public HUFTransactionsAdapter(XDocument xdoc)
    {
      CreateAdaptersForTransactions(xdoc);
    }

    public void CreateAdaptersForTransactions(XDocument xdoc)
    {
      HUFTransactions = CreateObjectFromXDocument(xdoc);
      TransactionAdapters = HUFTransactions.Transactions.Select(x => new TransactionAdapter(x, xdoc)).ToList();
      foreach(TransactionAdapter ta in TransactionAdapters)
      {
        ta.PropertyChanging += TransactionAdapter_PropertyChanging;
        ta.PropertyChanged += TransactionAdapter_PropertyChanged;
      }
    }

    private void TransactionAdapter_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
    }

    private void TransactionAdapter_PropertyChanging(object sender, PropertyChangingEventArgs e)
    {
      PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(e.PropertyName));
    }

    private HUFTransaction CreateObjectFromXDocument(XDocument xml)
    {
      var ser = new XmlSerializer(typeof(HUFTransaction));
      return (HUFTransaction)ser.Deserialize(xml.CreateReader());
    }

    private void CreateObjectFromXElement(XElement xml)
    {
      var ser = new XmlSerializer(typeof(Transaction));
      Transaction t = (Transaction)ser.Deserialize(xml.CreateReader());
      HUFTransactions.Transactions.Add(t);
    }
  }
}
