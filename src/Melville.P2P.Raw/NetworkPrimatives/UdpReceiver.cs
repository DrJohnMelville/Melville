using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Melville.P2P.Raw.NetworkPrimatives
{
    public static class UdpReceiveResultOperations
    {
        public static bool IsEmptyTargetAddress(this UdpReceiveResult Data) =>
            Data.Buffer.All(i => i == 0);
    }

    public interface IUdpReceiver
    {
        public IAsyncEnumerable<UdpReceiveResult> WaitForReads();
    }

    public sealed class UdpReceiver: IUdpReceiver, IDisposable
    {
        private readonly UdpClient client;
        public async IAsyncEnumerable<UdpReceiveResult> WaitForReads()
        {
            while (true)
            {
                UdpReceiveResult result;
                try
                {
                    result = await client.ReceiveAsync();
                }
                catch (ObjectDisposedException ) // cancellation occurred
                {
                    yield break;
                }

                yield return result;
            }
            
        }
        
        public UdpReceiver(int port) => client = new UdpClient(port, AddressFamily.InterNetwork);
        public void Dispose() => client.Dispose();
    }
}