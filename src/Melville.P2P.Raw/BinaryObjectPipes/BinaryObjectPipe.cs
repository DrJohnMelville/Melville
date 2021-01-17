using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Melville.P2P.Raw.BinaryObjectPipes
{
    
    public class BinaryObjectPipeWriter
    {
        private readonly PipeWriter writer;
        private readonly BinaryObjectDictionary typeDictionary;
        
        public BinaryObjectPipeWriter(PipeWriter writer, BinaryObjectDictionary typeDictionary)
        {
            this.writer = writer;
            this.typeDictionary = typeDictionary;
        }
        
        public BinaryObjectPipeWriter(Stream stream, BinaryObjectDictionary typeDictionary): 
            this(PipeWriter.Create(stream), typeDictionary){}

        public ValueTask<FlushResult> Write(ICanWriteToPipe value)
        {
            var mem = writer.GetMemory(1);
            mem.Span[0] = typeDictionary.IndexOf(value.GetType());
            writer.Advance(1);
            value.WriteToPipe(writer);
            return writer.FlushAsync();
        }

    }
            public class BinaryObjectPipeReader
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
                        var (items, finalPosition) = ParseBuffer(result.Buffer);
                        foreach (var outputObject in items)
                        {
                            yield return outputObject;
                        }
                        reader.AdvanceTo(finalPosition, result.Buffer.End);
                    } while (!(result.IsCompleted || result.IsCanceled));
                }
                
                private (List<object>, SequencePosition) ParseBuffer(ReadOnlySequence<byte> input)
                {
                    var ret = new List<object>();
                    var byteReader = new SequenceReader<byte>(input);
                    var finalPosition = byteReader.Position;
                    while (true)
                    {
                        if (!byteReader.TryRead(out var typeKey)) break;
                        var item = typeDictionary.GetCreator(typeKey)(ref byteReader);
                        if (item == null) break;
                        ret.Add(item);
                        finalPosition = byteReader.Position;
                    }
    
                    return (ret, finalPosition);
                }
            }

}