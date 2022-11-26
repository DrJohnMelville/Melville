using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.GenerationTools.CodeWriters;

public abstract class AttributeWriterStrategy
{
    public static readonly AttributeWriterStrategy OnePerLine = new OnePerLineClass();
    public static readonly AttributeWriterStrategy MultiplePerLine = new MultiplePerLineClass();
    private AttributeWriterStrategy()
    {
    }

    public abstract void RenderAttribute(CodeWriter cw, AttributeSyntax syntax);

    private class OnePerLineClass: AttributeWriterStrategy
    {
        public override void RenderAttribute(CodeWriter cw, AttributeSyntax syntax)
        {
            cw.AppendLine($"[{syntax}]");
        }
    }
    private class MultiplePerLineClass: AttributeWriterStrategy
    {
        public override void RenderAttribute(CodeWriter cw, AttributeSyntax syntax)
        {
            cw.Append($"[{syntax}] ");
        }
    }
}