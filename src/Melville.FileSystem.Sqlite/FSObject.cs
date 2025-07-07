namespace Melville.FileSystem.Sqlite;

public class FSObject
{
    public required long Id { get; set; }
    public required string Name { get; init; }
    public required long? Parent { get; init; }
    public required long CreatedTime { get; init; }
    public required long LastWrite { get; init; }
    public required FileAttributes Attributes { get; init; }
    public required long Length { get; init; }
    public required long BlockSize { get; init; }
}