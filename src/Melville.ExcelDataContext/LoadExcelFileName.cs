using LINQPad.Extensibility.DataContext;
using Microsoft.Win32;

namespace Melville.ExcelDataContext
{
    public static class LoadExcelFileName
    {
        public static string? GetFileNameFromUI() 
        {
            var ofd = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xlsx",
                DereferenceLinks = true,
                Filter =
                    "Data Files (*.xls;*.xlsx;*.csv)|*.xls;*.xlsx;*.csv|Modern EXcel (*.xlsx)|*.xlsx|Old Excel Files (*.xls)|*.xls|Comma Separated Value (*.csv)|*.csv|All Files (*.*)|*.*",
                Multiselect =
                    false,
                ReadOnlyChecked = true,
                Title = "Pick file to load",
                ValidateNames = true
            };
            return ofd.ShowDialog() ?? false ? ofd.FileName: null;
        }

        public static bool LoadExcelFileAsConnectionInfo(IConnectionInfo cxInfo)
        {
            var fileName = LoadExcelFileName.GetFileNameFromUI();
            if (fileName != null)
            {
                new ExcelFileProperties(cxInfo).FilePath = fileName;
                return true;
            }

            return false;
        }
    }
}