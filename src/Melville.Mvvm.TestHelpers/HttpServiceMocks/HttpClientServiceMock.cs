#nullable disable warnings
using  System.IO;
using System.Text;
using Melville.MVVM.Https;
using Moq;

namespace Melville.Mvvm.TestHelpers.HttpServiceMocks
{
  public static class HttpClientServiceMock
  {
    public static IHttpReply MockReply(string content)
    {
      var reply = new Mock<IHttpReply>();
      reply.SetupGet(i => i.Succeeded).Returns(true);
      reply.Setup(i => i.AsString()).ReturnsAsync(content);
      reply.Setup(i => i.AsStream()).ReturnsAsync(new MemoryStream(Encoding.UTF8.GetBytes(content)));
      reply.Setup(i => i.AsBytes()).ReturnsAsync(Encoding.UTF8.GetBytes(content));
      return reply.Object;
    }
  }
}