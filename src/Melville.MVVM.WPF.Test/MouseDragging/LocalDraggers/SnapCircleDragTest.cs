using System.Runtime.InteropServices;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers;

public class SnapCircleDragTest
{
    private readonly Mock<ILocalDragger<CircularPoint>> target = new();

    [Theory]
    [InlineData(20,100, 20)]
    [InlineData(39, 1, 39)]
    [InlineData(40.1, 10, 45)]
    [InlineData(43, 10, 45)]
    [InlineData(45, 10, 45)]
    [InlineData(49.9, 10, 45)]
    [InlineData(50.1, 120, 50.1)]
    [InlineData(55, 120, 55)]
    public void SmallSnap(double inputAngle, double length, double outputAngle)
    {
        var sut = LocalDragger.SnapToAngle(8, 10, target.Object);
        sut.NewPoint(MouseMessageType.Down, new CircularPoint(inputAngle, length));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new CircularPoint(outputAngle, length)));
        target.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(MouseMessageType.Up, 43, 45)]
    [InlineData(MouseMessageType.Down, 43, 43)]
    [InlineData(MouseMessageType.Move, 43, 43)]
    public void BigSnapSnapsMouseUp(MouseMessageType message, double inputAngle, double outputAngle)
    {
        var sut = LocalDragger.SnapMouseUpToAngle(8, 10, target.Object);
        sut.NewPoint(message, new CircularPoint(inputAngle, 15));
        target.Verify(i=>i.NewPoint(message, new CircularPoint(outputAngle, 15)));
        target.VerifyNoOtherCalls();
    }
}