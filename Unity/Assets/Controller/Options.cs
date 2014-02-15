using Assets.Views.ParticleMap;
using Assets.Views.Surface;
using Engine.GeodesicSphere;
using Engine.Simulation;

namespace Assets.Controller
{
    public class Options : IPolyhedronOptions, IPolyhedronRendererOptions, IMomentumModelParameters, IParticleMapOptions
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
        public int ParticleLifespan { get; set; }
        public string ParticleMaterialName { get; set; }
        public int ParticleTrailLifespan { get; set; }
    }
}
