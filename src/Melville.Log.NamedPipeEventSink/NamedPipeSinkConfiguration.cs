using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Melville.Log.NamedPipeEventSink;

public static class NamedPipeSinkConfiguration
{
    public static LoggerConfiguration NamedPipe(this LoggerSinkConfiguration config)
    {
        var sink = new NamedPipeEventSink(new NamedPipeWriterProtocol(new LoggingPipeName()));
        return config.Sink(sink, LogEventLevel.Verbose, sink.LevelSwitch);
    }
}