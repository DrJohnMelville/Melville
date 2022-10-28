using System.Collections;
using System.Collections.Generic;
using Melville.Hacks;
using Melville.INPC;
using Xunit;

namespace Melville.Mvvm.Test.CSharpHacks;

public partial class DictionaryExtensionsTest
{
    private partial class IntWrapper
    {
        [FromConstructor]public int  Data { get; }
    }

    private readonly Dictionary<int, IntWrapper> sut = new();
    private IntWrapper Wrap(int i) => new IntWrapper(i);

    [Fact]
    public void AddToEmptyCache()
    {
        Assert.Empty(sut);
        var item = sut.GetOrCreate(1, Wrap);
        Assert.Single(sut);
        Assert.Equal(1, item.Data);
        item = sut.GetOrCreate(2, Wrap);
        Assert.Equal(2, sut.Count);
        Assert.Equal(2, item.Data);
    }
    [Fact]
    public void RecycleDuplicateItem()
    {
        Assert.Empty(sut);
        var item1 = sut.GetOrCreate(1, Wrap);
        var item2 = sut.GetOrCreate(1, Wrap);
        Assert.Single(sut);
        Assert.Same(item1, item2);
    }
}