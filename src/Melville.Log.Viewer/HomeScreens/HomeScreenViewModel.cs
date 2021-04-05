using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using Melville.Lists.AdvancedLists;
using Melville.Log.Viewer.LogViews;
using Melville.Log.Viewer.NamedPipeServers;
using Melville.Log.Viewer.WelcomePage;
using Melville.MVVM.BusinessObjects;
using Melville.WpfAppFramework.StartupBases;

namespace Melville.Log.Viewer.HomeScreens
{
    public interface IHomeScreenPage
    {
        string Title { get; }
        void Stop();
    }
    public class HomeScreenViewModel: NotifyBase
    {
        private IHomeScreenPage currentPage;
        public IHomeScreenPage CurrentPage
        {
            get => currentPage;
            set => AssignAndNotify(ref currentPage, value);
        }

        public ICollection<IHomeScreenPage> Pages { get; } = new ThreadSafeBindableCollection<IHomeScreenPage>();

        public HomeScreenViewModel(WelcomePageViewModel welcomePage, IPipeListener pipeListener,
            Func<ILogConnection, LogViewModel> modelCreator)
        {
            Pages.Add(welcomePage);
            currentPage = welcomePage;
            pipeListener.NewClientConnection += (_, e) =>
                AddNewPage(modelCreator(new StreamLogConnection(e.ClientConnection)));
        }

        public void Remove(IHomeScreenPage page)
        {
            Pages.Remove(page);
            page.Stop();
        }

        public bool ShellKeyHandler(KeyEventArgs e)
        {
            if (e.Key == Key.F4 && e.KeyboardDevice.Modifiers == ModifierKeys.Control &&
                CurrentPage is LogViewModel)
            {
                Remove(CurrentPage);
                return true;
            }

            return false;
        }
        
        public void ConnectToWeb(IHasTargetUrl targetHolder)
        {
            try
            {
                if (targetHolder.CurrentSite == null) return;
                AddNewPage(new LogViewModel(
                    new HubLogConnection(targetHolder.CurrentSite), targetHolder.CurrentSite.Name));
            }
            catch (Exception)
            {
                // failed to connect to website
            }
        }

        private void AddNewPage(LogViewModel page)
        {
            Pages.Add(page);
            CurrentPage = page;
        }
    }
}