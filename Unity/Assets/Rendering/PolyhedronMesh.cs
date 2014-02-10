using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Rendering
{
    public class PolyhedronMesh
    {
        public readonly Mesh Mesh;

        private readonly IPolyhedron _polyhedron;

        public PolyhedronMesh(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;
            Mesh = CreateMesh();
        }

        #region CreateMesh methods
        private Mesh CreateMesh()
        {
            var mesh = new Mesh();
            mesh.vertices = CreateVectorArray();
            
            mesh.subMeshCount = 2;
            mesh.SetIndices(CreateTriangleArray(), MeshTopology.Triangles, 0);
            mesh.SetIndices(CreateLineArray(), MeshTopology.Lines, 1);

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

        private int[] CreateTriangleArray()
        {
            return _polyhedron.Faces.SelectMany(face => Triangles(face)).ToArray();
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
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                var triangle = new[]
                {
                    center,
                    _polyhedron.IndexOf(vertices[i]),
                    _polyhedron.IndexOf(vertices[i + 1])
                };
                triangles.AddRange(triangle);
            }
            var lastTriangle = new[]
            {
                center,
                _polyhedron.IndexOf(vertices[vertices.Count - 1]),
                _polyhedron.IndexOf(vertices[0]),
            };
            triangles.AddRange(lastTriangle);

            triangles.Reverse();

            return triangles;
        }

        private IEnumerable<int> Line(Edge edge)
        {
            return new List<int> { _polyhedron.IndexOf(edge.B), _polyhedron.IndexOf(edge.A) };
        }
        #endregion
    }
}
