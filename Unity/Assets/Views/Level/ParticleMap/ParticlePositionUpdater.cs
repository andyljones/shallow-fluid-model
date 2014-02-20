using System.Linq;
using System.Threading.Tasks;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.Level.ParticleMap
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
            _scaleFactor = (float)(options.ParticleSpeedScaleFactor * options.Timestep);

            _tracker = new ParticleNeighbourhoodTracker(polyhedron, options.ParticleCount);
            _vertexPositions = GetVertexPositions(polyhedron);

            _vertexVelocities = new Vector3[polyhedron.Vertices.Count];
            _particleVelocities = new Vector3[options.ParticleCount];
        }

        private static Vector3[] GetVertexPositions(IPolyhedron surface)
        {
            return surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position)).ToArray();
        }

        public void Update(Vector3[] particlePositions, VectorField<Vertex> velocityField)
        {
            UpdateVertexVelocities(velocityField);
            UpdateParticleVelocities(particlePositions);

            for (int i = 0; i < particlePositions.Length; i++)
            {
                CalculateNewPosition(particlePositions, i);
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

            for (int i = 0; i < _particleVelocities.Length; i++)
            {
                UpdateParticleVelocity(particlePositions, indicesOfNearestVertices, i);
            }
        }

        private void UpdateParticleVelocity(Vector3[] particlePositions, int[][] indicesOfNearestVertices, int i)
        {
            var particlePosition = particlePositions[i];
            var indicesOfNeighbourhood = indicesOfNearestVertices[i];
            _particleVelocities[i] = GetVelocity(particlePosition, indicesOfNeighbourhood);
        }

        private void CalculateNewPosition(Vector3[] particlePositions, int i)
        {
            var particlePosition = particlePositions[i];
            var radius = particlePosition.magnitude;

            var velocity = _particleVelocities[i];
            var newPosition = radius * (particlePosition + velocity).normalized;

            particlePositions[i] = newPosition;
        }

        private Vector3 GetVelocity(Vector3 position, int[] nearestVertices)
        {
            var sumOfVelocities = new Vector3();
            var sumOfWeights = 0f;
            for (int i = 0; i < nearestVertices.Length; i++)
            {
                var vertexIndex = nearestVertices[i];
                var velocity = _vertexVelocities[vertexIndex];

                var weight = 1.0f / (_vertexPositions[vertexIndex] - position).magnitude;

                sumOfVelocities = sumOfVelocities + weight * velocity;
                sumOfWeights = sumOfWeights + weight;
            }

            return _scaleFactor * (sumOfVelocities / sumOfWeights);
        }
    }
}
