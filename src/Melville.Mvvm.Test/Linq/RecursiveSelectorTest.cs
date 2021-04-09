using System.Collections.Generic;
using System.Runtime.InteropServices;
using Melville.Linq;
using Xunit;

namespace Melville.Mvvm.Test.Linq
{
    public class RecursiveSelectorTest
    {
        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "21")]
        [InlineData(3, "3211")]
        [InlineData(4, "43211211")]
        public void RecursionTest(int start, string output)
        {
            var sourceSequence = new EnumerateSingle<int>(start);
            Assert.Equal(output, OutputSequence(sourceSequence.SelectRecursive(CreateSequence)));
            Assert.Equal(output, 
                OutputSequence(new RecursiveSelector<int>(sourceSequence, CreateSequence)));
        }

        private string OutputSequence(IEnumerable<int> sequence)
        {
            return string.Join("", sequence);
        }

        private IEnumerable<int>? CreateSequence(int arg)
        {
            for (int i =arg -1; i > 0; i--)
            {
                yield return i;
            }
        }
    }
}