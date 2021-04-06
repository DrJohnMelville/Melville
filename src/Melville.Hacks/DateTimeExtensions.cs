using System;

namespace Melville.Hacks
{
  public static class DateTimeExtensions
  {
    public static DateTime StartOfMonth(this DateTime baseDate) => 
      new DateTime(baseDate.Year, baseDate.Month, 1);

    public static DateTime EndOfMonth(this DateTime baseDate) => 
      new DateTime(baseDate.Year, baseDate.Month, DateTime.DaysInMonth(baseDate.Year, baseDate.Month),
        23, 59, 59,999);

    public static DateTime StartOfWeek(this DateTime baseDate) =>
      baseDate.Date.AddDays(-1 * ((int)baseDate.DayOfWeek));

    public static DateTime EndOfWeek(this DateTime baseDate) =>
      baseDate.StartOfWeek() + new TimeSpan(6, 23, 59, 59, 999);

    public static DateTime StartOfDay(this DateTime baseDate) =>
      baseDate.Date;

    public static DateTime EndOfDay(this DateTime baseDate) =>
      baseDate.StartOfDay() + new TimeSpan(0, 23, 59, 59, 999);
  }
}
