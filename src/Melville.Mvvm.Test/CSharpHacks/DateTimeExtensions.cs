#nullable disable warnings
using  System;
using Melville.Hacks;
using Xunit;

namespace Melville.Mvvm.Test.CSharpHacks;

public sealed class DateTimeExtension
{
  [Theory]
  [InlineData(7, 2, 2018, 31)]
  [InlineData(8, 2, 2018, 31)]
  [InlineData(9, 2, 2018, 30)]
  [InlineData(2, 2, 2018, 28)]
  [InlineData(2, 2, 2020, 29)]
  public void LastDayOfMonth(int month, int day, int year, int lastDay)
  {
    Assert.Equal(new DateTime(year, month, lastDay, 23, 59, 59, 999),
      new DateTime(year, month, day).EndOfMonth());
    Assert.Equal(new DateTime(year, month, 1),
      new DateTime(year, month, day).StartOfMonth());
  }

  [Theory]
  [InlineData(10, 5, 11)]
  [InlineData(11, 5, 11)]
  [InlineData(12, 12, 18)]
  [InlineData(13, 12, 18)]
  [InlineData(14, 12, 18)]
  [InlineData(15, 12, 18)]
  [InlineData(16, 12, 18)]
  [InlineData(17, 12, 18)]
  [InlineData(18, 12, 18)]
  [InlineData(19, 19, 25)]
  [InlineData(20, 19, 25)]
  public void BeginningOfWeek(int day, int beginDay, int endDay)
  {
    var date = new DateTime(2018, 8, day);
    Assert.Equal(new DateTime(2018, 8, beginDay), date.StartOfWeek());
    Assert.Equal(new DateTime(2018, 8, endDay, 23, 59, 59, 999), date.EndOfWeek());
  }

  [Fact]
  public void BeginEndOfDay()
  {
    var date = new DateTime(1975, 7, 28, 12, 21, 11);
    Assert.Equal(date.Date, date.StartOfDay());
    Assert.Equal(new DateTime(1975, 7, 28, 23, 59, 59, 999), date.EndOfDay());
      
  }


}