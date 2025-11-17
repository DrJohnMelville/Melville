using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.Mvvm.TestHelpers.MockFiles;
using Moq;
using WebDashboard.NugetManager;
using Xunit;

namespace WebDashboard.Test.NugetManager;

public class NugetDeploymentCommandsTest
{
    private readonly Mock<INugetViewModel> viewModel = new();
    private readonly List<IFile> files = new()
    {
        new MockFile("C:\\x.nuspec"),
        new MockFile("C:\\y.nuspec"),
        new MockFile("C:\\z.nuspec"),
    };

    private readonly NugetDeploymentCommands sut;

    public NugetDeploymentCommandsTest()
    {
        sut = new NugetDeploymentCommands(viewModel.Object, files, new GitHubPushOperation());
    }

    [Fact]
    public void Navigation()
    {
        Assert.Equal(viewModel.Object, sut.NavigateOnReturn());
            
    }

    [Fact]
    public async Task RunCommands()
    {
        Assert.Equal(new []
        {
            ("gpr", "push \"C:\\x.nuspec\""),
            ("gpr", "push \"C:\\y.nuspec\""),
            ("gpr", "push \"C:\\z.nuspec\""),
        }, await sut.Commands().ToArrayAsync(cancellationToken: TestContext.Current.CancellationToken));
    }
}