using System.Text.Json;
using Melville.FileSystem.FileSystem;
using Moq;
using WebDashboard.SecretManager.Models;
using WebDashboard.SecretManager.Views;
using Xunit;

namespace WebDashboard.Test.Views
{
    public class SecretFileTextEditorViewModelTest
    {
        private SecretFileTextEditorViewModel CreateHolder(string jsonText)
        {
            var holder = new SecretFileHolder(Mock.Of<IFile>(), JsonDocument.Parse(jsonText));
            return new SecretFileTextEditorViewModel(holder);
        }

        [Fact]
        public void Parse()
        {
            var holder = CreateHolder("{\"A\": \"AAAA\"}");
            Assert.Equal("{\r\n  \"A\": \"AAAA\"\r\n}", holder.Text);
        }

        
        
    }
}