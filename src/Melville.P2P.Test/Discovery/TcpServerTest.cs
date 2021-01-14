using System;
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
    }
}