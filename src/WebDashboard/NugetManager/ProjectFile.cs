using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Melville.FileSystem;
using Melville.INPC;

namespace WebDashboard.NugetManager
{
    public partial class ProjectFile
    {
        public IFile File { get; }
        public List<ProjectFile> DependsOn { get; } = new();
        public XElement Content { get; }

        public bool GeneratesPackageOnBuild =>
            Content
                .Descendants("GeneratePackageOnBuild")
                .Any(i => i.Value.Equals("true", StringComparison.OrdinalIgnoreCase));
        [AutoNotify] private bool deploy;

        void OnDeployChanged()
        {
            foreach (var proj in DependsOn)
            {
                proj.Deploy = Deploy;
            }
        }
        
        public ProjectFile(IFile file, XElement content)
        {
            this.File = file;
            this.Content = content;
        }

        public IEnumerable<IFile> ComputeDependencies() =>
            Content
                .Descendants("ProjectReference")
                .Select(i=>i.Attribute("Include")?.Value)
                .OfType<string>()
                .Select(i=>File.FileAtRelativePath(i))
                .OfType<IFile>();

        public IFile? Package(string version) =>
            File.FileAtRelativePath($"bin/Release/{File.NameWithoutExtension()}.{version}.nupkg");
    }
}