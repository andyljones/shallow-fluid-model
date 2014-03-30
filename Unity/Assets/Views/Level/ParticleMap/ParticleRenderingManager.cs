using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Views.Level.ParticleMap
{
    /// <summary>
    /// Renders an array of moving particles. Particles are rendered as a trail with the particle's current
    /// position forming the head, and its past positions forming the tail.
    /// 
    /// In order to dodge a Unity limit on the number of vertices in a mesh, the ParticleRenderingManager partitions 
    /// the set of particles into reasonably-sized chunks and delegates each chunk to a ParticleMapRenderer.
    /// </summary>
    public class ParticleRenderingManager : IDisposable
    {
        private readonly int _numberOfLinesPerParticle;
        private readonly int _numberOfLines;
        private readonly int _numberOfRenderers;
        private readonly int _particlesPerRenderer;

        private readonly List<int> _indicesOfFirstParticles;
        private readonly List<int> _indicesOfOnePastLastParticles; 
        private readonly List<ParticleMapRenderer> _renderers;
        private readonly int _numberOfParticles;

        private const int MaxNumberOfVerticesPerRenderer = 50000;

        private readonly GameObject _gameObject;

        /// <summary>
        /// Construct a ParticleRenderingManager with the specified options.
        /// </summary>
        /// <param name="options"></param>
        public ParticleRenderingManager(IParticleMapOptions options)
        {
            _numberOfParticles = options.ParticleCount;

            // Calculate the number of lines needed, the number of vertices needed, the number of ParticleMapRenderers 
            // needed, and the number of particles that'll be delegated to each ParticleMapRenderer.
            _numberOfLinesPerParticle = options.ParticleTrailLifespan - 1;
            _numberOfLines = _numberOfParticles * _numberOfLinesPerParticle;
            var numberOfVertices = 2*_numberOfLines;
            
            _numberOfRenderers = Mathf.FloorToInt((float)numberOfVertices / MaxNumberOfVerticesPerRenderer) + 1;
            _particlesPerRenderer = _numberOfParticles / _numberOfRenderers;

            // Calculate the partition of the particles into chunks. Each chunk will be assigned to a different ParticleMapRenderer.
            _indicesOfFirstParticles = Enumerable.Range(0, _numberOfRenderers).Select(i => _particlesPerRenderer * i).ToList();
            _indicesOfOnePastLastParticles = Enumerable.Range(0, _numberOfRenderers).Select(i => Mathf.Min(_particlesPerRenderer * (i + 1), _numberOfParticles)).ToList();

            //TODO: Pass in the first & last index arrays, seeing as it depends on them?
            // Create the particle renderers.
            _renderers = InitializeParticleRenderers(options, out _gameObject);
        }

        private List<ParticleMapRenderer> InitializeParticleRenderers(IParticleMapOptions options, out GameObject parentObject)
        {
            parentObject = new GameObject("Particle Maps");

            var renderers = new List<ParticleMapRenderer>(_numberOfRenderers);
            for (int i = 0; i < _numberOfRenderers; i++)
            {
                // Delegate a subarray of particles specified by these indices to a new ParticleMapRenderer.
                var indexOfFirstVertex = _indicesOfFirstParticles[i];
                var indexOfOnePastLastVertex = _indicesOfOnePastLastParticles[i];
                var newRenderer = new ParticleMapRenderer(parentObject.transform, indexOfFirstVertex, indexOfOnePastLastVertex, options);
                renderers.Add(newRenderer);
            }

            return renderers;
        }

        /// <summary>
        /// Servant of Unity's Update(). Updates the positions of the particles using the give position data.
        /// </summary>
        /// <param name="particlePositions"></param>
        public void Update(Vector3[] particlePositions)
        {
            foreach (var renderer in _renderers)
            {
                renderer.Update(particlePositions);
            }
        }

        /// <summary>
        /// Resets the position of a specified particle, interrupting its trail.
        /// </summary>
        /// <param name="particleIndex"></param>
        /// <param name="newPosition"></param>
        public void Reset(int particleIndex, Vector3 newPosition)
        {
            var indexOfRendererResponsible = particleIndex/_particlesPerRenderer;
            _renderers[indexOfRendererResponsible].Reset(particleIndex, newPosition);
        }

        #region Destructor & IDisposable methods
        /// <summary>
        /// Destroy the parent particle map rendering object, and so all its children with it.
        /// </summary>
        public void Dispose()
        {
            Object.Destroy(_gameObject);
        }
        #endregion
    }
}
