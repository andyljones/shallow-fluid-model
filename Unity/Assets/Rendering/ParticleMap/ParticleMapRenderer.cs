using System;
using System.Linq;
using UnityEngine;

namespace Assets.Rendering.ParticleMap
{
    public class ParticleMapRenderer
    {
        private readonly MeshFilter _particlesMeshFilter;
        
        private readonly int _trailLifespan;
        private readonly int _verticesPerParticle;
        private readonly int _numberOfVertices;

        public ParticleMapRenderer(IParticleMapOptions options)
        {
            _trailLifespan = options.ParticleTrailLifespan;
            _verticesPerParticle = 2*(options.ParticleTrailLifespan - 1);
            _numberOfVertices = options.ParticleCount*_verticesPerParticle;
            _particleLines = new Vector3[_numberOfVertices];

            var particlesGameObject = CreateParticlesGameObject(_numberOfVertices, options.ParticleMaterialName);
            _particlesMeshFilter = particlesGameObject.GetComponent<MeshFilter>();
        }

        private static GameObject CreateParticlesGameObject(int numberOfVertices, String materialName)
        {
            var gameObject = new GameObject("ParticleMap");

            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = CreateParticlesMesh(numberOfVertices);

            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.material = Resources.Load<Material>(materialName);

            return gameObject;
        }

        private static Mesh CreateParticlesMesh(int vertexCount)
        {
            var mesh = new Mesh();
            mesh.vertices = new Vector3[vertexCount];
            mesh.SetIndices(Enumerable.Range(0, vertexCount).ToArray(), MeshTopology.Lines, 0);
            mesh.uv = Enumerable.Repeat(new Vector2(), vertexCount).ToArray();
            mesh.normals = Enumerable.Repeat(new Vector3(), vertexCount).ToArray();

            return mesh;
        }

        private bool isFirstUpdate = true;
        private int _offset = 0;
        private Vector3[] _particleLines;

        public void Update(Vector3[] particlePositions)
        {
            if (isFirstUpdate)
            {
                InitializeLineArray(particlePositions);
                isFirstUpdate = false;
            }
            else
            {
                UpdateLineArray(particlePositions);
            }

            _particlesMeshFilter.mesh.vertices = _particleLines;
        }

        private void UpdateLineArray(Vector3[] particlePositions)
        {
            for (int i = 0; i < particlePositions.Length; i++)
            {
                var previousIndex = GetIndexIntoLineArray(i, _offset - 1);
                var thisIndex = GetIndexIntoLineArray(i, _offset);
                var nextIndex = GetIndexIntoLineArray(i, _offset + 1);

                _particleLines[thisIndex] = _particleLines[previousIndex];
                _particleLines[nextIndex] = particlePositions[i];
            }

            IncrementOffset();

        }

        private void InitializeLineArray(Vector3[] particlePositions)
        {
            _particleLines = new Vector3[_numberOfVertices];
            for (int i = 0; i < particlePositions.Length; i++)
            {
                for (int j = 0; j < _verticesPerParticle; j++)
                {
                    var index = GetIndexIntoLineArray(i, 0) + j;
                    _particleLines[index] = particlePositions[i];
                }
            }

        }

        private int GetIndexIntoLineArray(int particleIndex, int offset)
        {
            return _verticesPerParticle*particleIndex + ((offset + _verticesPerParticle)%_verticesPerParticle);
        }

        private void IncrementOffset()
        {
            _offset = (_offset+2)%_verticesPerParticle;
        }

        public void Reset(int indexToReset, Vector3 newPosition)
        {
            for (int j = 0; j < _verticesPerParticle; j++)
            {
                var index = GetIndexIntoLineArray(indexToReset, 0) + j;
                _particleLines[index] = newPosition;
            }
        }
    }
}
