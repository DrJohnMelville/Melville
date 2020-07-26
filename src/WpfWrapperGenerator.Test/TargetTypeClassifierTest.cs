using System;
using Xunit;

namespace WpfWrapperGenerator.Test
{
    public class Open {}
    public sealed class Closed:Open{}
    public class TargetTypeClassifierTest
    {
        [Theory]
        [InlineData(typeof(Open), "TChild", "where TChild:WpfWrapperGenerator.Test.Open", "TChild, ")]
        [InlineData(typeof(Closed), "WpfWrapperGenerator.Test.Closed", "", "")]
        [InlineData(typeof(int), "System.Int32", "", "")]
        [InlineData(typeof(object), "System.Object", "", "")]
        public void ClassifyTest(Type type, string name, string where, string template)
        {
            var (typeName, whereClause, templateparam) = TargetTypeClassifier.Classify(type);
            Assert.Equal(name, typeName);
            Assert.Equal(where, whereClause);
            Assert.Equal(template, templateparam);
        }
    }
}