#nullable disable warnings
using  System.Windows;
using Melville.MVVM.Wpf.MouseDragging;
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

      var mouse = new TestMouseDataSource();
      
      mouse.Bind((x, y) =>
      {
        dragged++;
        Assert.Equal(0.5, x);
        Assert.Equal(0.7, y);
      });

      Assert.Equal(0, dragged);
      mouse.SendMousePosition(MouseMessageType.Move, new Point(0.5, 0.7));
      Assert.Equal(1, dragged);
    }

    [Fact]
    public void BeginDrag()
    {
      var mouse = new TestMouseDataSource();

      var dataSource = new Mock<IDataObject>();
      mouse.Drag(dataSource.Object, DragDropEffects.Move);

      Assert.Null(mouse.DataObject);
      Assert.Equal(DragDropEffects.None, mouse.EffectsIn);

      mouse.EffectsOut = DragDropEffects.Copy;

      mouse.SendMousePosition(MouseMessageType.Down, new Point(0.5,0.5));

      Assert.Equal(dataSource.Object, mouse.DataObject);
      Assert.Equal(DragDropEffects.Move, mouse.EffectsIn);
      Assert.Equal(0.5, mouse.DragArgs.TransformedPoint.X);
      Assert.Equal(0.5, mouse.DragArgs.TransformedPoint.Y);
      
    }

  }
}