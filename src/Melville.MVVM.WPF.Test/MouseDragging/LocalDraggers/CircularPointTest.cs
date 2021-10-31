using System.Runtime.InteropServices;
using System.Windows;
using Melville.MVVM.Wpf.MouseDragging.LocalDraggers;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.LocalDraggers;

public class CircularPointTest
{
    [Theory]
    [InlineData(1, 1, 360-45)]
    [InlineData(1, -1, 45)]
    [InlineData(1, 0, 0)]
    [InlineData(0, 1, 270)]
    [InlineData(-1, 0, 180)]
    [InlineData(0, -1, 90)]
    public void CreateCPAngle(double x, double y, double angle) => 
        Assert.Equal(angle, 
            CircularPoint.FromVectors(new Vector(1,0), new Vector(x,y)).AngleInDegrees, 3);

    [Theory]
    [InlineData(3,4,5)]
    public void CreateCPLength(double x, double y, double length) => 
        Assert.Equal(length,
            CircularPoint.FromVectors(new Vector(1,0), new Vector(x,y)).Length, 3);

    [Fact]
    public void FromPoints()
    {
        var cirPt = CircularPoint.FromPoints(new Point(10, 10), new Point(10, 12));
        Assert.Equal(2, cirPt.Length, 2);
        Assert.Equal(270, cirPt.AngleInDegrees, 2);
    }
}