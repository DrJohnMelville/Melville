using System.Text;
using Castle.DynamicProxy;
using Melville.Ioc.Interception;
using Melville.IOC.IocContainers;
using Xunit;

namespace Melville.IOC.Test.IocContainers
{
    public class InterceptionTest
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
        
        class SimpleInt:IInterceptor
        {
            private StringBuilder sb;

            public SimpleInt(StringBuilder sb)
            {
                this.sb = sb;
            }

            public void Intercept(IInvocation invocation)
            {
                sb.Append($"Pre {invocation.Method.Name} ");
                invocation.Proceed();
                sb.Append(" Post");
            }
        }

        private readonly IocContainer sut = new IocContainer();

        public InterceptionTest()
        {
            sut.Bind<StringBuilder>().ToConstant(sb);
        }

        [Fact]
        public void WrapWithOneInterceptor()
        {
            sut.Bind<ITarget>().To<Target>().InterceptWith(new SimpleInt(sb));
            var item = sut.Get<ITarget>();
            item.Write();
            Assert.Equal("Pre Write Target Post", sb.ToString());
        }
        [Fact]
        public void GlobalOneInterceptor()
        {
            sut.Bind<ITarget>().To<Target>();
            sut.Intercept<ITarget>(new SimpleInt(sb));
            var item = sut.Get<ITarget>();
            item.Write();
            Assert.Equal("Pre Write Target Post", sb.ToString());
        }
        [Fact]
        public void GlobalOneInterceptor2()
        {
            sut.Bind<ITarget>().To<Target>();
            sut.Intercept<Target, ITarget>(new SimpleInt(sb));
            var item = sut.Get<ITarget>();
            item.Write();
            Assert.Equal("Pre Write Target Post", sb.ToString());
        }
        [Fact]
        public void WrapWithOneInterceptorTyprVariable()
        {
            sut.Bind<ITarget>().To<Target>().InterceptWith(i=>i.Add<SimpleInt>());
            var item = sut.Get<ITarget>();
            item.Write();
            Assert.Equal("Pre Write Target Post", sb.ToString());
        }
        [Fact]
        public void WrapWithTwoInterceptorTypeVariable()
        {
            sut.Bind<ITarget>().To<Target>().InterceptWith(i=>i.Add<SimpleInt>().Add<SimpleInt>());
            var item = sut.Get<ITarget>();
            item.Write();
            Assert.Equal("Pre Write Pre Write Target Post Post", sb.ToString());
        }
        [Fact]
        public void GlobalTwoInterceptorTypeVariable()
        {
            sut.Bind<ITarget>().To<Target>();
            sut.Intercept<ITarget>(i=>i.Add<SimpleInt>().Add<SimpleInt>());
            var item = sut.Get<ITarget>();
            item.Write();
            Assert.Equal("Pre Write Pre Write Target Post Post", sb.ToString());
        }
        [Fact]
        public void GlobalTwoInterceptorTypeVariable2()
        {
            sut.Bind<ITarget>().To<Target>();
            sut.Intercept<Target,ITarget>(i=>i.Add<SimpleInt>().Add<SimpleInt>());
            var item = sut.Get<ITarget>();
            item.Write();
            Assert.Equal("Pre Write Pre Write Target Post Post", sb.ToString());
        }
        [Fact]
        public void WrapWithTwoInterceptor()
        {
            sut.Bind<ITarget>().To<Target>().InterceptWith(new SimpleInt(sb),new SimpleInt(sb));
            var item = sut.Get<ITarget>();
            item.Write();
            Assert.Equal("Pre Write Pre Write Target Post Post", sb.ToString());
        }
        [Fact]
        public void GlobalWrapWithTwoInterceptor()
        {
            sut.Bind<ITarget>().To<Target>();
            sut.Intercept<ITarget>(new SimpleInt(sb));
            sut.Intercept<ITarget>(new SimpleInt(sb));
            var item = sut.Get<ITarget>();
            item.Write();
            Assert.Equal("Pre Write Pre Write Target Post Post", sb.ToString());
        }
        
    }
}