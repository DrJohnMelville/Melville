using Melville.Generators.INPC.ProductionGenerators.Macros;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.Macros;

public class TextTest
{
    protected GeneratorTestBed RunTest(string s) =>
        new(new MacroGenerator(), $$"""
            using System.Diagnostics;
            namespace Melville.INPC 
            {
                [Conditional("ShowCodeGenAttributes")]
                [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | 
                                AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event |
                    AttributeTargets.Interface, Inherited=false, AllowMultiple=true)]
                public sealed class MacroItemAttribute: System.Attribute
                {
                    public MacroItemAttribute(params object[] text){}
                }
                [Conditional("ShowCodeGenAttributes")]
                [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | 
                                AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event |
                    AttributeTargets.Class, Inherited=false, AllowMultiple=true)]
                public sealed class MacroCodeAttribute: System.Attribute
                {
                    public object Prefix {get;set;} = "";
                    public object Postfix {get;set;} = "";
                    public MacroCodeAttribute(object text){}
                } 
            }
    
            namespace Outer
            {
                using Melville.INPC;
    
                public partial class C 
                {" 
                    {{s}}
                    private void Func();
                }
            }
            """);

    [Theory]
    [InlineData("[MacroCode(\"// Macro: ~0~\")] [MacroItem(\"One\")]", "namespace Outer")]
    [InlineData("[MacroCode(\"// Macro: ~0~\")] [MacroItem(\"One\")]", "class C")]
    [InlineData("[MacroCode(\"// Macro: ~0~\")] [MacroItem(\"One\")]", "// Macro: One")]
    [InlineData("[MacroCode(\"// Macro: ~0~\")] [MacroItem(1)]", "// Macro: 1")]
    [InlineData("[MacroCode(\"// Macro: ~0~\")] [MacroItem(\"One\")][MacroItem(\"Two\")]", "// Macro: One")]
    [InlineData("[MacroCode(\"// Macro: ~0~\")] [MacroItem(\"One\")][MacroItem(\"Two\")]", "// Macro: Two")]
    [InlineData("[MacroCode(\"// Macro: ~0~\", Prefix = \"// 233\")] [MacroItem(\"One\")][MacroItem(\"Two\")]",
        "// Macro: Two")]
    [InlineData("[MacroCode(\"// Macro: ~0~\", Prefix = \"// 233\")] [MacroItem(\"One\")][MacroItem(\"Two\")]",
        "// 233")]
    [InlineData("[MacroCode(\"// Macro: ~0~\", Postfix = \"// 233\")] [MacroItem(\"One\")][MacroItem(\"Two\")]",
        "// 233")]
    [InlineData("""
        [MacroCode("// Hello\r\n// World")]
        [MacroItem("One")]
        """, "// Hello\r\n        // World")]
    public void SimpleSub(string input, string output) => 
        RunTest(input).FromName("MacroGen.Outer.C.Func().cs").AssertContains(output);

    
    [Fact]
    public void AttrsOnInterface()
    {
        var gen = RunTest("""
            [MacroItem("A")]
            [MacroCode("// item: ~0~")]
            interface I {};
            """);
        gen.LastFile().AssertContains("// item: A");
    }
    [Fact]
    public void AttrsOnInterfaceMember()
    {
        var gen = RunTest("""
            interface I 
            {
                [MacroItem("A")]
                [MacroCode("// item: ~0~")]
                int X();
            };
            """);
        gen.LastFile().AssertContains("// item: A");
    }

    [Fact]
    public void Prefix()
    {
        RunTest("[MacroCode(\"// Macro: ~0~\", Prefix=\"// Prefix\")] [MacroItem(\"One\")]");
    }
    [Fact]
    public void DoubleCode()
    {
        RunTest("[MacroCode(\"// Macro: ~0~\", Prefix=\"// Prefix\")] [MacroCode(\"// Macro2: ~0~\", Prefix=\"// Prefix\")] [MacroItem(\"One\")]");
    }

    [Fact]
    public void RepeatedUsing()
    {
        var gen = RunTest(@"[MacroCode(""// Code: ~0~/~1~"", Prefix = ""public void Generated() {"", Postfix = ""}"")]");
        gen.FromName("MacroGen.Outer.C.Func().cs").AssertContains("using Melville.INPC");
    }    
    [Fact]
    public void Gen()
    {
        var generatorTestBed = RunTest(@"         [MacroCode(""// Code: ~0~/~1~"", Prefix = ""public void Generated() {"", Postfix = ""}"")]
        [MacroItem(1, ""One"")]
        [MacroItem(2, ""Two"")]
        [MacroItem(3, ""Three"")]
");
        generatorTestBed.FromName("MacroGen.Outer.C.Func().cs").AssertContains("// Code: 1/One");
    }
}