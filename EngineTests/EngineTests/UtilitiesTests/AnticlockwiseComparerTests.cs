using System;
using Engine.Utilities;
using EngineTests.Utilities;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.UtilitiesTests
{
    public class AnticlockwiseComparerTests
    {
        [Theory]
        [AutoData]
        public void Compare_ForAnticlockwiseVectorsAroundAPoint_ShouldReturnMinus1
            (double azimuth0, double azimuth1)
        {
            // Fixture setup
            var center = VectorUtilities.NewVector(1, 0, 0);
            var view = VectorUtilities.NewVector(0, 0, -1);
            var comparer = new AnticlockwiseComparer(center, view);

            var lesserAzimuth = azimuth0%(2*Math.PI);
            var greaterAzimuth = azimuth1%(2*Math.PI-lesserAzimuth) + lesserAzimuth;
            var lessAnticlockwise = VectorUtilities.NewVector(Math.PI/2, lesserAzimuth);
            var moreAnticlockwise = VectorUtilities.NewVector(Math.PI/2, greaterAzimuth);

            var expected = -1;

            // Exercise system
            var actual = comparer.Compare(lessAnticlockwise, moreAnticlockwise);

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.Equal(expected, actual);

            // Teardown
        }

        [Theory]
        [AutoData]
        public void Compare_ForClockwiseVectorsAroundAPoint_ShouldReturn1
            (double azimuth0, double azimuth1)
        {
            // Fixture setup
            var center = VectorUtilities.NewVector(1, 0, 0);
            var view = VectorUtilities.NewVector(0, 0, -1);
            var comparer = new AnticlockwiseComparer(center, view);

            var lesserAzimuth = azimuth0 % (2 * Math.PI);
            var greaterAzimuth = azimuth1 % (2 * Math.PI - lesserAzimuth) + lesserAzimuth;
            var lessAnticlockwise = VectorUtilities.NewVector(Math.PI / 2, lesserAzimuth);
            var moreAnticlockwise = VectorUtilities.NewVector(Math.PI / 2, greaterAzimuth);

            var expected = 1;

            // Exercise system
            var actual = comparer.Compare(moreAnticlockwise, lessAnticlockwise);

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.Equal(expected, actual);

            // Teardown
        }
    }
}
