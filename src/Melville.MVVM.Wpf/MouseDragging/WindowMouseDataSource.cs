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
    public event EventHandler<LocalDragEventArgs>? MouseMoved;
    public FrameworkElement Target { get; }
    object? IMouseDataSource.Target => Target;

    public WindowMouseDataSource(FrameworkElement target)
    {
      Target = target;
    }

    public void SendMousePosition(MouseMessageType type, Point position) =>
      MouseMoved?.Invoke(this, new LocalDragEventArgs(position, type));


    public void BindToPhysicalMouse(MouseButtonEventArgs args)
    {
      binding = new WindowMouseBinding(Target, args, SendMousePosition);
    }

    public void CancelMouseBinding() => binding?.ReleaseBindings();
  }
}