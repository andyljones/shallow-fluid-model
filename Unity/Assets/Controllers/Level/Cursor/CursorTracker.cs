using System;
using System.Linq;
using Assets.Views;
using Assets.Views.Level;
using Engine.Geometry;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Controllers.Level.Cursor
{
    /// <summary>
    /// Fetches the simulation geometry objects corresponding to the current cursor position.
    /// </summary>
    public class CursorTracker : IDisposable
    {
        private readonly Camera _camera;
        private readonly Func<int, Face> _faceAtTriangleIndex;

        private readonly GameObject _gameObject;

        /// <summary>
        /// Construct a CursorTracker that will track the simulation geometry objects that're under the cursor.
        /// </summary>
        /// <param name="camera">The point of view to use when judging what's underneath the cursor</param>
        /// <param name="meshManager">The mesh & mesh-triangle-to-simulation-geometry mapping to use to select geometry objects</param>
        public CursorTracker(Camera camera, MeshManager meshManager)
        {
            _camera = camera;
            _faceAtTriangleIndex = meshManager.FaceAtTriangleIndex;
            
            _gameObject = InitializePolyhedronCollider(meshManager.Mesh);
        }

        // Sets up the collider that'll be used to get the index of the triangle under the cursor.
        private static GameObject InitializePolyhedronCollider(Mesh mesh)
        {
            var gameObject = new GameObject("Collider");
            var collider = gameObject.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            collider.isTrigger = true;

            return gameObject;
        }

        /// <summary>
        /// Fetches the Face simulation geometry object under the cursor.
        /// </summary>
        /// <returns>The face currently under the cursor</returns>
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
            else
            {
                return null;                
            }
        }

        /// <summary>
        /// Fetches the Vertex simulation geometry object closest to the cursor.
        /// </summary>
        /// <returns></returns>
        public Vertex TryGetVertexUnderCursor()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var indexOfHitTriangle = hit.triangleIndex;
                var face = _faceAtTriangleIndex(indexOfHitTriangle);
                
                return FindClosestVertex(face, hit.point);
            }
            else
            {
                return null;
            }
        }

        // Select the Vertex of a Face that's closest to the given vector.
        private Vertex FindClosestVertex(Face face, Vector3 target)
        {
            var sortedVertices = face.Vertices.OrderBy(vertex => (GraphicsUtilities.Vector3(vertex.Position) - target).magnitude);

            return sortedVertices.First();
        }

        /// <summary>
        /// Destroy the CursorTracker's mesh collider.
        /// </summary>
        public void Dispose()
        {
            Object.Destroy(_gameObject);
        }
    }
}
