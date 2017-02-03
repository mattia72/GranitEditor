using GenericUndoRedo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GranitXMLEditor
{
  abstract class TransactionMemento : IMemento<TransactionPool>
  {
    #region IMemento<TransactionPool> Members

    public abstract IMemento<TransactionPool> Restore(TransactionPool target);

    #endregion
  }

  class InsertTransactionMemento : TransactionMemento
  {
    private int index;
    public InsertTransactionMemento(int index)
    {
      this.index = index;
    }

    public override IMemento<TransactionPool> Restore(TransactionPool target)
    {
      Transaction removed = target[index];
      IMemento<TransactionPool> inverse = new RemoveTransactionMemento(index, removed);
      target.RemoveAt(index);
      return inverse;
    }
  }

  class RemoveTransactionMemento : TransactionMemento
  {
    Transaction removed;
    int? index;

    public RemoveTransactionMemento(int index, Transaction item)
    {
      this.index = index;
      this.removed = item;
    }

    public RemoveTransactionMemento(Transaction item)
    {
      this.index = null;
      this.removed = item;
    }

    public override IMemento<TransactionPool> Restore(TransactionPool target)
    {
      IMemento<TransactionPool> inverse = null; 
      if (index != null)
      {
        inverse = new InsertTransactionMemento((int)index);
        target.Insert((int)index, removed);
      }
      else
      {
        inverse = new InsertTransactionMemento(target.Count);
        target.Add(removed);
      }
      return inverse;
    }
  }

  class AddTransactionMemento : TransactionMemento
  {
    public AddTransactionMemento()
    {
    }

    public override IMemento<TransactionPool> Restore(TransactionPool target)
    {
      int index = target.Count - 1;
      IMemento<TransactionPool> inverse = new RemoveTransactionMemento(index, target[index]);
      target.RemoveAt(target.Count - 1);
      return inverse;
    }
  }
}
