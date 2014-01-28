using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Engine.Polyhedra;
using EngineTests.AutoFixtureCustomizations;
using EngineTests.Utilities;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.PolyhedraTests
{
    public class PolyhedronTests
    {
        [Theory]
        [AutoPolyhedronData]
        public void Vertices_ShouldBeThoseGivenToTheConstructor
            (IPolyhedron polyhedron, IEnumerable<IEnumerable<Vertex>> vertexLists)
        {
            // Fixture setup
            var expected = vertexLists.SelectMany(list => list).ToList();
            
            // Exercise system
            var actual = polyhedron.Vertices.ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoPolyhedronData]
        public void Faces_ShouldBeThoseGivenToTheConstructor
            (IPolyhedron polyhedron, IEnumerable<IEnumerable<Vertex>> vertexLists)
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
        [AutoPolyhedronData]
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
        [AutoPolyhedronData]
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
        [AutoPolyhedronData]
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
        [AutoPolyhedronData]
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
        [AutoPolyhedronData]
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
