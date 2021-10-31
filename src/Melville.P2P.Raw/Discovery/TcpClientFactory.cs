using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Melville.P2P.Raw.Discovery;

public static class TcpClientFactory
{
    public static async Task<TcpClient> CreateTcpClient(byte[] address)
    {
        var ret = new TcpClient();
        await ret.ConnectAsync(new IPAddress(address.AsSpan()[..4]),
            (int)BitConverter.ToUInt16(address, 4));
        return ret;
    }
}