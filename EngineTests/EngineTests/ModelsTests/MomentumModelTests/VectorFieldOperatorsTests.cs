using System.Linq;
using Engine.Models;
using Engine.Models.MomentumModel;
using Engine.Polyhedra;
using EngineTests.AutoFixtureCustomizations;
using MathNet.Numerics;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.ModelsTests.MomentumModelTests
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
