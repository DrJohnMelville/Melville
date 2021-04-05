using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Melville.Linq;

namespace Melville.Lists.PersistentLinq
{
  public class SelectManyCollection<TParent, TItem> : IList<TItem>, INotifyCollectionChanged, INotifyPropertyChanged
  {
    private readonly Func<TParent, IList<TItem>> subItems;
    private readonly IList<TParent> parentList;
    public SelectManyCollection(IList<TParent> parentList,
      Func<TParent, IList<TItem>> subItem)
    {
      this.parentList = parentList;

      subItems = subItem;
      if (parentList is INotifyCollectionChanged parentListAsINCC)
      {
        parentListAsINCC.CollectionChanged += ParentCollectionChanged;
      }
      foreach (var item in this.parentList.Select(subItem).OfType<INotifyCollectionChanged>())
      {
        item.CollectionChanged += SubCollectionChanged;
      }
    }
    
    private void ParentCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
      // we need all the sublists to find new items when they show up.
      DoItemsAction(e.NewItems, i =>i.CollectionChanged += SubCollectionChanged);
      // we try to be a good citizen and only hang on to notifications that are our sublists
      DoItemsAction(e.OldItems, i =>
        RemoveSubCollectionChangedNotification(i));

      OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      OnPropertyChanged("Count"); 
    }

    private void RemoveSubCollectionChangedNotification(INotifyCollectionChanged i) => i.CollectionChanged -= SubCollectionChanged;

    private void DoItemsAction(IList? collection, Action<INotifyCollectionChanged> action) => 
      collection?.OfType<TParent>().Select(subItems).OfType<INotifyCollectionChanged>().ForEach(action);
    private void SubCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
      if (sender == null)
      {
        sender = FindParentListForItem(e);
      }
      else
      {
        if (EliminateSpuriousCollectionNotification(sender)) return;
      }

      var newBase = parentList.
        Select(i => subItems(i)).
        TakeWhile(i => i != sender).
        Sum(i => i.Count);

      OnCollectionChange(CreateNewNotifier(e, newBase));
    }

    /// <summary>
    /// This is a bugfix.  If the child collection executes a CLEAR, then by the time we find out about
    /// it the elements are gone, and I have no way to unsubscribe from the previous child collections sending me
    /// notifications that theyare changing.  This can lead to me getting notifications from lists that are no longer
    /// members of me.
    ///
    /// If I ger a notification from a list that is not one of my children, I just unsubscribe and
    /// then terminate the notification.
    /// </summary>
    /// <param name="sender">The sender of the change notification</param>
    /// <returns>True if the notification is spurious, false if the notification is valid.</returns>
    private bool EliminateSpuriousCollectionNotification(object sender)
    {
      if (parentList.Any(i => subItems(i) == sender) || !(sender is INotifyCollectionChanged incc)) return false;
      RemoveSubCollectionChangedNotification(incc);
      return true;
    }

    private object? FindParentListForItem(NotifyCollectionChangedEventArgs e) =>
      ItemToSearchFor(e) is {} item 
        ? parentList.Select(i => subItems(i)).FirstOrDefault(i =>
          i.Contains(item)) 
        : null;

    private static TItem? ItemToSearchFor(NotifyCollectionChangedEventArgs e) =>
      ((e.NewItems, e.OldItems) switch
      {
        (null, null) => Array.Empty<TItem>(),
        (var a, null) => a.Cast<TItem>(),
        (null, var b) => b.Cast<TItem>(),
        var (a, b) => a.Cast<TItem>().Concat(b.Cast<TItem>())
      }).FirstOrDefault();

    private NotifyCollectionChangedEventArgs CreateNewNotifier(NotifyCollectionChangedEventArgs args, int newBase)
    {
      switch (args.Action)
      {
        case NotifyCollectionChangedAction.Add:
          OnPropertyChanged("Count");
          return new NotifyCollectionChangedEventArgs(args.Action, args.NewItems, args.NewStartingIndex + newBase);
        case NotifyCollectionChangedAction.Remove:
          OnPropertyChanged("Count");
          return new NotifyCollectionChangedEventArgs(args.Action, args.OldItems, args.OldStartingIndex + newBase);
        case NotifyCollectionChangedAction.Replace:
          return new NotifyCollectionChangedEventArgs(args.Action, args.NewItems ?? Array.Empty<object>(), 
            args.OldItems ?? Array.Empty<object>(),
            args.NewStartingIndex + newBase);
        case NotifyCollectionChangedAction.Move:
          return new NotifyCollectionChangedEventArgs(args.Action, args.NewItems, args.NewStartingIndex + newBase,
            args.OldStartingIndex + newBase);
        case NotifyCollectionChangedAction.Reset:
          OnPropertyChanged("Count");
          return new NotifyCollectionChangedEventArgs(args.Action);
        default:
          Debug.Fail("Incorrect notify type");
          // we have to do something in the release build, the following line is dead in debug
          return new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
      }
    }

    #region Implementation of INotifyCollectionChanged
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private void OnCollectionChange(NotifyCollectionChangedEventArgs e) => 
      UiThreadBuilder.RunOnUiThread(()=> CollectionChanged?.Invoke(this, e));

    #endregion

    #region Implementation of INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string property) => 
      UiThreadBuilder.RunOnUiThread(()=>PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property)));

    #endregion

    #region Implementation of IEnumerable
    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
    public IEnumerator<TItem> GetEnumerator()
    {
      return WholeList().GetEnumerator();
    }
    private IEnumerable<TItem> WholeList()
    {
      return parentList.SelectMany(i => subItems(i));
    }
    #endregion

    #region Implementation of ICollection<T>
    public void Add(TItem item)
    {
      throw new NotSupportedException();
    }
    public void Clear()
    {
      throw new NotSupportedException();
    }
    public bool Contains(TItem item)
    {
      return WholeList().Contains(item);
    }
    public void CopyTo(TItem[] array, int arrayIndex)
    {
      int baseIndex = arrayIndex;
      foreach (var parent in parentList)
      {
        var list = subItems(parent);
        list.CopyTo(array, baseIndex);
        baseIndex += list.Count;
      }
    }
    public bool Remove(TItem item)
    {
      throw new NotImplementedException();
    }
    public int Count
    {
      get { return parentList.Sum(i => subItems(i).Count); }
    }
    public bool IsReadOnly
    {
      get { return false; }
    }
    #endregion

    #region Implementation of IList<TraumaItem>
    public int IndexOf(TItem item)
    {
      int baseAddr = 0;
      foreach (var traumaSheet in parentList)
      {
        int check = subItems(traumaSheet).IndexOf(item);
        if (check >= 0)
        {
          return check + baseAddr;
        }
        baseAddr += subItems(traumaSheet).Count;
      }
      return -1;
    }
    public void Insert(int index, TItem item)
    {
      throw new NotImplementedException();
    }
    public void RemoveAt(int index)
    {
      throw new NotImplementedException();
    }
    public TItem this[int index]
    {
      get
      {
        var reference = FindPosition(index);
        return reference.Item1[reference.Item2];
      }
      set
      {
        var reference = FindPosition(index);
        reference.Item1[reference.Item2] = value;
      }
    }
    private Tuple<IList<TItem>, int> FindPosition(int input)
    {
      int baseAddr = 0;
      foreach (var traumaSheet in parentList)
      {
        var items = subItems(traumaSheet);

        if (baseAddr + items.Count > input)
        {
          return Tuple.Create(items, input - baseAddr);
        }
        baseAddr += items.Count;
      }
      throw new IndexOutOfRangeException();
    }
    #endregion

  }
}