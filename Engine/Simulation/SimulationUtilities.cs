using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation
{
    /// <summary>
    /// A set of static utility methods for use by simulation classes.
    /// </summary>
    public static class SimulationUtilities
    {
        /// <summary>
        /// Constructs a table of the distances from each face's center to it's neighbour's centers. 
        /// Neighbours are listed in the same order as given by surface.NeighboursOf.
        /// </summary>
        public static double[][] DistancesTable(IPolyhedron surface)
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
        public static double[][] EdgeLengthsTable(IPolyhedron surface)
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
        public static double[] AreasTable(IPolyhedron surface)
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
        public static int[][] NeighboursTable(IPolyhedron surface)
        {
            var neighbours = new int[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var indicesOfNeighbours = surface.NeighboursOf(face).Select(neighbour => surface.IndexOf(neighbour));
                neighbours[surface.IndexOf(face)] = indicesOfNeighbours.ToArray();
            }

            return neighbours;
        }

        public static Vector[] NormalsTable(IPolyhedron surface)
        {
            var normals = new Vector[surface.Faces.Count];
            foreach (var face in surface.Faces)
            {
                normals[surface.IndexOf(face)] = face.SphericalCenter().Normalize();
            }

            return normals;
        }

        public static Vector[][] DirectionTable(IPolyhedron surface)
        {
            var neighbours = NeighboursTable(surface);

            var directions = new Vector[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var neighboursOfFace = neighbours[surface.IndexOf(face)];
                var localDirections = neighboursOfFace.Select(neighbour => Direction(face, surface.Faces[neighbour]));
                directions[surface.IndexOf(face)] = localDirections.ToArray();
            }

            return directions;
        }

        private static Vector Direction(Face from, Face to)
        {
            return VectorUtilities.LocalDirection(from.SphericalCenter(), to.SphericalCenter());
        }

        public static ScalarField<Face> CoriolisField(IPolyhedron surface, double rotationPeriod)
        {
            var normals = NormalsTable(surface);
            var angularVelocity = 2*Math.PI/rotationPeriod;

            var values = new double[surface.Faces.Count];
            foreach (var face in surface.Faces)
            {
                var faceIndex = surface.IndexOf(face);
                values[faceIndex] = 2*angularVelocity*Math.Cos(normals[faceIndex][2]);
            }

            return new ScalarField<Face>(surface.IndexOf, values);
        }
    }
}
