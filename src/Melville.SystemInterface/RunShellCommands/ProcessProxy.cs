using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Melville.SystemInterface.RunShellCommands;

public interface IProcessProxy
{
    bool HasExited { get; }
    Task<string?> ReadLineAsync();
}

public static class ProcessProxyOperations
{
    public static async Task PumpOutput(this IProcessProxy proxy, Action<string> outputLine)
    {
        while (!proxy.HasExited)
        {
            if (await proxy.ReadLineAsync() is { } line)
            {
                outputLine(line);
            }
        }
    }
}

public class ProcessProxy : IProcessProxy
{
    private readonly Process process;

    public ProcessProxy(Process process)
    {
        this.process = process;
    }


    public bool HasExited => process.HasExited;

    public Task<string?> ReadLineAsync() => process.StandardOutput.ReadLineAsync();
}