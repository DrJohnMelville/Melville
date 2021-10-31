using System.Windows;
using Melville.MVVM.Wpf.MouseDragging;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers;

public class LambdaDraggerTest
{
    [Fact]
    public void PassThrough()
    {
        var fired = 0;
        var ld = LocalDragger.Action((mmt, pt) =>
        {
            fired++;
            Assert.Equal(MouseMessageType.Up, mmt);
            Assert.Equal(new Point(1, 2), pt);
        });
        Assert.Equal(0, fired);
        ld.NewPoint(MouseMessageType.Up, new Point(1, 2));
        Assert.Equal(1, fired);
    }

    [Fact]
    public void Abbreviated()
    {
        var fired = 0;
        var ld = LocalDragger.Action( pt =>
        {
            fired++;
            Assert.Equal(new Point(1, 2), pt);
        });
        Assert.Equal(0, fired);
        ld.NewPoint(MouseMessageType.Up, new Point(1, 2));
        Assert.Equal(1, fired);
    }
}