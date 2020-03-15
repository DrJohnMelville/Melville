using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Melville.MVVM.FileSystem;
using Moq;
using WebDashboard.Models;
using WebDashboard.Views;
using Xunit;

namespace WebDashboard.Test.Views
{
    public class SecretFileEditorViewModelTest
    {
        private SecretFileEditorViewModel CreateHolder(string jsonText)
        {
            var holder = new SecretFileHolder(Mock.Of<IFile>(), JsonDocument.Parse(jsonText));
            return new SecretFileEditorViewModel(holder);
        }

        [Fact]
        public async Task CreateEmptyFileAndDirectory()
        {
            var ms = new MemoryStream();
            var dir = new Mock<IDirectory>();
            var file = new Mock<IFile>();
            file.Setup(i => i.CreateWrite(FileAttributes.Normal)).ReturnsAsync(()=>ms);
            file.SetupGet(i => i.Directory).Returns(dir.Object);
            var holder = new SecretFileHolder(file.Object, JsonDocument.Parse("{}"));
            var sut = new SecretFileEditorViewModel(holder);
            sut.NewValue();
            await sut.SaveFile();

            var streamContents = Encoding.UTF8.GetString(ms.ToArray());
            Assert.Equal("{\"ItemName\":\"Value\"}", streamContents);
            
        }
        [Fact]
        public async Task CreateComplexFile()
        {
            var ms = new MemoryStream();
            var dir = new Mock<IDirectory>();
            var file = new Mock<IFile>();
            file.Setup(i => i.CreateWrite(FileAttributes.Normal)).ReturnsAsync(()=>ms);
            file.SetupGet(i => i.Directory).Returns(dir.Object);
            var holder = new SecretFileHolder(file.Object, JsonDocument.Parse("{}"));
            var sut = new SecretFileEditorViewModel(holder);
            sut.NewValue();
            sut.NewNode();
            sut.Current = sut.Root.Children[1];
            sut.NewValue();
            await sut.SaveFile();

            var streamContents = Encoding.UTF8.GetString(ms.ToArray());
            Assert.Equal("{\"ItemName\":\"Value\",\"Name:ItemName\":\"Value\"}", streamContents);
            
        }
        
        [Fact]
        public void AddNodeToRootWhenNUll()
        {
            var sut = CreateHolder("{\"Val1\":\"Value\"}");
            sut.NewNode();
            Assert.Equal(2, sut.Root.Children.Count());
            Assert.Equal("Name", sut.Root.Children[1].Name);
        }
        [Fact]
        public void AddNodeToRootWhenRootValue()
        {
            var sut = CreateHolder("{\"Val1\":\"Value\"}");
            sut.Current = sut.Root.Children[0];
            sut.NewNode();
            Assert.Equal(2, sut.Root.Children.Count());
            Assert.Equal("Name", sut.Root.Children[1].Name);
        }
        [Fact]
        public void AddNodeToNode()
        {
            var sut = CreateHolder("{\"Node:Val1\":\"Value\"}");
            sut.Current = sut.Root.Children[0];
            sut.NewNode();
            Assert.Single(sut.Root.Children);
            Assert.Equal("Name", sut.Root.Children[0].Children[1].Name);
        }
        [Fact]
        public void AddItemToNode()
        {
            var sut = CreateHolder("{\"Node:Val1\":\"Value\"}");
            sut.Current = sut.Root.Children[0];
            sut.NewValue();
            Assert.Single(sut.Root.Children);
            Assert.Equal("ItemName", sut.Root.Children[0].Children[1].Name);
            Assert.Equal("Value", ((SecretValue)sut.Root.Children[0].Children[1]).Value);
        }
        [Fact]
        public void AddNodeToItemParentNode()
        {
            var sut = CreateHolder("{\"Node:Val1\":\"Value\"}");
            sut.Current = sut.Root.Children[0].Children[0];
            sut.NewNode();
            Assert.Single(sut.Root.Children);
            Assert.Equal("Name", sut.Root.Children[0].Children[1].Name);
        }
        [Fact]
        public void DleteNode()
        {
            var sut = CreateHolder("{\"Node:Val1\":\"Value\"}");
            sut.Current = sut.Root.Children[0];
            sut.DeleteCurrentNode();
            Assert.Empty(sut.Root.Children);
        }
    }
}