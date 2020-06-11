using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LINQPad.Extensibility.DataContext;
using Xunit;

namespace Melville.ExcelDataContext.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var ci = new FakeCi();
            new ExcelFileProperties(ci).FilePath = @"R:\WorkingData\Coombined.xlsx";
            ExcelFileCache.LoadFile(ci);
        }
    }

    public class FakeCi : IConnectionInfo
    {
        public string Encrypt(string data)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string data)
        {
            throw new NotImplementedException();
        }

        public IConnectionInfo CreateCopy()
        {
            throw new NotImplementedException();
        }

        public IConnectionInfo CreateChild()
        {
            throw new NotImplementedException();
        }

        public IDatabaseInfo DatabaseInfo { get; }
        public ICustomTypeInfo CustomTypeInfo { get; }
        public IDynamicSchemaOptions DynamicSchemaOptions { get; }
        public string DisplayName { get; set; }
        public string AppConfigPath { get; set; }
        public bool Persist { get; set; }
        public bool IsProduction { get; set; }
        public bool PopulateChildrenOnStartup { get; set; }
        public XElement DriverData { get; set; } = new XElement("Config");
        public IDictionary<string, object> SessionData { get; }
    }
}