using  System;
using System.Collections;

namespace Melville.MVVM.AdvancedLists
{
  public interface ICollectionWithUIMutex
  {
    void RegisterCollectionWithMutex(IEnumerable target);
  }
  /// <summary>
  /// This class can be accessed from any thread and will appropriately marshall notification changes to the proper thread for CollectionView
  /// </summary>
  /// <typeparam name="T">The type of elements contained in the collection.</typeparam>
  public class ThreadSafeBindableCollection<T> : ObservableCollectionWithProperClearMethod<T>, ICollectionWithUIMutex
  {
    private readonly object mutex = new object();
    public ThreadSafeBindableCollection()
    {
      ((ICollectionWithUIMutex)this).RegisterCollectionWithMutex(this);
    }

    protected override void ClearItems()
    {
      lock (mutex)
      {
        base.ClearItems();
      }
    }

    protected override void InsertItem(int index, T item)
    {
      lock (mutex)
      {
        base.InsertItem(index, item);
      }
    }

    protected override void MoveItem(int oldIndex, int newIndex)
    {
      lock (mutex)
      {
        base.MoveItem(oldIndex, newIndex);
      }
    }

    protected override void RemoveItem(int index)
    {
      lock (mutex)
      {
        base.RemoveItem(index);
      }
    }

    protected override void SetItem(int index, T item)
    {
      lock (mutex)
      {
        base.SetItem(index, item);
      }
    }

    void ICollectionWithUIMutex.RegisterCollectionWithMutex(IEnumerable target)
    {
      ThreadSafeCollectionBuilder.Fixup(target, mutex);
    }
  }

  public static class ThreadSafeCollectionBuilder
  {
    internal static Action<IEnumerable, object> Fixup { get; private set; } = (_, __) => { };
      // throw new InvalidOperationException("Cannot use threadsafe collections without calling ThreadsafeCollectionBuilder.SetFixupHook," +
      //                                     "usually with a value of BindingOperations.EnableColletionSynchronization.");

    public static void SetFixupHook(Action<IEnumerable, object> fixup) => Fixup = fixup;
  }
}