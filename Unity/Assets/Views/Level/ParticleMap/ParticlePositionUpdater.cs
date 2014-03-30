using System.Linq;
using System.Threading.Tasks;
using Engine.Geometry;
using Engine.Simulation;
using UnityEngine;

namespace Assets.Views.Level.ParticleMap
{
    //TODO: This class is an absolute mess. State all over the place.
    /// <summary>
    /// Updates the positions of particles according to a provided velocity field.
    /// </summary>
    public class ParticlePositionUpdater
    {
        private readonly ParticleNeighbourhoodTracker _tracker;

        private readonly float _scaleFactor;

        private readonly Vector3[] _vertexPositions;
        private readonly Vector3[] _vertexVelocities;
        private readonly Vector3[] _particleVelocities;

        /// <summary>
        /// Constructs an updater for the provided geometry.
        /// </summary>
        /// <param name="polyhedron"></param>
        /// <param name="options"></param>
        public ParticlePositionUpdater(IPolyhedron polyhedron, IParticleMapOptions options)
        {
            _scaleFactor = (float)(options.ParticleSpeedScaleFactor * options.Timestep);

            _tracker = new ParticleNeighbourhoodTracker(polyhedron, options.ParticleCount);
            _vertexPositions = GetVertexPositions(polyhedron);

            _vertexVelocities = new Vector3[polyhedron.Vertices.Count];
            _particleVelocities = new Vector3[options.ParticleCount];
        }

        // Returns the positions of each vertex in the given geometry.
        private static Vector3[] GetVertexPositions(IPolyhedron surface)
        {
            return surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position)).ToArray();
        }

        /// <summary>
        /// Servant for Unity's Update(). Combines the provided particle positions and velocity field to generate 
        /// new particle positions.
        /// </summary>
        /// <param name="particlePositions"></param>
        /// <param name="velocityField"></param>
        public void Update(Vector3[] particlePositions, VectorField<Vertex> velocityField)
        {
            UpdateVertexVelocities(velocityField);
            UpdateParticleVelocities(particlePositions);

            for (int i = 0; i < particlePositions.Length; i++)
            {
                CalculateNewPosition(particlePositions, i);
            }
        }

        //TODO: Seeing as this information isn't needed update-to-update, should it really be stored?
        // Updates the _vertexVelocities array using the new velocity field.
        private void UpdateVertexVelocities(VectorField<Vertex> velocityField)
        {
            for (int i = 0; i < _vertexVelocities.Length; i++)
            {
                // Convert from a MathNet.Iridium vector to a Unity vector.
                _vertexVelocities[i] = GraphicsUtilities.Vector3(velocityField[i]);
            }
        }

        // Updates the velocity at each particle using a weighted average of the velocity at each neighbouring vertex.
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

        // Calculates the velocity at a position using a weighted average of the velocities at the provided vertex.
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

        // Move the particle according to the velocity calculated for it.
        private void CalculateNewPosition(Vector3[] particlePositions, int i)
        {
            var particlePosition = particlePositions[i];
            var radius = particlePosition.magnitude;

            var velocity = _particleVelocities[i];
            var newPosition = radius * (particlePosition + velocity).normalized;

            particlePositions[i] = newPosition;
        }
    }
}
