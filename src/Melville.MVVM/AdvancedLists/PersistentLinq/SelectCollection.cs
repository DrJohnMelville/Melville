using  System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Melville.MVVM.Functional;
using Melville.MVVM.Invariants;

namespace Melville.MVVM.AdvancedLists.PersistentLinq
{

  /// <summary>
  /// Select collection allows the user to inzert an item into the select collection.
  /// In order to modify the source collection, must know the corresponding value in the source collection, which is what this does.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public interface IHasSelectCollectionKey<out T>
  {
    T Model { get; }
  }

  /// <summary>
  /// This is a persistent select clause
  /// 
  /// This class lazily loads its elements, but it does odd things if the
  /// elements of the base collection are not unique.
  /// </summary>
  /// <typeparam name="TSource"The soure of the select clause.</typeparam>
  /// <typeparam name="TDest">Destination type of the select clause</typeparam>
  public sealed class SelectCollection<TSource, TDest> : IList<TDest>, INotifyCollectionChanged, IDisposable, IList
    where TSource:notnull
  {
    private readonly IList<TSource> source;
    private readonly Func<TSource, TDest> selector;
    public bool DisposeOnRemove { get; set; }

    /// <summary>
    /// Creates a collection which mirrors another collection, each element being the result of a function of the element
    /// in the source collection.
    /// </summary>
    /// <param name="source">The collection to mirror</param>
    /// <param name="selector">the function that defines the transformation</param>
    /// <param name="collectionToShareCacheWith">Allows you to share the cache with another select collection.</param>
    public SelectCollection(IList<TSource> source, Func<TSource, TDest> selector) : this(source, selector, new Dictionary<TSource, TDest>())
    {
      DisposeOnRemove = true;
    }

    /// <summary>
    /// Creates a collection which mirrors another collection, each element being the result of a function of the element
    /// in the source collection.
    /// </summary>
    /// <param name="source">The collection to mirror</param>
    /// <param name="selector">the function that defines the transformation</param>
    /// <param name="collectionToShareCacheWith">Allows you to share the cache with another select collection.</param>
    public SelectCollection(IList<TSource> source, Func<TSource, TDest> selector,
      IDictionary<TSource, TDest> sharedCache) 
    {
      cache = sharedCache;

      this.source = source;
      this.selector = selector;

      if (source is INotifyCollectionChanged INCC)
      {
        INCC.CollectionChanged += SourceCollectionChanged;
      }
    }

    #region Cache
    private IDictionary<TSource, TDest> cache;
    [return: MaybeNull]
    public TDest Map(TSource input)
    {
      TDest ret;
      if (cache == null)
      {
        return default!;
      }
      if (!cache.TryGetValue(input, out ret))
      {
        ret = selector(input);
        cache.Add(input, ret);
      }
      return ret;
    }

    [return:MaybeNull]
    public TSource ReverseMap(TDest output)
    {
      if (cache == null)
      {
        return default(TSource)!;
      }

      return cache
        .Where(i => Equals(output, i.Value))
        .Select(i => i.Key)
        .FirstOrDefault();
    }

    [return:MaybeNull]
    public TDest TryCache([AllowNull]TSource key) => 
      (!object.Equals(key, default(TSource)!)) && cache.TryGetValue(key, out var ret) ? ret : default!;
    private void RemoveItemsFromCache(IList items) => items.OfType<TSource>().ForEach(i => cache.Remove(i));
    #endregion

    #region Implementation of IEnumerable
    public IEnumerator<TDest> GetEnumerator() => ConverterEnumerable().GetEnumerator();
    private IEnumerable<TDest> ConverterEnumerable() => source.Select(Map);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    #region Implementation of ICollection<TDest>
    public void Add(TDest item) => Insert(Count, item);
    public void Clear() => throw new NotSupportedException();
    public bool Contains(TDest item) => ConverterEnumerable().Contains(item);
    public void CopyTo(TDest[] array, int arrayIndex) => ConverterEnumerable().ToArray().CopyTo(array, arrayIndex);

    public bool Remove(TDest item) => source.Remove(cache.Where(i => Object.Equals(i.Value, item)).Select(i => i.Key).FirstOrDefault());
    public int Count => source.Count;
    public bool IsReadOnly => true;
    #endregion

    #region Implementation of IList<TDest>
    public int IndexOf(TDest item)
    {
      for (int i = 0; i < source.Count; i++)
      {
        if (Equals(this[i], item))
        {
          return i;
        }
      }
      return -1;
    }
    public void Insert(int index, TDest item)
    {
      var model = GetSelectionKey(item);
      cache[model] = item;
      source.Insert(index, model);
    }

    private TSource GetSelectionKey(TDest item) =>
      item is IHasSelectCollectionKey<TSource> hm
        ? hm.Model
        : cache.First(i => Object.Equals(i.Value, item)).Key;

    public void RemoveAt(int index)
    {
      source.RemoveAt(index);
    }
    public TDest this[int index]
    {
      get => Map(source[index]);
      set => throw new NotSupportedException();
    }
    #endregion

    #region IList Implementation
    void ICollection.CopyTo(Array array, int index) => ConverterEnumerable().ToArray().CopyTo(array, index);
    bool ICollection.IsSynchronized => false;
    object ICollection.SyncRoot => this;
    int IList.Add(object? value)
    {
      if (value is TDest d)
      {
        Add(d);
        return Count - 1;
      }

      return -1;
    }
    bool IList.Contains(object? value) => value is TDest d && Contains(d);
    int IList.IndexOf(object? value) => value is TDest d ?IndexOf(d):-1;
    void IList.Insert(int index, object? value)
    {
      if (value is TDest d)
      {
        Insert(index, d);
      }
    }

    object? IList.this[int index]
    {
      get => this[index]; 
      set
      {
        if (value is TDest d)
        {
          this[index] = d;
        }
      } 
    }
    bool IList.IsFixedSize => false;
    void IList.Remove(object? value)
    {
      if (value is TDest d)
      {
        Remove(d);
      }
    }

    #endregion

    #region Implementation of INotifyCollectionChanged
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    private void FireCollectionChanged(NotifyCollectionChangedEventArgs args) => CollectionChanged?.Invoke(this, args);
    private void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => FireCollectionChanged(ConvertCollectionChangedArgs(e));
    private NotifyCollectionChangedEventArgs ConvertCollectionChangedArgs(NotifyCollectionChangedEventArgs e) =>
      e.Action switch
      {
        NotifyCollectionChangedAction.Add => HandleAddEvent(e),
        NotifyCollectionChangedAction.Remove => HandleRemoveEvent(e),
        NotifyCollectionChangedAction.Move => HandleMoveEvent(e),
        NotifyCollectionChangedAction.Replace => HandleReplaceEvent(e),
        NotifyCollectionChangedAction.Reset => new NotifyCollectionChangedEventArgs(e.Action), 
        _ => throw new ArgumentOutOfRangeException("Action", "Action is not a valid collection change")
      };

    private NotifyCollectionChangedEventArgs HandleMoveEvent(NotifyCollectionChangedEventArgs e) =>
      new NotifyCollectionChangedEventArgs(e.Action,
        e.OldItems.OfType<TSource>().Select(Map).ToArray(), e.NewStartingIndex,
        e.OldStartingIndex);

    private NotifyCollectionChangedEventArgs HandleReplaceEvent(NotifyCollectionChangedEventArgs e)
    {
      IList itemsToDispose = e.OldItems.OfType<TSource>().Select(Map).ToArray();
      var ret = new NotifyCollectionChangedEventArgs(e.Action,
        e.NewItems.OfType<TSource>().Select(Map).ToArray(),
        itemsToDispose);
      RemoveItemsFromCache(e.OldItems);
      DisposeList(itemsToDispose);
      return ret;
    }

    private NotifyCollectionChangedEventArgs HandleRemoveEvent(NotifyCollectionChangedEventArgs e)
    {
      IList itemsToDispose = e.OldItems.OfType<TSource>().Select(Map).ToArray();
      MyDebug.Assert(itemsToDispose.Count == e.OldItems.Count);
      var ret = new NotifyCollectionChangedEventArgs(e.Action,
        itemsToDispose, e.OldStartingIndex);
      RemoveItemsFromCache(e.OldItems);
      DisposeList(itemsToDispose);
      return ret;
    }

    private NotifyCollectionChangedEventArgs HandleAddEvent(NotifyCollectionChangedEventArgs e)
    {
      var itemsToAdd = e.NewItems.OfType<TSource>().Select(Map).ToArray();
      MyDebug.Assert(itemsToAdd.Length == e.NewItems.Count);
      return new NotifyCollectionChangedEventArgs(e.Action,
        itemsToAdd, e.NewStartingIndex);
    }
    #endregion

    #region Implementation of IDisposable
    public void Dispose()
    {
      if (cache == null) return;
      if (source is INotifyCollectionChanged INCC)
      {
        INCC.CollectionChanged -= SourceCollectionChanged;
      }
      // clean out the cache
      if (DisposeOnRemove)
      {
        cache.Values.OfType<IDisposable>().ForEach(i => i.Dispose());
        cache.Clear(); 
      }
    }

    public void DisposeList(IEnumerable items)
    {
      if (!DisposeOnRemove) return;
      items.OfType<IDisposable>().ForEach(i => i.Dispose());
    }
    #endregion

    public IEnumerable<TDest> AllMappedDestinationObjects() => cache.Values;

  }
}
