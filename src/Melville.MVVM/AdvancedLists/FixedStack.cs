using System;
using  System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Melville.MVVM.AdvancedLists
{
  public sealed class FixedStack<T> : INotifyPropertyChanged
  {
    private readonly List<T> items;


    private readonly int depth;

    public FixedStack(int depth)
    {
      this.depth = depth;
      items = new List<T>(depth);
    }

    public void Push(T item)
    {
      if (items.Count >= depth)
      {
        items.RemoveAt(0);
      }
      items.Add(item);
      if (items.Count == 1)
      {
        OnPropertyChanged("HasItems");
      }
    }

    public T Pop()
    {
      if (!HasItems)
      {
        throw new InvalidOperationException("Attempt to pop an empty stack.");
      }
      var item = items[items.Count - 1];
      items.RemoveAt(items.Count - 1);
      if (items.Count == 0)
      {
        OnPropertyChanged("HasItems");
      }
      return item;
    }

    public T Peek()
    {
      if (!HasItems)
      {
        throw new InvalidOperationException("Attempt to peek an empty stack.");
      }

      return items[items.Count - 1];
    }

    public bool HasItems => items.Count > 0;

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = "") => 
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void Clear()
    {
      items.Clear();
      OnPropertyChanged("HasItems");
    }

    public bool Contains(T item) => items.Contains(item);
  }
}