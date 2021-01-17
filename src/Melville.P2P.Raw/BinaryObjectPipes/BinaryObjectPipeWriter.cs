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
}