using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using Engine.Utilities;
using UnityEngine;

namespace Assets.Views.Surface
{
    public class PolyhedronMeshHandler
    {
        public readonly Mesh Mesh;

        public Face FaceAtTriangleIndex(int index) { return _facesAtTriangleIndex[index]; } 
        private readonly Face[] _facesAtTriangleIndex;

        private readonly IPolyhedron _polyhedron;

        public PolyhedronMeshHandler(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;

            var lines = CreateLineArray();
            var triangles = CreateTriangleArray(out _facesAtTriangleIndex);
            Mesh = CreateMesh(triangles, lines);
        }

        #region CreateMesh methods
        private Mesh CreateMesh(int[] triangles, int[] lines)
        {
            var mesh = new Mesh();
            mesh.vertices = CreateVectorArray();

            mesh.subMeshCount = 1;//2;
            mesh.SetIndices(triangles, MeshTopology.Triangles, 0);
            //mesh.SetIndices(lines, MeshTopology.Lines, 1);

            mesh.uv = Enumerable.Repeat(new Vector2(), mesh.vertexCount).ToArray();

            mesh.normals = mesh.vertices.Select(v => v.normalized).ToArray();

            return mesh;
        }

        private Vector3[] CreateVectorArray()
        {
            var vertexVectors = _polyhedron.Vertices.Select(vertex => GraphicsUtilities.Vector3(vertex.Position));
            var faceVectors = _polyhedron.Faces.Select(face => GraphicsUtilities.Vector3(face.SphericalCenter()));

            return vertexVectors.Concat(faceVectors).ToArray();
        }

        private int[] CreateTriangleArray(out Face[] triangleIndexLookup)
        {
            triangleIndexLookup = _polyhedron.Faces.SelectMany(face => Enumerable.Repeat(face, face.Vertices.Count)).ToArray();
            var triangles = _polyhedron.Faces.SelectMany(face => Triangles(face)).ToArray();

            return triangles;
        }

        private int[] CreateLineArray()
        {
            return _polyhedron.Edges.SelectMany(edge => Line(edge)).ToArray();
        }

        private IEnumerable<int> Triangles(Face face)
        {
            var vertices = face.Vertices;
            var center = _polyhedron.Vertices.Count + _polyhedron.IndexOf(face);
            var triangles = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                var triangle = new[]
                {
                    center,
                    _polyhedron.IndexOf(vertices.AtCyclicIndex(i + 1)),
                    _polyhedron.IndexOf(vertices.AtCyclicIndex(i))
                };
                triangles.AddRange(triangle);
            }

            return triangles;
        }

        private IEnumerable<int> Line(Edge edge)
        {
            return new List<int> { _polyhedron.IndexOf(edge.B), _polyhedron.IndexOf(edge.A) };
        }
        #endregion


    }
}
