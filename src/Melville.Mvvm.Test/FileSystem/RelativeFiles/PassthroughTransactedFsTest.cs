#nullable disable warnings
using Melville.FileSystem;
using Melville.FileSystem.PseudoTransactedFS;
using Melville.Mvvm.TestHelpers.MockFiles;
using Xunit;

namespace Melville.Mvvm.Test.FileSystem.RelativeFiles;

public sealed class PassthroughTransactedFsTest
{
    private readonly IDirectory source = new MockDirectory("S:\\aaa");
    private readonly UntransactedStore sut;

    public PassthroughTransactedFsTest()
    {
        sut = new UntransactedStore(source);
    }

    [Fact]
    public void GetWriteToken() => Assert.Equal(source.SubDirectory("Audio").WriteToken(), sut.WriteToken());

    [Fact]
    public void GetUntransactedRoot() => Assert.Equal(source, sut.UntransactedRoot);
    [Fact]
    public void IsLocalStore() => Assert.True(sut.IsLocalStore);
    [Fact]
    public void ProgressStore() => Assert.Null(sut.ProgressStore);

    [Fact]
    public void GetTransactedDirectory()
    {
        var trans = sut.BeginTransaction();
        Assert.True(trans is PassthroughTransactedDirectory);
        Assert.Equal("S:\\aaa", trans.Path);
      
    }

}