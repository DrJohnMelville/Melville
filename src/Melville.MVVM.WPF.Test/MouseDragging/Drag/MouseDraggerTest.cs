#nullable disable warnings
using  System.Windows;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.Drag;

public sealed class MouseDraggerTest
{
  [Fact]
  public void TestSimpleDrag()
  {
    var mouse = new Mock<IMouseDataSource>();
    var target = new Mock<ILocalDragger<Point>>();
      
    mouse.Object.BindLocalDragger(target.Object);
      
    target.Verify(i=>i.NewPoint(MouseMessageType.Down, new Point()));
    target.VerifyNoOtherCalls();
      
    mouse.Raise(i=>i.MouseMoved += null, MouseMessageType.Move, new Point(0.5, 0.7));

    target.Verify(i=>i.NewPoint(MouseMessageType.Move, new Point(0.5,0.7)));
    target.VerifyNoOtherCalls();
  }
}