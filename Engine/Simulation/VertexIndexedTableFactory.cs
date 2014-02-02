using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation
{
    /// <summary>
    /// Static methods for constructing fast-access tables to geometric information about a surface.
    /// </summary>
    public static class VertexIndexedTableFactory
    {
        /// <summary>
        /// Constructs a table of the neighbours of each vertex.
        /// </summary>
        public static int[][] Neighbours(IPolyhedron surface)
        {
            var neighbours = new int[surface.Vertices.Count][];
            foreach (var vertex in surface.Vertices)
            {
                var indicesOfNeighbours = surface.NeighboursOf(vertex).Select(neighbour => surface.IndexOf(neighbour));
                neighbours[surface.IndexOf(vertex)] = indicesOfNeighbours.ToArray();
            }

            return neighbours;
        }

        /// <summary>
        /// Constructs a table of the vectors perpendicular to a vertex and normal to each adjoining edge.
        /// 
        /// Normals point anticlockwise around the vertex.
        /// </summary>
        public static Vector[][] EdgeNormals(IPolyhedron surface)
        {
            var edgeNormalsTable = new Vector[surface.Vertices.Count][];
            foreach (var vertex in surface.Vertices)
            {
                var vertexVector = vertex.Position;
                var edgeVectors = surface.NeighboursOf(vertex).Select(neighbour => (neighbour.Position - vertexVector));
                var edgeNormals = edgeVectors.Select(edgeVector => Vector.CrossProduct(vertexVector, edgeVector).Normalize()).ToArray();
                edgeNormalsTable[surface.IndexOf(vertex)] = edgeNormals;
            }

            return edgeNormalsTable;
        }

        /// <summary>
        /// Constructs a table of the distances from each vertex to the bisectors running across each neighbouring edge.
        /// </summary>
        public static double[][] BisectorDistances(IPolyhedron surface)
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

        /// <summary>
        /// Constructs a table of the spherical areas associated with each vertex. 
        /// 
        /// Only valid for degree-3 vertices.
        /// </summary>
        public static double[] Areas(IPolyhedron surface)
        {
            //TODO: Does not actually calculate spherical areas.
            var areas = new double[surface.Vertices.Count];
            foreach (var vertex in surface.Vertices)
            {
                var centers = surface.FacesOf(vertex).Select(face => face.SphericalCenter()).ToList();
                var crossProduct = Vector.CrossProduct(centers[0] - centers[1], centers[2] - centers[1]);
                var area = crossProduct.Norm()/2;
                areas[surface.IndexOf(vertex)] = area;
            }

            return areas;
        }

        /// <summary>
        /// Constructs a table of the spherical distances from each vertex to its neighbours.
        /// </summary>
        public static double[][] Distances(IPolyhedron surface)
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

        /// <summary>
        /// Constructs a table of the faces around each vertex.
        /// 
        /// The ith edge given by surface.EdgesOf is anticlockwise of the ith face. 
        /// </summary>
        public static int[][] Faces(IPolyhedron surface)
        {
            var faceTable = new int[surface.Vertices.Count][];
            foreach (var vertex in surface.Vertices)
            {
                faceTable[surface.IndexOf(vertex)] = FacesAroundVertex(vertex, surface);
            }

            return faceTable;
        }

        private static int[] FacesAroundVertex(Vertex vertex, IPolyhedron surface)
        {
            var edges = surface.EdgesOf(vertex);

            var faces = new List<int>();
            for (int i = 0; i < edges.Count; i++)
            {
                var previousEdge = edges.AtCyclicIndex(i-1);
                var thisEdge = edges[i];
                var faceInCommon = surface.FacesOf(previousEdge).Intersect(surface.FacesOf(thisEdge)).First();
                var indexOfFace = surface.IndexOf(faceInCommon);
                faces.Add(indexOfFace);
            }

            return faces.ToArray();
        }

        public static Vector[] Normals(IPolyhedron surface)
        {
            var normals = new Vector[surface.Vertices.Count];
            foreach (var vertex in surface.Vertices)
            {
                var normal = vertex.Position.Normalize();
                normals[surface.IndexOf(vertex)] = normal;
            }

            return normals;
        }
    }
}
