using  System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Melville.WpfControls.PieCompletionControl;

public sealed class PieProgressControl : Control
{
  public double Completed
  {
    get { return (double) GetValue(CompletedProperty); }
    set { SetValue(CompletedProperty, value); }
  }

  // Using a DependencyProperty as the backing store for Completed.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty CompletedProperty =
    DependencyProperty.Register("Completed", typeof(double), typeof(PieProgressControl),
      new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));
  public double Total
  {
    get { return (double) GetValue(TotalProperty); }
    set { SetValue(TotalProperty, value); }
  }

  // Using a DependencyProperty as the backing store for TotaL.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty TotalProperty =
    DependencyProperty.Register("Total", typeof(double), typeof(PieProgressControl),
      new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

  protected override void OnRender(DrawingContext drawingContext)
  {
    base.OnRender(drawingContext);
    var width = ActualWidth;
    var height = ActualHeight;
    drawingContext.DrawEllipse(Total > Completed ? Background : Foreground, null,
      new Point(width / 2, height / 2), width / 2, height / 2);

    if (Total == 0.0) return;
    if (Total <= Completed) return;

    double angle = 2 * Math.PI * Completed / Total;
    Point piePoint = new Point((width / 2) * (1 + Math.Cos(angle)), (height / 2) * (1 - Math.Sin(angle)));
    bool isLargeArc = angle > Math.PI;

    drawingContext.DrawGeometry(Foreground, null, new PathGeometry(new PathFigure[]
    {
      new PathFigure(new Point(width / 2, height / 2), new PathSegment[]
      {
        new LineSegment(new Point(width, height / 2), true),
        new ArcSegment(piePoint, new Size(width / 2, height / 2), 0.0, isLargeArc,
          SweepDirection.Counterclockwise,false)
      }, true),
    }));
  }
}