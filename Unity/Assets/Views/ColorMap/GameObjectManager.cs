using UnityEngine;

namespace Assets.Views.ColorMap
{
    public class GameObjectManager
    {
        private readonly MeshFilter _meshFilter;

        public GameObjectManager(Mesh mesh, string materialName)
        {
            var gameObject = BuildGameObject(mesh, materialName);
            _meshFilter = gameObject.GetComponent<MeshFilter>();
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
    }
}
