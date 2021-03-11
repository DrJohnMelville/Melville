using System.Runtime.InteropServices;
using System.Threading.Tasks;
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

        [Theory]
        [InlineData("3.21.1")]
        [InlineData("1.2.3")]
        public async Task ParsesVersionString(string version)
        {
            var bp = root.File("Directory.Build.props");
            bp.Create($"<Project><Version>{version}</Version></Project>");

            var model = await sut.Create(bp);
            Assert.Equal(version, model.Version);
            
        }
    }
}