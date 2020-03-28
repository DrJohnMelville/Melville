using System;
using Melville.MVVM.FileSystem;

namespace Melville.Mvvm.TestHelpers.MockFiles
{
    internal class MockDirectoryTreeBuilderParent
    {
        
    }
    public class MockDirectoryTreeBuilder
    {
        private MockDirectory dir;

        public MockDirectoryTreeBuilder(MockDirectory dir)
        {
            this.dir = dir;
            dir.Create();
        }

        public MockDirectory Object => dir;

        public MockDirectoryTreeBuilder(string path):this(new MockDirectory(path))
        {
        }

        public MockDirectoryTreeBuilder():this("Q:\\Fake\\Dir")
        {
        }

        public MockDirectoryTreeBuilder Folder(string name, 
            Func<MockDirectoryTreeBuilder, object>? children = null)
        {
            var sub = (MockDirectory)dir.SubDirectory(name); 
              // the cast has to succed because MockDirectory always creates mock subdirectories
            sub.Create();
            if (children != null)
            {
                children(new MockDirectoryTreeBuilder(sub));
            }
            return this;
        }

        public MockDirectoryTreeBuilder File(string name) =>
            File(name, "Fake file content.");

        public MockDirectoryTreeBuilder File(string name, string fileContent)
        {
            dir.File(name).Create(fileContent);
            return this;
        }
    }
}