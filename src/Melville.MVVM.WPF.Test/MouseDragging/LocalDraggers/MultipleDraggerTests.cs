using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.KeyboardFacade;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Melville.MVVM.WPF.Test.EventBindings;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers;

public class MultipleDraggerTests
{
    private readonly Mock<ILocalDragger<Point>> target = new();

    [Theory]
    [InlineData(10,20)]
    public void InitialPointDragTest(double iX, double iY)
    {
        var dragger = LocalDragger.InitialPoint(iX, iY, target.Object);
        dragger.NewPoint(MouseMessageType.Down, new Point(345,55));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(iX, iY)));
        dragger.NewPoint(MouseMessageType.Move, new Point(350,65));
        target.Verify(i=>i.NewPoint(MouseMessageType.Move, new Point(iX+5, iY+10)));
    }

    [Theory]
    [InlineData(5, 10, 0, 10)]
    [InlineData(15, 10, 15, 0)]
    public void RestrictAxis(double iX, double iY, double fX, double fY)
    {
        var sut = new RestrictToAxis(target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(10,23));
        sut.NewPoint(MouseMessageType.Move, new Point(10+iX, 23 + iY));
        target.Verify(i=>i.NewPoint(MouseMessageType.Move, new Point(10+fX, 23+fY)));
    }

    [Theory]
    [InlineData(ModifierKeys.Shift, ModifierKeys.Shift, true)]
    [InlineData(ModifierKeys.Shift, ModifierKeys.None, false)]
    public void KeySwitchingDragger(ModifierKeys requried, ModifierKeys seen, bool useSecond)
    {
        var target2 = new Mock<ILocalDragger<Point>>();
        var keyboard = new Mock<IKeyboardQuery>();
        keyboard.SetupGet(i => i.Modifiers).Returns(seen);
        var sut = new KeySwitcherDragger<Point>(keyboard.Object, requried, 
            target2.Object, target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(1,2));
        sut.NewPoint(MouseMessageType.Move, new Point(2,2));
        sut.NewPoint(MouseMessageType.Up, new Point(3,2));
            
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(1,2)));
        target2.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(1,2)));
        var finalTarget = useSecond ? target2 : target;
        finalTarget.Verify(i=>i.NewPoint(MouseMessageType.Move, new Point(2,2)));
        finalTarget.Verify(i=>i.NewPoint(MouseMessageType.Up, new Point(3,2)));

        target.VerifyNoOtherCalls();
        target2.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(9,11,10,10)]
    [InlineData(7.6,12.4,10,10)]
    [InlineData(7.4,12.6,5,15)]
    public void SnapToGrid(double iX, double iY, double fX, double fY)
    {
        var sut = LocalDragger.GridSnapping(5, target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(iX,iY));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(fX,fY)));
        target.VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(7,25,7,25)]
    [InlineData(7,15,7,20)]
    [InlineData(7,19.99,7,20)]
    [InlineData(7,20.01,7,20.01)]
    [InlineData(7,24,7,24)]
    [InlineData(7,29.99,7,29.99)]
    [InlineData(7,30.01,7,30)]
    [InlineData(7,60,7,30)]
    [InlineData(1,25,5,25)]
    [InlineData(4.999,25,5,25)]
    [InlineData(5.02,25,5.02,25)]
    [InlineData(9.99,25,9.99,25)]
    [InlineData(10.1,25,10,25)]
    [InlineData(100,25,10,25)]
    public void ConstrainedDriver(double iX, double iY, double fX, double fY)
    {
        var sut = LocalDragger.Constrain(5,10,20,30, target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(iX,iY));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(fX,fY)));
        target.VerifyNoOtherCalls();
    }

    [Fact]
    public void ConstrainToUnitSquaere()
    {
        var sut = LocalDragger.ConstrainToUnitSquare(target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(5,5));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(1,1)));
        target.VerifyNoOtherCalls();
    }
        
    [Theory]
    [InlineData(5,1,5,9)]
    [InlineData(15,5,15,5)]
    [InlineData(15,9,15,1)]
    public void InvertYDragger(double iX, double iY, double fX, double fY)
    {
        var sut = LocalDragger.InvertY(10, target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(iX,iY));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(fX,fY)));
        target.VerifyNoOtherCalls();
    }
    [Theory]
    [InlineData(5,4,10,12)]
    public void ScaleDragger(double iX, double iY, double fX, double fY)
    {
        var sut = LocalDragger.ScaleDragger(2,3, target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(iX,iY));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(fX,fY)));
        target.VerifyNoOtherCalls();
    }
    [Fact]
    public void RelativeToSizeTest()
    {
        var sut = LocalDragger.RelativeToSize(new Size(10, 100), target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(5,5));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(0.5,0.05)));
        target.VerifyNoOtherCalls();
    }
    [Theory]
    [InlineData(5,4,10,12)]
    public void TransformDragger(double iX, double iY, double fX, double fY)
    {
        var sut = LocalDragger.Transform(pt=>new Point(pt.X *2, pt.Y * 3), target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(iX,iY));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(fX,fY)));
        target.VerifyNoOtherCalls();
    }

    [Fact]
    public void MaxMovesTest()
    {
        var sut = LocalDragger.MaxMoves(2, target.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(1,2));
        sut.NewPoint(MouseMessageType.Down, new Point(1,2));
        sut.NewPoint(MouseMessageType.Down, new Point(1,2));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(1,2)), Times.Exactly(2));
    }
    [Fact]
    public void MultuMove()
    {
        var target2 = new Mock<ILocalDragger<Point>>();
        var sut = LocalDragger.Multiple(target.Object, target2.Object);
        sut.NewPoint(MouseMessageType.Down, new Point(1,2));
        target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(1,2)), Times.Exactly(1));
        target2.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(1,2)), Times.Exactly(1));
    }
        
}