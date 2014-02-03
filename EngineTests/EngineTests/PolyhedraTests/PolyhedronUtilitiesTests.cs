using System;
using Engine.Polyhedra;
using EngineTests.AutoFixtureCustomizations;
using MathNet.Numerics;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.PolyhedraTests
{
    public class PolyhedronUtilitiesTests
    {
        [Theory]
        [AutoCubeData]
        public void BisectionPoint_OfTheEdgeOfACube_ShouldBeEquidistantFromBothEnds
            (IPolyhedron polyhedron)
        {
            // Fixture setup
            var edge = polyhedron.Edges[new Random().Next(0, polyhedron.Edges.Count)];

            // Exercise system
            var point = polyhedron.BisectionPoint(edge);

            // Verify outcome
            var expected = (point - edge.A.Position).Norm();
            var actual = (point - edge.B.Position).Norm();

            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Number.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }
    }
}
