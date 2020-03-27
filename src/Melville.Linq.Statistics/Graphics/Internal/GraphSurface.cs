using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Accord.Statistics.Models.Regression.Linear;
using Melville.Linq.Statistics.DescriptiveStats;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.Graphics.Gutters;
using Melville.Linq.Statistics.Graphics.Internal.Axes;

namespace Melville.Linq.Statistics.Graphics.Internal
{
  public sealed class GraphSurface
  {
    
    private ILowLevelGraphSurface target;
    public Axis XAxis { get; set; }
    public Axis YAxis { get; set; }

    public GraphSurface(ILowLevelGraphSurface target)
    {
      this.target = target;
    }

    public void SetAxes(Axis xAxis, Axis yAxis)
    {
      XAxis = xAxis;
      YAxis = yAxis;
    }

    public void Line(GraphValue x1, GraphValue y1, GraphValue x2, GraphValue y2, Pen pen)
    {
      target.Line(x1.FinalValue(XAxis), y1.FinalValue(YAxis), x2.FinalValue(XAxis), y2.FinalValue(YAxis), pen);
    }

    public void Polygon(IEnumerable<(GraphValue x, GraphValue y)> points, Pen pen, Brush brush, bool closed = true) => 
      target.Polygon(ConvertPoints(points), pen, brush, closed);

    public void PolyLine(IEnumerable<(GraphValue x, GraphValue y)> points, Pen pen)
    {
      var localPoints = ConvertPoints(points);
      localPoints.Zip(localPoints.Skip(1), (a, b) =>
      {
        target.Line(a.X, a.Y, b.X, b.Y, pen);
        return 1;
      }).AsList();
    }

    private IList<Point> ConvertPoints(IEnumerable<(GraphValue x, GraphValue y)> points)
    {
      var localPoints = points.Select(i => new Point(i.x.FinalValue(XAxis), i.y.FinalValue(YAxis))).AsList();
      return localPoints;
    }

    public void DrawGlyph(Brush brush, Pen pen, double radius, 
      GraphValue x, GraphValue y, Action<ILowLevelGraphSurface, 
      Brush, Pen, double, double, double> glyph)
    {
      glyph(target, brush, pen, radius, x.FinalValue(XAxis), y.FinalValue(YAxis));
    }

    public void Polynomial(PolynomialRegression equation, Pen pen)
    {
      var cols = XAxis.RawColumns().AsList();
      if (cols.Count < 2) return;
      if (equation.Degree < 2)
      {
        Line(new RelativeValue(0), new ScaledValue(equation.Transform(cols[0])),
          new RelativeValue(1), new ScaledValue(equation.Transform(cols.Last())), pen);
      }
      else
      {
        PolyLine(cols.Select<double,(GraphValue, GraphValue)>
          (i=>(new ScaledValue(i), new ScaledValue(equation.Transform(i)))), pen);
      }
    }

    public void Rectangle(GraphValue x1, GraphValue y1, GraphValue x2, GraphValue y2,
      Pen pen, Brush brush)
    {
      target.Rectangle(x1.FinalValue(XAxis), y1.FinalValue(YAxis), x2.FinalValue(XAxis), y2.FinalValue(YAxis), pen,
        brush);
    }

    public void Text(GraphValue x, GraphValue y, string text, double rotation = 0, double size = 10)
    {
      var formattedText = new FormattedText(text, CultureInfo.CurrentUICulture,
        FlowDirection.LeftToRight,
        new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Normal, 
          FontStretches.Normal), size,
        Brushes.Black, 1.0);

      var finalXValue = x.FinalValue(XAxis);
      var finalYValue = y.FinalValue(YAxis);
      target.Text(formattedText, finalXValue, finalYValue, rotation);
    }
    public GlyphSeparator GetGlyphSeparator() => new GlyphSeparator(this);

    public class GlyphSeparator
    {
      public IList<Rect> glyphs = new List<Rect>();
      public GraphSurface surface;

      public GlyphSeparator(GraphSurface surface)
      {
        this.surface = surface;
      }

      private double delta = 1.0;

      public void DrawGlyph(Brush brush, Pen pen, double radius,
        GraphValue x, GraphValue y, Action<ILowLevelGraphSurface,
          Brush, Pen, double, double, double> glyph)
      {
        var centerX = x.FinalValue(surface.XAxis);
        var centerY = y.FinalValue(surface.YAxis);
        if (HasConflict(centerX, centerY, radius))
        {
          do
          {
            centerX += delta;
          } while (HasConflict(centerX, centerY, radius));
          delta *= -1;
        }
        glyph(surface.target, brush, pen, radius, centerX, centerY);
      }

      private bool HasConflict(double centerX, double centerY, double radius)
      {
        var rect = new Rect(centerX - radius, centerY-radius, 2*radius, 2* radius);
        if (glyphs.Any(i => i.IntersectsWith(rect))) return true;
        // otherwise remember this spot as used.
        glyphs.Add(rect);
        return false;
      }
    }
  }
}