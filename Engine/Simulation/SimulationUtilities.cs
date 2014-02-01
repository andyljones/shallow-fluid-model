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
        public static double[] FaceAreasTable(IPolyhedron surface)
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
        public static int[][] FaceNeighboursTable(IPolyhedron surface)
        {
            var neighbours = new int[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var indicesOfNeighbours = surface.NeighboursOf(face).Select(neighbour => surface.IndexOf(neighbour));
                neighbours[surface.IndexOf(face)] = indicesOfNeighbours.ToArray();
            }

            return neighbours;
        }

        public static Vector[] FaceNormalsTable(IPolyhedron surface)
        {
            var normals = new Vector[surface.Faces.Count];
            foreach (var face in surface.Faces)
            {
                normals[surface.IndexOf(face)] = face.SphericalCenter().Normalize();
            }

            return normals;
        }

        public static Vector[][] InterfaceDirectionsTable(IPolyhedron surface)
        {
            var directions = new Vector[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var neighboursOfFace = surface.NeighboursOf(face);
                var localDirections = neighboursOfFace.Select(neighbour => InterfaceDirection(face, neighbour));
                directions[surface.IndexOf(face)] = localDirections.ToArray();
            }

            return directions;
        }

        private static Vector InterfaceDirection(Face from, Face to)
        {
            return VectorUtilities.LocalDirection(from.SphericalCenter(), to.SphericalCenter());
        }

        public static ScalarField<Face> CoriolisField(IPolyhedron surface, double rotationFrequency)
        {
            var normals = FaceNormalsTable(surface);
            var angularVelocity = 2*Math.PI*rotationFrequency;

            var values = new double[surface.Faces.Count];
            foreach (var face in surface.Faces)
            {
                var faceIndex = surface.IndexOf(face);
                values[faceIndex] = 2*angularVelocity*normals[faceIndex][2];
            }

            return new ScalarField<Face>(surface.IndexOf, values);
        }

        public static int[][] VertexNeighboursTable(IPolyhedron surface)
        {
            var neighbours = new int[surface.Vertices.Count][];
            foreach (var vertex in surface.Vertices)
            {
                var indicesOfNeighbours = surface.NeighboursOf(vertex).Select(neighbour => surface.IndexOf(neighbour));
                neighbours[surface.IndexOf(vertex)] = indicesOfNeighbours.ToArray();
            }

            return neighbours;
        }

        public static Vector[][] EdgeNormalsTable(IPolyhedron surface)
        {
            var edgeNormalsTable = new Vector[surface.Vertices.Count][];
            foreach (var vertex in surface.Vertices)
            {
                var vertexVector = vertex.Position;
                var edgeVectors = surface.NeighboursOf(vertex).Select(neighbour => (neighbour.Position - vertexVector));
                var edgeNormals = edgeVectors.Select(vector => Vector.CrossProduct(vertexVector, vector).Normalize()).ToArray();
                edgeNormalsTable[surface.IndexOf(vertex)] = edgeNormals;
            }

            return edgeNormalsTable;
        }

        public static double[][] HalfEdgeLengthsTable(IPolyhedron surface)
        {
            var halfLengthsTable = new double[surface.Vertices.Count][];
            foreach (var vertex in surface.Vertices)
            {
                var edges = surface.EdgesOf(vertex);
                var lengths = new List<double>();
                foreach (var edge in edges)
                {
                    var neighbour = edge.Vertices().First(v => v != vertex);
                    var center = surface.FacesOf(edge).First().SphericalCenter();
                    var length = Vector.ScalarProduct(center - vertex.Position, (neighbour.Position - vertex.Position).Normalize());
                    lengths.Add(length);
                }
                halfLengthsTable[surface.IndexOf(vertex)] = lengths.ToArray();
            }

            return halfLengthsTable;
        }

        public static double[] VertexAreasTable(IPolyhedron surface)
        {
            var areas = new double[surface.Vertices.Count];
            foreach (var vertex in surface.Vertices)
            {
                var centers = surface.FacesOf(vertex).Select(face => face.SphericalCenter()).ToList();
                var area = Vector.ScalarProduct(Vector.CrossProduct(centers[0] - centers[1], centers[2] - centers[1]), vertex.Position.Normalize());
                areas[surface.IndexOf(vertex)] = area;
            }

            return areas;
        }

        public static double[][] VertexDistanceTable(IPolyhedron surface)
        {
            var distanceTable = new double[surface.Vertices.Count][];
            foreach (var vertex in surface.Vertices)
            {
                var edges = surface.EdgesOf(vertex);
                var distances = edges.Select(edge => edge.Length()).ToArray();
                distanceTable[surface.IndexOf(vertex)] = distances;
            }

            return distanceTable;


        }
}
