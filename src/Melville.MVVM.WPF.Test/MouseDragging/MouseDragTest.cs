using System.Windows;
using  System.Windows.Controls;
using Melville.MVVM.Wpf.MouseClicks;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.VisualTreeLocations;
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

    private FrameworkElement GetTarget(IMouseClickReport mcr) => mcr.Target;

    [StaFact]
    public void FindByName() => Assert.Equal(middle, sut.AttachToName("Middle").Target);

    [StaFact]
    public void FindByType() => Assert.Equal(middle, sut.AttachToType(typeof(Grid)).Target);

    [StaFact]
    public void FindTop() => Assert.Equal(root, sut.AttachToTop().Target);
  }
}