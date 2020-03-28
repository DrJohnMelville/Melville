using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Media;
using Melville.Linq.Statistics.Functional;
using OfficeOpenXml.Drawing.Chart;

namespace Melville.Linq.Statistics.Graphics.Internal
{
  public interface ILowLevelGraphSurface
  {
    void Line(double x1, double y1, double x2, double y2, Pen pen);
    void Ellipse(double x, double y, double xRadius, double yRadius, Brush brush, Pen pen);
    void RegularPolygon(double x, double y, int sides, int revolutions, Brush brush, Pen pen, 
      double radius, double initialAngle);
    void PolyLine(IEnumerable<Point> points, Pen Pen);
    void Polygon(IEnumerable<Point> points, Pen pen, Brush brush, bool closed = true);
    void Rectangle(double x1, double y1, double x2, double y2, Pen pen, Brush brush);
    void Text(FormattedText text, double x, double y, double angle);
  }

  public class LowLevelGraphSurface : ILowLevelGraphSurface
  {
    private readonly DrawingContext context;
    public LowLevelGraphSurface(DrawingContext context)
    {
      this.context = context;
    }
    
    public void Line(double x1, double y1, double x2, double y2, Pen pen) =>
      context.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));

    public void Ellipse(double x, double y, double xRadius, double yRadius, Brush brush, Pen pen)=>
      context.DrawEllipse(brush, pen, new Point(x, y), xRadius, yRadius);

    public void RegularPolygon(double x, double y, int sides, int revolutions, Brush brush, Pen pen, double radius,
      double initialAngle) => context.DrawDrawing(new GeometryDrawing(brush,pen, new PathGeometry(
      new [] {RegularPolygon(sides, revolutions, radius, initialAngle)}, 
      FillRule.Nonzero, new TranslateTransform(x,y))));

    private PathFigure RegularPolygon(int sides, int revolutions, double radius, double initialAngle)
    {
      Point Point(double angle) => new Point(radius*Math.Cos(angle), radius*Math.Sin(angle));
      var delta = 2 * Math.PI * revolutions / sides;

      return new PathFigure(Point(initialAngle),
        Enumerable.Range(1,sides).Select(i=>new LineSegment(Point(initialAngle+(i*delta)),true)),true);
    }

    public void PolyLine(IEnumerable<Point> points, Pen pen) => Polygon(points, pen, null, false);
    
    public void Polygon(IEnumerable<Point> points, Pen pen, Brush brush, bool closed = true)
    {
      var pointList = points.AsList();
      if (pointList.Count < 2) return;
      context.DrawDrawing(new GeometryDrawing(brush, pen, new PathGeometry(new[]{new PathFigure(
        pointList.First(), pointList.Skip(1).Select(i=>new LineSegment(i, true)), closed)})));
    }

    public void Rectangle(double x1, double y1, double x2, double y2, Pen pen, Brush brush)
    {
      context.DrawRectangle(brush, pen, new Rect(new Point(x1,y1), new Point(x2,y2)));
    }

    public void Text(FormattedText text, double x, double y, double angle)
    {
      context.PushTransform(new RotateTransform(angle, x, y));
      context.DrawText(text, new Point(x,y));
      context.Pop();
    }
  }

  public sealed class VoidSurface: ILowLevelGraphSurface
  {
    public void Line(double x1, double y1, double x2, double y2, Pen pen)
    {
    }

    public void Ellipse(double x, double y, double xRadius, double yRadius, Brush brush, Pen pen)
    {
    }

    public void RegularPolygon(double x, double y, int sides, int revolutions, Brush brush, Pen pen, double radius,
      double initialAngle)
    {
    }

    public void Polygon(IEnumerable<Point> points, Pen pen, Brush brush, bool closed = true)
    {
      var list = points.AsList(); // make sure we enumerate the list so to all gets evaluated
    }

    public void PolyLine(IEnumerable<Point> points, Pen Pen)
    {
      var list = points.AsList(); // make sure we enumerate the list so to all gets evaluated
    }

    public void Rectangle(double x1, double y1, double x2, double y2, Pen pen, Brush brush)
    {
    }

    public void Text(FormattedText text, double x, double y, double angle)
    {
    }
  }
}