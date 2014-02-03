using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
using Engine.Simulation2;
using EngineTests.AutoFixtureCustomizations;
using EngineTests.Utilities;
using MathNet.Numerics;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.Simulation2Tests
{
    public class VectorFieldOperatorsTests
    {
        [Theory]
        [AutoFieldsOnCubeData]
        public void FluxDivergence_OverACube_ShouldSumToZero
            (IPolyhedron polyhedron, VectorField<Vertex> V, ScalarField<Face> F)
        {
            // Fixture setup
            var operators = new VectorFieldOperators(polyhedron);

            // Exercise system
            var divergence = operators.FluxDivergence(V, F);

            // Verify outcome
            var expected = 0.0;

            var actual = divergence.Values.Sum();

            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Number.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }
    }
}
