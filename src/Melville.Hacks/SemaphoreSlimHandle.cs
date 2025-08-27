using System;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.Hacks;

public static class SemaphoreSlimHandle
{
    public static async ValueTask<SemaphoreReleaser> WaitForHandleAsync(this SemaphoreSlim semaphore)
    {
        await semaphore.WaitAsync();
        return new(semaphore);
    }

    public static SemaphoreReleaser WaitForHandle(this SemaphoreSlim semaphore)
    {
        semaphore.Wait();
        return new(semaphore);
    }

    public readonly struct SemaphoreReleaser(SemaphoreSlim semaphore): IDisposable
    {
        public void Dispose() => semaphore.Release();
    }
}