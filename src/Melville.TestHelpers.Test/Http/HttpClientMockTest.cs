using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Melville.TestHelpers.Http;
using Moq;
using Xunit;

namespace Melville.TestHelpers.Test.Http
{
    public class HttpClientMockTest
    {
        private readonly Mock<IHttpClientMock> mock = new Mock<IHttpClientMock>();
        

        [Theory]
        [InlineData("https://www.drjonmelville.com/", "/")]
        [InlineData("https://www.drjonmelville.com/?x=y", "/?x=y")]
        [InlineData("https://www.drjonmelville.com/ESearch?x=y", "/esearch?x=y")]
        public async Task SimpleGet(string url, string pathAndQuery)
        {
            mock.Setup(pathAndQuery, HttpMethod.Get).ReturnsHttp("FooBar", "text/plain");
            var http = mock.ToHttpClient();
            Assert.Equal("FooBar", await http.GetStringAsync(url));
            await Assert.ThrowsAsync<InvalidOperationException>(() => http.GetStringAsync(url + "x"));
        }
        [Theory]
        [InlineData("https://www.drjonmelville.com/")]
        [InlineData("https://www.drjonmelville.com/?x=y")]
        [InlineData("https://www.drjonmelville.com/ESearch?x=y")]
        public async Task Regex(string url)
        {
            mock.Setup(new Regex("www."), HttpMethod.Get).ReturnsHttp("FooBar", "text/plain");
            var http = mock.ToHttpClient();
            Assert.Equal("FooBar", await http.GetStringAsync(url));
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => http.GetStringAsync(url.Replace("www","xxx")));
        }
        [Theory]
        [InlineData("https://www.drjonmelville.com/")]
        [InlineData("https://www.drjonmelville.com/?x=y")]
        [InlineData("https://www.drjonmelville.com/ESearch?x=y")]
        public async Task NullMatchesEverything(string url)
        {
            mock.Setup(null, HttpMethod.Get).ReturnsHttp("FooBar", "text/plain");
            var http = mock.ToHttpClient();
            Assert.Equal("FooBar", await http.GetStringAsync(url));
        }
        [Theory]
        [InlineData("https://www.drjonmelville.com/")]
        [InlineData("https://www.drjonmelville.com/?x=y")]
        [InlineData("https://www.drjonmelville.com/ESearch?x=y")]
        public async Task WhiteSpaceMatchesEverything(string url)
        {
            mock.Setup("   ", HttpMethod.Get).ReturnsHttp("FooBar", "text/plain");
            var http = mock.ToHttpClient();
            Assert.Equal("FooBar", await http.GetStringAsync(url));
        }
        [Theory]
        [InlineData("https://www.drjonmelville.com/")]
        [InlineData("https://www.drjonmelville.com/?x=y")]
        [InlineData("https://www.drjonmelville.com/ESearch?x=y")]
        public async Task MatchWithFunc(string url)
        {
            mock.Setup(s=>s.Equals(url), HttpMethod.Get).ReturnsHttp("FooBar", "text/plain");
            var http = mock.ToHttpClient();
            Assert.Equal("FooBar", await http.GetStringAsync(url));
            await Assert.ThrowsAsync<InvalidOperationException>(() => http.GetStringAsync(url + "x"));
        }

    }
}