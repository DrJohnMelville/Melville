using System.Text;
using LINQPad.Extensibility.DataContext;

namespace Melville.ExcelDataContext.SchemaComputation
{
    public class SchemaFieldDecl
    {
        public ISchemaTargetType Type { get; }
        public string Name { get; }

        public SchemaFieldDecl(string name, ISchemaTargetType type)
        {
            Type = type;
            Name = name;
        }

        public ExplorerItem SchemaDisplay() =>
            new ExplorerItem($"{Name} ({Type.TypeName})", ExplorerItemKind.Property, ExplorerIcon.Column);

        public void GenerateFieldDecl(StringBuilder code)
        {
            code.AppendFormat("public {0} {1} {{get; set;}}\r\n", Type.TypeName, Name);
        }
    }
}