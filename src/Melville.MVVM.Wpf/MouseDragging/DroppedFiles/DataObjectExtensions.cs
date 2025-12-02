using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Windows;
using Windows.Win32.UI.Shell;
using Melville.FileSystem;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Windows.Win32;
using Melville.Hacks.Reflection;
using IDataObject = System.Windows.IDataObject;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

public static class DataObjectReaderExtensions
{
    public static IEnumerable<IFile> GetDroppedFiles(this IDataObject item,
        Func<string, IFile> fileFromDiskPath) =>
        item.GetData(DataFormats.FileDrop) is string[]{Length:>0} fileNames ?
            fileNames.Select(fileFromDiskPath) : 
            item.GetFileDescriptorFiles();

    public static IEnumerable<IFile> GetDroppedFiles(
        this IDropInfo item, Func<string, IFile> fileFromDiskPath) =>
        item.Item.GetDroppedFiles(fileFromDiskPath);


    public static IEnumerable<IFile> GetFileDescriptorFiles(this IDataObject data)
    {
        return NamesWithPositions(data)
            .Select((name, index) => new LazyStreamFile(name, () => GetFileByIndex(data, index)));
    }


    #region FileDescriptorStream Methods

    private static string[] NamesWithPositions(
        this IDataObject target) =>
        GetWideFileNames(target) is { Length: > 0 } ret ? ret : GetNarrowFileNames(target);

    private static string[] GetWideFileNames(IDataObject target)
    {
        var src = ReadFileNamesBlob(target, StreamingFileClipboardFormats.WideGroup);
        if (src.Length == 0) return [];
        var record = MemoryMarshal.Cast<byte, FILEGROUPDESCRIPTORW>(src)[0].Files();
        return GetFileNames(record);
    }
 
    private static string[] GetNarrowFileNames(IDataObject target)
    {
        var src = ReadFileNamesBlob(target, StreamingFileClipboardFormats.NarrowGroup);
        if (src.Length == 0) return [];
        var record = MemoryMarshal.Cast<byte, FILEGROUPDESCRIPTORA>(src)[0].Files();
        return GetFileNames(record);
    }

    private static Span<byte> ReadFileNamesBlob(IDataObject target, string format)
    {
        return target.GetData(format) switch
        {
            MemoryStream ms => ms.ToArray().AsSpan(),
            byte[] ba => ba.AsSpan(),
            _ => []
        };
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
        (short)DataFormats.GetDataFormat(StreamingFileClipboardFormats.FileContents).Id;

    public static Stream GetFileByIndex(IDataObject target, int position)
    {
         return target switch
        {
            System.Runtime.InteropServices.ComTypes.IDataObject comDataObject => 
                ExtractFromComType(position, comDataObject),
            _=> target.GetData(StreamingFileClipboardFormats.FileContents) as Stream??
                throw new ArgumentNullException("Was not a stream")
        };
    }

    private static Stream ExtractFromComType(int position, System.Runtime.InteropServices.ComTypes.IDataObject comDataObject)
    {
        FORMATETC fmtCC = CreateStructToRequestStream(position);
        STGMEDIUM result = new STGMEDIUM();
        comDataObject.GetData(ref fmtCC, out result);
        return result.ExtractFileStream();
    }

    private static FORMATETC CreateStructToRequestStream(int position) => new()
        {
            cfFormat = FileContentIndex,
            dwAspect = DVASPECT.DVASPECT_CONTENT,
            lindex = position,
            ptd = IntPtr.Zero,
            tymed = TYMED.TYMED_ISTREAM
        };

    #endregion
}