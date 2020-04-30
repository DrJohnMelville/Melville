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
        void Stop();
    }
    public class HomeScreenViewModel
    {
        public ICollection<IHomeScreenPage> Pages { get; } = new ThreadSafeBindableCollection<IHomeScreenPage>();

        public HomeScreenViewModel(WelcomePageViewModel welcomePage, IPipeListener pipeListener,
            Func<ILogConnection, LogViewModel> modelCreator)
        {
            Pages.Add(welcomePage);
            pipeListener.NewClientConnection += (_, e) =>
                Pages.Add(modelCreator(new StreamLogConnection(e.ClientConnection)));
        }

        public void Remove(IHomeScreenPage page)
        {
            Pages.Remove(page);
            page.Stop();
        }
        
        public void ConnectToWeb(IHasTargetUrl targetHolder)
        {
            try
            {
                Pages.Add(new LogViewModel(
                    new HubLogConnection(targetHolder.TargetUrl), targetHolder.TargetUrl));
            }
            catch (Exception)
            {
                // failed to connect to website
            }
        }

    }
}