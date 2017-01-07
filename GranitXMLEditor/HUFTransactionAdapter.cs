using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GranitXMLEditor
{
  class HUFTransactionAdapter
  {
    private HUFTransaction HUFTransactions { get; set; }
    public List<TransactionAdapter> Transactions { get; set; }
    public XDocument GranitXDocument { get; private set; }

    public HUFTransactionAdapter(HUFTransaction ht, XDocument xdoc)
    {
      CreateAdaptersForTransactions(ht, xdoc);
    }

    public void CreateAdaptersForTransactions(HUFTransaction ht, XDocument xdoc)
    {
      HUFTransactions = ht;
      Transactions = ht.Transactions.Select(x => new TransactionAdapter(x, xdoc)).ToList();
    }
  }
}
