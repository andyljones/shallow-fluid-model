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
        public void DistancesTable_OnACube_ShouldCreateSixListsOfFourElements
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(4, 6).ToList();

            // Exercise system
            var distances = FaceIndexedTableFactory.DistancesTable(polyhedron);

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
            var correctDistance = Math.Sqrt(3)*Math.PI/2;
            var expected = Enumerable.Repeat(correctDistance, 24).ToList();

            // Exercise system
            var distances = FaceIndexedTableFactory.DistancesTable(polyhedron);

            var actual = distances.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgeLengthsTable_OnACube_ShouldCreateSixListsOfFourElements
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(4, 6).ToList();

            // Exercise system
            var edgeLengths = FaceIndexedTableFactory.EdgeLengthsTable(polyhedron);

            var actual = edgeLengths.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void EdgeLengthsTable_OnACube_ShouldCalculateTheCorrectLengths
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var correctLength = Math.Sqrt(3)*Trig.InverseCosine(1.0/3.0);
            var expected = Enumerable.Repeat(correctLength, 24).ToList();

            // Exercise system
            var edgeLengths = FaceIndexedTableFactory.EdgeLengthsTable(polyhedron);

            var actual = edgeLengths.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void NeighboursTable_OnACube_ShouldCreateSixListsOfFourElements
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(4, 6).ToList();

            // Exercise system
            var neighbours = FaceIndexedTableFactory.NeighboursTable(polyhedron);

            var actual = neighbours.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void NeighboursTable_OnACube_ShouldHaveEachIndexAppearFourTimesTotal
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(Enumerable.Range(0, 6), 4).SelectMany(list => list).ToList();

            // Exercise system
            var neighbours = FaceIndexedTableFactory.NeighboursTable(polyhedron);

            var actual = neighbours.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void NeighboursTable_OnACube_ShouldBeCommutative
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var neighbourTable = FaceIndexedTableFactory.NeighboursTable(polyhedron);

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
        public void NeighboursTable_OnACube_ShouldListNeighboursInClockwiseOrder
            (IPolyhedron polyhedron)
        {
            // Fixture setup

            // Exercise system
            var neighbourTable = FaceIndexedTableFactory.NeighboursTable(polyhedron);

            // Verify outcome
            for (int i = 0; i < neighbourTable.Length; i++)
            {
                var neighbours = neighbourTable[i];
                var centerOfFace = polyhedron.Faces[i].SphericalCenter();
                for (int j = 0; j < neighbours.Length-1; j++)
                {
                    var centerOfThisNeighbour = polyhedron.Faces[neighbours[j]].SphericalCenter();
                    var centerOfNextNeighbour = polyhedron.Faces[neighbours[j + 1]].SphericalCenter();

                    Assert.True(Vector.CrossProduct(centerOfNextNeighbour - centerOfFace, centerOfThisNeighbour - centerOfFace).ScalarMultiply(centerOfFace) >= 0);
                }
                var centerOfLastNeighbour = polyhedron.Faces[neighbours[neighbours.Length - 1]].SphericalCenter();
                var centerOfFirstNeighbour = polyhedron.Faces[neighbours[0]].SphericalCenter();

                Assert.True(Vector.CrossProduct(centerOfFirstNeighbour - centerOfFace, centerOfLastNeighbour - centerOfFace).ScalarMultiply(centerOfFace) >= 0);

            }

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void AreasTable_OnACube_ShouldCalculateTheCorrectAreas
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var correctArea = 4.0;
            var expected = Enumerable.Repeat(correctArea, 6).ToList();

            // Exercise system
            var actual = FaceIndexedTableFactory.AreasTable(polyhedron);

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void NormalsTable_OnACube_ShouldCalculateTheCorrectNormals
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
            var actual = FaceIndexedTableFactory.NormalsTable(polyhedron);

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoCubeData]
        public void DirectionsTable_OfTheTopFaceOfACube_ShouldCalculateTheCorrectDirections
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
            var actual = FaceIndexedTableFactory.DirectionsTable(polyhedron)[polyhedron.Faces.IndexOf(topFace)];

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }
    }
}
