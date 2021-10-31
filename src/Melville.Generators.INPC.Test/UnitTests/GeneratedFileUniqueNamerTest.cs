using Melville.Generators.INPC.CodeWriters;
using Xunit;

namespace Melville.Generators.INPC.Test.UnitTests;

public class GeneratedFileUniqueNamerTest
{
    private readonly GeneratedFileUniqueNamer sut = new ("Generated");

    [Fact]
    public void SimpleName()
    {
        Assert.Equal("Foo.Generated.cs", sut.CreateFileName("Foo"));
    }

    [Fact] public void DuplicateNamesGetNumbers()
    {
        Assert.Equal("Foo.Generated.cs", sut.CreateFileName("Foo"));
        Assert.Equal("Foo.Generated.1.cs", sut.CreateFileName("Foo"));
        Assert.Equal("Foo.Generated.2.cs", sut.CreateFileName("Foo"));
        Assert.Equal("Foo.Generated.3.cs", sut.CreateFileName("Foo"));
    }
    [Fact]
    public void UniqueNamesDoNotGetNumbered()
    {
        Assert.Equal("Foo.Generated.cs", sut.CreateFileName("Foo"));
        Assert.Equal("Bar.Generated.cs", sut.CreateFileName("Bar"));
    }

}