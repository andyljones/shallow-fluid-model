using UnityEngine;

namespace Assets.Controllers.Manipulator
{
    public class PolyhedronCollider
    {
        public PolyhedronCollider(Mesh mesh)
        {
            InitializePolyhedronCollider(mesh);
        }

        private static void InitializePolyhedronCollider(Mesh mesh)
        {
            var gameObject = new GameObject("Collider");
            var collider = gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.isTrigger = true;
        }
    }
}
