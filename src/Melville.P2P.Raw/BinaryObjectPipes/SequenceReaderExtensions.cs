using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Melville.INPC;

namespace Melville.P2P.Raw.BinaryObjectPipes
{
    public static partial class SequenceReaderExtensions
    {
        [MacroItem("ushort", "ToUInt16")]
        [MacroItem("uint", "ToUInt32")]
        [MacroItem("ulong", "ToUInt64")]
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
}