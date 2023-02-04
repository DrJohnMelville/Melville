using System.Collections.Generic;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators;

public class MethodGenerator : MemberGenerator<IMethodSymbol>
{
    private readonly MappedMethod typeMapping;

    public MethodGenerator(IMethodSymbol sourceSymbol, ClassGenerator host, string memberName) : base(
        sourceSymbol, host, memberName)
    {
        typeMapping = host.WrappingStrategy.MethodMappingFor(sourceSymbol.ReturnType);

    }

    protected override ITypeSymbol ResultType => typeMapping.FinalType;
    protected override string CopiedAttributeName() => "method";

    public override bool IsSuppressedBy(ISymbol comparisonItem)
    {
        if (!base.IsSuppressedBy(comparisonItem)) return false;
        if (comparisonItem is not IMethodSymbol ms) return true;
        if (SourceSymbol.TypeArguments.Length != ms.TypeArguments.Length) return false;
        return SourceSymbol.Parameters.IsEquivilentTo(ms.Parameters);
    }

    public override void WriteSymbol(CodeWriter cw)
    {
        base.WriteSymbol(cw);
        AppendTypeParamList(cw);
        var pList = new ParameterlistWriter(cw, "(", SourceSymbol.Parameters, ")");
        pList.RenderParameterList();
        cw.Append(typeMapping.OpenBody);
        AppendHostSymbolAccess(cw);
        cw.Append(".");
        cw.Append(SourceSymbol.Name);
        AppendTypeParamList(cw);
        pList.AppendArgumentList();
        cw.AppendLine(typeMapping.CloseBody);
    }

    private void AppendTypeParamList(CodeWriter cw)
    {
        if (SourceSymbol.TypeParameters.Length == 0) return;
        cw.Append("<");
        cw.Append(string.Join(",", SourceSymbol.TypeParameters.Select(i => i.Name)));
        cw.Append(">");
    }
}