using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Melville.FileSystem.BlockFile.FileSystemObjects;

public static class BinaryIo
{
    public static void WriteEncodedString(
        this PipeWriter writer, string name)
    {
        var length = Encoding.UTF8.GetByteCount(name);

        writer.WriteCompactUint((uint)length);
        var bytes = writer.GetSpan(length);
        if (!Encoding.UTF8.TryGetBytes(name, bytes, out _))
            throw new InvalidOperationException(
                "Unable to encode string to utf8");
        writer.Advance(length);
    }
    public static void WriteCompactUint(
        this PipeWriter writer, uint number)
    {
        switch (number)
        {
            case < 254:
                writer.WriteSizedUint(1, (byte)number);
                break;
            case < ushort.MaxValue:
                writer.WriteSizedUint(1, (byte)254);
                writer.WriteSizedUint(2, (ushort)number);
                break;
            default:
                writer.WriteSizedUint(1, (byte)255);
                writer.WriteSizedUint(4, number);
                break;
        }
    }

    public static void WriteSizedUint<T>(this PipeWriter pw, int bytes,
        T datum) where T: unmanaged
    {
        var buffer = pw.GetSpan(bytes);
        MemoryMarshal.Cast<byte, T>(buffer)[0] = datum;
        pw.Advance(bytes);
    }

    public static async ValueTask<uint> ReadCompactUint(this PipeReader input)
    {
        var readResult = await input.ReadAsync();
        var indicator = readResult.Buffer.FirstSpan[0];
        input.AdvanceTo(readResult.Buffer.GetPosition(1));
        return indicator switch
        {
            <254 => indicator,
            254 => await input.ReadUintBySize<ushort>(2),
            255 => await input.ReadUintBySize<uint>(4)
        };
    }

    public static async ValueTask<T> ReadUintBySize<T>(
        this PipeReader r, int bytes) where T: unmanaged
    {
        var readResult = await r.ReadAtLeastAsync(bytes);
        var ret = MemoryMarshal.Cast<byte, T>(
            readResult.Buffer.MinSpan(stackalloc byte[bytes]))[0];
        r.AdvanceTo(readResult.Buffer.GetPosition(bytes));
        return ret;
    }

    public static ReadOnlySpan<T> MinSpan<T>(this ReadOnlySequence<T> source,
        Span<T> scratch)
    {
        if (source.FirstSpan.Length >= scratch.Length)
            return source.FirstSpan[..scratch.Length];
        var destSpan = scratch;
        foreach (var srcSpan in source)
        {
            var size = Math.Min(srcSpan.Length, destSpan.Length);
            srcSpan.Span[..size].CopyTo(destSpan);
            destSpan = destSpan[size..];
            if (destSpan.IsEmpty) break;
        }

        return scratch;
    }
}