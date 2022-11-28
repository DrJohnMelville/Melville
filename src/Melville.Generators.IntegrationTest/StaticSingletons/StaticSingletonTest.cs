using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melville.INPC;
using Xunit;

namespace Melville.Generators.IntegrationTest.StaticSingletons;

[StaticSingleton]
public partial class SingletonItem{}

public class StaticSingletonTest
{
    [Fact]
    public void SingletonExists() => Assert.NotNull(SingletonItem.Instance);
}