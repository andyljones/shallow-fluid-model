using System.Collections.Generic;

namespace Engine.Geometry
{
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
        /// <summary>
        /// The edges of each vertex, ordered so that the ith edge in EdgesOf(v) is anticlockwise of the ith face in FacesOf(v)
        /// </summary>
        public List<Edge> EdgesOf(Vertex vertex) { return _vertexToEdges[vertex]; }
        private readonly Dictionary<Vertex, List<Edge>> _vertexToEdges;

        /// <summary>
        /// The edges of each face, ordered so that the ith vertex in VerticesOf(f) is anticlockwise of the ith edge in EdgesOf(v)
        /// </summary>
        public List<Edge> EdgesOf(Face face) { return _faceToEdges[face]; }
        private readonly Dictionary<Face, List<Edge>> _faceToEdges;

        /// <summary>
        /// The faces of each vertex, ordered so that the ith edge in EdgesOf(v) is clockwise of the ith face in FacesOf(v)
        /// </summary>
        public List<Face> FacesOf(Vertex vertex) { return _vertexToFaces[vertex]; } 
        private readonly Dictionary<Vertex, List<Face>> _vertexToFaces;

        public List<Face> FacesOf(Edge edge) { return _edgeToFaces[edge]; } 
        private readonly Dictionary<Edge, List<Face>> _edgeToFaces;
        #endregion

        #region Indexes
        public int IndexOf(Face face) { return _faceIndices[face]; }
        private readonly Dictionary<Face, int> _faceIndices;

        public int IndexOf(Edge edge) { return _edgeIndices[edge]; }
        private readonly Dictionary<Edge, int> _edgeIndices;

        public int IndexOf(Vertex vertex) { return _vertexIndices[vertex]; }
        private readonly Dictionary<Vertex, int> _vertexIndices;
        #endregion

        /// <summary>
        /// Construct a polyhedron from a collection of convex, planar collections of vertices.
        /// </summary>
        public Polyhedron(List<List<Vertex>> verticesInEachFace)
        {
            _vertices = PolyhedronInitialization.Vertices(verticesInEachFace);
            _faces = PolyhedronInitialization.Faces(verticesInEachFace);
            _edges = PolyhedronInitialization.Edges(Faces);

            _vertexToEdges = PolyhedronInitialization.VertexToEdgeDictionary(Vertices, Edges);
            _vertexToFaces = PolyhedronInitialization.VertexToFaceDictionary(Vertices, Faces, _vertexToEdges);
            _faceToEdges = PolyhedronInitialization.FaceToEdgeDictionary(Faces, EdgesOf);
            _edgeToFaces = PolyhedronInitialization.EdgeToFaceDictionary(Edges, Faces, EdgesOf);

            _faceIndices = PolyhedronInitialization.ItemToIndexDictionary(Faces);
            _edgeIndices = PolyhedronInitialization.ItemToIndexDictionary(Edges);
            _vertexIndices = PolyhedronInitialization.ItemToIndexDictionary(Vertices);
        }
    }
}
