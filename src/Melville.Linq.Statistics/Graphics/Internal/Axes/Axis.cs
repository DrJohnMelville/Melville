using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Melville.Linq.Statistics.DescriptiveStats;
using Melville.Linq.Statistics.Graphics.Gutters;

namespace Melville.Linq.Statistics.Graphics.Internal.Axes
{
  public sealed class XAxis : Axis
  {
    protected override void InnerDrawGrid(GraphSurface surface, GraphValue scaledValue, Pen pen) =>
      surface.Line(scaledValue, new RelativeValue(0), scaledValue, new RelativeValue(1), pen);
  }

  public sealed class YAxis : Axis
  {
    public override double ScaledValue(double relative)
    {
      return TargetRange - base.ScaledValue(relative);
    }

    protected override void InnerDrawGrid(GraphSurface surface, GraphValue scaledValue, Pen pen) =>
      surface.Line(new RelativeValue(0), scaledValue, new RelativeValue(1), scaledValue, pen);
  }

  public class AxisFunction
  {
    public Func<double, Axis, double> Forward { get; }
    public Func<double, Axis, double> Back { get; }

    public AxisFunction(Func<double, Axis, double> forward, Func<double, Axis, double> back)
    {
      Forward = forward;
      Back = back;
    }
  }

  public abstract class Axis
  {
    public Func<double, Pen> GridPen { get; set; }
    public Gutter Gutter { get; set; } = new Gutter();
    public AxisStack Parent { get; set; }
    public bool ShowLabels { get; set; } = true;
    

    public void RegisterOutputFuction(Func<double, Axis, double> forward, Func<double, Axis, double> back) => 
      outputFunctions.Add(new AxisFunction(forward, back));
    public void RegisterInputFuction(Func<double, Axis, double> forward, Func<double, Axis, double> back) => 
      inputFunctions.Add(new AxisFunction(forward, back));

    protected Axis()
    {
      RegisterOutputFuction((i,a)=>MapToTargetRange(i, 
        ApplyInputFuncsForward(a.Minimum), 
        ApplyInputFuncsForward(a.Maximum)),
        (i,a)=>MapFromTargetRange(i, a.Minimum, a.Maximum, a.TargetRange));
    }

    public double Relative(double value) => PlaceOutput(TargetRange * value);


    #region Scaled Values
    public double Minimum { get; private set; } = Double.MaxValue;
    public double Maximum { get; private set; } = double.MinValue;
    private List<AxisFunction> outputFunctions { get; } = new List<AxisFunction>();
    private List<AxisFunction> inputFunctions { get; } = new List<AxisFunction>();
    public virtual double ScaledValue(double relative)
    {
      if (!relative.IsValidAndFinite()) return relative;
      Minimum = Math.Min(relative, Minimum);
      Maximum = Math.Max(relative, Maximum);
      return PlaceOutput(ApplyOutputFuncsForward(ApplyInputFuncsForward(relative)));
    }

    private double ApplyInputFuncsForward(double value) => ApplyFuncs(value, inputFunctions, j => j.Forward);

    private double ApplyInputFuncsReverse(double value) => ApplyFuncs(value, 
      ((IEnumerable<AxisFunction>)inputFunctions).Reverse(), j => j.Back);

    private double ApplyOutputFuncsForward(double value) => ApplyFuncs(value, outputFunctions, j=>j.Forward);

    private double MapToTargetRange(double relative, double minimum, double maximum)
    {
      return ScaleToTargetRange((relative - minimum) / (maximum - minimum));
    }

    private double targetRange;
    public double TargetRange
    {
      get => targetRange;
      set
      {
        targetRange = value;
        effectiveTaargetRange = targetRange * 0.95;
        targetOffset = targetRange * 0.025;

      }
    }
    private double effectiveTaargetRange;
    private double targetOffset;

    private double ScaleToTargetRange(double fraction)
    {
      return (fraction * effectiveTaargetRange) + targetOffset;
    }

    public static double MapFromTargetRange(double screen, double minimum, double maximum, double targetRange)
    {
      return (ScaleFromTargetRange(screen, targetRange) * (maximum - minimum)) + minimum;
    }

    private static double ScaleFromTargetRange(double final, double targetRange) => final / targetRange;

    public double ApplyFuncs(double value, IEnumerable<AxisFunction> funcs, Func<AxisFunction, Func<double, Axis, double>> which)
    {
      var ret = value;
      foreach (var func in funcs)
      {
        ret = which(func)(ret, this);
      }
      return ret;
    }
    public double Invert(double screen)
    {
      return
        UnplaceOutput(ApplyFuncs(ApplyFuncs(screen,
            ((IEnumerable<AxisFunction>)outputFunctions).Reverse(), f => f.Back),
          ((IEnumerable<AxisFunction>)inputFunctions).Reverse(), f => f.Back));
    }


    #endregion

    #region Labels
    public IEnumerable<double> RawColumns()
    {
      var delta = (Maximum - Minimum) / TargetRange;
      for (var d = Minimum; d <= Maximum; d += delta)
      {
        yield return d;
      }
    }

    public IList<(double Value, string Display)> ExplicitLabels { get; set; }
    public bool LogLabels { get; set; }
    public Double LabelSize { get; set; } = 10;
    public Double LabelRotation { get; set; } = 0;
    public IEnumerable<(double Value, string Display)> Labels()
    {
      return ExplicitLabels ?? GenerateLabels();
    }

    private IEnumerable<ValueTuple<double, string>> GenerateLabels()
    {
      var range = Maximum - Minimum;
      var logInterval = Math.Ceiling(Math.Log10(range / 10));
      var labels = InnerGenerateLabel(logInterval).ToList();
      return labels.Count > 4 ? labels : InnerGenerateLabel(logInterval - 1.0);
    }

    private IEnumerable<ValueTuple<double, string>> InnerGenerateLabel(double logInterval)
    {
      var interval = Math.Pow(10.0, logInterval);
      var first = interval * Math.Ceiling(Minimum / interval);
      var displayFormat = LabelFormatString(logInterval);
      for (double i = first; i <= Maximum; i += interval)
      {
        var value = i;
        do
        {
          yield return (value, value.ToString(displayFormat));
          value /= 10;
        } while (LogLabels && value > Minimum);
      }
    }

    private bool ShowLines => GridPen != null;


    public void RenderLabelsAndLines(GraphSurface surface)
    {
      if (!(ShowLabels || ShowLines) ) return;
      foreach (var label in Labels())
      {
        if (ShowLabels)
        {
          var scaledValue = ScaledValue(label.Value)/TargetRange;
          Gutter.Add(new GutterText(scaledValue, label.Display, LabelRotation, LabelSize, int.MaxValue));
        }
        if (ShowLines)
        {
          var pen = GridPen(label.Value);
          InnerDrawGrid(surface, new PrecomputedValue(ScaledValue(label.Value)), pen);
        }
      }
    }

    public void RenderTitle(string text, int titleLevel, int? labelSize = null, 
      int? labelRotation = null)
    {
      if (titleLevel >= 0 && !string.IsNullOrWhiteSpace(text))
      {
        var location = (Relative(0.0) + Relative(1.0)) / (2.0 * targetRange);
        Gutter.Add(new GutterText(location, text, labelRotation??LabelRotation, 
          labelSize??LabelSize, titleLevel));
      }
    }

    protected abstract void InnerDrawGrid(GraphSurface surface, GraphValue scaledValue, Pen pen);

    private string LabelFormatString(double log10Interval)
    {
      var digits = (int)Math.Max(0, -log10Interval);
      return "############0" + (digits < 1 ? string.Empty : ("." + String.Join(string.Empty, Enumerable.Repeat("#", digits))));
    }
    #endregion

    #region OutputPlacement

    private double placementScale = 1.0;
    private double placementOffset = 0.0;
    private double PlaceOutput(double value) => (value * placementScale) + (placementOffset*TargetRange);
    private double UnplaceOutput(double value) => (value - (TargetRange*placementOffset))/placementScale;

    public void SetOutputPlacement(double scale, double offset)
    {
      placementScale = scale;
      placementOffset = offset;
    }

    #endregion

    #region Output title placement

    public int GutterBand { get; set; } = 0;
    public int NextGutterBand() => GutterBand++;

    public void CopyGutterFrom(Axis other)
    {
      Gutter = other.Gutter;
      GutterBand = other.GutterBand;
    }

    #endregion
  }

}