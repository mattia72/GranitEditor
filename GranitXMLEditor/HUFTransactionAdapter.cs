using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace GranitXMLEditor
{
  public class HUFTransactionsAdapter : INotifyPropertyChanged
  {
    private HUFTransaction HUFTransactions { get; set; }
    public List<TransactionAdapter> Transactions { get; set; }
    public XDocument GranitXDocument { get; private set; }
    public event PropertyChangedEventHandler PropertyChanged;

    public HUFTransactionsAdapter(HUFTransaction ht, XDocument xdoc)
    {
      CreateAdaptersForTransactions(ht, xdoc);
    }

    public void CreateAdaptersForTransactions(HUFTransaction ht, XDocument xdoc)
    {
      HUFTransactions = ht;
      Transactions = ht.Transactions.Select(x => new TransactionAdapter(x, xdoc)).ToList();
    }

    public void Sort(IComparer<TransactionAdapter> comparer)
    {
      Transactions.Sort(comparer);

      OnPropertyChanged("Transactions");
    }

    protected void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
