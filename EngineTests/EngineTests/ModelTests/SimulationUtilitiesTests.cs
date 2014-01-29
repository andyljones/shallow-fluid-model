using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
using EngineTests.AutoFixtureCustomizations;
using EngineTests.Utilities;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.ModelTests
{
    public class SimulationUtilitiesTests
    {
        [Theory]
        [AutoFaceIndexedCubeData]
        public void BuildDistancesTable_OnACube_ShouldCreateSixListsOfFourElements
            (IPolyhedron polyhedron, Dictionary<Face, int> index)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(4, 6).ToList();

            // Exercise system
            var distances = SimulationUtilities.BuildDistancesTable(polyhedron, index);

            var actual = distances.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoFaceIndexedCubeData]
        public void BuildDistancesTable_OnACube_ShouldCalculateTheCorrectDistances
            (IPolyhedron polyhedron, Dictionary<Face, int> index)
        {
            // Fixture setup
            var correctDistance = Math.Sqrt(3)*Math.PI/2;
            var expected = Enumerable.Repeat(correctDistance, 24).ToList();

            // Exercise system
            var distances = SimulationUtilities.BuildDistancesTable(polyhedron, index);

            var actual = distances.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoFaceIndexedCubeData]
        public void BuildEdgeLengthsTable_OnACube_ShouldCreateSixListsOfFourElements
            (IPolyhedron polyhedron, Dictionary<Face, int> index)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(4, 6).ToList();

            // Exercise system
            var edgeLengths = SimulationUtilities.BuildEdgeLengthsTable(polyhedron, index);

            var actual = edgeLengths.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoFaceIndexedCubeData]
        public void BuildEdgeLengthsTable_OnACube_ShouldCalculateTheCorrectLengths
            (IPolyhedron polyhedron, Dictionary<Face, int> index)
        {
            // Fixture setup
            var correctLength = Math.Sqrt(3)*Trig.InverseCosine(1.0/3.0);
            var expected = Enumerable.Repeat(correctLength, 24).ToList();

            // Exercise system
            var edgeLengths = SimulationUtilities.BuildEdgeLengthsTable(polyhedron, index);

            var actual = edgeLengths.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoFaceIndexedCubeData]
        public void BuildNeighboursTable_OnACube_ShouldCreateSixListsOfFourElements
            (IPolyhedron polyhedron, Dictionary<Face, int> index)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(4, 6).ToList();

            // Exercise system
            var neighbours = SimulationUtilities.BuildNeighboursTable(polyhedron, index);

            var actual = neighbours.Select(list => list.Count()).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoFaceIndexedCubeData]
        public void BuildNeighboursTable_OnACube_ShouldHaveEachIndexAppearFourTimesTotal
            (IPolyhedron polyhedron, Dictionary<Face, int> index)
        {
            // Fixture setup
            var expected = Enumerable.Repeat(Enumerable.Range(0, 6), 4).SelectMany(list => list).ToList();

            // Exercise system
            var neighbours = SimulationUtilities.BuildNeighboursTable(polyhedron, index);

            var actual = neighbours.SelectMany(list => list).ToList();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual));

            // Teardown
        }

        [Theory]
        [AutoFaceIndexedCubeData]
        public void BuildNeighboursTable_OnACube_ShouldBeCommutative
            (IPolyhedron polyhedron, Dictionary<Face, int> index)
        {
            // Fixture setup

            // Exercise system
            var neighbourTable = SimulationUtilities.BuildNeighboursTable(polyhedron, index);

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
        [AutoFaceIndexedCubeData]
        public void BuildNeighboursTable_OnACube_ShouldListNeighboursInClockwiseOrder
            (IPolyhedron polyhedron, Dictionary<Face, int> index)
        {
            // Fixture setup

            // Exercise system
            var neighbourTable = SimulationUtilities.BuildNeighboursTable(polyhedron, index);

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
        [AutoFaceIndexedCubeData]
        public void BuildAreasTable_OnACube_ShouldCalculateTheCorrectAreas
            (IPolyhedron polyhedron, Dictionary<Face, int> index)
        {
            // Fixture setup
            var correctArea = 4.0;
            var expected = Enumerable.Repeat(correctArea, 6).ToList();

            // Exercise system
            var actual = SimulationUtilities.BuildAreasTable(polyhedron, index);

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(TestUtilities.UnorderedEquals(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }
    }
}
