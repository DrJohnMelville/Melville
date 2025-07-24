using System.Collections.Generic;
using System.Linq;
using Melville.Log.Viewer.HomeScreens;
using Melville.MVVM.BusinessObjects;

namespace Melville.Log.Viewer.WelcomePage;

public class WelcomePageViewModel: NotifyBase, IHomeScreenPage
{
    public string Title => "Home Page";
    public void Stop()
    {
        // do nothing
    }
}