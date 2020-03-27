using LINQPad.Extensibility.DataContext;

namespace Melville.ExcelDataContext
{
    public class ExcelFileProperties
    {
        private readonly IConnectionInfo connectionInfo;

        public ExcelFileProperties(IConnectionInfo connectionInfo)
        {
            this.connectionInfo = connectionInfo;
        }

        public string FilePath
        {
            get { return (string) connectionInfo.DriverData.Element("Path") ?? ""; }
            set { connectionInfo.DriverData.SetElementValue("Path", value); }
        }
    }
}