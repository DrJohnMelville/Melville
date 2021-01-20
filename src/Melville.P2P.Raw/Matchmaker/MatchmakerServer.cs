using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Melville.P2P.Raw.BinaryObjectPipes;
using Melville.P2P.Raw.Discovery;
using Melville.P2P.Raw.NetworkPrimatives;

namespace Melville.P2P.Raw.Matchmaker
{
    public sealed class MatchmakerServer: IDisposable
    {
        private readonly DiscoveryServer discoveryServer;
        private readonly TcpServer tcpServer;
        private readonly BinaryObjectDictionary dictionary;
        private UdpReceiver udpReceiver;

        public MatchmakerServer(int broadcastPort, BinaryObjectDictionary dictionary)
        {
            this.dictionary = dictionary;
            tcpServer = new TcpServer();
            udpReceiver = new UdpReceiver(broadcastPort);
            discoveryServer = new DiscoveryServer(new UdpBroadcaster(broadcastPort+1), udpReceiver,
              tcpServer.AddressArray());
        }

        public async IAsyncEnumerable<(BinaryObjectPipeReader, BinaryObjectPipeWriter)> AcceptConnections()
        {
            var connections = tcpServer.MonitorForConnection();
            GC.KeepAlive(discoveryServer.AcceptConnections());
            await foreach (var connection in connections)
            {
                var networkStream = connection.GetStream();
                yield return (new BinaryObjectPipeReader(networkStream, dictionary),
                    new BinaryObjectPipeWriter(networkStream, dictionary));
            }
        }

        public void Dispose()
        {
            tcpServer.Dispose();
            udpReceiver.Dispose();
        }
    }
}