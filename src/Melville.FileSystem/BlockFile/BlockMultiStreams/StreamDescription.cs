namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public record struct StreamDescription(uint Start, uint End, long Length)
{
    public StreamEnds StreamEnds => new(Start, End);

    public static StreamDescription Invalid => new(0xFFFFFFFF, 0xFFFFFFFF, 0);

    public bool Exists() => StreamEnds.IsValid();
}