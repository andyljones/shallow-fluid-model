using System.Diagnostics;
using System.Linq;
using Engine.Polyhedra;
using Engine.Polyhedra.IcosahedronBased;
using EngineTests.Utilities;
using MathNet.Numerics;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace EngineTests.PolyhedraTests.IcosahedronBasedTests
{
    public class GeodesicTests
    {
        [Theory]
        [AutoPolyhedronOptionsData]
        public void Vertices_ShouldHaveTheSameLengthsAsTheRadius
            (IPolyhedronOptions options)
        {
            // Fixture setup
            var polyhedron = GeodesicSphereFactory.Build(options);

            // Exercise system
            var vertices = polyhedron.Vertices;

            // Verify outcome
            var lengths = vertices.Select(vertex => vertex.Position.Norm());

            Debug.WriteLine("Lengths were " + StringUtilities.CollectionToString(lengths));
            Assert.True(lengths.All(length => Number.AlmostEqual(length, options.Radius)));

            // Teardown
        }
    }
}
