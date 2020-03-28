using System.Collections.Generic;
using System.Linq;
using LINQPad.Extensibility.DataContext;
using Melville.ExcelDataContext.SchemaComputation;

namespace Melville.ExcelDataContext.FileReader
{
  public sealed class RawFileParser : ISchemaDataSource
    {
        private readonly Dictionary<string, IList<IList<string>>> tables;
        private readonly SchemaTableDecl[] schema;

        public RawFileParser(Dictionary<string, IList<IList<string>>> tables)
        {
            this.tables = tables;
            schema = SchemaComputer.GetSchemaDecl(tables);
        }

        public string GetSourceCode(string nameSpace, string typeName)
        {
            return schema.GenerateDataObjectSourceCode(nameSpace, typeName);
        }

        public List<ExplorerItem> GetSchema()
        {
            return schema
                .Select(i=>i.SchemaDisplay())
                .ToList();
        }
    }
}