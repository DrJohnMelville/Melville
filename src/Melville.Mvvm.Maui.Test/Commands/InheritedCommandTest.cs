using Melville.MVVM.Maui.Commands;
using Xunit.Sdk;

namespace Melville.Mvvm.Maui.Test.Commands;

public class InheritedCommandTest
{
    [Fact]
    public void SimpleCommand()
    {
        string result = "Nope";
        var cmd = InheritedCommandFactory.Create((string s) => result = s);
        cmd.Execute("Hello");
        result.Should().Be("Hello");
    }

    private class ConcreteBindableObject : BindableObject
    {
    }

    [Fact]
    public void CommandWithDataContext()
    {
        string result = "Nope";
        var cmd = InheritedCommandFactory.Create((Action<string>)((string s) => result = s));
        cmd.Execute(new ConcreteBindableObject() { BindingContext = "Hello" });
        result.Should().Be("Hello");
    }

    private class ConcreteElement: Element
    {
    }

    [Fact]
    public void InheritedValues()
    {
        string result = "Nope";
        var cmd = InheritedCommandFactory.Create((string s) => result = s);
        cmd.Execute(
            new ConcreteElement {Parent = 
                    new ConcreteElement(){BindingContext = "Hello" }});
        result.Should().Be("Hello");
    }
    [Fact]
    public void ExplicitValue()
    {
        string result = "Nope";
        var cmd = InheritedCommandFactory.Create((string s) => result = s);
        var target = new ConcreteElement();
        InheritedCommand.SetInheritedCommandParameter(target, "Hello");
        cmd.Execute(target);
        result.Should().Be("Hello");
    }
    [Fact]
    public void ExplicitArray()
    {
        string result = "Nope";
        var cmd = InheritedCommandFactory.Create((string s) => result = s);
        var target = new ConcreteElement();
        InheritedCommand.SetInheritedCommandParameter(target, 
            new string[]{"Hello"});
        cmd.Execute(target);
        result.Should().Be("Hello");
    }

    [Fact]
    public void UseIoc()
    {
        string result = "Nope";
        var context = SetupIocContext(out var win1);

        var cmd = InheritedCommandFactory.Create(
            ([FromServices]string s) => result = s);
        cmd.Execute(win1);

        context.Verify(i => i.Dispose(), Times.Once);
        result.Should().Be("World");
    }
    
    private static Mock<IServiceScope> SetupIocContext(out ConcreteElement win1)
    {
        var context = new Mock<IServiceProvider>();
        context.Setup(i => i.GetService(typeof(string)))
            .Returns("World");
        
        var scope = new Mock<IServiceScope>();
        scope.SetupGet(i => i.ServiceProvider).Returns(context.Object);
        var factory = new Mock<IServiceScopeFactory>();
        factory.Setup(i => i.CreateScope()).Returns(scope.Object);
        context.Setup(i => i.GetService(typeof(IServiceScopeFactory)))
            .Returns(factory.Object);

        var mauiContext = new Mock<IMauiContext>();
        mauiContext.SetupGet(i => i.Services).Returns(context.Object);
        var handler = new Mock<IElementHandler>();
        handler.SetupGet(i => i.MauiContext).Returns(mauiContext.Object);

        win1 = new ConcreteElement() { Handler = handler.Object};
        return scope;
    }

    [Fact]
    public void UseIocDisposeAfterTask()
    {
        var tcs = new TaskCompletionSource();
        string result = "Nope";
        var context = SetupIocContext(out var win1);

        var cmd = InheritedCommandFactory.Create(
            ([FromServices]string s) =>
            {
                result = s;
                return tcs.Task;
            });
        cmd.Execute(win1);
        result.Should().Be("World");
        context.Verify(i => i.Dispose(), Times.Never);
        tcs.SetResult();
        context.Verify(i => i.Dispose(), Times.Once);
    }
    [Fact]
    public void UseIocDisposeAfterValueTask()
    {
        var tcs = new TaskCompletionSource();
        string result = "Nope";
        var context = SetupIocContext(out var win1);

        var cmd = InheritedCommandFactory.Create(
            ([FromServices]string s) =>
            {
                result = s;
                return new ValueTask(tcs.Task);
            });
        cmd.Execute(win1);
        result.Should().Be("World");
        context.Verify(i => i.Dispose(), Times.Never);
        tcs.SetResult();
        context.Verify(i => i.Dispose(), Times.Once);
    }
    [Fact]
    public void UseIocDisposeAfterTaskofT()
    {
        var tcs = new TaskCompletionSource();
        string result = "Nope";
        var context = SetupIocContext(out var win1);

        var cmd = InheritedCommandFactory.Create(
            async ([FromServices]string s) =>
            {
                result = s;
                await tcs.Task;
                return s;
            });
        cmd.Execute(win1);
        result.Should().Be("World");
        context.Verify(i => i.Dispose(), Times.Never);
        tcs.SetResult();
        context.Verify(i => i.Dispose(), Times.Once);
    }
    [Fact]
    public void UseIocDisposeAfterValueTaskofT()
    {
        var tcs = new TaskCompletionSource();
        string result = "Nope";
        var context = SetupIocContext(out var win1);

        var cmd = InheritedCommandFactory.Create(
            async ValueTask<string> ([FromServices]string s) =>
            {
                result = s;
                await tcs.Task;
                return s;
            });
        cmd.Execute(win1);
        result.Should().Be("World");
        context.Verify(i => i.Dispose(), Times.Never);
        tcs.SetResult();
        context.Verify(i => i.Dispose(), Times.Once);
    }
}