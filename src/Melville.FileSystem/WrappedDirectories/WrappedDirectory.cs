using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melville.INPC;

namespace Melville.FileSystem.WrappedDirectories;

public interface IFileWrapper
{
    IFile WrapFile(IFile source, IDirectory? wrappedDirectory);
    IDirectory wrapDirectory(IDirectory source, IDirectory wrappedParent);
}

public static class WrappedDirectoryOperations
{
    public static IDirectory WrapWith(this IDirectory inner, IFileWrapper wrapper) =>
        new WrappedDirectory(inner, wrapper);
}

#warning move to melville librry
public partial class WrappedDirectory: IDirectory
{
    [DelegateTo] [FromConstructor] internal readonly IDirectory innerDirectory;
    [FromConstructor] private readonly IDirectory? parent;
    [FromConstructor] private readonly IFileWrapper wrapper;

    public WrappedDirectory(IDirectory innerDirectory, IFileWrapper wrapper): this(innerDirectory, null, wrapper)
    {
        
    }

    public IDirectory? Directory => parent ?? innerDirectory.Directory;

    public void Create(FileAttributes attributes = FileAttributes.Directory) => 
        innerDirectory.Create(attributes);

    public IFile File(string name) => WrapFile(innerDirectory.File(name));
    private IFile WrapFile(IFile inner) => wrapper.WrapFile(inner, this);
    public IEnumerable<IFile> AllFiles() => innerDirectory.AllFiles().Select(WrapFile);
    public IEnumerable<IFile> AllFiles(string glob) =>
        innerDirectory.AllFiles(glob).Select(WrapFile);

    public IEnumerable<IDirectory> AllSubDirectories() =>
        innerDirectory.AllSubDirectories().Select(WrapDirectory);
    public IDirectory SubDirectory(string name) => 
        WrapDirectory(innerDirectory.SubDirectory(name));
    private IDirectory WrapDirectory(IDirectory directory) => 
        wrapper.wrapDirectory(directory, this);
}