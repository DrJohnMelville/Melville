using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace Melville.ExcelDataContext.FileReader
{
    public class CsvFileReader : RawFileReader
    {
        public override void Load(string fileName)
        {
            using (var parser = new TextFieldParser(fileName))
            {
                parser.SetDelimiters(",");
                var rows = new List<IList<string>>();
                while (!parser.EndOfData)
                {
                    if (parser.ReadFields() is {} fields) rows.Add(fields.ToList());
                }
                InsertTable(Path.GetFileNameWithoutExtension(fileName), rows);
            }
        }
    }
}