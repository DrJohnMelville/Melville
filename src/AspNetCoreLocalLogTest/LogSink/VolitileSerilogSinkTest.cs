using System;
using System.Transactions;
using AspNetCoreLocalLog.LogSink;
using Moq;
using Serilog.Events;
using Serilog.Parsing;
using Xunit;

namespace AspNetCoreLocalLogTest.LogSink
{
  public sealed class VolitileSerilogSinkTest
  {
    private readonly Mock<ICircularMemorySink> target = new Mock<ICircularMemorySink>();
    private readonly VolitileSerilogSink sut;

    public VolitileSerilogSinkTest()
    {
      sut = new VolitileSerilogSink(target.Object);
    }

    [Fact]
    public void EmitPushesEvent()
    {
      var logEvent = CreateFakeEvent();
      sut.Emit(logEvent);
      target.Verify(i=>i.Push(logEvent), Times.Once);
    }

    private static LogEvent CreateFakeEvent() =>
      new LogEvent(DateTimeOffset.Now, LogEventLevel.Debug, null, 
        new MessageTemplate("Text", new MessageTemplateToken[0]),
        new LogEventProperty[0]);
  }
}