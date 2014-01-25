using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using UnityEngine;

namespace Assets.Rendering
{
    /// <summary>
    /// Renders a Polyhedron as a solid object with the requested properties.
    /// </summary>
    public class SolidPolyhedronRenderer
    {
        private readonly GameObject _gameObject;
        private readonly Dictionary<Vertex, int> _vertexIndices; 

        public SolidPolyhedronRenderer(Polyhedron polyhedron, String name, String materialName)
        {
            _vertexIndices = GraphicsUtilities.CreateItemToIndexMap(polyhedron.Vertices);

            var mesh = CreateMesh(polyhedron, _vertexIndices); 
            _gameObject = GraphicsUtilities.CreateRenderingObject(name, materialName, mesh);
        }

        #region CreateMesh methods
        private static Mesh CreateMesh(Polyhedron polyhedron, Dictionary<Vertex, int> vertexIndices)
        {
            var mesh = new Mesh();
            mesh.vertices = CreateVertexArray(vertexIndices);
            //mesh.subMeshCount = 2;

            mesh.SetIndices(CreateTriangleArray(vertexIndices, polyhedron.Faces), MeshTopology.Triangles, 0);
            //mesh.SetIndices(CreateLineStripArray(vertexIndices, polyhedron.Edges), MeshTopology.LineStrip, 1);

            mesh.uv = new Vector2[] {};
            mesh.RecalculateNormals();

            return mesh;
        }

        private static Vector3[] CreateVertexArray(Dictionary<Vertex, int> vertexIndices)
        {
            var unityVertices = new Vector3[vertexIndices.Count];
            foreach (var vertexAndIndex in vertexIndices)
            {
                var vertex = vertexAndIndex.Key;
                var index = vertexAndIndex.Value;

                unityVertices[index] = GraphicsUtilities.Vector3(vertex.Position);
            }

            return unityVertices;
        }

        private static int[] CreateTriangleArray(Dictionary<Vertex, int> vertexIndices, IEnumerable<Face> faces)
        {
            return faces.SelectMany(face => Indices(vertexIndices, face)).ToArray();
        }

        private static int[] CreateLineStripArray(Dictionary<Vertex, int> vertexIndices, IEnumerable<Edge> edges)
        {
            Debug.Log(edges.Count());
            return edges.SelectMany(edge => Indices(vertexIndices, edge)).ToArray();
        }

        private static IEnumerable<int> Indices(Dictionary<Vertex, int> vertexIndices, Face face)
        {
            return face.Vertices.Select(vertex => vertexIndices[vertex]);
        }

        private static IEnumerable<int> Indices(Dictionary<Vertex, int> vertexIndices, Edge edge)
        {
            return new List<int> {vertexIndices[edge.A], vertexIndices[edge.B]};
        }
        #endregion
    }
}
