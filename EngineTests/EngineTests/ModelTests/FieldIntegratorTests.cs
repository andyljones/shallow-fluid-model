using System;
using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
using EngineTests.AutoFixtureCustomizations;
using EngineTests.Utilities;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.ModelTests
{
    public class FieldIntegratorTests
    {
        //TODO: Write more FieldIntegrator tests.

        [Theory]
        [AutoFieldIntegratorData(20)]
        public void Integrate_ingAFieldWithALargeNumberOfIterations_ShouldReturnAFieldWhoseDerivativeMatchesTheInputField
            (FieldIntegrator integrator, ScalarField<Face> randomField, FieldOperators operators)
        {
            // Fixture setup
            var zero = new ScalarField<Face>(randomField.IndexOf, Enumerable.Repeat(0.0, randomField.Count).ToArray());

            // Exercise system
            var integral = integrator.Integrate(zero, randomField);
            var derivative = operators.Laplacian(integral);

            var integralOfDerivative = integrator.Integrate(zero, derivative);
            var derivativeOfIntegralOfDerivative = operators.Laplacian(integralOfDerivative);

            // Verify outcome
            var error = derivativeOfIntegralOfDerivative - derivative;
            var expectedTotalError = derivative.Values.Select(Math.Abs).Sum() * TestUtilities.RelativeAccuracy;
            var actualTotalError = error.Values.Select(Math.Abs).Sum();

            TestUtilities.WriteExpectedAndActual(expectedTotalError, actualTotalError);
            Assert.True(actualTotalError < expectedTotalError);

            // Teardown
        }
    }
}
