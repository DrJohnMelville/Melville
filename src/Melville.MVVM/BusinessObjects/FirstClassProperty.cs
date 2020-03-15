using  System.ComponentModel;

namespace Melville.MVVM.BusinessObjects
{
  public sealed class FirstClassProperty<T>:IFirstClassProperty<T>
  {
    private T value;
    public T Value
    {
      get { return value; }
      set { 
        this.value = value;
        OnPropertyChanged("Value");
      }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public FirstClassProperty(T value)
    {
      this.value = value;
    }
  }
}
