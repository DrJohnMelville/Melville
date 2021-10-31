using System;
using System.Threading.Tasks;
using Melville.P2P.Raw.Discovery;
using Melville.P2P.Raw.NetworkPrimatives;
using Xunit;

namespace Melville.P2P.Test.Discovery;

public class TcpServerTest: IDisposable
{
    private TcpServer server = new TcpServer();

    public void Dispose() => server.Dispose();
    [Fact]
    public async Task ConnectTcp()
    {
        await using var iterator = server.MonitorForConnection().GetAsyncEnumerator();
        var task = iterator.MoveNextAsync();
        using var client = await TcpClientFactory.CreateTcpClient(server.AddressArray());
        Assert.True(await task);
        Assert.NotNull(iterator.Current);
    }
}