using Engine.Polyhedra;
using Engine.Polyhedra.IcosahedronBased;

namespace Engine
{
    public class Options : IPolyhedronOptions
    {
        public double Radius { get; set; }
        public int MinimumNumberOfFaces { get; set; }
    }
}
