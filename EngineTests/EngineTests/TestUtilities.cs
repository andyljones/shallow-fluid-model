using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Xunit;
using Xunit.Sdk;

namespace EngineTests.Utilities
{
    public static class TestUtilities
    {
        public readonly static double RelativeAccuracy = 1.0/1000000;

        public static string CollectionToString<T>(IEnumerable<T> collection)
        {
            var itemStrings = collection.Select(item => item.ToString()).ToArray();
            var enumerableString = String.Join(", ", itemStrings);

            return enumerableString;
        }

        public static bool UnorderedEquals<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            var expectedList = expected.ToList();
            var actualList = actual.ToList();

            var missingFromActual = expectedList.Where(x => expectedList.Count(y => x.Equals(y)) != actualList.Count(y => x.Equals(y)));
            var missingFromExpected = actualList.Where(x => expectedList.Count(y => x.Equals(y)) != actualList.Count(y => x.Equals(y)));

            return !(missingFromActual.Any() || missingFromExpected.Any());
        }

        public static bool UnorderedEquals(IEnumerable<double> expected, IEnumerable<double> actual, double relativeAccuracy)
        {
            var expectedList = expected.ToList();
            var actualList = actual.ToList();

            var missingFromActual = expectedList.Where(x => expectedList.Count(y => Number.AlmostEqual(x, y, relativeAccuracy)) != actualList.Count(y => Number.AlmostEqual(x, y, relativeAccuracy)));
            var missingFromExpected = actualList.Where(x => expectedList.Count(y => Number.AlmostEqual(x, y, relativeAccuracy)) != actualList.Count(y => Number.AlmostEqual(x, y, relativeAccuracy)));

            return !(missingFromActual.Any() || missingFromExpected.Any());
        }

        public static bool UnorderedEquals(IEnumerable<Vector> expected, IEnumerable<Vector> actual, double relativeAccuracy)
        {
            var expectedList = expected.ToList();
            var actualList = actual.ToList();

            var missingFromActual = expectedList.Where(x => expectedList.Count(y => Vector.AlmostEqual(x, y, relativeAccuracy)) != actualList.Count(y => Vector.AlmostEqual(x, y, relativeAccuracy)));
            var missingFromExpected = actualList.Where(x => expectedList.Count(y => Vector.AlmostEqual(x, y, relativeAccuracy)) != actualList.Count(y => Vector.AlmostEqual(x, y, relativeAccuracy)));

            return !(missingFromActual.Any() || missingFromExpected.Any());
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
