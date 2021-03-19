using System;
using System.Collections.Generic;
using System.Linq;
using Melville.MVVM.FileSystem;

namespace WebDashboard.NugetManager
{
    public class NugetViewModel
    {
        public NugetModel Model { get; }

        public NugetViewModel(NugetModel model)
        {
            Model = model;
        }
        
        public void UploadPackages()
        {
            var files = Model.Files
                .Where(i => i.Deploy)
                .Select(i => i.Package(Model.Version))
                .OfType<IFile>()
                .ToArray();
        }

    }
}