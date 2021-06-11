using System;
using System.Windows;
using Melville.WpfControls.CalendarControls;
using Xunit;

namespace Melville.WpfControls.Test.CalendarControls
{
    public class ClickToDayComputerTest
    {
        private readonly ClickToDayComputer sut = new(new DateTime(2021,8,1), 92,
            new double[] {10, 20, 30, 40, 50});

        [Theory]
        [InlineData(5, 4, 2, 0, 10, 10)] // row 0 col 0
        [InlineData(5, 14, 2, 10, 10, 20)] // row 1
        [InlineData(5, 24, 2, 10, 10, 20)] // row 1
        [InlineData(5, 34, 2, 30, 10, 30)] // row 2
        [InlineData(17, 4, 15, 0, 10, 10)] // column 1
        [InlineData(31, 4, 28, 0, 10, 10)] // column 2
        public void RectangleFromPointTest(double x, double y, double left, double top, double width, double height) => 
            Assert.Equal(new Rect(left, top, width, height), sut.RectangleFromPoint(new Point(x,y)));
        
        [Theory]
        [InlineData(5,5,1)]
        [InlineData(5,15,8)]
        [InlineData(17,5,2)]
        public void DateFromPointTest(double x, double y, int dayInAug2021)=>
            Assert.Equal(new DateTime(2021, 8, dayInAug2021), sut.DateFromPoint(new Point(x,y)));
      
    }
}