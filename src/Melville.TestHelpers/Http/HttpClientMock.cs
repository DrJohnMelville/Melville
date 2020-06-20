using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Language.Flow;

namespace Melville.TestHelpers.Http
{
  public interface IHttpClientMock
  {
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancel);
  }

  public static class HttpClientMockOperations
  {
    public static HttpClient ToHttpClient(this Mock<IHttpClientMock> mockInt)
    {
      return new HttpClient(new WrappingHandler(mockInt.Object), true){BaseAddress = new Uri("https://localhost:1/")};
    }

    private class WrappingHandler:HttpMessageHandler
    {
      private readonly IHttpClientMock target;
      public WrappingHandler(IHttpClientMock target) => this.target = target;
      protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) => 
        target.SendAsync(request, cancellationToken);
    }

    public static ISetup<IHttpClientMock, Task<HttpResponseMessage>> Setup(this Mock<IHttpClientMock> mockInt,
      Func<string, bool> uri, HttpMethod? method = null) => Setup(mockInt, (object) uri, method);
    /// <summary>
    /// Setup a http response
    /// </summary>
    /// <param name="mockInt">The mock to setip</param>
    /// <param name="uri">Can : null or whitespace (match anything), string (match the path and query),
    /// a Regex (execute on the whole url) or a Func<string, bool> (execute on the url) </param>
    /// <param name="method">Optional -- http method for the request</param>
    /// <returns></returns>
    public static ISetup<IHttpClientMock, Task<HttpResponseMessage>> Setup(this Mock<IHttpClientMock> mockInt,
       object? uri, HttpMethod? method = null) =>
      mockInt.Setup(i => i.SendAsync(
        It.Is<HttpRequestMessage>(j => VerifyUri(uri, method, j)),
        It.IsAny<CancellationToken>()));

    private static bool VerifyUri(object? uri, HttpMethod? method, HttpRequestMessage j) =>
      VerifyUriText(uri, j) && VerifyMethod(method, j);

    private static bool VerifyMethod(HttpMethod method, HttpRequestMessage j) => method == null || j.Method == method;

    private static bool VerifyUriText(object? uri, HttpRequestMessage j) =>
      uri switch
      {
        null => true,
        Regex exp => exp.IsMatch(j.RequestUri.ToString()??""),
        Func<string, bool> func => func(j.RequestUri.ToString()),
        _ when string.IsNullOrWhiteSpace(uri.ToString()) => true,
        _ => j.RequestUri.PathAndQuery.Equals(uri.ToString(), StringComparison.OrdinalIgnoreCase)
      };

    public static void Verify(this Mock<IHttpClientMock> mockInt,  object? uri, Func<Times> times) =>
      Verify(mockInt, uri, null, times());
    public static void Verify(this Mock<IHttpClientMock> mockInt,  object? uri, Times times) =>
      Verify(mockInt, uri, null, times);

    public static void Verify(this Mock<IHttpClientMock> mockInt,  object? uri, HttpMethod? method, Func<Times> times) =>
      Verify(mockInt, uri, method, times());
    
    public static void Verify(this Mock<IHttpClientMock> mockInt, object? uri, HttpMethod? method, Times times) =>
      mockInt.Verify(i => i.SendAsync(
        It.Is<HttpRequestMessage>(j => VerifyUri(uri, method, j)),
        It.IsAny<CancellationToken>()), times);
    
    public static void ReturnsHttp(this ISetup<IHttpClientMock, Task<HttpResponseMessage>> setup, string httpContent,
      string mediaType) =>
      setup.ReturnsAsync(() => new HttpResponseMessage(HttpStatusCode.OK) 
        {Content = new StringContent(httpContent, Encoding.UTF8, mediaType)});

    public static void ReturnsJson(this ISetup<IHttpClientMock, Task<HttpResponseMessage>> setup, string json) =>
      setup.ReturnsHttp(json, "application/json");
    public static void ReturnsJsonObject(this ISetup<IHttpClientMock, Task<HttpResponseMessage>> setup, object payload) =>
      setup.ReturnsJson(JsonSerializer.Serialize(payload));

  }
}