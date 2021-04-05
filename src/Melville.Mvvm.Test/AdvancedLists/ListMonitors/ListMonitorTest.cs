#nullable disable warnings
using  System;
using System.Collections.Generic;
using System.Text;
using Melville.Lists.AdvancedLists;
using Melville.Lists.AdvancedLists.ListMonitors;
using Moq;
using Xunit;

namespace Melville.Mvvm.Test.AdvancedLists.ListMonitors
{
  public sealed class ListMonitorTest
  {
    // most of this class is tested in ParentListTest, because this interface was
    // originally a refactoring of that class.  This class tests a few features not
    // used by ParentList

    [Fact]
    public void DisposeUnhooksFromCollectionChanged()
    {
      ThreadSafeBindableCollection<int> col = new ThreadSafeBindableCollection<int>();
      var mock = new Mock<IListMonitor<int>>();
      var releaseDelegate = mock.Object.AttachToList(col);

      col.Add(1);
      mock.Verify(o => o.NewItem(It.IsAny<int>(), It.IsAny<int>()), Times.Once());

      releaseDelegate();

      col.Add(1);
      mock.Verify(o => o.NewItem(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
    }
    [Fact]
    public void DisposeUnhooksFromClearing()
    {
      ThreadSafeBindableCollection<int> col = new ThreadSafeBindableCollection<int>();
      var mock = new Mock<IListMonitor<int>>();
      var releaseDelegate = mock.Object.AttachToList(col);
      col.Add(1);
      col.Clear();
      mock.Verify(o => o.DestroyItem(It.IsAny<int>(), It.IsAny<int>()), Times.Once());

      col.Add(1);
      releaseDelegate();

      col.Clear();
      mock.Verify(o => o.DestroyItem(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
    }
  }
}
