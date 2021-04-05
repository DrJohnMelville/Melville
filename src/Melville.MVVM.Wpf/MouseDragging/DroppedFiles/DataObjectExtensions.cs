using  System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows;
using Melville.FileSystem;
using Melville.MVVM.Wpf.MouseDragging.Drop;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles
{
  public static class DataObjectReaderExtensions
    {
      public const int FileGroupDescriptorNameLocation = 75;

      public static IEnumerable<IFile> GetDroppedFiles(this System.Windows.IDataObject item, Func<string, IFile> fileFromDiskPath)
      {
        var fileNames = (item.GetData(DataFormats.FileDrop) as string[]) ?? new string[0];
        return fileNames.Length > 0 ? fileNames.Select(fileFromDiskPath) : item.GetFileDescriptorFiles();
      }

      public static IEnumerable<IFile> GetDroppedFiles(this IDropInfo item, Func<string, IFile> fileFromDiskPath) => item.Item.GetDroppedFiles(fileFromDiskPath);


      public static IEnumerable<IFile> GetFileDescriptorFiles(this System.Windows.IDataObject data)
      {
        if (data == null || !data.GetDataPresent(WideFileGroupDescriptorFormat)) return new IFile[0];
        return NamesWithPositions(data).Select(i => new LazyStreamFile(i.Item2, () => GetFileByIndex(data, i.Item1)));
      }


      #region FileDescriptorStream Methods
      private static IEnumerable<Tuple<int, string>> NamesWithPositions(this System.Windows.IDataObject target)
      {
        var files = InnerGetNamesWithPosition(target,
          WideFileGroupDescriptorFormat, Encoding.Unicode, 0x250).ToArray();
        if (files.Any())
        {
          return files;
        }
        return InnerGetNamesWithPosition(target,
          NarrowFileGroupDescriptorFormat, Encoding.ASCII, 0x14C);
      }
      private static IEnumerable<Tuple<int, string>> InnerGetNamesWithPosition(System.Windows.IDataObject target,
        string groupDescriptorTitle, Encoding encoding, int recordSize)
      {
        using (var inputStream = (MemoryStream)target.GetData(
          groupDescriptorTitle))
        {
          int count = inputStream.ReadByte();

          for (int i = 0; i < count; i++)
          {
            var buffer = new byte[recordSize];
            int count1 = buffer.Length;
            inputStream.FillBuffer(buffer, 0, count1);
            var end = FileGroupDescriptorNameLocation;
            while (buffer[end] != 0 || buffer[end + 1] != 0)
            {
              end += 2;
            }
            var retStr = encoding.GetString(buffer, FileGroupDescriptorNameLocation,
              end - FileGroupDescriptorNameLocation);
            yield return Tuple.Create(i, retStr);
          }
        }
      }

      private static class NativeMethods
      {
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern uint RegisterClipboardFormat(string lpszFormat);
      }
      // this must be a cast and not a ConvertTo call because we want an unchecked
      // unsigned to signed conversion
      private static readonly short FileContentIndex =
        (short)(NativeMethods.RegisterClipboardFormat(FileContentsDescriptorFormat));
      public static Stream GetFileByIndex(System.Windows.IDataObject target, int position)
      {
        var comDataObject = (System.Runtime.InteropServices.ComTypes.IDataObject)target;
        FORMATETC fmtCC = CreateStructToRequestStream(position);
        STGMEDIUM result = new STGMEDIUM();
        comDataObject.GetData(ref fmtCC, out result);
        return result.ExtractFileStream();
      }
      public static FORMATETC CreateStructToRequestStream(int position)
      {
        return new FORMATETC
        {
          cfFormat = FileContentIndex,
          dwAspect = DVASPECT.DVASPECT_CONTENT,
          lindex = position,
          ptd = IntPtr.Zero,
          tymed = TYMED.TYMED_ISTREAM
        };
      }
      #endregion

      private const string WideFileGroupDescriptorFormat = "FileGroupDescriptorW";
      private const string NarrowFileGroupDescriptorFormat = "FileGroupDescriptor";
      private const string FileContentsDescriptorFormat = "FileContents";
    }
}