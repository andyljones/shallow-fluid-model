using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Views.Level.ColorMap
{
    /// <summary>
    /// Wraps the color map Unity object, exposing a method that can set the color field.
    /// </summary>
    public class GameObjectManager : IDisposable
    {
        private readonly MeshFilter _meshFilter;
        private readonly GameObject _gameObject;

        /// <summary>
        /// Construct a game object for use in displaying a color map using the specified mesh and material.
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="materialName"></param>
        public GameObjectManager(Mesh mesh, string materialName)
        {
            _gameObject = BuildGameObject(mesh, materialName);
            _meshFilter = _gameObject.GetComponent<MeshFilter>();
        }

        // Creates a game object and gives it the specified mesh and material.
        private static GameObject BuildGameObject(Mesh mesh, string surfaceMaterialName)
        {
            var gameObject = new GameObject("Color map");

            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.materials = new[]
            {
                Resources.Load<Material>(surfaceMaterialName)
            };

            return gameObject;
        }

        //TODO: Replace with a map from Unity vectors (representing 3D vertices) and colors.
        /// <summary>
        /// Sets the vertex colors of the color map. The ith color will be assigned to the ith vertex in mesh.vertices.
        /// </summary>
        /// <param name="colors"></param>
        public void Set(Color[] colors)
        {
            _meshFilter.mesh.colors = colors;
        }

        /// <summary>
        /// Destroys the color map game object.
        /// </summary>
        public void Dispose()
        {
            Object.Destroy(_gameObject);
        }
    }
}
