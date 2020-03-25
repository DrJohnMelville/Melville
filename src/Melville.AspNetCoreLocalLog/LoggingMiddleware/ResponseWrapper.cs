using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreLocalLog.LoggingMiddleware
{
  public interface IHttpOutput
  {
    Task WriteAsync(string text);
  }

  public class ResponseWrapper: IHttpOutput
  {
    private readonly HttpResponse contextResponse;

    public ResponseWrapper(HttpResponse contextResponse)
    {
      this.contextResponse = contextResponse;
    }

    public Task WriteAsync(string text) => contextResponse.WriteAsync(text, Encoding.UTF8);
  }
}