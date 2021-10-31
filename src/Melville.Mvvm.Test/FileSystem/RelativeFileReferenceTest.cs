using Melville.FileSystem;
using Melville.Mvvm.TestHelpers.MockFiles;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem;

public class RelativeFileReferenceTest
{
    private readonly MockDirectory root = new("Z:\\");
    private readonly IFile baseFile;

    public RelativeFileReferenceTest()
    {
        var fdir = root.SubDirectory("Base");
        baseFile = fdir.File("A.txt");
        var sibDir = root.SubDirectory("Sib");
        sibDir.Create();
        var childDir = sibDir.SubDirectory("Child");
        childDir.Create();
    }
        
    [Theory]
    [InlineData("", null)]
    [InlineData("..\\..\\a\\b\\x\\b.txt", null)]
    [InlineData("b.txt", "Z:\\Base\\b.txt")]
    [InlineData("..\\b.txt", "Z:\\b.txt")]
    [InlineData("Sib\\b.txt", "Z:\\Base\\Sib\\b.txt")]
    [InlineData(".\\Sib\\Child\\b.txt", "Z:\\Base\\Sib\\Child\\b.txt")]
    [InlineData("..\\Base\\Sib\\.\\Child\\b.txt", "Z:\\Base\\Sib\\Child\\b.txt")]
    [InlineData(".\\Sib\\..\\Sib\\Child\\b.txt", "Z:\\Base\\Sib\\Child\\b.txt")]
    [InlineData("b.1234.txt", "Z:\\Base\\b.1234.txt")]
    public void RelativePathTest(string path, string result) =>
        Assert.Equal(result, baseFile.FileAtRelativePath(path)?.Path);

        
}