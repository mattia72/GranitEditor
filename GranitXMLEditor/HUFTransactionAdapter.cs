using System.Collections.Generic;
using System.Linq;
using Xml2CSharp;

namespace GranitXMLTemplate
{
    class HUFTransactionAdapter
    {
        private HUFTransactions HUFTransactions { get; set; }
        public List<TransactionAdapter> Transactions { get; set; }

        public HUFTransactionAdapter( HUFTransactions ht)
        {
            HUFTransactions = ht;
            Transactions = ht.Transaction.Select(x => new TransactionAdapter(x)).ToList();
        }
    }
}
