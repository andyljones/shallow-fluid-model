using System;
using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;
using Ploeh.AutoFixture;

namespace EngineTests.AutoFixtureCustomizations
{
    public class VectorFieldCustomization : ICustomization
    {
        private readonly Random _prng = new Random();

        private const double LowerBound = -10;
        private const double UpperBound = 10;

        public void Customize(IFixture fixture)
        {
            fixture.Inject(CreateRandomVectorField(fixture));
        }

        private VectorField<Vertex> CreateRandomVectorField(IFixture fixture)
        {
            var polyhedron = fixture.Create<IPolyhedron>();
            var values = polyhedron.Vertices.Select(vertex => CreateRandomVector()).ToArray();
            var field = new VectorField<Vertex>(polyhedron.IndexOf, values);

            return field;
        }

        private Vector CreateRandomVector()
        {
            var x = CreateRandomDouble();
            var y = CreateRandomDouble();
            var z = CreateRandomDouble();

            return VectorUtilities.NewVector(x, y, z);
        }

        private double CreateRandomDouble()
        {
            return LowerBound + (UpperBound-LowerBound)*_prng.NextDouble();
        }
    }
}
