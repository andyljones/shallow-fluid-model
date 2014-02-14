using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Rendering.ParticleMap
{
    public class ParticleRenderingManager
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

        public ParticleRenderingManager(IParticleMapOptions options)
        {
            _numberOfParticles = options.ParticleCount;

            _numberOfLinesPerParticle = options.ParticleTrailLifespan - 1;
            _numberOfLines = _numberOfParticles * _numberOfLinesPerParticle;
            var numberOfVertices = 2*_numberOfLines;
            
            _numberOfRenderers = Mathf.CeilToInt((float)numberOfVertices / (float)MaxNumberOfVerticesPerRenderer);
            _particlesPerRenderer = options.ParticleCount / _numberOfRenderers;

            _indicesOfFirstParticles = Enumerable.Range(0, _numberOfRenderers).Select(i => _particlesPerRenderer * i).ToList();
            _indicesOfOnePastLastParticles = Enumerable.Range(0, _numberOfRenderers).Select(i => Mathf.Min(_particlesPerRenderer * (i + 1), _numberOfParticles)).ToList();
            _renderers = InitializeParticleRenderers(options);
        }

        private List<ParticleMapRenderer> InitializeParticleRenderers(IParticleMapOptions options)
        {
            var parentObject = new GameObject("Particle Maps");

            var renderers = new List<ParticleMapRenderer>(_numberOfRenderers);
            for (int i = 0; i < _numberOfRenderers; i++)
            {
                var indexOfFirstVertex = _indicesOfFirstParticles[i];
                var indexOfOnePastLastVertex = _indicesOfOnePastLastParticles[i];
                var newRenderer = new ParticleMapRenderer(parentObject.transform, indexOfFirstVertex, indexOfOnePastLastVertex, options);
                renderers.Add(newRenderer);
            }

            return renderers;
        }

        public void Update(Vector3[] particlePositions)
        {
            foreach (var renderer in _renderers)
            {
                renderer.Update(particlePositions);
            }
        }

        public void Reset(int particleIndex, Vector3 newPosition)
        {
            var indexOfRendererResponsible = particleIndex/_particlesPerRenderer;
            _renderers[indexOfRendererResponsible].Reset(particleIndex, newPosition);
        }

    }
}
