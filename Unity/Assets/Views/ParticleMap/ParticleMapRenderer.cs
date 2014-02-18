using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Views.ParticleMap
{
    public class ParticleMapRenderer : IDisposable
    {
        private readonly MeshFilter _particlesMeshFilter;
        
        private readonly int _verticesPerParticle;
        private readonly int _numberOfVertices;

        private bool _isFirstUpdate = true;
        private int _offset = 0;
        private Vector3[] _particleLines;

        private readonly int _indexOfFirstParticle;
        private readonly int _indexOfOnePastLastParticle;
        private readonly GameObject _particleMapGameObject;

        public ParticleMapRenderer(Transform parentTransform, int indexOfFirstParticle, int indexOfOnePastLastParticle, IParticleMapOptions options)
        {
            _indexOfFirstParticle = indexOfFirstParticle;
            _indexOfOnePastLastParticle = indexOfOnePastLastParticle;

            _verticesPerParticle = 2*(options.ParticleTrailLifespan - 1);
            _numberOfVertices = (_indexOfOnePastLastParticle - _indexOfFirstParticle)*_verticesPerParticle;
            _particleLines = new Vector3[_numberOfVertices];

            _particleMapGameObject = CreateParticleMapGameObject(parentTransform, _numberOfVertices, options.ParticleMaterialName);
            _particlesMeshFilter = _particleMapGameObject.GetComponent<MeshFilter>();
        }

        private static GameObject CreateParticleMapGameObject(Transform parentTransform, int numberOfVertices, String materialName)
        {
            var gameObject = new GameObject("ParticleMap");
            gameObject.transform.parent = parentTransform;

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

        public void Update(Vector3[] particlePositions)
        {
            if (_isFirstUpdate)
            {
                InitializeLineArray(particlePositions);
                _isFirstUpdate = false;
            }
            else
            {
                UpdateLineArray(particlePositions);
            }

            _particlesMeshFilter.mesh.vertices = _particleLines;
        }

        private void UpdateLineArray(Vector3[] particlePositions)
        {
            for (int i = _indexOfFirstParticle; i < _indexOfOnePastLastParticle; i++)
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
            for (int i = _indexOfFirstParticle; i < _indexOfOnePastLastParticle; i++)
            {
                for (int j = 0; j < _verticesPerParticle; j++)
                {
                    var index = GetIndexIntoLineArray(i, 0) + j;
                    _particleLines[index] = particlePositions[i];
                }
            }

        }

        public void Reset(int indexToReset, Vector3 newPosition)
        {
            var secondPreviousIndex = GetIndexIntoLineArray(indexToReset, _offset - 2);
            var previousIndex = GetIndexIntoLineArray(indexToReset, _offset-1);
            _particleLines[secondPreviousIndex] = newPosition;            
            _particleLines[previousIndex] = newPosition;
        }

        private int GetIndexIntoLineArray(int particleIndex, int offset)
        {
            return _verticesPerParticle*(particleIndex - _indexOfFirstParticle) + ((offset + _verticesPerParticle)%_verticesPerParticle);
        }

        private void IncrementOffset()
        {
            _offset = (_offset+2)%_verticesPerParticle;
        }

        #region Destructor & IDisposable methods
        public void Dispose()
        {
            Object.Destroy(_particleMapGameObject);
        }
        #endregion
    }
}
