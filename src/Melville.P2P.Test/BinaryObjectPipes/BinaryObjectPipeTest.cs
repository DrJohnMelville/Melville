using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Threading.Tasks;
using Melville.P2P.Raw.BinaryObjectPipes;
using Xunit;

namespace Melville.P2P.Test.BinaryObjectPipes;

public class BinaryObjectPipeTest
{
    public class FakeObject : ICanWriteToPipe
    {
        public void WriteToPipe(PipeWriter write)
        {
            var mem = write.GetMemory(1);
            mem.Span[0] = 73;
            write.Advance(1);
        }

        public static object? Read(ref SequenceReader<byte> reader)
        {
            if (!reader.TryRead(out var b)) return null;
            if (b != 73)
            {
                throw new InvalidOperationException("Invalid Data");
            }
            return new FakeObject();
        }
    }

    private readonly BinaryObjectDictionary dict = new();
    private readonly MemoryStream ms = new();
    private readonly BinaryObjectPipeWriter writer;
    private BinaryObjectPipeReader reader;

    public BinaryObjectPipeTest()
    {
        dict.Register<FakeObject>(FakeObject.Read);
        writer = new BinaryObjectPipeWriter(ms, dict);
        reader = new BinaryObjectPipeReader(ms, dict);
    }

    [Fact]
    public async Task WriteSingle()
    {
        await writer.Write(new FakeObject());
        var arr = ms.GetBuffer();
        Assert.Equal(0, arr[0]);
        Assert.Equal(73, arr[1]);
    }
    [Fact]
    public async Task WriteTwo()
    {
        await writer.Write(new FakeObject());
        await writer.Write(new FakeObject());
        var arr = ms.GetBuffer();
        Assert.Equal(0, arr[0]);
        Assert.Equal(73, arr[1]);
        Assert.Equal(0, arr[2]);
        Assert.Equal(73, arr[3]);
    }

    [Fact]
    public async Task ReadOne()
    {
        ms.Write(new byte[]{0,73}.AsSpan());
        ms.Seek(0, SeekOrigin.Begin);
        int iters = 0;
        await foreach (var item in reader.Read())
        {
            Assert.True(item is FakeObject);
            iters++;
        }

        Assert.Equal(1, iters);
            
    }
    [Fact]
    public async Task ReadTwo()
    {
        ms.Write(new byte[]{0,73,0,73}.AsSpan());
        ms.Seek(0, SeekOrigin.Begin);
        int iters = 0;
        await foreach (var item in reader.Read())
        {
            Assert.True(item is FakeObject);
            iters++;
        }

        Assert.Equal(2, iters);
            
    }
}