using System;
using System.Linq;
using Engine.Models;
using Engine.Models.VorticityDivergenceModel;
using Engine.Polyhedra;
using EngineTests.AutoFixtureCustomizations;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.ModelsTests.VorticityDivergenceModelTests
{
    public class FieldIntegratorTests
    {
        //TODO: Write more FieldIntegrator tests.

        [Theory]
        [AutoFieldIntegratorData(20)]
        public void Integrate_ingAFieldWithALargeNumberOfIterations_ShouldReturnAFieldWhoseDerivativeMatchesTheInputField
            (FieldIntegrator integrator, ScalarField<Face> randomField, ScalarFieldOperators operators)
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
