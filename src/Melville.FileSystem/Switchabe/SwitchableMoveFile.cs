using Melville.FileSystem.CopyWithProgress;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.FileSystem.Switchabe;

public class SwitchableMoveFile : SwitchableFileBase
{
  public SwitchableMoveFile(IFile source, IFile destination) : base(source, destination)
  {
  }

  public override async Task<Stream> OpenRead()
  {
    // since move destroys the source, we cannot allow any streams from the source to be connected.
    // just wait for the move to complete before servicing any streams;
    await WaitForFinal;
    return await currentTarget.OpenRead();
  }

  public override Task RelocateOverride() => destination.MoveFrom(currentTarget, CancellationToken.None, FileAttributes.Normal);
}

public class SwitchableCopyFile : SwitchableFileBase
{
    private readonly CopyProgressRoutine progressRoutine;
  public SwitchableCopyFile(IFile source, IFile destination, CopyProgressRoutine? progressRoutine = null) : base(source, destination)
  {
        this.progressRoutine = progressRoutine ?? NoProgressResult.Handler;
    }
  private List<SwitchableStream> streams = new List<SwitchableStream>();

  public override Task<Stream> OpenRead() =>
    IsSwitched ? currentTarget.OpenRead() : CreateSwitchableStream();

  private async Task<Stream> CreateSwitchableStream()
  {
    var ret = new SwitchableStream(await currentTarget.OpenRead());
    streams.Add(ret);
    return ret;
  }

    public override Task RelocateOverride() =>
        destination.CopyFrom(currentTarget, CancellationToken.None, FileAttributes.Normal, progressRoutine);

    protected override async Task CleanupAfterSwitch()
  {
    foreach (var stream in streams.ToList())
    {
      await stream.SwitchSource(currentTarget.OpenRead);
    }
  }
}