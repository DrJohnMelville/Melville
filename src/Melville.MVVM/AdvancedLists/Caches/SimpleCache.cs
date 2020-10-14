using  System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Melville.MVVM.AdvancedLists.Caches
{
  public sealed class SimpleCache<TKey, TResult>
  {
    private readonly int size;
    private readonly Func<TKey, TResult> create;
    private List<(TKey, TResult)> data;

    public SimpleCache(Func<TKey, TResult> create) : this(int.MaxValue, create)
    {
      // do nothing
    }

    public IEnumerable<TResult> CachedItems => data.Select(i => i.Item2);
    public SimpleCache(int size, Func<TKey, TResult> create)
    {
      this.size = size;
      this.create = create;
      // if size is maxInt then the class is a buffer, a cache that grows without bound.
      data = size == int.MaxValue ? new List<(TKey, TResult)>() : new List<(TKey, TResult)>(size);
    }

    private bool TryGet(TKey key, out (TKey, TResult) result)
    {
      foreach (var candidate in data.Where(candidate => candidate.Item1?.Equals(key) ?? false))
      {
        result = candidate;
        return true;
      }
      result = default;
      return false;
    }

    public TResult Get(TKey key)
    {
      lock (data)
      {
        var item = GetOrCreateRequestedItem(key);
        data.Insert(0, item);
        return item.Item2;
      }
    }

    private (TKey, TResult) GetOrCreateRequestedItem(TKey key)
    {
      if (TryGet(key, out var item))
      {
        data.Remove(item);
        return item;
      }
      else
      {
        MakeSpaceForNewItem();
        return (key, create(key));
      }
    }

    private void MakeSpaceForNewItem()
    {
      if (data.Count >= size)
      {
        RemoveItem(data[data.Count - 1]);
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
        if (TryGet(key, out var item))
        {
          RemoveItem(item);
        };

      }
    }

    private void RemoveItem((TKey, TResult) itemToRemove)
    {
      (itemToRemove.Item2 as IDisposable)?.Dispose();
      data.Remove(itemToRemove);
    }

    public void Rekey(Func<TKey, TKey> func)
    {
      lock (data)
      {
        data = data.Select(i => (func(i.Item1), i.Item2)).ToList();

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
