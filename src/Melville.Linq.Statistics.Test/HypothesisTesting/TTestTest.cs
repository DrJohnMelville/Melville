using System.Linq;
using Melville.Linq.Statistics.HypothesisTesting;
using Xunit;

namespace Test.HypothesisTesting
{
  public sealed class TTestTest
  {
    [Fact]
    public void OneSampleRaw()
    {
      var result = TTest.OneSample(142.1, 140.5, 10, 30);
      Assert.Equal(-0.87635609, result.T, 4);
      Assert.Equal(29, result.DegreesOfFreedom);
      Assert.Equal(0.80598089, result.OneSidedGreaterP, 6);
      Assert.Equal(0.19401911, result.OneSidedLessP, 6);
      Assert.Equal(0.38803823, result.TwoTailedP, 7);
    }

    [Fact]
    public void OneSampleFromList()
    {
      var result = TTest.OneSample(7, new[] {1.0, 2, 3, 4, 5, 6, 7, 8, 9, 10});
      Assert.Equal(-1.566699, result.T, 4);
    }
    [Fact]
    public void OneSampleFromListFunctional()
    {
      var result = new[] {1.0, 2, 3, 4, 5, 6, 7, 8, 9, 10}.TTest(7);
      Assert.Equal(-1.566699, result.T, 4);
    }

    [Fact]
    public void TwoSampleTest()
    {
      var result = TTest.TwoSample(5.5, 3.0277, 10, 5.97, 3.0782, 10);
      Assert.Equal(-0.3442, result.T, 3);
      Assert.Equal(18, result.DegreesOfFreedom, 2);
    }

    [Fact]
    public void TwoSampleTestFromSamples()
    {
      var result = TTest.TwoSample(new []{1.0,2,3,4,5,6,7,8,9,10},
        new[] { 1.0, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.Select(i=>i+0.5));
      Assert.Equal(-0.3693, result.T, 3);
      Assert.Equal(18, result.DegreesOfFreedom, 2);
    }

    [Fact]
    public void TwoSampleTestFromDelegate()
    {
      var result = Enumerable.Range(0, 200).Select(i => i % 13).TTest(i => i % 2 == 0,
        i=>i);
      Assert.Equal(-0.046, result.T, 3);
      Assert.Equal(197.99105, result.DegreesOfFreedom, 2);
    }

    [Fact]
    public void PairedTTest()
    {
      var stat = TTest.PairedTTest(Enumerable.Range(1, 8), i => i, i => i+(i/10.0));
      Assert.Equal(5.1962, stat.T, 3);
      Assert.Equal(7.0, stat.DegreesOfFreedom, 5);
      
    }
    [Fact]
    public void PairedTTestFunctional()
    {
      var stat = Enumerable.Range(1, 8).Select(i=>(double)i).TTest(i => i, i => i+(i/10.0));
      Assert.Equal(5.1962, stat.T, 3);
      Assert.Equal(7.0, stat.DegreesOfFreedom, 5);
      
    }
  }

  public class ProportionStatisticTest
  {
    [Fact]
    public void DifferenceOfProportionsTest()
    {
      var stat = ProportionStatistics.DifferenceOfProportions(38, 100, 102, 200);
      Assert.Equal(-2.13, stat.ZScore, 2);
      
    }
  }
}