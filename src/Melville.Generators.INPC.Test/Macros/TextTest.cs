using Melville.Generators.INPC.ProductionGenerators.Macros;
using Melville.Generators.INPC.Test.UnitTests;
using Xunit;

namespace Melville.Generators.INPC.Test.Macros;

public class TextTest
{
    protected GeneratorTestBed RunTest(string s) =>
        new(new MacroGenerator(), $$"""
            namespace Melville.INPC 
            {
                public sealed class MacroItemAttribute: System.Attribute
                {
                    public MacroItemAttribute(params object[] text){}
                }
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
    [InlineData("[MacroCode(\"// Macro: ~0~\", Prefix = \"// 233\")] [MacroItem(\"One\")][MacroItem(\"Two\")]", "// Macro: Two")]
    [InlineData("[MacroCode(\"// Macro: ~0~\", Prefix = \"// 233\")] [MacroItem(\"One\")][MacroItem(\"Two\")]", "// 233")]
    [InlineData("[MacroCode(\"// Macro: ~0~\", Postfix = \"// 233\")] [MacroItem(\"One\")][MacroItem(\"Two\")]", "// 233")]
        
    public void SimpleSub(string input, string output) => 
        RunTest(input).FileContains("MacroGen.Outer.C.Func().cs", output);

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
        gen.FileContains("MacroGen.Outer.C.Func().cs", "using Melville.INPC");
    }    
    [Fact]
    public void Gen()
    {
        var generatorTestBed = RunTest(@"         [MacroCode(""// Code: ~0~/~1~"", Prefix = ""public void Generated() {"", Postfix = ""}"")]
        [MacroItem(1, ""One"")]
        [MacroItem(2, ""Two"")]
        [MacroItem(3, ""Three"")]
");
        generatorTestBed.FileContains("MacroGen.Outer.C.Func().cs", "// Code: 1/One");
    }
}