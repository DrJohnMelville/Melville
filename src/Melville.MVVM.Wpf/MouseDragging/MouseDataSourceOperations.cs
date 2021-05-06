using  System;
using System.Windows;
using Melville.Hacks;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;

namespace Melville.MVVM.Wpf.MouseDragging
{
  public static class MouseDataSourceOperations
  {
    [Obsolete]
    public static IMouseDataSource Bind(this IMouseDataSource mon, Action<double, double> boundAction)
    {
      mon.MouseMoved += (s, e) => boundAction(e.TransformedPoint.X, e.TransformedPoint.Y);
      return mon;
    }

    public static void Drag(this IMouseDataSource mon, IDataObject dataToDrag, DragDropEffects allowedEffects,
      Action<DragDropEffects>? onDragDone = null) => 
      Drag(mon, () => dataToDrag, allowedEffects, onDragDone);
    public static void Drag(this IMouseDataSource mon, Func<IDataObject> dataToDrag, DragDropEffects allowedEffects,
      Action<DragDropEffects>? onDragDone = null)
    {
      mon.BindDragger(
        LocalDragger.MinimumDrag(SystemParameters.MinimumVerticalDragDistance,
        LocalDragger.MaxMoves(1, LocalDragger.Action((_, __) =>
      {
        var ret = mon.InitiateDrag(dataToDrag(), allowedEffects);
        onDragDone?.Invoke(ret);
      }))));
    }
  }
} 