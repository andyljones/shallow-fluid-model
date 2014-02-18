using System;

namespace Assets.Views.Level.ParticleMap
{
    public interface IParticleMapOptions
    {
        double Radius { get; }
        double Timestep { get; }
        double ParticleSpeedScaleFactor { get; }
        int ParticleCount { get; }
        int ParticleLifespan { get; }
        int ParticleTrailLifespan { get; }

        String ParticleMaterialName { get; }
    }
}
