using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public readonly Dictionary<Vertex, HashSet<Edge>> VertexToEdges;
        public readonly Dictionary<Vertex, HashSet<Face>> VertexToFaces;
        public readonly Dictionary<Face, HashSet<Edge>> FaceToEdges;

        /// <summary>
        /// Construct a polyhedron from a collection of convex, planar collections of vertices.
        /// </summary>
        public Polyhedron(IEnumerable<IEnumerable<Vertex>> verticesInEachFace)
        {
            Vertices = InitializeVertices(verticesInEachFace);
            Faces = InitializeFaces(verticesInEachFace);
            Edges = InitializeEdges(Faces);

            VertexToEdges = BuildVertexToEdgeDictionary(Vertices, Edges);
            VertexToFaces = BuildVertexToFaceDictionary(Vertices, Faces);
            FaceToEdges = BuildFaceToEdgeDictionary(Faces, VertexToEdges);
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

        private Dictionary<Face, HashSet<Edge>> BuildFaceToEdgeDictionary(IEnumerable<Face> faces, Dictionary<Vertex, HashSet<Edge>> vertexToEdgeDictionary)
        {
            var faceToEdges = new Dictionary<Face, HashSet<Edge>>();
            foreach (var face in faces)
            {
                var vertices = face.Vertices;
                var allEdges = vertices.SelectMany(vertex => vertexToEdgeDictionary[vertex]).Distinct();
                var adjacentEdges = allEdges.Where(edge => vertices.Contains(edge.A) && vertices.Contains(edge.B));
                faceToEdges.Add(face, new HashSet<Edge>(adjacentEdges));
            }
            return faceToEdges;
        }
        #endregion
    }
}
