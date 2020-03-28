using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.Graphics.Gutters;
using Melville.Linq.Statistics.Graphics.Internal;

namespace Testr.Graphics.Internal
{
  public sealed class TestSurface:ILowLevelGraphSurface
  {
    private readonly StringBuilder output = new StringBuilder();
    #region formatters

    private string StringTuple(params object[] items) =>
      string.Concat("(", string.Join(",", items.Select(Format)), ")");

    private string Format(object o)
    {
      switch (o)
      {
        case null:
          return "<null>";
        case double dbl:
          return dbl.ToString("##########0.0");
        case Pen pen:
          return StringTuple("pen", pen.Thickness, pen.Brush);
        case SolidColorBrush brush:
          return StringTuple("solidBrush", brush.Color);
        case Brush brush:
          return "<Non SolidColor Brush>";
        case Point pt:
          return StringTuple(pt.X, pt.Y);
        case string str:
          return str;
        case GutterText gutterText:
          return StringTuple("GutterText", gutterText.Text, gutterText.Rotation, gutterText.TextSize);
        case IEnumerable enumerable:
          return StringTuple(enumerable.Cast<object>().Prepend("Array").ToArray());
        default:
          return o.ToString();
      }
    }

    #endregion

    public void Line(double x1, double y1, double x2, double y2, Pen pen) =>
      output.AppendLine(StringTuple("Line", x1, y1, x2, y2, Format(pen)));

    public void Ellipse(double x, double y, double xRadius, double yRadius, Brush brush, Pen pen) =>
      output.AppendLine(StringTuple("ellipse", x, y, xRadius, yRadius, Format(brush), Format(pen)));

    public void RegularPolygon(double x, double y, int sides, int revolutions, Brush brush, Pen pen, double radius,
      double initialAngle) =>
      output.AppendLine(
        StringTuple("RegPoly", x, y, sides, revolutions, Format(brush), Format(pen), radius));

    public void Polygon(IEnumerable<Point> points, Pen pen, Brush brush, bool closed = true)
    {
      output.AppendLine(StringTuple("Polygon", pen, brush, closed, points));
    }

    public void PolyLine(IEnumerable<Point> points, Pen pen)
    {
      output.AppendLine(StringTuple("PolyLine", pen, points));
    }

    public void Rectangle(double x1, double y1, double x2, double y2, Pen pen, Brush brush) =>
      output.AppendLine(StringTuple("Rect", pen, brush, x1, y1, x2, y2));

    public void Text(FormattedText text, double x, double y, double angle) =>
      output.AppendLine(StringTuple("Text", text.Text, x, y));

    public override string ToString() => output.ToString();

    public void HandleGutter(Gutter gutter)
    {
      foreach (var item in gutter.Items)
      {
        output.AppendLine(StringTuple(item));
      }
    }
  }
}