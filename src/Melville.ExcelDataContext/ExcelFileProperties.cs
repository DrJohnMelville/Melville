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
            get { return connectionInfo.DriverData.Element("Path")?.ToString() ?? ""; }
            set { connectionInfo.DriverData.SetElementValue("Path", value); }
        }
    }
}