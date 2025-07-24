using System.IO;
using System.Threading.Tasks;
using Melville.INPC;

namespace Melville.FileSystem.WrappedDirectories;

public interface IStreamWrapper
{
    Stream WrapReadingStream(IFile file, Stream stream);
    Stream WrapWritingStream(IFile file, Stream stream);
}

public class FileToStreamWrapper(IStreamWrapper inner) : IFileWrapper
{
    /// <inheritdoc />
    public IFile WrapFile(IFile source, IDirectory? wrappedDirectory) => 
        new StreamWrappingFile(source, inner);

    /// <inheritdoc />
    public IDirectory WrapDirectory(IDirectory source, IDirectory wrappedParent) =>
        source.WrapWith(this);
}

public partial class StreamWrappingFile : IFile
{
    [FromConstructor][DelegateTo] private readonly IFile innerFile;
    [FromConstructor] private readonly IStreamWrapper streamWrapper;

    /// <inheritdoc />
    public async Task<Stream> OpenRead() => 
        streamWrapper.WrapReadingStream(innerFile, await innerFile.OpenRead());

    /// <inheritdoc />
    public async Task<Stream> CreateWrite(FileAttributes attributes = FileAttributes.Normal) =>
        streamWrapper.WrapWritingStream(innerFile, await innerFile.CreateWrite(attributes));
}