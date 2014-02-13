using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Polyhedra;
using MathNet.Numerics.LinearAlgebra;
using UnityEngine;

namespace Assets.Rendering.ParticleMap
{
    public class ParticlePositionUpdater
    {
        private readonly ParticleNeighbourhoodTracker _tracker;

        private readonly IParticleMapOptions _options;

        public ParticlePositionUpdater(IPolyhedron polyhedron, IParticleMapOptions options)
        {
            _options = options;

            _tracker = new ParticleNeighbourhoodTracker(polyhedron);
        }

        public void Update(ref Vector3[] particlePositions, VectorField<Vertex> velocityField)
        {
            var newParticlePositions = new Vector3[particlePositions.Count()];
            for (int i = 0; i < newParticlePositions.Count(); i++)
            {
                var position = particlePositions[i];
                var velocity = GetVelocity(velocityField, position);
                var newPosition = CalculateNewPosition(velocity, position);
                newParticlePositions[i] = newPosition;
            }

            particlePositions = newParticlePositions;
        }

        private Vector3 CalculateNewPosition(Vector3 velocity, Vector3 particlePosition)
        {
            var radius = particlePosition.magnitude;
            var normalAtParticle = particlePosition.normalized;
            var velocityAlongNormal = Vector3.Dot(velocity, normalAtParticle) * normalAtParticle;
            var directionOfMovement = (velocity - velocityAlongNormal).normalized;
            var angularSpeed = velocity.magnitude / particlePosition.magnitude;

            var newPosition = Mathf.Cos(angularSpeed) * particlePosition + Mathf.Sin(angularSpeed) * radius * directionOfMovement;

            return newPosition;
        }

        private Vector3 GetVelocity(VectorField<Vertex> velocityField, Vector3 particlePosition)
        {
            var indices = _tracker.GetIndicesOfVerticesNearest(particlePosition);
            var velocity = velocityField[indices.First()];

            return GraphicsUtilities.Vector3(_options.WindmapScaleFactor * _options.Timestep * velocity);
        }
    }
}
