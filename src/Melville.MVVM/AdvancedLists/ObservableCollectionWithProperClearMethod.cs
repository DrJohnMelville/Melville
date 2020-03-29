using  System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace Melville.MVVM.AdvancedLists
{
  
  public class ObservableCollectionWithProperClearMethod<T> : ObservableCollection<T>
  {
    protected override void ClearItems()
    {
      var items = new List<T>(this);
      base.ClearItems();
      foreach (var item in items)
      {
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
      }
    }
  }
  
}