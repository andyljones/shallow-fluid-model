using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Polyhedra
{
    public static class PolyhedronInitialization
    {
        public static List<Vertex> Vertices(IEnumerable<IEnumerable<Vertex>> vertexLists)
        {
            var vertices = vertexLists.SelectMany(list => list).Distinct().ToList();

            return vertices;
        }

        #region InitializeEdges methods
        public static List<Edge> Edges(IEnumerable<Face> faces)
        {
            var edges = faces.SelectMany(face => EdgesAroundFace(face)).Distinct().ToList();

            return edges;
        }

        private static IEnumerable<Edge> EdgesAroundFace(Face face)
        {
            var vertices = face.Vertices;

            var edges = new List<Edge>();
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                edges.Add(new Edge(vertices[i], vertices[i + 1]));
            }
            edges.Add(new Edge(vertices[vertices.Count - 1], vertices[0]));

            return edges;
        }
        #endregion

        #region InitializeFaces methods
        public static List<Face> Faces(IEnumerable<IEnumerable<Vertex>> vertexLists)
        {
            var faces = vertexLists.Select(vertexList => new Face(SortVertices(vertexList))).ToList();

            return faces;
        }

        private static IEnumerable<Vertex> SortVertices(IEnumerable<Vertex> vertices)
        {
            var vertexList = vertices.ToList();
            var centroid = vertexList.Aggregate(Vector.Zeros(3), (c, v) => c + v.Position) / vertexList.Count;

            if (centroid == Vector.Zeros(3))
            {
                Debug.WriteLine("Centroid of face was the zero vector! Picking an arbitrary centroid instead");
                centroid = new Vector(new[] { 0, 0, 1.0 });
            }

            var sortedVertices = vertexList.OrderBy(vertex => vertex.Position, new ClockwiseCompare(centroid));

            return sortedVertices;
        }
        #endregion

        #region BuildDictionary methods
        public static Dictionary<Vertex, HashSet<Edge>> VertexToEdgeDictionary(IEnumerable<Vertex> vertices, IEnumerable<Edge> edges)
        {
            var vertexToEdges = vertices.ToDictionary(vertex => vertex, vertex => new HashSet<Edge>());
            foreach (var edge in edges)
            {
                vertexToEdges[edge.A].Add(edge);
                vertexToEdges[edge.B].Add(edge);
            }

            return vertexToEdges;
        }

        public static Dictionary<Vertex, HashSet<Face>> VertexToFaceDictionary(IEnumerable<Vertex> vertices, IEnumerable<Face> faces)
        {
            var vertexToFaces = vertices.ToDictionary(vertex => vertex, vertex => new HashSet<Face>());
            foreach (var face in faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    vertexToFaces[vertex].Add(face);
                }
            }

            return vertexToFaces;
        }

        public static Dictionary<Face, HashSet<Edge>> FaceToEdgeDictionary(IEnumerable<Face> faces, Func<Vertex, HashSet<Edge>> vertexToEdges)
        {
            var faceToEdges = new Dictionary<Face, HashSet<Edge>>();
            foreach (var face in faces)
            {
                var edges = new HashSet<Edge>();
                var vertices = face.Vertices;
                for (int i = 0; i < vertices.Count-1; i++)
                {
                    var edge = vertexToEdges(vertices[i]).Intersect(vertexToEdges(vertices[i + 1])).Single();
                    edges.Add(edge);
                }
                var lastEdge = vertexToEdges(vertices[vertices.Count-1]).Intersect(vertexToEdges(vertices[0])).Single();
                edges.Add(lastEdge);
                faceToEdges.Add(face, edges);
            }
            return faceToEdges;
        }

        public static Dictionary<Edge, HashSet<Face>> EdgeToFaceDictionary(IEnumerable<Edge> edges, IEnumerable<Face> faces, Func<Face, HashSet<Edge>> faceToEdges)
        {
            var edgeToFaces = edges.ToDictionary(edge => edge, edge => new HashSet<Face>());
            foreach (var face in faces)
            {
                foreach (var edge in faceToEdges(face))
                {
                    edgeToFaces[edge].Add(face);
                }
            }
            return edgeToFaces;
        }
        #endregion
    }
}
