using System;
using Melville.WpfControls.CalendarControls;

namespace Melville.Wpf.Samples.CalendarControls
{
    public record CalendarItem (object Content, DateTime Begin, DateTime End, DateTime LastDisplayDate): ICalendarItem
    {
        public CalendarItem(object content, DateTime begin, DateTime end):
            this(content, begin, end, end.Date)
        {
        }

        public CalendarItem(string title, int day1, int hour1, int day2, int hour2) :
            this(title, MakeDate(day1, hour1), MakeDate(day2, hour2))
        {
        }

        private static DateTime MakeDate(int day, int hour) => new(2021, 08, day, hour, 0,0);
    }

    public class CalendarControlViewModel
    {
        public CalendarItem[] Items { get; } = new[]
        {
            new CalendarItem("First Day", 1, 8, 1, 17),
            new CalendarItem("Weekend Appointment", 13, 8, 16, 17),
            new CalendarItem("3 Days", 1,7,4,10)
        };

    }
}