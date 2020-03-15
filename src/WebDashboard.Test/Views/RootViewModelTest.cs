using System.Threading.Tasks;
using Melville.MVVM.Wpf.RootWindows;
using Moq;
using WebDashboard.Models;
using WebDashboard.Test.Models;
using WebDashboard.Views;
using Xunit;

namespace WebDashboard.Test.Views
{
    public class RootViewModelTest
    {
        private RootModel model;
        private RootViewModel sut;

        private async Task Create()
        {
            model = await new RootModelFactoryTest().ConstructedModel();
            sut = new RootViewModel(model);
        }

        [Fact]
        public async Task UpdateConfig()
        {
            await Create();
            Assert.Equal("", sut.WebConfig);
            sut.UpdateWebConfig();
            Assert.NotEqual("", sut.WebConfig);
        }

        [Fact]
        public async Task ClickDeploy()
        {
            await Create();
            var navMock = new Mock<INavigationWindow>();
            var dvm = new DeploymentViewModel(navMock.Object, sut, Mock.Of<IHasPassword>());
            
            sut.Deploy((i,j)=>dvm, navMock.Object, Mock.Of<IHasPassword>());
            navMock.Verify(i=>i.NavigateTo(dvm), Times.Once);
        }
    }
}