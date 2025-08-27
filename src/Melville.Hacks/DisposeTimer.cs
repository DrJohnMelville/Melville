using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Melville.Hacks;

[Obsolete("Should not be used in production code -- it is a debugging tool.")]
public class DisposeTimer: IDisposable
{
    private volatile string? Created = $"TimerCreated at:\r\n{new StackTrace()}";

    public DisposeTimer(TimeSpan expectedLife)
    {
        new Timer(CheckDispose, this, expectedLife, Timeout.InfiniteTimeSpan);
    }

    private static void CheckDispose(object? state)
    {
        (state as DisposeTimer)?.CheckDispose();
    }

    private void CheckDispose()
    {
        if (Created is { } innerCreated)
            UdpConsole.WriteLine(innerCreated);
    }

    /// <inheritdoc />
    public void Dispose() => Created = null;
}

[Obsolete("Should not be used in production code -- it is a debugging tool.")]
public static class UdpConsole
{
    private static UdpClient? client = null;
    private static UdpClient Client
    {
        get
        {
            client ??= new UdpClient();
            return client;
        }
    }

    public static string WriteLine(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        Client.Send(bytes, bytes.Length, "127.0.0.1", 15321);
        return str;
    }
}