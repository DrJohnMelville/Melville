using System.Linq;
using Melville.Linq.Statistics.DescriptiveStats;
using Melville.Linq.Statistics.Functional;
using Xunit;

namespace Test.Functional
{
  public class FunctionalMethodsTest
  {
    [Fact]
    public void SimpleMap()
    {
      Assert.Equal("One", FunctionalMethods.Map(1, (1,"One"), (2, "Two")) );
      Assert.Equal("One", FunctionalMethods.Map(1, "Foo", (1,"One"), (2, "Two")) );
    }
    [Fact]
    public void SimpleMapDefault()
    {
      Assert.Equal("Foo", FunctionalMethods.Map(4, "Foo", (1,"One"), (2, "Two")) );
    }
    [Fact]
    public void MapNull()
    {
      Assert.Equal("Three", FunctionalMethods.Map<int?,string>(null, (1 as int?,"One"), (2, "Two"), (null, "Three")) );
      Assert.Equal("Three", FunctionalMethods.Map<int?,string>(null, "Foo", (1 as int?,"One"), (2, "Two"), (null, "Three")) );
    }
  }
}