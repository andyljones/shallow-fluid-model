using System.Diagnostics;
using System.Linq;
using Engine;
using Engine.Polyhedra;
using Engine.Polyhedra.IcosahedronBased;
using MathNet.Numerics;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.PolyhedraTests.IcosahedronBasedTests
{
    public class GeodesicTests
    {
        [Theory]
        [RandomPolyhedronOptionsData]
        public void Vertices_ShouldHaveTheSameLengthsAsTheRadius
            (IPolyhedronOptions options)
        {
            // Fixture setup
            var polyhedron = GeodesicSphereFactory.Build(options);

            // Exercise system
            var vertices = polyhedron.Vertices;

            // Verify outcome
            var lengths = vertices.Select(vertex => vertex.Position.Norm()).ToList();

            Debug.WriteLine("Lengths were " + TestUtilities.CollectionToString(lengths));
            Assert.True(lengths.All(length => Number.AlmostEqual(length, options.Radius)));

            // Teardown
        }

        [Theory]
        [RandomPolyhedronOptionsData]
        public void NumbersOfVerticesAndEdgesAndFaces_ShouldSatisfyEulersFormula
            (IPolyhedronOptions options)
        {
            // Fixture setup
            var polyhedron = GeodesicSphereFactory.Build(options);

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

        [Theory]
        [RandomPolyhedronOptionsData]
        public void TwelveFaces_ShouldHaveFiveVertices
            (IPolyhedronOptions options)
        {
            // Fixture setup
            var polyhedron = GeodesicSphereFactory.Build(options);

            // Exercise system
            var numberOfFacesWithFiveVertices = polyhedron.Faces.Count(face => face.Vertices.Count == 5);

            // Verify outcome
            Debug.WriteLine("Number of faces with five vertices is " + numberOfFacesWithFiveVertices);
            Assert.True(numberOfFacesWithFiveVertices == 12);

            // Teardown
        }

        [Theory]
        [RandomPolyhedronOptionsData]
        public void AllButTwelveFaces_ShouldHaveSixVertices
            (IPolyhedronOptions options)
        {
            // Fixture setup
            var polyhedron = GeodesicSphereFactory.Build(options);

            // Exercise system
            var numberOfFacesWithSixVertices = polyhedron.Faces.Count(face => face.Vertices.Count == 6);

            // Verify outcome
            var numberOfFaces = polyhedron.Faces.Count;

            Debug.WriteLine("Number of faces is " + numberOfFaces);            
            Debug.WriteLine("Number of faces with six vertices is " + numberOfFacesWithSixVertices);
            Assert.True(numberOfFacesWithSixVertices == numberOfFaces - 12);

            // Teardown
        }

        [Theory]
        [RandomPolyhedronOptionsData]
        public void EveryVertex_ShouldNeighbourThreeFaces
            (IPolyhedronOptions options)
        {
            // Fixture setup
            var polyhedron = GeodesicSphereFactory.Build(options);

            // Exercise system
            var numberOfVerticesWithThreeFaces = polyhedron.Vertices.Count(vertex => polyhedron.FacesOf(vertex).Count == 3);

            // Verify outcome
            var numberOfVertices = polyhedron.Vertices.Count;

            Debug.WriteLine("Number of vertices is " + numberOfVertices);
            Debug.WriteLine("Number of vertices neighbouring three faces is " + numberOfVerticesWithThreeFaces);
            Assert.True(numberOfVerticesWithThreeFaces == numberOfVertices);

            // Teardown
        }

        [Theory]
        [RandomPolyhedronOptionsData]
        public void EveryEdge_ShouldNeighbourTwoFaces
            (IPolyhedronOptions options)
        {
            // Fixture setup
            var polyhedron = GeodesicSphereFactory.Build(options);

            // Exercise system
            var numberOfEdgesWithThreeFaces = polyhedron.Edges.Count(edge => polyhedron.FacesOf(edge).Count == 2);

            // Verify outcome
            var numberOfEdges = polyhedron.Edges.Count;

            Debug.WriteLine("Number of edges is " + numberOfEdges);
            Debug.WriteLine("Number of edges neighbouring two faces is " + numberOfEdgesWithThreeFaces);
            Assert.True(numberOfEdgesWithThreeFaces == numberOfEdges);

            // Teardown
        }


        [Fact]
        public void NumberOfFaces_If42FacesAreRequested_ShouldBe42()
        {
            // Fixture setup
            var options = new Options {MinimumNumberOfFaces = 42, Radius = 1};
            var polyhedron = GeodesicSphereFactory.Build(options);

            // Exercise system
            var numberOfFaces = polyhedron.Faces.Count;

            // Verify outcome
            Debug.WriteLine("Number of faces is " + numberOfFaces);
            Assert.True(numberOfFaces == 42);

            // Teardown
        }

        [Fact]
        public void NumberOfFaces_If43FacesAreRequested_ShouldBe162()
        {
            // Fixture setup
            var options = new Options { MinimumNumberOfFaces = 43, Radius = 1 };
            var polyhedron = GeodesicSphereFactory.Build(options);

            // Exercise system
            var numberOfFaces = polyhedron.Faces.Count;

            // Verify outcome
            Debug.WriteLine("Number of faces is " + numberOfFaces);
            Assert.True(numberOfFaces == 162);

            // Teardown
        }
    }
}
