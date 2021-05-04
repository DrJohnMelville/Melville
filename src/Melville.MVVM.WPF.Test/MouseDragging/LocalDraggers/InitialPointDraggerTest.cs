using System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.KeyboardFacade;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers
{
    public class InitialPointDraggerTest
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
        
    }
}