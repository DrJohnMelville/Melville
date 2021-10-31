#nullable disable warnings
using  System;
using Melville.Lists;
using Xunit;

namespace Melville.Mvvm.Test.AdvancedLists;

public sealed class FixedStackTest
{
  [Fact]
  public void SimplePushAndPop()
  {
    var us = new FixedStack<int>(10);
    Assert.False(us.HasItems);
    us.Push(12);
    Assert.True(us.HasItems);
    Assert.Equal(12, us.Pop());
    Assert.False(us.HasItems);
  }
  [Fact]
  public void ComplexPushAndPop()
  {
    var us = new FixedStack<int>(10);

    us.Push(10);
    us.Push(25);
    Assert.Equal(25, us.Pop());
    Assert.Equal(10, us.Pop());
    Assert.Throws<InvalidOperationException>(() => us.Pop());

  }
  [Fact]
  public void Contains()
  {
    var us = new FixedStack<int>(10);

    Assert.False(us.Contains(10));
    Assert.False(us.Contains(25));
    Assert.False(us.Contains(35));
    us.Push(10);
    Assert.True(us.Contains(10));
    Assert.False(us.Contains(25));
    Assert.False(us.Contains(35));
    us.Push(25);
    Assert.True(us.Contains(10));
    Assert.True(us.Contains(25));
    Assert.False(us.Contains(35));
    Assert.Equal(25, us.Pop());
    Assert.True(us.Contains(10));
    Assert.False(us.Contains(25));
    Assert.False(us.Contains(35));
    Assert.Equal(10, us.Pop());
    Assert.False(us.Contains(10));
    Assert.False(us.Contains(25));
    Assert.False(us.Contains(35));
  }
  [Fact]
  public void StackHasBottom()
  {
    int popped = 0;
    var us = new FixedStack<int>(20);
    for (int i = 0; i < 40; i++)
    {
      us.Push(i);
    }
    Assert.Equal(0, popped);

    for (int j = 0; j < 20; j++)
    {
      Assert.Equal(39 - j, us.Pop());
    }

    Assert.Throws<InvalidOperationException>(() => us.Pop());
  }

  [Fact]
  public void Flush()
  {
    var us = new FixedStack<int>(12);
    us.Push(10);
    us.Push(10);
    us.Push(10);
    us.Push(10);
    Assert.True(us.HasItems);
    us.Clear();
    Assert.False(us.HasItems);
  }
}