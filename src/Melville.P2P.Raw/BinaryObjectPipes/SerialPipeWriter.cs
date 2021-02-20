using System;
using System.IO.Pipelines;
using System.Text;
using Melville.INPC;

namespace Melville.P2P.Raw.BinaryObjectPipes
{
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

        [MacroItem("ushort")]
        [MacroItem("short")]
        [MacroItem("int")]
        [MacroItem("uint")]
        [MacroItem("long")]
        [MacroItem("ulong")]
        [MacroItem("double")]
        [MacroItem("System.Single")]
        [MacroCode(@"public void Write(~0~ value)
{
    System.BitConverter.TryWriteBytes(NextPosition(), value);
    position += sizeof(~0~);
}")]

        public void Write(bool b) => Write((byte) (b ? 1 : 0));  
        public void Write(string s)
        {
            var stringLen = Encoding.UTF8.GetByteCount(s);
            Write((short)stringLen);
            Encoding.UTF8.GetBytes(s, NextPosition());
            position += stringLen;
        }
        public void Dispose() => writer.Advance(position);

        public static int SpaceForString(string input) => 2 + Encoding.UTF8.GetByteCount(input);
    }
}