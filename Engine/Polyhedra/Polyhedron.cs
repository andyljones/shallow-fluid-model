using System;
using System.Collections.Generic;
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
        public List<Edge> EdgesOf(Vertex vertex) { return _vertexToEdges[vertex]; }
        private readonly Dictionary<Vertex, List<Edge>> _vertexToEdges;

        public List<Edge> EdgesOf(Face face) { return _faceToEdges[face]; }
        private readonly Dictionary<Face, List<Edge>> _faceToEdges;

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
        public Polyhedron(IEnumerable<IEnumerable<Vertex>> verticesInEachFace)
        {
            _vertices = PolyhedronInitialization.Vertices(verticesInEachFace);
            _faces = PolyhedronInitialization.Faces(verticesInEachFace);
            _edges = PolyhedronInitialization.Edges(Faces);

            _vertexToEdges = PolyhedronInitialization.VertexToEdgeDictionary(Vertices, Edges);
            _vertexToFaces = PolyhedronInitialization.VertexToFaceDictionary(Vertices, Faces);
            _faceToEdges = PolyhedronInitialization.FaceToEdgeDictionary(Faces, EdgesOf);
            _edgeToFaces = PolyhedronInitialization.EdgeToFaceDictionary(Edges, Faces, EdgesOf);

            _faceIndices = PolyhedronInitialization.ItemToIndexDictionary(Faces);
            _edgeIndices = PolyhedronInitialization.ItemToIndexDictionary(Edges);
            _vertexIndices = PolyhedronInitialization.ItemToIndexDictionary(Vertices);
        }
    }
}
