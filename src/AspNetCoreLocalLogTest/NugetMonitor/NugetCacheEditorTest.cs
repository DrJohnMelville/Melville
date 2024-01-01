using FluentAssertions;
using Melville.FileSystem;
using Melville.Log.Viewer.NugetMonitor;
using Melville.MVVM.BusinessObjects;
using Melville.Mvvm.TestHelpers.MockFiles;
using Moq;
using Xunit;

namespace AspNetCoreLocalLogTest.NugetMonitor;

public class NugetCacheEditorTest
{
    private readonly Mock<ILogConsole> console = new();
    private readonly MockDirectory cache = new MockDirectory("e:\\dir\\path");
    private readonly NugetCacheEditor sut;
    private readonly IDirectory package;
    private readonly IDirectory version;
    private readonly IDirectory subdir;
    private readonly IFile file;

    public NugetCacheEditorTest()
    {
        sut = new NugetCacheEditor(console.Object, (__, _) => cache);
        package = cache.SubDirectory("A.Package");
        package.Create();
        version = package.SubDirectory("2.3.4-preview5");
        version.Create();
        version.File("A.Package.2.3.4-preview5.nupkg").Create("ssss");
        subdir = version.SubDirectory("bin");
        subdir.Create();
        file = subdir.File("binary.dll");
        file.Create("lkhdjkghbf");

    }

    [Fact]
    public void NotifyResultsInPackageDeletion()
    {
        file.Exists().Should().BeTrue();

        sut.UpdateLibrary("A.Package.2.3.4-preview5.nupkg");
        file.Exists().Should().BeFalse();
        version.Exists().Should().BeFalse();
    }
    [Fact]
    public void FailDeletionWithoutPackageCopy()
    {
        file.Exists().Should().BeTrue();
        foreach (var allFile in version.AllFiles())
        {
            allFile.Delete();
        }
        sut.UpdateLibrary("A.Package.2.3.4-preview5.nupkg");
        file.Exists().Should().BeTrue();
        version.Exists().Should().BeTrue();
    }
    [Fact]
    public void NoDeleteDueToWrongPackageName()
    {
        sut.UpdateLibrary("A.Package.2.3.4-preview6.nupkg");
        file.Exists().Should().BeTrue();
        version.Exists().Should().BeTrue();
    }
}