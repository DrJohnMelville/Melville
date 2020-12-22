using System;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Melville.Generators.Tools.CodeWriters
{
    public class CodeWriter
    {
        private readonly GeneratorExecutionContext context;
        private readonly IndentedStringBuilder target = new IndentedStringBuilder();
        public CodeWriter(GeneratorExecutionContext context)
        {
            this.context = context;
        }

        public void PublishCodeInFile(string fileName) => 
            context.AddSource(fileName, SourceText.From(target.ToString(), Encoding.UTF8));

        public void AppendLine(string s = "") => target.AppendLine(s);
        public void Append(string s) => target.Append(s);
        public IDisposable CurlyBlock() => target.CurlyBlock();
        public IDisposable IndentedRun() => target.IndentedRun();
        public override string ToString() => target.ToString();

        public void ReportDiagnostic(Diagnostic diagnostic) => context.ReportDiagnostic(diagnostic);
    }
}