using  System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Melville.MVVM.Functional
{
  public static class ListExtensions
  {
    public static int InsertOrMoveItem<T>(this IList<T> list, int newPosition, T item,
      bool allowMoveOptimination = true)
    {
      if (newPosition < 0)
      {
        newPosition = list.Count;
      }

      int removalIndex = list.IndexOf(item);
      if (removalIndex >= 0)
      {
        if (removalIndex == newPosition) return newPosition;
        var oc = list as ObservableCollection<T>;
        if (removalIndex < newPosition)
        {
          newPosition--;
        }

        if (oc != null && allowMoveOptimination)
        {
          oc.Move(removalIndex, newPosition);
          return newPosition;
        }

        list.RemoveAt(removalIndex);
      }

      int finalIndex = (newPosition < 0 ? list.Count : newPosition).Clamp(0, list.Count);
      list.Insert(finalIndex, item);
      return finalIndex;
    }

    public static void AddIfNotPresent<T>(this IList<T> list, T item)
    {
      if (!list.Contains(item))
      {
        list.Add(item);
      }
    }

    public static void InsertMax<T>(this IList<T> list, int position, T newItem)
    {
      list.Insert(position.Clamp(0, list.Count), newItem);
    }

    public static void InsertAfter<T>(this IList<T> list, T priorItem, T newItem)
    {
      list.InsertOrMoveItem(list.IndexOf(priorItem) + 1, newItem);
    }

    public static void InsertBefore<T>(this IList<T> list, T priorItem, T newItem)
    {
      list.InsertOrMoveItem(list.IndexOf(priorItem), newItem);
    }

    public static int ForceIndexOf<T>(this List<T> list, T item, Action<T>? createItem = null)
    {
      var ret = list.IndexOf(item);
      if (ret < 0)
      {
        ret = list.Count;
        list.Add(item);
        createItem?.Invoke(item);
      }

      return ret;
    }

    /// <summary>
    /// Add all of the items in items to the list.
    /// </summary>
    /// <typeparam name="T">Element type of the list</typeparam>
    /// <param name="list">List to add elements to</param>
    /// <param name="items">Items to add</param>
    public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
    {
      foreach (var item in items)
      {
        list.Add(item);
      }
    }

    /// <summary>
    /// Remove all of the items in items from the list.
    /// </summary>
    /// <typeparam name="T">Element type of the list</typeparam>
    /// <param name="list">List to remove elements from</param>
    /// <param name="items">Items to remove</param>
    public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> items)
    {
      foreach (var item in items)
      {
        list.Remove(item);
      }
    }

    public static async Task AddRangeAsync<T>(this ICollection<T> list, IAsyncEnumerable<T> source)
    {
      await foreach(var item in source) list.Add(item);
    }
  }
}