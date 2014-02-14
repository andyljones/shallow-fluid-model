using System.Collections.Generic;
using Engine.Models;
using Engine.Polyhedra;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Rendering.ParticleMap
{
    public class ParticleMap
    {
        private readonly IParticleMapOptions _options;
        private readonly float _radius;

        private readonly ParticleRenewalScheduler _particleRenewalScheduler;
        private readonly ParticlePositionUpdater _particlePositionUpdater;
        private readonly ParticleMapRenderer _particleRenderer;

        private Vector3[] _particlePositions;

        public ParticleMap(IPolyhedron polyhedron, IParticleMapOptions options)
        {
            _options = options;
            _radius = (float)_options.Radius;

            _particlePositions = CreateParticles(options.ParticleCount, _radius);

            _particleRenewalScheduler = new ParticleRenewalScheduler(options);
            _particlePositionUpdater = new ParticlePositionUpdater(polyhedron, options);
            _particleRenderer = new ParticleMapRenderer(0, options.ParticleCount, options);
        }

        private static Vector3[] CreateParticles(int particleCount, float radius)
        {
            var particlePositions = new Vector3[particleCount];
            for (int i = 0; i < particleCount; i++)
            {
                particlePositions[i] = CreateParticle(radius);
            }

            return particlePositions;
        }

        private static Vector3 CreateParticle(float radius)
        {
            return 1.01f*radius*Random.onUnitSphere;
        }

        public void Update(VectorField<Vertex> velocity)
        {
            var indicesToRenew = _particleRenewalScheduler.IndicesToBeRenewed();
            RenewOldParticles(indicesToRenew);

            _particlePositionUpdater.Update(ref _particlePositions, velocity);
            _particleRenderer.Update(_particlePositions);
        }

        private void RenewOldParticles(IEnumerable<int> indicesToRenew)
        {
            foreach (var i in indicesToRenew )
            {
                var newPosition = CreateParticle(_radius);
                _particleRenderer.Reset(i, newPosition);
                _particlePositions[i] = newPosition;
            }
        }
    }
}
