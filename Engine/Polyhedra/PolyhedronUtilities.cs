using System.Collections.Generic;
using System.Linq;

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
            var edges = polyhedron.EdgesOf(face);
            var neighbours = edges.SelectMany(edge => polyhedron.FacesOf(edge));

            return neighbours.Where(neighbour => neighbour != face);
        }
    }
}
