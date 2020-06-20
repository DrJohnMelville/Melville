using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Melville.MVVM.FileSystem;
using WebDashboard.SystemInterface;

namespace WebDashboard.Models
{
    public interface IRootModelFactory
    {
        Task<RootModel> Create(IFile pubXmlFile);
    }
    public class RootModelFactory: IRootModelFactory
    {
        private readonly IEnvironmentExpander environment;

        public RootModelFactory(IEnvironmentExpander environment)
        {
            this.environment = environment;
        }

        public async Task<RootModel> Create(IFile pubXmlFile)
        {
            var (publishFileHolder,projectFileHolder )=
                await FindFiles(pubXmlFile);
            return new RootModel(
                publishFileHolder,
                projectFileHolder,
                await GetSecretFile(projectFileHolder.UserSecretId, pubXmlFile),
                await GetSecretFile(publishFileHolder?.UserSecretId, pubXmlFile)
                );
        }

        private async Task<(PublishFileHolder?, ProjectFileHolder)> FindFiles(IFile pubXmlFile)
        {
            if (pubXmlFile.Extension().Equals("csproj", StringComparison.OrdinalIgnoreCase))
            {
                return (null, await CreateProjectFileHolder(pubXmlFile));
            }
            return (new PublishFileHolder(pubXmlFile, await LoadXmlFile(pubXmlFile)),
                await CreateProjectFileHolder(FindProjectFile(pubXmlFile)));
        }

        private async Task<ProjectFileHolder> CreateProjectFileHolder(IFile projectFile) => 
            new ProjectFileHolder(projectFile, await LoadXmlFile(projectFile));

        private async Task<SecretFileHolder?> GetSecretFile(string? secretId, IFile pubXmlFile) =>
            string.IsNullOrWhiteSpace(secretId)?null: 
                await ParseSecretFile(await FindSecretFile(secretId, pubXmlFile.Directory));

        private static async Task<SecretFileHolder?> ParseSecretFile(IFile? secretFile)
        {
            if (secretFile == null) return null;
            JsonDocument doc = JsonDocument.Parse("{}");
            try
            {
                doc= await JsonDocument.ParseAsync(await secretFile.OpenRead());
            }
            catch (Exception)
            {
            }
            return new SecretFileHolder(secretFile, doc);
        }

        private async Task<IFile?> FindSecretFile(string secretId, IDirectory? directory)
        {
            if (string.IsNullOrWhiteSpace(secretId)) return null;
            var secretFile = FindSecretFileOnDisk(secretId, directory);
            if (secretFile?.Directory == null) return null;
            if (secretFile.Exists()) return secretFile;
            EnsureDirectoryExists(secretFile.Directory);
            await WriteEmptySecretFile(secretFile);
            return secretFile;
        }

        private static async Task WriteEmptySecretFile(IFile secretFile)
        {
            await using var output = await secretFile.CreateWrite();
            output.Write(new byte[] {123, 125}); // Utf8 for {}
        }

        private static void EnsureDirectoryExists(IDirectory directory)
        {
            if (!directory.Exists())
            {
                directory.Create();
            }
        }

        private IFile? FindSecretFileOnDisk(string secretId, IDirectory? directory) =>
            directory?.FileFromRawPath( environment.Expand(
                $@"%APPDATA%\Microsoft\UserSecrets\{secretId}\secrets.json"));

        private IFile CreateFakeSecretFile() =>
            new MemoryFile("c:\\Fake\\secrets.json", Encoding.UTF8.GetBytes(
                @"{""NotEnabled"": ""This project does not have secrets enabled.  Changes will not be saved.""}"));

        private async Task<XElement> LoadXmlFile(IFile file) =>
            await LoadXmlFile(await file.OpenRead());
        private Task<XElement> LoadXmlFile(Stream stream) => 
             XElement.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

        private IFile FindProjectFile(IFile pubXmlFile) => FindProjectFile(pubXmlFile.Directory);

        private IFile FindProjectFile(IDirectory? directory)
        {
            if (directory == null) throw new InvalidOperationException("Project File Not Found");
            return directory.AllFiles("*.csproj").FirstOrDefault() ?? FindProjectFile(directory.Directory);
        }
    }
}