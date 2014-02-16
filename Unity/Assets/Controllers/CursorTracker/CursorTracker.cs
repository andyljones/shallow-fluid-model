using System;
using Engine.Geometry;
using UnityEngine;

namespace Assets.Controllers.CursorTracker
{
    public class CursorTracker
    {
        private readonly Camera _camera;
        private readonly Func<int, Face> _faceAtTriangleIndex; 

        public CursorTracker(Camera camera, MeshManager meshManager)
        {
            _camera = camera;
            _faceAtTriangleIndex = meshManager.FaceAtTriangleIndex;
            
            InitializePolyhedronCollider(meshManager.Mesh);
        }

        private static void InitializePolyhedronCollider(Mesh mesh)
        {
            var gameObject = new GameObject("Collider");
            var collider = gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.isTrigger = true;
        }

        public Face TryGetFaceUnderCursor()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var indexOfHitTriangle = hit.triangleIndex;
                var face = _faceAtTriangleIndex(indexOfHitTriangle);
                return face;
            }

            return null;
        }
    }
}
