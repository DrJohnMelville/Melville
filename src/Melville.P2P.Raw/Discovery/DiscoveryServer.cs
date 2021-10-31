using System.Threading.Tasks;
using Melville.P2P.Raw.NetworkPrimatives;

namespace Melville.P2P.Raw.Discovery;

public interface IDiscoveryServer
{
    Task AcceptConnections();
}
    
public class DiscoveryServer : IDiscoveryServer
{
    private readonly IUdpBroadcaster broadcast;
    private readonly IUdpReceiver receiver;
    private readonly byte[] targetAddress;

    public DiscoveryServer(IUdpBroadcaster broadcast, IUdpReceiver receiver, byte[] targetAddress)
    {
        this.broadcast = broadcast;
        this.receiver = receiver;
        this.targetAddress = targetAddress;
    }

    public async Task AcceptConnections()
    {
        await SendServerAddress();
        await foreach (var packet in receiver.WaitForReads())
        {
            if (packet.IsEmptyTargetAddress())
            {
                await SendServerAddress();
            }
        }
    }
    private Task<int> SendServerAddress() => broadcast.Send(targetAddress);
}