using Melville.MVVM.Wpf.RootWindows;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.RootWindows;

public class NavigationWindowTest
{
    private readonly NavigationWindow sut;

    public NavigationWindowTest()
    {
        sut = new NavigationWindow();
    }

    [Fact]
    public void NavigateToTest()
    {
        var str = "content";
        sut.NavigateTo(str);
        Assert.Equal(str, sut.Content);
            
    }

    [Fact]
    public void DefaultBackNavigationDoesNothing()
    {
        sut.NavigateTo("Foo");
        var str = "content";
        sut.NavigateTo(str);
        sut.NavigateToPriorPage();
        Assert.Equal(str, sut.Content);
    }

    [Fact]
    public void NavigationNotifications()
    {
        var content = new Mock<IAcceptNavigationNotifications>();
        content.VerifyNoOtherCalls();
        sut.NavigateTo(content.Object);
        content.Verify(i=>i.NavigatedTo(), Times.Once);
        content.VerifyNoOtherCalls();
        sut.NavigateTo("Foo");
        content.Verify(i=>i.NavigatedAwayFrom(), Times.Once);
        content.VerifyNoOtherCalls();
    }
}

public class NavigationWithHistoryTest
{
    private readonly Mock<INavigationHistory> history = new Mock<INavigationHistory>();
    private readonly NavigationWindow sut;

    public NavigationWithHistoryTest()
    {
        sut = new NavigationWindow(history.Object);
    }
    [Fact]
    public void NavigateBack()
    {
        history.Setup(i => i.Pop()).Returns("Bar");
        sut.NavigateToPriorPage();
        Assert.Equal("Bar", sut.Content);
        history.Verify(i=>i.Pop(), Times.Once);
        history.VerifyNoOtherCalls();
    }

    [Fact]
    public void NavigationRecordsHistory()
    {
        sut.NavigateTo("Foo");
        history.Verify(i=>i.Push("No Content"), Times.Once);
        history.VerifyNoOtherCalls();
    }
}