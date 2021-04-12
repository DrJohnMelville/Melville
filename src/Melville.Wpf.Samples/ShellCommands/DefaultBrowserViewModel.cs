using Melville.SystemInterface.RunShellCommands;

namespace Melville.Wpf.Samples.ShellCommands
{
    public class DefaultBrowserViewModel
    {
        private readonly IRunShellCommand runner;

        public DefaultBrowserViewModel(IRunShellCommand runner)
        {
            this.runner = runner;
        }

        public void LaunchGoogle()
        {
            runner.ShellExecute("https://www.google.com");
        }
    }
}