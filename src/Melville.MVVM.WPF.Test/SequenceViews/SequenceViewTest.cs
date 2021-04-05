#nullable disable warnings
using System.Collections.Generic;
using System.Linq;
using Melville.MVVM.Wpf.SequenceViews;
using Xunit;

namespace Melville.MVVM.WPF.Test.SequenceViews
{
  public sealed class SequenceViewTest
  {
    private readonly List<int> seq = new List<int>() {1, 2, 3};
    private readonly SequenceViewModel<int> sut;

    public SequenceViewTest()
    {
      sut = new SequenceViewModel<int>(seq);
    }

    [Fact]
    public void AddNewItermDoesNothingWithNoCreator()
    {
      Assert.Equal(3, seq.Count);
      sut.AddNewItem();
      Assert.Equal(3, seq.Count);
    }

    [Fact]
    public void AddNewItemAddsItem()
    {
      sut.NewItemFactory = ()=> 10;
      Assert.Equal(3, seq.Count);
      sut.AddNewItem();
      Assert.Equal(4, seq.Count);
      Assert.Equal(10, sut.Current);
    }

    [Fact]
    public void CannotDeleteWhenNotAllowed()
    {
      sut.Current = 1;
      // CanDelete defaults to false 
      Assert.Equal(3, sut.Collection.Count);
      sut.DeleteItem();
      Assert.Equal(3, sut.Collection.Count);
      Assert.Equal(1, sut.Collection.First());
      Assert.Equal(1, sut.Current);      
    }

    [Fact]
    public void DeleteWhenAble()
    {
      sut.CanDelete = true;
      sut.Current = 1;
      Assert.Equal(3, sut.Collection.Count);
      sut.DeleteItem();
      Assert.Equal(2, sut.Collection.Count);
      Assert.Equal(2, sut.Collection.First());
      Assert.Equal(0, sut.Current);      
    }

  }
}