using  System;
using System.Windows;
using Melville.MVVM.Wpf.MouseDragging.Drag;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

namespace Melville.MVVM.Wpf.MouseDragging
{
  public enum MouseMessageType
  {
    Down = 0,
    Move = 1,
    Up = 2
  }

  public delegate void ReportMouseMove(MouseMessageType type, Point point);
  public interface IMouseDataSource
  {
    event ReportMouseMove MouseMoved;
    void SendMousePosition(MouseMessageType type, Point position);
    void CancelMouseBinding();
    object? Target { get; }
  }

  public static class MouseDataSourceOperations
  {
    public static void Drag(this IMouseDataSource mon, IDataObject dataToDrag, DragDropEffects allowedEffects,
      Action<DragDropEffects>? onDragDone = null) => 
      Drag(mon, () => dataToDrag, allowedEffects, onDragDone);
    public static void Drag(this IMouseDataSource mon, Func<IDataObject> dataToDrag, DragDropEffects allowedEffects,
      Action<DragDropEffects>? onDragDone = null)
    {
      mon.BindLocalDragger(
        LocalDragger.MinimumDrag(SystemParameters.MinimumVerticalDragDistance,
          LocalDragger.MaxMoves(1, LocalDragger.Action((_, __) =>
          {
            var ret = new DragHandler(mon).InitiateDrag(dataToDrag(), allowedEffects);
            onDragDone?.Invoke(ret);
          }))));
    }

    public static void BindLocalDragger(
      this IMouseDataSource source, Func<IMouseDataSource, ILocalDragger<Point>> dragger) =>
      BindLocalDragger(source, dragger(source));
    public static void BindLocalDragger(this IMouseDataSource source, ILocalDragger<Point> dragger) => 
      source.MouseMoved += dragger.NewPoint;
  }
}