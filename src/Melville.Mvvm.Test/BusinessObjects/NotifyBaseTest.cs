#nullable disable warnings
using  System;
using Melville.MVVM.BusinessObjects;
using Melville.TestHelpers.InpcTesting;
using Xunit;

namespace Melville.Mvvm.Test.BusinessObjects
{
  public class NotifyBaseTests
  {
    [Fact]
    public void FirePropChange()
    {
      int count = 0;
      var obj = new SimpleContainer();
      obj.PropertyChanged += (s, e) =>
      {
        Assert.Equal("Value", e.PropertyName);
        count++;
      };

      Assert.Equal(0, count);
      obj.Value = "Hello";

      Assert.Equal(1, count);
      Assert.Equal("Hello", obj.Value);

      // show that equal values do not fire
      obj.Value = "Hello";
      Assert.Equal(1, count);
      Assert.Equal("Hello", obj.Value);

      obj.Value = "Hello";
      Assert.Equal(1, count);
      Assert.Equal("Hello", obj.Value);
    }
#if DEBUG
    [Fact]
    public void TestIfPropertyExists()
    {
      // throws a deeply internal contract exception who'se name I cannot say
      try
      {
        new SimpleContainer().RaiseInvalidEvent();
      }
      catch (Exception)
      {
        return;
      }
      Assert.True(false, "Should never get here");
    }
#endif

    private class SimpleContainer : NotifyBase
    {
      private string value;
      public string Value
      {
        get { return value; }
        set { AssignAndNotify(ref this.value, value, "Value"); }
      }
      public void RaiseInvalidEvent()
      {
        OnPropertyChanged("This is not a property");
      }
    }

    [Fact]
    public void MonitorForeignEvent()
    {
      var sc1 = new SimpleContainer();
      var sc2 = new SimpleContainer();
      sc2.DelegatePropertyChangeFrom(sc1, "Value", "Value");

      using (var foo = INPCCounter.VerifyInpcFired(sc2, o => o.Value))
      {
        sc1.Value = "This is not the default value";
      }
    }

    [Fact]
    public void DelegateChange()
    {
      var sc1 = new SimpleContainer();
      var sc2 = new SimpleContainer();
      sc2.DelegatePropertyChangeFrom(sc1, "Value", "Value");
      var invoked = 0;
      sc2.PropertyChanged += (s, e) => invoked++;
      Assert.Equal(0, invoked);
      sc1.Value = "foo";
      Assert.Equal(1, invoked);
    }
  }
}
