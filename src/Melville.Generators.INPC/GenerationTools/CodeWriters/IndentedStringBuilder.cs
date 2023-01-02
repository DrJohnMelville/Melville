using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Melville.Generators.INPC.GenerationTools.CodeWriters;

public partial class IndentedStringBuilder
{
    private readonly int indentSize;
    private readonly StringBuilder target = new StringBuilder();
    private int indent;
    private string indentString = "";
    private bool needsIndent = true;

    public IndentedStringBuilder(int indentSize = 4)
    {
        this.indentSize = indentSize;
    }
        
    public void AppendLine(string s = "")
    {
        Append(s);
        target.AppendLine();
        needsIndent = true;
    }
    public void Append(string s)
    {
        AppendIndentIfNeeded();
        target.Append(InsertInternalIndentStrings(s));
    }

    private string InsertInternalIndentStrings(string s) => s.Replace(Environment.NewLine, Environment.NewLine+indentString);

    private void AppendIndentIfNeeded()
    {
        if (needsIndent)
        {
            target.Append(indentString);
        }
        needsIndent = false;
    }

    private void AddIndent(int indentSize)
    {
        indent = Math.Max(0, indent + indentSize);
        indentString = new string(' ', indent);
    }

    public IDisposable CurlyBlock()
    {
        AppendLine("{");
        return IndentedRun(indentSize, "}");
    }
    public IDisposable IndentedRun() => IndentedRun(indentSize);

    private IDisposable IndentedRun(int indentSize, string? closer = null)
    {
        AddIndent(indentSize);
        return new RunActionOnDispose(()=>
        {
            AddIndent(-this.indentSize);
            if (closer != null)
            {
                AppendLine(closer);
            }
        });
    }
    public override string ToString() => target.ToString();

}