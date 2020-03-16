#nullable disable warnings
using  System;
using Melville.WpfControls.TimePickers;
using Xunit;

namespace Melville.WpfControls.Test.TimePickers
{
  public class TimePickerTest
  {
    [Theory]
    [InlineData("1:23:45 am", 1, 23, 45)]
    [InlineData("1:23 am", 1, 23, 0)]
    [InlineData("1 am", 1, 0, 0)]
    [InlineData("1am", 1, 0, 0)]
    [InlineData("1:23:45", 1, 23, 45)]
    [InlineData("1:23", 1, 23, 0)]
    [InlineData("1", 1, 0, 0)]
    [InlineData("1:23:45 pm", 13, 23, 45)]
    [InlineData("1:23 pm", 13, 23, 0)]
    [InlineData("1 pm", 13, 0, 0)]
    [InlineData("1pm", 13, 0, 0)]
    [InlineData("23:00:00", 23, 0, 0)]
    [InlineData("12:00A", 0, 0, 0)]
    [InlineData("12:00", 12, 0, 0)]
    [InlineData("12:00p", 12, 0, 0)]
    [InlineData("h-1", 11, 0, 0)]
    [InlineData("h+8", 20, 0, 0)]
    [InlineData("m-5", 11, 55, 0)]
    [InlineData("m+5", 12, 05, 0)]
    [InlineData("s+20", 12, 00, 20)]
    [InlineData("s-20", 11, 59, 40)]
    [InlineData("H-1", 11, 0, 0)]
    [InlineData("H+8", 20, 0, 0)]
    [InlineData("M-5", 11, 55, 0)]
    [InlineData("M+5", 12, 05, 0)]
    [InlineData("S+20", 12, 00, 20)]
    [InlineData("S-20", 11, 59, 40)]
    [InlineData("-1:00", 1, 0, 0)]
    [InlineData("23:59:59", 23, 59, 59)]
    [InlineData("24:00:00", 9, 9, 9)]
    public void TestTimeParser(string input, int hour, int min, int sec)
    {
      var result = TimeParser.Parse(input, new DateTime(1, 1, 1, 12, 0, 0)) ?? new TimeSpan(9, 9, 9, 9, 9);
      Assert.Equal(hour, result.Hours);
      Assert.Equal(min, result.Minutes);
      Assert.Equal(sec, result.Seconds);
    }
  }
}