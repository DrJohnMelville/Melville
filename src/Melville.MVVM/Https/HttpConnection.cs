using  System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Melville.MVVM.Functional;

namespace Melville.MVVM.Https
{
  public interface IHttpClientService
  {
    string Prefix { get; set; }
    Task<IHttpReply> Request(string url, HttpMethod method, HttpContent? content = null);
    void AddDecorator(Action<HttpRequestMessage> decorator);
  }

  public static class HttpClientServiceOps
  {
    public static void AddAuthentication(this IHttpClientService service, string scheme, string parameter)
    {
      var token = new AuthenticationHeaderValue(scheme, parameter);
      service.AddDecorator(i=>i.Headers.Authorization = token);
    }
    public static async Task<Stream> GetStream(this IHttpClientService service, string url) =>
      await (await service.Request(url, HttpMethod.Get)).AsStream();
    public static async Task<string> GetString(this IHttpClientService service, string url) =>
      await (await service.Request(url, HttpMethod.Get)).AsString();
  }

  // multiple configuration from one httpclient  https://contrivedexample.com/2017/07/01/using-httpclient-as-it-was-intended-because-youre-not/
  public sealed class HttpClientService : IHttpClientService
  {
    private readonly IHttpHardware hardware;
    private readonly List<Action<HttpRequestMessage>> decorators = 
      new List<Action<HttpRequestMessage>>();

    public string Prefix { get; set; } = "";
    public HttpClientService(IHttpHardware hardware)
    {
      this.hardware = hardware;
    }

    public Task<IHttpReply> Request(string url, HttpMethod method, HttpContent? content = null)
    {
      var httpRequestMessage = new HttpRequestMessage(method, ComputeFinalPath(url))
      {
        Content = content
      };
      foreach (var decorator in decorators)
      {
        decorator(httpRequestMessage);
      }
      return hardware.SendAsync(httpRequestMessage);
    }

    private string ComputeFinalPath(string url) => string.IsNullOrWhiteSpace(Prefix)?url: UrlFormatter.ConcatUrlParts(Prefix, url);

    public void AddDecorator(Action<HttpRequestMessage> decorator) =>
      decorators.Add(decorator);
  }
}