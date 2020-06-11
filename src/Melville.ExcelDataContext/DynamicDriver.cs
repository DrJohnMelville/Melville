using System;
using LINQPad;
using LINQPad.Extensibility.DataContext;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Melville.ExcelDataContext
{
	public class DynamicDriver : DynamicDataContextDriver
	{
		static DynamicDriver()
		{
			AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
			{
				//File.WriteAllText(@"C:\users\jom252\desktop\linqException.txt",args.Exception.Message+args.Exception.StackTrace);
				if (args.Exception.StackTrace?.Contains(typeof (DynamicDriver).Namespace??"") ?? true)
					Debugger.Launch ();
			};
	
		}
		#region UI Identifiers
		public override string Name => "Excel or CSV File";
		public override string Author => "John Melville";
		public override string GetConnectionDescription(IConnectionInfo cxInfo) =>
			Path.GetFileName(new ExcelFileProperties(cxInfo).FilePath);
		#endregion
		public override bool ShowConnectionDialog(IConnectionInfo cxInfo, ConnectionDialogOptions options) => 
			LoadExcelFileName.LoadExcelFileAsConnectionInfo(cxInfo);

		public override List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo cxInfo, AssemblyName assemblyToBuild, 
			ref string nameSpace, ref string typeName) => 
			TargetObjectBuilder.GetSchemaAndBuildAssembly(assemblyToBuild, nameSpace, typeName, 
				ExcelFileCache.LoadFile(cxInfo).GetSchemaDataSource());

		public override ParameterDescriptor[] GetContextConstructorParameters(IConnectionInfo cxInfo)
		{  
			return new[]
			{
				new ParameterDescriptor("rawFileReader", "Melville.ExcelDataContext.FileReader.RawFileReader")
			};
		}
		
		public override object[] GetContextConstructorArguments(IConnectionInfo cxInfo)
		{
			return new[] {ExcelFileCache.LoadFile(cxInfo)};
		}

		public override bool AreRepositoriesEquivalent(IConnectionInfo c1, IConnectionInfo c2)
		{
			string FullName(IConnectionInfo info) => new FileInfo(new ExcelFileProperties(info).FilePath).FullName;
			return FullName(c1).Equals(FullName(c2), StringComparison.OrdinalIgnoreCase);
		}

		public override DateTime? GetLastSchemaUpdate(IConnectionInfo cxInfo) => DateTime.Now;
		
	}
}