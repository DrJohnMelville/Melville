using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Melville.Generators.INPC.CodeWriters
{
    public class CodeWriter
    {
        private readonly GeneratorExecutionContext context;
        private readonly List<string> prefixLines = new();
        private readonly IndentedStringBuilder target = new();
        public CodeWriter(GeneratorExecutionContext context)
        {
            this.context = context;
        }

        public void PublishCodeInFile(string fileName) => 
            context.AddSource(fileName, SourceText.From(this.ToString(), Encoding.UTF8));

            

        public void AddPrefixLine(string line) => prefixLines.Add(line);
        public void AppendLine(string s = "") => target.AppendLine(s);
        public void Append(string s) => target.Append(s);
        public IDisposable CurlyBlock() => target.CurlyBlock();
        public IDisposable IndentedRun() => target.IndentedRun();
        public override string ToString() => string.Join(Environment.NewLine, prefixLines.Append(target.ToString()));

        public void ReportDiagnostic(Diagnostic diagnostic) => context.ReportDiagnostic(diagnostic);
    }

}