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

        private class TargetType
        {
            public ISchemaTargetType WithNulls => new UnknownSchemaAdaptor(WithoutNulls);
            public ISchemaTargetType WithoutNulls { get; }
            private Func<string, bool> test;
            public bool InContention { get; set; } = true;

            public TargetType(Func<string, bool> test, SchemaTargetType withoutNulls)
            {
                this.test = test;
                WithoutNulls = withoutNulls;
            }

            public void DoTest(string sample)
            {
                if (!InContention) return;
                if (!test(sample))
                {
                    InContention = false;
                }
            }
        }

        private static Regex BooleanDetector = 
            new Regex(@"^\s*(Y|Yes|T|True|N|No|F|False)\s*$", RegexOptions.IgnoreCase);
        /// <summary>
        /// This is a complicated method.  Basically we want to find out if all the values are ints,
        /// doubles, dates, etc so we can use a better datatype.  In addition, whitespace indicates
        /// missing values.
        /// 
        /// So we loop through the data testing only the nonwhitespace items, and keeping track if we
        /// see whitespace and nonwhitespace.
        /// 
        /// If there is no notwhitespace, then it is a string column.
        /// Otherwise the first type in the list that matched all of the inputs is the winner.
        /// If no one wins, then string is the default.
        /// 
        /// After we have a winner, each winner has two variants, a nullable variant if there are missing
        /// values and a nonnullable variant if there are not.
        /// </summary>
        /// <param name="values">Values to be inspected</param>
        /// <returns>SchemaTargetType that describes these values</returns>
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
            bool hasKnown = false;
            bool hasUnknown = false;
            foreach (var value in values)
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    hasUnknown = true;
                }
                else
                {
                    hasKnown = true;
                    foreach (var test in candidateTypes)
                    {
                        test.DoTest(value);
                    }
                }
            }
            if (!hasKnown) return SchemaTargetType.String;
      
            return candidateTypes
                .Where(i => i.InContention)
                .Select(i => hasUnknown ? i.WithNulls : i.WithoutNulls)
                .DefaultIfEmpty(SchemaTargetType.String)
                .First();
        }
    }
}