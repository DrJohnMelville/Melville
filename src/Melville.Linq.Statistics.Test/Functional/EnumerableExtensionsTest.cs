using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Melville.Linq.Statistics.Functional;
using Xunit;

namespace Test.Functional
{
  public class EnumerableExtensionsTest
  {
    private void TestInterleave(string result, params string[] args)
    {
      Assert.Equal(result, args.Interleave(",", "|").ConcatenateStrings());
      Assert.Equal(result.Replace('|', ','), args.Interleave(",").ConcatenateStrings());
    }
    [Fact]
    public void InterleveTests()
    {
      TestInterleave("");
      TestInterleave("A", "A");
      TestInterleave("A|B", "AB".ToCharArray().Select(i => i.ToString()).ToArray());
      TestInterleave("A,B|C", "ABC".ToCharArray().Select(i => i.ToString()).ToArray());
      TestInterleave("A,B,C|D", "ABCD".ToCharArray().Select(i => i.ToString()).ToArray());
    }

    [Theory]
    [InlineData("", new object[0])]
    [InlineData("aa", new[] { "aa" })]
    [InlineData("aabb", new[] { "aa", "bb" })]
    [InlineData("aabbcccc", new[] { "aa", "bb", "cccc" })]

    public void ConcatStrings(string result, object[] items)
    {
      Assert.Equal(result, items.ConcatenateStrings());
    }

    [Fact]
    public void FirstOrDefaultTest()
    {
      Assert.Equal(10, new int[] { }.FirstOrDefault(10));
      Assert.Equal(1, new int[] { 1, 2, 3 }.FirstOrDefault(10));
    }

    [Theory]
    [InlineData(10, 1)]
    [InlineData(10, 3)]
    [InlineData(10, 9)]
    [InlineData(10, 10)]
    [InlineData(10, 11)]
    [InlineData(10, 25)]
    [InlineData(0, 25)]
    public void TestDecolate(int max, int cols)
    {
      Debug.Assert(max >= 0);
      Debug.Assert(cols > 0);
      var lists = EnumerableExtensions.Decolate<int>(Enumerable.Range(0, max), cols);
      for (int i = 0; i < lists.Length; i++)
      {
        Assert.True(lists[i].All(j => i == (j % cols)));
      }
    }

    [Fact]
    public void ConcatItemsTest()
    {
      Assert.Equal("1234", (new int[] { 1, 2 }).Concat(3, 4).ConcatenateStrings());
    }
    [Fact]
    public void PrependTest()
    {
      Assert.Equal("3412", (new int[] { 1, 2 }).Prepend(3, 4).ConcatenateStrings());
    }
    [Fact]
    public void PrependTest2()
    {
      Assert.Equal("3412", (new int[] { 1, 2 }).Prepend(new int[] { 3, 4 }.AsEnumerable()).ConcatenateStrings());
    }
    private void InnerAllBeforeTest(int sentinal, int length, params int[] data)
    {
      var result = data.AllBefore(sentinal).ToArray();
      Assert.Equal(length, result.Length);
      int i = 0;
      Assert.True(result.All(elt => data[i++] == elt));
    }
    [Fact]
    public void AllBeforeTest()
    {
      InnerAllBeforeTest(3, 2, 1, 2, 3, 4, 5, 6);
      InnerAllBeforeTest(1, 0, 1, 2, 3, 4, 5, 6);
      InnerAllBeforeTest(109, 0);
      InnerAllBeforeTest(100, 6, 1, 2, 3, 4, 5, 6);
    }

    [Fact]
    public void TestEnglishList()
    {
      Assert.Equal("", new string[0].EnglishList());
      Assert.Equal("a", new[] { "a" }.EnglishList());
      Assert.Equal("a and b", new[] { "a", "b" }.EnglishList());
      Assert.Equal("a, b, and c", new[] { "a", "b", "c" }.EnglishList());
      Assert.Equal("a, b, c, and d", new[] { "a", "b", "c", "d" }.EnglishList());

    }

    [Fact]
    public void TestForeachExtension()
    {
      int count = 0;
      Enumerable.Range(0, 12).ForEach(i => count++);
      Assert.Equal(12, count);

    }

    [Theory]
    [InlineData(0,"0123456789")]
    [InlineData(100,"0123456789")]
    [InlineData(1,"1234567890")]
    [InlineData(5, "5678901234")]
    public void TestRotateDirect(int rotation, string result)=>
      Assert.Equal(result, Enumerable.Range(0,10).Rotate(rotation).ConcatenateStrings());

    

    [Theory]
    [InlineData(0, 10)]
    [InlineData(0, 0)]
    [InlineData(3, 10)]
    public void TestRotate(int rotation, int elementLength)
    {
      // setup the test
      Assert.True(rotation >= 0);
      Assert.True(rotation <= elementLength);
      var elements = Enumerable.Range(0, elementLength).ToArray();

      var newList = elements.Rotate(rotation).ToArray();

      Assert.Equal(elements.Length, newList.Length);
      for (int i = 0; i < elements.Length; i++)
      {
        Assert.Equal(elements[(i + rotation) % elements.Length], newList[i]);
      }
    }

    [Theory]
    [InlineData(new[] { 1 })]
    [InlineData(new[] { 1, 1, 1 })]
    [InlineData(new[] { 1, 2, 3 })]
    [InlineData(new[] { 3, 2, 1 })]
    [InlineData(new[] { 3, 2, 1, 2, 2, 2, 1 })]
    public void TestMaxMin(int[] data)
    {
      var result = data.MinMax();
      Assert.True(data.All(i => i <= result.Item2 && i >= result.Item1));
    }

  
    [Fact]
    public void SkipOver()
    {
      TestIntSequence(new[] { 1, 2, 3, 4, 5 }.SkipOver(3), 4, 5);
      TestIntSequence(new[] { 1, 2, 3, 4, 5 }.SkipOver(1), 2, 3, 4, 5);
      TestIntSequence(new[] { 1, 2, 3, 4, 5 }.SkipOver(0), 1, 2, 3, 4, 5);
      TestIntSequence(new[] { 1, 2, 3, 4, 5 }.SkipOver(5));
      TestIntSequence(new[] { 1, 2, 3, 4, 5 }.SkipOver(6));
    }

    private void TestIntSequence(IEnumerable<int> a, params int[] b)
    {
      Assert.Equal(a, b);
    }

    [Fact]
    public void IndexOfTest()
    {
      var seq = Enumerable.Range(0, 15).ToArray();
      for (int i = 0; i < seq.Length; i++)
      {
        Assert.Equal(i, seq.IndexOf(i));
      }
    }

    [Fact]
    public void IndexOfNotFoundIsMinus1()
    {
      Assert.Equal(-1, new int[0].IndexOf(5));
    }

    [Fact]
    public void SideEffectTest()
    {
      var sum = 0;
      Assert.Equal(55, Enumerable.Range(1, 10).SideEffect(i => sum += i).Sum());
      Assert.Equal(55, sum);

    }

    [Fact]
    public void Cycle()
    {
      Assert.Equal("123123123", Enumerable.Range(1,3).Cycle(9).ConcatenateStrings());
      
    }

  }
}
