using Melville.TestHelpers.StringDatabase;
using Xunit;

namespace Melville.TestHelpers.Test.StringDatabase;

public class SiblingToCodeFileTest
{
    [Fact]
    public void LoadSiblingText()
    {
        Assert.Equal("This is a Test.", SiblingToCodeFile.AsUtf8String("TestData.txt"));
    }

    [Fact]
    public void LoadSiblingBytes()
    {
        var data = SiblingToCodeFile.AsBytes("TestData.txt");
        Assert.Equal(18, data.Length);
        Assert.Equal(32, data[7]);
    }

}