using System;
using System.Threading.Tasks;

namespace Melville.SystemInterface.Time;

public interface IWallClock
{
  DateTime CurrentLocalTime();
  Task Wait(TimeSpan span);
  bool IsPhysicalClock { get; }
}

public static class WallClockOperations
{
  public static Task WaitUntil(this IWallClock clock, DateTime targetTime)
  {
    var now = clock.CurrentLocalTime();
    return now >= targetTime ? Task.CompletedTask : clock.Wait(targetTime - now);
  }
}