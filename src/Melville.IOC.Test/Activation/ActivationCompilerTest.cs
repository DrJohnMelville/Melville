using System;
using Melville.IOC.Activation;
using Xunit;

namespace Melville.IOC.Test.Activation;

public class ActivationCompilerTest
{
        
    public class SimpleObject
    {
    }

    [Fact]
    public void ActivateSimpleObject()
    {
        var ret = MakeObject<SimpleObject>(Array.Empty<Type>(), Array.Empty<object>());
        Assert.NotNull(ret);
    }

    private T MakeObject<T>(Type[] parameters, object[] arguments)
    {
        var func = ActivationCompiler.Compile(typeof(T), parameters);
        return (T) func(arguments);
    }

    public class StringHolder
    {
        public StringHolder(string inside)
        {
            Inside = inside;
        }

        public String Inside { get; }
    }
    [Fact]
    public void StringHolderCreate()
    {
        var ret = MakeObject<StringHolder>(new[] {typeof(string)}, new[] {"XXXy"});
        Assert.Equal("XXXy", ret.Inside);
    }

    public class Complex
    {
        public string A { get; }
        public string B { get; }
        public int C { get; }

        public Complex(string a, string b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
    [Fact]
    public void ComplexCreate()
    {
        var ret = MakeObject<Complex>(new[] {typeof(string), typeof(string), typeof(int)}, 
            new object[] {"A","B", 3});
        Assert.Equal("A", ret.A);
        Assert.Equal("B", ret.B);
        Assert.Equal(3, ret.C);
    }
}