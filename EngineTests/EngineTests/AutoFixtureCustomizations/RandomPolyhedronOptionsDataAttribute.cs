using System;
using Engine;
using Engine.Geometry;
using EngineTests.GeometryTests;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;

namespace EngineTests.AutoFixtureCustomizations
{
    public class RandomPolyhedronOptionsDataAttribute : AutoDataAttribute
    {
        private const int MinimumMinNumberOfFaces = 30;
        private const int MaximumMinNumberOfFaces = 100;

        private const double MinimumRadius = 0.5;
        private const double MaximumRadius = 20;

        public RandomPolyhedronOptionsDataAttribute()
        {
            Fixture.Register(CreateRandomPolyhedronOptions);
        }

        private static IPolyhedronOptions CreateRandomPolyhedronOptions()
        {
            var rng = new Random();
            
            var radius = MinimumRadius + (MaximumRadius - MinimumRadius)*rng.NextDouble();
            var minNumberOfFaces = (int)(MinimumMinNumberOfFaces + (MaximumMinNumberOfFaces - MinimumMinNumberOfFaces)*rng.NextDouble());

            return new TestPolyhedronOptions {MinimumNumberOfFaces = minNumberOfFaces, Radius = radius};
        }
    }
}
