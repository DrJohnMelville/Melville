using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Melville.Linq.Statistics.Functional;
using Melville.Linq.Statistics.Graphics;
using Melville.Linq.Statistics.Graphics.Internal;
using Melville.Linq.Statistics.HypothesisTesting;
using Test.TestStringDB;
using Testr.Graphics.Internal;
using Xunit;

namespace Test.Graphics.Internal
{
  public sealed class GraphAreaData : StringTestDatabase
  {
    public GraphAreaData() : base()
    {
    }
  }

  public sealed class GraphAreaTest : IClassFixture<GraphAreaData>
  {
    private readonly GraphAreaData data;

    public GraphAreaTest(GraphAreaData data)
    {
      this.data = data;
    }

    private string RenderToString(object area) => InnerRenderToString(((IDataSerries)area).GraphHost as Graph);


    private static string InnerRenderToString(Graph parent)
    {
      var inner = new TestSurface();
      parent.SetActualExtent(600, 200);
      parent.AssignColors();
      parent.Render(inner);
      inner.HandleGutter(parent.BottomGutter);
      return inner.ToString();
    }

    [Fact]
    public void IncludeValue()
    {
      var graphArea = new Graph().Scatter(Enumerable.Range(1, 10), i => i, i => i * i).XAxis.IncludeValues(-1, 15);
      data.AssertDatabase(RenderToString(graphArea));
    }
    [Fact]
    public void SimpleScatter()
    {
      var graphArea = new Graph().Scatter(Enumerable.Range(1, 10), i => i, i => i * i);
      data.AssertDatabase(RenderToString(graphArea));
    }

    [Fact]
    public void SimpleLine()
    {
      var graphArea = new Graph().Line(Enumerable.Range(-5, 11), i => i, i => i * i)
        .Line(Enumerable.Range(-5, 11), i => i, i => i);

      data.AssertDatabase(RenderToString(graphArea));
    }

    [Fact]
    public void LabelSize()
    {
      var graphArea = new Graph().Line(Enumerable.Range(-5, 11), i => i, i => i * i)
        .XAxis.LabelSize(20);

      data.AssertDatabase(RenderToString(graphArea));
    }

    [Fact]
    public void InvertLine()
    {
      var graphArea = new Graph().Line(Enumerable.Range(-5, 11), i => i, i => i * i)
        .Line(Enumerable.Range(-5, 11), i => i, i => i).XAxis.Invert();

      data.AssertDatabase(RenderToString(graphArea));
    }

    [Fact]
    public void LogAxisTest()
    {
      var graph = new Graph().Line(Enumerable.Range(1, 10), i => i, i => i * i).XAxis.Log();

      var axis = graph.XAxis.InnerAxis;
      axis.ScaledValue(1);
      axis.ScaledValue(10);
      axis.TargetRange = 100;
      var labels = axis.Labels().AsList();
      var positions = labels.Select(i => axis.ScaledValue(i.Value)).AsList();
    }

    [Fact]
    public void SimpleBar()
    {

      var graphArea = new Graph().Bar(Enumerable.Range(1, 10), i => i).Bar(i => i / 2, _ => "Label them all the same")
        .WithTextAngle(45)
        .WithTextSize(18).
        WithBrush(i => new SolidColorBrush(new Color() { A = 255, R = (byte)(i * 20) }));

      data.AssertDatabase(RenderToString(graphArea));
    }

    [Fact]
    public void ChainLineBug()
    {
      
      var graphArea = new Graph().Scatter(Enumerable.Range(1, 10), i => i, i => i * i).Line(i => i, i => i);
      data.AssertDatabase(RenderToString(graphArea));
    }

    [Fact]
    public void BarWithoutLabel()
    {
      var graphArea = new Graph().Bar(Enumerable.Range(1, 10), i => i, i => i.ToString() + " label").WithTextAngle(45);

      data.AssertDatabase(RenderToString(graphArea));
    }

    [Fact]
    public void BoxPlot()
    {
      var B1 = new[] { 1.26, 0.34, 0.70, 1.75, 50.57, 1.55, 0.08, 0.42, 0.50, 3.20, 0.15, 0.49, 0.95, 0.24, 1.37, 0.17, 6.98, 0.10, 0.94, 0.38 };
      var B2 = new[] { 2.37, 2.16, 14.82, 1.73, 41.04, 0.23, 1.32, 2.91, 39.41, 39.41, 39.41, 0.11, 27.44, 4.51, 0.51, 4.50, 0.18, 14.68, 4.66, 1.30, 2.06, 1.19 };
      var elt = B1.Select(i => new { Name = "B1", Value = i }).Concat(B2.Select(i => new { Name = "B2", Value = i }));
      var graphArea = new Graph().Box(elt, i => i.Value, i => i.Name);
      
      data.AssertDatabase(RenderToString(graphArea));
    }

    [Fact]
    public void TwoSequenceWithdifferentColors()
    {
      var graphArea = new Graph()
        .Scatter(Enumerable.Range(1, 10), i => i, i => i * i)
        .Scatter(i => i, i => -i);
      data.AssertDatabase(RenderToString(graphArea));
    }

    [Fact]
    public void CurveFitting()
    {
      var dat = Enumerable.Range(-10, 21).ToList();
      Func<int, double> f = i => 3 * i * i * i;
      var graphArea = new Graph()
        .Regression(dat, i => i, f, 3);
      data.AssertDatabase(RenderToString(graphArea));
    }



    [Fact]
    public void LineFitting()
    {
      var dat = Enumerable.Range(-10, 21).ToList();
      Func<int, double> f = i => 3 * i * i * i;
      var graphArea = new Graph()
        .Regression(dat, i => i, f, 1);
      data.AssertDatabase(RenderToString(graphArea));
    }
    [Fact]
    public void LogLog()
    {
      var basePlot = new Graph("Log Log of high order polynomials")
        .Line(Enumerable.Range(1, 100), i => i, i => i * i * i);
      var both = basePlot.BothAxes;
      var graphArea = both.Log()
        .Line(i => i, i => i * i).Line(i => i, i => i).Line(i => i, i => Math.Sqrt(i));
      data.AssertDatabase(RenderToString(graphArea));
    }
    [Fact]
    public void IgnoresInvalidEntries()
    {
      var graphArea = new Graph("1/x ffunc").Line(Enumerable.Range(-100, 201), i => i, i => 10.0 / i);
      data.AssertDatabase(RenderToString(graphArea));
    }
   [Fact]
    public void Has2YAxes()
   {
     var graphArea = new Graph("1/x ffunc").Line(Enumerable.Range(1, 201), i => i, i => i).YAxis.NewAxis()
       .Line(i => i, i => i * i * i);
      data.AssertDatabase(RenderToString(graphArea));
    }
   [Fact]
    public void GridLines()
   {
     var graphArea = new Graph("1/x ffunc").Line(Enumerable.Range(-10, 21), i => i, i => i).YAxis.NewAxis()
       .Line(i => i, i => i * i + 1).YAxis.Log().BothAxes.WithGrid();
      data.AssertDatabase(RenderToString(graphArea));
    }
   [Fact]
    public void Simplehistogram()
   {
     var graphArea = Enumerable.Range(1, 9).SelectMany(i => Enumerable.Repeat(i, i)).Graph().Histogram(i => i);
      data.AssertDatabase(RenderToString(graphArea));
    }

    [Theory]
    [InlineData(-1,-1, 6)]
    [InlineData(-1,1, 8)]
    [InlineData(5,-1, 5)]
    [InlineData(7,1, 7)]
    public void CreateHistogram(int numberOfBins, double width, int answer)
    {
      var graphArea = Enumerable.Range(1, 9).SelectMany(i => Enumerable.Repeat(i, i)).Graph().Histogram(i => i)
        .WithBinCount(numberOfBins).WithBinWidth(width);
      var render = RenderToString(graphArea);
      var binCount = Regex.Matches(render, "Rect").Count;
      Assert.Equal(answer, binCount);
    }
    [Fact]
    public void SideBySide()
   {
     var graphArea = Enumerable.Range(1, 10).Graph("1/x ffunc").XAxis
       .SideBySide(i => i.Line(j => j, j => j), i => i.Line(j => j, j => j * j));
     var stringRendering = RenderToString(graphArea);
     data.AssertDatabase(stringRendering);
    }


  }
}