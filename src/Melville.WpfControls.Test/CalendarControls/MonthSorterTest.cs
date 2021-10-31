using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using Melville.WpfControls.CalendarControls;
using Moq;
using Xunit;

namespace Melville.WpfControls.Test.CalendarControls;

public record FakeSortSource(DateTime Begin, DateTime End, double Height) : ICalendarItem
{
    public object Content => this;

    public DateTime LastDisplayDate => End.Date;
}
public interface IFakeSortTarget
{
    double DoIt(FakeSortSource target, int column, int columnSpan, double yPosition);
} 
public class MonthSorterTest
{
    private readonly DateTime SundayTheFirst = new DateTime(2021, 08, 01);
    private readonly Mock<IFakeSortTarget> sortTarget = new();
    private readonly MonthSorter<FakeSortSource> sut;

    public MonthSorterTest()
    {
        sortTarget.Setup(i => i.DoIt(It.IsAny<FakeSortSource>(), It.IsAny<int>(), It.IsAny<int>(),
            It.IsAny<double>())).Returns((FakeSortSource fss, int _, int _, double _) => fss.Height);
        sut = new MonthSorter<FakeSortSource>(SundayTheFirst, 20, 
            i => new CalendarBar(i, i.Begin.Date), sortTarget.Object.DoIt);
    }

    public IList<FakeSortSource>
        MakeItems(params (double startHours, double durationHours, double height)[] input) =>
        input.Select(i => new FakeSortSource(
                SundayTheFirst.AddHours(i.startHours),
                SundayTheFirst.AddHours(i.startHours + i.durationHours),
                i.height))
            .ToList();
        
    [Fact]
    public void FirstItemOnFirstDay()
    {
        var sources = MakeItems((8, 4, 12));
        sut.SortDates(sources);
        sortTarget.Verify(i=>i.DoIt(sources[0], 0, 1, 20));
        sortTarget.VerifyNoOtherCalls();
        Assert.Equal(112, sut.TotalHeight);
    }
    [Fact]
    public void ItemLastsTwoDays()
    {
        var sources = MakeItems((8, 34, 12));
        sut.SortDates(sources);
        sortTarget.Verify(i=>i.DoIt(sources[0], 0, 2, 20));
        sortTarget.VerifyNoOtherCalls();
        Assert.Equal(112, sut.TotalHeight);
    }
    [Fact]
    public void MultiDayItemDisplacesSecondItem()
    {
        var sources = MakeItems((8, 34, 12), (8+24,4,12));
        sut.SortDates(sources);
        sortTarget.Verify(i=>i.DoIt(sources[0], 0, 2, 20));
        sortTarget.Verify(i=>i.DoIt(sources[1], 1, 1, 32));
        sortTarget.VerifyNoOtherCalls();
        Assert.Equal(124, sut.TotalHeight);
    }
    [Fact]
    public void FirstItemOnSecondDay()
    {
        var sources = MakeItems((24+8, 4, 12));
        sut.SortDates(sources);
        sortTarget.Verify(i=>i.DoIt(sources[0], 1, 1, 20));
        sortTarget.VerifyNoOtherCalls();
        Assert.Equal(112, sut.TotalHeight);
    }
    [Fact]
    public void TwoItemsOnDayOne()
    {
        var sources = MakeItems((8, 4, 12),(13,1,10));
        sut.SortDates(sources);
        sortTarget.Verify(i=>i.DoIt(sources[0], 0, 1, 20));
        sortTarget.Verify(i=>i.DoIt(sources[1], 0, 1, 32));
        sortTarget.VerifyNoOtherCalls();
        Assert.Equal(122, sut.TotalHeight);
    }

    [Fact]
    public void EmptyMonthHasCorrectNumberOfWeeks()
    {
        var sources = MakeItems((8, 34, 12), (8+24,4,12));
        var heights=sut.SortDates(sources);
        Assert.Equal(5, heights.Count);
    }
}