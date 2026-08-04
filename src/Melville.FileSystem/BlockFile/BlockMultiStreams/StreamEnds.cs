namespace Melville.FileSystem.BlockFile.BlockMultiStreams;

public record struct StreamEnds(uint Start, uint End)
{
    public static StreamEnds Invalid => new(0xFFFFFFFF, 0xFFFFFFFF);
    public bool IsValid() => Start != 0xFFFFFFFF && End != 0xFFFFFFFF;
}
