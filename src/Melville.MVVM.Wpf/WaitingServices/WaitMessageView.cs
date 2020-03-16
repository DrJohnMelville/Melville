using  System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Melville.MVVM.WaitingServices;
using Melville.MVVM.Wpf.EventBindings;

namespace Melville.MVVM.Wpf.WaitingServices
{
  public sealed class WaitMessageView : ContentControl, IAdditionlTargets
  {

    static WaitMessageView()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(WaitMessageView), new FrameworkPropertyMetadata(typeof(WaitMessageView)));
    }

    private static readonly DependencyPropertyKey ServicePropertyKey = DependencyProperty.RegisterReadOnly(
      "Service", typeof(IWaitingService), typeof(WaitMessageView), new PropertyMetadata(null));
    public static readonly DependencyProperty ServiceProperty = ServicePropertyKey.DependencyProperty;
    public IWaitingService Service
    {
      get => (IWaitingService)GetValue(ServiceProperty);
      private set => SetValue(ServicePropertyKey, value);
    }

    public IEnumerable<object> Targets() => new object[] { Service };

    public WaitMessageView()
    {
      Service = new WaitMessageDriver();
    }
  }
}