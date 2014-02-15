using System.Linq;
using Engine.Geometry;
using Engine.Simulation;
using EngineTests.AutoFixtureCustomizations;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.SimulationTests
{
    public class VectorFieldOperatorsTests
    {
        //[Theory]
        //[AutoFieldsOnCubeData]
        //public void Test
        //    ()
        //{
        //    // Fixture setup
        //    var polyhedron = IcosasphereFactory.Build(new Options {MinimumNumberOfFaces = 1, Radius = 10});
        //    var operators = new VectorFieldOperators(polyhedron);
        //    var factory = new PrognosticFieldsFactory(polyhedron);
        //    // Exercise system
        //    //var gradient = operators.Gradient(factory.RandomScalarField(10, 20));
        //    //Debug.WriteLine(gradient);
        //    var lengths = FaceIndexedTableFactory.Areas(polyhedron);

        //    Debug.WriteLine(String.Join(", ", lengths.Select(l => l)));

        //    // Verify outcome
        //    Assert.True(false, "Test not implemented");

        //    // Teardown
        //}

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

        //[Fact]
        //public void Test()
        //{
        //    // Fixture setup
        //    var options = new Options { MinimumNumberOfFaces = 200, Radius = 6000 };
        //    var polyhedron = GeodesicSphereFactory.Build(options);

        //    // Exercise system
        //    var fieldsFactory = new PrognosticFieldsFactory(polyhedron);

        //    // Verify outcome
        //    Assert.True(false, "Test not implemented");

        //    // Teardown
        //}
    }
}
