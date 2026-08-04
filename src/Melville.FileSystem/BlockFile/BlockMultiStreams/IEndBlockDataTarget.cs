namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public interface IEndBlockDataTarget
{
    void EndStreamWrite(in StreamEnds ends, long length);
    public void EndStreamRead();
}
