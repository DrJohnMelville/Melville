using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Melville.Hacks;
using Melville.Linq;
using Melville.Lists;

namespace Melville.WpfControls.CalendarControls;

public class CalendarBarCollection : ThreadSafeBindableCollection<CalendarBar>
{
    private DateTime beginDate;
    private DateTime endDate;
        
    public CalendarBarCollection(IEnumerable<ICalendarItem> source, DateTime beginDate, DateTime endDate)
    {
        this.beginDate = beginDate;
        this.endDate = endDate;
        Debug.Assert(beginDate.DayOfWeek == DayOfWeek.Sunday);
        Debug.Assert(endDate.DayOfWeek == DayOfWeek.Saturday);
        source.NotifyAllAndMonitorCollection(
            AddCalendarItem, RemoveCalendarItem, ()=>Clear());
    }

    private void AddCalendarItem(ICalendarItem item) => this.AddRange(AllBarsForItem(item));

    private IEnumerable<CalendarBar> AllBarsForItem(ICalendarItem coverage) =>
        new CalendarBar(coverage, coverage.Begin.Date)
            .BarsForAllWeeks()
            .Where(i=>IsVisibleDate(i, i.BaseDate));

    private bool IsVisibleDate(CalendarBar i, DateTime date) => date >= beginDate && date <= endDate;

    private void RemoveCalendarItem(ICalendarItem item) => 
        this.RemoveRange(this.Where(i=>i.Item == item).ToList());
}