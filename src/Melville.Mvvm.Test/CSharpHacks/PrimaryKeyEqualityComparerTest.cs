#nullable disable warnings
using Melville.Hacks;
using Xunit;

namespace Melville.Mvvm.Test.CSharpHacks
{
  public sealed class PrimaryKeyEqualityComparerTest
  {
    [Theory]
    [InlineData(0, 5, true)]
    [InlineData(1, 6, true)]
    [InlineData(1, 5, false)]
    public void Modulo5Comparer(int a, int b, bool result)
    {
      var comparer = new PrimaryKeyEqualityComparer<int, int>(i => i % 5);
      Assert.Equal(result, comparer.Equals(a,b));
      Assert.Equal(result, comparer.GetHashCode(a) == comparer.GetHashCode(b));
            
    }

    [Fact]
    public void EveryFixtureNeedsAFact()
    {
      
    }

  }
}