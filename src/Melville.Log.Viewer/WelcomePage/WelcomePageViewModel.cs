using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Melville.Log.NamedPipeEventSink;
using Melville.Log.Viewer.HomeScreens;
using Melville.Log.Viewer.NamedPipeServers;
using Melville.MVVM.BusinessObjects;
using Melville.MVVM.Wpf.DiParameterSources;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact.Reader;

namespace Melville.Log.Viewer.WelcomePage
{
    public interface IHasTargetUrl
    {
        TargetSite? CurrentSite { get; }
    }
    public class WelcomePageViewModel: NotifyBase, IHomeScreenPage, IHasTargetUrl
    {
        public IList<TargetSite> Sites { get; }
        public TargetSite? CurrentSite { get; set; }
        public WelcomePageViewModel(IList<TargetSite> sites)
        {
            this.Sites = sites;
            CurrentSite = sites.FirstOrDefault();
        }

        public string Title => "Home Page";
        public void Stop()
        {
            // do nothing
        }
    }
}