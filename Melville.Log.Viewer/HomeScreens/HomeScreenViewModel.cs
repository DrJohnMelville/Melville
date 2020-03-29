using System.Collections;
using System.Collections.Generic;
using Melville.Log.Viewer.WelcomePage;
using Melville.MVVM.AdvancedLists;

namespace Melville.Log.Viewer.HomeScreens
{
    public interface IHomeScreenPage
    {
        string Title { get; }
    }
    public class HomeScreenViewModel
    {
        public ICollection<IHomeScreenPage> Pages { get; } = new ThreadSafeBindableCollection<IHomeScreenPage>();

        public HomeScreenViewModel(WelcomePageViewModel welcomePage)
        {
            Pages.Add(welcomePage);
        }
    }
}