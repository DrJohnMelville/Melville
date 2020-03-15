using  System.Net.Http;
using System.Threading.Tasks;

namespace Melville.MVVM.Https
{
  public interface IHttpHardware
  {
    Task<IHttpReply> SendAsync(HttpRequestMessage message);
  }

  public class HttpHardware: IHttpHardware{
    private readonly HttpClient client = new HttpClient();
    public async Task<IHttpReply> SendAsync(HttpRequestMessage message)
    {
      return new HttpReply(await client.SendAsync(message));
    }


  }
}