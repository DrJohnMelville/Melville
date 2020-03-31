using  System;
using System.Collections;
using System.Diagnostics;

namespace Melville.MVVM.AdvancedLists
{
  /// <summary>
  /// This class can be accessed from any thread and will appropriately marshall notification changes to the proper thread for CollectionView
  /// </summary>
  /// <typeparam name="T">The type of elements contained in the collection.</typeparam>
  public class ThreadSafeBindableCollection<T> : ObservableCollectionWithProperClearMethod<T>
  {
 
    protected override void ClearItems() => UiThreadBuilder.RunOnUiThread(base.ClearItems);

    protected override void InsertItem(int index, T item)
    {
      UiThreadBuilder.RunOnUiThread(() => base.InsertItem(index, item));
    }

    protected override void MoveItem(int oldIndex, int newIndex)
    {
      UiThreadBuilder.RunOnUiThread(() => base.MoveItem(oldIndex, newIndex));
    }

    protected override void RemoveItem(int index)
    {
      UiThreadBuilder.RunOnUiThread(() => base.RemoveItem(index));
    }

    protected override void SetItem(int index, T item)
    {
      UiThreadBuilder.RunOnUiThread(() => base.SetItem(index, item));
    }
  }

  public static class UiThreadBuilder
  {
    public static Action<Action> RunOnUiThread { get; set; } = a =>
    {
      a();
      Debug.WriteLine("Melville.MVVM.AdvancedLists.UiThreadBuilder.RunOnUIThread Delegate not registered -- collections not being routed to UI thread");
    };
  }
}