using System.Linq;
using Melville.Linq.Statistics.HypothesisTesting;
using Xunit;

namespace Test.HypothesisTesting
{
  public sealed class RankSumTest
  {

    #region Mann Whitney U

    [Fact]
    public void RankSumTest1()
    {
      var stat = Enumerable.Range(1,10).RankSum(i=>i, i=>i%2 == 1);
      Assert.Equal(15, stat.UMax);
      Assert.Equal(10, stat.UMin);
      Assert.Equal(0.69048, stat.PValue, 5);
      Assert.Equal(5, stat.N1);
      Assert.Equal(5, stat.N2);
      
    }

    [Fact]
    public void RankSumTest2()
    {
      var stat = Enumerable.Range(1,10).RankSum(i=>i%2 == 1?1000*i:i, i=>i%2 == 1);
      Assert.Equal(0.00793650, stat.PValue, 5);
      Assert.Equal(5, stat.N1);
      Assert.Equal(5, stat.N2);
      
    }

    [Fact]
    public void RankSumTestWithNulls()
    {
      var ints = new int?[] {1, 2, 3, 4, 5, null, 6, 7, 8, 9, 10, 11};
      var stat = Enumerable.Range(0,10).RankSum(i=>ints[i], i=>i%2 == 1);
      Assert.Equal(12, stat.UMax);
      Assert.Equal(0.730158730, stat.PValue, 5);
      Assert.Equal(4, stat.N1);
      Assert.Equal(5, stat.N2);
      
    }
    #endregion
  }
}