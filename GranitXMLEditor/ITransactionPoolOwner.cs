using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GranitXMLEditor
{
  interface ITransactionPoolOwner
  {
    TransactionPool TransactionPool { get; set; }
  }
}
