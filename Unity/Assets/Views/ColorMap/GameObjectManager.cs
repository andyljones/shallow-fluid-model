using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Views.ColorMap
{
    public class GameObjectManager : IDisposable
    {
        private readonly MeshFilter _meshFilter;
        private readonly GameObject _gameObject;

        public GameObjectManager(Mesh mesh, string materialName)
        {
            _gameObject = BuildGameObject(mesh, materialName);
            _meshFilter = _gameObject.GetComponent<MeshFilter>();
        }

        private static GameObject BuildGameObject(Mesh mesh, string surfaceMaterialName)
        {
            var gameObject = new GameObject("Polyhedron");

            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.materials = new[]
            {
                Resources.Load<Material>(surfaceMaterialName)
            };

            return gameObject;
        }

        public void Set(Color[] colors)
        {
            _meshFilter.mesh.colors = colors;
        }

        public void Dispose()
        {
            Object.Destroy(_gameObject);
        }
    }
}
