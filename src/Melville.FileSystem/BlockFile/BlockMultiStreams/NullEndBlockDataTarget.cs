using Melville.INPC;

namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

[StaticSingleton]
public partial class NullEndBlockDataTarget : IEndBlockDataTarget
{
    /// <inheritdoc />
    public void EndStreamWrite(in StreamEnds ends, long length)
    {
    }

    public void EndStreamRead()
    {
    }
}
