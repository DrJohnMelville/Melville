using System;
using System.Linq;
using Melville.Linq.Statistics.HypothesisTesting;
using Xunit;

namespace Test.HypothesisTesting
{
  public class LogisticRegressionTest
  {
    [Fact]
    public void LRTest()
    {
      var data = new[]
      {
        new {Age = 55, Smoker = false, Cancer = false},
        new {Age = 28, Smoker = false, Cancer = false},
        new {Age = 65, Smoker = true, Cancer = false},
        new {Age = 46, Smoker = false, Cancer = true},
        new {Age = 86, Smoker = true, Cancer = true},
        new {Age = 56, Smoker = true, Cancer = true},
        new {Age = 85, Smoker = false, Cancer = false},
        new {Age = 33, Smoker = false, Cancer = false},
        new {Age = 21, Smoker = true, Cancer = false},
        new {Age = 42, Smoker = true, Cancer = true},
      };

      var regression = data.LogisticRegression(i=>i.Cancer)
        .WithVariable(i => i.Age)
        .WithVariable(i=>i.Smoker)
        .Regress();
      Assert.Equal("Age", regression.Coefficients[1].Name);
      Assert.Equal("Smoker", regression.Coefficients[2].Name);

      Assert.Equal(1.0208597, regression.Coefficients[1].OddsRatio,5);
      Assert.Equal(5.8584748, regression.Coefficients[2].OddsRatio,5);
      
    }
    [Fact]
    public void LRTestMissingVariable()
    {
      var data = new[]
      {
        new {Age = (double?)55, Smoker = (bool?)false, Cancer = (bool?)false},
        new {Age = (double?)null, Smoker = (bool?)false, Cancer = (bool?)false},
        new {Age = (double?)55, Smoker = (bool?)null, Cancer = (bool?)false},
        new {Age = (double?)55, Smoker = (bool?)false, Cancer = (bool?)null},
        new {Age = (double?)55, Smoker = (bool?)false, Cancer = (bool?)false},
      };

      var regression = data.LogisticRegression(i=>i.Cancer)
        .WithVariable(i => i.Age)
        .WithVariable(i=>i.Smoker)
        .Regress();
      Assert.Equal(2, regression.NumberOfSamples);
    }

    [Fact]
    public void TryDummyVariable()
    {
      var result = Enumerable.Range(1, 100).LogisticRegression(i => i > 40)
        .WithVariable("Mod 5", i => i % 5 == 0)
        .WithVariable("Mod 7", i => i % 7 == 0)
        .WithDummyVariable("Mod 4", i=>i % 4).Regress();

      Assert.Equal(5, result.Inputs.Length);
      Assert.Equal("Mod 4: 1", result.Coefficients[3].Name);
      Assert.Equal("Mod 4: 2", result.Coefficients[4].Name);
      Assert.Equal("Mod 4: 3", result.Coefficients[5].Name);
      
    }
    [Fact]
    public void TryDummyVariableWithReferrant()
    {
      var result = Enumerable.Range(1, 100).LogisticRegression(i => i > 40)
        .WithVariable(i => i % 5 == 0)
        .WithVariable(i => i % 7 == 0)
        .WithDummyVariable("Mod 4", i => i % 4, 1).Regress();

      Assert.Equal(5, result.Inputs.Length);
      Assert.Equal("(i % 5) == 0", result.Coefficients[1].Name);
      Assert.Equal("Mod 4: 0", result.Coefficients[3].Name);
      Assert.Equal("Mod 4: 2", result.Coefficients[4].Name);
      Assert.Equal("Mod 4: 3", result.Coefficients[5].Name);
    }

    [Fact]
    public void LinearRegressionTest()
    {
      var data = new[] {
        new {Quantity = 8500, Price = 2, Advertising=2800},
        new {Quantity = 4700, Price = 5, Advertising=200},
        new {Quantity = 5800, Price = 3, Advertising=400},
        new {Quantity = 7400, Price = 2, Advertising=500},
        new {Quantity = 6200, Price = 5, Advertising=3200},
        new {Quantity = 7300, Price = 3, Advertising=1800},
        new {Quantity = 5600, Price = 4, Advertising=900},
      };

      var regression = data.LinearRegression(i => i.Quantity)
        .WithVariable(i => i.Price)
        .WithVariable(i => i.Advertising).Regress();

      Assert.Equal(0.001106, regression.Coefficients[0].TTest.PValue,3);
      Assert.Equal(0.004755, regression.Coefficients[1].TTest.PValue,3);
      
    }
  }
}