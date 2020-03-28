using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Accord.Statistics.Models.Regression.Linear;
using Melville.Linq.Statistics.DescriptiveStats;

namespace Melville.Linq.Statistics.Graphics.Internal
{
  public sealed class RegreesionPlot<T> : ScatterPlotBase<T, RegreesionPlot<T>>
  {
    public PolynomialRegression Equation { get; }

    public RegreesionPlot(IEnumerable<T> data, Func<T, double> xValue, Func<T, double> yValue, int order) :
      base(data, xValue, yValue)
    {
      WithGlyph(Glyphs.Hide);
      Equation = Data.Fit(xValue, yValue, order);
    }

    public override void Render(GraphSurface surface)
    {
      if (!Data.Any()) return;
      base.Render(surface);
      surface.Polynomial(Equation, Pen(Data.First()));
    }

  }

  public class ScatterPlot<T> : ScatterPlotBase<T, ScatterPlot<T>>
  {
    public ScatterPlot(IEnumerable<T> data, Func<T, double> xValue, Func<T, double> yValue) :
      base(data, xValue, yValue)
    {
      Pen = _ => null;
    }
  }
  public class ScatterPlotBase<T, TReturn> : XYDataSerries<T, TReturn>
    where TReturn : ScatterPlotBase<T, TReturn>
  {
    private Func<T, double> radius;

    // item, desstination, brush, pen, radius, x, y
    protected Func<T, Action<ILowLevelGraphSurface, Brush, Pen, double, double, double>> Glyph { get; set; }

    public ScatterPlotBase(IEnumerable<T> data, Func<T, double> xValue, Func<T, double> yValue) : 
      base(data, xValue, yValue)
    {
      this.radius = _ => 1;
      this.Glyph = _ => Glyphs.Circle;
    }

    #region AttributeSetters

    public TReturn WithRadius(double radius) => WithRadius(_ => radius);
    public TReturn WithRadius(Func<T, double> radius)
    {
      this.radius = radius;
      return (TReturn) this;
    }    
    public TReturn WithGlyph(Action<ILowLevelGraphSurface, Brush, Pen, double, double, double> glyph) => WithGlyph(_ => glyph);
    public TReturn WithGlyph(Func<T, Action<ILowLevelGraphSurface, Brush, Pen, double, double, double>> glyph)
    {
      Glyph = glyph;
      return (TReturn) this;
    }    

    #endregion

    public override void Render(GraphSurface surface)
    {
      foreach (var datum in Data)
      {
        surface.DrawGlyph(Brush(datum), Pen(datum), radius(datum),
          new ScaledValue(XFunc(datum)), new ScaledValue(YFunc(datum)),
          Glyph(datum));
      }
    }
  }
}