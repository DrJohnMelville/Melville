using System;
using System.Threading.Tasks;
using Melville.P2P.Raw.Discovery;
using Melville.P2P.Raw.NetworkPrimatives;
using Xunit;

namespace Melville.P2P.Test.Discovery
{
    public class TcpServerTest: IDisposable
    {
        private TcpServer server = new TcpServer();

        public void Dispose() => server.Dispose();

        [Fact]
        public void Address()
        {
            var addr = server.AddressArray();
            Assert.Equal(6, addr.Length);
            Assert.Equal(192, addr[0]);
            Assert.Equal(168, addr[1]);
            Assert.Equal(0, addr[2]);
        }

        [Fact]
        public async Task ConnectTcp()
        {
            await using var iterator = server.MonitorForConnection().GetAsyncEnumerator();
            var task = iterator.MoveNextAsync();
            using var client = await TcpClientFactory.CreateTcpClient(server.AddressArray());
            Assert.True(await task);
            Assert.NotNull(iterator.Current);
        }
    }
}