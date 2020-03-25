using System;
using System.Collections.Generic;
using System.Resources;
using System.Threading.Tasks;
using AspNetCoreLocalLog.LogSink;
using Serilog.Events;

namespace AspNetCoreLocalLog.LoggingMiddleware
{
  public interface IRetrieveLog
  {
    public Task<bool> TryLogCommand(string command, IHttpOutput output);
  }
  public class RetrieveLog : IRetrieveLog
  {
    private readonly ICircularMemorySink source;
    private readonly IFormatProvider? formatProvider;

    public RetrieveLog(ICircularMemorySink source, IFormatProvider? formatProvider = null)
    {
      this.source = source;
      this.formatProvider = formatProvider;
    }

    public async Task<bool> TryLogCommand(string command, IHttpOutput output)
    {
      switch (command)
      {
        case "html":
          await WriteToHtml(output);
          return true;
        case "css":
          await output.WriteAsync(HtmlLogFormatter.CssFile);
          return true;
      }
      return false;
    }

    private async Task WriteToHtml(IHttpOutput output)
    {
      await new HtmlLogFormatter(output, formatProvider).RenderHtmlPage(source.All());
      source.Clear();
    }
  }

  public class HtmlLogFormatter
  {
    private readonly IHttpOutput output;
    private readonly IFormatProvider? formatProvider;

    public HtmlLogFormatter(IHttpOutput output, IFormatProvider? formatProvider = null)
    {
      this.output = output;
      this.formatProvider = formatProvider;
    }

    public const string CssFile = @"
table{
  border-collapse:collapse;
  border:1px solid black;
}

td,th{
  border:1px solid black;
  padding: 3px;
}";
    private const string HtmlLogHeader = "<html><head><link rel='stylesheet' href='css'/></head><body><h1>Quick Log</h1><table><tr><th>Time</th><th>Level</th><th>Event</th></tr>";
    private const string HtmlLogFooter = "</table></body></html>";

    public async Task RenderHtmlPage(IEnumerable<LogEvent> lines)
    {
      await WritePageHeader();
      foreach (var line in lines)
      {
        await RenderSingleLine(line);
      }
      await WritePageFooter();
    }

    private Task RenderSingleLine(LogEvent line) => 
      output.WriteAsync($"<tr><td>{line.Timestamp.LocalDateTime.ToLongTimeString()}</td><td>{line.Level}</td><td><pre>{line.RenderMessage(formatProvider)}</pre></td></tr>");

    private Task WritePageHeader() =>
      output.WriteAsync(HtmlLogHeader);

    private Task WritePageFooter() => output.WriteAsync(HtmlLogFooter);
  }
}