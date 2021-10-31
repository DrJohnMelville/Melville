using System;
using System.Threading.Tasks;
using Melville.P2P.Raw.BinaryObjectPipes;
using Melville.P2P.Raw.Discovery;
using Melville.P2P.Raw.NetworkPrimatives;

namespace Melville.P2P.Raw.Matchmaker;

public sealed class MatchMakerClient: IDisposable
{
    private readonly DiscoveryClient client;
    private readonly BinaryObjectDictionary dictionary;
    private UdpReceiver udpReceiver;

    public MatchMakerClient(int discoveryPort, BinaryObjectDictionary dictionary)
    {
        udpReceiver = new UdpReceiver(discoveryPort+1);
        this.client = new DiscoveryClient(new UdpBroadcaster(discoveryPort), udpReceiver);
        this.dictionary = dictionary;
    }

    public async Task<(BinaryObjectPipeReader, BinaryObjectPipeWriter)> Connect()
    {
        var addr = await client.Connect();
        var networkStream = (await TcpClientFactory.CreateTcpClient(addr)).GetStream();
        return (new BinaryObjectPipeReader(networkStream, dictionary),
            new BinaryObjectPipeWriter(networkStream, dictionary));
    }

    public void Dispose()
    {
        udpReceiver.Dispose();
    }
}