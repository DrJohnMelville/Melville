using System.Collections.Generic;
using AspNetCoreLocalLog.Data;
using Serilog.Core;
using Serilog.Events;

namespace AspNetCoreLocalLog.LogSink
{
  public interface ICircularMemorySink
  {
    void Push(LogEvent item);
    IEnumerable<LogEvent> All();
    void Clear();
  }
  public sealed class CircularMemorySink: CircularBuffer<LogEvent>, ICircularMemorySink
  {
    public CircularMemorySink() : base(1000)
    {
    }
  }

  public class VolitileSerilogSink: ILogEventSink
  {
    private readonly ICircularMemorySink innerSink;

    public VolitileSerilogSink(ICircularMemorySink innerSink)
    {
      this.innerSink = innerSink;
    }
    public void Emit(LogEvent logEvent)
    {
      innerSink.Push(logEvent);
    }
  }
}