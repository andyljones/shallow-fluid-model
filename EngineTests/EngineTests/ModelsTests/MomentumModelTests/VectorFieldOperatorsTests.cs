using System.Diagnostics;
using System.Linq;
using Engine.Models;
using Engine.Models.MomentumModel;
using Engine.Polyhedra;
using EngineTests.AutoFixtureCustomizations;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
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

        [Theory]
        [AutoFieldsOnCubeData]
        public void Curl_OverACube_ShouldSumToZero
            (IPolyhedron polyhedron, VectorField<Vertex> V)
        {
            // Fixture setup
            var operators = new VectorFieldOperators(polyhedron);

            // Exercise system
            var divergence = operators.Curl(V);

            // Verify outcome
            var expected = Vector.Zeros(3);

            var actual = divergence.Values.Aggregate(Vector.Zeros(3), (c, v) => c + v);

            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Vector.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoFieldsOnCubeData]
        public void KineticEnergy_OverACube_ShouldSumToTheCorrectValue
            (IPolyhedron polyhedron, VectorField<Vertex> V)
        {
            // Fixture setup
            var operators = new VectorFieldOperators(polyhedron);

            // Exercise system
            var energy = operators.KineticEnergy(V);

            // Verify outcome
            var expected = 3.0 / 8.0 * V.Values.Select(v => v.Norm()*v.Norm()).Sum();

            var actual = energy.Values.Sum();

            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Number.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }
    }
}
