using System;
using Melville.Lists;
using Melville.WpfControls.CalendarControls;
using Moq;
using Xunit;

namespace Melville.WpfControls.Test.CalendarControls;

public class CalendarBarCollectionTest
{
    private readonly DateTime baseDate = new DateTime(1975, 07, 28);

    private readonly ThreadSafeBindableCollection<ICalendarItem> source = new();
    private readonly CalendarBarCollection sut;

    public CalendarBarCollectionTest()
    {
        sut = new CalendarBarCollection(source, 
            new DateTime(1975, 06,29), new DateTime(1975,08,2));
    }

    private void AddItem(double start, double durration, string item = "") => 
        AddItem(baseDate.AddHours(start), baseDate.AddHours(start + durration), item);

    private void AddItem(DateTime startTime, DateTime endTime, string name = "")
    {
        var item = new Mock<ICalendarItem>();
        item.SetupGet(i => i.Begin).Returns(startTime);
        item.SetupGet(i => i.End).Returns(endTime);
        item.SetupGet(i => i.LastDisplayDate).Returns(endTime.Date);
        item.SetupGet(i => i.Content).Returns(name);
        source.Add(item.Object);
    }

    [Fact]
    public void AddingAssingmentAddsScheduleLine()
    {
        AddItem(8, 4, "AddedItem");
        Assert.Single(sut);
        Assert.Equal("AddedItem", sut[0].Item.Content.ToString());
    }
    [Fact]
    public void RemoveAssignmentLine()
    {
        Assert.Empty(sut);
        AddItem(6,4);
        Assert.Single(sut);
        source.RemoveAt(0);
        Assert.Empty(sut);
    }
    [Fact]
    public void ClearSource()
    {
        Assert.Empty(sut);
        AddItem(8,4);
        Assert.Single(sut);
        source.Clear();
        Assert.Empty(sut);
    }

    [Theory]
    [InlineData(6)]
    [InlineData(8)]
    public void IgnoreOutOfRangeItems(int month)
    {
        AddItem(2400, 3);
        AddItem(new DateTime(1975,month,15, 8, 0,0), new DateTime(1975,month,15, 12, 0,0));
        Assert.Empty(sut);
    }

    [Fact]
    public void AddAssignmentThatOverlappsBeginDate()
    {
        AddItem(new DateTime(1975,6,28, 17, 0,0), new DateTime(1975,7,1, 17, 0,0));
        Assert.Single(sut);
        Assert.Equal("3", sut[0].LeftTime);
        Assert.Equal("5p", sut[0].RightTime);
        Assert.Equal(new DateTime(1975,06,29), sut[0].BaseDate);
            
    }

    [Fact]
    public void AddWeekBreakItem()
    {
        AddItem(new DateTime(1975,7,05, 17, 0,0), new DateTime(1975,7,8, 17, 0,0), "Split");
        Assert.Equal(2, sut.Count);
        var v1 = sut[0];
        var v2 = sut[1];
        Assert.Equal("Split", v1.Item.Content.ToString());
        Assert.Equal("Split", v2.Item.Content.ToString());
        Assert.Equal(v1.Item, v2.Item);
            
        Assert.Equal("5p", v1.LeftTime);
        Assert.Equal("4", v1.RightTime);
    }

    [Fact]
    public void DoNotExtendAfterLAstDate()
    {
        AddItem(new DateTime(1975,7,31, 17, 0,0), new DateTime(1975,8,15, 17, 0,0), "Split");
        Assert.Single(sut);

    }
}