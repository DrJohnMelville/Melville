using System.Collections.Generic;
using System.Windows;
using Melville.MVVM.Undo;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers;

public class UndoDraggerTest
{
    private UndoEngine undo = new();
    private readonly Mock<ILocalDragger<Point>> target = new();
        
    [Fact]
    public void UndoDragger()
    {
        var sut = LocalDragger.Undo(undo, target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(1,2));
        sut.NewPoint(MouseMessageType.Move, new Point(1,3));
        sut.NewPoint(MouseMessageType.Move, new Point(1,3));
        sut.NewPoint(MouseMessageType.Up, new Point(1,4));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(1,2)), Times.Once);
        target.Verify(i=>i.NewPoint(MouseMessageType.Move, new Point(1,3)), Times.Exactly(2));
        target.Verify(i=>i.NewPoint(MouseMessageType.Up, new Point(1,4)), Times.Once);
        target.VerifyNoOtherCalls();
        undo.Undo();
        target.Verify(i=>i.NewPoint(MouseMessageType.Up, new Point(1,2)), Times.Once);
        target.VerifyNoOtherCalls();
        undo.Redo();
        target.Verify(i=>i.NewPoint(MouseMessageType.Up, new Point(1,4)), Times.Exactly(2));
        target.VerifyNoOtherCalls();
    }
}