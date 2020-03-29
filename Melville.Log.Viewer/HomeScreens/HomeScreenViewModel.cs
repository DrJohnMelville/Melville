using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Melville.Log.Viewer.LogViews;
using Melville.Log.Viewer.NamedPipeServers;
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

        public HomeScreenViewModel(WelcomePageViewModel welcomePage, IPipeListener pipeListener,
            Func<Stream, LogViewModel> modelCreator)
        {
            Pages.Add(welcomePage);
            pipeListener.NewClientConnection += (_, e) => Pages.Add(modelCreator(e.ClientConnection));
        }
    }
}