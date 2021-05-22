using System;
using System.Collections.Generic;
using System.Linq;
using Melville.Linq;

namespace Melville.WpfControls.CalendarControls
{
    public sealed class CalendarBar
    {
        public ICalendarItem Item {get;}
        public DateTime BaseDate { get; }
        public DateTime EndDate => Item.LastDisplayDate;
        public int TotalWidth() => 1 + (EndDate - BaseDate).Days;
        public int DisplayWidth() => Math.Min(TotalWidth(), 7 - (int) BaseDate.DayOfWeek);
        public bool IsFinalBlock() => TotalWidth() == DisplayWidth();
        
        public string LeftTime
        {
            get
            {
                var startTime = Item.Begin;
                return startTime.Date == BaseDate ? startTime.ShortDisplayTimeString() : "3";
            }
        }

        public string RightTime => this.IsFinalBlock()?
            Item.End.ShortDisplayTimeString():
            "4";

        public CalendarBar(ICalendarItem item, DateTime baseDate)
        {
            Item = item;
            BaseDate = baseDate;
        }

        public IEnumerable<CalendarBar> BarsForAllWeeks() =>
            FunctionalMethods.Sequence<CalendarBar?>(this, i => i?.ContinuationForNextWeek())
                .OfType<CalendarBar>();

        private CalendarBar? ContinuationForNextWeek()
        {
            var nextDate = BaseDate.AddDays(DisplayWidth());
            return ( nextDate <= EndDate) ? new CalendarBar(Item, nextDate) : null;
        }
    }
}