using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Melville.P2P.Raw.NetworkPrimatives
{
    public interface ITcpServer
    {
        public IAsyncEnumerable<TcpClient> MonitorForConnection();

    }
    public sealed class TcpServer: ITcpServer, IDisposable
    {
        private readonly TcpListener listener;

        public TcpServer()
        {
            listener = new TcpListener( MyIpAddress(), 0);
        }

        public async IAsyncEnumerable<TcpClient> MonitorForConnection()
        {
            listener.Start();
            while (await listener.AcceptTcpClientAsync() is { } client)
            {
                yield return client;
            }
        }

        private static IPAddress MyIpAddress()
        {
            var hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
            return hostAddresses.First(i=>i.AddressFamily == AddressFamily.InterNetwork);
        }

        public void Dispose()
        {
            listener.Stop();
        }

        public byte[] AddressArray()
        {
            var addr = (IPEndPoint)listener.LocalEndpoint;
            var ret = new byte[6];
            Array.Copy(addr.Address.GetAddressBytes(), ret, 4);
            BitConverter.TryWriteBytes(ret.AsSpan(4, 2), (ushort) addr.Port);
            return ret;
        }
    }
}