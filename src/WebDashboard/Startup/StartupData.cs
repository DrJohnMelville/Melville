using Melville.FileSystem;

namespace WebDashboard.Startup
{
    public interface IStartupData
    {
        string[] CommandLineArguments { get; }
        IFile ArgumentAsFile(int i);
    }

    public sealed class StartupData: IStartupData
    {
        public string[] CommandLineArguments { get; }
        private readonly IDiskFileSystemConnector diskFileSystemConnector;
        public StartupData(string[] commandLineArguments, IDiskFileSystemConnector diskFileSystemConnector)
        {
            CommandLineArguments = commandLineArguments;
            this.diskFileSystemConnector = diskFileSystemConnector;
        }

        public IFile ArgumentAsFile(int i) => 
            diskFileSystemConnector.FileFromPath(CommandLineArguments[i]);
    }
}