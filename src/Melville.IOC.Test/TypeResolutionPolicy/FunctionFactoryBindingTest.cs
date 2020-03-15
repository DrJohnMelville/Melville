using System;
using System.Threading.Tasks;
using Melville.IOC.IocContainers;
using Melville.IOC.TypeResolutionPolicy;
using Xunit;

namespace Melville.IOC.Test.TypeResolutionPolicy
{
    public class FunctionFactoryBindingTest
    {
        private readonly IocContainer sut = new IocContainer();
        public class Simple { }

        [Fact]
        public void NoArguments()
        {
            var f = sut.Get<Func<Simple>>();
            Assert.NotNull(f());
        }
        [Fact]
        public void OneArgument()
        {
            var f = sut.Get<Func<int, Simple>>();
            Assert.NotNull(f(1));
        }
        [Fact]
        public void ManyArguments()
        {
            var f = sut.Get<Func<int, string, int, int, int, int, int, int, int, int, int, int, int, int,
                int, int, Simple>>();
            Assert.NotNull(f(1, "2", 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 15, 15, 16));
        }

        public class HasThreeArguments
        {
            public int Int { get; }
            public int Int2 { get; }
            public Simple Simple { get; }

            public HasThreeArguments(int i, int int2, Simple simple)
            {
                Int = i;
                Int2 = int2;
                Simple = simple;
            }
        }

        [Fact]
        public void InitializeWithParameters()
        {
            var fact = sut.Get<Func<int, int, HasThreeArguments>>();
            var item = fact(1, 2);
            Assert.Equal(1, item.Int);
            Assert.Equal(2, item.Int2);
            Assert.NotNull(item.Simple);
        }

        [Fact]
        public void BindWithParameters()
        {
            sut.Bind<HasThreeArguments>().To<HasThreeArguments>().WithParameters(1, 2);
            var item = sut.Get<HasThreeArguments>();
            Assert.Equal(1, item.Int);
            Assert.Equal(2, item.Int2);
            Assert.NotNull(item.Simple);
            var item2 = sut.Get<HasThreeArguments>();
            Assert.Equal(1, item2.Int);
            Assert.Equal(2, item2.Int2);
            Assert.NotNull(item2.Simple);
        }
        
        public class AsyncCreatable
        {
            public AsyncCreatable(int a)
            {
                A = a;
            }

            public string B { get; set; }

            public int A { get; }

            private Task Create(string b)
            {
                B = b;
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task CreateAsyncFactory()
        {
            var fact = sut.Get<Func<int, Func<string, Task<AsyncCreatable>>>>();
            var ret = await fact(1)("Hello World");
            Assert.Equal(1, ret.A);
            Assert.Equal("Hello World", ret.B);
        }

        [Fact]
        public async Task CreayAsyncUniqueObjects()
        {
            var fact = sut.Get<Func<int, Func<string, Task<AsyncCreatable>>>>();
            var ret = await fact(1)("Hello World");
            var ret2 = await fact(1)("Hello World");
            Assert.NotEqual(ret,ret2);
        }
        [Fact]
        public async Task CreateAndClearAsyncCachedObjects()
        {
            sut.BindAsyncFactory<AsyncCreatable>().AsSingleton();
            var (clear,fact) = sut.Get<(Action,Func<int, Func<string, Task<AsyncCreatable>>>)>();
            var ret = await fact(1)("Hello World");
            var ret2 = await fact(10)("aaa");
            Assert.Equal(1, ret2.A);
            Assert.Equal("Hello World", ret2.B);
            Assert.Equal(ret,ret2);

            clear();

            var ret3 = await fact(200)("new");
            Assert.NotEqual(ret, ret3);
            Assert.Equal(200, ret3.A);
            Assert.Equal("new", ret3.B);
        }

        public class ClassWithAsyncFactoryMethod
        {
            public int Id { get; set;}

            public static Task<ClassWithAsyncFactoryMethod> Create(int id)
            {
                return Task.FromResult(new ClassWithAsyncFactoryMethod {Id = id});
            }
        }

        [Fact]
        public async Task CreateFrpmAsyncStaticFactory()
        {
            var output = await sut.Get <Func<int, Task<ClassWithAsyncFactoryMethod>>>()(121);
            Assert.Equal(121, output.Id);
        }
    }
}