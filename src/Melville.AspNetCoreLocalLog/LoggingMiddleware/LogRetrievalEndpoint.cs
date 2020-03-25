using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreLocalLog.LoggingMiddleware
{
  public interface IConfigureLogRetrieval
  {
    IConfigureLogRetrieval WithSecret(string secret);
  }
  
  public sealed class LogRetrievalEndpoint: IConfigureLogRetrieval
  {
    private readonly IRetrieveLog logSource;
    public LogRetrievalEndpoint(IRetrieveLog logSource)
    {
      this.logSource = logSource;
    }
    
    public async Task Process(HttpContext context, Func<Task> next)
    {
      if (! await logSource.TryLogCommand(ParseCommand(context.Request.Path), new ResponseWrapper(context.Response)))
      {
        await next();
      }
    }

    private string ParseCommand(string pathValue)
    {
      var match = Regex.Match(pathValue, $"^/QuickLog/{Regex.Escape(secret)}/(.+)$");
      return match.Success ? match.Groups[1].Value : "";
    }

    private string secret = "DefaultSecret";
    public IConfigureLogRetrieval WithSecret(string secret)
    {
      this.secret = secret;
      return this;
    }
  }
}