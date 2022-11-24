using System.Runtime.CompilerServices.ProductionGenerators.Constructors;
using System.Runtime.CompilerServices.ProductionGenerators.StaticSingletons;
using Melville.Generators.INPC.Test.UnitTests;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.INPC.Test.StaticSingletons;

public class StaticSingletonTests
{
    private GeneratorTestBed RunTestWithDeclaredAttr(string s, string parentCode = "") =>
        new(new StaticSingletonGenerator(), $$"""
            namespace Melville.INPC
            {
                [Conditional(""ShowCodeGenAttributes"")]
                [AttributeUsage( AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
                public sealed class StaticSingletonAttribute : Attribute
                {  
                    public string? Name { get; }
                    public StaticSingletonAttribute(string? name)
                    {           
                     Name = name;
                    }
                } 
            }
            namespace Outer
            {
                using Melville.INPC;
                {{s}}
            }
            """);

    [Fact]
    public void GenerateSingleton()
    {
        var tb = RunTestWithDeclaredAttr("[StaticSingleton] public partial class X {}");
        tb.LastFileContains("private X() {}");
        tb.LastFileContains("public static readonly Outer.X Instance = new();");
    }
    [Fact]
    public void GenerateSingletonWithName()
    {
        var tb = RunTestWithDeclaredAttr("[StaticSingleton(\"FooBar\")] public partial class X {}");
        tb.LastFileContains("private X() {}");
        tb.LastFileContains("public static readonly Outer.X FooBar = new();");
    }
    [Fact]
    public void GenerateSingletonWithExplicitName()
    {
        var tb = RunTestWithDeclaredAttr("[StaticSingleton(Name = \"FooBar\")] public partial class X {}");
        tb.LastFileContains("private X() {}");
        tb.LastFileContains("public static readonly Outer.X FooBar = new();");
    }
}