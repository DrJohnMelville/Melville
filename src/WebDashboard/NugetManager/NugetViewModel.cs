using System.Collections.Generic;

namespace WebDashboard.NugetManager
{
    public class NugetViewModel
    {
        public NugetModel Model { get; }

        public NugetViewModel(NugetModel model)
        {
            Model = model;
        }
    }
}