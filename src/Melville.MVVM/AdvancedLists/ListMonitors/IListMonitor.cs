using  System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Melville.MVVM.AdvancedLists.ListMonitors
{
  public interface IListMonitor<TItem>
  {
    void NewItem(TItem item, int position);
    void DestroyItem(TItem item, int position);
    void Move(int oldPosition, int newPosition, int length);
    void Reset();
    void Initalize(IList<TItem> collection);
  }

  public static class ListMonitorOperations
  {
    public static Action AttachToList<T, TList>(this IListMonitor<T> monitor, TList list) where TList :
      ObservableCollectionWithProperClearMethod<T>, INotifyCollectionChanged
    {
      void CollectionChanged(object? s, NotifyCollectionChangedEventArgs e) => HandleCollectionChanged(list, monitor, e);
      list.CollectionChanged += CollectionChanged;
      monitor.Initalize(list);
      return () => list.CollectionChanged -= CollectionChanged;
    }
    private static void Clearing<T>(IEnumerable<T> list, IListMonitor<T> monitor)
    {
      int position = 0;
      foreach (var item in list)
      {
        monitor.DestroyItem(item, position++);
      }
    }

    private static void HandleCollectionChanged<T>(IList<T> list,
      IListMonitor<T> monitor, NotifyCollectionChangedEventArgs e)
    {
      int position = 0;
      switch (e.Action)
      {
        case NotifyCollectionChangedAction.Add:
          position = e.NewStartingIndex;
          foreach (var item in e.NewItems?.Cast<T>() ?? Array.Empty<T>())
          {
            monitor.NewItem(item, position++);
          }
          break;
        case NotifyCollectionChangedAction.Remove:
          position = e.OldStartingIndex;
          foreach (var item in e.OldItems?.Cast<T>() ?? Array.Empty<T>())
          {
            monitor.DestroyItem(item, position++);
          }
          break;
        case NotifyCollectionChangedAction.Replace:
          if (e.NewItems == null || e.OldItems == null) break;
          for (int i = 0; i < e.NewItems.Count; i++)
          {
            if (e.OldItems[i] is T oldItem)
            {
              monitor.DestroyItem(oldItem, e.OldStartingIndex + i);
            }

            if (e.NewItems[i] is T newItem)
            {
              monitor.NewItem(newItem, e.NewStartingIndex + i);
            }
          }
          break;
        case NotifyCollectionChangedAction.Move:
          monitor.Move(e.OldStartingIndex, e.NewStartingIndex, e.NewItems?.Count ?? 0);
          break;
        case NotifyCollectionChangedAction.Reset:
          monitor.Reset();
          if (e.OldItems != null)
          {
            Clearing(e.OldItems.OfType<T>(), monitor);
          }
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

}
