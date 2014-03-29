using System.Collections.Generic;
using System.Linq;
using Assets.Views;
using Assets.Views.Level;
using Engine.Geometry;
using Engine.Utilities;
using UnityEngine;

namespace Assets.Controllers.Level.Cursor
{
    /// <summary>
    /// Converts simulation geometry to Unity 3D geometry, and keeps hold of the mapping between them.
    /// </summary>
    public class MeshManager
    {
        public readonly Mesh Mesh;

        private readonly Face[] _faceAtTriangleIndex;

        /// <summary>
        /// Construct a Unity mesh from a simulation surface.
        /// </summary>
        /// <param name="surface"></param>
        public MeshManager(IPolyhedron surface)
        {
            Mesh = BuildMesh(surface);
            _faceAtTriangleIndex = GetFacesOfTriangleIndices(surface);
        }

        #region BuildMesh methods
        private static Mesh BuildMesh(IPolyhedron surface)
        {
            var vertices = CreateVertexArray(surface);
            var triangles = CreateTriangleArray(surface);
            
            return CreateMesh(vertices, triangles);
        }

        //TODO: Fix the dependency on the specific ordering of vertex vectors then face vectors.
        // Constructs Vector3 for at each Vertex and at the center of each Face. Returns an array which contains
        // the Vertex vectors followed by the Face vectors.
        private static Vector3[] CreateVertexArray(IPolyhedron surface)
        {
            var vertexVectors = surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position));
            var faceVectors = surface.Faces.Select(face => GraphicsUtilities.Vector3(face.SphericalCenter()));

            return vertexVectors.Concat(faceVectors).ToArray();
        }

        // Constructs an array of indices into the vertex array. Each triplet corresponds to a single triangle, which 
        // contains two Vertex vectors and a Face center vector.
        //
        //TODO: I don't like the ambiguity in this representation, but it's what the Unity engine eats so there's not much 
        // arguing. Maybe delay converting it to Unity's representation until the last moment possible?
        private static int[] CreateTriangleArray(IPolyhedron surface)
        {
            var triangles = surface.Faces.SelectMany(face => Triangles(surface, face)).ToArray();

            return triangles;
        }

        // Constructs the indices into the vertex array that correspond to the mesh triangles for a specific face.
        private static IEnumerable<int> Triangles(IPolyhedron surface, Face face)
        {
            var vertices = face.Vertices;

            // Get the index of the vector of the center of the face
            var center = surface.Vertices.Count + surface.IndexOf(face);

            // Loop round the edges of the Face and use each as the base of a triangle, with the third point being the
            // center of the face.
            var triangles = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                var triangle = new[]
                {
                    center,
                    surface.IndexOf(vertices.AtCyclicIndex(i + 1)),
                    surface.IndexOf(vertices.AtCyclicIndex(i))
                };
                triangles.AddRange(triangle);
            }

            return triangles;
        }

        // Takes an array of vectors and an array of indices into that vector array and combines them into a Unity mesh.
        private static Mesh CreateMesh(Vector3[] vertices, int[] triangles)
        {
            var mesh = new Mesh();
            mesh.vertices = vertices;

            mesh.subMeshCount = 1;
            mesh.SetIndices(triangles, MeshTopology.Triangles, 0);

            mesh.uv = Enumerable.Repeat(new Vector2(), mesh.vertexCount).ToArray();

            mesh.normals = mesh.vertices.Select(v => v.normalized).ToArray();

            return mesh;
        }
        #endregion

        // Construct an array which maps the index of a mesh triangle to the Face object which was used to construct it.
        private static Face[] GetFacesOfTriangleIndices(IPolyhedron surface)
        {
            var triangleIndexLookup = surface.Faces.SelectMany(face => Enumerable.Repeat(face, face.Vertices.Count)).ToArray();

            return triangleIndexLookup;
        }

        /// <summary>
        /// Fetch the Face simulation geometry object corresponding to the ith mesh triangle.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Face FaceAtTriangleIndex(int i)
        {
            return _faceAtTriangleIndex[i];
        }
    }
}
