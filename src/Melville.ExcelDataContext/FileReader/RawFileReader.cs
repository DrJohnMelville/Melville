using System;
using System.Collections.Generic;
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
        public Dictionary<string, IList<IList<String>>> Tables { get; } = new Dictionary<string, IList<IList<string>>>();
        public abstract void Load(string fileName);

        public static RawFileReader LoadFile(string path)
        {
            RawFileReader ret = null;
            if (path.ToLower().EndsWith(".xls"))
            {
                ret = new XlsFileReader();
            }
            if (path.ToLower().EndsWith(".xlsx"))
            {
                ret = new XlsxFileReader();
            }

            ret = ret??new CsvFileReader();
            ret.Load(path);
            return ret;
      
        }

        public ISchemaDataSource GetSchemaDataSource()
        {
            return new RawFileParser(Tables);
        }
        private readonly NameSpace tableNames = new NameSpace();
        protected void InsertTable(string tableName, IList<IList<string>> rows)
        {
            Tables[tableNames.Rename(tableName)] = rows;
        }
    }
}