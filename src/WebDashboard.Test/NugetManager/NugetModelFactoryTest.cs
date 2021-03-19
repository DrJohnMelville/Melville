using System;
using System.Linq;
using System.Threading.Tasks;
using Melville.MVVM.FileSystem;
using Melville.Mvvm.TestHelpers.MockFiles;
using WebDashboard.NugetManager;
using Xunit;

namespace WebDashboard.Test.NugetManager
{
    public class NugetModelFactoryTest
    {
        private readonly MockDirectory root = new("Z:\\md");
        private readonly NugetModelFactory sut = new();

        public NugetModelFactoryTest()
        {
            
        }

        private IFile BuildPropFile(string version)
        {
            var bp = root.File("Directory.Build.props");
            bp.Create($"<Project><Version>{version}</Version></Project>");
            return bp;
        }

        [Theory]
        [InlineData("3.21.1")]
        [InlineData("1.2.3")]
        public async Task ParsesVersionString(string version)
        {
            var model = await sut.Create(BuildPropFile(version));
            Assert.Equal(version, model.Version);
        }

        private void CreateProjectDir(string dirName, string fileName, string gpob, params string[] deps)
        {
            var projDir = root.SubDirectory(dirName);
            projDir.Create();
            var bindir = projDir.SubDirectory("bin");
            bindir.Create();
            var relDir = bindir.SubDirectory("Release");
            relDir.Create();
            var projFile = projDir.File(fileName);
            projFile.Create(
                $"<Project><PropertyGroup><GeneratePackageOnBuild>{gpob}</GeneratePackageOnBuild>" +
                string.Join(Environment.NewLine, deps.Select(i=>$"<ProjectReference Include=\"{i}\"/>"))+
                "</PropertyGroup></Project>");
        }

        [Theory]
        [InlineData("Sample.Project", "Sample.Project.csproj", "true", 1)]
        [InlineData("Sample.Project", "Sample.Project.csproj", "false", 0)]
        [InlineData("Sample.NoMatch", "Sample.Project.csproj", "true", 0)]
        public async Task FindsProjectThatMakesNugetPackage(string dirName,string fileName, string gpob, int files)
        {
            CreateProjectDir(dirName, fileName, gpob);
            var model = await sut.Create(BuildPropFile("1.2.3")); 
            Assert.Equal(files, model.Files.Count);
        }

        [Fact]
        public async Task FindMultipleFiles()
        {
            CreateProjectDir("Project.A", "Project.A.csproj", "true");
            CreateProjectDir("Project.B", "Project.B.csproj", "true");
            CreateProjectDir("Project.C", "Project.C.csproj", "true");
            var model = await sut.Create(BuildPropFile("1.2.3"));
            Assert.Equal(3, model.Files.Count);
            Assert.Equal(@"Z:\md\Project.A\bin\Release\Project.A.1.2.3.nupkg", 
                model.Files[0].Package("1.2.3")?.Path);
            
        }

        [Fact]
        public async Task SimpleDependency()
        {
            CreateProjectDir("Project.A", "Project.A.csproj", "true");
            CreateProjectDir("Project.B", "Project.B.csproj", "true", "..\\Project.A\\Project.A.csproj");
            var model = await sut.Create(BuildPropFile("1.2.3"));
            Assert.Single(model.Files[1].DependsOn);
        }
    }
}