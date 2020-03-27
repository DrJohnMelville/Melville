using System.Linq;
using Melville.Linq.Statistics.HypothesisTesting;
using Xunit;

namespace Test.HypothesisTesting
{
  public sealed class UnknownFilterTest
  {
    private class SortedClass
    {
      public string S { get; }
      public bool? B { get; }
      public int? I { get; }

      public SortedClass(string s, bool? b, int? x)
      {
        S = s;
        B = b;
        I = x;
      }
    }
    [Fact]
    public void FilterItems()
    {
      var items = new[]
      {
        new SortedClass("Hello", true, 1),
        new SortedClass(null, true, 1),
        new SortedClass("Hello", null, 1),
        new SortedClass("Hello", true, null),
        new SortedClass("Hello", true, 1),
      };

      var uf = UnknownFilter.Create(items);
      uf.AddFilter(i=>i.S);
      uf.AddFilter(i=>i.B);
      uf.AddFilter(i=>i.I);

      Assert.Equal(2, uf.FilteredResult().Count());
      
    }
  }
}