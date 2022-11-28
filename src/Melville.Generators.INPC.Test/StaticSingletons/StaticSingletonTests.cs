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
                public sealed class StaticSingletonAttribute : System.Attribute
                {  
                    public string? Name { get; set;}
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
        tb.LastFile().AssertContains("private X() {}");
        tb.LastFile().AssertContains("public static readonly Outer.X Instance = new();");
    }
    [Fact]
    public void GenerateSingletonWithName()
    {
        var tb = RunTestWithDeclaredAttr("[StaticSingleton(\"FooBar\")] public partial class X {}");
        tb.LastFile().AssertContains("private X() {}"); 
        tb.LastFile().AssertContains("public static readonly Outer.X FooBar = new();");
    }
}