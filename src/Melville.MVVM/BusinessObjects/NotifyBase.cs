using  System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Melville.MVVM.CSharpHacks;
using Melville.MVVM.Functional;

namespace Melville.MVVM.BusinessObjects
{
  /// <summary>
  ///   A base class that implements the infrastructure for property change notification and automatically performs UI thread marshalling.
  /// </summary>
  public class NotifyBase : INotifyPropertyChanged
  {
    /// <summary>
    ///   Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///   Raises a change notification indicating that all bindings should be refreshed.
    /// </summary>
    public void Refresh()
    {
      OnPropertyChanged("");
    }

    /// <summary>
    ///   Notifies subscribers of the property change.
    /// </summary>
    /// <param name = "propertyName">SheetName of the property.</param>
    protected void OnPropertyChanged(string propertyName)
    {
      VerifyPropertyExists.InDebugBuilds(this, propertyName);
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void OnPropertyChanged(params string[] otherParams)
    {
      otherParams.ForEach(OnPropertyChanged);
    }


    protected bool AssignAndNotify<T>(ref T target, T value, string propertyName, params string[] otherParams)
    {
      var ret = AssignAndNotify(ref target, value, propertyName);
      if (ret)
      {
        OnPropertyChanged(otherParams);
      }
      return ret;
    }

    protected bool AssignAndNotify<T>(ref T target, T value, [CallerMemberName] string propertyName = "")
    {
      if (Equals(target, value)) return false;
      target = value;
      OnPropertyChanged(propertyName);
      return true;
    }

    public IDisposable DelegatePropertyChangeFrom(INotifyPropertyChanged? foreignObject,
      string foreignProperty, params string[] localProperties)
    {
      if (foreignObject == null) return new ActionOnDispose(() => { });
      VerifyValidDelegation(foreignObject, foreignProperty, localProperties);

      PropertyChangedEventHandler foreignObjectOnPropertyChanged = (s, e) =>
      {
        if (e.PropertyName.Equals(foreignProperty, StringComparison.Ordinal))
        {
          localProperties.ForEach(OnPropertyChanged);
        }
      };

      foreignObject.PropertyChanged += foreignObjectOnPropertyChanged;


      return new ActionOnDispose(() => foreignObject.PropertyChanged -= foreignObjectOnPropertyChanged);
    }

    [Conditional("DEBUG")]
    protected void VerifyValidDelegation(INotifyPropertyChanged foreignObject, string foreignProperty,
      string[] localProperties)
    {
#if DEBUG
      VerifyPropertyExists.InDebugBuilds(this, localProperties);
      if (foreignObject == null)
      {
        throw new InvalidOperationException("Delegation to a null object");
      }
      VerifyPropertyExists.InDebugBuilds(foreignObject, foreignProperty);
#endif
    }
  }
}