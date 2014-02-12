using Assets.Rendering;
using Assets.Rendering.WindMapper;
using Engine.Models.MomentumModel;
using Engine.Polyhedra.IcosahedronBased;

namespace Assets
{
    public class Options : IPolyhedronOptions, IPolyhedronRendererOptions, IMomentumModelParameters, IWindMapOptions
    {
        public double Radius { get; set; }
        public int MinimumNumberOfFaces { get; set; }

        public string SurfaceMaterialName { get; set; }
        public string WireframeMaterialName { get; set; }

        public double RotationFrequency { get; set; }
        public double Gravity { get; set; }
        public double Timestep { get; set; }

        public double WindmapScaleFactor { get; set; }
        public int ParticleCount { get; set; }
        public double RenewalRate { get; set; }
        public string WindMapMaterialName { get; set; }
        public int TrailLength { get; set; }
    }
}
