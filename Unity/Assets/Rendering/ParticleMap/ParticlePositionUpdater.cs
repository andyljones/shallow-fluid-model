using System.Linq;
using Engine.Models;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Rendering.ParticleMap
{
    public class ParticlePositionUpdater
    {
        private readonly ParticleNeighbourhoodTracker _tracker;

        private readonly float _scaleFactor;

        private readonly Vector3[] _vertexPositions;
        private readonly Vector3[] _vertexVelocities;
        private readonly Vector3[] _particleVelocities;

        public ParticlePositionUpdater(IPolyhedron polyhedron, IParticleMapOptions options)
        {
            _scaleFactor = (float)(options.WindmapScaleFactor*options.Timestep);

            _tracker = new ParticleNeighbourhoodTracker(polyhedron, options.ParticleCount);
            _vertexPositions = GetVertexPositions(polyhedron);

            _vertexVelocities = new Vector3[polyhedron.Vertices.Count];
            _particleVelocities = new Vector3[options.ParticleCount];
        }

        private static Vector3[] GetVertexPositions(IPolyhedron surface)
        {
            return surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position)).ToArray();
        }

        public void Update(ref Vector3[] particlePositions, VectorField<Vertex> velocityField)
        {
            UpdateVertexVelocities(velocityField);
            UpdateParticleVelocities(particlePositions);

            for (int i = 0; i < particlePositions.Length; i++)
            {
                var position = particlePositions[i];
                var velocity = _particleVelocities[i];
                var newPosition = CalculateNewPosition(velocity, position);
                particlePositions[i] = newPosition;
            }
        }

        private void UpdateVertexVelocities(VectorField<Vertex> velocityField)
        {
            for (int i = 0; i < _vertexVelocities.Length; i++)
            {
                _vertexVelocities[i] = GraphicsUtilities.Vector3(velocityField[i]);
            }
        }

        private void UpdateParticleVelocities(Vector3[] particlePositions)
        {
            var indicesOfNearestVertices = _tracker.GetIndicesOfVerticesNearest(particlePositions);

            for (int i = 0; i < particlePositions.Length; i++)
            {
                var particlePosition = particlePositions[i];
                var indicesOfNeighbourhood = indicesOfNearestVertices[i];
                _particleVelocities[i] = GetVelocity(particlePosition, indicesOfNeighbourhood);
            }

        }

        private Vector3 CalculateNewPosition(Vector3 velocity, Vector3 particlePosition)
        {
            var radius = particlePosition.magnitude;

            var newPosition = radius*(particlePosition + velocity).normalized;

            return newPosition;
        }

        private Vector3 GetVelocity(Vector3 position, int[] nearestVertices)
        {
            var sumOfVelocities = new Vector3();
            var sumOfWeights = 0f;
            for (int i = 0; i < nearestVertices.Length; i++)
            {
                var vertexIndex = nearestVertices[i];
                var velocity = _vertexVelocities[vertexIndex];
                
                var weight = 1.0f/(_vertexPositions[vertexIndex] - position).magnitude;

                sumOfVelocities = sumOfVelocities + weight*velocity;
                sumOfWeights = sumOfWeights + weight;
            }

            return _scaleFactor * (sumOfVelocities / sumOfWeights);
        }
    }
}
