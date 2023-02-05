using System.Globalization;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen;
using Melville.Generators.INPC.Test.UnitTests;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.DelegateToGen;

public class PostProcessTest
{
    private GeneratorTestBed RunTest(string mixinMembers, string hostMembers) =>
        new(new DelegateToGenerator(), $$"""
            namespace Outer
            {
                using Melville.INPC;
                public class Mixin
                {
                    {{mixinMembers}}
                }     
                public partial class C
                {
                    {{hostMembers}}}
                }
            }
            """, typeof(DelegateToAttribute));

    [Theory]
    [InlineData("public int X() => 1;", "public int X() => Wrap(this.mix.X());")]
    [InlineData("public int X => 1;", "get => Wrap(this.mix.X);")]
    [InlineData("public int X {get;set;}", "get => Wrap(this.mix.X);")]
    [InlineData("public int X {get;set;}", "set {this.mix.X = value; Wrap();}")]
    [InlineData("public int this[int y] => 1;", "get => Wrap(this.mix[y]);")]
    [InlineData("public void X() {}", "public void X() {this.mix.X(); Wrap();}")]
    public void WrapIntTest(string forwardedItem, string result)
    {
        var res = RunTest(forwardedItem, """
            [DelegateTo(WrapWith = "Wrap")] private Mixin mix;
            public int Wrap(int i) => i;
            public void Wrap();
            """);
        res.LastFile().AssertContains(result);
    }

    [Theory]
    [InlineData("public int X() => 1;", "public long X() => Wrap(this.mix.X());")]
    [InlineData("public int X => 1;", "public long X")]
    [InlineData("public int X => 1;", "get => Wrap(this.mix.X);")]
    [InlineData("public int X {get;set;}", "set {this.mix.X = (int)value; Wrap();}")]
    public void WrapIntToLongTest(string forwardedItem, string result)
    {
        var res = RunTest(forwardedItem, """
            [DelegateTo(WrapWith = "Wrap")] private Mixin mix;
            public long Wrap(int i) => i;
            public void Wrap();
            """);
        res.LastFile().AssertContains(result);
    }

    [Theory]
    [InlineData("public void X() {}", "public int X() {this.mix.X(); return Wrap();}", "public int Wrap() => i;", "public void Wrap1(){}")]
    [InlineData("public void X() {}", "public void X() {this.mix.X(); Wrap();}", "public void Wrap(){}", "public void Wrap1(){}")]
    [InlineData("public void X() {}", "public void X() {this.mix.X(); Wrap();}", "public void Wrap(int x = 0){}", "public void Wrap(int x){}")]
    [InlineData("public int X() => 1;", "Wrap", "private int Wrap(int x)=>1", "private int Wrap(string x)=>1")]
    [InlineData("public int X() => 1;", "public int X() => Wrap(this.mix.X());", "private T Wrap<T>(T x)=>x", "private T Wrap<T,U>(T x) => x;")]
    [InlineData("public int X() => 1;", "public int X() => Wrap(this.mix.X());", "private T Wrap<T>(T x)=>x;", 
        "private T Wrap<T>(int x) => x;")]
    [InlineData("public int X() => 1;", "public void X() => Wrap(this.mix.X());", "private void Wrap(int x, int y = 0)=>x", 
        "private int Wrap(int x)=>y;")]
    [InlineData("public int X() => 1;", "public int X() => Wrap(this.mix.X());", "private int Wrap(int x = 1, int y = 0)=>x", 
        "private int Wrap(int x, int y)=>y;")]
    public void MethodMatchingTest(string forwardedItem, string result, string succeeditem, string failItem)
    {
        RunSubstitutionItemTest(forwardedItem, succeeditem).LastFile().AssertContains(result);
        RunSubstitutionItemTest(forwardedItem, failItem).LastFile().AssertDoesNotContain(result);
    }

    private GeneratorTestBed RunSubstitutionItemTest(string forwardedItem, string succeeditem)
    {
        return RunTest(forwardedItem, $"""
            [DelegateTo(WrapWith = "Wrap")] private Mixin mix;
            {succeeditem}
            """);
    }
}