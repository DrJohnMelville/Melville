using System;
using System.Collections.Generic;
using Melville.Linq.Statistics.Graphics.Gutters;
using Melville.Linq.Statistics.Graphics.Internal.Axes;

namespace Melville.Linq.Statistics.Graphics.Internal
{
  public class BarPlot<T, TLabel> : BarPlotBase<T, TLabel, BarPlot<T, TLabel>>
  {
    
    public BarPlot(IEnumerable<T> data, Func<T, double> yFunc, Func<T, TLabel> label) : 
      base(data, yFunc, label)
    {
      this.Pen = _ => null;
    }
  }
  public class BarPlotBase<T, TLabel, TReturn> : YDataSerries<T, TReturn>
   where TReturn:BarPlotBase<T, TLabel, TReturn>
  {
    private Func<T, TLabel> label;
    private Func<T, double> size;
    private Func<T, double> angle;

    public BarPlotBase(IEnumerable<T> data, Func<T, double> yFunc, Func<T, TLabel> label) : base(data, yFunc)
    {
      this.size = (_ => 10.0);
      this.angle =(_ => 0.0);
      this.label = label ?? (_ => default(TLabel));
    }

    public override void Render(GraphSurface surface)
    {
      var pos = 0.1;
      foreach (var datum in Data)
      {
        surface.Rectangle(new ScaledValue(pos), new ScaledValue(0), new ScaledValue(pos + 0.9),
          new ScaledValue(YFunc(datum)),
          Pen(datum), Brush(datum));
        AddLabel(surface, datum, new ScaledValue(pos + 0.5));
        pos += 1;
      }
    }

    protected override void SetAxesProtected(Axis xAxis, Axis yAxis)
    {
      base.SetAxesProtected(xAxis, yAxis);
      xAxis.ShowLabels = false;
    }

    protected void AddLabel(GraphSurface surface, T datum, ScaledValue midLine)
    {
      var text = label(datum)?.ToString();
      if (!string.IsNullOrWhiteSpace(text))
      {
        surface.XAxis.Gutter.Add(new GutterText(midLine.FinalFraction(surface.XAxis),
          text, angle(datum), size(datum), int.MaxValue - 1));
      }
    }

    #region SetterMethods 

    public TReturn WithTextAngle(double angle) => WithTextAngle(_ => angle);
    public TReturn WithTextAngle(Func<T, double> angle)
    {
      this.angle = angle;
      return (TReturn)this;
    }
    public TReturn WithTextSize(double size) => WithTextSize(_ => size);
    public TReturn WithTextSize(Func<T, double> size)
    {
      this.size = size;
      return (TReturn)this;
    }
    #endregion
  }
}