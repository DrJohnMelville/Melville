using Melville.Linq.Statistics.DescriptiveStats;
using Xunit;

namespace Test.DescriptiveStats
{
  public class MapperTest
  {
    [Fact]
    public void SimpleMapping()
    {
      var mapper = new Mapper<int>();
      mapper.SetMapping(1,10,11,12,13,14,15);
      Assert.Equal(1,mapper.Map(12));
      Assert.Equal(22,mapper.Map(22));
      mapper.SetMapping(2, 21, 22, 23);
      Assert.Equal(2,mapper.Map(22));
    }
  }
}