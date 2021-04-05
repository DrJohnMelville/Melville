using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Melville.Lists.PersistentLinq
{
  public sealed class WhereCollection<T> : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
  {
    private readonly IList<T> source;
    private List<T> cache;
    private readonly Func<T, bool> predicate;

    public WhereCollection(IList<T> source, Func<T, bool> predicate)
    {
      this.source = source;
      this.predicate = predicate;

      cache = RefreshCache(); // assignment is unnecessary but it tells the compiler that cache is initalized
      if (source is INotifyCollectionChanged incc)
      {
        incc.CollectionChanged += (foo, bar) => Requery();
      }
    }
    
    #region IList Members
    public IEnumerator<T> GetEnumerator()
    {
      return cache.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return ((IEnumerable)cache).GetEnumerator();
    }

    public void Add(T item)
    {
      cache.Add(item);
    }

    public void Clear()
    {
      cache.Clear();
    }

    public bool Contains(T item)
    {
      return cache.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      cache.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
      return cache.Remove(item);
    }

    public int Count
    {
      get { return cache.Count; }
    }
    public bool IsReadOnly
    {
      get { return true; }
    }
    public int IndexOf(T item)
    {
      return cache.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
      cache.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
      cache.RemoveAt(index);
    }

    public T this[int index]
    {
      get { return cache[index]; }
      set { source[source.IndexOf(cache[index])] = value; }
    }
    #endregion

    #region ChangeNotification
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public void Requery()
    {
      RefreshCache();
      var cc = CollectionChanged;
      if (cc != null)
      {
        cc(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }
      var pc = PropertyChanged;
      if (pc != null)
      {
        pc(this, new PropertyChangedEventArgs("Count"));
      }
    }
    private List<T> RefreshCache()
    {
      return (cache = source.Where(predicate).ToList());
    }
    #endregion
  }
}