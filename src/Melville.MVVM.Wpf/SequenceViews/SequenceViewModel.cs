using  System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Melville.MVVM.BusinessObjects;

namespace Melville.MVVM.Wpf.SequenceViews
{
  public sealed class SequenceViewModel<T> : NotifyBase
  {
    public ICollection<T> Collection { get; }
    [MaybeNull]
    [AllowNull]
    private T current = default;
    [MaybeNull]
    [AllowNull]
    public T Current
    {
      get => current;
      set => AssignAndNotify(ref current, value);
    }

    private Func<T>? newItemFactory;
    public Func<T>? NewItemFactory
    {
      get => newItemFactory;
      set => AssignAndNotify(ref newItemFactory, value);
    }

    private bool canDelete;
    public bool CanDelete
    {
      get => canDelete;
      set => AssignAndNotify(ref canDelete, value);
    }

    private bool canArrange;
    public bool CanArrange
    {
      get => canArrange;
      set => AssignAndNotify(ref canArrange, value, nameof(CanArrange), nameof(ArrangeType));
    }
    public Type? ArrangeType => CanArrange ? typeof(T) : null;

    private string title = "";
    public string Title
    {
      get => title;
      set => AssignAndNotify(ref title, value);
    }


    public SequenceViewModel(ICollection<T> collection)
    {
      Collection = collection;
    }

    public void AddNewItem()
    {
      if (NewItemFactory == null) return;
      var ret = NewItemFactory();
      Collection.Add(ret);
      Current = ret;
    }

    public void DeleteItem()
    {
      if (!CanDelete) return;
      Collection.Remove(Current);
      Current = default;
    }
  }
}