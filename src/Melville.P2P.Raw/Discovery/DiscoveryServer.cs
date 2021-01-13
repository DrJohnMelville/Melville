using System;
using System.Linq;
using System.Threading.Tasks;
using Melville.P2P.Raw.NetworkPrimatives;

namespace Melville.P2P.Raw.Discovery
{
    public interface IDiscoveryServer
    {
        Task AcceptConnections();
    }

    public class DiscoveryServer : IDiscoveryServer
    {
        private readonly IUdpBroadcaster broadcast;
        private readonly IUdpReceiver receiver;
        private readonly byte[] targetAddress;

        public DiscoveryServer(IUdpBroadcaster broadcast, IUdpReceiver receiver, byte[] targetAddress)
        {
            this.broadcast = broadcast;
            this.receiver = receiver;
            this.targetAddress = targetAddress;
        }

        public async Task AcceptConnections()
        {
            receiver.ReceivedPacket += ReceivePacket;
            GC.KeepAlive(receiver.WaitForReads());
            await SendServerAddress();
        }

        private void ReceivePacket(object? sender, UdpArrivedEventArgs e)
        {
            if (e.IsEmptyTargetAddress())
            {
                GC.KeepAlive(SendServerAddress());
            }
        }

        private Task<int> SendServerAddress() => broadcast.Send(targetAddress);
    }
}