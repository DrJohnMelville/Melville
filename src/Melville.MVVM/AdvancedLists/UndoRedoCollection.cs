using  System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melville.Linq;
using Melville.MVVM.Undo;

namespace Melville.MVVM.AdvancedLists
{
  public class UndoRedoCollection<T> : IList<T>, IList
  {
    #region IList Implementation
    // required for collection view source
    private IList listImplementation => (IList)Target;
    IEnumerator IEnumerable.GetEnumerator() => listImplementation.GetEnumerator();

    void ICollection.CopyTo(Array array, int index) => listImplementation.CopyTo(array, index);

    object ICollection.SyncRoot => listImplementation.SyncRoot;
    bool ICollection.IsSynchronized => listImplementation.IsSynchronized;

    int IList.Add(object? value)
    {
      var ret = -1;
      undoEngine.PushAndDoAction(
        () => ret = listImplementation.Add(value),
        () => listImplementation.Remove(value));
      return ret;
    }

    bool IList.Contains(object? value) => listImplementation.Contains(value);


    int IList.IndexOf(object? value) => listImplementation.IndexOf(value);

    void IList.Insert(int index, object? value) =>
      undoEngine.PushAndDoAction(
        () => listImplementation.Insert(index, value),
        () => listImplementation.Remove(value));

    void IList.Remove(object? value)
    {
      var index = listImplementation.IndexOf(value);
      undoEngine.PushAndDoAction(
        () => listImplementation.Remove(value),
        () => listImplementation.Insert(index, value));
    }


    object? IList.this[int index]
    {
      get => listImplementation[index];
      set => listImplementation[index] = value;
    }
    bool IList.IsFixedSize => listImplementation.IsFixedSize;

    #endregion
    #region IList<T> Implementation

    private readonly UndoEngine undoEngine;
    protected readonly IList<T> Target;
    public UndoRedoCollection(IList<T> target, UndoEngine undoEngine)
    {
      this.Target = target;
      this.undoEngine = undoEngine;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Target.GetEnumerator();

    public void Add(T item) =>
      undoEngine.PushAndDoAction(
        () => Target.Add(item),
        () => Target.Remove(item));

    public void Clear()
    {
      var old = Target.ToArray();
      undoEngine.PushAndDoAction(
        () => Target.Clear(),
        () => Target.AddRange(old)
      );
    }

    public bool Contains(T item) => Target.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => Target.CopyTo(array, arrayIndex);

    public int Count => Target.Count;

    public bool IsReadOnly => Target.IsReadOnly;

    public int IndexOf(T item) => Target.IndexOf(item);

    public void Insert(int index, T item)
    {
      undoEngine.PushAndDoAction(() => Target.Insert(index, item), () => Target.RemoveAt(index));
    }

    public void RemoveAt(int index)
    {
      var value = Target[index];
      InnerDoRemove(index, value);
    }

    public bool Remove(T item)
    {
      int pos = Target.IndexOf(item);
      if (pos < 0) return false;
      InnerDoRemove(pos, item);
      return true;
    }

    private void InnerDoRemove(int index, T value)
    {
      undoEngine.PushAndDoAction(() => Target.RemoveAt(index), () => Target.Insert(index, value));
    }

    public T this[int index]
    {
      get => Target[index];
      set => Target[index] = value;
    }
    #endregion
  }
}