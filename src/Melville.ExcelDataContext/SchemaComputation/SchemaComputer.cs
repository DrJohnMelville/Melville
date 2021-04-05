using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Melville.ExcelDataContext.SchemaComputation
{
    public static class SchemaComputer
    {
        public static SchemaTableDecl[] GetSchemaDecl(Dictionary<string, IList<IList<string>>> tables)
        {
            return tables.Select(i => new SchemaTableDecl(i.Key,
                    ComputeColumnSchema(i.Value)))
                .ToArray();
        }

        private static SchemaFieldDecl[] ComputeColumnSchema(IList<IList<string>> value)
        {
            var titleRow = value.FirstOrDefault();
            if (titleRow == null) return new SchemaFieldDecl[0];
            var namer = new NameSpace();
            return titleRow.Select((columnName,columnIndex) => 
                new SchemaFieldDecl(namer.Rename(columnName), 
                    ComputeType(DataForColumnByIndex(value, columnIndex))))
                .ToArray();
        }

        private static IEnumerable<string> DataForColumnByIndex(IList<IList<string>> value, int count)
        {
            return value.Select(j=>CellFromRowByColumn(j, count)).Skip(1);
        }

        private static string CellFromRowByColumn(IList<string> row, int count) => 
            count < row.Count? row[count]:"";
        
        private static Regex BooleanDetector = 
            new Regex(@"^\s*(Y|Yes|T|True|N|No|F|False)\s*$", RegexOptions.IgnoreCase);
        private static ISchemaTargetType ComputeType(IEnumerable<string> values)
        {
            var candidateTypes = new[]
            {
                new TargetType(i => int.TryParse(i, out var _), SchemaTargetType.Int),
                new TargetType(i => Double.TryParse(i, out var _),
                    SchemaTargetType.Double),
                new TargetType(i => DateTime.TryParse(i, out var _), SchemaTargetType.Date),
                new TargetType(i=> BooleanDetector.Match(i).Success, SchemaTargetType.Bool), 
            };
            foreach (var value in values)
            {
                {
                    foreach (var test in candidateTypes)
                    {
                        test.DoTest(value);
                    }
                }
            }
      
            return candidateTypes
                .Select(i => i.FinalSchema)
                .OfType<ISchemaTargetType>()
                .DefaultIfEmpty(SchemaTargetType.String)
                .First();
        }
    }
}