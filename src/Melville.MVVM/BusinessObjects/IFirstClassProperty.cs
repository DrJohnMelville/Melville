using  System.ComponentModel;

namespace Melville.MVVM.BusinessObjects
{
  public interface IFirstClassProperty<T> : INotifyPropertyChanged
  {
    T Value { get; set; }
  }
}