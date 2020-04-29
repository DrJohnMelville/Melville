using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Serilog.Events;

namespace Melville.Log.NamedPipeEventSink
{
    public interface INamedPipeWriterProtocol
    {
        Task Write(byte[] data, int origin, int length);
        Task<bool> Connect();
        Task<LogEventLevel> ReadLevel();
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
            return outputPipe?.WriteAsync(data, origin, length) ?? Task.CompletedTask;
        }

        public async Task<LogEventLevel> ReadLevel()
        {
            var readBuff = new byte[10];
            if (outputPipe == null)
            {
                throw new InvalidOperationException("The stream must be open before being read.");
            }
            var inputLength = await outputPipe.ReadAsync(readBuff, 0, readBuff.Length);
            return inputLength == 0? LogEventLevel.Information: (LogEventLevel) (readBuff[inputLength - 1]);
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
            var pipe = new NamedPipeClientStream(".", newPipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            await pipe.ConnectAsync(1000);
            return pipe;
        }
    }
}