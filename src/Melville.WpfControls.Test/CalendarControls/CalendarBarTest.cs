using System;
using Melville.WpfControls.CalendarControls;
using Moq;
using Xunit;

namespace Melville.WpfControls.Test.CalendarControls
{
    public class CalendarBarTest
    {
        private static readonly DateTime baseDate = new DateTime(1975, 07, 28);
        private readonly Mock<ICalendarItem> calItem = new();
        private readonly CalendarBar sut;

        public CalendarBarTest()
        {
            sut = new (calItem.Object, baseDate);
        }

        [Theory]
        [InlineData(8, 12, "8a", "12p")]
        [InlineData(8, 23, "8a", "11p")]
        [InlineData(8, 2401, "8a", "4")]
        [InlineData(-3, 12, "3", "12p")]
        [InlineData(-0.01, 12, "3", "12p")]
        [InlineData(0.01, 12, "12a", "12p")]
        public void EndLabels(double beginHours, double endHours, string left, string right)
        {
            calItem.SetupGet(i => i.Begin).Returns(baseDate.AddHours(beginHours));
            calItem.SetupGet(i => i.End).Returns(baseDate.AddHours(endHours));
            calItem.SetupGet(i => i.LastDisplayDate).Returns(baseDate.AddHours(endHours).Date);
            Assert.Equal(calItem.Object, sut.Item);
            Assert.Equal(baseDate, sut.BaseDate);
            Assert.Equal(left, sut.LeftTime);
            Assert.Equal(right, sut.RightTime);
            
        }
    }
}