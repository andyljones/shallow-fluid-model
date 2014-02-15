using System.Linq;
using Engine.Geometry;
using EngineTests.AutoFixtureCustomizations;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.GeometryTests
{
    public class PolyhedronUtilitiesTests
    {
        [Theory]
        [AutoCubeData]
        public void BisectionPoint_OfTheEdgeOfACube_ShouldBeEquidistantFromBothEnds
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var points = polyhedron.Edges.Select(edge => PolyhedronUtilities.BisectionPoint(polyhedron, edge)).ToList();

            // Verify outcome
            var expected = polyhedron.Edges.Select((edge, i) => (edge.A.Position - points[i]).Norm()).ToList();
            var actual = polyhedron.Edges.Select((edge, i) => (edge.B.Position - points[i]).Norm()).ToList();

            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Enumerable.SequenceEqual(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void AreaSharedByVertexAndFace_OnACube_ShouldProduceTheCorrectValues
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var vertices = polyhedron.Vertices;
            var faces = polyhedron.Faces;

            var expected = Enumerable.Repeat(1.0, 24).Concat(Enumerable.Repeat(0.0, 24)).ToList();

            // Exercise system
            var actual = vertices.SelectMany(vertex => faces.Select(face => PolyhedronUtilities.AreaSharedByVertexAndFace(polyhedron, vertex, face))).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }
    }
}
