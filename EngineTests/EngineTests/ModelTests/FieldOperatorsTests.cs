using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
using EngineTests.AutoFixtureCustomizations;
using EngineTests.Utilities;
using MathNet.Numerics;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.ModelTests
{
    public class FieldOperatorsTests
    {
        //TODO: Write more FieldOperators tests.

        [Theory]
        [AutoScalarFieldOnCubeData]
        public void Jacobian_OfAnyTwoFieldsOnACube_ShouldSumToZero
            (FieldOperators operators, ScalarField<Face> A, ScalarField<Face> B)
        {
            // Fixture setup
            var expected = 0.0;

            // Exercise system
            var jacobian = operators.Jacobian(A, B);

            var actual = jacobian.Values.Sum();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Number.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoScalarFieldOnCubeData]
        public void FluxDivergence_OfAnyTwoFieldsOnACube_ShouldSumToZero
            (FieldOperators operators, ScalarField<Face> A, ScalarField<Face> B)
        {
            // Fixture setup
            var expected = 0.0;

            // Exercise system
            var fluxDivergence = operators.FluxDivergence(A, B);

            var actual = fluxDivergence.Values.Sum();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Number.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }

        [Theory]
        [AutoScalarFieldOnCubeData]
        public void Laplacian_OfAnyTwoFieldsOnACube_ShouldSumToZero
            (FieldOperators operators, ScalarField<Face> A)
        {
            // Fixture setup
            var expected = 0.0;

            // Exercise system
            var laplacian = operators.Laplacian(A);

            var actual = laplacian.Values.Sum();

            // Verify outcome
            TestUtilities.WriteExpectedAndActual(expected, actual);
            Assert.True(Number.AlmostEqual(expected, actual, TestUtilities.RelativeAccuracy));

            // Teardown
        }
    }
}
