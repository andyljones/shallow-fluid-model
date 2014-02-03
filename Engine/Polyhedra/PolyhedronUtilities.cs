using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Polyhedra
{
    /// <summary>
    /// Extension methods for the Polyhedron class
    /// </summary>
    public static class PolyhedronUtilities
    {
        /// <summary>
        /// Returns the neighbours of a face in the same order that the edges they have in common appear.
        /// </summary>
        public static IEnumerable<Face> NeighboursOf(this IPolyhedron polyhedron, Face face)
        {
            var faceSingleton = new[] {face};

            var edges = polyhedron.EdgesOf(face);
            var neighbours = edges.SelectMany(edge => polyhedron.FacesOf(edge).Except(faceSingleton));

            return neighbours;
        }

        public static IEnumerable<Vertex> NeighboursOf(this IPolyhedron polyhedron, Vertex vertex)
        {
            var vertexSingleton = new[] {vertex};

            var edges = polyhedron.EdgesOf(vertex);
            var neighbours = edges.SelectMany(edge => edge.Vertices().Except(vertexSingleton));

            return neighbours.Where(neighbour => neighbour != vertex);
        }


        //TODO: test.
        public static Vector BisectionPoint(this IPolyhedron polyhedron, Edge edge)
        {
            var aFace = polyhedron.FacesOf(edge).First();
            var origin = edge.A.Position;

            var edgeVector = edge.B.Position - origin;
            var vectorToFace = aFace.SphericalCenter() - origin;

            var vectorToBisector = Vector.ScalarProduct(vectorToFace, edgeVector.Normalize())*edgeVector.Normalize();

            return vectorToBisector + origin;
        }
    }
}
