using System;
using System.Collections.Generic;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Views.ParticleMap
{
    public class ParticleMapView : IDisposable
    {
        private readonly IParticleMapOptions _options;
        private readonly float _radius;

        private readonly ParticleRenewalScheduler _renewalScheduler;
        private readonly ParticlePositionUpdater _positionUpdater;
        private readonly ParticleRenderingManager _renderingManager;

        private readonly Vector3[] _positions;

        public ParticleMapView(IPolyhedron polyhedron, IParticleMapOptions options)
        {
            _options = options;
            _radius = (float)_options.Radius;

            _positions = CreateParticles(options.ParticleCount, _radius);

            _renewalScheduler = new ParticleRenewalScheduler(options);
            _positionUpdater = new ParticlePositionUpdater(polyhedron, options);
            _renderingManager = new ParticleRenderingManager(options);
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
            return 1.01f * radius * Random.onUnitSphere;
        }

        public void Update(VectorField<Vertex> velocity)
        {
            var indicesToRenew = _renewalScheduler.IndicesToBeRenewed();
            RenewOldParticles(indicesToRenew);

            _positionUpdater.Update(_positions, velocity);
            _renderingManager.Update(_positions);
        }

        private void RenewOldParticles(IEnumerable<int> indicesToRenew)
        {
            foreach (var i in indicesToRenew)
            {
                var newPosition = CreateParticle(_radius);
                _renderingManager.Reset(i, newPosition);
                _positions[i] = newPosition;
            }
        }

        #region Destruction methods
        public void Dispose()
        {
            _renderingManager.Dispose();
        }

        #endregion
    }
}
