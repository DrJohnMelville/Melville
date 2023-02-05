using System.Collections.Generic;
using System.Linq;
using Melville.FileSystem.WrappedDirectories;
using Melville.INPC;

namespace Melville.FileSystem.RelativeFiles;

public abstract partial class RelativeDirectoryBase : WrappedDirectoryBase
{
    protected override IFile WrapChild(IFile arg) =>
        new RelativeFile(GetTargetDirectory, arg.Name);

    protected override IDirectory WrapChild(IDirectory arg) =>
        new RelativeDirectory(GetTargetDirectory, arg.Name);
}