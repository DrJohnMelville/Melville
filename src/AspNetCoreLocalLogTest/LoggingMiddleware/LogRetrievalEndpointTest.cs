using System;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCoreLocalLog.LoggingMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Xunit;

namespace AspNetCoreLocalLogTest.LoggingMiddleware
{
  public sealed class LogRetrievalEndpointTest
  {
    private int finalCalls = 0;

    private readonly LogRetrievalEndpoint sut;
    private readonly TestServer server;
    private readonly HttpClient client;
    private readonly Mock<IRetrieveLog> logSource = new Mock<IRetrieveLog>();
    public LogRetrievalEndpointTest()
    {
      logSource.Setup(i => i.TryLogCommand(It.IsAny<string>(), It.IsAny<IHttpOutput>())).ReturnsAsync(
        (string s, IHttpOutput output) => s.Equals("json") || s.Equals("html"));
      sut = new LogRetrievalEndpoint(logSource.Object);
      sut.WithSecret("WowSecret");
      server = new TestServer(CreateHostBuilder());
      client = server.CreateClient();
    }

    private IWebHostBuilder CreateHostBuilder()
    {
      var builder = new WebHostBuilder()
        .UseEnvironment("Testing")
        .ConfigureServices(services => { })
        .Configure(app =>
        {
          app.Use(sut.Process);
          app.Use(DefaultEndpoint);
        });
      return builder;
    }

    private Task DefaultEndpoint(HttpContext context, Func<Task> neverCalled)
    {
      finalCalls++;
      return Task.CompletedTask;
    }

    [Theory]
    [InlineData("/Home/Index", 1)] // typical urls get through.
    [InlineData("/QuickLog/WowSecret/html", 0)] // typical urls get through.
    [InlineData("/QuickLog/WowSecret/json", 0)] // typical urls get through.
    [InlineData("/QuickLog/WowSecret/Foo", 1)] // Unknown Command
    [InlineData("/QuickLog/WowSecre1t/json", 1)] // Secret wrong
    [InlineData("/Quicklog/WowSecret/json", 1)] // prefix wrong
    public async Task PassthroughIsValid(string uri, int expectedCalls)
    {
      await client.GetAsync(uri);
      Assert.Equal(expectedCalls, finalCalls);
      
    }
  }
}