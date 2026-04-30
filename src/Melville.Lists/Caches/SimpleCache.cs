using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Melville.Lists.Caches;

public sealed class SimpleCache<TKey, TResult>(int size, Func<TKey, TResult> create)
{
    private class IndexedItems (TResult item, int counter)
    {
        public TResult Item { get; } = item;
        public int Counter { get; set; } = counter;
    }

  private volatile int indexes = int.MinValue;
  private int NextIndex() => Interlocked.Increment(ref indexes);

  private readonly ConcurrentDictionary<TKey, IndexedItems> items = new();

    public SimpleCache(Func<TKey, TResult> create) : this(int.MaxValue, create)
  {
    // do nothing
  }

  public IEnumerable<TResult> CachedItems => items.Values.Select(i => i.Item);

  public TResult Get(TKey key)
    {
        var item = items.GetOrAdd(key, k => new IndexedItems(create(k), 0));
        item.Counter = NextIndex();
        TryTrimToSize();
        return item.Item;
    }

    private void TryTrimToSize()
    {
        if (items.Count > size)
        {
            Forget(OldestItem().Key);
        }
    }

    private KeyValuePair<TKey, IndexedItems> OldestItem()
    {
        return items.MinBy(i => i.Value.Counter);
    }

    public void Forget(TKey key) => items.Remove(key, out _);
  
    public void Clear() => items.Clear();
}