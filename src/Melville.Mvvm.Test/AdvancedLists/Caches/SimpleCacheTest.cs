#nullable disable warnings
using  System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Melville.MVVM.AdvancedLists.Caches;
using Xunit;

namespace Melville.Mvvm.Test.AdvancedLists.Caches
{
  public sealed class SimpleCacheTest
  {
    [Fact]
    public void RetreiveItem()
    {
      var cache = new SimpleCache<int, int>(10, i => 100 + i);
      for (int i = 0; i < 100; i++)
      {
        Assert.Equal(i+100, cache.Get(i));
      }
    }

    private class IntHolder
    {
      public int Int { get; set; }
    }

    [Fact]
    public void RememberOldEntries()
    {
      var cache = new SimpleCache<int, IntHolder>(10, i => new IntHolder {Int = i});
      var original = Enumerable.Range(1, 10).Select(cache.Get).ToArray();
      var second = Enumerable.Range(1, 10).Select(cache.Get).ToArray();
      Assert.True(original.Zip(second, (i, j) => i == j).All(i=>i));
    }
    [Fact]
    public void ForgetOldEntries()
    {
      var cache = new SimpleCache<int, IntHolder>(10, i => new IntHolder {Int = i});
      var original = Enumerable.Range(1, 10).Select(cache.Get).ToArray();
      var second = Enumerable.Range(1, 10).Select(cache.Get).ToArray();
      Assert.True(original.Zip(second, (i, j) => i == j).All(i=>i));
    }

    [Fact]
    public void Dump()
    {
      var cache = new SimpleCache<int, IntHolder>(10, i => new IntHolder { Int = i });
      var a = cache.Get(1);
      Assert.Equal(a, cache.Get(1));
      cache.Dump();
      Assert.NotEqual(a, cache.Get(1));
    }
    [Fact]
    public void Forget()
    {
      var cache = new SimpleCache<int, IntHolder>(10, i => new IntHolder { Int = i });
      var a = cache.Get(1);
      var b = cache.Get(2);
      Assert.Equal(a, cache.Get(1));
      Assert.Equal(b, cache.Get(2));
      cache.Forget(1);
      Assert.NotEqual(a, cache.Get(1));
      Assert.Equal(b, cache.Get(2));
    }

    [Fact]
    public void Rekey()
    {
      var cache = new SimpleCache<int, IntHolder>(10, i => new IntHolder { Int = i });
      var a = cache.Get(1);
      var b = cache.Get(2);
      Assert.Equal(a, cache.Get(1));
      Assert.Equal(b, cache.Get(2));
      cache.Rekey(i=>i*10);
      Assert.Equal(a, cache.Get(10));
      Assert.Equal(b, cache.Get(20));
    }

    [Fact]
    public void GetAndPreserveAccrossGC()
    {
      int count = 0;
      var cache = new WeakCache<int, IntHolder>(10, key => new IntHolder() {Int = count++});
      var v1 = cache.Get(1);
      GC.Collect();
      Assert.Equal(v1.Int, cache.Get(1).Int);
    }
    [Fact]
    public void GetAndDoNotPreserveAccrossGC()
    {
      int count = 0;
      var cache = new WeakCache<int, IntHolder>(10, key => new IntHolder() {Int = count++});
      var v1Value = GetValue(cache);
      GC.Collect();
      Assert.NotEqual(v1Value, cache.Get(1).Int);
    }

    private static int GetValue(WeakCache<int, IntHolder> cache)
    {
      //Put this in its own method so that the implicit local reference does not save the weak reference from the GC
      return cache.Get(1).Int;
    }
  }
}
