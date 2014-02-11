using Assets.Rendering;
using Engine.Polyhedra.IcosahedronBased;

namespace Assets
{
    public class Options : IPolyhedronOptions, IPolyhedronRendererOptions
    {
        public double Radius { get; set; }
        public int MinimumNumberOfFaces { get; set; }

        public string SurfaceMaterialName { get; set; }
        public string WireframeMaterialName { get; set; }
    }
}
