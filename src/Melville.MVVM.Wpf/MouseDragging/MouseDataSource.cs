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


  public interface IMouseDataSource
  {
    event EventHandler<LocalDragEventArgs> MouseMoved;
    IMouseDataSource Root { get; }
    void SendMousePosition(MouseMessageType type, Point position);
    void CancelMouseBinding();
    DragDropEffects InitiateDrag(IDataObject draggedData, DragDropEffects allowedEffects);
    object? Target { get; }
  }
}