using System.Windows;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers
{
    public class DeltaDragTest
    {
        private readonly Mock<ILocalDragger<Point>> target = new();

        [Fact]
        public void DeltaDrag()
        {
            var sut = LocalDragger.Delta(target.Object);
            sut.NewPoint(MouseMessageType.Down, new Point(10, 10));
            VerifyMove(MouseMessageType.Down, 0, 0);
            sut.NewPoint(MouseMessageType.Move, new Point(12, 10));
            VerifyMove(MouseMessageType.Move, 2, 0);
            sut.NewPoint(MouseMessageType.Move, new Point(8, 100));
            VerifyMove(MouseMessageType.Move, -4, 90);
            sut.NewPoint(MouseMessageType.Up, new Point(8, 100));
            VerifyMove(MouseMessageType.Up, 0, 0);
        }

        private void VerifyMove(MouseMessageType message, double x, double y)
        {
            target.Verify(i => i.NewPoint(message, new Point(x, y)));
            target.VerifyNoOtherCalls();
        }
    }
}