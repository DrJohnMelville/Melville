#nullable disable warnings
using  System.Collections.Specialized;
using System.Linq;
using Melville.Hacks;
using Melville.Linq;
using Melville.Lists;
using Melville.Lists.PersistentLinq;
using Xunit;

namespace Melville.Mvvm.Test.AdvancedLists.PersistentLinq;

public sealed class WhereCollectionTest
{
  [Fact]
  public void WhereDoesFiltering()
  {
    var list = Enumerable.Range(1, 100).ToList();
    var filtered = new WhereCollection<int>(list, i => i % 2 == 0);
    Assert.Equal(50, filtered.Count);
    for (int i = 0; i < 50; i++)
    {
      Assert.Equal((i + 1) * 2, filtered[i]);
    }
  }

  [Fact]
  public void RequeryOnSourceChange()
  {
    var list = new ThreadSafeBindableCollection<int>();
    list.AddRange(Enumerable.Range(1, 100));
    var filtered = new WhereCollection<int>(list, i => i % 2 == 0);
    Assert.Equal(50, filtered.Count);
    list.Add(3);
    Assert.Equal(50, filtered.Count);
    list.Add(4);
    Assert.Equal(51, filtered.Count);
  }
  [Fact]
  public void SendListChangeNotification()
  {
    var list = new ThreadSafeBindableCollection<int>();
    list.AddRange(Enumerable.Range(1, 100));
    var filtered = new WhereCollection<int>(list, i => i % 2 == 0);
    int ccFired = 0;
    filtered.CollectionChanged += (s, e) =>
    {
      Assert.Equal(filtered, s);
      Assert.Equal(NotifyCollectionChangedAction.Reset, e.Action);
      ccFired++;
    };
    list.Add(4);
    Assert.Equal(1, ccFired);
  }

  [Fact]
  public void SendPropertyChangeNotification()
  {
    var list = new ThreadSafeBindableCollection<int>();
    list.AddRange(Enumerable.Range(1, 100));
    var filtered = new WhereCollection<int>(list, i => i % 2 == 0);
    int ccFired = 0;
    filtered.PropertyChanged += (s, e) =>
    {
      Assert.Equal(filtered, s);
      Assert.Equal("Count", e.PropertyName);
      ccFired++;
    };
    list.Add(4);
    Assert.Equal(1, ccFired);
  }
}