using  System;
using System.Windows;
using Melville.MVVM.Wpf.MouseDragging.Drag;

namespace Melville.MVVM.Wpf.MouseDragging
{
  public enum MouseMessageType
  {
    Down = 0,
    Move = 1,
    Up = 2
  }

  public interface IMouseDataRoot
  {
    void SendMousePosition(MouseMessageType type, Point position);
    void CancelMouseBinding();
    DragDropEffects InitiateDrag(IDataObject draggedData, DragDropEffects allowedEffects, LocalDragEventArgs args);
    IDragUIWindow ConstructDragWindow(FrameworkElement target, double opacity);
  }

  public interface IMouseDataSource
  {
    event EventHandler<LocalDragEventArgs> MouseMoved;
    IMouseDataRoot Root { get; }
  }

  public abstract class MouseDataSource : IMouseDataSource, IMouseDataRoot
  {
    public event EventHandler<LocalDragEventArgs>? MouseMoved;

    // this is a test hook
    public virtual void SendMousePosition(MouseMessageType type, Point position)
    {
      MouseMoved?.Invoke(this, CreateLocalDragEventArgs(type, position));
    }

    public abstract void CancelMouseBinding();
    public abstract DragDropEffects InitiateDrag(IDataObject draggedData, DragDropEffects allowedEffects,
      LocalDragEventArgs args);

    public abstract IDragUIWindow ConstructDragWindow(FrameworkElement target, double opacity);
    protected abstract LocalDragEventArgs CreateLocalDragEventArgs(MouseMessageType type, Point position);
    public IMouseDataRoot Root => this;

  }
}