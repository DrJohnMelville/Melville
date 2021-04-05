using System;
using System.ComponentModel;
using Melville.Lists.ListMonitors;

namespace Melville.Lists
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