using System.Linq;
using System.Text.Json;
using Melville.FileSystem;
using Moq;
using WebDashboard.SecretManager.Models;
using Xunit;

namespace WebDashboard.Test.Models
{
    public class SecretFileHolderTest
    {
        private SecretFileHolder ConstructSut(string json) => 
            new SecretFileHolder(Mock.Of<IFile>(), JsonDocument.Parse(json));

        [Fact]
        public void EmptyDocument()
        {
            var sut = ConstructSut("{}");
            Assert.Empty(sut.Root.Children);
        }

        [Fact]
        public void OneValue()
        {
            var sut = ConstructSut(@"{""a"":""b""}");
            Assert.Single(sut.Root.Children);
            Assert.Equal("a", sut.Root.Children.First().Name);
            Assert.Equal("b", sut.Root.Children.First().Value);
        }

        [Theory]
        [InlineData(@"{""a"":""b"",""c"":""d""}")]
        [InlineData(@"{""c"":""d"", ""a"":""b""}")]
        public void TwoValuesAreSorted(string source)
        {
            var sut = ConstructSut(source);
            Assert.Equal(2, sut.Root.Children.Count());
            Assert.Equal("a", sut.Root.Children.First().Name);
            Assert.Equal("b", sut.Root.Children.First().Value);
            Assert.Equal("c", sut.Root.Children.Last().Name);
            Assert.Equal("d", sut.Root.Children.Last().Value);
        }

        [Fact]
        public void Hierarchy()
        {
            var sut = ConstructSut(@"{""root:a"":""b"",""root:c"":""d"",""e"": ""f""}");
            Assert.Equal(2, sut.Root.Children.Count());
            Assert.Equal("e", sut.Root.Children.First().Name);
            Assert.Equal("f", sut.Root.Children.First().Value);
            Assert.Equal("c", sut.Root.Children.Last().Children.Last().Name);
            Assert.Equal("d", sut.Root.Children.Last().Children.Last().Value);
        }
    }
}