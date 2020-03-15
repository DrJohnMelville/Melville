using  System;
using System.Collections.Generic;
using Xunit;

namespace Melville.TestHelpers.Assertions
{
    public static class AssertEx
    {
        /// <summary>
        /// Asseqrts that the enumerable parameter contains the remaining parameters.  (Adds params convenience to Assert.Equal(
        /// </summary>
        /// <typeparam name="T">Basis type of the enumerable parameter.</typeparam>
        /// <param name="actual">The enumerable parameters</param>
        /// <param name="expected">The expected elements of the first parameter</param>
        public static void AssertContent<T>(this IEnumerable<T> actual, params T[] expected)
        {
            Assert.Equal(actual, expected);

        }

        /// <summary>
        /// Assert that an action throws a given exception type with a given message
        /// </summary>
        /// <typeparam name="T">The expected exception type.</typeparam>
        /// <param name="message">The message that should be generated</param>
        /// <param name="code">Action for the code under test</param>
        public static void Throws<T>(string message, Action code) where T:Exception
        {
            try
            {
                code();
            }
            catch (T e)
            {
                Assert.Equal(message, e.Message);
                return;
            }
            catch (Exception e1)
            {
                Assert.Equal(typeof(T).Name, e1.GetType().Name);
                return;
            }
            Assert.True(false, "Exception expected, but did not throw.");
        }

    }
}