using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Polyhedra;
using Ploeh.AutoFixture;

namespace EngineTests.AutoFixtureCustomizations
{
    public class ScalarFieldCustomization : ICustomization
    {
        private readonly Random _prng = new Random();

        private const double LowerBound = -10;
        private const double UpperBound = 10;

        public void Customize(IFixture fixture)
        {
            fixture.Inject(CreateRandomScalarField(fixture));
        }

        private ScalarField<Face> CreateRandomScalarField(IFixture fixture)
        {
            var polyhedron = fixture.Create<IPolyhedron>();
            var randomValues = polyhedron.Faces.Select(i => CreateRandomDouble()).ToArray();
            var field = new ScalarField<Face>(polyhedron.IndexOf, randomValues);

            return field;
        }

        private double CreateRandomDouble()
        {
            return LowerBound + (UpperBound - LowerBound) * _prng.NextDouble();
        }
    }
}
