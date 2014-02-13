using System;
using System.Linq;
using UnityEngine;

namespace Assets.Rendering.ParticleMap
{
    public class ParticleRenderer
    {
        private readonly MeshFilter _particlesMeshFilter;

        public ParticleRenderer(IParticleMapOptions options)
        {
            var particlesGameObject = CreateParticlesGameObject(options.ParticleCount, options.ParticleMaterialName);
            _particlesMeshFilter = particlesGameObject.GetComponent<MeshFilter>();
        }

        private static GameObject CreateParticlesGameObject(int particleCount, String materialName)
        {
            var gameObject = new GameObject("ParticleMap");

            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = CreateParticlesMesh(particleCount);

            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = Resources.Load<Material>(materialName);

            return gameObject;
        }

        private static Mesh CreateParticlesMesh(int particleCount)
        {
            var mesh = new Mesh();
            mesh.vertices = new Vector3[particleCount];
            mesh.SetIndices(Enumerable.Range(0, particleCount).ToArray(), MeshTopology.Points, 0);
            mesh.uv = Enumerable.Repeat(new Vector2(), particleCount).ToArray();
            mesh.normals = Enumerable.Repeat(new Vector3(), particleCount).ToArray();

            return mesh;
        }

        public void Update(Vector3[] particlePositions)
        {
            _particlesMeshFilter.mesh.vertices = particlePositions;
        }


    }
}
