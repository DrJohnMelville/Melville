using System;
using System.Collections.Generic;
using System.Linq;

namespace Melville.Linq.Statistics.Graphics.Internal
{


  public sealed class LinePlot<T> : XYDataSerries<T, LinePlot<T>>
  {
    public LinePlot(IEnumerable<T> data, Func<T, double> xValue, Func<T, double> yValue):
      base(data, xValue, yValue)
    {
      WithBrush(_ => null);
    }

    public override void Render(GraphSurface surface)
    {
      surface.PolyLine(Data.Select(i=>(new ScaledValue(XFunc(i)) as GraphValue, 
        new ScaledValue(YFunc(i)) as GraphValue)), Pen(default(T)));
    }

  }
}