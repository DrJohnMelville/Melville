using System;
using Melville.MVVM.Wpf.EventBindings;
using Xunit;

namespace Melville.MVVM.WPF.Test.EventBindings
{
    public sealed class CompositeListExpanderTest
    {
        [Fact]
        public void NoSubstitutions()
        {
            var sut = new CompositeListExpander(Array.Empty<IListExpander>());
            Assert.Equal(new object[]{1,2,3}, sut.Expand(new object[]{1,2,3}));
        }

        class Doubler : IListExpander
        {
            public void Push(object value, Action<object> target)
            {
                target(value);
                if (value is int i) target(i * 2);
            }
        }

        [Fact] 
        public void SingleSubstitution()
        {
            var sut = new CompositeListExpander(new[]
            {
                new ListExpander<int>((value,target)=>target(value*2))
            });
            Assert.Equal(new object[]{2,1,4,6,3}, sut.Expand(new object[]{1,2,3}));
        }
        [Fact] 
        public void DoubleSubstitution()
        {
            var sut = new CompositeListExpander(new[]
            {
                new ListExpander<int>((value,target)=>target(value*2)),
                new ListExpander<int>((value,target)=>target(value*3))
            });
            Assert.Equal(new object[]{6,2,3,1,12,4,18,9}, sut.Expand(new object[]{1,2,3}));
        }
    }
}