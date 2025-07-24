using FluentAssertions;
using System.Windows;
using Melville.MVVM.Wpf.MouseDragging.DroppedFiles;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.Drag;

public class ComDataObjectTest
{
    private readonly ComDataObject target = new ComDataObject();
    private readonly DataObject dObj;

    public ComDataObjectTest()
    {
        dObj = new DataObject(target);
    }

    [Fact]
    public void PushAndEnumerateString()
    {
        target.SetText("Hello world");
        dObj.GetFormats(false).Should().BeEquivalentTo("UnicodeText");
        dObj.GetDataPresent("UnicodeText").Should().BeTrue();
        dObj.GetData("UnicodeText").Should().Be("Hello world");
    }
}