using System.Collections.Generic;
using LINQPad.Extensibility.DataContext;

namespace Melville.ExcelDataContext.SchemaComputation
{
    public interface ISchemaDataSource
    {
        string GetSourceCode(string nameSpace, string typeName);
        List<ExplorerItem> GetSchema();
    }
}