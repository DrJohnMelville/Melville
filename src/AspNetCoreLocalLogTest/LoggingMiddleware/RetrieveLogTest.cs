using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using AspNetCoreLocalLog.LoggingMiddleware;
using AspNetCoreLocalLog.LogSink;
using Moq;
using Serilog.Events;
using Serilog.Parsing;
using Xunit;

namespace AspNetCoreLocalLogTest.LoggingMiddleware
{
  public class RetrieveLogTest
  {
    private readonly Mock<ICircularMemorySink> source = new Mock<ICircularMemorySink>();
    private readonly Mock<IHttpOutput> output = new Mock<IHttpOutput>();
    private readonly RetrieveLog sut;

    public RetrieveLogTest()
    {
      sut = new RetrieveLog(source.Object);
    }

    [Fact]
    public async Task UnrecognizedCommandReturnsFalseAndDoesNotTouchOutput()
    {
      Assert.False(await sut.TryLogCommand("foooBar", output.Object));
      output.VerifyNoOtherCalls();
    }

    private static LogEvent CreateFakeEvent(DateTimeOffset timeStamp, string MessageText) =>
      new LogEvent(timeStamp, LogEventLevel.Debug, null, 
        new MessageTemplate(MessageText, new MessageTemplateToken[0]),
        new LogEventProperty[0]);

    [Fact]
    public async Task HtmlOutputAsTable()
    {
      source.Setup(i => i.All()).Returns(new[]
      {
        CreateFakeEvent(new DateTimeOffset(1975,7,28, 0, 0, 0, TimeSpan.Zero), "John Was Born"),
        CreateFakeEvent(new DateTimeOffset(1976,7,28, 0, 0, 0, TimeSpan.Zero), "Happy First Birthday"),
      });
      
      Assert.True(await sut.TryLogCommand("html", output.Object));
      output.Verify(i=>i.WriteAsync(
        "<html><head><link rel='stylesheet' href='css'/></head><body><h1>Quick Log</h1><table><tr><th>Time</th><th>Level</th><th>Event</th></tr>"));
      output.Verify(i=>i.WriteAsync(
        "</table></body></html>"));
      source.Verify(i=>i.Clear());
    }

    [Fact]
    public async Task OutputCss()
    {
      Assert.True(await sut.TryLogCommand("css", output.Object));
      output.Verify(i=>i.WriteAsync(HtmlLogFormatter.CssFile));
    }



  }
}