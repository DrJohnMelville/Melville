using System.Linq;
using Melville.Linq.Statistics.DescriptiveStats;
using Xunit;

namespace Test.DescriptiveStats
{
  public sealed class DescriptiveStatsImplementationTest
  {
    #region Medin

    [Fact]
    public void MedianTestOdd()
    {
      Assert.Equal(5, new[] {1, 1, 5, 10, 10}.Median());
      Assert.Equal(5, new[] {10, 1, 5, 10, 1}.Median());
      Assert.Equal(5, new[] {10, 1, 5, 10, 1}.Median(i => i * 4));
    }

    [Fact]
    public void MedianOf1()
    {
      Assert.Equal(1, new[] {1}.Median());

    }

    [Fact]
    public void MedianOf0()
    {
      Assert.Equal(0,  new int[0].Median());
    }

    [Fact]
    public void SampleStandardDev()
    {
      var result = new double[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}.MeanAndStandardDeviation();
      Assert.Equal(5.5, result.Mean, 3);
      Assert.Equal(3.0276, result.StdDev, 3);
    }

    [Fact]
    public void CheckCentiles()
    {
      for (int i = 0; i < 10; i++)
      {
        Assert.Equal(i, Enumerable.Range(0,10).Centile(0.02 +i/10.0));
      }
    }

    [Fact]
    public void PopulationStandardDev()
    {
      var result = new double[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}.MeanAndStandardDeviation(true);
      Assert.Equal(5.5, result.Mean, 3);
      Assert.Equal(2.8722, result.StdDev, 3);
      Assert.Equal(2.8722, new double[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}.StandardDeviation(true), 3);
    }

    [Fact]
    public void SampleStandardDevInt()
    {
      var result = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}.MeanAndStandardDeviation();
      Assert.Equal(5.5, result.Mean, 3);
      Assert.Equal(3.0276, result.StdDev, 3);
    }

    [Fact]
    public void PopulationStandardDevInt()
    {
      var result = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}.MeanAndStandardDeviation(true);
      Assert.Equal(5.5, result.Mean, 3);
      Assert.Equal(2.8722, result.StdDev, 3);
    }

    [Fact]
    public void CountAndPercentTest()
    {
      Assert.Equal("3 (33%)", Enumerable.Range(1,9).CountAndPercent(i=>i%3 == 0));
      Assert.Equal("333 (33.3%)", Enumerable.Range(1,999).CountAndPercent(i=>i%3 == 0));
      
    }

    #endregion  

  }
}