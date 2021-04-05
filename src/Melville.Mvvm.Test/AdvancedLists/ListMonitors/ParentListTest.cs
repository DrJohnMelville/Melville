#nullable disable warnings
using Melville.Lists.AdvancedLists.ListMonitors;
using Melville.TestHelpers.InpcTesting;
using Xunit;

namespace Melville.Mvvm.Test.AdvancedLists.ListMonitors
{
  public sealed class ParentListTest
  {

    [Fact]
    public void AddAndReleaseSemantics()
    {
      var parent = new SimpleParent();
      var child = new NotifyChildBase<SimpleParent>();
      var list = new ParentList<NotifyChildBase<SimpleParent>, SimpleParent>(parent);

      Assert.Null(child.Parent);

      using (var foo = INPCCounter.VerifyInpcFired(child, o => o.Parent))
      {
        list.Add(child);
      }

      Assert.Equal(parent, child.Parent);

      using (var foo = INPCCounter.VerifyInpcFired(child, o => o.Parent))
      {
        list.RemoveAt(0);
      }

      Assert.Null(child.Parent);
    }
    [Fact]
    public void SetAttachesParent()
    {
      var parent = new SimpleParent();
      var child = new NotifyChildBase<SimpleParent>();
      var list = new ParentList<NotifyChildBase<SimpleParent>, SimpleParent>(parent);

      Assert.Null(child.Parent);

      list.Add(null);
      list[0] = child;

      Assert.Equal(parent, child.Parent);
    }
    [Fact]
    public void SetReleasesOutgoingParent()
    {
      var parent = new SimpleParent();
      var child1 = new NotifyChildBase<SimpleParent>();
      var child2 = new NotifyChildBase<SimpleParent>();
      var list = new ParentList<NotifyChildBase<SimpleParent>, SimpleParent>(parent);

      Assert.Null(child2.Parent);

      list.Add(child1);
      Assert.Equal(parent, child1.Parent);

      list[0] = child2;

      Assert.Null(child1.Parent);
      Assert.Equal(parent, child2.Parent);
    }
    [Fact]
    public void ClearSemantics()
    {
      var parent = new SimpleParent();
      var child1 = new NotifyChildBase<SimpleParent>();
      var child2 = new NotifyChildBase<SimpleParent>();
      var list = new ParentList<NotifyChildBase<SimpleParent>, SimpleParent>(parent);

      Assert.Null(child2.Parent);

      list.Add(child1);
      Assert.Equal(parent, child1.Parent);

      list.Add(child2);
      Assert.Equal(parent, child2.Parent);

      list.Clear();

      Assert.Null(child1.Parent);
      Assert.Null(child2.Parent);
    }

    class SimpleParent : IParent<NotifyChildBase<SimpleParent>>
    {
      public ParentList<NotifyChildBase<SimpleParent>, SimpleParent> List { get; set; }
      public SimpleParent()
      {
        List = new ParentList<NotifyChildBase<SimpleParent>, SimpleParent>(this);
      }
      public void LosingChild(NotifyChildBase<SimpleParent> child)
      {
        List.Remove(child);
      }
    }
    [Fact]
    public void TransferBetweenLists()
    {
      var parent1 = new SimpleParent();
      var parent2 = new SimpleParent();
      var list1 = parent1.List;
      var list2 = parent2.List;
      var item = new NotifyChildBase<SimpleParent>();

      Assert.Null(item.Parent);

      list1.Add(item);
      Assert.Equal(parent1, item.Parent);
      Assert.Contains(item, list1);
      Assert.DoesNotContain(item, list2);

      list2.Add(item);
      Assert.Equal(parent2, item.Parent);
      Assert.DoesNotContain(item, list1);
      Assert.Contains(item, list2);
    }
  }
}