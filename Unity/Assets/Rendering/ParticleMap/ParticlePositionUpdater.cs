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
        private readonly Vector3[] _vertexPositions;

        private readonly double _scaleFactor;

        public ParticlePositionUpdater(IPolyhedron polyhedron, IParticleMapOptions options)
        {
            _options = options;
            _scaleFactor = options.WindmapScaleFactor*options.Timestep;

            _tracker = new ParticleNeighbourhoodTracker(polyhedron, options.ParticleCount);
            _vertexPositions = GetVertexPositions(polyhedron);
        }

        private static Vector3[] GetVertexPositions(IPolyhedron surface)
        {
            return surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position)).ToArray();
        }

        public void Update(ref Vector3[] particlePositions, VectorField<Vertex> velocityField)
        {
            var indicesOfNearestVertices = _tracker.GetIndicesOfVerticesNearest(particlePositions);

            for (int i = 0; i < particlePositions.Length; i++)
            {
                var position = particlePositions[i];
                var velocity = GetVelocity(velocityField, indicesOfNearestVertices[i]);
                var newPosition = CalculateNewPosition(velocity, position);
                particlePositions[i] = newPosition;
            }

        }

        private Vector3 CalculateNewPosition(Vector3 velocity, Vector3 particlePosition)
        {
            var radius = particlePosition.magnitude;

            var newPosition = radius*(particlePosition + velocity).normalized;

            return newPosition;
        }

        private Vector3 GetVelocity(VectorField<Vertex> velocityField, int nearestVertex)
        {
            var v = velocityField[nearestVertex];

            return GraphicsUtilities.Vector3(_scaleFactor * v);
        }
    }
}
