using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;
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
        private readonly Mock<IUdpReceiver> rec1 = new();
        private readonly Mock<IUdpReceiver> rec2 = new();
        private readonly DiscoveryServer server;
        private readonly DiscoveryClient client;
        private readonly byte[] targetAddress = {127,0,0,1,5,6};
        private readonly Channel<byte[]> channel1 = Channel.CreateUnbounded<byte[]>();
        private readonly Channel<byte[]> channel2 = Channel.CreateUnbounded<byte[]>();

        public DiscoveryTest()
        {
            rec1.Setup(i => i.WaitForReads()).Returns(ReadFromChannel(channel1));
            rec2.Setup(i => i.WaitForReads()).Returns(ReadFromChannel(channel2));
            server = new DiscoveryServer(broadcast.Object, rec1.Object,
                targetAddress);
            client = new DiscoveryClient(broadcast.Object, rec2.Object);
            
        }

        private async IAsyncEnumerable<UdpReceiveResult> ReadFromChannel(Channel<byte[]> channel)
        {
            while (await channel.Reader.ReadAsync() is {} addr)
            {
                yield return new UdpReceiveResult(addr, new IPEndPoint(1,1));
            }
        }

        [Fact]
        public void ServerSendsDefaultPacket()
        {
            GC.KeepAlive(server.AcceptConnections());
            broadcast.Verify(i=>i.Send(targetAddress, -1), Times.Once);
        }
        [Fact]
        public void ServerRespondsToRequestPacket()
        {
            GC.KeepAlive(server.AcceptConnections());
            RaisePacketReceived(new byte[6]);
            Thread.Sleep(20);
            broadcast.Verify(i=>i.Send(targetAddress, -1), Times.Exactly(2));
        }
        

        private async void RaisePacketReceived(byte[] address)
        {
            await channel1.Writer.WriteAsync(address);
            await channel2.Writer.WriteAsync(address);
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
            GC.KeepAlive(server.AcceptConnections());
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
            GC.KeepAlive(server.AcceptConnections());
            using var ret = await client.Connect();
            VerifyValidClient(ret);
        }
    }
}