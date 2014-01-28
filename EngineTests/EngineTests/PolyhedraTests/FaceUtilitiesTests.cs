using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Polyhedra;
using EngineTests.Utilities;
using MathNet.Numerics.LinearAlgebra;
using Xunit;

namespace EngineTests.PolyhedraTests
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

            var expected = new Vector(new[] {0, 0, Math.Sqrt(3)});

            // Exercise system
            var actual = fakeFace.SphericalCenter();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.Equal(expected, actual);

            // Teardown
        }

        [Fact]
        public void Area_OfASquareFace_ShouldBeCorrect()
        {
            // Fixture setup
            var fakeFace = new Face(new[]
            {
                VertexUtilities.NewVertex(1, 1, 1),
                VertexUtilities.NewVertex(1, -1, 1),
                VertexUtilities.NewVertex(-1, -1, 1),
                VertexUtilities.NewVertex(-1, 1, 1)
            });

            var expected = 4.0;

            // Exercise system
            var actual = fakeFace.Area();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.Equal(expected, actual);

            // Teardown
        }
    }
}
