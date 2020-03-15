using  System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Melville.TestHelpers.DeepComparisons
{
    public sealed class DeepComparison
    {
        /// <summary>
        /// Test if two objects are the same, following links and enumerating collections along the way
        /// </summary>
        /// <param name="o1">The first object</param>
        /// <param name="o2">The second object</param>
        /// <param name="propertiesToExclude">Ignore properties or fields with the given names</param>
        /// <returns></returns>
        public static bool AreSame(object? o1, object? o2, params string[] propertiesToExclude)
        {
            return new DeepComparison(propertiesToExclude, false).InnerAreSame(o1, o2);
        }


        /// <summary>
        /// Test if two objects are the same, following links and enumerating collections along the way
        /// </summary>
        /// <param name="o1">The first object</param>
        /// <param name="o2">The second object</param>
        /// <param name="assert">If true, the assert any failures, so the failure path is visible</param>
        /// <param name="propertiesToExclude">Ignore properties or fields with the given names</param>
        /// <returns></returns>
        public static bool AreSame(object? o1, object? o2, bool assert, params string[] propertiesToExclude)
        { 
            return new DeepComparison(propertiesToExclude, assert).InnerAreSame(o1, o2);
        }

        private readonly string[] propertiesToExclude;
        private readonly bool assert;

        private readonly Stack<object> considered = new Stack<object>();
        private readonly Stack<string> path = new Stack<string>();

        private DeepComparison(string[] propertiesToExclude, bool assert)
        {
            this.propertiesToExclude = propertiesToExclude;
            this.assert = assert;
            path.Push("{Item}");
        }

        private bool InnerAreSame(object? o1, object? o2)
        {
            if (o1 == null || o2 == null)
            {
                return CheckTrue(Equals(o1, o2), o1, o2, "");
            }

            if (considered.Contains(o1)) return true;

            considered.Push(o1);
            if (o1 is Task) return true;

            try
            {
                if (ReferenceEquals(o1, o2))
                {
                    return true; // if the references are the same object then quit early
                }

                var type1 = o1.GetType();

                if (type1 != o2.GetType())
                {
                    return CheckTrue(false, o1, o2, " (Types Differ)");
                }

                var date = o1 as DateTime?;
                if (date.HasValue)
                {
                    return (Math.Abs((date.Value - ((DateTime) o2)).TotalSeconds) < 5.0);
                }

                var comp1 = o1 as IComparable;
                if (comp1 != null)
                {
                    return CheckTrue(comp1.CompareTo(o2) == 0, o1, o2, " (IComparable Differs)");
                }

                var enum1 = o1 as IEnumerable;
                if (enum1 != null && !EnumerableEqual(enum1, (IEnumerable) o2))
                {
                    return CheckTrue(false, enum1, o2, " (Enumerable Differs)");
                }

                if (!FieldsEqual(o1, o2, type1))
                {
                    return false;
                }

                if (!PropertiesEqual(o1, o2, type1))
                {
                    return false;
                }

                return true;
            }
            finally
            {
                considered.Pop();
            }
        }

        private bool CheckTrue(bool test, object? expected, object? actual, string postfix)
        {
            if (assert && !test)
            {
                throw new AssertActualExpectedException(expected, actual, 
                    String.Join("", path.Reverse()) + postfix);
            }

            return test;
        }

        private bool EnumerableEqual(IEnumerable enum1, IEnumerable enum2)
        {
            var enumerator1 = enum1.GetEnumerator();
            var enumerator2 = enum2.GetEnumerator();
            try
            {
                bool savedResult1;
                bool savedResult2;
                int position = 0;
                while ((savedResult1 = enumerator1.MoveNext()) & (savedResult2 = enumerator2.MoveNext()))
                {
                    path.Push($"[{position++}]");
                    if (!InnerAreSame(enumerator1.Current, enumerator2.Current)) return false;
                    path.Pop();
                }

                return CheckTrue(savedResult1 == savedResult2, enum1, enum2, " (Lengths Unequal)");
            }
            finally
            {
                TryDispose(enum1);
                TryDispose(enum2);
            }
        }

        private static void TryDispose(IEnumerable enum1)
        {
            var disp = enum1 as IDisposable;
            if (disp != null)
            {
                disp.Dispose();
            }
        }

        private bool FieldsEqual(object o1, object o2, Type type)
        {
            return type.GetFields()
                .Where(i=>!propertiesToExclude.Contains(i.Name))
                .All(field => DoNamedComparison($".{field.Name}", 
                    ()=>InnerAreSame(field.GetValue(o1), field.GetValue(o2))));
        }

        private bool PropertiesEqual(object o1, object o2, Type type) => 
            type.GetProperties()
            .Where(i => !i.GetIndexParameters().Any() && !propertiesToExclude.Contains(i.Name))
            .All(prop => DoNamedComparison($".{prop.Name}",
                () => InnerAreSame(prop.GetValue(o1, null), prop.GetValue(o2, null))));

        private bool DoNamedComparison(string name, Func<bool> innerOp)
        {
            path.Push(name);
            try
            {
                return innerOp();
            }
            finally
            {
                path.Pop();
            }
        }
    }
}