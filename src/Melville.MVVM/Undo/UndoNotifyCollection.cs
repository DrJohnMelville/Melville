using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Melville.MVVM.Undo
{
  public class UndoNotifyCollection<T, TColl> : UndoRedoCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    where TColl : IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
  {
    public UndoNotifyCollection(TColl target, UndoEngine undoEngine) : base(target, undoEngine)
    {
    }

    public event PropertyChangedEventHandler? PropertyChanged
    {
      add => ((INotifyPropertyChanged)Target).PropertyChanged += value;
      remove => ((INotifyPropertyChanged)Target).PropertyChanged -= value;
    }
    public event NotifyCollectionChangedEventHandler? CollectionChanged
    {
      add => ((INotifyCollectionChanged)Target).CollectionChanged += value;
      remove => ((INotifyCollectionChanged)Target).CollectionChanged -= value;
    }

  }
}