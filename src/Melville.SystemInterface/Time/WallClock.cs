using System;
using System.Threading.Tasks;

namespace Melville.SystemInterface.Time;

public class WallClock : IWallClock
{
  public DateTime CurrentLocalTime() => DateTime.Now;
  public Task Wait(TimeSpan span) => Task.Delay(span);
  public bool IsPhysicalClock => true;
}