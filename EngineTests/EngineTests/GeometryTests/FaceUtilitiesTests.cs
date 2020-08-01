using System;
using Engine.Geometry;
using MathNet.Numerics.LinearAlgebra;
using Xunit;

namespace EngineTests.GeometryTests
{
    public class FaceUtilitiesTests
    {
        [Fact]
        public void Center_OfASquareFace_ShouldBeCorrect()
        {
            // Fixture setup
            var fakeFace = new Face(new[]
            {
                VertexUtilities.NewVertex(1, 1, 1),
                VertexUtilities.NewVertex(1, -1, 1),
                VertexUtilities.NewVertex(-1, -1, 1),
                VertexUtilities.NewVertex(-1, 1, 1)
            });

            var expected = Vector.Build.DenseOfArray(new[] {0, 0, Math.Sqrt(3)});

            // Exercise system
            var actual = fakeFace.SphericalCenter();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.Equal(expected, actual);

            // Teardown
        }
    }
}
