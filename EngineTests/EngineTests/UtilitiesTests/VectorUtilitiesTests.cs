﻿using System;
using System.Diagnostics;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;
using AutoFixture.Xunit2;
using Xunit;
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
            Assert.True(Precision.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

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
            expected = expected.Normalize(2);

            // Exercise system
            var actual = VectorUtilities.LocalDirection(northPole, vector);
            
            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Precision.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }
    }
}
