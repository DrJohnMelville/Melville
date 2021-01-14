using System;
using System.Threading.Tasks;
using Melville.P2P.Raw.NetworkPrimatives;

namespace Melville.P2P.Raw.Discovery
{
    public interface IDiscoveryClient
    {
        Task<byte[]> Connect();
    }

    public class DiscoveryClient : IDiscoveryClient
    {
        private readonly IUdpBroadcaster broadcast;
        private readonly IUdpReceiver receiver;

        public DiscoveryClient(IUdpBroadcaster broadcast, IUdpReceiver receiver)
        {
            this.broadcast = broadcast;
            this.receiver = receiver;
        }

        public async Task<byte[]> Connect()
        {
            var serverTask = ListenForServerAddress();
            await RequestServerAddress();
            return await serverTask;
        }

        private async Task<byte[]> ListenForServerAddress()
        {
            await foreach (var packet in receiver.WaitForReads())
            {
                if (!packet.IsEmptyTargetAddress())
                {
                    return packet.Buffer;
                }
            }
            return Array.Empty<byte>();
        }

        private Task RequestServerAddress() => broadcast.Send(new byte[6]);
    }
}