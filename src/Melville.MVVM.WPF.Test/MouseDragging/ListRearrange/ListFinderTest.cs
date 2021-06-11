using System;
using System.Collections.Generic;
using System.Collections;
using Melville.MVVM.Wpf.MouseDragging.ListRearrange;
using Xunit;

namespace Melville.MVVM.WPF.Test.MouseDragging.ListRearrange
{
    public class ListFinderTest
    {
        [Fact] public void StringIsNotAList() => Assert.False(ListFinder.IsAMutableListOf("string", typeof(int)));
        [Fact] public void ListOfIntIsAList() => Assert.True(ListFinder.IsAMutableListOf(new List<int>(), typeof(int)));
        [Fact] public void ListIsAList() => Assert.True(ListFinder.IsAMutableListOf(new ArrayList(), typeof(int)));
        [Fact] public void WrongListOfType() => Assert.False(ListFinder.IsAMutableListOf(new List<DateTime>(), typeof(int)));
        [Fact] public void ArrayIsNotAMutableList() => Assert.False(ListFinder.IsAMutableListOf(new int[0], typeof(int)));
    }
}