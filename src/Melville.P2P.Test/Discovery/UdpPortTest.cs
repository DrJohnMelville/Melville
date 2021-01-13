using System.Net;
using System.Threading.Tasks;
using Melville.P2P.Raw.Discovery;
using Melville.P2P.Raw.NetworkPrimatives;
using Xunit;

namespace Melville.P2P.Test.Discovery
{
    public class UdpPortTest
    {
        [Fact]
        public async Task SimpleSend()
        {
            int received = 0;
            var data = new byte[10];
            var port = MakePort();
            port.ReceivedPacket += (s, e) => received++;
            var done = port.WaitForReads();
            await new UdpBroadcaster(70).Send(data);
            port.Dispose();
            await done;
            Assert.Equal(1, received);
        }

        private UdpReceiver MakePort()
        {
            return new UdpReceiver(70);
        }
    }

    public class TcpServerTest
    {
        private TcpServer server = new TcpServer();

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