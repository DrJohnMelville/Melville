using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.Log.Viewer.NamedPipeServers
{
    public class NewPipeConnectionEventArgs: EventArgs
    {
        public Stream ClientConnection { get; }

        public NewPipeConnectionEventArgs(Stream clientConnection)
        {
            ClientConnection = clientConnection;
        }
    }
    public interface IPipeListener
    { 
        event EventHandler<NewPipeConnectionEventArgs>? NewClientConnection;
    }
    public class PipeListener: IPipeListener
    {
        public event EventHandler<NewPipeConnectionEventArgs>? NewClientConnection;
        private readonly LoggingPipeName pipeName;
        private readonly CancellationToken cancellationToken;

        public PipeListener(LoggingPipeName pipeName, IShutdownMonitor shutdownMonitor)
        {
            this.pipeName = pipeName;
            cancellationToken = shutdownMonitor.CancellationToken;
        }

        private static NamedPipeServerStream CreateNamedPipe(string name) =>
            new NamedPipeServerStream(name, PipeDirection.InOut, 
                NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message, 
                PipeOptions.Asynchronous|PipeOptions.CurrentUserOnly);

        // return immediately and then wait for new connections until the app shuts down
        public async void Start()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await using  var serverPipe = CreateNamedPipe(pipeName.ServerName);
                await serverPipe.WaitForConnectionAsync(cancellationToken);
                var newPipeName = pipeName.NewClientPipeName(20);
                WaitForClientPipeConnection(newPipeName);
                await serverPipe.WriteAsync(Encoding.UTF8.GetBytes(newPipeName+Environment.NewLine));
            }
        }

        // wait 5 seconds for the client to connect on the private channel
        private async void WaitForClientPipeConnection(string newPipeName)
        {
            var localConnectionPipe = CreateNamedPipe(newPipeName);
            var cts = new CancellationTokenSource();
            var task = localConnectionPipe.WaitForConnectionAsync(cts.Token);
            var finalTask = await Task.WhenAny(task, Task.Delay(5000));
            if (finalTask == task)
            {
                NewClientConnection?.Invoke(this, new NewPipeConnectionEventArgs(localConnectionPipe));
            }
            else
            {
                cts.Cancel();
            }
        }
    }
}