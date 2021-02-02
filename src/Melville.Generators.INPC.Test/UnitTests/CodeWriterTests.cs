using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Xunit;

namespace Melville.Generators.INPC.Test.UnitTests
{
    public class CodeWriterTests
    {
        private readonly CodeWriter sut = new CodeWriter(new GeneratorExecutionContext());
        [Fact]
        public void TestName()
        {
            sut.Append("Foo");   
            sut.Append("Bar");
            Assert.Equal("FooBar", sut.ToString());
        }
        [Fact]
        public void TestNameEoln()
        {
            sut.AppendLine("Foo");   
            sut.Append("Bar");
            Assert.Equal("Foo\r\nBar", sut.ToString());
        }
        [Fact]
        public void TestNameIndent()
        {
            sut.AppendLine("Foo");
            using (sut.IndentedRun())
            {
                sut.Append("Bar");
                sut.AppendLine("Bar");
            }
            sut.Append("Baz");
            Assert.Equal("Foo\r\n    BarBar\r\nBaz", sut.ToString());
        }
        [Fact]
        public void TestBlock()
        {
            sut.AppendLine("Foo");
            using (sut.CurlyBlock())
            {
                sut.Append("Bar");
                sut.AppendLine("Bar");
            }
            sut.Append("Baz");
            Assert.Equal("Foo\r\n{\r\n    BarBar\r\n}\r\nBaz", sut.ToString());
        }

    }
}