using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Melville.Log.Viewer.NamedPipeServers;

namespace Melville.Log.Viewer.EventSink
{
    public interface INamedPipeWriterProtocol
    {
        Task Write(byte[] data, int origin, int length);
        Task<bool> Connect();
    }
    public class NamedPipeWriterProtocol:INamedPipeWriterProtocol
    {
        private readonly LoggingPipeName loggingPipeName;
        private Stream? outputPipe;

        public NamedPipeWriterProtocol(LoggingPipeName loggingPipeName)
        {
            this.loggingPipeName = loggingPipeName;
        }

        public Task Write(byte[] data, int origin, int length)
        {
            return outputPipe?.WriteAsync(data, origin, length);
        }

        public async Task<bool> Connect()
        {
            try
            {
                await using var initiationPipe = await ConnectToPipe(loggingPipeName.ServerName);
                var newPipeName = new StreamReader(initiationPipe).ReadLine();
                if (newPipeName == null) return false;
                outputPipe = await ConnectToPipe(newPipeName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static async Task<NamedPipeClientStream> ConnectToPipe(string newPipeName)
        {
            var pipe = new NamedPipeClientStream(newPipeName);
            await pipe.ConnectAsync(1000);
            return pipe;
        }
    }
}