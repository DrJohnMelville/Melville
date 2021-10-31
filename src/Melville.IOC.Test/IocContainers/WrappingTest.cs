using System;
using System.Text;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers;

public sealed class WrappingTest
{
    private readonly StringBuilder sb = new StringBuilder();
    public interface ITarget
    {
        void Write();
    }
    public class Target : ITarget
    {
        private readonly StringBuilder output;

        public Target(StringBuilder output)
        {
            this.output = output;
        }

        public void Write()
        {
            output.Append("Target");
        }
    }
        
    public class TargetWrapper:ITarget
    {
        private ITarget targetImplementation;
        private StringBuilder sb;
        private readonly string? text;

        public TargetWrapper(ITarget targetImplementation, StringBuilder sb, string? text = "")
        {
            this.targetImplementation = targetImplementation;
            this.sb = sb;
            this.text = text;
        }

        public void Write()
        {
            sb.Append($"Pre{text} ");
            targetImplementation.Write();
            sb.Append(" Post");
        }
    }
    public class DisposableTargetWrapper:ITarget, IDisposable
    {
        private ITarget targetImplementation;
        private StringBuilder sb;

        public DisposableTargetWrapper(ITarget targetImplementation, StringBuilder sb)
        {
            this.targetImplementation = targetImplementation;
            this.sb = sb;
        }

        public void Write()
        {
            sb.Append("Pre ");
            targetImplementation.Write();
            sb.Append(" Post");
        }

        public void Dispose()
        {
            sb.Append(" Disposed!");
        }
    }

    private readonly IocContainer sut = new IocContainer();

    public WrappingTest()
    {
        sut.Bind<StringBuilder>().ToConstant(sb);
    }

    [Fact]
    public void NoWrapping()
    {
        sut.Get<ITarget>().Write();
        Assert.Equal("Target", sb.ToString());
    }

    [Fact]
    public void WrapOnce()
    {
        sut.Bind<ITarget>().To<Target>().WrapWith(i => new TargetWrapper(i, sb));
        sut.Get<ITarget>().Write();
        Assert.Equal("Pre Target Post", sb.ToString());
    }
    [Fact]
    public void WrapTwice()
    {
        sut.Bind<ITarget>().To<Target>().WrapWith(i => new TargetWrapper(i, sb))
            .WrapWith(i => new TargetWrapper(i, sb));
        sut.Get<ITarget>().Write();
        Assert.Equal("Pre Pre Target Post Post", sb.ToString());
    }
    [Fact]
    public void WrapFromContainer()
    {
        sut.Bind<ITarget>().To<Target>().WrapWith<TargetWrapper>();
        sut.Get<ITarget>().Write();
        Assert.Equal("Pre Target Post", sb.ToString());
    }
    [Fact]
    public void WrapFromContainerWithArgument()
    {
        sut.Bind<ITarget>().To<Target>().WrapWith<TargetWrapper>("Argument");
        sut.Get<ITarget>().Write();
        Assert.Equal("PreArgument Target Post", sb.ToString());
    }
    [Fact]
    public void WrapAndDisposeFromContainer()
    {
        sut.Bind<ITarget>().To<Target>().WrapWith<DisposableTargetWrapper>();
        using (var scope = sut.CreateScope())
        {
            scope.Get<ITarget>().Write();
        }
        Assert.Equal("Pre Target Post Disposed!", sb.ToString());
    }
    [Fact]
    public void WrapAndDisposeFunction()
    {
        sut.Bind<ITarget>().To<Target>()
            .WrapWith(i=>new DisposableTargetWrapper(i, sb))
            .RegisterWrapperForDisposal();
        using (var scope = sut.CreateScope())
        {
            scope.Get<ITarget>().Write();
        }
        Assert.Equal("Pre Target Post Disposed!", sb.ToString());
    }
}