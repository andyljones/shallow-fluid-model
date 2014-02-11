using Assets.Rendering;
using Engine.Models.MomentumModel;
using Engine.Polyhedra.IcosahedronBased;

namespace Assets
{
    public class Options : IPolyhedronOptions, IPolyhedronRendererOptions, IMomentumModelParameters
    {
        public double Radius { get; set; }
        public int MinimumNumberOfFaces { get; set; }

        public string SurfaceMaterialName { get; set; }
        public string WireframeMaterialName { get; set; }

        public double RotationFrequency { get; set; }
        public double Gravity { get; set; }
        public double Timestep { get; set; }
    }
}
