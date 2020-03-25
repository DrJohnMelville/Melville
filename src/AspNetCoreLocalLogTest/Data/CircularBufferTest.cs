using System;
using AspNetCoreLocalLog.Data;
using Xunit;

namespace AspNetCoreLocalLogTest.Data
{
  public sealed class CircularBufferTest
  {

    private readonly CircularBuffer<int> sut = new CircularBuffer<int>(5);

    [Fact]
    public void BufferStartsEmpty()
    {
      Assert.Empty(sut.All());
    }

    [Theory]
    [InlineData(new []{1}, new[]{1})]
    [InlineData(new []{1, 2}, new[]{1, 2})]
    [InlineData(new []{1, 2, 3}, new[]{1, 2, 3})]
    [InlineData(new []{1, 2, 3, 4}, new[]{1, 2, 3, 4})]
    [InlineData(new []{1, 2, 3, 4, 5}, new[]{1, 2, 3, 4, 5})]
    [InlineData(new []{1, 2, 3, 4, 5, 6}, new[]{2, 3, 4, 5, 6})]
    [InlineData(new []{1, 2, 3, 4, 5, 6, 7, 8}, new[]{4, 5, 6, 7, 8})]
    [InlineData(new []{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}, new[]{8, 9, 10, 11, 12})]
    public void Return1(int[] src, int[] dest)
    {
      foreach (var i in src)
      {
        sut.Push(i);
      }
      Assert.Equal(dest, sut.All());
      
    }

    [Fact]
    public void Clear()
    {
      sut.Push(1);
      sut.Push(2);
      sut.Push(2);
      sut.Push(2);
      sut.Push(2);
      sut.Push(2);
      sut.Push(2);
      sut.Push(2);
      sut.Push(2);
      sut.Push(2);
      sut.Push(2);
      sut.Clear();
      sut.Push(3);
      sut.Push(4);
      Assert.Equal(new []{3,4}, sut.All());
      
    }

  }
}