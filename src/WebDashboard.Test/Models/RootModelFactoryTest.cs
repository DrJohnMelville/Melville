using System;
using System.Linq;
using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.Mvvm.TestHelpers.MockFiles;
using Moq;
using WebDashboard.SecretManager.Models;
using WebDashboard.SystemInterface;
using Xunit;

namespace WebDashboard.Test.Models
{
    public class RootModelFactoryTest
    {
        private readonly IDirectory root;
        private readonly IDirectory pubProfiles;
        private readonly IFile pubXmlFile;
        private readonly IFile projFile;
        private readonly IFile secretFile;
        private readonly IFile pubSecretFile;
        private readonly Mock<IEnvironmentExpander> expander = new Mock<IEnvironmentExpander>();
        private readonly IRootModelFactory sut;

        public RootModelFactoryTest()
        {
            root = new MockDirectory("c:\\projdirr");
            projFile = root.File("proj.csproj");
            projFile.Create(ProjText);
            var prop = root.SubDirectory("Properties");
            prop.Create();
            pubProfiles = prop.SubDirectory("PublishProfiles");
            pubProfiles.Create();
            pubXmlFile = pubProfiles.File("Publish.pubxml");
            pubXmlFile.Create(PubXmlText);
            expander.Setup(i => i.Expand(It.IsAny<String>()))
                .Returns((string s) => s.Replace("%APPDATA%", "C:\\Profile"));

            secretFile = MockSecretFile("5e7f3d51-4b7a-41f8-a32a-8f6c11c29274", SecretText);
            pubSecretFile = MockSecretFile("thisisthesecretkey", PubSecretText);
            sut = new RootModelFactory(expander.Object);
        }

        private IFile MockSecretFile(string secretKey, string text)
        {
            var file = new MockFile("c:\\sss\\secret.txt", null);
            file.Create(text);
            ((MockDirectory) pubProfiles).AddRawFile(
                $@"C:\Profile\Microsoft\UserSecrets\{secretKey}\secrets.json",
                file);
            return file;
        }

        public Task<RootModel> ConstructedModel() => sut.Create(pubXmlFile);

        [Fact]
        public async Task Create()
        {
            var ret = await sut.Create(pubXmlFile);
            Assert.Equal("Ewd.drjohnmelville.com", ret.PublishFile.DeployedPath);
            Assert.Equal("5e7f3d51-4b7a-41f8-a32a-8f6c11c29274", ret.ProjectFile.UserSecretId);
            Assert.Equal(2, ret.RootSecretFile.Root.Children.Count());
        }

        [Fact]
        public async Task CreateSecretFile()
        {
            secretFile.Delete();
            Assert.False(secretFile.Exists());
            Assert.False(secretFile.Directory.Exists());
            var ret = await sut.Create(pubXmlFile);
            Assert.True(secretFile.Directory.Exists());
            Assert.True(secretFile.Exists());
        }

        [Fact]
        public async Task LoadFromProjectFile()
        {
            var ret = await sut.Create(projFile);
            Assert.Equal("5e7f3d51-4b7a-41f8-a32a-8f6c11c29274", ret.ProjectFile.UserSecretId);
            Assert.Null(ret.PublishFile);            
        }

 
        private const string SecretText = @"{
  ""root:Secret"": ""secret 1"",
  ""root:child:s2"": ""s 2"",
  ""root:child:s3"": ""s 3"",
  ""NoTree"":""No Tree"",
  ""root:"": ""blank""
}";
        private const string PubSecretText = @"{
  ""root:Secret"": ""secret 2"",
  ""SupplementalSecret"": ""Supplement""
}";
        private const string ProjText = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project Sdk=""Microsoft.NET.Sdk.Web"">
  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
    <RuntimeIdentifiers>win-x86;win-x64</RuntimeIdentifiers>
    <UserSecretsId>5e7f3d51-4b7a-41f8-a32a-8f6c11c29274</UserSecretsId>
    
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include=""JetBrains.Annotations"" Version=""2019.1.3"" />
    <PackageReference Include=""Melville.AspNetCoreLocalLog"" Version=""0.1.0"" />
    <PackageReference Include=""Microsoft.AspNetCore.Mvc.NewtonsoftJson"" Version=""3.1.1"" />
    <PackageReference Include=""Microsoft.AspNetCore.StaticFiles"" Version=""2.2.0"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.Design"" Version=""3.1.1"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.Sqlite"" Version=""3.1.0"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.SqlServer"" Version=""3.1.1"" />
    <PackageReference Include=""Microsoft.EntityFrameworkCore.Tools"" Version=""3.1.0"">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include=""Microsoft.Extensions.Logging.Debug"" Version=""3.1.0"" />
    <PackageReference Include=""Microsoft.VisualStudio.Web.CodeGeneration.Design"" Version=""3.1.0"" />
    <PackageReference Include=""SendMailService"" Version=""0.1.1"" />
    <PackageReference Include=""Serilog.AspNetCore"" Version=""3.2.0"" />
    <PackageReference Include=""Syncfusion.Licensing"" Version=""17.4.0.47"" />
    <PackageReference Include=""TokenServiceClient.Website"" Version=""0.1.2"" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include=""..\CoreWebData\CoreWebData.csproj"" />
    <ProjectReference Include=""..\CoreWebLogic\CoreWebLogic.csproj"" />
    <ProjectReference Include=""..\Melville.IOC\Melville.IOC.csproj"" />
  </ItemGroup>
  <ItemGroup>
    <Folder Include=""PostedContent"" />
  </ItemGroup>
</Project>";
        private const string PubXmlText =
            @"<Project ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
    <PropertyGroup>
        <WebPublishMethod>MSDeploy</WebPublishMethod>
        <ExcludeApp_Data>False</ExcludeApp_Data>
        <MSDeployServiceURL>https://EWd.drJohnMelville.com:8172/msdeploy.axd?site=Ewd.drjohnmelville.com</MSDeployServiceURL>
        <DeployIisAppPath>Ewd.drjohnmelville.com</DeployIisAppPath>
        <RemoteSitePhysicalPath/>
        <SkipExtraFilesOnServer>True</SkipExtraFilesOnServer>
        <MSDeployPublishMethod>WMSVC</MSDeployPublishMethod>
        <EnableMSDeployBackup>True</EnableMSDeployBackup>
        <UserName>johnmelv</UserName>
        <_SavePWD>False</_SavePWD>
        <TargetFramework>net5.0</TargetFramework>
        <SelfContained>true</SelfContained>
        <_IsPortable>false</_IsPortable>
        <RuntimeIdentifier>win-x86</RuntimeIdentifier>
        <AllowUntrustedCertificate>True</AllowUntrustedCertificate>
        <UserSecretsId>thisisthesecretkey</UserSecretsId>
    </PropertyGroup>
</Project>";
    }
}