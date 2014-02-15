using UnityEngine;

namespace Assets.Controller.UserInterface
{
    public class PolyhedronCollider
    {
        private readonly GameObject _gameObject;

        public PolyhedronCollider(GameObject polyhedronGameObject, Mesh mesh)
        {
            _gameObject = polyhedronGameObject;
            InitializePolyhedronCollider(polyhedronGameObject, mesh);

        }

        private static void InitializePolyhedronCollider(GameObject polyhedronGameObject, Mesh mesh)
        {
            var collider = polyhedronGameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.isTrigger = true;
        }
    }
}
