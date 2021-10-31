using System.Collections.Generic;
using System.ComponentModel;
using Xunit;
using Melville.Linq;

namespace Melville.Mvvm.Test.Linq;

public class EnumerateSingleTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(1345)]
    public void SingleEnumeration(int value)
    {
        Assert.Equal(new []{value}, new EnumerateSingle<int>(value));
    }

    [Fact]
    public void ResetValue()
    {
        var iter = new EnumerateSingle<int>(1).GetEnumerator();
        TryIteratorFunctions(iter);
        iter.Reset();
        TryIteratorFunctions(iter);
            
    }

    private static void TryIteratorFunctions(IEnumerator<int> iter)
    {
        Assert.Equal(1, iter.Current);
        Assert.True(iter.MoveNext());
        Assert.Equal(1, iter.Current);
        Assert.False(iter.MoveNext());
        Assert.Equal(1, iter.Current);
    }
}