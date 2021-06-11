using System;

namespace Melville.WpfControls.CalendarControls
{
    public interface ICalendarItem
    {
        object Content { get; }
        DateTime Begin { get; }
        DateTime End { get; }
        DateTime LastDisplayDate { get; }
    }
}