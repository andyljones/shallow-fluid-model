using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Engine.Utilities;

namespace EngineTests
{
    using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;

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

            var missingFromActual = expectedList.Where(x => expectedList.Count(y => Precision.AlmostEqual(x, y, relativeAccuracy)) != actualList.Count(y => Precision.AlmostEqual(x, y, relativeAccuracy)));
            var missingFromExpected = actualList.Where(x => expectedList.Count(y => Precision.AlmostEqual(x, y, relativeAccuracy)) != actualList.Count(y => Precision.AlmostEqual(x, y, relativeAccuracy)));

            return !(missingFromActual.Any() || missingFromExpected.Any());
        }

        public static bool UnorderedEquals(IEnumerable<Vector> expected, IEnumerable<Vector> actual, double relativeAccuracy)
        {
            var expectedList = expected.ToList();
            var actualList = actual.ToList();

            var missingFromActual = expectedList.Where(x => expectedList.Count(y => Precision.AlmostEqual(x, y, relativeAccuracy)) != actualList.Count(y => Precision.AlmostEqual(x, y, relativeAccuracy)));
            var missingFromExpected = actualList.Where(x => expectedList.Count(y => Precision.AlmostEqual(x, y, relativeAccuracy)) != actualList.Count(y => Precision.AlmostEqual(x, y, relativeAccuracy)));

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

        public static bool AreInAntiClockwiseOrder(List<Vector> vectors, Vector center, Vector viewDirection)
        {
            var areInOrder = true;
            for (int i = 0; i < vectors.Count() - 1; i++)
            {
                var thisVector = vectors[i];
                var nextVector = vectors[i + 1];
                var crossProduct = VectorUtilities.CrossProduct(thisVector - center, nextVector - center);
                var componentAlongView = VectorUtilities.ScalarProduct(crossProduct, viewDirection);

                if (componentAlongView > 0)
                {
                    areInOrder = false;
                }

            }

            return areInOrder;
        }
    }
}
