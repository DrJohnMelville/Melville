using Melville.TestHelpers.Assertions;
using Xunit;

namespace Melville.TestHelpers.Test.Assertions;

public class JsonAssertTest
{
    [Theory]
    [InlineData("1","1", "2")]
    [InlineData("[1]","[1,]", "1")]
    [InlineData("[1]","[1]", "[2]")]
    [InlineData("[1]","[1]", "[1,1]")]
    [InlineData("{\"a\" : 1}","{\"a\":1}", "{\"b\" : 1}")]
    [InlineData("{\"a\" : 1, \"b\":2}","{\"b\": 2, \"a\":1}", "{\"b\" : 2}")]
    public void AreEqualTest(string model, string same, string notSame)
    {
        JsonAssert.Equal(model, same);
        JsonAssert.NotEqual(model, notSame);
    }
}