
using System.Linq;
using FluentAssertions;
using Melville.Linq;
using Xunit;

namespace Melville.Mvvm.Test.Linq;

public class CountedTest
{
    [Fact]
    public void CountedEnumerabeTest()
    {
        Enumerable.Range(1, 3).AsCounted().Should().BeEquivalentTo(
            [
                (1, 0, false),
                (2, 1, false),
                (3, 2, true)
            ]);
    }
}
