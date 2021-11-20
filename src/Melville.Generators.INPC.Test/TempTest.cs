using System.Linq;
using Melville.Generators.INPC.CodeWriters;
using Melville.Generators.INPC.INPC;
using Melville.Generators.INPC.PartialTypeGenerators;
using Melville.Generators.INPC.Test.UnitTests;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace Melville.Generators.INPC.Test;

public class simpleTestGen : NewPartialTypeGenerator<string>
{
    public simpleTestGen() : base("MacroGen", 
        "Melville.INPC.MacroCodeAttribute", "Melville.INPC.MacroCodeAttribute")
    {
    }
    
    protected override string PreProcess(IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input, GeneratorExecutionContext context)
    {
        return $"//{input.Key} : {string.Join("\r\n     //", input.Select(i => i.ToString()))}";
    }

    protected override bool GlobalDeclarations(CodeWriter cw)
    {
        cw.AppendLine("// global file");
        return true;
    }

    protected override bool GenerateClassContents(string input, CodeWriter cw)
    {
        cw.AppendLine(input);
        return true;
    }
}

public class TempTest
{
    [Fact]
    public void SingleTest()
    {
        var tb = new GeneratorTestBed(new simpleTestGen(),
            $@"
using Melville.INPC;
namespace Outer{{
namespace NM
{{
  public partial class C
  {{ 
    private int zeroProp;
    [AutoNotify] private int wrongProp;
    [MacroCode()] private int OneProp;
    [MacroCode][MacroCode] private int TwoProp;
  }}
  public partial class D {{
    [MacroCode] private int in2ndClass;
  }}
}}");
        tb.FileContains("MacroGenAttributes.MacroGen.cs", "// global file");
        tb.FileContains("Hello0.cs", "GlobalFile");
    }
}