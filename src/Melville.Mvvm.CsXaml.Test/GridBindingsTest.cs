using Melville.Mvvm.CsXaml;
using System;
using System.Linq;
using System.Windows;
using Xunit;

namespace Melville.Mvvm.CsXaml.Test
{
    public class GridBindingsTest
    {
        [Theory]
        [InlineData(", ,  , ")]
        [InlineData("")]
        public void ParseEmpty(string text)
        {
            Assert.Empty(GridDimensionParser.ParseGridDimensions(text));            
        }

        [Fact]
        public void SingleAutoField()
        {
            var ret = GridDimensionParser.ParseGridDimensions("auto").ToList();
            Assert.Single(ret);
            Assert.True(ret.First().IsAuto);
            
        }
        [Theory]
        [InlineData("auto", 1, GridUnitType.Auto)]
        [InlineData("*", 1, GridUnitType.Star)]
        [InlineData("2*", 2, GridUnitType.Star)]
        [InlineData("2.3*", 2.3, GridUnitType.Star)]
        [InlineData("22", 22, GridUnitType.Pixel)]
        public void FieldParsing(string input, double len, GridUnitType type)
        {
            var ret = GridDimensionParser.ParseGridDimensions(input).ToList();
            Assert.Single(ret);
            Assert.Equal(len, ret.First().Value);
            Assert.Equal(type, ret.First().GridUnitType);
        }
        
        [Fact]
        public void TwoItems()
        {
            var ret = GridDimensionParser.ParseGridDimensions("auto, auto").ToList();
            Assert.Equal(2, ret.Count());
            Assert.True(ret.All(i=>i.IsAuto));
        }
    }

    public class GridPositionerTest
    {
        [Fact]
        public void AssignSequential()
        {
            var sut = new GridPositioner(3);
            Assert.Equal((0,0), sut.NextPlaceAsRowCol(1,1));
            Assert.Equal((0,1), sut.NextPlaceAsRowCol(1,1));
            Assert.Equal((0,2), sut.NextPlaceAsRowCol(1,1));
        }
        [Fact]
        public void WrapRow()
        {
            var sut = new GridPositioner(3);
            Assert.Equal((0,0), sut.NextPlaceAsRowCol(1,1));
            Assert.Equal((0,1), sut.NextPlaceAsRowCol(1,1));
            Assert.Equal((0,2), sut.NextPlaceAsRowCol(1,1));
            Assert.Equal((1,0), sut.NextPlaceAsRowCol(1,1));
        }
        [Fact]
        public void AvoidSubsequentRow()
        {
            var sut = new GridPositioner(3);
            Assert.Equal((0,0), sut.NextPlaceAsRowCol(2,1));
            Assert.Equal((0,1), sut.NextPlaceAsRowCol(1,1));
            Assert.Equal((0,2), sut.NextPlaceAsRowCol(1,1));
            Assert.Equal((1,1), sut.NextPlaceAsRowCol(1,1));
        }
        [Fact]
        public void AvoidSubsequentRowTwoCol()
        {
            var sut = new GridPositioner(3);
            Assert.Equal((0,0), sut.NextPlaceAsRowCol(2,2));
            Assert.Equal((0,2), sut.NextPlaceAsRowCol(1,1));
            Assert.Equal((1,2), sut.NextPlaceAsRowCol(1,1));
        }
        [Fact]
        public void AssignWithColSpan()
        {
            var sut = new GridPositioner(3);
            Assert.Equal((0,0), sut.NextPlaceAsRowCol(1,2));
            Assert.Equal((0,2), sut.NextPlaceAsRowCol(1,1));
        }

    }
}