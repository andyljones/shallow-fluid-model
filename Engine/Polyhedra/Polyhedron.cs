using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Polyhedra
{
    /// <summary>
    /// Represents a polyhedron containing the origin.
    /// </summary>
    public class Polyhedron
    {
        /// <summary>
        /// The faces of the polyhedron, with their vertices listed in clockwise order when looking toward the origin.
        /// </summary>
        public readonly List<Face> Faces;
        public readonly List<Edge> Edges;
        public readonly List<Vertex> Vertices;

        public readonly Dictionary<Vertex, HashSet<Edge>> VertexToEdgeDictionary;
        public readonly Dictionary<Vertex, HashSet<Face>> VertexToFaceDictionary;
        public readonly Dictionary<Face, HashSet<Edge>> FaceToEdgeDictionary; 

        /// <summary>
        /// Construct a polyhedron from a collection of convex, planar collections of vertices.
        /// </summary>
        public Polyhedron(IEnumerable<IEnumerable<Vertex>> vertexLists)
        {
            Vertices = InitializeVertices(vertexLists);
            Faces = InitializeFaces(vertexLists);
            Edges = InitializeEdges(Faces);

            VertexToEdgeDictionary = BuildVertexToEdgeDictionary(Vertices, Edges);
            VertexToFaceDictionary = BuildVertexToFaceDictionary(Vertices, Faces);
            FaceToEdgeDictionary = BuildFaceToEdgeDictionary(Faces, VertexToEdgeDictionary);
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
            var vertexToEdgeDictionary = vertices.ToDictionary(vertex => vertex, vertex => new HashSet<Edge>());
            foreach (var edge in edges)
            {
                vertexToEdgeDictionary[edge.A].Add(edge);
                vertexToEdgeDictionary[edge.B].Add(edge);
            }

            return vertexToEdgeDictionary;
        }

        private Dictionary<Vertex, HashSet<Face>> BuildVertexToFaceDictionary(IEnumerable<Vertex> vertices, IEnumerable<Face> faces)
        {
            var vertexToFaceDictionary = vertices.ToDictionary(vertex => vertex, vertex => new HashSet<Face>());
            foreach (var face in faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    vertexToFaceDictionary[vertex].Add(face);
                }
            }

            return vertexToFaceDictionary;
        }

        private Dictionary<Face, HashSet<Edge>> BuildFaceToEdgeDictionary(IEnumerable<Face> faces, Dictionary<Vertex, HashSet<Edge>> vertexToEdgeDictionary)
        {
            var faceToEdgeDictionary = new Dictionary<Face, HashSet<Edge>>();
            foreach (var face in faces)
            {
                var vertices = face.Vertices;
                var allEdges = vertices.SelectMany(vertex => vertexToEdgeDictionary[vertex]).Distinct();
                var adjacentEdges = allEdges.Where(edge => vertices.Contains(edge.A) && vertices.Contains(edge.B));
                faceToEdgeDictionary.Add(face, new HashSet<Edge>(adjacentEdges));
            }
            return faceToEdgeDictionary;
        }
        #endregion
    }
}
