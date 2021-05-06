#nullable disable warnings
using  System.Windows;
using System.Windows.Input;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.Drag
{
  public sealed class MouseDraggerTest
  {
    [Fact]
    public void TestSimpleDrag()
    {
      int dragged = 0;

      var mouse = new Mock<IMouseDataSource>();
      
      mouse.Object.BindDragger(LocalDragger.Action((pt) =>
      {
        dragged++;
        Assert.Equal(0.5, pt.X);
        Assert.Equal(0.7, pt.Y);
      }));

      Assert.Equal(0, dragged);
      mouse.Raise(i=>i.MouseMoved += null, 
        new LocalDragEventArgs(new Point(0.5, 0.7), MouseMessageType.Move,
          new Size(), MouseButton.Left, null!));
      Assert.Equal(1, dragged);
    }
  }
}