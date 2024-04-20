using DependencyMapper.Model;
using DependencyMapper.ModelFactory;
using Melville.FileSystem;
using Melville.Mvvm.TestHelpers.MockFiles;

namespace DependencyMapper.Test;

public class DependencyTest
{
    private readonly List<Dependency> items = new();
    private readonly ProjectReader sut;
    private IDirectory PackageRoot = new MockDirectoryTreeBuilder("X:\\yyy")
        .Folder("c.d", p =>
            p.Folder("1.2.3", v=>v.File("c.d.nuspec", """
                <?xml version="1.0" encoding="utf-8" standalone="yes"?>
                <package xmlns="http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd">
                </package>
                """)))
        .Folder("e.f", p =>
            p.Folder("5.6-preview3", v=>v.File("e.f.nuspec", """
                <?xml version="1.0" encoding="utf-8" standalone="yes"?>
                <package xmlns="http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd">
                    <metadata>
                        <dependencies>
                            <dependency id="c.d" version="1.2.3"/>
                        </dependencies>
                    </metadata>
                </package>
                """)))
        .Object;

    public DependencyTest()
    {
        sut = new ProjectReader(items, PackageRoot);
    }

    [Fact]
    public async Task ProjectReferenceNamedAfterProject()
    {
        var source = new MockFile("ProjName.csproj", new MockDirectory("X:\\"));
        source.Create("<Project></Project>");

        var proj = await sut.ReadProject(source);

        proj.Title.Should().Be("ProjName");
    }

    [Fact]
    public async Task ProjectToPackageReference()
    {
        var dir = new MockDirectoryTreeBuilder("C:\\Solution")
            .Folder("a", a => a.File("a.csproj", """
                <Project>
                    <ItemGroup>
                        <PackageReference Include="c.d" Version="1.2.3"/>
                    </ItemGroup>
                </Project>
                """))
            .Object;
        var proj = await sut.ReadProject(dir.FileAtRelativePath("a\\a.csproj")!);

        proj.Dependencies.Should().HaveCount(1);
        proj.Dependencies[0].Title.Should().Be("c.d (1.2.3)");
    }
    [Fact]
    public async Task PackageToPackageReference()
    {
        var dir = new MockDirectoryTreeBuilder("C:\\Solution")
            .Folder("a", a => a.File("a.csproj", """
                <Project>
                    <ItemGroup>
                        <PackageReference Include="e.f" Version="5.6-preview3"/>
                    </ItemGroup>
                </Project>
                """))
            .Object;
        var proj = await sut.ReadProject(dir.FileAtRelativePath("a\\a.csproj")!);

        proj.Dependencies.Should().HaveCount(1);
        var firstDep = proj.Dependencies[0];
        firstDep.Title.Should().Be("e.f (5.6-preview3)");
        firstDep.Dependencies.Should().HaveCount(1);
    }

    [Fact]
    public async Task ProjectToProjectReference()
    {
        var dir = new MockDirectoryTreeBuilder("C:\\Solution")
            .Folder("a", a => a.File("a.csproj", """
                <Project>
                    <ItemGroup>
                        <ProjectReference Include="..\b\b.csproj"/>
                    </ItemGroup>
                </Project>
                """))
            .Folder("b", b => b.File("b.csproj", "<Project></Project>"))
            .Object;
        var proj = await sut.ReadProject(dir.FileAtRelativePath("a\\a.csproj")!);

        proj.Dependencies.Should().HaveCount(1);
        proj.Dependencies[0].Title.Should().Be("b");
    }

    [Fact]
    public async Task ProjectToProjectNotFoundReference()
    {
        var dir = new MockDirectoryTreeBuilder("C:\\Solution")
            .Folder("a", a => a.File("a.csproj", """
                <Project>
                    <ItemGroup>
                        <ProjectReference Include="..\b\b.csproj"/>
                    </ItemGroup>
                </Project>
                """))
            .Object;
        var proj = await sut.ReadProject(dir.FileAtRelativePath("a\\a.csproj")!);

        proj.Dependencies.Should().HaveCount(1);
        proj.Dependencies[0].Title.Should().Be("b (Cannot Read)");
    }

    [Fact] 
    public async Task ProjectCircularReference()
    {
        var dir = new MockDirectoryTreeBuilder("C:\\Solution")
            .Folder("a", a => a.File("a.csproj", """
                <Project>
                    <ItemGroup>
                        <ProjectReference Include="..\b\b.csproj"/>
                    </ItemGroup>
                </Project>
                """))
            .Folder("b", b => b.File("b.csproj", """
            <Project>
                 <ProjectReference Include="..\a\a.csproj"/>
            </Project>
            """))
            .Object;
        var proj = await sut.ReadProject(dir.FileAtRelativePath("a\\a.csproj")!);

        proj.Dependencies.Should().HaveCount(1);
    }
}