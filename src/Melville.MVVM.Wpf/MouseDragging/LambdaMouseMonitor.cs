using  System;
using System.Windows;

namespace Melville.MVVM.Wpf.MouseDragging
{
  public class LambdaMouseMonitor: IMouseDataSource
  {
    private IMouseDataSource other;
    public static LambdaMouseMonitor FromFull(IMouseDataSource other, Func<LocalDragEventArgs, Point> lambda)=> 
      new LambdaMouseMonitor(other, lambda);
    private LambdaMouseMonitor(IMouseDataSource other, Func<LocalDragEventArgs, Point> lambda)
    {
      this.other = other;
      other.MouseMoved += (s, e) =>
      {
        e.TransformedPoint = lambda(e);
        MouseMoved?.Invoke(s, e);
      };
    }
    public static LambdaMouseMonitor FromPoint(IMouseDataSource other, Func<Point, Point> lambda) => 
      new LambdaMouseMonitor(other, lambda);
    private LambdaMouseMonitor(IMouseDataSource other, Func<Point, Point> lambda)
    {
      this.other = other;
      other.MouseMoved += (s, e) =>
      {
        e.TransformedPoint = lambda(e.TransformedPoint);
        MouseMoved?.Invoke(s, e);
      };

    }
    public event EventHandler<LocalDragEventArgs>? MouseMoved;
    public IMouseDataRoot Root => other.Root;
  }
}