using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Melville.Linq.Statistics.DescriptiveStats;
using Melville.Linq.Statistics.Functional;

namespace Melville.Linq.Statistics.Graphics.Internal
{
  public class BoxPlot<T,TLabel> : BarPlotBase<T, TLabel, BoxPlot<T, TLabel>> where TLabel:IComparable
  {
    private readonly IList<BoxValues> innerData;
    public BoxPlot(IEnumerable<T> data, Func<T, double> yFunc, Func<T, TLabel> label, bool supressOutliers = false) : base(data, yFunc, label)
    { 
      innerData = Data.GroupBy(label)
        .OrderBy(i => i.Key)
        .Select(i => new BoxValues(i,yFunc, supressOutliers))
        .ToList();
    }

    public override void Render(GraphSurface surface)
    {
      var pos = 0.1;
      surface.XAxis.ScaledValue(0.0); // set the margins right
      foreach (var datum in innerData)
      {
        
        var midline = new ScaledValue(pos + 0.4);
        AddLabel(surface, datum.c50.Item, midline);
        datum.DrawBox(surface, new ScaledValue(pos), midline, new ScaledValue(0.8+pos), Pen, Brush, BackgroundBrush);

        pos += 1;
      }
      surface.XAxis.ScaledValue(pos - 0.1); // set the margins right
    }

    public Func<T, Brush> BackgroundBrush { get; set; } = _ => Brushes.Transparent;
    public BoxPlot<T, TLabel> WithBackgroundBrush(Brush brush) => WithBackgroundBrush(_ => brush);
    public BoxPlot<T, TLabel> WithBackgroundBrush(Func<T, Brush> brush)
    {
      BackgroundBrush= brush;
      return this;
    }



    private class BoxValues
    {
      private readonly SingleBoxValue min ;
      private readonly SingleBoxValue c25 ;
      public readonly SingleBoxValue c50 ;
      private readonly SingleBoxValue c75 ;
      private readonly SingleBoxValue max ;
      private readonly double iqr ;
      private readonly double lowPossibleOutlier ;
      private readonly double lowOutlier ;
      private readonly double highPossibleOutlier ;
      private readonly double highOutlier ;
      private readonly IList<SingleBoxValue> outliers ;


      public BoxValues(IEnumerable<T> data, Func<T, double> yFunc, bool suppressOutliers = false)
      {
        var dataList = data.Select(i=>new SingleBoxValue(i, yFunc(i))).OrderBy(i=>i.Value).AsList();
        min = dataList[0];
        c25 = dataList.OrderedCentile(0.25);
        c50 = dataList.OrderedCentile(0.5);
        c75 = dataList.OrderedCentile(0.75);
        max = dataList[dataList.Count-1];
        iqr = c75.Value - c25.Value;
        lowPossibleOutlier = c25.Value - (1.5 * iqr);
        lowOutlier = c25.Value - (3.0 * iqr);
        highPossibleOutlier = c25.Value + (1.5 * iqr);
        highOutlier = c25.Value + (3.0 * iqr);
        outliers = suppressOutliers ? 
          Array.Empty<SingleBoxValue>():
          dataList.Where(i => i.Value > highPossibleOutlier || i.Value < lowPossibleOutlier).ToList();
        if (outliers.Any(i=>i.Value > c75.Value)) { max = new SingleBoxValue(max.Item, highPossibleOutlier);}
        if (outliers.Any(i=>i.Value < c25.Value)) { min = new SingleBoxValue(min.Item, lowPossibleOutlier);}
      }

      public void DrawBox(GraphSurface surface, ScaledValue left, ScaledValue midline, ScaledValue right,
        Func<T, Pen> pen, Func<T, Brush> brush, Func<T, Brush> backgroundBrush)
      {
        surface.Rectangle(left, c75.Position, right, c25.Position, pen(c75.Item), backgroundBrush(c50.Item));
        void InnerLine(SingleBoxValue value, Pen usePen = null) => surface.Line(left, value.Position, right, value.Position,
          usePen ??pen(value.Item));
        InnerLine(min);
        var lightPen = pen(c50.Item);
        InnerLine(c50, new Pen(lightPen.Brush, lightPen.Thickness * 2));
        InnerLine(max);
        surface.Line(midline, c75.Position, midline, max.Position, pen(max.Item));
        surface.Line(midline, min.Position, midline, c25.Position, pen(min.Item));
        var separator = surface.GetGlyphSeparator();
        foreach (var outlier in outliers)
        {
          separator.DrawGlyph((outlier.Value < highOutlier && outlier.Value > lowOutlier)?null:brush(outlier.Item), pen(outlier.Item), 3, midline, outlier.Position,
            Glyphs.Circle);
        }
        
      }

    }
    public struct SingleBoxValue
    {
      public T Item { get; }
      public double Value => Position.RawValue; 
      public ScaledValue Position { get; }

      public SingleBoxValue(T item, double value)
      {
        Item = item;
        Position = new ScaledValue(value);
      }
    }
  }
}