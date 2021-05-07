#nullable disable warnings
using  System.Windows;
using Melville.MVVM.Wpf.MouseDragging;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging
{
  public sealed class MousePositionTargetTest
  {
    private Mock<IMouseDataSource> sutMock = new Mock<IMouseDataSource>();
    private IMouseDataSource sut => sutMock.Object;

    public interface IMouseRecorder
    {
      void Point(double x, double y);
    }

    private LocalDragEventArgs CreateLdea(MouseMessageType type, (double X, double Y) pt) =>
      new LocalDragEventArgs(new Point(pt.X, pt.Y), type);

    private readonly Mock<IMouseRecorder> Recorder = new Mock<IMouseRecorder>();

    private void VerifySequence((double X, double Y)[] inputs, (double X, double Y)[] outputs)
    {
      for (int i = 0; i < inputs.Length; i++)
      {
        sutMock.Raise(j=>j.MouseMoved += null, CreateLdea(MouseMessageType(i), inputs[i]));

      }

      for (int i = 0; i < outputs.Length; i++)
      {
        Recorder.Verify(j => j.Point(outputs[i].X, outputs[i].Y), Times.Once);

      }

      MouseMessageType MouseMessageType(int i)
      {
        return i == 0 ? Wpf.MouseDragging.MouseMessageType.Down :
          (i >= (inputs.Length - 1)) ? Wpf.MouseDragging.MouseMessageType.Up : Wpf.MouseDragging.MouseMessageType.Move;
      }
    }
  }
}