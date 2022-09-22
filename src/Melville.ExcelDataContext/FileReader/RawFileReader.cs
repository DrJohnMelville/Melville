using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Melville.ExcelDataContext.SchemaComputation;

namespace Melville.ExcelDataContext.FileReader
{
    public abstract class RawFileReader
    {
        static RawFileReader()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        //Tables  must be public because the generated datacontext code accesses it.
        public Dictionary<string, IList<IList<String>>> Tables { get; } = new();
        protected abstract void Load(string fileName);

        public static RawFileReader LoadFile(string path)
        {
            var ret = FileFormatReader(path);
            ret.Load(path);
            return ret;
        }

        private static RawFileReader FileFormatReader(string path) =>
            path switch
            {
                var s when s.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) => new XlsFileReader(),
                var s when s.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) => new XlsxFileReader(),
                _ => new CsvFileReader()
            };

        public ISchemaDataSource GetSchemaDataSource() => new RawFileParser(Tables);
        private readonly NameSpace tableNames = new();
        protected void InsertTable(string tableName, IList<IList<string>> rows) => 
            Tables[tableNames.Rename(tableName)] = rows;
    }
}