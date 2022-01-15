using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Melville.Log.Viewer.LogViews;
using Serilog.Events;
using Serilog.Parsing;

namespace Melville.Log.Viewer.UdpServers;


public sealed class UdpReceiver: IDisposable
{
    private readonly UdpClient client;
    public async IAsyncEnumerable<UdpReceiveResult> WaitForReads()
    {
        while (true)
        {
            UdpReceiveResult result;
            try
            {
                result = await client.ReceiveAsync();
            }
            catch (ObjectDisposedException ) // cancellation occurred
            {
                yield break;
            }

            yield return result;
        }
            
    }
        
    public UdpReceiver(int port) => client = new UdpClient(port, AddressFamily.InterNetwork);
    public void Dispose() => client.Dispose();
}

public class UdpLogConnection: ILogConnection
{
    private UdpClient recv = new UdpClient(15321, AddressFamily.InterNetwork);
    private CancellationTokenSource cancelled = new CancellationTokenSource();

    public UdpLogConnection()
    {
        ReadPortAsync();
    }

    private async void ReadPortAsync()
    {
        while (!cancelled.IsCancellationRequested)
        {
            try
            {
                var result = await recv.ReceiveAsync(cancelled.Token);
                SendToLog(Encoding.UTF8.GetString(result.Buffer));
            }
            catch (Exception)
            {
                // cancellation or network error
            }
            
        }
    }

    private void SendToLog(string content)
    {
        if (LogEventArrived is not { } target) return;
        target(this, new LogEventArrivedEventArgs(new LogEvent(DateTimeOffset.Now, 
            LogEventLevel.Information, null, new MessageTemplateParser().Parse(content),
            Array.Empty<LogEventProperty>())));
    }

    public ValueTask SetDesiredLevel(LogEventLevel level)
    {
        return ValueTask.CompletedTask;
    }

    public event EventHandler<LogEventArrivedEventArgs>? LogEventArrived;
    public void StopReading()
    {
    }
}