using  System;
using System.Windows;
using Melville.Hacks;

namespace Melville.MVVM.Wpf.MouseDragging
{
  public static class MouseDataSourceOperations
  {
    public static IMouseDataSource Bind(this IMouseDataSource mon, Action<double, double> boundAction)
    {
      mon.MouseMoved += (s, e) => boundAction(e.TransformedPoint.X, e.TransformedPoint.Y);
      return mon;
    }
    public static IMouseDataSource Bind(this IMouseDataSource mon, Action<MouseMessageType,double, double> boundAction)
    {
      mon.MouseMoved += (s, e) => boundAction(e.MessageType, e.TransformedPoint.X, e.TransformedPoint.Y);
      return mon;
    }

    public static void Drag(this IMouseDataSource mon, IDataObject dataToDrag, DragDropEffects allowedEffects,
      Action<DragDropEffects>? onDragDone = null) => Drag(mon, () => dataToDrag, allowedEffects, onDragDone);
    public static void Drag(this IMouseDataSource mon, Func<IDataObject> dataToDrag, DragDropEffects allowedEffects,
      Action<DragDropEffects>? onDragDone = null)
    {
      var once = false;
      mon.MouseMoved += (s, e) =>
      {
        if (e.MessageType != MouseMessageType.Down) return;
        if (once) return;
        once = true;
        var ret = mon.Root.InitiateDrag(dataToDrag(), allowedEffects, e);
          onDragDone?.Invoke(ret);
      };
    }


    public static IMouseDataSource AsDeltas(this IMouseDataSource mon) =>
      LambdaMouseMonitor.FromFull(mon, CreateDeltaFilter());

    public static IMouseDataSource 
      Transform(this IMouseDataSource mon, Func<double, double, (double X, double Y)> func) =>
      LambdaMouseMonitor.FromPoint(mon, p =>
      {
        var ret = func(p.X, p.Y);
        return new Point(ret.X, ret.Y);
      });

    public static IMouseDataSource RelativeToTarget(this IMouseDataSource mon) =>
      LambdaMouseMonitor.FromFull(mon, ea => new Point(ea.TransformedPoint.X / ea.TargetSize.Width,
        ea.TransformedPoint.Y / ea.TargetSize.Height));

    public static IMouseDataSource InvertX(this IMouseDataSource mon) =>
      LambdaMouseMonitor.FromFull(mon, ea => new Point(ea.TargetSize.Width - ea.TransformedPoint.X,
        ea.TransformedPoint.Y));

    public static IMouseDataSource InvertY(this IMouseDataSource mon) =>
      LambdaMouseMonitor.FromFull(mon, ea => new Point(ea.TransformedPoint.X,
        ea.TargetSize.Height - ea.TransformedPoint.Y));

    public static IMouseDataSource TranslateInitialPoint(this IMouseDataSource mon, double x, double y) =>
      LambdaMouseMonitor.FromFull(mon, CreateTranslateFilter(x, y));

    public static IMouseDataSource RestrictToTarget(this IMouseDataSource mon) =>
      LambdaMouseMonitor.FromFull(mon, ea => new Point(
        ea.TransformedPoint.X.Clamp(0, ea.TargetSize.Width),
        ea.TransformedPoint.Y.Clamp(0, ea.TargetSize.Height)));

    public static IMouseDataSource RestrictToRange(this IMouseDataSource mon, double left, double top,
      double right, double bottom) =>
      LambdaMouseMonitor.FromFull(mon, ea => new Point(
        ea.TransformedPoint.X.Clamp(left, right),
        ea.TransformedPoint.Y.Clamp(top, bottom)));

    public static IMouseDataSource RequireInitialDelta(this IMouseDataSource src) =>
      RequireInitialDelta(src, SystemParameters.MinimumHorizontalDragDistance,
        SystemParameters.MinimumVerticalDragDistance);

    public static IMouseDataSource RequireInitialDelta(this IMouseDataSource src, double xTol, double yTol) =>
      new InitialDeltaSource(src, xTol, yTol);

    private static Func<LocalDragEventArgs, Point> CreateTranslateFilter(double x, double y)
    {
      var offset = new Vector();
      return ea =>
      {
        if (ea.MessageType == MouseMessageType.Down)
        {
          offset = new Point(x, y) - ea.TransformedPoint;
        }

        return ea.TransformedPoint + offset;
      };
    }

    private static Func<LocalDragEventArgs, Point> CreateDeltaFilter()
    {
      var oldPoint = new Point();
      return args =>
      {
        if (args.MessageType == MouseMessageType.Down)
        {
          oldPoint = args.TransformedPoint;
        }

        var ret = args.TransformedPoint - oldPoint;
        oldPoint = args.TransformedPoint;
        return (Point) ret;
      };
    }
  }
}