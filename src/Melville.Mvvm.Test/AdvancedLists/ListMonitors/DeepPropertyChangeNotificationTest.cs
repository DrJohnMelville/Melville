#nullable disable warnings
using Melville.Lists;
using Melville.Lists.ListMonitors;
using Melville.MVVM.BusinessObjects;
using Xunit;

namespace Melville.Mvvm.Test.AdvancedLists.ListMonitors;

public sealed class DeepPropertyChangeNotificationTest
{
  private class Inner:NotifyBase
  {
    private int property;
    public int Property
    {
      get => property;
      set => AssignAndNotify(ref property, value);
    }      
  }

  private readonly Inner existing = new Inner();
  private readonly ObservableCollectionWithProperClearMethod<Inner> col = new ObservableCollectionWithProperClearMethod<Inner>();
  private DeepPropertyChangeNotification<Inner> sut = new DeepPropertyChangeNotification<Inner>();
  private int calls = 0;

  public DeepPropertyChangeNotificationTest()
  {
    col.Add(existing);
    sut.AttachToList(col);
    sut.SubPropertyChanged += (_, __) => ++calls;
  }

  [Fact]
  public void FactName()
  {
    Assert.Equal(0, calls);
    ++existing.Property;
    Assert.Equal(1, calls);
        
  }

  [Fact]
  public void MonitorNewItem()
  {
    var newObj = new Inner();
    ++newObj.Property;
    Assert.Equal(0, calls);
    col.Add(newObj);
    ++newObj.Property;
    Assert.Equal(1, calls);
  }

  [Fact]
  public void RemovePreventsTracking()
  {
    col.Remove(existing);
    ++existing.Property;
    Assert.Equal(0, calls);
      
  }
  [Fact]
  public void ClearPreventsTracking()
  {
    col.Clear();
    ++existing.Property;
    Assert.Equal(0, calls);
      
  }



}