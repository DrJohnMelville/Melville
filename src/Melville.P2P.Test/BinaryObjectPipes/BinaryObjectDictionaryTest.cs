using System.Buffers;
using System.IO.Pipelines;
using Melville.P2P.Raw.BinaryObjectPipes;
using Xunit;

namespace Melville.P2P.Test.BinaryObjectPipes
{
    public class BinaryObjectDictionaryTest
    {
        public class FakeObject1 : ICanWriteToPipe
        {
            public void WriteToPipe(PipeWriter write) => throw new System.NotImplementedException();
        }
        public class FakeObject2 : ICanWriteToPipe
        {
            public void WriteToPipe(PipeWriter write) => throw new System.NotImplementedException();
        }

        private readonly BinaryObjectDictionary sut = new BinaryObjectDictionary();
        
        [Fact]
        public void RegisterItem()
        {
            sut.Register<FakeObject1>(delegate { return new FakeObject1(); });
            sut.Register<FakeObject2>(delegate { return new FakeObject2(); });

            Assert.Equal(0, sut.IndexOf(typeof(FakeObject1)));
            Assert.Equal(1, sut.IndexOf(typeof(FakeObject2)));

            var reader = new SequenceReader<byte>(new ReadOnlySequence<byte>());
            Assert.True(sut.GetCreator(0)(ref reader) is FakeObject1);
            Assert.True(sut.GetCreator(1)(ref reader) is FakeObject2);
            
        }
    }
}