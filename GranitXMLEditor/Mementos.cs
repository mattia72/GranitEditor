using GenericUndoRedo;
using System.Collections.Generic;

namespace GranitXMLEditor
{
  abstract class TransactionMemento : IMemento<List<Transaction>>
  {
    #region IMemento<List<Transaction>> Members

    public abstract IMemento<List<Transaction>> Restore(ref List<Transaction> target);

    #endregion
  }

  class UnSortTransactionMemento : TransactionMemento
  {
    List<Transaction>  sorted;
    public UnSortTransactionMemento(List<Transaction> itemList)
    {
      sorted = itemList;
    }

    public override IMemento<List<Transaction>> Restore(ref List<Transaction> target)
    {
      IMemento<List<Transaction>> inverse = new SortTransactionMemento(sorted);
      target = sorted;
      return inverse;
    }
  }

  class SortTransactionMemento : TransactionMemento
  {
    List<Transaction>  unsorted;
    public SortTransactionMemento(List<Transaction> items)
    {
      unsorted = items;
    }

    public override IMemento<List<Transaction>> Restore(ref List<Transaction> target)
    {
      IMemento<List<Transaction>> inverse = new UnSortTransactionMemento(unsorted);
      target = unsorted;
      return inverse;
    }
  }

  class InsertTransactionMemento : TransactionMemento
  {
    private int index;
    public InsertTransactionMemento(int index)
    {
      this.index = index;
    }

    public override IMemento<List<Transaction>> Restore(ref List<Transaction> target)
    {
      Transaction removed = target[index];
      IMemento<List<Transaction>> inverse = new RemoveTransactionMemento(index, removed);
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

    public override IMemento<List<Transaction>> Restore(ref List<Transaction> target)
    {
      IMemento<List<Transaction>> inverse = null; 
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

    public override IMemento<List<Transaction>> Restore(ref List<Transaction> target)
    {
      int index = target.Count - 1;
      IMemento<List<Transaction>> inverse = new RemoveTransactionMemento(index, target[index]);
      target.RemoveAt(target.Count - 1);
      return inverse;
    }
  }
}
