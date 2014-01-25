using System.Diagnostics;
using System.Linq;
using Engine.Icosasphere;
using Engine.Polyhedra;
using EngineTests.Utilities;
using MathNet.Numerics;
using Xunit;

namespace EngineTests.IcosasphereTests
{
    public class IcosahedronTests
    {
        [Fact]
        public void Vertices_ShouldAllHaveUnitNorm()
        {
            // Fixture setup
            var icosahedron = new Icosahedron();

            // Exercise system
            var faces = icosahedron.Faces;

            // Verify outcome
            var vertices = faces.SelectMany(face => face.Vertices).Distinct();
            var norms = vertices.Select(vertex => vertex.Position.Norm()).ToArray();

            Debug.WriteLine("Norms are " + StringUtilities.CollectionToString(norms));
            Assert.True(norms.All(norm => Number.AlmostEqual(norm, 1.0)));

            // Teardown
        }

        //[Fact]
        //public void Edges_ShouldAllHaveTheSameNorm()
        //{
        //    // Fixture setup
        //    var icosahedron = new Icosahedron();

        //    // Exercise system
        //    var faces = icosahedron.Faces;

        //    // Verify outcome
        //    var edges = faces.SelectMany(face => face.Edges()).ToArray();
        //    var norms = edges.Select(edge => (edge.A.Position - edge.B.Position).Norm()).ToArray();

        //    var expectedNorm = norms.First();

        //    Debug.WriteLine("Norms are " + StringUtilities.CollectionToString(norms));
        //    Assert.True(norms.All(norm => Number.AlmostEqual(norm, expectedNorm)));

        //    // Teardown
        //}
    }
}
