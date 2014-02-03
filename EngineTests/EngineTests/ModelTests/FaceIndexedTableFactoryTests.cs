using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
using Engine.Utilities;
using EngineTests.AutoFixtureCustomizations;
using EngineTests.Utilities;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.ModelTests
{
    public class FaceIndexedTableFactoryTests
    {
        [Theory]
        [AutoCubeData]
        public void Distances_OnACube_ShouldCreateSixListsOfFourElements
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(4, 6).ToList();

            // Exercise system
            var distances = FaceIndexedTableFactory.Distances(polyhedron);

            var actual = distances.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Distances_OnACube_ShouldCalculateTheCorrectDistances
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var correctDistance = Math.Sqrt(3)*Math.PI/2;
            var expected = Enumerable.Repeat(correctDistance, 24).ToList();

            // Exercise system
            var distances = FaceIndexedTableFactory.Distances(polyhedron);

            var actual = distances.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgeLengths_OnACube_ShouldCreateSixListsOfFourElements
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(4, 6).ToList();

            // Exercise system
            var edgeLengths = FaceIndexedTableFactory.EdgeLengths(polyhedron);

            var actual = edgeLengths.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgeLengths_OnACube_ShouldCalculateTheCorrectLengths
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var correctLength = Math.Sqrt(3)*Trig.InverseCosine(1.0/3.0);
            var expected = Enumerable.Repeat(correctLength, 24).ToList();

            // Exercise system
            var edgeLengths = FaceIndexedTableFactory.EdgeLengths(polyhedron);

            var actual = edgeLengths.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Neighbours_OnACube_ShouldCreateSixListsOfFourElements
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(4, 6).ToList();

            // Exercise system
            var neighbours = FaceIndexedTableFactory.Neighbours(polyhedron);

            var actual = neighbours.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Neighbours_OnACube_ShouldHaveEachIndexAppearFourTimesTotal
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(Enumerable.Range(0, 6), 4).SelectMany(list => list).ToList();

            // Exercise system
            var neighbours = FaceIndexedTableFactory.Neighbours(polyhedron);

            var actual = neighbours.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Neighbours_OnACube_ShouldBeCommutative
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var neighbourTable = FaceIndexedTableFactory.Neighbours(polyhedron);

            // Verify outcome
            for (int face = 0; face < neighbourTable.Length; face++)
            {
                foreach (var neighbour in neighbourTable[face])
                {
                    Assert.Contains(face, neighbourTable[neighbour]);
                }
            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Neighbours_OnACube_ShouldListNeighboursInAnticlockwiseOrder
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var neighbourTable = FaceIndexedTableFactory.Neighbours(polyhedron);

            // Verify outcome
            for (int i = 0; i < neighbourTable.Length; i++)
            {
                var centerOfFace = polyhedron.Faces[i].SphericalCenter();
                var viewVector = -centerOfFace.Normalize();

                var neighbours = neighbourTable[i].Select(index => polyhedron.Faces[index]);
                var centersOfNeighbours = neighbours.Select(neighbour => neighbour.SphericalCenter()).ToList();

                Assert.True(TestUtilities.AreInAntiClockwiseOrder(centersOfNeighbours, centerOfFace, viewVector));
            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Areas_OnACube_ShouldCalculateTheCorrectAreas
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var correctArea = 4.0;
            var expected = Enumerable.Repeat(correctArea, 6).ToList();

            // Exercise system
            var actual = FaceIndexedTableFactory.Areas(polyhedron);

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Normals_OnACube_ShouldCalculateTheCorrectNormals
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = new List<Vector>
            {
                VectorUtilities.NewVector(1, 0, 0),
                VectorUtilities.NewVector(-1, 0, 0),
                VectorUtilities.NewVector(0, 1, 0),
                VectorUtilities.NewVector(0, -1, 0),
                VectorUtilities.NewVector(0, 0, 1),
                VectorUtilities.NewVector(0, 0, -1)
            };

            // Exercise system
            var actual = FaceIndexedTableFactory.Normals(polyhedron);

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Directions_OfTheTopFaceOfACube_ShouldCalculateTheCorrectDirections
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = new List<Vector>
            {
                VectorUtilities.NewVector(1, 0, 0),
                VectorUtilities.NewVector(-1, 0, 0),
                VectorUtilities.NewVector(0, 1, 0),
                VectorUtilities.NewVector(0, -1, 0)
            };

            // Exercise system
            var topFace = polyhedron.Faces.First(face => face.Vertices.All(vertex => vertex.Position[2] > 0.5));
            var actual = FaceIndexedTableFactory.Directions(polyhedron)[polyhedron.Faces.IndexOf(topFace)];

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Vertices_ShouldBeCorrectIndices
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var vertexIndices = FaceIndexedTableFactory.Vertices(polyhedron);

            // Verify outcome
            for (int i = 0; i < polyhedron.Faces.Count; i++)
            {
                var face = polyhedron.Faces[i];
                var expected = face.Vertices;

                var actual = vertexIndices[i].Select(j => polyhedron.Vertices[j]).ToList();

                TestUtilities.WriteExpectedAndActual(expected, actual);
                Assert.True(Enumerable.SequenceEqual(expected, actual));
            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void FaceInVertices_ShouldBeCorrectIndices
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var vertexIndices = FaceIndexedTableFactory.FaceInFacesOfVertices(polyhedron);

            // Verify outcome
            for (int i = 0; i < polyhedron.Faces.Count; i++)
            {
                var face = polyhedron.Faces[i];
                var expected = Enumerable.Repeat(face, face.Vertices.Count).ToList();

                var indices = vertexIndices[i];
                var vertices = face.Vertices;
                var actual = vertices.Select((v, j) => polyhedron.FacesOf(v)[indices[j]]).ToList();

                TestUtilities.WriteExpectedAndActual(expected, actual);
                Assert.True(Enumerable.SequenceEqual(expected, actual));
            }

            // Teardown
        }
    }
}
