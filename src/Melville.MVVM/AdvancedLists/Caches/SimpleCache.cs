using  System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Melville.MVVM.AdvancedLists.Caches
{
  public sealed class SimpleCache<TKey, TResult>
  {
    private readonly int size;
    private readonly Func<TKey, TResult> create;
    private List<Tuple<TKey, TResult>> data;

    public SimpleCache(Func<TKey, TResult> create) : this(int.MaxValue, create)
    {
      // do nothing
    }
    public SimpleCache(int size, Func<TKey, TResult> create)
    {
      this.size = size;
      this.create = create;
      // if size is maxInt then the class is a buffer, a cache that grows without bound.
      data = size == int.MaxValue ? new List<Tuple<TKey, TResult>>() : new List<Tuple<TKey, TResult>>(size);
    }

    public TResult Get(TKey key)
    {
      lock (data)
      {
        Tuple<TKey, TResult> item = data.FirstOrDefault(i => Equals(i.Item1, key));
        if (item != null)
        {
          data.Remove(item);
        }
        else
        {
          if (data.Count >= size)
          {
            RemoveItem(data[data.Count - 1]);
          }
          item = Tuple.Create(key, create(key));
        }
        data.Insert(0, item);
        return item.Item2;

      }
    }

    public void Dump()
    {
      lock (data)
      {
        data.Clear();

      }
    }

    public void Forget(TKey key)
    {
      lock (data)
      {
        var item =data.FirstOrDefault(i => Equals(key, i.Item1));
        if (item != null)
        {
          RemoveItem(item);
        };

      }
    }

    private void RemoveItem(Tuple<TKey, TResult> itemToRemove)
    {
      (itemToRemove.Item2 as IDisposable)?.Dispose();
      data.Remove(itemToRemove);
    }

    public void Rekey(Func<TKey, TKey> func)
    {
      lock (data)
      {
        data = data.Select(i => Tuple.Create(func(i.Item1), i.Item2)).ToList();

      }
    }

    public void Clear()
    {
      lock (data)
      {
        data.ForEach(i => (i.Item2 as IDisposable)?.Dispose());
        data.Clear();
      }
    }
  }
}
