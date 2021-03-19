using System.Collections.Generic;
using System.Threading.Tasks;
using Melville.INPC;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.RunShellCommands;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.RootWindows;
using Melville.MVVM.Wpf.ViewFrames;

namespace WebDashboard.ConsoleWindows
{
    public interface IConsoleCommands
    {
        IAsyncEnumerable<(string Command, string Param)> Commands();
        object NavigateOnReturn();
    }

    [OnDisplayed(nameof(RunCommand))]
    public partial class ConsoleWindowViewModel
    {
        [AutoNotify] private bool done;
        public IList<string> Output { get; } = new ThreadSafeBindableCollection<string>();
       private readonly IConsoleCommands consoleCommands;

        public ConsoleWindowViewModel(IConsoleCommands consoleCommands)
        {
            this.consoleCommands = consoleCommands;
        }

        public async Task RunCommand([FromServices]IRunShellCommand shell)
        {
            await foreach (var (cmd, parameters) in consoleCommands.Commands())
            {
                await shell.CapturedShellExecute(cmd, parameters).PumpOutput(Output.Add);
            }

            Done = true;
        }
        public void Return(INavigationWindow navigator) => 
            navigator.NavigateTo(consoleCommands.NavigateOnReturn());
    }
}