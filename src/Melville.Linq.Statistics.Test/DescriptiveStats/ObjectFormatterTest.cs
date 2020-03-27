using Melville.Linq.Statistics.Tables;
using Xunit;

namespace Test.DescriptiveStats
{
  public sealed class ObjectFormatterTest
  {
    [Fact]
    public void DefaultFormat()
    {
      var formatter = new ObjectFormatter();
      Assert.Equal("10", formatter.Format(10) );
      Assert.Equal("<null>", formatter.Format(null) );
    }

    [Fact]
    public void ApplyDoubleFormat()
    {
      var formatter = new ObjectFormatter();
      formatter.AddFormatter((double d) => d.ToString("#####0.#"));
      Assert.Equal("10.5", formatter.Format(10.52345643) );
    }

  }
}