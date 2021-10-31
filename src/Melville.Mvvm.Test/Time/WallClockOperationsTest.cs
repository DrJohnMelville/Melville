#nullable disable warnings
using  System;
using System.Threading.Tasks;
using Melville.SystemInterface.Time;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.Time;

public sealed class WallClockOperationsTest
{
  private readonly  Mock<IWallClock> clock = new Mock<IWallClock>();

  public WallClockOperationsTest()
  {
    clock.Setup(i => i.Wait(It.IsAny<TimeSpan>())).Returns(Task.CompletedTask);
    clock.Setup(i => i.CurrentLocalTime()).Returns(new DateTime(1975, 7, 28));
  }

  [Theory]
  [InlineData(5)]
  [InlineData(10)]
  [InlineData(34)]
  public void WaitOneHour(int minutes)
  {
    clock.Object.WaitUntil(new DateTime(1975, 7, 28, 0, minutes, 0));
    clock.Verify(i=>i.Wait(TimeSpan.FromMinutes(minutes)), Times.Once);
  }

  [Fact]
  public void DoNotWaitForThePast()
  {
    clock.Object.WaitUntil(new DateTime(1975, 7, 27, 0, 1, 0));
    clock.Verify(i => i.Wait(It.IsAny<TimeSpan>()), Times.Never);
  }
  [Fact]
  public void DoNotWaitForNow()
  {
    clock.Object.WaitUntil(new DateTime(1975, 7, 28));
    clock.Verify(i => i.Wait(It.IsAny<TimeSpan>()), Times.Never);
  }


}