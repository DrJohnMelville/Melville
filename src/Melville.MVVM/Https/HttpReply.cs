using  System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Melville.MVVM.Https
{
  public interface IHttpReply
  {
    Task<Stream> AsStream();
    Task<string> AsString();
    Task<byte[]> AsBytes();
    bool Succeeded { get; }
  }

  public class HttpReply : IHttpReply
  {
    private readonly HttpResponseMessage response;

    public HttpReply(HttpResponseMessage response)
    {
      this.response = response;
    }

    public Task<Stream> AsStream() => response.Content.ReadAsStreamAsync();

    public Task<string> AsString() => response.Content.ReadAsStringAsync();

    public Task<byte[]> AsBytes() => response.Content.ReadAsByteArrayAsync();

    public bool Succeeded => response.IsSuccessStatusCode;
  }
}