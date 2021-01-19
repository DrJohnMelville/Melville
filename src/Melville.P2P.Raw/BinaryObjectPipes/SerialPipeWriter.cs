using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipelines;
using System.Text;
using Melville.MacroGen;

namespace Melville.P2P.Raw.BinaryObjectPipes
{
    public static class SequenceReaderExtensions
    {
        public static bool TryRead(this ref SequenceReader<byte> reader, [NotNullWhen(true)] out string? ret)
        {
            ret = null; 
            if (!reader.TryReadLittleEndian(out short len)) return false;
            if (reader.UnreadSpan.Length < len) return false;
            ret = Encoding.UTF8.GetString(reader.UnreadSpan[..len]);
            reader.Advance(len);
            return true;
        }

        public static bool TryReadLittleEndian(this ref SequenceReader<byte> reader, out ushort value)
        {
            value = 0;
            var baseSpan = reader.UnreadSpan;
            if (baseSpan.Length < 2) return false;
            value = BitConverter.ToUInt16(baseSpan[..2]);
            reader.Advance(2);
            return true;
        }
    }
    public ref partial struct SerialPipeWriter
    {
        private readonly PipeWriter writer;
        private readonly Span<byte> buffer;
        private int position;

        private Span<byte> NextPosition() => buffer[position..];

        public SerialPipeWriter(PipeWriter writer, int bytesToReserve)
        {
            this.writer = writer;
            buffer = writer.GetMemory(bytesToReserve).Span;
            position = 0;
        }

        [MacroCode(@"public void Write(~0~ value)
{
    System.BitConverter.TryWriteBytes(NextPosition(), value);
    position += sizeof(~0~);
}")]
        [MacroItem("ushort")]
        [MacroItem("short")]
        public void Write(int value)
        {
            BitConverter.TryWriteBytes(NextPosition(), value);
            position += sizeof(int);
        }
        public void Write(uint value)
        {
            BitConverter.TryWriteBytes(NextPosition(), value);
            position += sizeof(uint);
        }
        public void Write(long value)
        {
            BitConverter.TryWriteBytes(NextPosition(), value);
            position += sizeof(long);
        }
        public void Write(ulong value)
        {
            BitConverter.TryWriteBytes(NextPosition(), value);
            position += sizeof(ulong);
        }
        public void Write(double value)
        {
            BitConverter.TryWriteBytes(NextPosition(), value);
            position += sizeof(double);
        }

        public void Write(string s)
        {
            var stringLen = Encoding.UTF8.GetByteCount(s);
            Write((short)stringLen);
            Encoding.UTF8.GetBytes(s, NextPosition());
            position += stringLen;
        }

        public void Dispose() => writer.Advance(position);
    }
}