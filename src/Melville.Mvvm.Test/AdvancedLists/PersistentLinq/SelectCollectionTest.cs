#nullable disable warnings
using  System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.AdvancedLists.PersistentLinq;
using Melville.MVVM.Functional;
using Xunit;

namespace Melville.Mvvm.Test.AdvancedLists.PersistentLinq
{
  public sealed class SelectCollectionTest
  {
    private static SelectCollection<int, int> TestList1To5()
    {
      var ints = new[] { 0, 1, 2, 3, 4 };
      var sc = new SelectCollection<int, int>(ints, i => -i);
      return sc;
    }
    private static Tuple<ThreadSafeBindableCollection<int>, SelectCollection<int, int>> BindingList1To5()
    {
      var ints = new ThreadSafeBindableCollection<int>();
      ints.AddRange(new[] { 0, 1, 2, 3, 4 });
      var sc = new SelectCollection<int, int>(ints, i => -i);
      return Tuple.Create(ints, sc);
    }
    [Fact]
    public void CountWorks()
    {
      Assert.Equal(5, TestList1To5().Count);

    }
    [Fact]
    public void SimpleSelect()
    {
      var sc = TestList1To5();

      for (int i = 0; i < 5; i++)
      {
        Assert.Equal(-i, sc[i]);
      }
    }
    [Fact]
    public void ObjectEnumerator()
    {
      var ints = TestList1To5();
      int i = 0;
      foreach (int item in ((IEnumerable)ints))
      {
        Assert.Equal(i--, item);

      }
    }
    [Fact]
    public void IsReadOnly()
    {
      Assert.True(TestList1To5().IsReadOnly);

    }
    [Fact]
    public void TypedEnumerator()
    {
      var ints = TestList1To5();
      int i = 0;
      foreach (var item in ints)
      {
        Assert.Equal(i--, item);

      }
    }
    [Fact]
    public void Contains()
    {
      var items = TestList1To5();

      Assert.Contains(-3, items);
      Assert.DoesNotContain(-30, items);
      Assert.DoesNotContain(3, items);
    }
    [Fact]
    public void CopyArrayWorks()
    {
      var destArray = new int[10];
      TestList1To5().CopyTo(destArray, 2);
      for (int i = 0; i < 5; i++)
      {
        Assert.Equal(-i, destArray[i + 2]);
      }
    }
    [Fact]
    public void IndexOf()
    {
      var list = TestList1To5();

      Assert.Equal(1, list.IndexOf(-1));
      Assert.Equal(3, list.IndexOf(-3));
      Assert.Equal(-1, list.IndexOf(3));
      Assert.Equal(-1, list.IndexOf(10));
      Assert.Equal(-1, list.IndexOf(-7));
    }
    [Fact]
    public void BindingListProvidesNotificationOnAdd()
    {
      var data = BindingList1To5();
      int collChangedCount = 0;
      data.Item2.CollectionChanged += (s, e) =>
      {
        collChangedCount++;
        Assert.Equal(1, e.NewItems.Count);
        Assert.Equal(-6, e.NewItems[0]);
        Assert.Equal(5, e.NewStartingIndex);

      };
      Assert.Equal(0, collChangedCount);

      data.Item1.Add(6);

      Assert.Equal(1, collChangedCount);

    }
    [Fact]
    public void BindingListProvidesNoNotificationsAfterDispose()
    {
      var data = BindingList1To5();
      int collChangedCount = 0;
      data.Item2.CollectionChanged += (s, e) =>
      {
        collChangedCount++;
      };
      Assert.Equal(0, collChangedCount);
      data.Item2.Dispose();
      data.Item1.Add(6);

      Assert.Equal(0, collChangedCount);

    }
    [Fact]
    public void BindingListProvidesNotificationOnRemove()
    {
      var data = BindingList1To5();
      int collChangedCount = 0;
      data.Item2.CollectionChanged += (s, e) =>
      {
        collChangedCount++;
        Assert.Equal(1, e.OldItems.Count);
        Assert.Equal(-3, e.OldItems[0]);
        Assert.Equal(3, e.OldStartingIndex);
        //        Assert.Equal(NotifyCollectionChangedAction.Reset, e.Action);
      };
      Assert.Equal(0, collChangedCount);

      data.Item1.Remove(3);

      Assert.Equal(1, collChangedCount);
    }
    [Fact]
    public void BindingListProvidesNotificationOnReplace()
    {
      var data = BindingList1To5();
      int collChangedCount = 0;
      data.Item2.CollectionChanged += (s, e) =>
      {
        collChangedCount++;
        Assert.Equal(1, e.OldItems.Count);
        Assert.Equal(1, e.NewItems.Count);
        Assert.Equal(-2, e.OldItems[0]);
        Assert.Equal(-10, e.NewItems[0]);
      };
      Assert.Equal(0, collChangedCount);

      data.Item1[2] = 10;

      Assert.Equal(1, collChangedCount);
    }
    [Fact]
    public void BindingListProvidesNotificationOnClear()
    {
      var data = BindingList1To5();
      int collChangedCount = 0;
      data.Item2.CollectionChanged += (s, e) =>
      {
        collChangedCount++;
      };
      Assert.Equal(0, collChangedCount);

      data.Item1.Clear();

      Assert.Equal(6, collChangedCount);
    }
    [Fact]
    public void BindingListProvidesNotificationOnMove()
    {
      var data = BindingList1To5();
      int collChangedCount = 0;
      data.Item2.CollectionChanged += (s, e) =>
      {
        collChangedCount++;
        Assert.Equal(1, e.OldItems.Count);
        Assert.Equal(1, e.NewItems.Count);
        Assert.Equal(-2, e.OldItems[0]);
        Assert.Equal(-2, e.NewItems[0]);
        Assert.Equal(2, e.OldStartingIndex);
        Assert.Equal(0, e.NewStartingIndex);
      };
      Assert.Equal(0, collChangedCount);

      data.Item1.Move(2, 0);

      Assert.Equal(1, collChangedCount);
    }
    private class IntHolder
    {
      private readonly int value;
      public int Value
      {
        get { return value; }
      }

      public IntHolder(int value)
      {
        this.value = value;
      }
    }

    [Fact]
    public void SelectCollectionCachesObjects()
    {
      int objectCount = 0;
      var baseColl = new ThreadSafeBindableCollection<int>();
      var coll = new SelectCollection<int, IntHolder>(baseColl,
        i => new IntHolder(objectCount++));

      baseColl.Add(1);
      baseColl.Add(2);
      baseColl.Add(3);

      Assert.Equal(0, coll[0].Value);
      Assert.Equal(1, coll[1].Value);
      Assert.Equal(2, coll[2].Value);
      Assert.Equal(0, coll[0].Value);
      Assert.Equal(1, coll[1].Value);
      Assert.Equal(2, coll[2].Value);

      Assert.Equal(3, objectCount);
    }

    [Fact]
    public void DeleteWorks()
    {
      int objectCount = 0;
      var baseColl = new ThreadSafeBindableCollection<int>();
      var coll = new SelectCollection<int, IntHolder>(baseColl,
        i => new IntHolder(objectCount++));

      baseColl.Add(1);
      baseColl.Add(2);
      baseColl.Add(3);

      Assert.Equal(0, coll[0].Value);
      Assert.Equal(1, coll[1].Value);
      Assert.Equal(2, coll[2].Value);

      baseColl.Remove(2);
      Assert.Equal(2, coll[1].Value);

      baseColl.Add(2); // adding it back in should create a new object as old one was deleted
      Assert.Equal(3, coll[2].Value); // should not have created objects when deleting

      Assert.Equal(4, objectCount);
    }

    [Fact]
    public void PropogateRemoveAtToInerCollection()
    {
      int objectCount = 0;
      var baseColl = new ThreadSafeBindableCollection<int>();
      var coll = new SelectCollection<int, IntHolder>(baseColl,
        i => new IntHolder(objectCount++));

      baseColl.Add(1);
      baseColl.Add(2);
      baseColl.Add(3);

      Assert.Equal(0, coll[0].Value);
      Assert.Equal(1, coll[1].Value);
      Assert.Equal(2, coll[2].Value);

      coll.RemoveAt(1);
      Assert.Equal(2, coll[1].Value);
    }
    [Fact]
    public void SetDeletesObjectWorks()
    {
      int objectCount = 0;
      var baseColl = new ThreadSafeBindableCollection<int>();
      var coll = new SelectCollection<int, IntHolder>(baseColl,
        i => new IntHolder(objectCount++));

      baseColl.Add(1);
      baseColl.Add(2);
      baseColl.Add(3);

      Assert.Equal(0, coll[0].Value);
      Assert.Equal(1, coll[1].Value);
      Assert.Equal(2, coll[2].Value);

      baseColl[1] = 4;
      Assert.Equal(3, coll[1].Value);

      baseColl.Add(2); // adding it back in should create a new object as old one was deleted
      Assert.Equal(4, coll[3].Value); // should not have created objects when deleting

      Assert.Equal(5, objectCount);
    }
  }
}
