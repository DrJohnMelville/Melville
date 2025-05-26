using System;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Win32.UI.Shell;
using Melville.INPC;
using Melville.Lists.PersistentLinq;
using IDataObject = System.Windows.IDataObject;

namespace Melville.MVVM.Wpf.MouseDragging.DroppedFiles;

public static partial class PushFilesAsStreams
{
    public static void PushStreams(this ComDataObject dataObj,
           params ReadOnlySpan<(string Name, Stream Data)> files)
       {
           if (files.Length == 0) return;
           int filesLength = files.Length;
           var wideBuffer = new NameBuffer<FILEDESCRIPTORW>(filesLength);
           var narrowBuffer = new NameBuffer<FILEDESCRIPTORA>(filesLength);

           for (int i = files.Length-1; i >= 0; i--)
           {
               dataObj.SetData(StreamingFileClipboardFormats.FileContents,
                   files[i].Data, i);
               wideBuffer.Files[i].SetData(files[i].Name);
               narrowBuffer.Files[i].SetData(files[i].Name);
           }
           dataObj.SetData(StreamingFileClipboardFormats.WideGroup, wideBuffer.Buffer);
           dataObj.SetData(StreamingFileClipboardFormats.NarrowGroup, narrowBuffer.Buffer);
       }

    internal readonly ref struct NameBuffer<T> where T: unmanaged
    {
        public byte[] Buffer { get; }
        public Span<T> Files { get; }

        public unsafe NameBuffer(int length)
        {
            Buffer = new byte[(sizeof(T) * length) + 4];
            MemoryMarshal.Cast<byte, int>(Buffer)[0] = length;
            Files = MemoryMarshal.Cast<byte, T>(Buffer.AsSpan(4));
        }
    }
}