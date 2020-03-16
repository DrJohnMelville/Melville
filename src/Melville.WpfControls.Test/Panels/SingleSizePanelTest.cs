#nullable disable warnings
using  System.Windows;
using Melville.WpfControls.Panels;
using Moq;
using Xunit;

namespace Melville.WpfControls.Test.Panels
{
  public sealed class SingleSizePanelTest
  {
    private readonly Mock<IPanelAdapter> panelAdaptor = new Mock<IPanelAdapter>();
    private readonly SingleSizePanel sut = new SingleSizePanel();

    [StaFact]
    public void FitsExactly()
    {
      panelAdaptor.SetupGet(i => i.ChildrenCount).Returns(3);
      panelAdaptor.Setup(i => i.GetDesiredSize(0)).Returns(new Size(100, 200));
      panelAdaptor.Setup(i => i.GetDesiredSize(1)).Returns(new Size(10, 20));
      panelAdaptor.Setup(i => i.GetDesiredSize(2)).Returns(new Size(1000, 2000));

      Assert.Equal(new Size(100, 200), sut.MeasureOverride(panelAdaptor.Object, new Size(100, 200)));
      Assert.Equal(new Size(100, 200), sut.ArrangeOverride(panelAdaptor.Object, new Size(100, 200)));

      panelAdaptor.Verify(i=>i.Measure(0, new Size(100, 200)));
      panelAdaptor.Verify(i=>i.Measure(1, new Size(100, 200)));
      panelAdaptor.Verify(i=>i.Measure(2, new Size(100, 200)));
      
      panelAdaptor.Verify(i=>i.Arrange(0, new Rect(0,0,100,200)));
      panelAdaptor.Verify(i=>i.Arrange(1, new Rect(0,0,100,200)));
      panelAdaptor.Verify(i=>i.Arrange(2, new Rect(0,0,100,200)));
    }

    [StaFact]
    public void Centered()
    {
      panelAdaptor.SetupGet(i => i.ChildrenCount).Returns(3);
      panelAdaptor.Setup(i => i.GetDesiredSize(0)).Returns(new Size(100, 200));
      panelAdaptor.Setup(i => i.GetDesiredSize(1)).Returns(new Size(10, 20));
      panelAdaptor.Setup(i => i.GetDesiredSize(2)).Returns(new Size(1000, 2000));

      Assert.Equal(new Size(100,200), sut.MeasureOverride(panelAdaptor.Object, new Size(300, 300)));
      Assert.Equal(new Size(300,300), sut.ArrangeOverride(panelAdaptor.Object, new Size(300, 300)));

      panelAdaptor.Verify(i=>i.Measure(0, new Size(300, 300)));
      panelAdaptor.Verify(i=>i.Measure(1, new Size(100, 200)));
      panelAdaptor.Verify(i=>i.Measure(2, new Size(100, 200)));
      
      panelAdaptor.Verify(i=>i.Arrange(0, new Rect(100,50,100,200)));
      panelAdaptor.Verify(i=>i.Arrange(1, new Rect(100,50,100,200)));
      panelAdaptor.Verify(i=>i.Arrange(2, new Rect(100,50,100,200)));
    }

    [StaFact]
    public void OffsetButNotApplied()
    {
      panelAdaptor.SetupGet(i => i.ChildrenCount).Returns(3);
      panelAdaptor.Setup(i => i.GetDesiredSize(0)).Returns(new Size(100, 200));
      panelAdaptor.Setup(i => i.GetDesiredSize(1)).Returns(new Size(10, 20));
      panelAdaptor.Setup(i => i.GetDesiredSize(2)).Returns(new Size(1000, 2000));

      sut.Offset = new Thickness(.25, .25, .25, .25);

      Assert.Equal(new Size(100,200), sut.MeasureOverride(panelAdaptor.Object, new Size(300, 300)));
      Assert.Equal(new Size(300,300), sut.ArrangeOverride(panelAdaptor.Object, new Size(300, 300)));

      panelAdaptor.Verify(i=>i.Measure(0, new Size(300, 300)));
      panelAdaptor.Verify(i=>i.Measure(1, new Size(100, 200)));
      panelAdaptor.Verify(i=>i.Measure(2, new Size(100, 200)));
      
      panelAdaptor.Verify(i=>i.Arrange(0, new Rect(100,50,100,200)));
      panelAdaptor.Verify(i=>i.Arrange(1, new Rect(100,50,100,200)));
      panelAdaptor.Verify(i=>i.Arrange(2, new Rect(100,50,100,200)));
    }

    [StaFact]
    public void OffsetApplied()
    {
      panelAdaptor.SetupGet(i => i.ChildrenCount).Returns(3);
      panelAdaptor.Setup(i => i.GetDesiredSize(0)).Returns(new Size(100, 200));
      panelAdaptor.Setup(i => i.GetDesiredSize(1)).Returns(new Size(10, 20));
      panelAdaptor.Setup(i => i.GetDesiredSize(2)).Returns(new Size(1000, 2000));

      sut.Offset = new Thickness(.25, .15, .75, .85);
      sut.ApplyOffset = true;

      Assert.Equal(new Size(100,200), sut.MeasureOverride(panelAdaptor.Object, new Size(300, 300)));
      Assert.Equal(new Size(300,300), sut.ArrangeOverride(panelAdaptor.Object, new Size(300, 300)));

      panelAdaptor.Verify(i=>i.Measure(0, new Size(300, 300)));
      panelAdaptor.Verify(i=>i.Measure(1, new Size(100, 200)));
      panelAdaptor.Verify(i=>i.Measure(2, new Size(100, 200)));
      
      panelAdaptor.Verify(i=>i.Arrange(0, new Rect(100,50,100,200)));
      panelAdaptor.Verify(i=>i.Arrange(1, new Rect(125,80,50,140)));
      panelAdaptor.Verify(i=>i.Arrange(2, new Rect(125,80,50,140)));
    }

  }
}