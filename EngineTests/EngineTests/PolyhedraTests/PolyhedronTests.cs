using System;
using System.Diagnostics;
using System.Linq;
using Engine.Polyhedra;
using Engine.Utilities;
using Xunit;

namespace EngineTests.PolyhedraTests
{
    public class PolyhedronTests
    {
        [Fact]
        public void Constructor_GivenAnAnticlockwiseSetOfVectors_SortsThemClockwiseInTheCorrespondingFace()
        {
            // Fixture setup
            var antiClockwiseFace = new[]
            {
                VertexUtilities.NewVertex(0, 0), 
                VertexUtilities.NewVertex(Math.PI/2, 0), 
                VertexUtilities.NewVertex(Math.PI/2, Math.PI/2)
            };

            var clockwiseOrderingA = new[] { antiClockwiseFace[0], antiClockwiseFace[2], antiClockwiseFace[1] };
            var clockwiseOrderingB = new[] { antiClockwiseFace[2], antiClockwiseFace[1], antiClockwiseFace[0] };
            var clockwiseOrderingC = new[] { antiClockwiseFace[1], antiClockwiseFace[0], antiClockwiseFace[2] };

            // Exercise system
            var polyhedron = new Polyhedron(new[] {antiClockwiseFace});
            var resultingFace = polyhedron.Faces.First();

            // Verify outcome
            Debug.WriteLine("Resulting face was " + resultingFace);

            var resultIsClockwiseOrdered =
                resultingFace.Vertices.SequenceEqual(clockwiseOrderingA) ||
                resultingFace.Vertices.SequenceEqual(clockwiseOrderingB) ||
                resultingFace.Vertices.SequenceEqual(clockwiseOrderingC);

            Assert.True(resultIsClockwiseOrdered);

            // Teardown
        }

        //TODO: Add a pile more tests.
    }
}
