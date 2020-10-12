﻿using Melville.MVVM.BusinessObjects;

namespace Melville.MVVM.Wpf.RootWindows
{
    public interface INavigationWindow
    {
        void NavigateTo(object content);
    }

    public interface IAcceptNavigationNotifications
    {
        void NavigatedTo();
        void NavigatedAwayFrom();
    }

    public class NavigationWindow : NotifyBase, INavigationWindow
    {
        private object content = "No Content";
        public object Content
        {
            get => content;
            set => AssignAndNotify(ref content, value);
        }

        public void NavigateTo(object target)
        {
            (Content as IAcceptNavigationNotifications)?.NavigatedAwayFrom();
            Content = target;
            (Content as IAcceptNavigationNotifications)?.NavigatedTo();
        }
    }
}