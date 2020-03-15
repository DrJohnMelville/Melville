using System.IO;
using System.Text;
using System.Threading.Tasks;
using Melville.MVVM.FileSystem;
using Melville.MVVM.Wpf.RootWindows;
using Moq;
using WebDashboard.Views;
using Xunit;

namespace WebDashboard.Test.Views
{
    public class DeploymentViewModelTest
    {
        private readonly Mock<INavigationWindow> nav = new Mock<INavigationWindow>();
        private readonly Mock<IRootViewModel> rvm = new Mock<IRootViewModel>();
        private readonly Mock<IDirectory> projectDir = new Mock<IDirectory>();
        private readonly Mock<IFile> projectFile = new Mock<IFile>();
        private readonly Mock<IFile> configFile = new Mock<IFile>();
        private readonly Mock<IFile> publishFile = new Mock<IFile>();
        private readonly MemoryStream configStream = new MemoryStream();
        private readonly Mock<IRunProcess> runner = new Mock<IRunProcess>();
        private readonly Mock<IProcessProxy> proxy = new Mock<IProcessProxy>();
        private readonly Mock<IHasPassword> password = new Mock<IHasPassword>();
        
        
        private readonly DeploymentViewModel sut;

        public DeploymentViewModelTest()
        {
            rvm.Setup(i => i.ProjectFile()).Returns(projectFile.Object);
            projectFile.SetupGet(i => i.Directory).Returns(projectDir.Object);
            projectFile.SetupGet(i => i.Path).Returns(@"C:\dir\Target.csproj");
            projectDir.Setup(i => i.File("web.config")).Returns(configFile.Object);
            publishFile.SetupGet(i => i.Name).Returns("PubFile.pubxml");
            configFile.Setup(i => i.CreateWrite(FileAttributes.Normal)).ReturnsAsync(configStream);
            proxy.SetupGet(i => i.HasExited).Returns(true);
            rvm.SetupGet(i => i.WebConfig).Returns("New Web Config");
            rvm.Setup(i => i.PublishFile()).Returns(publishFile.Object);
            runner.Setup(i => i.Run("dotnet", It.IsAny<string>())).Returns(proxy.Object);
            sut = new DeploymentViewModel(nav.Object, rvm.Object, password.Object);
        }

        [Fact]
        public void NavigateBack()
        {
            sut.Return();
            nav.Verify(i=>i.NavigateTo(rvm.Object), Times.Once);
            nav.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task RunDeployment()
        {
            password.Setup(i => i.Password()).Returns("StupidPass");
            await sut.RunDeployment(runner.Object);
            Assert.Equal("New Web Config", Encoding.UTF8.GetString(configStream.ToArray()));
            runner.Verify(i=>i.Run("dotnet", 
                @"publish C:\dir\Target.csproj /p:PublishProfile=PubFile /p:Password=StupidPass"), Times.Once);
            configFile.Verify(i=>i.Delete(), Times.Once);
        }
    }
}