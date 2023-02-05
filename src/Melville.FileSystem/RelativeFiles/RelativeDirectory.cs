using System;

namespace Melville.FileSystem.RelativeFiles;

public sealed class RelativeDirectory : RelativeDirectoryBase
{
    private readonly Func<IDirectory> root;
    private readonly string name;

    public RelativeDirectory(Func<IDirectory> root, string name)
    {
        this.root = root;
        this.name = name;
    }

    protected override IDirectory GetTargetDirectory() => root().SubDirectory(name);
}