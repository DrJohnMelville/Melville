using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text;
using Melville.INPC;

namespace Melville.P2P.Raw.BinaryObjectPipes;

public static partial class SequenceReaderExtensions
{
    [MacroItem("double", "ToDouble")]
    [MacroItem("System.Single", "ToSingle")]
    [MacroCode(
        @"public static bool TryReadLittleEndian(this ref System.Buffers.SequenceReader<byte> reader, out ~0~ value)
    {
        value = 0;
        var baseSpan = reader.UnreadSpan;
        if (baseSpan.Length < sizeof(~0~)) return false;
        value = System.BitConverter.~1~(baseSpan);
        reader.Advance(sizeof(~0~));
        return true;
    }")]
    public static bool TryReadLittleEndian(this ref SequenceReader<byte> reader, out byte value) => 
        reader.TryRead(out value);

    public static bool TryReadLittleEndian(this ref SequenceReader<byte> reader, out bool value)
    {
        value = false;
        if (!reader.TryRead(out byte valueByte)) return false;
        value = valueByte != 0;
        return true;
    }

    public static bool TryReadLittleEndian<T>(this ref SequenceReader<byte> reader, out T value)
        where T : IBinaryInteger<T>, IUnsignedNumber<T>
    {  
        var len = T.AdditiveIdentity.GetByteCount();
        Span<byte> bytes = stackalloc byte[len];
        if (!reader.TryCopyTo(bytes))
        {
            value = T.Zero;
            return false;
        }
        reader.Advance(len);
        return T.TryReadLittleEndian(bytes, T.Zero is IUnsignedNumber<T>, out value);
    }

    public static bool TryRead(this ref SequenceReader<byte> reader, 
        [NotNullWhen(true)] out string? ret)
    {
        ret = null;
        if (!reader.TryReadLittleEndian(out short len)) return false;
        if (reader.UnreadSpan.Length < len) return false;
        ret = Encoding.UTF8.GetString(reader.UnreadSpan[..len]);
        reader.Advance(len);
        return true;
    }
}