using System;
using System.IO;
using System.Threading.Tasks;
using Accord;
using Melville.FileSystem;
using Melville.FileSystem.Sqlite;
using Melville.INPC;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.MvvmDialogs;
using OfficeOpenXml.Utils;

namespace Melville.Wpf.Samples.SqliteFileSystem;

public partial class SqliteFsViewModel
{
    [AutoNotify] public partial string FilePath { get; set; } //# = "";
    [AutoNotify] public partial string Console { get; set; } //# = "";
    [AutoNotify] public partial int TotalSize{ get; set; } //# = 50_000_000;
    [AutoNotify] public partial int BufferSize{ get; set; } //# = 5213;

    public void SelectFile([FromServices] IOpenSaveFile osf) => 
        FilePath = osf
            .GetSaveFileName(null, "*.db", "Database (*.db)|*.db", "Pick file name") ?? "";

    public async Task RunTest()
    {
        Console = "";
        var writer = new Writer(TotalSize, BufferSize, WriteLine);
        if (FilePath.Length > 0)
        {
            WriteLine("Write directly to disk file");
            var file = new FileSystemFile(FilePath);
            await writer.RunFile(file, false);
        }
        WriteLine("Write untransacted db blocks");
        using var fs = SqliteFileStore.Create(FilePath);
        var utr = fs.UntransactedRoot("");
        utr.Create();
        await writer.RunFile(utr.File("untrans.txt"), false);
        WriteLine("Transacted File");
        await writer.RunFile(fs.TransactedRoot("").File("trans.txt"), false);
        WriteLine("Transacted File with explicit length");
        await writer.RunFile(fs.TransactedRoot("").File("trans2.txt"), true);
    }

    private void WriteLine(string text) => Console += Environment.NewLine + text;
}

public readonly struct Writer
{
    private readonly int TotalSize;
    private readonly int blocks, extra;
    private readonly int bytes;
    private readonly byte[] data;
    private readonly Action<string> WriteLine;

    public Writer(int totalSize, int bufferSize, Action<string> writeline)
    {
        WriteLine = writeline;
        TotalSize = totalSize;
        data = new byte[bufferSize];
        new Random().NextBytes(data);
        (blocks, extra) = Math.DivRem(totalSize, bufferSize);
    }

    public async Task RunFile(IFile file, bool setSize)
    {
        file.Delete();
        var prior = DateTime.Now;
        await using (var stream = await file.CreateWrite(FileAttributes.Normal))
        {
            if(setSize)
            {
                stream.SetLength(TotalSize);
            }
            for (int i = 0; i < blocks; i++)
            {
                await stream.WriteAsync(data, 0, data.Length);
            }

            if (extra > 0)
            {
                await stream.WriteAsync(data, 0, extra);
            }
        }

        WriteLine($"Wrote {TotalSize} bytes in {DateTime.Now - prior}");
        prior = DateTime.Now;
        await using (var stream = await file.OpenRead())
        {
            while ((await stream.ReadAsync(data.AsMemory())) > 0) ;
        }

        WriteLine($"Read {TotalSize} bytes in {DateTime.Now - prior}");
        file.Delete();
    }

}