using System.Collections.Generic;
using System.Linq;
using Engine.Utilities;
using MathNet.Numerics;
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
        public IEnumerable<Face> Faces { get; private set; }

        /// <summary>
        /// Construct a polyhedron from a collection of convex, planar collections of vertices.
        /// </summary>
        public Polyhedron(IEnumerable<IEnumerable<Vertex>> faces)
        {
            Faces = InitializeFaces(faces);
        }

        private static IEnumerable<Face> InitializeFaces(IEnumerable<IEnumerable<Vertex>> faces)
        {
            return faces.Select(vertices => new Face(SortVertices(vertices)));
        }

        private static IEnumerable<Vertex> SortVertices(IEnumerable<Vertex> vertices)
        {
            var vertexList = vertices.ToList();
            var center = Center(vertexList);
            var sortedVertices = vertexList.OrderBy(vertex => vertex.Position, new ClockwiseCompare(center));

            return sortedVertices;
        }

        private static Vector Center(IEnumerable<Vertex> vertices)
        {
            var center = vertices.Aggregate(Vector.Zeros(3), (c, v) => c + v.Position).Normalize();

            if (center == Vector.Zeros(3))
            {
                center = new Vector(new[] { 0, 0, 1.0 });
            }

            return center;
        }
    }
}
