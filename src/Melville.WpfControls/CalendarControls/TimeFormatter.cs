using System;

namespace Melville.WpfControls.CalendarControls;

public static class TimeFormatter
{
    public static string ShortDisplayTimeString(this DateTime time) =>
        time.ToString(time.Minute == 0 ? "ht" : "h:mmt").ToLower();

    public static string TimeSpanStringOnDay(DateTime begin, DateTime end, DateTime displayDate)
    {
        var endDate = displayDate.AddDays(1);
        return TimeSpanString(begin > displayDate ? begin : displayDate, end < endDate ? end : endDate);

    }

    private static string TimeSpanString(DateTime begin, DateTime end)
    {
        return $"{begin.ShortDisplayTimeString()}-{end.ShortDisplayTimeString()}";
    }
}