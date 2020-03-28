using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Statistics.Visualizations;

namespace Melville.Linq.Statistics.Graphics.Internal
{
  public class HistogramPlot<T> : YDataSerries<T, HistogramPlot<T>>
  {
    public HistogramPlot(IEnumerable<T> data, Func<T, double> yFunc) : base(data, yFunc)
    {
    }

    public override void Render(GraphSurface surface)
    {
      var histo = ComputeHistogram();
      if (showContinuious)
      {
        RennderContinuious(surface, histo);
      }
      else
      {
        RennderDiscrete(surface, histo);
      }
    }

    private void RennderContinuious(GraphSurface surface, Histogram histo)
    {
      IEnumerable<(GraphValue X, GraphValue Y)> Vertices()
      {
        yield return (new ScaledValue(histo.Bins.First().Range.Min), new ScaledValue(0));
        foreach (var bin in histo.Bins)
        {
          yield return (new ScaledValue((bin.Range.Min + bin.Range.Min) / 2), new ScaledValue(bin.Value));
        }
        yield return (new ScaledValue(histo.Bins.Last().Range.Max), new ScaledValue(0));
      }

      var leader = Data.First();
      surface.Polygon(Vertices(), Pen(leader), Brush(leader));
    }

    private void RennderDiscrete(GraphSurface surface, Histogram histo)
    {
      foreach (var bin in histo.Bins)
      {
        var leader = Data.FirstOrDefault(i => bin.Contains(YFunc(i)));
        surface.Rectangle(new ScaledValue(bin.Range.Min) + 1.0, new ScaledValue(0), new ScaledValue(bin.Range.Max) - 1.0,
          new ScaledValue(bin.Value),
          Pen(leader), Brush(leader));
      }
    }

    #region Compute Histogram
    private Histogram ComputeHistogram()
    {
      var histo = new Histogram();
      var innerData = Data.Select(YFunc).ToArray();
      if (binCount > 0)
      {
        if (binWidth > 0)
        {
          histo.Compute(innerData, binCount, binWidth);
        }
        else
        {
          histo.Compute(innerData, binCount);
        }
      }
      else // no bin count
      {
          if (binWidth > 0)
          {
            histo.Compute(innerData, binWidth);
          }
          else
          {
            histo.Compute(innerData);
          }
        }
        return histo;
    }

    private int binCount;

    public HistogramPlot<T> WithBinCount(int binCount)
    {
      this.binCount = binCount;
      return this;
    }

    private double binWidth = -1;

    public HistogramPlot<T> WithBinWidth(double binWidth)
    {
      this.binWidth = binWidth;
      return this;
    }

    private bool showContinuious;

    public HistogramPlot<T> AsContinuious(bool continuious = true)
    {
      showContinuious = continuious;
      return this;
    }
    #endregion
  }
}