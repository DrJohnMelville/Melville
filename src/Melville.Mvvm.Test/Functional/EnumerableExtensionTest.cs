#nullable disable warnings
using  System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Melville.MVVM.Functional;
using Xunit;

namespace Melville.Mvvm.Test.Functional
{
  public sealed class EnumerableExtensionTest
  {
    [Theory]
    [InlineData(2, -1, "2134")]
    [InlineData(2, 1, "1324")]
    [InlineData(2, 2, "1432")]
    public void SwapRelative(int elt, int delta, string result)
    {
      var list = Enumerable.Range(1,4).ToList();
      list.SwapRelative(elt, delta);
      Assert.Equal(result, list.Select(i=>i.ToString()).ConcatenateStrings());
    }

    [Fact]
    public void ZipActionTest()
    {
      int count = 0;
      new[] {1, 2, 3}.Zip(new[] {3, 2, 1}, (i, j) =>
      {
        count++;
        Assert.Equal(4, i+j);
      });
      Assert.Equal(3, count);
      
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void ByIndexSucceed(int i)
    {
      Assert.Equal(i, new[]{0,1,2,3}.ByIndex(i));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(22)]
    public void ByIndexFail(int i)
    {
      Assert.Throws<InvalidOperationException>(()=>new[] {1}.ByIndex(i));
    }

    [Fact]
    public void EmptyIfNullTests()
    {
      Assert.Empty(((IEnumerable<string>)null).EmptyIfNull());
      Assert.Empty(((IEnumerable)null).EmptyIfNull());
      Assert.Empty(new int[0].EmptyIfNull());
      Assert.Single(new []{1}.EmptyIfNull());
      
    }
  }
}
