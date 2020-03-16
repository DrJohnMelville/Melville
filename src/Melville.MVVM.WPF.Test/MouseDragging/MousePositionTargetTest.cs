#nullable disable warnings
using  System.Windows;
using System.Windows.Input;
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
      new LocalDragEventArgs(new Point(pt.X, pt.Y), type, new Size(100, 1000), MouseButton.Left,
        null);

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

    [Fact]
    public void BindTest()
    {
      sut.Bind(Recorder.Object.Point);
      VerifySequence(new(double X, double Y)[] { (10, 20), (30, 40), (130, 20) },
        new(double X, double Y)[] { (10, 20), (30, 40), (130, 20) }
      );
    }


    [Fact]
    public void DeltaDragTest()
    {
      sut.AsDeltas()
        .Bind(Recorder.Object.Point);

      VerifySequence(new (double X, double Y)[]{(10,20),(30,40),(130, 20)},
        new (double X, double Y)[]{(0,0),(20,20),(100, -20)}
      );
    }
    [Fact]
    public void TransformTest()
    {
      sut.Transform((x,y)=>(x*10, y /2))
        .Bind(Recorder.Object.Point);

      VerifySequence(new (double X, double Y)[]{(10,20),(30,40),(130, 20)},
        new (double X, double Y)[]{(100,10),(300,20),(1300, 10)}
      );
    }

    [Fact]
    public void RelativeTest()
    {
      sut.RelativeToTarget()
        .Bind(Recorder.Object.Point);

      VerifySequence(new (double X, double Y)[]{(10,20),(30,40),(50, 60)},
        new (double X, double Y)[]{(0.1, 0.02),(0.3, 0.04),(0.5, 0.06)}
      );
    }

    [Fact]
    public void InvertXTest()
    {
      sut.InvertX()
        .Bind(Recorder.Object.Point);

      VerifySequence(new (double X, double Y)[]{(10,20),(30,40),(50, 60)},
        new (double X, double Y)[]{(90, 20),(70, 40),(50, 60)}
      );
    }

    [Fact]
    public void InvertYTest()
    {
      sut.InvertY()
        .Bind(Recorder.Object.Point);

      VerifySequence(new (double X, double Y)[]{(10,20),(30,40),(50, 60)},
        new (double X, double Y)[]{(10, 980),(30, 960),(50, 940)}
      );
    }

    [Fact]
    public void FieldTest()
    {
      sut.TranslateInitialPoint(50,50)
        .Bind(Recorder.Object.Point);

      VerifySequence(new (double X, double Y)[]{(10,20),(30,40),(50, 60)},
        new (double X, double Y)[]{(50,50),(70, 70),(90,90)}
      );
    }

    [Fact]
    public void RestrictToTarget()
    {
      sut.RestrictToTarget()
        .Bind(Recorder.Object.Point);

      VerifySequence(new (double X, double Y)[]{(10,20),(-30,-40),(10000, 100000)},
        new (double X, double Y)[]{(10,20),(0,0),(100,1000)}
      );
    }
    [Fact]
    public void RestrictToRange()
    {
      sut.RestrictToRange(5,7, 15, 96)
        .Bind(Recorder.Object.Point);

      VerifySequence(new (double X, double Y)[]{(10,20),(-30,-40),(10000, 100000)},
        new (double X, double Y)[]{(10,20),(5,7),(15,96)}
      );
    }

    [Fact]
    public void RequireInitialDragDelta()
    {
      sut.RequireInitialDelta(10,10)
        .Bind(Recorder.Object.Point);

      VerifySequence(
        new (double X, double Y)[] {(0, 0), (-7, 7), (2, -5), (12, 3), (4, 4)},
        new (double X, double Y)[] {(0, 0), (12, 3), (4, 4)});
    }


  }
}