using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Views.Level.ParticleMap
{
    /// <summary>
    /// Renders a subset of an array of moving particles. Particles are rendered as a trail with the particle's current
    /// position forming the head, and its past positions forming the tail.
    /// 
    /// Because it renders only a subset, multiple ParticleMapRenderers can be used to display far more particles than 
    /// Unity allows in a single mesh.
    /// </summary>
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

        /// <summary>
        /// Construct a particle map renderer that'll display the subarray, defined by the two provided indices, 
        /// of an array of particles.
        /// </summary>
        /// <param name="parentTransform">The transform to render points relative to</param>
        /// <param name="indexOfFirstParticle"></param>
        /// <param name="indexOfOnePastLastParticle"></param>
        /// <param name="options"></param>
        public ParticleMapRenderer(Transform parentTransform, int indexOfFirstParticle, int indexOfOnePastLastParticle, IParticleMapOptions options)
        {
            _indexOfFirstParticle = indexOfFirstParticle;
            _indexOfOnePastLastParticle = indexOfOnePastLastParticle;

            // The trail that follows each particle requires (ParticleTrailLifespan - 1) lines, and each line requires 
            // two vertices. So the number of vertices per line is
            _verticesPerParticle = 2*(options.ParticleTrailLifespan - 1);
            // And the total number of vertices is
            _numberOfVertices = (_indexOfOnePastLastParticle - _indexOfFirstParticle)*_verticesPerParticle;
            _particleLines = new Vector3[_numberOfVertices];

            _particleMapGameObject = CreateParticleMapGameObject(parentTransform, _numberOfVertices, options.ParticleMaterialName);
            _particlesMeshFilter = _particleMapGameObject.GetComponent<MeshFilter>();
        }

        // Creates a mesh of lines and the object that renders it.
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

        // Create a mesh of lines with the specified number of vertices.
        private static Mesh CreateParticlesMesh(int vertexCount)
        {
            var mesh = new Mesh();
            mesh.vertices = new Vector3[vertexCount];
            mesh.SetIndices(Enumerable.Range(0, vertexCount).ToArray(), MeshTopology.Lines, 0);
            mesh.uv = Enumerable.Repeat(new Vector2(), vertexCount).ToArray();
            mesh.normals = Enumerable.Repeat(new Vector3(), vertexCount).ToArray();

            return mesh;
        }

        /// <summary>
        /// Servant for Unity's Update() function. Adds the particlePositions that fall in this ParticleMapRenderer's 
        /// domain to the head of their respective trails, overwriting the "oldest" line in each trail.
        /// </summary>
        /// <param name="particlePositions"></param>
        public void Update(Vector3[] particlePositions)
        {
            if (_isFirstUpdate)
            {
                InitializeLineArray(particlePositions);
                _isFirstUpdate = false;
            }
            else
            {
                // Update this._particleLines
                UpdateLineArray(particlePositions);
            }

            _particlesMeshFilter.mesh.vertices = _particleLines;
        }

        // Creates a new line between each particle's previous position and current position, and overwrites the 
        // oldest line in each trail with it.
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

            // Increment the cyclic index so it points to the next line in the trail.
            IncrementOffset();
        }

        // Initialize the line array so each trail is a zero-length line at the corresponding particle's location..
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

        /// <summary>
        /// Interrupts the specified particle's trail. 
        /// 
        /// Useful when picking a random new location for a particle, as it stops the trail jumping across the globe.
        /// </summary>
        /// <param name="indexToReset"></param>
        /// <param name="newPosition"></param>
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

        // Increment the offset to point to the cyclically next line in each particle's trail.
        private void IncrementOffset()
        {
            _offset = (_offset+2)%_verticesPerParticle;
        }

        #region Destructor & IDisposable methods
        /// <summary>
        /// Destroy the game object that renders the particle map.
        /// </summary>
        public void Dispose()
        {
            Object.Destroy(_particleMapGameObject);
        }
        #endregion
    }
}
