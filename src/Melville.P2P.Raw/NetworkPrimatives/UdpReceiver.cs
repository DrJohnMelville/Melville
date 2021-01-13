using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Melville.P2P.Raw.NetworkPrimatives
{
    public class UdpArrivedEventArgs
    {
        public UdpReceiveResult Data { get; }
        public UdpArrivedEventArgs(UdpReceiveResult data)
        {
            Data = data;
        }

        public bool IsEmptyTargetAddress() => Data.Buffer.All(i => i == 0);
    }

    public interface IUdpReceiver
    {
        public event EventHandler<UdpArrivedEventArgs>? ReceivedPacket;
        public Task WaitForReads();
    }

    public sealed class UdpReceiver: IUdpReceiver, IDisposable
    {
        private readonly UdpClient client;
        public event EventHandler<UdpArrivedEventArgs>? ReceivedPacket;

        public async Task WaitForReads()
        {
            while (true)
            {
                try
                {
                    var data = await client.ReceiveAsync();
                    ReceivedPacket?.Invoke(this, new UdpArrivedEventArgs(data));
                }
                catch (ObjectDisposedException ) // cancellation occurred
                {
                    return;
                }
            }
        }

        public UdpReceiver(int port) => client = new UdpClient(port, AddressFamily.InterNetwork);
        public void Dispose() => client.Dispose();
    }
}