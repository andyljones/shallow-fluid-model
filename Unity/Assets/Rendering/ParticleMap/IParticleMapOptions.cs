using System;

namespace Assets.Rendering.ParticleMap
{
    public interface IParticleMapOptions
    {
        double Radius { get; }
        double Timestep { get; }
        double WindmapScaleFactor { get; }
        int ParticleCount { get; }
        int ParticleLifespan { get; }
        int ParticleTrailLifespan { get; }

        String ParticleMaterialName { get; }
    }
}
