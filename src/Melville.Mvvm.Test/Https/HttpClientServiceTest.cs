#nullable disable warnings
using  System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Melville.Mvvm.TestHelpers.HttpServiceMocks;
using Melville.MVVM.Https;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.Https
{
  public sealed class HttpClientServiceTest
  {
    private readonly Mock<IHttpHardware> hardware = new Mock<IHttpHardware>();
    private readonly HttpClientService sut;

    public HttpClientServiceTest()
    {
      sut = new HttpClientService(hardware.Object);
    }

    [Theory]
    [InlineData("https://a.b.com", "dir", "https://a.b.com/dir")]
    [InlineData("https://a.b.com/", "dir", "https://a.b.com/dir")]
    [InlineData("https://a.b.com/", "/dir", "https://a.b.com/dir")]
    [InlineData("https://a.b.com/", "////dir", "https://a.b.com/dir")]
    [InlineData("https://a.b.com", "/dir", "https://a.b.com/dir")]
    [InlineData("", "https://a.b.com/dir", "https://a.b.com/dir")]
    public async Task FormFinalUrl(string prefix, string postFix, string final)
    {
      sut.Prefix = prefix;
      hardware.Setup(i => i.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(
        (HttpRequestMessage h) =>
        {
          Assert.Equal(final, h.RequestUri.ToString());
          return null;
        });
      await sut.Request(postFix, HttpMethod.Get);
      hardware.Verify(i => i.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
    }

    [Fact]
    public async Task CanGet()
    {
      hardware.Setup(i => i.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(
        (HttpRequestMessage m) =>
        {
          Assert.Equal(HttpMethod.Get, m.Method);
          Assert.Equal("www.abac.com/foo", m.RequestUri.ToString());
          return HttpClientServiceMock.MockReply("Foo Bar");
        });
      var res = await sut.Request("www.abac.com/foo", HttpMethod.Get);
      Assert.Equal("Foo Bar", await res.AsString());

      hardware.Verify(i => i.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
    }

    [Fact]
    public async Task Prefix()
    {
      var cnt = 0;
      sut.Prefix = "https://www.abac.com/";
      hardware.Setup(i => i.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(
        (HttpRequestMessage m) =>
        {
          cnt++;
          Assert.Equal("https://www.abac.com/foo", m.RequestUri.ToString());
          return HttpClientServiceMock.MockReply("Foo Bar");
        });
      var res = await sut.Request("foo", HttpMethod.Get);

      Assert.Equal(1, cnt);
    }
    [Fact]
    public async Task Authenticated()
    {
      var cnt = 0;
      sut.AddAuthentication("bearer", "Token");
      hardware.Setup(i => i.SendAsync(It.IsAny<HttpRequestMessage>())).ReturnsAsync(
        (HttpRequestMessage m) =>
        {
          cnt++;
          Assert.Equal("bearer", m.Headers.Authorization.Scheme);
          Assert.Equal("Token", m.Headers.Authorization.Parameter);
          
          return HttpClientServiceMock.MockReply("Foo Bar");
        });
      var res = await sut.Request("www.abac.com/foo", HttpMethod.Get);

      Assert.Equal(1, cnt);
    }
  }
}