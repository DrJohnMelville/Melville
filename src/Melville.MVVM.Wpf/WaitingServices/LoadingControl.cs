using  System.Windows;
using System.Windows.Controls;

namespace Melville.MVVM.Wpf.WaitingServices;

public sealed class LoadingControl : ContentControl
{
  static LoadingControl()
  {
    DefaultStyleKeyProperty.OverrideMetadata(typeof(LoadingControl), new FrameworkPropertyMetadata(typeof(LoadingControl)));
  }



  public object Target
  {
    get { return GetValue(TargetProperty); }
    set { SetValue(TargetProperty, value); }
  }

  // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty TargetProperty =
    DependencyProperty.Register("Target", typeof(object), typeof(LoadingControl), new PropertyMetadata(null));

}