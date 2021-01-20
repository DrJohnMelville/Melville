using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;

namespace Melville.P2P.Raw.BinaryObjectPipes
{
    public interface IBinaryObjectPipeReader
    {
        IAsyncEnumerable<object> Read();
    }

    public class BinaryObjectPipeReader : IBinaryObjectPipeReader
    {
        private readonly PipeReader reader;
        private readonly BinaryObjectDictionary typeDictionary;
    
        public BinaryObjectPipeReader(Stream stream, BinaryObjectDictionary typeDictionary) :
            this(PipeReader.Create(stream), typeDictionary)
        {
                    
        }
        public BinaryObjectPipeReader(PipeReader reader, BinaryObjectDictionary typeDictionary)
        {
            this.reader = reader;
            this.typeDictionary = typeDictionary;
        }  
    
        public async IAsyncEnumerable<object> Read()
        {
            ReadResult result;
            do
            {
                result = await reader.ReadAsync();
                var buffer = result.Buffer;
                object? item;
                SequencePosition finalPosition = buffer.Start;
                do
                {
                    
                    (item, finalPosition) = FirstObjectFromBuffer(buffer.Slice(finalPosition));
                    if (item != null)
                    {
                        yield return item;
                    }
                    reader.AdvanceTo(finalPosition, buffer.End);
                } while (item != null && ! finalPosition.Equals(buffer.End));
            } while (!(result.IsCompleted || result.IsCanceled));
        }
                
        private (object?, SequencePosition) FirstObjectFromBuffer(ReadOnlySequence<byte> input)
        {
            var byteReader = new SequenceReader<byte>(input);
            var ret = ReadObject(ref byteReader);
            return (ret, ret == null?input.Start:byteReader.Position);
        }

        private object? ReadObject(ref SequenceReader<byte> byteReader) =>
            byteReader.TryRead(out var typeKey) &&
            ReadOnjectByKey(typeKey, ref byteReader) is { } item
                ? item
                : null;

        private object? ReadOnjectByKey(byte typeKey, ref SequenceReader<byte> byteReader) =>
            typeDictionary.GetCreator(typeKey)(ref byteReader);
    }
}