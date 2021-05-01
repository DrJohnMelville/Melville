using System.Windows;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers
{
    public class MinimumDraggerTest
    {
        private readonly Mock<ILocalDragger<Point>> target = new();
        private readonly MinimumDragger sut;

        public MinimumDraggerTest()
        {
            sut = new MinimumDragger(10, target.Object);
        }

        [Theory]
        [InlineData(0, 0, -233, 0, true)]
        [InlineData(0, 0, -10.1, 0, true)]
        [InlineData(0, 0, -9.9, 0, false)]
        [InlineData(0, 0, -1, 0, false)]
        [InlineData(0, 0, 0, 0, false)]
        [InlineData(0, 0, 3, 0, false)]
        [InlineData(0, 0, 9.9, 0, false)]
        [InlineData(0, 0, 23789, 0, true)]
        [InlineData(0, 0, 0, -233, true)]
        [InlineData(0, 0, 0, -10.1, true)]
        [InlineData(0, 0, 0, -9.9, false)]
        [InlineData(0, 0, 0, -1, false)]
        [InlineData(0, 0, 0, 3, false)]
        [InlineData(0, 0, 0, 9.9, false)]
        [InlineData(0, 0, 0, 23789, true)]
        public void MinimumTest(double iX, double iY, double fX, double fY, bool triggered)
        {
            sut.NewPoint(MouseMessageType.Down, new Point(iX, iY));
            sut.NewPoint(MouseMessageType.Move, new Point(fX,fY));
            if (triggered)
            {
                target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(iX,iY)));
                target.Verify(i=>i.NewPoint(MouseMessageType.Move, new Point(fX,fY)));
            }
            target.VerifyNoOtherCalls();
        }
    }
}