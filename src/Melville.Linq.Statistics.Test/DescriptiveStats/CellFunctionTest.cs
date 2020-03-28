using System.Linq;
using Melville.Linq.Statistics.Tables;
using Xunit;

namespace Test.DescriptiveStats
{
  public class CellFunctionTest
  {
    [Theory]
    [InlineData(1, 2, "50% (1/2)")]
    [InlineData(10, 101, "9.9% (10/101)")] // make sure we do real, not integer, division
    [InlineData(100, 200, "50.0% (100/200)")]
    [InlineData(1,0, "1/0")]
    public void PercentAndFraction(int num, int denom, string result)
    {
      Assert.Equal(result, CellFunctions.PercentAndFraction(num, denom));
 
    }

    [Fact]
    public void PercentAndFractionByCollections()
    {
      Assert.Equal("13.7% (32/234)", CellFunctions.PercentAndFraction(new int[32], new int[234]));
    }

    [Fact]
    public void PercentAndFractionByPredicate()
    {
      Assert.Equal("33% (33/99)", 
        Enumerable.Range(1,99).PercentAndFraction(i=> i % 3 ==0));
    }
  }
}