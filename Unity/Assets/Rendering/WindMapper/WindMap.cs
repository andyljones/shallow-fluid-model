using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Models;
using Engine.Polyhedra;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Rendering.WindMapper
{
    public class WindMap
    {
        private Vector3[] _particlePositions;
        private MeshFilter _particlesMeshFilter;

        private readonly IWindMapOptions _options;

        private readonly KDTree _vertexPositions;

        public WindMap(IPolyhedron surface, IWindMapOptions options)
        {
            _options = options;
 
            _vertexPositions = KDTree.MakeFromPoints(FetchVertexPositions(surface));

            _particlePositions = CreateParticles(options.ParticleCount, (float)options.Radius);

            var gameObject = CreateParticlesGameObject(_particlePositions, options.WindMapMaterialName);
            _particlesMeshFilter = gameObject.GetComponent<MeshFilter>();
        }

        private static Vector3[] FetchVertexPositions(IPolyhedron surface)
        {
            return surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position)).ToArray();
        }

        private static GameObject CreateParticlesGameObject(Vector3[] particlePositions, String materialName)
        {
            var gameObject = new GameObject("WindMap");

            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = CreateParticlesMesh(particlePositions);

            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = Resources.Load<Material>(materialName);

            return gameObject;
        }

        private static Mesh CreateParticlesMesh(Vector3[] particles)
        {
            var mesh = new Mesh();
            mesh.vertices = particles;
            mesh.SetIndices(Enumerable.Range(0, particles.Count()).ToArray(), MeshTopology.Points, 0);
            mesh.uv = Enumerable.Repeat(new Vector2(), particles.Count()).ToArray();
            mesh.normals = Enumerable.Repeat(new Vector3(), particles.Count()).ToArray();

            return mesh;
        }

        private static Vector3[] CreateParticles(int particleCount, float radius)
        {
            var particlePositions = new Vector3[particleCount];
            for (int i = 0; i < particleCount; i++)
            {
                particlePositions[i] = 1.01f*radius*Random.onUnitSphere;
            }

            return particlePositions;
        }

        public void Update(VectorField<Vertex> velocityField)
        {
            var newParticlePositions = new Vector3[_particlePositions.Count()];
            for (int i = 0; i < newParticlePositions.Count(); i++)
            {
                newParticlePositions[i] = UpdatePosition(velocityField, _particlePositions[i]);
            }

            _particlePositions = newParticlePositions;
            _particlesMeshFilter.mesh.vertices = _particlePositions;
        }

        private Vector3 UpdatePosition(VectorField<Vertex> velocityField, Vector3 particlePosition)
        {
            var velocity = GetVelocityAtPosition(velocityField, particlePosition);
            var newPosition = CalculateNewPosition(velocity, particlePosition);

            return newPosition;
        }

        private Vector3 CalculateNewPosition(Vector3 velocity, Vector3 particlePosition)
        {
            var radius = particlePosition.magnitude;
            var normalAtParticle = particlePosition.normalized;
            var velocityAlongNormal = Vector3.Dot(velocity, normalAtParticle)*normalAtParticle;
            var directionOfMovement = (velocity - velocityAlongNormal).normalized;
            var angularSpeed = velocity.magnitude / particlePosition.magnitude;

            var newPosition = Mathf.Cos(angularSpeed)*particlePosition + Mathf.Sin(angularSpeed)*radius*directionOfMovement;

            return newPosition;
        }

        private Vector3 GetVelocityAtPosition(VectorField<Vertex> velocityField, Vector3 particlePosition)
        {
            var nearestVertex = GetIndexOfNearestVertex(particlePosition);
            var velocity = velocityField[nearestVertex];

            return GraphicsUtilities.Vector3(_options.WindmapScaleFactor * _options.Timestep * velocity);
        }

        private int GetIndexOfNearestVertex(Vector3 particlePosition)
        {
            return _vertexPositions.FindNearest(particlePosition);
        }
    }
}
