#nullable disable warnings
using  System.Collections.Specialized;
using System.Linq;
using Melville.Lists;
using Melville.Lists.PersistentLinq;
using Melville.TestHelpers.InpcTesting;
using Xunit;

namespace Melville.Mvvm.Test.AdvancedLists.PersistentLinq;

public sealed class SelectManyCollectionTest
{
  private readonly ThreadSafeBindableCollection<int> ints1;
  private readonly ThreadSafeBindableCollection<int> ints2;
  private readonly ThreadSafeBindableCollection<ThreadSafeBindableCollection<int>> allInts;
  private readonly SelectManyCollection<ThreadSafeBindableCollection<int>, int> smc;

  public SelectManyCollectionTest()
  {
    ints1 = new ThreadSafeBindableCollection<int> { 0, 1, 2, 3, 4, 5 };
    ints2 = new ThreadSafeBindableCollection<int> { 6, 7, 8, 9 };

    allInts = new ThreadSafeBindableCollection<ThreadSafeBindableCollection<int>>
      { ints1, ints2 };
    smc = new SelectManyCollection<ThreadSafeBindableCollection<int>, int>(allInts, i => i);

  }

  [Fact]
  public void SimpleAddressing()
  {
    Assert.Equal(10, smc.Count);


    for (int i = 0; i < smc.Count; i++)
    {
      Assert.Equal(i, smc[i]);
    }
  }
  [Fact]
  public void IndexOfTest()
  {
    for (int i = 0; i < smc.Count; i++)
    {
      Assert.Equal(i, smc.IndexOf(i));
    }
  }
  [Fact]
  public void Contains()
  {
    for (int i = 0; i < smc.Count; i++)
    {
      Assert.Contains(i, smc);
      Assert.DoesNotContain((i * 100) + 30, smc);
    }
  }
  [Fact]
  public void GetEnumerator()
  {
    var enumOutput = smc.ToArray();
    for (int i = 0; i < smc.Count; i++)
    {
      Assert.Equal(smc[i], enumOutput[i]);
    }
  }

  [Fact]
  public void WriteAddressing()
  {
    smc[7] = 100;
    Assert.Equal(100, ints2[1]);
    Assert.Equal(100, smc[7]);

    smc[3] = 200;
    Assert.Equal(200, ints1[3]);
    Assert.Equal(200, smc[3]);
  }

  [Fact]
  public void InsertInSubList()
  {
    int fired = 0;
    smc.CollectionChanged += (s, e) =>
    {
      Assert.Equal(1, e.NewItems.Count);
      Assert.Equal(100, e.NewItems[0]);
      Assert.Equal(8, e.NewStartingIndex);
      fired++;
    };

    using (var foo = INPCCounter.VerifyInpcFired(smc, i => i.Count))
    {
      ints2.Insert(2, 100);
    }

    Assert.Equal(1, fired);
  }
  [Fact]
  public void DeleteFromSubList()
  {
    int fired = 0;
    smc.CollectionChanged += (s, e) =>
    {
      Assert.Equal(1, e.OldItems.Count);
      Assert.Equal(8, e.OldItems[0]);
      Assert.Equal(8, e.OldStartingIndex);
      fired++;
    };

    using (var foo = INPCCounter.VerifyInpcFired(smc, i => i.Count))
    {
      ints2.RemoveAt(2);
    }

    Assert.Equal(1, fired);
  }

  [Fact]
  public void InsertInMainList()
  {
    int fired = 0;
    smc.CollectionChanged += (s, e) =>
    {
      Assert.Equal(NotifyCollectionChangedAction.Reset, e.Action);

      fired++;
    };

    using (var foo = INPCCounter.VerifyInpcFired(smc, i => i.Count))
    {
      allInts.Add(new ThreadSafeBindableCollection<int>());
    }

    Assert.Equal(1, fired);
  }
  [Fact]
  public void DeleteFromMainList()
  {
    int fired = 0;
    smc.CollectionChanged += (s, e) =>
    {
      Assert.Equal(NotifyCollectionChangedAction.Reset, e.Action);

      fired++;
    };

    using (var foo = INPCCounter.VerifyInpcFired(smc, i => i.Count))
    {
      allInts.RemoveAt(1);
    }

    Assert.Equal(1, fired);
  }

  [Fact]
  public void BugTestOnNotifyChangeAfterClear()
  {
    int fired = 0;
    smc.CollectionChanged += (s, e) => fired++;
    ints2.Add(12); // should work
    Assert.Equal(1, fired);
    allInts.Clear(); // now we have some outstanding property change notifications
    Assert.Equal(4, fired); // we get a notification for the clear operation
    Assert.Empty(smc);
    ints2.Add(13); // we get an add at an invalid index, but now the bug is fixed
    Assert.Equal(4, fired); // the second prop notification should be suppressed.
  }
}