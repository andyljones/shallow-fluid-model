using System.Collections.Generic;
using System.Linq;
using Assets.Views;
using Assets.Views.Level;
using Engine.Geometry;
using Engine.Utilities;
using UnityEngine;

namespace Assets.Controllers.Level.Cursor
{
    public class MeshManager
    {
        public readonly Mesh Mesh;

        private readonly Face[] _faceAtTriangleIndex;

        public MeshManager(IPolyhedron surface)
        {
            Mesh = BuildMesh(surface);
            _faceAtTriangleIndex = GetFacesOfTriangleIndices(surface);
        }

        #region BuildMesh methods
        public static Mesh BuildMesh(IPolyhedron surface)
        {
            var vertices = CreateVertexArray(surface);
            var triangles = CreateTriangleArray(surface);
            
            return CreateMesh(vertices, triangles);
        }

        private static Vector3[] CreateVertexArray(IPolyhedron surface)
        {
            var vertexVectors = surface.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position));
            var faceVectors = surface.Faces.Select(face => GraphicsUtilities.Vector3(face.SphericalCenter()));

            return vertexVectors.Concat(faceVectors).ToArray();
        }

        private static int[] CreateTriangleArray(IPolyhedron surface)
        {
            var triangles = surface.Faces.SelectMany(face => Triangles(surface, face)).ToArray();

            return triangles;
        }

        private static IEnumerable<int> Triangles(IPolyhedron surface, Face face)
        {
            var vertices = face.Vertices;
            var center = surface.Vertices.Count + surface.IndexOf(face);
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

        private static Face[] GetFacesOfTriangleIndices(IPolyhedron surface)
        {
            var triangleIndexLookup = surface.Faces.SelectMany(face => Enumerable.Repeat(face, face.Vertices.Count)).ToArray();

            return triangleIndexLookup;
        }

        public Face FaceAtTriangleIndex(int i)
        {
            return _faceAtTriangleIndex[i];
        }
    }
}
