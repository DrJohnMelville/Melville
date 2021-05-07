using System.Windows;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers
{
    public class MouseDataSourceBinderTest
    {
        private readonly Mock<IMouseDataSource> source = new();
        private readonly Mock<ILocalDragger<Point>> destination = new();
        
        [Fact]
        public void TransmitMessage()
        {
            source.Object.BindLocalDragger(destination.Object);
            source.Raise(i=>i.MouseMoved += null, MouseMessageType.Down, new Point(23,14));
            
            destination.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point(23,14)));
        }
    }
    
}