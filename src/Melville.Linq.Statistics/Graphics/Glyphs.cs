using System;
using System.Windows.Media;
using Melville.Linq.Statistics.Graphics.Internal;

namespace Melville.Linq.Statistics.Graphics
{
  public static class Pens
  {
    public static readonly Pen Black = new Pen(System.Windows.Media.Brushes.Black, 1);
    public static readonly Pen LightGray = new Pen(System.Windows.Media.Brushes.LightGray, 1);
    public static readonly Pen Blue = new Pen(System.Windows.Media.Brushes.Blue, 1);
    public static readonly Pen Red = new Pen(System.Windows.Media.Brushes.Red, 1);
    public static readonly Pen Null = new Pen(System.Windows.Media.Brushes.Transparent, 0);
  }

  public static class Glyphs
  {
    public static void Diamond(ILowLevelGraphSurface surface, Brush brush, Pen pen, double radius,
      double x, double y) => surface.RegularPolygon(x,y, 4, 1, brush, pen, radius, 0.0);
    public static void Square(ILowLevelGraphSurface surface, Brush brush, Pen pen, double radius,
      double x, double y) => surface.RegularPolygon(x,y, 4, 1, brush, pen, radius, Math.PI/4.0);
    public static void Star(ILowLevelGraphSurface surface, Brush brush, Pen pen, double radius,
      double x, double y) => surface.RegularPolygon(x,y, 5, 2, brush, pen, radius, -Math.PI/2.0);
    public static void Triangle(ILowLevelGraphSurface surface, Brush brush, Pen pen, double radius,
      double x, double y) => surface.RegularPolygon(x,y, 3, 1, brush, pen, radius, -Math.PI/2.0);
    public static void InvertedTriangle(ILowLevelGraphSurface surface, Brush brush, Pen pen, double radius,
      double x, double y) => surface.RegularPolygon(x,y, 3, 1, brush, pen, radius, Math.PI/2.0);
    public static void Circle (ILowLevelGraphSurface surface, Brush brush, Pen pen, double radius, 
      double x, double y) => surface.Ellipse(x,y,radius, radius, brush, pen);

    public static void Hide(ILowLevelGraphSurface surface, Brush brush, Pen pen, double radius,
      double x, double y)
    {
    }
  }
}