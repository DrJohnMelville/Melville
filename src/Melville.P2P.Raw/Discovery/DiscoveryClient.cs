using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Melville.P2P.Raw.NetworkPrimatives;

namespace Melville.P2P.Raw.Discovery
{
    public interface IDiscoveryClient
    {
        Task<TcpClient?> Connect();
    }

    public class DiscoveryClient : IDiscoveryClient
    {
        private readonly IUdpBroadcaster broadcast;
        private readonly IUdpReceiver receiver;
        private readonly TaskCompletionSource<TcpClient> serverAddressReceived = new();

        public DiscoveryClient(IUdpBroadcaster broadcast, IUdpReceiver receiver)
        {
            this.broadcast = broadcast;
            this.receiver = receiver;
        }

        public async Task<TcpClient?> Connect()
        {
            var serverTask = ListenForServerAddress();
            await RequestServerAddress();
            return await serverTask;
        }

        private async Task<TcpClient?> ListenForServerAddress()
        {
            await foreach (var packet in receiver.WaitForReads())
            {
                if (!packet.IsEmptyTargetAddress())
                {
                    return CreateTcpClient(packet.Buffer);
                }
            }
            return null;
        }

        private Task RequestServerAddress() => broadcast.Send(new byte[6]);

        private static TcpClient CreateTcpClient(byte[] address) =>
            new(new IPEndPoint(new IPAddress(address.AsSpan()[..4]),
                BitConverter.ToUInt16(address, 4)));
    }
}