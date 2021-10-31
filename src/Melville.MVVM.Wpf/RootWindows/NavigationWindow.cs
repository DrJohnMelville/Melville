using Melville.MVVM.BusinessObjects;

namespace Melville.MVVM.Wpf.RootWindows;

public interface INavigationWindow
{
    void NavigateTo(object content);
    void NavigateToPriorPage();
}

public interface IAcceptNavigationNotifications
{
    void NavigatedTo();
    void NavigatedAwayFrom();
}

public class NavigationWindow : NotifyBase, INavigationWindow
{
    private readonly INavigationHistory history;

    public NavigationWindow(INavigationHistory? history = null)
    {
        this.history = history ?? new NoNavigationHistory();
    }

    private object content = "No Content";
    public object Content
    {
        get => content;
        private set => AssignAndNotify(ref content, value);
    }

    public void NavigateTo(object target)
    {
        history.Push(Content);
        NavigateWithoutRecordingHistory(target);
    }

    private void NavigateWithoutRecordingHistory(object target)
    {
        (Content as IAcceptNavigationNotifications)?.NavigatedAwayFrom();
        Content = target;
        (Content as IAcceptNavigationNotifications)?.NavigatedTo();
    }

    public void NavigateToPriorPage()
    {
        if (history.Pop() is {} last)
        {
            NavigateWithoutRecordingHistory(last);
        }
    }
}