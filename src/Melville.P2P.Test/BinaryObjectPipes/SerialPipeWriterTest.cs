using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Melville.P2P.Raw.BinaryObjectPipes;
using Xunit;

namespace Melville.P2P.Test.BinaryObjectPipes
{
    public class SerialPipeWriterTest
    {
        private delegate void SerialPipeWriterAction(ref SerialPipeWriter spw);
        private ValueTask<FlushResult> WriteToBuff(byte[] buff, SerialPipeWriterAction writeFunc)
        {
            var writer = PipeWriter.Create(new MemoryStream(buff));
            var spw = new SerialPipeWriter(writer, buff.Length);
            writeFunc(ref spw);
            spw.Dispose();
            return writer.FlushAsync();
        }

        private delegate bool ReadFromBuffFunc<T>(ref SequenceReader<byte> sr, out T value);

        private (bool success, T, int position) ReadFromBuff<T>(byte[] buff, ReadFromBuffFunc<T> reader)
        {
            var sr = new SequenceReader<byte>(new ReadOnlySequence<byte>(buff));
            var success = reader(ref sr, out T value);
            return (success, value, sr.Position.GetInteger());
        }

        [Theory]
        [InlineData(1,1,0)]
        [InlineData(2,2,0)]
        [InlineData(257,1,1)]
        [InlineData(ushort.MinValue,0,0)]
        [InlineData(ushort.MaxValue, 255, 255)]
        public async Task WriteUshort(ushort value, byte first, byte second)
        {
            var buff = new byte[20];
            await WriteToBuff(buff, (ref SerialPipeWriter spw) => spw.Write(value));
            Assert.Equal(first, buff[0]);
            Assert.Equal(second, buff[1]);

            var (success, newValue, nextPosition) = ReadFromBuff(buff, 
                (ref SequenceReader<byte> sr, out ushort readVal) => sr.TryReadLittleEndian(out readVal));
            Assert.True(success);
            Assert.Equal(value, newValue);
            Assert.Equal(2, nextPosition);
        }

        [Theory]
        [InlineData(1,1,0)]
        [InlineData(2,2,0)]
        [InlineData(257,1,1)]
        [InlineData(short.MinValue,0,128)]
        [InlineData(-1,255,255)]
        [InlineData(short.MaxValue, 255, 127)]
        public async Task WriteShort(short value, byte first, byte second)
        {
            var buff = new byte[20];
            await WriteToBuff(buff, (ref SerialPipeWriter spw) => spw.Write(value));
            Assert.Equal(first, buff[0]);
            Assert.Equal(second, buff[1]);

            var (success, newValue, nextPosition) = ReadFromBuff(buff, 
                (ref SequenceReader<byte> sr, out short readVal) => sr.TryReadLittleEndian(out readVal));
            Assert.True(success);
            Assert.Equal(value, newValue);
            Assert.Equal(2, nextPosition);
        }

        [Fact]
        public void TwoSmallBuffer()
        {
            var buff = new byte[1];
            var (success, newValue, nextPosition) = ReadFromBuff(buff, 
                (ref SequenceReader<byte> sr, out ushort readVal) => sr.TryReadLittleEndian(out readVal));
            Assert.False(success);
            Assert.Equal(0, newValue);
            Assert.Equal(0, nextPosition);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData("Foo")]
        [InlineData("THIS is a very very very very long string")]
        public async Task WriteString(string value)
        {
            var buff = new byte[value.Length+5];
            await WriteToBuff(buff, (ref SerialPipeWriter spw) => spw.Write(value));

            var (success, newValue, nextPosition) = ReadFromBuff(buff, 
                (ref SequenceReader<byte> sr, out string? readVal) => sr.TryRead(out readVal));
            Assert.True(success);
            Assert.Equal(value, newValue);
            Assert.Equal(2+value.Length, nextPosition);
        }


    }
}