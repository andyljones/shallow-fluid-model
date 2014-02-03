using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Engine.Polyhedra;
using EngineTests.AutoFixtureCustomizations;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.PolyhedraTests
{
    public class PolyhedronTests
    {
        [Theory]
        [AutoCubeData]
        public void Vertices_ShouldBeThoseGivenToTheConstructor
            (IPolyhedron polyhedron, List<List<Vertex>> vertexLists)
        {
            // Fixture setup
            var expected = vertexLists.SelectMany(list => list).Distinct().ToList();
            
            // Exercise system
            var actual = polyhedron.Vertices.ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Faces_ShouldBeThoseGivenToTheConstructor
            (IPolyhedron polyhedron, List<List<Vertex>> vertexLists)
        {
            // Fixture setup

            // Exercise system
            var faces = polyhedron.Faces;

            // Verify outcome
            foreach (var vertexList in vertexLists)
            {
                Assert.True(faces.Any(face => TestUtilities.UnorderedEquals(face.Vertices, vertexList)));
            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Faces_ShouldHaveVerticesInAnticlockwiseOrder
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var faces = polyhedron.Faces;

            // Verify outcome
            foreach (var face in faces)
            {
                var center = face.SphericalCenter();
                var viewDirection = -face.SphericalCenter();
                var vectors = face.Vertices.Select(vertex => vertex.Position).ToList();
                Assert.True(TestUtilities.AreInAntiClockwiseOrder(vectors, center, viewDirection));
            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgesOf_OverAllVertices_ReturnsAllEdges
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = polyhedron.Edges.ToList();

            // Exercise system
            var actual = polyhedron.Vertices.SelectMany(polyhedron.EdgesOf).Distinct().ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgesOf_OverEachVertex_ReturnsEdgesInAnticlockwiseOrder
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var facesAndEdges = polyhedron.Vertices.ToDictionary(vertex => vertex, vertex => polyhedron.EdgesOf(vertex));

            // Verify outcome
            foreach (var vertexAndEdges in facesAndEdges)
            {
                var vertex = vertexAndEdges.Key;
                var center = vertex.Position;
                var viewDirection = -center;

                var edges = vertexAndEdges.Value;
                var vectors = edges.Select(edge => edge.SphericalCenter()).ToList();
                Assert.True(TestUtilities.AreInAntiClockwiseOrder(vectors, center, viewDirection));
            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgesOf_OverAllFaces_ReturnsAllEdges
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = polyhedron.Edges.ToList();

            // Exercise system
            var actual = polyhedron.Faces.SelectMany(polyhedron.EdgesOf).Distinct().ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgesOf_OverEachFace_ReturnsEdgesInAnticlockwiseOrder
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var facesAndEdges = polyhedron.Faces.ToDictionary(face => face, face => polyhedron.EdgesOf(face));
            
            // Verify outcome
            foreach (var faceAndEdges in facesAndEdges)
            {
                var face = faceAndEdges.Key;
                var center = face.SphericalCenter();
                var viewDirection = -center;

                var edges = faceAndEdges.Value;
                var vectors = edges.Select(edge => edge.SphericalCenter()).ToList();
                Assert.True(TestUtilities.AreInAntiClockwiseOrder(vectors, center, viewDirection));    
            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void FacesOf_OverAllVertices_ReturnsAllFaces
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = polyhedron.Faces.ToList();

            // Exercise system
            var actual = polyhedron.Vertices.SelectMany(polyhedron.FacesOf).Distinct().ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void FacesOf_OverAllEdges_ReturnsAllFaces
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = polyhedron.Faces.ToList();

            // Exercise system
            var actual = polyhedron.Edges.SelectMany(polyhedron.FacesOf).Distinct().ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void NumberOfVertexAndEdgeAndFaces_ShouldSatisfyEulersFormula
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            
            // Exercise system
            var v = polyhedron.Vertices.Count;
            var e = polyhedron.Edges.Count;
            var f = polyhedron.Faces.Count;

            // Verify outcome
            Debug.WriteLine("Number of vertices: " + v);
            Debug.WriteLine("Number of edges: " + e);
            Debug.WriteLine("Number of faces: " + f);
            Assert.True(v - e + f == 2);

            // Teardown
        }
    }
}
