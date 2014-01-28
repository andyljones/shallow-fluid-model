using System;
using Engine.Polyhedra;
using Engine.Utilities;
using EngineTests.Utilities;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.UtilitiesTests
{
    public class VectorUtilitiesTests
    {
        private const int Precision = 6;

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
            Assert.Equal(expected, actual, Precision);

            // Teardown
        }
    }
}
