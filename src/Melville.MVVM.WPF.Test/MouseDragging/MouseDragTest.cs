using  System.Windows.Controls;
using Melville.MVVM.Wpf.MouseClicks;
using Melville.MVVM.Wpf.MouseDragging;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging
{
  public sealed class MouseDragTest
  {
    private readonly Border root = new Border {Name = "Root"};
    private readonly Grid middle = new Grid {Name = "Middle"};
    private readonly Border child = new Border {Name = "Child"};
    private readonly MouseClickReport sut;

    public MouseDragTest()
    {
      root.Child = middle;
      middle.Children.Add(child);
      sut = new MouseClickReport(child, null!);
    }

    [StaFact]
    public void FindImmediate()
    {
      var dragger = sut.DragLeaf();
      Assert.Equal(child, ((WindowMouseDataSource)dragger).Target);
    }
    [StaFact]
    public void FindByName()
    {
      var dragger = sut.DragByName("Middle");
      Assert.Equal(middle, ((WindowMouseDataSource)dragger).Target);
    }
    [StaFact]
    public void FindByType()
    {
      var dragger = sut.DragByViewType<Grid>();
      Assert.Equal(middle, ((WindowMouseDataSource)dragger).Target);
    }
    [StaFact]
    public void FindTop()
    {
      var dragger = sut.DragTop();
      Assert.Equal(root, ((WindowMouseDataSource)dragger).Target);
    }
  }
}