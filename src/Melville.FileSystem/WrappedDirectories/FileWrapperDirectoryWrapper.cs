using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melville.FileSystem.RelativeFiles;
using Melville.INPC;

namespace Melville.FileSystem.WrappedDirectories;

public interface IFileWrapper
{
    IFile WrapFile(IFile source, IDirectory? wrappedDirectory);
    IDirectory WrapDirectory(IDirectory source, IDirectory wrappedParent);
}

public static class WrappedDirectoryOperations
{
    public static IDirectory WrapWith(this IDirectory inner, IFileWrapper wrapper) =>
        new FileWrapperDirectoryWrapper(inner, wrapper);
}

public abstract partial class WrappedDirectoryBase : IDirectory
{
    [DelegateTo(WrapWith = nameof(WrapChild))]
    protected abstract IDirectory GetTargetDirectory();

    //Two methods that do not get wrapped as relative outputs
    public virtual IFile FileFromRawPath(string path) => GetTargetDirectory().FileFromRawPath(path);
    public virtual IDirectory? Directory => GetTargetDirectory().Directory;

    // methods to wrap the various output types.
    private IEnumerable<IFile> WrapChild(IEnumerable<IFile> arg) =>
        arg.Select(WrapChild);

    private IEnumerable<IDirectory> WrapChild(IEnumerable<IDirectory> arg) =>
        arg.Select(WrapChild);

    protected abstract IFile WrapChild(IFile arg);
    protected abstract IDirectory WrapChild(IDirectory arg);

}

public partial class FileWrapperDirectoryWrapper: WrappedDirectoryBase
{
    [FromConstructor] private readonly IDirectory innerDirectory;
    [FromConstructor] private readonly IDirectory? parent;
    [FromConstructor] private readonly IFileWrapper wrapper;

    public FileWrapperDirectoryWrapper(IDirectory innerDirectory, IFileWrapper wrapper) :
        this(innerDirectory, null, wrapper)
    {
    }

    public override IDirectory? Directory => innerDirectory.Directory;

    protected override IFile WrapChild(IFile file) => wrapper.WrapFile(file, this);
    protected override IDirectory WrapChild(IDirectory dir) => wrapper.WrapDirectory(dir, this);
    protected override IDirectory GetTargetDirectory() => innerDirectory;
}
