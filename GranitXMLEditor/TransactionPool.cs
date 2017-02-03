using System;
using System.Collections;
using System.Collections.Generic;

namespace GranitXMLEditor
{
  public class TransactionPool: IEnumerable<Transaction>
  {
    List<Transaction> _pool = new List<Transaction>();

    internal int Count { get { return _pool.Count; } }

    public Transaction this[int index]
    {
      get { return _pool[index]; }
    }
    public IEnumerator<Transaction> GetEnumerator()
    {
      return _pool.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _pool.GetEnumerator();
    }

    internal void RemoveAt(int index)
    {
      _pool.RemoveAt(index);
    }

    internal void Remove(Transaction item)
    {
      _pool.Remove(item);
    }

    internal void Insert(int index, Transaction item)
    {
      _pool.Insert(index, item);
    }

    internal void Add(Transaction item)
    {
      _pool.Add( item);
    }
  }
}