using System.Collections.Generic;
using System.Linq;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Geometry
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

        /// <summary>
        /// Returns the neighbours of a vertex in the same order that their connecting edges are listed by the 
        /// polyhedron's vertexToEdge lookup.
        /// </summary>
        /// <param name="polyhedron"></param>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public static IEnumerable<Vertex> NeighboursOf(this IPolyhedron polyhedron, Vertex vertex)
        {
            var vertexSingleton = new[] {vertex};

            var edges = polyhedron.EdgesOf(vertex);
            var neighbours = edges.SelectMany(edge => edge.Vertices().Except(vertexSingleton));

            return neighbours.Where(neighbour => neighbour != vertex);
        }

        /// <summary>
        /// Calculates the point at which the specified edge is closest to the line between its neighbouring faces' centers.
        /// </summary>
        public static Vector BisectionPoint(IPolyhedron polyhedron, Edge edge)
        {
            var aFace = polyhedron.FacesOf(edge).First();
            var origin = edge.A.Position;

            var edgeVector = edge.B.Position - origin;
            var vectorToFace = aFace.SphericalCenter() - origin;

            var vectorToBisector = Vector.ScalarProduct(vectorToFace, edgeVector.Normalize())*edgeVector.Normalize();

            return vectorToBisector + origin;
        }

        /// <summary>
        /// Calculates the size of the area that's both in the specified face and closer to the specified vertex than any other vertex.
        /// </summary>
        public static double AreaSharedByVertexAndFace(IPolyhedron surface, Vertex vertex, Face face)
        {
            var vertexPosition = vertex.Position;
            var faces = surface.FacesOf(vertex);
            var edges = surface.EdgesOf(vertex);
            var index = faces.IndexOf(face);

            if (!faces.Contains(face))
            {
                return 0.0;
            }

            var midpointOfFace = face.Center();

            var previousEdge = edges.AtCyclicIndex(index - 1);
            var midpointOfPreviousEdge = BisectionPoint(surface, previousEdge);

            var nextEdge = edges.AtCyclicIndex(index);
            var midpointOfNextEdge = BisectionPoint(surface, nextEdge);

            var crossProductOfFirstSegment = Vector.CrossProduct(midpointOfPreviousEdge - vertexPosition, midpointOfFace - vertexPosition);
            var areaOfFirstSegment = Vector.ScalarProduct(crossProductOfFirstSegment, midpointOfFace.Normalize()) / 2;

            var crossProductOfSecondSegment = Vector.CrossProduct(midpointOfFace - vertexPosition, midpointOfNextEdge - vertexPosition);
            var areaOfSecondSegment = Vector.ScalarProduct(crossProductOfSecondSegment, midpointOfFace.Normalize()) / 2;

            return areaOfFirstSegment + areaOfSecondSegment;
        }
    }
}
