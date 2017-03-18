using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GranitEditor
{
  [Serializable]
  public class HistoryXDocumentPool: IEnumerable<XDocument>, ICloneable
  {
    List<XDocument> _pool;

    internal int Count { get { return _pool.Count; } }

    public static explicit operator HistoryXDocumentPool(List<XDocument> list)
    {
      var tp = new HistoryXDocumentPool(list);
      return tp;
    }

    public HistoryXDocumentPool(XDocument xdoc)
    {
      _pool = new List<XDocument>();
      _pool.Add(new XDocument(xdoc));
    }

    public HistoryXDocumentPool(List<XDocument> list)
    {
      _pool = list;
    }

    public XDocument this[int index]
    {
      get { return _pool[index]; }
    }
    public IEnumerator<XDocument> GetEnumerator()
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

    internal void Remove(XDocument item)
    {
      _pool.Remove(item);
    }

    internal void Insert(int index, XDocument item)
    {
      _pool.Insert(index, item);
    }

    internal void Add(XDocument item)
    {
      _pool.Add( item);
    }

    internal void CopyTo(out HistoryXDocumentPool _memento)
    {
      XDocument[] ta = new XDocument[_pool.Count];
      _pool.CopyTo(ta);
      _memento = new HistoryXDocumentPool(new List<XDocument>(ta));
    }

    public object Clone()
    {
      XDocument[] ta = new XDocument[_pool.Count];
      _pool.CopyTo(ta);
      return new HistoryXDocumentPool(new List<XDocument>(ta));
    }
  }
}