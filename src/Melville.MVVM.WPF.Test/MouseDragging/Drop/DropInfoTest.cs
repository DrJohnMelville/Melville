#nullable disable warnings
using  System.Windows;
using Melville.MVVM.Wpf.MouseDragging.Adorners;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.Drop
{
  public sealed class DropInfoTest
  {

    [Theory]
    [InlineData(0.0, DropAdornerKind.Left)]
    [InlineData(0.1, DropAdornerKind.Left)]
    [InlineData(0.49, DropAdornerKind.Left)]
    [InlineData(0.51, DropAdornerKind.Right)]
    [InlineData(0.71, DropAdornerKind.Right)]
    [InlineData(0.99, DropAdornerKind.Right)]
    [InlineData(1, DropAdornerKind.Right)]
    public void AdornLeftRight(double relativeX, DropAdornerKind adornerKind)
    {
      var query = new Mock<IDropQuery>();
      query.Setup(i => i.GetRelativeTargetLocation()).Returns(new Point(relativeX, 0.09));
      query.Object.AdornLeftRight();

      query.Verify(i=>i.AdornTarget(adornerKind), Times.Once);
    }

    [Theory]
    [InlineData(0.0, DropAdornerKind.Left)]
    [InlineData(0.1, DropAdornerKind.Left)]
    [InlineData(0.249, DropAdornerKind.Left)]
    [InlineData(0.251, DropAdornerKind.Rectangle)]
    [InlineData(0.51, DropAdornerKind.Rectangle)]
    [InlineData(0.749, DropAdornerKind.Rectangle)]
    [InlineData(0.7511, DropAdornerKind.Right)]
    [InlineData(0.99, DropAdornerKind.Right)]
    [InlineData(1, DropAdornerKind.Right)]
    public void AdornLeftRightMiddle(double relativeX, DropAdornerKind adornerKind)
    {
      var query = new Mock<IDropQuery>();
      query.Setup(i => i.GetRelativeTargetLocation()).Returns(new Point(relativeX, 0.09));
      query.Object.AdornLeftRightMiddle();

      query.Verify(i=>i.AdornTarget(adornerKind), Times.Once);
    }

    [Theory]
    [InlineData(0.0,  DropAdornerKind.Top)]
    [InlineData(0.1,  DropAdornerKind.Top)]
    [InlineData(0.49, DropAdornerKind.Top)]
    [InlineData(0.51, DropAdornerKind.Bottom)]
    [InlineData(0.71, DropAdornerKind.Bottom)]
    [InlineData(0.99, DropAdornerKind.Bottom)]
    [InlineData(1, DropAdornerKind.Bottom)]
    public void AdornTopBottomTest(double relativeY, DropAdornerKind adornerKind)
    {
      var query = new Mock<IDropQuery>();
      query.Setup(i => i.GetRelativeTargetLocation()).Returns(new Point(0.43, relativeY));
      query.Object.AdornTopBottom();

      query.Verify(i=>i.AdornTarget(adornerKind), Times.Once);
    }

    [Theory]
    [InlineData(0.0, DropAdornerKind.Top)]
    [InlineData(0.1, DropAdornerKind.Top)]
    [InlineData(0.249, DropAdornerKind.Top)]
    [InlineData(0.251, DropAdornerKind.Rectangle)]
    [InlineData(0.51, DropAdornerKind.Rectangle)]
    [InlineData(0.749, DropAdornerKind.Rectangle)]
    [InlineData(0.7511, DropAdornerKind.Bottom)]
    [InlineData(0.99, DropAdornerKind.Bottom)]
    [InlineData(1, DropAdornerKind.Bottom)]
    public void AdornTopBottomMiddle(double relativey, DropAdornerKind adornerKind)
    {
      var query = new Mock<IDropQuery>();
      query.Setup(i => i.GetRelativeTargetLocation()).Returns(new Point(0.01, relativey));
      query.Object.AdornTopBottomMiddle();

      query.Verify(i=>i.AdornTarget(adornerKind), Times.Once);
    }
  }
}