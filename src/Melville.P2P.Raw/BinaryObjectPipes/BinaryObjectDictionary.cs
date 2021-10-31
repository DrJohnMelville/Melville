using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;

namespace Melville.P2P.Raw.BinaryObjectPipes;

public interface ICanWriteToPipe
{
    void WriteToPipe(PipeWriter write);
}

public delegate object? ReaderFunc(ref SequenceReader<byte> b);

public class BinaryObjectDictionary
{
    private readonly Dictionary<Type, byte> typeToIndex = new();
    private readonly List<ReaderFunc> creators = new();


    public void Register<T>(ReaderFunc func)
    {
        typeToIndex.Add(typeof(T), (byte)creators.Count);
        creators.Add(func);
    }

    public byte IndexOf(Type type) => typeToIndex[type];
    public ReaderFunc GetCreator(byte index) => creators[index];
}