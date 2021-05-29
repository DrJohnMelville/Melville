using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.WpfHacks;

namespace Melville.MVVM.Wpf.MouseDragging
{

  public sealed class WindowMouseDataSource  : IMouseDataSource
  {
    private WindowMouseBinding? binding;
    public event ReportMouseMove? MouseMoved;
    public FrameworkElement Target { get; }
    object? IMouseDataSource.Target => Target;
    public Point InitialPoint { get; }

    public WindowMouseDataSource(FrameworkElement target, Point initialPoint)
    {
      Target = target;
      InitialPoint = initialPoint;
    }

    public void SendMousePosition(MouseMessageType type, Point position) =>
      MouseMoved?.Invoke(type, position);


    public void BindToPhysicalMouse(MouseButtonEventArgs args)
    {
      binding = new WindowMouseBinding(Target, args, SendMousePosition);
    }

    public void CancelMouseBinding() => binding?.ReleaseBindings();
  }
}