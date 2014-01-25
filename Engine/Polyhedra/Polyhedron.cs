using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ReadOnlyCollection<Face> Faces { get; private set; }
        public ReadOnlyCollection<Edge> Edges { get; private set; } 
        public ReadOnlyCollection<Vertex> Vertices { get; private set; } 

        /// <summary>
        /// Construct a polyhedron from a collection of convex, planar collections of vertices.
        /// </summary>
        public Polyhedron(IEnumerable<IEnumerable<Vertex>> faces)
        {
            Faces = InitializeFaces(faces);
            Edges = InitializeEdges(Faces);
            Vertices = InitializeVertices(Faces);
        }

        #region InitializeFaces methods
        private static ReadOnlyCollection<Face> InitializeFaces(IEnumerable<IEnumerable<Vertex>> vertexCollections)
        {
            var faces = vertexCollections.Select(vertices => new Face(SortVertices(vertices))).ToList();

            return new ReadOnlyCollection<Face>(faces);
        }

        private static IEnumerable<Vertex> SortVertices(IEnumerable<Vertex> vertices)
        {
            var vertexList = vertices.ToList();
            var normalizedCentroid = vertexList.Aggregate(Vector.Zeros(3), (c, v) => c + v.Position).Normalize();

            if (normalizedCentroid == Vector.Zeros(3))
            {
                Debug.WriteLine("Centroid of face was the zero vector! Picking an arbitrary centroid instead");
                normalizedCentroid = new Vector(new[] { 0, 0, 1.0 });
            }

            var sortedVertices = vertexList.OrderBy(vertex => vertex.Position, new ClockwiseCompare(normalizedCentroid));

            return sortedVertices;
        }
        #endregion

        private ReadOnlyCollection<Vertex> InitializeVertices(IEnumerable<Face> faces)
        {
            var vertices = faces.SelectMany(face => face.Vertices).Distinct().ToList();

            return new ReadOnlyCollection<Vertex>(vertices);
        }

        private ReadOnlyCollection<Edge> InitializeEdges(IEnumerable<Face> faces)
        {
            var edges = faces.SelectMany(face => face.Edges()).Distinct().ToList();

            return new ReadOnlyCollection<Edge>(edges);
        }
    }
}
