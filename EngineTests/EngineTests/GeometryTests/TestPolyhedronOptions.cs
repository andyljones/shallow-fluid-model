using Engine.Geometry;

namespace EngineTests.GeometryTests
{
    public class TestPolyhedronOptions : IPolyhedronOptions
    {
        public double Radius { get; set; }
        public int MinimumNumberOfFaces { get; set; }
    }
}
