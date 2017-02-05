using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GranitXMLEditor
{
  public class HUFTransactionsAdapter : INotifyPropertyChanged
  {
    private HUFTransaction HUFTransactions { get; set; }
    public List<TransactionAdapter> TransactionAdapters { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    public HUFTransactionsAdapter(XDocument xdoc)
    {
      CreateAdaptersForTransactions(xdoc);
    }

    public void CreateAdaptersForTransactions(XDocument xdoc)
    {
      HUFTransactions = CreateObjectFromXDocument(xdoc);
      TransactionAdapters = HUFTransactions.Transactions.Select(x => new TransactionAdapter(x, xdoc)).ToList();
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

    public void Sort(IComparer<TransactionAdapter> comparer)
    {
      TransactionAdapters.Sort(comparer);

      OnPropertyChanged("Transactions");
    }

    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
