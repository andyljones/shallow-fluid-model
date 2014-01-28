using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Polyhedra
{
    public interface IPolyhedron
    {
        /// <summary>
        /// The faces of the polyhedron, with their vertices listed in clockwise order when looking toward the origin.
        /// </summary>
        List<Face> Faces { get; }
        List<Edge> Edges { get; }
        List<Vertex> Vertices { get; }
        HashSet<Edge> EdgesOf(Vertex vertex);
        HashSet<Edge> EdgesOf(Face face);
        HashSet<Face> FacesOf(Vertex vertex);
        HashSet<Face> FacesOf(Edge edge);
    }

    /// <summary>
    /// Represents a polyhedron containing the origin.
    /// </summary>
    public class Polyhedron : IPolyhedron
    {
        /// <summary>
        /// The faces of the polyhedron, with their vertices listed in clockwise order when looking toward the origin.
        /// </summary>
        public List<Face> Faces { get { return _faces; } }
        private readonly List<Face> _faces;

        public List<Edge> Edges { get { return _edges; } }
        private readonly List<Edge> _edges;

        public List<Vertex> Vertices { get { return _vertices; } }
        private readonly List<Vertex> _vertices;

        #region Lookup functions
        public HashSet<Edge> EdgesOf(Vertex vertex) { return _vertexToEdges[vertex]; }
        private readonly Dictionary<Vertex, HashSet<Edge>> _vertexToEdges;

        public HashSet<Edge> EdgesOf(Face face) { return _faceToEdges[face]; }
        private readonly Dictionary<Face, HashSet<Edge>> _faceToEdges;

        public HashSet<Face> FacesOf(Vertex vertex) { return _vertexToFaces[vertex]; } 
        private readonly Dictionary<Vertex, HashSet<Face>> _vertexToFaces;

        public HashSet<Face> FacesOf(Edge edge) { return _edgeToFaces[edge]; } 
        private readonly Dictionary<Edge, HashSet<Face>> _edgeToFaces;
        #endregion

        /// <summary>
        /// Construct a polyhedron from a collection of convex, planar collections of vertices.
        /// </summary>
        public Polyhedron(IEnumerable<IEnumerable<Vertex>> verticesInEachFace)
        {
            _vertices = InitializeVertices(verticesInEachFace);
            _faces = InitializeFaces(verticesInEachFace);
            _edges = InitializeEdges(Faces);

            _vertexToEdges = BuildVertexToEdgeDictionary(Vertices, Edges);
            _vertexToFaces = BuildVertexToFaceDictionary(Vertices, Faces);
            _faceToEdges = BuildFaceToEdgeDictionary(Faces, EdgesOf);
            _edgeToFaces = BuildEdgeToFaceDictionary(Edges, Faces, EdgesOf);
        }

        private static List<Vertex> InitializeVertices(IEnumerable<IEnumerable<Vertex>> vertexLists)
        {
            var vertices = vertexLists.SelectMany(list => list).Distinct().ToList();

            return vertices;
        }

        #region InitializeEdges methods
        private static List<Edge> InitializeEdges(IEnumerable<Face> faces)
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
        private static List<Face> InitializeFaces(IEnumerable<IEnumerable<Vertex>> vertexLists)
        {
            var faces = vertexLists.Select(vertexList => new Face(SortVertices(vertexList))).ToList();

            return faces;
        }

        private static IEnumerable<Vertex> SortVertices(IEnumerable<Vertex> vertices)
        {
            var vertexList = vertices.ToList();
            var centroid = vertexList.Aggregate(Vector.Zeros(3), (c, v) => c + v.Position)/vertexList.Count;

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
        private Dictionary<Vertex, HashSet<Edge>> BuildVertexToEdgeDictionary(IEnumerable<Vertex> vertices, IEnumerable<Edge> edges)
        {
            var vertexToEdges = vertices.ToDictionary(vertex => vertex, vertex => new HashSet<Edge>());
            foreach (var edge in edges)
            {
                vertexToEdges[edge.A].Add(edge);
                vertexToEdges[edge.B].Add(edge);
            }

            return vertexToEdges;
        }

        private Dictionary<Vertex, HashSet<Face>> BuildVertexToFaceDictionary(IEnumerable<Vertex> vertices, IEnumerable<Face> faces)
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

        private Dictionary<Face, HashSet<Edge>> BuildFaceToEdgeDictionary(IEnumerable<Face> faces, Func<Vertex, HashSet<Edge>> vertexToEdges)
        {
            var faceToEdges = new Dictionary<Face, HashSet<Edge>>();
            foreach (var face in faces)
            {
                var vertices = face.Vertices;
                var allEdges = vertices.SelectMany(vertex => vertexToEdges(vertex)).Distinct();
                var adjacentEdges = allEdges.Where(edge => vertices.Contains(edge.A) && vertices.Contains(edge.B));
                faceToEdges.Add(face, new HashSet<Edge>(adjacentEdges));
            }
            return faceToEdges;
        }

        private Dictionary<Edge, HashSet<Face>> BuildEdgeToFaceDictionary(IEnumerable<Edge> edges, IEnumerable<Face> faces, Func<Face, HashSet<Edge>> faceToEdges)
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
