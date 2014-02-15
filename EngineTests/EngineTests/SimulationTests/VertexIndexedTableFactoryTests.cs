using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Geometry;
using Engine.Simulation;
using Engine.Utilities;
using EngineTests.AutoFixtureCustomizations;
using MathNet.Numerics.LinearAlgebra;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.SimulationTests
{
    public class VertexIndexedTableFactoryTests
    {
        [Theory]
        [AutoCubeData]
        public void Distances_OnACube_ShouldCreateRightNumberAndLengthsOfLists
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(3, 8).ToList();

            // Exercise system
            var distances = VertexIndexedTableFactory.Distances(polyhedron);

            var actual = distances.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void DistancesTable_OnACube_ShouldCalculateTheCorrectDistances
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var correctDistance = Math.Sqrt(3) * Math.Acos(1.0/3.0);
            var expected = Enumerable.Repeat(correctDistance, 24).ToList();

            // Exercise system
            var distances = VertexIndexedTableFactory.Distances(polyhedron);

            var actual = distances.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void BisectorDistances_OnACube_ShouldCreateTheRightNumberAndLengthsOfLists
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(3, 8).ToList();

            // Exercise system
            var edgeLengths = VertexIndexedTableFactory.HalfEdgeLengths(polyhedron);

            var actual = edgeLengths.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void BisectorDistances_OnACube_ShouldCalculateTheCorrectDistancesToBisectors
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var correctLength = 1.0;
            var expected = Enumerable.Repeat(correctLength, 24).ToList();

            // Exercise system
            var edgeLengths = VertexIndexedTableFactory.HalfEdgeLengths(polyhedron);

            var actual = edgeLengths.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Neighbours_OnACube_ShouldCreateTheRightNumberAndLengthsOfLists
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(3, 8).ToList();

            // Exercise system
            var neighbours = VertexIndexedTableFactory.Neighbours(polyhedron);

            var actual = neighbours.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Neighbours_OnACube_ShouldHaveEachIndexAppearTheRightNumberOfTimes
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(Enumerable.Range(0, 8), 3).SelectMany(list => list).ToList();

            // Exercise system
            var neighbours = VertexIndexedTableFactory.Neighbours(polyhedron);

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
            var neighbourTable = VertexIndexedTableFactory.Neighbours(polyhedron);

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
            var neighbourTable = VertexIndexedTableFactory.Neighbours(polyhedron);
            // Verify outcome
            for (int i = 0; i < neighbourTable.Length; i++)
            {
                var vector = polyhedron.Vertices[i].Position;
                var neighbours = neighbourTable[i].Select(index => polyhedron.Vertices[index]);
                var vectors = neighbours.Select(neighbour => neighbour.Position).ToList();
                Assert.True(TestUtilities.AreInAntiClockwiseOrder(vectors, vector, -vector));
            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void AreasTable_OnACube_ShouldCalculateTheCorrectAreas
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var correctArea = 3.0;
            var expected = Enumerable.Repeat(correctArea, 8).ToList();

            // Exercise system
            var actual = VertexIndexedTableFactory.Areas(polyhedron);

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgeNormals_OnTheAllPositiveVertexOfACube_ShouldCalculateTheCorrectNormals
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = new List<Vector>
            {
                VectorUtilities.NewVector(0, -1, 1).Normalize(),
                VectorUtilities.NewVector(1, 0, -1).Normalize(),
                VectorUtilities.NewVector(-1, 1, 0).Normalize(),
            };

            // Exercise system
            var normals = VertexIndexedTableFactory.EdgeNormals(polyhedron);
            
            // Verify outcome
            var vertex = polyhedron.Vertices.First(v => Vector.AlmostEqual(v.Position, VectorUtilities.NewVector(1, 1, 1)));
            var actual = normals[polyhedron.IndexOf(vertex)];
            
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgeNormals_OnACube_ShouldCreateTheRightNumberAndLengthsOfLists
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(3, 8).ToList();

            // Exercise system
            var edgeNormals = VertexIndexedTableFactory.EdgeNormals(polyhedron);

            var actual = edgeNormals.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void Faces_OfEachVertex_ShouldHaveTheSameOrderAsEdgesOfEachVertex
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var faces = VertexIndexedTableFactory.Faces(polyhedron);

            // Verify outcome
            for (int vertex = 0; vertex < polyhedron.Vertices.Count; vertex++)
            {
                var faceList = faces[vertex];
                var edgeList = polyhedron.EdgesOf(polyhedron.Vertices[vertex]);

                var firstFace = polyhedron.Faces[faceList[0]];
                var lastEdge = edgeList[edgeList.Count - 1];
                var firstEdge = edgeList[0];
                Assert.True(polyhedron.EdgesOf(firstFace).Contains(lastEdge));
                Assert.True(polyhedron.EdgesOf(firstFace).Contains(firstEdge));
                
                for (int i = 1; i < edgeList.Count; i++)
                {
                    var thisFace = polyhedron.Faces[faceList[i]];
                    var previousEdge = edgeList[i-1];
                    var thisEdge = edgeList[i];

                    Assert.True(polyhedron.EdgesOf(thisFace).Contains(previousEdge));
                    Assert.True(polyhedron.EdgesOf(thisFace).Contains(thisEdge));
                }
            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void AreasInEachFace_OnACube_ShouldAllBeCorrect
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(1.0, 24).ToList();

            // Exercise system
            var areas = VertexIndexedTableFactory.AreaInEachFace(polyhedron);

            // Verify outcome
            var actual = areas.SelectMany(listOfAreas => listOfAreas).ToList();

            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }
    }
}
