using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using Windows.Win32.UI.Shell;
using Melville.FileSystem;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using System.Xml.Linq;
using Windows.Win32;
using IDataObject = System.Windows.IDataObject;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

public static class DataObjectReaderExtensions
{
    public const int FileGroupDescriptorNameLocation = 75;

    public static IEnumerable<IFile> GetDroppedFiles(this System.Windows.IDataObject item,
        Func<string, IFile> fileFromDiskPath) =>
        item.GetData(DataFormats.FileDrop) is string[]{Length:>0} fileNames ?
            fileNames.Select(fileFromDiskPath) : 
            item.GetFileDescriptorFiles();

    public static IEnumerable<IFile> GetDroppedFiles(
        this IDropInfo item, Func<string, IFile> fileFromDiskPath) =>
        item.Item.GetDroppedFiles(fileFromDiskPath);


    public static IEnumerable<IFile> GetFileDescriptorFiles(this System.Windows.IDataObject data)
    {
        return NamesWithPositions(data)
            .Select((name, index) => new LazyStreamFile(name, () => GetFileByIndex(data, index)));
    }


    #region FileDescriptorStream Methods

    private static string[] NamesWithPositions(
        this System.Windows.IDataObject target) =>
        GetWideFileNames(target) is { Length: > 0 } ret ? ret : GetNarrowFileNames(target);

    private static string[] GetWideFileNames(System.Windows.IDataObject target)
    {
        var src = ReadFileNamesBlob(target, WideFileGroupDescriptorFormat);
        if (src.Length == 0) return [];
        var record = MemoryMarshal.Cast<byte, FILEGROUPDESCRIPTORW>(src)[0].Files();
        return GetFileNames(record);
    }
 
    private static string[] GetNarrowFileNames(System.Windows.IDataObject target)
    {
        var src = ReadFileNamesBlob(target, NarrowFileGroupDescriptorFormat);
        if (src.Length == 0) return [];
        var record = MemoryMarshal.Cast<byte, FILEGROUPDESCRIPTORA>(src)[0].Files();
        return GetFileNames(record);
    }

    private static Span<byte> ReadFileNamesBlob(IDataObject target, string format)
    {
        target.GetData(format);
        using var inputStream = (MemoryStream?)target.GetData(format);
        return (inputStream?.ToArray() ?? []).AsSpan();
    }

    private static string[] GetFileNames<T>(ReadOnlySpan<T> record) where T:IFileDescriptor
    {
        var ret = new string[record.Length];
        for (int i = 0; i < ret.Length; i++)
        {
            ret[i] =record[i].NameAsString();
        }
        return ret;
    }

    // this must be a cast and not a ConvertTo call because we want an unchecked
    // unsigned to signed conversion
    private static readonly short FileContentIndex =
        (short)(PInvoke.RegisterClipboardFormat(FileContentsDescriptorFormat));

    public static Stream GetFileByIndex(System.Windows.IDataObject target, int position)
    {
        var comDataObject = (System.Runtime.InteropServices.ComTypes.IDataObject)target;
        FORMATETC fmtCC = CreateStructToRequestStream(position);
        STGMEDIUM result = new STGMEDIUM();
        comDataObject.GetData(ref fmtCC, out result);
        return result.ExtractFileStream();
    }

    public static FORMATETC CreateStructToRequestStream(int position) =>
        new()
        {
            cfFormat = FileContentIndex,
            dwAspect = DVASPECT.DVASPECT_CONTENT,
            lindex = position,
            ptd = IntPtr.Zero,
            tymed = TYMED.TYMED_ISTREAM
        };

    #endregion

    public const string WideFileGroupDescriptorFormat = "FileGroupDescriptorW";
    public const string NarrowFileGroupDescriptorFormat = "FileGroupDescriptor";
    private const string FileContentsDescriptorFormat = "FileContents";
}