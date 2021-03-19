using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Melville.MVVM.FileSystem;
using WebDashboard.ConsoleWindows;

namespace WebDashboard.SecretManager.Views
{
    public class DeploymentCommandSource: IConsoleCommands
    {
        public IRootViewModel Model { get; }
        private readonly IHasPassword password;

        public DeploymentCommandSource(IHasPassword password, IRootViewModel model)
        {
            this.password = password;
            Model = model;
        }

        public async IAsyncEnumerable<(string Command, string Param)> Commands()
        {
            var webConfig = FindWebConfigFile();
            await using var preservedFile = new PreserveFile(webConfig);
            await preservedFile.Initialize();
            await WriteWebConfig(webConfig);
            yield return ("dotnet", PublishSyntax());
            webConfig.Delete();
        }

        private IFile FindWebConfigFile() =>
            Model.ProjectFile().Directory?.File("web.config") ??
            throw new InvalidOperationException("Cannot find project directory.");

        private async Task WriteWebConfig(IFile webConfig)
        {
            await using var outputStream = await webConfig.CreateWrite();
            outputStream.Write(Encoding.UTF8.GetBytes(Model.WebConfig));
        }
        
        private string PublishSyntax() => 
            $"publish {Model.ProjectFile().Path} /p:PublishProfile={Model?.PublishFile()?.NameWithoutExtension()} /p:Password={password.Password()}";

        public object NavigateOnReturn() => Model;
    }
}