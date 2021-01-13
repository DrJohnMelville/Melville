using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Melville.P2P.Raw.Discovery;
using Melville.P2P.Raw.NetworkPrimatives;
using Moq;
using Xunit;

namespace Melville.P2P.Test.Discovery
{
    public class DiscoveryTest
    {
        private readonly Mock<IUdpBroadcaster> broadcast = new();
        private readonly Mock<IUdpReceiver> rec = new();
        private readonly DiscoveryServer server;
        private readonly DiscoveryClient client;
        private readonly byte[] targetAddress = {127,0,0,1,5,6};

        public DiscoveryTest()
        {
            server = new DiscoveryServer(broadcast.Object, rec.Object,
                targetAddress);
            client = new DiscoveryClient(broadcast.Object, rec.Object);
        }

        [Fact]
        public void ServerSendsDefaultPacket()
        {
            GC.KeepAlive(server.AcceptConnections());
            broadcast.Verify(i=>i.Send(targetAddress, -1), Times.Once);
        }
        [Fact]
        public async Task ServerRespondsToRequestPacket()
        {
            await server.AcceptConnections();
            RaisePacketReceived(new byte[6]);
            broadcast.Verify(i=>i.Send(targetAddress, -1), Times.Exactly(2));
        }

        private void RaisePacketReceived(byte[] address)
        {
            rec.Raise(i => i.ReceivedPacket += null, this,
                new UdpArrivedEventArgs(new UdpReceiveResult(address, new IPEndPoint(100, 1))));
        }

        private void WireBroadcastToReceiver()
        {
            broadcast.Setup(i => i.Send(It.IsAny<byte[]>(), -1)).Returns((byte[] b, int _) =>
            {
                RaisePacketReceived(b);
                return Task.FromResult(6);
            });
        }

        [Fact]
        public async Task ClientBeforeServerConnection()
        {
            WireBroadcastToReceiver();
            var task = client.Connect();
            await server.AcceptConnections();
            using var ret = await task;
            VerifyValidClient(ret);
        }

        private static void VerifyValidClient(TcpClient ret)
        {
            Assert.Equal(AddressFamily.InterNetwork ,ret.Client.AddressFamily);
        }

        [Fact]
        public async Task ServerBeforeClient()
        {
            WireBroadcastToReceiver();
            await server.AcceptConnections();
            var task = client.Connect();
            using var ret = await task;
            VerifyValidClient(ret);
        }
    }
}