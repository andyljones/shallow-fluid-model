using System;
using Engine.Utilities;
using EngineTests.Utilities;
using MathNet.Numerics.LinearAlgebra;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;
using MathNet.Numerics;

namespace EngineTests.UtilitiesTests
{
    public class VectorUtilitiesTests
    {
        [Theory]
        [AutoData]
        public void GeodesicDistance_OfAVectorFromTheNorthPole_ShouldEqualItsColatitude
            (double colatitude, double azimuth)
        {
            // Fixture setup
            var normalizedColatitude = (colatitude%Math.PI + Math.PI)%Math.PI;
            var vector = VectorUtilities.NewVector(normalizedColatitude, azimuth);
            var northPole = VectorUtilities.NewVector(0, 0);

            var expected = normalizedColatitude;

            // Exercise system
            var actual = VectorUtilities.GeodesicDistance(northPole, vector);

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Number.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoData]
        public void LocalDirection_AnyVectorFromTheNorthPole_ShouldBeTheSameAsThatVectorProjectedOntoEquator
            (double colatitude, double azimuth)
        {
            // Fixture setup
            var vector = VectorUtilities.NewVector(colatitude, azimuth);
            var northPole = VectorUtilities.NewVector(0, 0);

            var expected = vector;
            expected[2] = 0;
            expected = expected.Normalize();

            // Exercise system
            var actual = VectorUtilities.LocalDirection(northPole, vector);
            
            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Vector.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }
    }
}
