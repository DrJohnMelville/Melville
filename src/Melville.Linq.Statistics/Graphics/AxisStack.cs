using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Linq.Statistics.Graphics.Internal.Axes;

namespace Melville.Linq.Statistics.Graphics
{
  public sealed class AxisStack
  {
    public List<Axis> Axes = new List<Axis>();
    private readonly Func<Axis> factory;

    public AxisStack(Func<Axis> factory)
    {
      this.factory = factory;
      NewAxis();
    }

    public Axis NewAxis()
    {
      var axis = factory();
      axis.Parent = this;
      Axes.Add(axis);
      return axis;
    }

    public Axis NewAxisShareGutter()
    {
      var axis = NewAxis();
      axis.CopyGutterFrom(Axes[Axes.Count - 2]);
      return axis;
    }

    public void PopAxis() => Axes.RemoveAt(Axes.Count - 1);

    public Axis Current => Axes.Last();
  }
}