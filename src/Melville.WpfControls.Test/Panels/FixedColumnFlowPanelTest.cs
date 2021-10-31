#nullable disable warnings
using  System.Windows;
using Melville.WpfControls.Panels;
using Moq;
using Xunit;

namespace Melville.WpfControls.Test.Panels;

public sealed class FixedColumnFlowPanelTest
{
  private readonly Mock<IPanelAdapter> panelAdaptor = new Mock<IPanelAdapter>();
  public FixedColumnFlowPanel sut = new FixedColumnFlowPanel();

  public FixedColumnFlowPanelTest()
  {
    panelAdaptor.SetupGet(i => i.ChildrenCount).Returns(10);
    for (int i = 1; i <= 10; i++)
    {
      panelAdaptor.Setup(j => j.GetDesiredSize(i-1)).Returns(new Size(i * 10, i * 20));
    }
  }

  [StaFact]
  public void TwoRowsOfFive()
  {
    sut.Columns = 5;
    Assert.Equal(new Size(100, 300), sut.MeasureOverride(panelAdaptor.Object, new Size(100, 10000)));
    Assert.Equal(new Size(100, 300), sut.ArrangeOverride(panelAdaptor.Object, new Size(100, 10000)));

    panelAdaptor.Verify(i=>i.Arrange(0, new Rect(0, 0, 20, 100)));
    panelAdaptor.Verify(i=>i.Arrange(1, new Rect(20, 0, 20, 100)));
    panelAdaptor.Verify(i=>i.Arrange(2, new Rect(40, 0, 20, 100)));
    panelAdaptor.Verify(i=>i.Arrange(3, new Rect(60, 0, 20, 100)));
    panelAdaptor.Verify(i=>i.Arrange(4, new Rect(80, 0, 20, 100)));
    panelAdaptor.Verify(i=>i.Arrange(5, new Rect(00, 100, 20, 200)));
    panelAdaptor.Verify(i=>i.Arrange(6, new Rect(20, 100, 20, 200)));
    panelAdaptor.Verify(i=>i.Arrange(7, new Rect(40, 100, 20, 200)));
    panelAdaptor.Verify(i=>i.Arrange(8, new Rect(60, 100, 20, 200)));
    panelAdaptor.Verify(i=>i.Arrange(9, new Rect(80, 100, 20, 200)));
  }

}