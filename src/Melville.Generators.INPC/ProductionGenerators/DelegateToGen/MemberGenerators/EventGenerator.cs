using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen.MemberGenerators;

public class EventGenerator : MemberGenerator<IEventSymbol>
{
    public EventGenerator(IEventSymbol sourceSymbol, ClassGenerator host, string memberName) : 
        base(sourceSymbol, host, memberName)
    {
    }

    protected override ITypeSymbol ResultType => SourceSymbol.Type;
    protected override string CopiedAttributeName() => "event";
    protected override string EventElt() => "event ";

    public override void WriteSymbol(CodeWriter cw)
    {
        base.WriteSymbol(cw);
        cw.AppendLine();
        using var blk = cw.CurlyBlock();
        TryWriteMethod(cw, SourceSymbol.AddMethod, "add", " +");
        TryWriteMethod(cw, SourceSymbol.RemoveMethod, "remove", " -");
    }

    private void TryWriteMethod(
        CodeWriter cw, IMethodSymbol? subMethod, string memberName, string operationType)
    {
        if (subMethod is not null)
        {
            cw.Append(memberName);
            cw.Append(" => ");
            AppendHostSymbolAccess(cw);
            cw.Append(".");
            cw.Append(SourceSymbol.Name);
            cw.Append(operationType);
            cw.AppendLine("= value;");
        }
    }
}