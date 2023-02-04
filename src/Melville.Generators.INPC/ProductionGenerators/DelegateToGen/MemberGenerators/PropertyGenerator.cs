using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.ClassGenerators;
using Melville.Generators.INPC.ProductionGenerators.DelegateToGen.OurputWrapping;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators;

public class IndexerGenerator : PropertyGenerator
{
    public IndexerGenerator(IPropertySymbol sourceSymbol, ClassGenerator host) 
        : base(sourceSymbol, host, "this")
    {
    }

    protected override void WriteArgumentList(CodeWriter cw) => 
        ParameterListWriter(cw).RenderParameterList();

    private ParameterlistWriter ParameterListWriter(CodeWriter cw) => 
        new(cw, "[", SourceSymbol.Parameters, "]");

    protected override void SourcePropertyCall(CodeWriter cw)
    {
        ParameterListWriter(cw).AppendArgumentList();
    }

    public override bool IsSuppressedBy(ISymbol comparisonItem) =>
        comparisonItem is IPropertySymbol ps &&
        SourceSymbol.Parameters.IsEquivilentTo(ps.Parameters);
}

public class PropertyGenerator : MemberGenerator<IPropertySymbol>
{
    private readonly MappedMethod getMethodMapping;
    private readonly MappedMethod setMethodMapping;

    public PropertyGenerator(IPropertySymbol sourceSymbol, ClassGenerator host, string memberName) :
        base(sourceSymbol, host, memberName)
    {
        getMethodMapping = host.WrappingStrategy.MethodMappingFor(sourceSymbol.Type);
        setMethodMapping = (sourceSymbol.SetMethod is { } sm)
            ? host.WrappingStrategy.MethodMappingFor(sm.ReturnType)
            : getMethodMapping;
    }

    protected override ITypeSymbol ResultType => getMethodMapping.FinalType;
    protected override string CopiedAttributeName() => "property";

    public override bool IsSuppressedBy(ISymbol comparisonItem) =>
        base.IsSuppressedBy(comparisonItem) &&
        (comparisonItem is not IPropertySymbol ps ||
         SourceSymbol.Parameters.IsEquivilentTo(ps.Parameters));

    public override void WriteSymbol(CodeWriter cw)
    {
        base.WriteSymbol(cw);
        WriteArgumentList(cw);
        cw.AppendLine();
        PropertyBlock(cw);
    }

    protected virtual void WriteArgumentList(CodeWriter cw){}

    private void PropertyBlock(CodeWriter cw)
    {
        using var _ = cw.CurlyBlock();
        if (SourceSymbol.GetMethod != null)
        {
            cw.Append("get");
            cw.Append(getMethodMapping.OpenBody);
            AppendHostSymbolAccess(cw);
            SourcePropertyCall(cw);
            cw.AppendLine(getMethodMapping.CloseBody);
        }

        if (SourceSymbol.SetMethod is { })
        {
            cw.Append(SetMethodKeyword());
            cw.Append(setMethodMapping.OpenBody);
            AppendHostSymbolAccess(cw);
            SourcePropertyCall(cw);
            cw.Append(" = ");
            cw.Append(getMethodMapping.CastResultTo(SourceSymbol.Type));
            cw.Append("value");
            cw.AppendLine(setMethodMapping.CloseBody);
        }
    }

    protected virtual void SourcePropertyCall(CodeWriter cw)
    {
        cw.Append(".");
        cw.Append(SourceSymbol.Name);
    }

    private string SetMethodKeyword() => SourceSymbol.SetMethod?.IsInitOnly??false ? "init" : "set";
}