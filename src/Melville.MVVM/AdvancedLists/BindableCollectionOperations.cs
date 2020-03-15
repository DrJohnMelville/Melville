using  System;
using System.ComponentModel;
using Melville.MVVM.AdvancedLists.ListMonitors;

namespace Melville.MVVM.AdvancedLists
{
  public static class BindableCollectionOperations
  {
    public static Action MonitorMemberPropertyChanges<T>(this ThreadSafeBindableCollection<T> list,
      EventHandler<PropertyChangedEventArgs> handler) where T : INotifyPropertyChanged
    {
      var monitor = new DeepPropertyChangeNotification<T>();
      monitor.SubPropertyChanged += handler;
      return monitor.AttachToList(list);
    }
  }
}