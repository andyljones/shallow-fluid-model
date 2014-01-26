using Engine.Polyhedra;

namespace Engine
{
    public class Options : IPolyhedronOptions
    {
        public double Radius { get; set; }
        public int MinimumNumberOfFaces { get; set; }
    }
}
