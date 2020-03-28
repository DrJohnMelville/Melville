using System.Collections.Generic;
using System.Linq;

namespace Melville.ExcelDataContext.FileReader
{
    public class LiteralRawFileReader:RawFileReader
    {
        public override void Load(string fileName)
        {
            // do Nothing
        }

        /// <summary>
        /// This is a debugging hook
        /// </summary>
        /// <param name="tableName">Name of the data to add</param>
        /// <param name="data">Debug data to add</param>
        public void AddTable(string tableName, IEnumerable<IEnumerable<string>> data)
        {
            InsertTable(tableName, data.Select(i => i.ToList() as IList<string>).ToList());
        }
    }
}