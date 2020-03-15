using  System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Melville.MVVM.AdvancedLists.ListMonitors
{
  public sealed class DeepPropertyChangeNotification<T> : IListMonitor<T>
    where T : INotifyPropertyChanged
  {
    public EventHandler<PropertyChangedEventArgs>? SubPropertyChanged;

    private void ItemPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
      SubPropertyChanged?.Invoke(sender, args);
    }


    public void NewItem(T item, int position)
    {
      if (item == null) return;
      item.PropertyChanged += ItemPropertyChanged;
    }
    public void DestroyItem(T item, int position)
    {
      if (item == null) return;
      item.PropertyChanged -= ItemPropertyChanged;
    }
    public void Move(int oldPosition, int newPosition, int length)
    {
      // couldn't care less
    }
    public void Reset()
    {
      // couldn't care less detaching is done through the destroy item methods
    }
    public void Initalize(IList<T> collection)
    {
      foreach (var item in collection)
      {
        NewItem(item, -1);
      }
    }
  }
}