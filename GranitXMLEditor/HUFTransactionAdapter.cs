using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GranitXMLEditor
{
  class HUFTransactionAdapter
  {
    private HUFTransactions HUFTransactions { get; set; }
    public List<TransactionAdapter> Transactions { get; set; }
    public XDocument GranitXDocument { get; private set; }

    public HUFTransactionAdapter(HUFTransactions ht, XDocument xdoc)
    {
      HUFTransactions = ht;
      Transactions = ht.Transaction.Select(x => new TransactionAdapter(x, xdoc)).ToList();
    }
  }
}
