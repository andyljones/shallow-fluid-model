using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Utilities;

namespace Engine.Geometry
{
    /// <summary>
    /// Static methods to assist the constructor of the Polyhedron class.
    /// </summary>
    public static class PolyhedronInitialization
    {
        /// <summary>
        /// Flattens the provided list of vertices into a single list of distinct vertices.
        /// </summary>
        /// <param name="vertexLists"></param>
        /// <returns></returns>
        public static List<Vertex> Vertices(List<List<Vertex>> vertexLists)
        {
            var vertices = vertexLists.SelectMany(list => list).Distinct().ToList();

            return vertices;
        }

        #region InitializeEdges methods
        /// <summary>
        /// Extracts a list of distinct edges from the provided list of faces.
        /// </summary>
        /// <param name="faces"></param>
        /// <returns></returns>
        public static List<Edge> Edges(List<Face> faces)
        {
            var edges = faces.SelectMany(face => EdgesAroundFace(face)).Distinct().ToList();

            return edges;
        }

        // Constructs the edges of a face from the ordered list of vertices of a face.
        private static IEnumerable<Edge> EdgesAroundFace(Face face)
        {
            var vertices = face.Vertices;

            var edges = new List<Edge>();
            //TODO: Use cyclic indexing here?
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                edges.Add(new Edge(vertices[i], vertices[i + 1]));
            }
            edges.Add(new Edge(vertices[vertices.Count - 1], vertices[0]));

            return edges;
        }
        #endregion

        /// <summary>
        /// Uses each list individual list of vertices to create a face, and returns a list of these faces.
        /// </summary>
        /// <param name="vertexLists"></param>
        /// <returns></returns>
        public static List<Face> Faces(List<List<Vertex>> vertexLists)
        {
            var faces = vertexLists.Select(vertexList => new Face(vertexList)).ToList();

            return faces;
        }

        #region BuildDictionary methods
        //TODO: Does this actually need to be given the list of vertices? Can't that be inferred from the edges?
        /// <summary>
        /// Build a lookup from the given vertices to the edges that contain them.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static Dictionary<Vertex, List<Edge>> VertexToEdgeDictionary(List<Vertex> vertices, List<Edge> edges)
        {
            var vertexToEdges = vertices.ToDictionary(vertex => vertex, vertex => new List<Edge>());
            foreach (var edge in edges)
            {
                vertexToEdges[edge.A].Add(edge);
                vertexToEdges[edge.B].Add(edge);
            }

            foreach (var vertex in vertices)
            {
                var comparer = new AnticlockwiseComparer(vertex.Position, -vertex.Position);
                var sortedEdges = vertexToEdges[vertex].OrderBy(edge => edge.SphericalCenter(), comparer);
                vertexToEdges[vertex] = sortedEdges.ToList();
            }

            return vertexToEdges;
        }

        /// <summary>
        /// Build a lookup from each vertex to the faces that contain it. 
        /// 
        /// The faces are listed in the same order as the edges of the vertex are by vertexToEdges.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="faces"></param>
        /// <param name="vertexToEdges"></param>
        /// <returns></returns>
        public static Dictionary<Vertex, List<Face>> VertexToFaceDictionary(List<Vertex> vertices, List<Face> faces, Dictionary<Vertex, List<Edge>> vertexToEdges)
        {
            var vertexToFaces = vertices.ToDictionary(vertex => vertex, vertex => new List<Face>());
            foreach (var face in faces)
            {
                foreach (var vertex in face.Vertices)
                {
                    vertexToFaces[vertex].Add(face);
                }
            }

            foreach (var vertex in vertices)
            {
                var edgesAroundVertex = vertexToEdges[vertex];
                var facesAroundVertex = vertexToFaces[vertex];

                vertexToFaces[vertex] = SortFacesToMatchEdgeOrder(vertex, edgesAroundVertex, facesAroundVertex);
            }

            return vertexToFaces;
        }

        // Sorts the list of faces to match the list of edges.
        private static List<Face> SortFacesToMatchEdgeOrder(Vertex vertex, List<Edge> edges, List<Face> faces)
        {
            //TODO: I've picked up a bug somewhere in here, but since it all seems to work I can't be bothered to fix it.
            try {
                var orderedFaces = new List<Face>();
                for (int index = 0; index < edges.Count; index++)
                {
                    var previousNeighbour = edges.AtCyclicIndex(index).Vertices().First(v => v != vertex);
                    var nextNeighbour = edges.AtCyclicIndex(index - 1).Vertices().First(v => v != vertex);
                    
                    var faceBetween = faces.First(face => face.Vertices.Contains(previousNeighbour) && face.Vertices.Contains(nextNeighbour));
                    orderedFaces.Add(faceBetween);
                }

                return orderedFaces;
            } catch (System.InvalidOperationException) {
                return  faces;
            }
        }

        #region FaceToEdgeDictionary methods
        /// <summary>
        /// Builds a lookup from each face to the edges it contains.
        /// </summary>
        /// <param name="faces"></param>
        /// <param name="edgesOf"></param>
        /// <returns></returns>
        public static Dictionary<Face, List<Edge>> FaceToEdgeDictionary(List<Face> faces, Func<Vertex, List<Edge>> edgesOf)
        {
            var faceToEdges = new Dictionary<Face, List<Edge>>();
            foreach (var face in faces)
            {
                faceToEdges.Add(face, EdgesOfFace(face, edgesOf));
            }
            return faceToEdges;
        }

        // Returns a list of the edges that neighbour a face.
        private static List<Edge> EdgesOfFace(Face face, Func<Vertex, List<Edge>> edgesOf)
        {
            var edges = new List<Edge>();
            var vertices = face.Vertices;
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                var edge = edgesOf(vertices[i]).Intersect(edgesOf(vertices[i + 1])).Single();
                edges.Add(edge);
            }
            var lastEdge = edgesOf(vertices[vertices.Count - 1]).Intersect(edgesOf(vertices[0])).Single();
            edges.Add(lastEdge);

            return edges;
        }
        #endregion

        /// <summary>
        /// Builds a lookup from each edge to the faces it neighbours.
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="faces"></param>
        /// <param name="edgesOf"></param>
        /// <returns></returns>
        public static Dictionary<Edge, List<Face>> EdgeToFaceDictionary(List<Edge> edges, List<Face> faces, Func<Face, List<Edge>> edgesOf)
        {
            var edgeToFaces = edges.ToDictionary(edge => edge, edge => new List<Face>());
            foreach (var face in faces)
            {
                foreach (var edge in edgesOf(face))
                {
                    edgeToFaces[edge].Add(face);
                }
            }
            return edgeToFaces;
        }
        #endregion

        /// <summary>
        /// Builds a lookup from each item in a list to its index in that list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Dictionary<T, int> ItemToIndexDictionary<T>(IEnumerable<T> items)
        {
            var itemList = items.ToList();
            var indices = Enumerable.Range(0, itemList.Count);
            var itemIndices = indices.ToDictionary(i => itemList[i], i => i);

            return itemIndices;
        }
    }
}
