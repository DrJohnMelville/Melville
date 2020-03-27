using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.Graphics.Internal.Axes;
using Xunit;

namespace Test.Graphics.Internal.Axes
{
  public sealed class AxisTest
  {
    [Theory]
    [InlineData(10, 0.7, 7)]
    [InlineData(100, 0.7, 70)]
    public void RelativeTest(double target, double prop, double result)
    {
      Assert.Equal(result, new XAxis {TargetRange = target}.Relative(prop), 3);
    }
    [Theory]
    [InlineData(100, 15, 50)]
    public void ScaleTest(double target, double prop, double result)
    {
      var axis = new XAxis {TargetRange = target};
      axis.ScaledValue(10);
      axis.ScaledValue(20);
      Assert.Equal(result, axis.ScaledValue(prop), 3);
      Assert.Equal(prop, axis.Invert(result),3);
    }

    private void Equal(IEnumerable<double> expected, IEnumerable<Double> actual, int places)
    {
      var el = expected.AsList();
      var al = actual.AsList();
      Assert.Equal(el.Count, al.Count);
      for (int i = 0; i < al.Count; i++)
      {
        Assert.Equal(el[i], al[i], places);
      }
    }

    [Fact]
    public void SimleAxisLabels()
    {
      var axis = new XAxis();
      axis.ScaledValue(0.7);
      axis.ScaledValue(11);
      var result = axis.Labels().Select(i=>i.Value).ToArray();
      Assert.Equal(new[] {1,2,3,4,5,6,7,8,9,10.0, 11}, result);

    }

    [Fact]
    public void SimleAxisLabels10()
    {
      var axis = new XAxis();
      axis.ScaledValue(0);
      axis.ScaledValue(100);
      Assert.Equal(Enumerable.Range(0,11).Select(i=>i*10.0), axis.Labels().Select(i => i.Value));
      Assert.Equal(Enumerable.Range(0,11).Select(i=>(i*10.0).ToString("####0")), axis.Labels().Select(i => i.Display));
    }    
    [Fact]
    public void AxplicitAxisLabel()
    {
      var axis = new XAxis();
      axis.ScaledValue(0);
      axis.ScaledValue(100);
      axis.ExplicitLabels = new List<(double Value, string Display)>();
      axis.ExplicitLabels.Add((3.14, "PI"));
      Assert.Single(axis.Labels());
      Assert.Equal("PI", axis.Labels().First().Display);
    }    
    [Fact]
    public void SimleAxisLabelsOneTenth()
    {
      var axis = new XAxis();
      axis.ScaledValue(0);
      axis.ScaledValue(1);
      var labels = axis.Labels().ToArray();
      Equal(Enumerable.Range(0,11).Select(i=>i/10.0), labels.Select(i => i.Value), 3);
      Assert.Equal(Enumerable.Range(0, 11).Select(i => (i / 10.0).ToString("####0.#")), axis.Labels().Select(i => i.Display));
    }
    [Fact]
    public void SimleAxisLabelsOnehundredth()
    {
      var axis = new XAxis();
      axis.ScaledValue(0);
      axis.ScaledValue(0.1);
      var labels = axis.Labels().ToArray();
      Equal(Enumerable.Range(0,11).Select(i=>i/100.0), labels.Select(i => i.Value), 3);
      Assert.Equal(Enumerable.Range(0, 11).Select(i => (i / 100.0).ToString("####0.##")), axis.Labels().Select(i => i.Display));
    }
  }
}     