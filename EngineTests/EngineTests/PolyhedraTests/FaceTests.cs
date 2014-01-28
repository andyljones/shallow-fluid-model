using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Polyhedra;
using Xunit;

namespace EngineTests.PolyhedraTests
{
    public class FaceTests
    {
        [Fact]
        public void Faces_GivenAnAnticlockwiseSetOfVertices_SortsVerticesInEachFaceInClockwiseOrder()
        {
            // Fixture setup
            var antiClockwiseVertices = new[]
            {
                VertexUtilities.NewVertex(0, 0), 
                VertexUtilities.NewVertex(Math.PI/2, 0), 
                VertexUtilities.NewVertex(Math.PI/2, Math.PI/2)
            };

            var clockwiseOrderingA = new[] { antiClockwiseVertices[0], antiClockwiseVertices[2], antiClockwiseVertices[1] };
            var clockwiseOrderingB = new[] { antiClockwiseVertices[2], antiClockwiseVertices[1], antiClockwiseVertices[0] };
            var clockwiseOrderingC = new[] { antiClockwiseVertices[1], antiClockwiseVertices[0], antiClockwiseVertices[2] };

            // Exercise system
            var face = new Face(antiClockwiseVertices);

            // Verify outcome
            Debug.WriteLine("Resulting face was " + face);

            var resultIsClockwiseOrdered =
                face.Vertices.SequenceEqual(clockwiseOrderingA) ||
                face.Vertices.SequenceEqual(clockwiseOrderingB) ||
                face.Vertices.SequenceEqual(clockwiseOrderingC);

            Assert.True(resultIsClockwiseOrdered);

            // Teardown
        }
    }
}
