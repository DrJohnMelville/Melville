using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Documents;
using System.Windows.Shell;
using Windows.Win32.Foundation;

namespace Windows.Win32.UI.Shell;


internal static class FileGroupDescriptorExtensions
{
    public static ReadOnlySpan<FILEDESCRIPTORW> Files(this ref FILEGROUPDESCRIPTORW desc)
    {
        return desc.fgd.AsSpan((int)desc.cItems);
    }
    public static ReadOnlySpan<FILEDESCRIPTORA> Files(this ref FILEGROUPDESCRIPTORA desc)
    {
        return desc.fgd.AsSpan((int)desc.cItems);
    }
}


internal interface IFileDescriptor
{
    string NameAsString();
}

internal partial struct FILEDESCRIPTORW: IFileDescriptor
{
    public readonly string NameAsString() => NonZeros(cFileName.AsReadOnlySpan()).ToString();

    private readonly ReadOnlySpan<char> NonZeros(ReadOnlySpan<char> asReadOnlySpan) =>
        (asReadOnlySpan.IndexOf((char)0) is var index and >= 0) ? 
            asReadOnlySpan[..index] : asReadOnlySpan;

    public void SetData(string fileName)
    {
        dwFlags = 0x8000_0000;
        var span = cFileName.AsSpan();
        span.Fill((char)0);
        if (fileName.Length > span.Length)
            throw new InvalidOperationException("File name is too long to drag.");
        fileName.AsSpan().CopyTo(span);
    }
}

internal partial struct FILEDESCRIPTORA: IFileDescriptor
{
    public readonly string NameAsString() =>
        Encoding.UTF8.GetString(
            NonZeros(
                MemoryMarshal.Cast<CHAR, byte>(cFileName.AsReadOnlySpan())));

    private readonly ReadOnlySpan<byte> NonZeros(ReadOnlySpan<byte> asReadOnlySpan) =>
        (asReadOnlySpan.IndexOf((byte)0) is var index and >= 0) ? 
            asReadOnlySpan[..index] : asReadOnlySpan;

    public void SetData(string fileName)
    {
        dwFlags = 0x8000_0000;
        var span = MemoryMarshal.Cast<CHAR, byte>(cFileName.AsSpan());
        span.Fill((byte)0);
        if (fileName.Length > span.Length)
            throw new InvalidOperationException("File name is too long to drag.");
        Encoding.UTF8.GetBytes(fileName.AsSpan(), span);
    }
}