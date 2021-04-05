using System.IO;
using System.Reflection;
using System.Threading.Channels;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Compact;

namespace Melville.Log.NamedPipeEventSink
{
    public class NamedPipeEventSink : ILogEventSink
    {
        public LoggingLevelSwitch LevelSwitch { get; } = new LoggingLevelSwitch();
        private Channel<LogEvent>? buffer;

        private INamedPipeWriterProtocol targetProtocol;
        private ITextFormatter logEventFormatter = new CompactJsonFormatter();

        public NamedPipeEventSink(INamedPipeWriterProtocol targetProtocol)
        {
            this.targetProtocol = targetProtocol;
            TryConnectToServer();
        }

        private async void StartLoopToReceiveLogLevelMessagesFromClient()
        {
            while (true)
            {
                LevelSwitch.MinimumLevel = await targetProtocol.ReadLevel();
            }
        }

        private async void TryConnectToServer()
        {
            if (!await targetProtocol.Connect()) return; // no connection so do not setup Logginc infrastrucure
            CreateLogEventBuffer();
            StartLoopToReceiveLogLevelMessagesFromClient();
            WriteExecutableNameToRemoteLogger();
            SendLogEvents();
        }

        private async void SendLogEvents()
        {
            while (true)
            {
                if (buffer == null) return; // nullability guard that should never happen
                LogEvent logEvent = await buffer.Reader.ReadAsync();
                using var ms = new MemoryStream();
                using var write = new StreamWriter(ms);
                logEventFormatter.Format(logEvent, write);
                write.Flush();
                await targetProtocol.Write(ms.GetBuffer(), 0, (int) ms.Length);
            }
        }

        public void CreateLogEventBuffer()
        {
            buffer = Channel.CreateBounded<LogEvent>(new BoundedChannelOptions(1000)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = true
            });
        }

        private void WriteExecutableNameToRemoteLogger()
        {
            var localLogger = new LoggerConfiguration().WriteTo.Sink(this, LogEventLevel.Verbose).CreateLogger();
            localLogger.Information("Source Process: {AssignProcessName}",
                Assembly.GetEntryAssembly()?.GetName().Name ?? "No Name");
        }

        public void Emit(LogEvent logEvent)
        {
            buffer?.Writer.TryWrite(logEvent);
        }
    }
}