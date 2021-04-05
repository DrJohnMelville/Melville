using System.Collections.Generic;
using System.Linq;
using Melville.Log.Viewer.HomeScreens;
using Melville.MVVM.BusinessObjects;

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