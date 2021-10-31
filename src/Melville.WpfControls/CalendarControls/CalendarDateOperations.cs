using System;
using Melville.Hacks;

namespace Melville.WpfControls.CalendarControls;

public static class CalendarDateOperations
{
    public static DateTime FirstDayOnCalendar(this DateTime index) => index.StartOfMonth().StartOfWeek();
    public static DateTime LastDayOnCalendar(this DateTime index) => index.EndOfMonth().EndOfWeek();
}