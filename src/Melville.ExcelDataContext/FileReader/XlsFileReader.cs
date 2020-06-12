using System.Collections.Generic;
using System.IO;
using System.Linq;
using ExcelDataReader;

namespace Melville.ExcelDataContext.FileReader
{
    public class XlsFileReader : RawFileReader
    {
        public override void Load(string fileName)
        {
            using (var inputStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 10240))
            {
                var dataReader = CreateReader(inputStream);
                do
                {
                    var rows = new List<IList<string>>();
                    while (dataReader.Read())
                    {
                        rows.Add(Enumerable.Range(0, dataReader.FieldCount)
                            .Select(i => dataReader.GetValue(i)?.ToString())
                            .Select(i=> i??"")
                            .ToList());
                    }
                    InsertTable(dataReader.Name,rows);
          
                } while (dataReader.NextResult());
            }
        }

        protected virtual IExcelDataReader CreateReader(FileStream inputStream) => 
            ExcelReaderFactory.CreateBinaryReader(inputStream);
    }
}