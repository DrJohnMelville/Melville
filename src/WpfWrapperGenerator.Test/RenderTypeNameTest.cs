using System;
using Xunit;

namespace WpfWrapperGenerator.Test
{
    public class RenderTypeNameTest
    {
        [Theory]
        [InlineData(typeof(int), "System.Int32")]
        [InlineData(typeof(int?), "System.Nullable<System.Int32>")]
        [InlineData(typeof(string), "System.String")]
        public void TestName(Type type, string name)
        {
            Assert.Equal(name, RenderTypeName.CSharpName(type));
            
        }

    }
}