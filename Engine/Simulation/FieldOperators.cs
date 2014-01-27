using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;
using Engine.Utilities;

namespace Engine.Simulation
{
    public class FieldOperators
    {
        private readonly double[] _areas;
        private readonly int[][] _neighbours;
        private readonly double[][] _edgeLengths;
        private readonly double[][] _distances;

        public FieldOperators(Polyhedron surface, Dictionary<Face, int> index)
        {
            _areas = BuildAreasTable(surface, index);
            _neighbours = BuildNeighboursTable(surface, index);
            _edgeLengths = BuildEdgeLengthsTable(surface, index);
            _distances = BuildDistancesTable(surface, index);
        }

        private double[][] BuildDistancesTable(Polyhedron surface, Dictionary<Face, int> index)
        {
            var edgeLengths = new double[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var distances = 
                    surface.
                    NeighboursOf(face).
                    Select(neighbour => VectorUtilities.GeodesicDistance(face.Center(), neighbour.Center())).
                    ToArray();
                edgeLengths[index[face]] = distances;
            }

            return edgeLengths;
        }

        private double[][] BuildEdgeLengthsTable(Polyhedron surface, Dictionary<Face, int> index)
        {
            var edgeLengths = new double[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var lengths = surface.FaceToEdges[face].Select(edge => edge.Length()).ToArray();
                edgeLengths[index[face]] = lengths;
            }

            return edgeLengths;
        }

        private double[] BuildAreasTable(Polyhedron surface, Dictionary<Face, int> index)
        {
            var areas = new double[surface.Faces.Count];
            foreach (var face in surface.Faces)
            {
                areas[index[face]] = face.Area();
            }

            return areas;
        }

        private static int[][] BuildNeighboursTable(Polyhedron surface, Dictionary<Face, int> index)
        {
            var neighbours = new int[surface.Faces.Count][];
            foreach (var face in surface.Faces)
            {
                var indicesOfNeighbours = surface.NeighboursOf(face).Select(neighbour => index[neighbour]).ToArray();
                neighbours[index[face]] = indicesOfNeighbours;
            }

            return neighbours;
        }

        public ScalarField<Face> Jacobian(ScalarField<Face> a, ScalarField<Face> b)
        {
            throw new NotImplementedException();
        }
        
        public ScalarField<Face> FluxDivergence(ScalarField<Face> a, ScalarField<Face> b)
        {
            throw new NotImplementedException();
        }

        public ScalarField<Face> Laplacian(ScalarField<Face> a)
        {
            throw new NotImplementedException();
        }
    }
}
