using System.Windows;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers;

public class RectToCircleDraggerTest
{
    private readonly Mock<ILocalDragger<CircularPoint>> target = new();
        
    [Fact]
    public void DragCircle()
    {
        var sut = LocalDragger.RectToCircle(new Point(10, 10), target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(10,12));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new CircularPoint(270,2)));
        target.VerifyNoOtherCalls();
    }
}