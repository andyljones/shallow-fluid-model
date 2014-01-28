using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit.Sdk;

namespace EngineTests.Utilities
{
    public static class TestUtilities
    {
        public static string CollectionToString<T>(IEnumerable<T> collection)
        {
            var itemStrings = collection.Select(item => item.ToString()).ToArray();
            var enumerableString = String.Join(", ", itemStrings);

            return enumerableString;
        }

        public static bool SetEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var expectedSet = new HashSet<T>(expected);
            var actualSet = new HashSet<T>(actual);

            return expectedSet.SetEquals(actualSet);
        }

        public static void WriteExpectedAndActual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            Debug.WriteLine("Expected: " + CollectionToString(expected));
            Debug.WriteLine("Actual: " + CollectionToString(actual));
        }

        public static void WriteExpectedAndActual(object expected, object actual)
        {
            Debug.WriteLine("Expected: " + expected);
            Debug.WriteLine("Actual: " + actual);
        }
    }
}
