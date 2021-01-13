using System.Net.Sockets;
using System.Threading.Tasks;

namespace Melville.P2P.Raw.NetworkPrimatives
{
    public interface IUdpBroadcaster
    {
        public Task<int> Send(byte[] data, int len = -1);
    }

    public sealed class UdpBroadcaster: IUdpBroadcaster
    {
        private readonly int port;

        public UdpBroadcaster(int port) => this.port = port;

        public Task<int> Send(byte[] data, int len = -1) => 
            new UdpClient()
                .SendAsync(data, len >= 0 ? len : data.Length, "255.255.255.255", port);
    }
}