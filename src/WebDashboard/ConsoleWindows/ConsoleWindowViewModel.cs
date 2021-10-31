using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melville.INPC;
using Melville.Lists;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.RootWindows;
using Melville.MVVM.Wpf.ViewFrames;
using Melville.SystemInterface.RunShellCommands;

namespace WebDashboard.ConsoleWindows;

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
        try
        {
            await foreach (var (cmd, parameters) in consoleCommands.Commands())
            {
                await shell.CapturedShellExecute(cmd, parameters).PumpOutput(Output.Add);
            }
        }
        catch (Exception e)
        {
            Output.Add("Exception Thrown: "+e.GetType().Name);
            Output.Add(e.Message);
            Output.Add(e.StackTrace??"");
        }
        finally
        {
            Done = true;
        }
    }
        
    public void Return(INavigationWindow navigator) => 
        navigator.NavigateTo(consoleCommands.NavigateOnReturn());
}