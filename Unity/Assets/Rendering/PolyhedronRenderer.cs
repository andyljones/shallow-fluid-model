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
    public class PolyhedronRenderer
    {
        private readonly GameObject _gameObject;
        private readonly Dictionary<Vertex, int> _vertexIndices; 

        public PolyhedronRenderer(Polyhedron polyhedron, String name, String wireframeMaterialName = "", String surfaceMaterialName = "")
        {
            _vertexIndices = GraphicsUtilities.CreateItemToIndexMap(polyhedron.Vertices);
            _gameObject = GraphicsUtilities.CreateRenderingObject(name, CreateVertexArray(_vertexIndices));

            if (wireframeMaterialName != "")
            { 
                GraphicsUtilities.AddWireframe(_gameObject, CreateLineArray(_vertexIndices, polyhedron.Edges), wireframeMaterialName);
            }

            if (surfaceMaterialName != "")
            {
                GraphicsUtilities.AddSurface(_gameObject, CreateTriangleArray(_vertexIndices, polyhedron.Faces), surfaceMaterialName);
            }
        }

        #region CreateMesh methods
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

        private static int[] CreateLineArray(Dictionary<Vertex, int> vertexIndices, IEnumerable<Edge> edges)
        {
            return edges.SelectMany(edge => Indices(vertexIndices, edge)).ToArray();
        }

        private static IEnumerable<int> Indices(Dictionary<Vertex, int> vertexIndices, Face face)
        {
            var vertices = face.Vertices;
            var triangles = new List<int>();
            for (int i = 1; i < vertices.Count-1; i++)
            {
                var triangle = new [] { vertexIndices[vertices[0]], vertexIndices[vertices[i]], vertexIndices[vertices[i+1]] };
                triangles.AddRange(triangle);
            }
            return triangles;
        }

        private static IEnumerable<int> Indices(Dictionary<Vertex, int> vertexIndices, Edge edge)
        {
            return new List<int> {vertexIndices[edge.A], vertexIndices[edge.B]};
        }
        #endregion
    }
}
