using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using Engine.Simulation;
using Ploeh.AutoFixture;

namespace EngineTests.AutoFixtureCustomizations
{
    public class ScalarFieldCustomization : ICustomization
    {
        private readonly Random _rng = new Random();

        public void Customize(IFixture fixture)
        {
            fixture.Register(() => CreateRandomScalarField(fixture));
        }

        private ScalarField<Face> CreateRandomScalarField(IFixture fixture)
        {
            var polyhedron = fixture.Create<IPolyhedron>();
            var randomValues = polyhedron.Faces.Select(i => (-10 + 20 * _rng.NextDouble())).ToArray();
            var field = new ScalarField<Face>(polyhedron.IndexOf, randomValues);

            return field;
        }
    }
}
