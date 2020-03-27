using System;
using System.Collections.Generic;
using Melville.Linq.Statistics.Graphics.Gutters;
using Melville.Linq.Statistics.Graphics.Internal;
using Melville.Linq.Statistics.Graphics.Internal.Axes;

namespace Melville.Linq.Statistics.Graphics
{
  public class AxisLabel<T> : DataSerries<T, AxisLabel<T>>
  {
    private readonly string text;
    private readonly int? labelSize;
    private readonly int? labelRotation;
    private readonly Axis axis;
    public AxisLabel(IEnumerable<T> data, Axis axis, string text, int? labelSize = null,
      int? labelRotation = null) : base(data)
    {
      this.text = text;
      this.labelSize = labelSize;
      this.labelRotation = labelRotation;
      this.axis = axis;
      Pen = _ => null;
      Brush = _ => null;
      gutterBand = axis.NextGutterBand();
    }


    private int gutterBand;

    public override void Render(GraphSurface surface)
    {
      axis.RenderTitle(text, gutterBand, labelSize, labelRotation);
    }
  }
}