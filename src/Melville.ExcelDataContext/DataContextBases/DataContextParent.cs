using Melville.ExcelDataContext.FileReader;

namespace Melville.ExcelDataContext.DataContextBases
{
    public abstract class DataContextBase
    {
        protected DataContextBase(RawFileReader rawFileReader)
        {
            RawFileReader = rawFileReader;
        }

        public RawFileReader RawFileReader { get; }
    }
}