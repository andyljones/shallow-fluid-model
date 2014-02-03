using System.Diagnostics;
using System.Linq;
using Engine.Polyhedra.IcosahedronBased;
using MathNet.Numerics;
using Xunit;

namespace EngineTests.PolyhedraTests.IcosahedronBasedTests
{
    public class IcosahedronTests
    {
        [Fact]
        public void Vertices_ShouldAllHaveUnitNorm()
        {
            // Fixture setup
            var icosahedron = IcosahedronFactory.Build();

            // Exercise system
            var vertices = icosahedron.Vertices;

            // Verify outcome
            var norms = vertices.Select(vertex => vertex.Position.Norm()).ToArray();

            Debug.WriteLine("Norms are " + TestUtilities.CollectionToString(norms));
            Assert.True(norms.All(norm => Number.AlmostEqual(norm, 1.0)));

            // Teardown
        }

        [Fact]
        public void Edges_ShouldAllHaveTheSameNorm()
        {
            // Fixture setup
            var icosahedron = IcosahedronFactory.Build();

            // Exercise system
            var edges = icosahedron.Edges;

            // Verify outcome
            var norms = edges.Select(edge => (edge.A.Position - edge.B.Position).Norm()).ToArray();

            var expectedNorm = norms.First();

            Debug.WriteLine("Norms are " + TestUtilities.CollectionToString(norms));
            Assert.True(norms.All(norm => Number.AlmostEqual(norm, expectedNorm)));

            // Teardown
        }

        [Fact]
        public void NumberOfVertexAndEdgeAndFaces_ShouldSatisfyEulersFormula()
        {
            // Fixture setup
            var icosahedron = IcosahedronFactory.Build();

            // Exercise system
            var v = icosahedron.Vertices.Count;
            var e = icosahedron.Edges.Count;
            var f = icosahedron.Faces.Count;

            // Verify outcome
            Debug.WriteLine("Number of vertices: " + v);
            Debug.WriteLine("Number of edges: " + e);
            Debug.WriteLine("Number of faces: " + f);
            Assert.True(v - e + f == 2);

            // Teardown
        }
    }
}
