using System.Threading;

namespace Melville.Log.Viewer.NamedPipeServers;

public interface IShutdownMonitor
{
    CancellationToken CancellationToken { get; }
    void InitiateShutdown();
}
public class ShutdownMonitor:IShutdownMonitor
{
    private CancellationTokenSource tokenSource = new CancellationTokenSource();

    public CancellationToken CancellationToken => tokenSource.Token;
    public void InitiateShutdown()
    {
        tokenSource.Cancel(true);
    }
}