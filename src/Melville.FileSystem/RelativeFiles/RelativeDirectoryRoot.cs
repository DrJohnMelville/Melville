namespace Melville.FileSystem.RelativeFiles;

public sealed class RelativeDirectoryRoot : RelativeDirectoryBase
{
    public IDirectory BaseDirectory { get; set; }

    public RelativeDirectoryRoot(IDirectory baseDirectory)
    {
        BaseDirectory = baseDirectory;
    }

    protected override IDirectory GetTargetDirectory() => BaseDirectory;
}