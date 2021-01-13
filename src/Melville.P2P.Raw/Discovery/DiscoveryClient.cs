using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Melville.P2P.Raw.NetworkPrimatives;

namespace Melville.P2P.Raw.Discovery
{
    public interface IDiscoveryClient
    {
        Task<TcpClient> Connect();
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

        public Task<TcpClient> Connect()
        {
            ListenForServerAddress();
            RequestServerAddress();
            return serverAddressReceived.Task;
        }

        private void ListenForServerAddress()
        {
            receiver.ReceivedPacket += HandlePacket;
            GC.KeepAlive(receiver.WaitForReads());
        }

        private void RequestServerAddress() => GC.KeepAlive(broadcast.Send(new byte[6]));

        private void HandlePacket(object? sender, UdpArrivedEventArgs e)
        {
            if (!e.IsEmptyTargetAddress())
            {
                serverAddressReceived.SetResult(CreateTcpClient(e.Data.Buffer));
            }
        }

        private static TcpClient CreateTcpClient(byte[] address) =>
            new(new IPEndPoint(new IPAddress(address.AsSpan()[..4]),
                BitConverter.ToUInt16(address, 4)));
    }
}