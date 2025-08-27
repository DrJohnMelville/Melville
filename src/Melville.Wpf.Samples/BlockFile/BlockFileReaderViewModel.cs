using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Melville.FileSystem;
using Melville.INPC;
using Melville.Linq;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;

namespace Melville.Wpf.Samples.BlockFile;

public partial class BlockFileReaderViewModel
{
    [AutoNotify] public partial Stream? Stream { get; private set; }
    [AutoNotify] public partial uint BlockSize { get; private set; }
    [AutoNotify] public partial uint FreeHead { get; private set; }
    [AutoNotify] public partial uint RootDir { get; private set; }
    [AutoNotify] public partial uint NextBlock { get; private set; }
    [AutoNotify] public partial uint DisplayBlock { get; set; } //# = 0xFFFFFFFFu;
    [AutoNotify] public partial uint BlockNextField { get; set; }
    [AutoNotify] public partial string Dump { get; set; } //# = "";

    public async Task LoadFile([FromServices]IOpenSaveFile dlg)
    {
        await (Stream?.DisposeAsync() ?? ValueTask.CompletedTask);
        Stream = null;
        var file = dlg.GetLoadFile(null, ".db", "Block File|*.db", "Open Block File");
        if (file is null) return;
        Stream = await file.OpenRead();
        await ReadHeaderAsync();
    }

    private async Task ReadHeaderAsync()
    {
        if (Stream is null) return;
        var buffer = new byte[16];
        await Stream.ReadExactlyAsync(buffer, 0, 16);
        var sizes = MemoryMarshal.Cast<byte, uint>(buffer.AsSpan());
        BlockSize = sizes[0];
        FreeHead = sizes[1];
        RootDir = sizes[2];
        NextBlock = sizes[3];
    }

    public async void OnDisplayBlockChanged()
    {
        if (Stream is null) return;
        var buffer = new byte[BlockSize];
        Stream.Seek(16 + (DisplayBlock * BlockSize), SeekOrigin.Begin);
        await Stream.ReadExactlyAsync(buffer, 0, (int)BlockSize);
        Dump = buffer.BinaryDump();
        await Stream.ReadExactlyAsync(buffer, 0, (int)BlockSize);

        BlockNextField = MemoryMarshal.Cast<byte, uint>(buffer.AsSpan())[^1];
    }
}