using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation
{
    public static class VertexIndexedTableFactory
    {

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
                    //TODO: This is a flat distance, not a geodesic
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
}
