using Melville.Linq;
using Xunit;

namespace Melville.Mvvm.Test.Linq;

public class EmptyEnumeratorTest
{
    private readonly EnumerateEmpty<int> sut = new();

    [Fact]
    public void IsEmpty()
    {
        Assert.Equal(0, sut.Current);
        Assert.False(sut.MoveNext());
            
    }
}