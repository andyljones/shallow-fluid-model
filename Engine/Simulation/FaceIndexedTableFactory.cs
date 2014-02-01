using System.Linq;
using Engine.Polyhedra;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation
{
    /// <summary>
    /// Static methods for constructing fast-access tables to geometric information about a surface.
    /// </summary>
    public static class FaceIndexedTableFactory
    {
        /// <summary>
        /// Constructs a table of the distances from each face's center to it's neighbour's centers. 
        /// Neighbours are listed in the same order as given by surface.NeighboursOf.
        /// </summary>
        public static double[][] Distances(IPolyhedron surface)
        {
            var edgeLengths = new double[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var distances =
                    surface.
                    NeighboursOf(face).
                    Select(neighbour => VectorUtilities.GeodesicDistance(face.SphericalCenter(), neighbour.SphericalCenter()));
                edgeLengths[surface.IndexOf(face)] = distances.ToArray();
            }

            //TODO: Work out how to turn this into spherical area.

            return edgeLengths;
        }

        /// <summary>
        /// Constructs a table of the lengths of edges surrounding each face. 
        /// Edges are listed in the same order as the opposing faces are given by surface.NeighboursOf.
        /// </summary>
        public static double[][] EdgeLengths(IPolyhedron surface)
        {
            var edgeLengths = new double[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var lengths = surface.EdgesOf(face).Select(edge => edge.Length());
                edgeLengths[surface.IndexOf(face)] = lengths.ToArray();
            }

            return edgeLengths;
        }

        /// <summary>
        /// Constructs a table of the areas of the faces.
        /// </summary>
        public static double[] Areas(IPolyhedron surface)
        {
            var areas = new double[surface.Faces.Count];
            foreach (var face in surface.Faces)
            {
                areas[surface.IndexOf(face)] = face.Area();
            }

            return areas;
        }

        /// <summary>
        /// Constructs a table of the neighbours of each face. 
        /// Neighbours are listed in the same order as given by surface.NeighboursOf.
        /// </summary>
        public static int[][] Neighbours(IPolyhedron surface)
        {
            var neighbours = new int[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var indicesOfNeighbours = surface.NeighboursOf(face).Select(neighbour => surface.IndexOf(neighbour));
                neighbours[surface.IndexOf(face)] = indicesOfNeighbours.ToArray();
            }

            return neighbours;
        }

        /// <summary>
        /// Constructs a table of the normals to the sphere at the center of each face.
        /// </summary>
        public static Vector[] Normals(IPolyhedron surface)
        {
            var normals = new Vector[surface.Faces.Count];
            foreach (var face in surface.Faces)
            {
                normals[surface.IndexOf(face)] = face.SphericalCenter().Normalize();
            }

            return normals;
        }

        #region Directions methods.
        /// <summary>
        /// Constructs a table of the directions towards the faces neighbours in the face's tangent space.
        /// </summary>
        public static Vector[][] Directions(IPolyhedron surface)
        {
            var directions = new Vector[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var neighboursOfFace = surface.NeighboursOf(face);
                var localDirections = neighboursOfFace.Select(neighbour => Direction(face, neighbour));
                directions[surface.IndexOf(face)] = localDirections.ToArray();
            }

            return directions;
        }

        private static Vector Direction(Face from, Face to)
        {
            return VectorUtilities.LocalDirection(from.SphericalCenter(), to.SphericalCenter());
        }
        #endregion
    }
}
