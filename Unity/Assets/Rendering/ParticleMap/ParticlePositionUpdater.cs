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

        public ParticlePositionUpdater(IPolyhedron polyhedron, IParticleMapOptions options)
        {
            _options = options;

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

            var newParticlePositions = new Vector3[particlePositions.Count()];
            for (int i = 0; i < newParticlePositions.Count(); i++)
            {
                var position = particlePositions[i];
                var velocity = GetVelocity(velocityField, indicesOfNearestVertices[i]);
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

        private Vector3 GetVelocity(VectorField<Vertex> velocityField, int nearestVertex)
        {
            //var p1 = _vertexPositions[indices[0]];
            //var p2 = _vertexPositions[indices[1]];
            //var p3 = _vertexPositions[indices[2]];

            //var f1 = p1 - f;
            //var f2 = p2 - f;
            //var f3 = p3 - f;

            //var a = Vector3.Cross(p1 - p2, p1 - p3).magnitude;

            //var a1 = Vector3.Cross(f2, f3).magnitude / a;
            //var a2 = Vector3.Cross(f3, f1).magnitude / a;
            //var a3 = Vector3.Cross(f1, f2).magnitude / a;

            var uv1 = velocityField[nearestVertex];
            //var uv2 = velocityField[indices[1]];
            //var uv3 = velocityField[indices[2]];
            //var uv = uv1 * a1 + uv2 * a2 + uv3 * a3;

            return GraphicsUtilities.Vector3(_options.WindmapScaleFactor * _options.Timestep * uv1);
        }
    }
}
