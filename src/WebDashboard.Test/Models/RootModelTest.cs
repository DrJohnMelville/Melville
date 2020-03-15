using System.Threading.Tasks;
using Xunit;

namespace WebDashboard.Test.Models
{
    public class RootModelTest
    {
        [Fact]
        public async Task WebConfigFile()
        {
            var sut = await new RootModelFactoryTest().ConstructedModel();

            Assert.Equal(@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
  <location path=""."" inheritInChildApplications=""false"">
    <system.webServer>
      <modules runAllManagedModulesForAllRequests=""false"">
        <remove name=""WebDAVModule"" />
      </modules>
      <handlers>
        <add name=""aspNetCore"" path=""*"" verb=""*"" modules=""AspNetCoreModuleV2"" resourceType=""Unspecified"" />
      </handlers>
      <aspNetCore processPath="".\proj.exe"" stdoutLogEnabled=""false"" stdoutLogFile="".\logs\stdout"" hostingModel=""inprocess"">
        <environmentVariables>
          <environmentVariable name=""NoTree"" value=""No Tree"" />
          <environmentVariable name=""root:"" value=""blank"" />
          <environmentVariable name=""root:child:s2"" value=""s 2"" />
          <environmentVariable name=""root:child:s3"" value=""s 3"" />
          <environmentVariable name=""root:Secret"" value=""secret 2"" />
          <environmentVariable name=""SupplementalSecret"" value=""Supplement"" />
        </environmentVariables>
      </aspNetCore>
    </system.webServer>
  </location>
</configuration>", sut.ComputeWebConfig());
        }

        [Fact]
        public async Task WebDavModel()
        {
          var sut = await new RootModelFactoryTest().ConstructedModel();
          sut.SuppressWebDavModule = false;
          Assert.DoesNotContain("WebDAVModule", sut.ComputeWebConfig());
        }
        [Fact]
        public async Task DevelopmentMode()
        {
          var sut = await new RootModelFactoryTest().ConstructedModel();
          sut.DevelopmentMode = false;
          Assert.DoesNotContain(@"<environmentVariable name=""ASPNETCORE_ENVIRONMENT"" value=""Development"" />", 
            sut.ComputeWebConfig());
          sut.DevelopmentMode = true;
          Assert.Contains(@"<environmentVariable name=""ASPNETCORE_ENVIRONMENT"" value=""Development"" />", 
            sut.ComputeWebConfig());
        }
    }
}