using  System.Windows.Controls;
using Melville.MVVM.Wpf.MouseDragging;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging
{
  public sealed class MouseDragTest
  {
    private readonly Border root = new Border {Name = "Root"};
    private readonly Grid middle = new Grid {Name = "Middle"};
    private readonly Border child = new Border {Name = "Child"};
    private readonly MouseDragger sut;

    public MouseDragTest()
    {
      root.Child = middle;
      middle.Children.Add(child);
      sut = new MouseDragger(child, null!);
    }

    [StaFact]
    public void FindImmediate()
    {
      var dragger = sut.LeafTarget();
      Assert.Equal(child, ((WindowMouseDataSource)dragger).Target);
    }
    [StaFact]
    public void FindByName()
    {
      var dragger = sut.NamedTarget("Middle");
      Assert.Equal(middle, ((WindowMouseDataSource)dragger).Target);
    }
    [StaFact]
    public void FindByType()
    {
      var dragger = sut.NamedTarget("Middle");
      Assert.Equal(middle, ((WindowMouseDataSource)dragger).Target);
    }
    [StaFact]
    public void FindTop()
    {
      var dragger = sut.TopTarget();
      Assert.Equal(root, ((WindowMouseDataSource)dragger).Target);
    }
  }
}