using System.IO;
using ExcelDataReader;

namespace Melville.ExcelDataContext.FileReader
{
    public sealed class XlsxFileReader : XlsFileReader
    {
        protected override IExcelDataReader CreateReader(FileStream inputStream) =>
            ExcelReaderFactory.CreateOpenXmlReader(inputStream);
    }
}