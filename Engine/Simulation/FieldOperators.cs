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

        private readonly int _numberOfFaces;

        #region Constructor methods
        public FieldOperators(Polyhedron surface, Dictionary<Face, int> index)
        {
            _areas = BuildAreasTable(surface, index);
            _neighbours = BuildNeighboursTable(surface, index);
            _edgeLengths = BuildEdgeLengthsTable(surface, index);
            _distances = BuildDistancesTable(surface, index);
            _numberOfFaces = surface.Faces.Count;
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
        #endregion

        #region Jacobian methods
        public ScalarField<Face> Jacobian(ScalarField<Face> A, ScalarField<Face> B)
        {
            var results = new double[_numberOfFaces];
            foreach (var face in Enumerable.Range(0, _numberOfFaces))
            {
                results[face] = JacobianAtFace(face, A, B);
            }
            return new ScalarField<Face>(A.Index, results);
        }

        private double JacobianAtFace(int face, ScalarField<Face> A, ScalarField<Face> B)
        {
            var neighbours = _neighbours[face];

            var firstNeighbour = neighbours[0];
            var secondNeighbour = neighbours[1];
            var secondToLastNeighbour = neighbours[neighbours.Length - 2];
            var lastNeighbour = neighbours[neighbours.Length - 1];

            var result = 0.0;
            result += (A[firstNeighbour] + A[face]) * (B[secondNeighbour] - B[lastNeighbour]);
            for (int j = 1; j < neighbours.Length - 1; j++)
            {
                var previousNeighbour = neighbours[j - 1];
                var currentNeighbour = neighbours[j];
                var nextNeighbour = neighbours[j + 1];

                result += (A[face] + A[currentNeighbour]) * (B[nextNeighbour] - B[previousNeighbour]);
            }
            result += (A[lastNeighbour] + A[face]) * (B[firstNeighbour] - B[secondToLastNeighbour]);

            return result / (6 * _areas[face]);
        }
        #endregion

        #region FluxDivergence methods
        public ScalarField<Face> FluxDivergence(ScalarField<Face> A, ScalarField<Face> B)
        {
            var results = new double[_numberOfFaces];
            foreach (var face in Enumerable.Range(0, _numberOfFaces))
            {
                results[face] = FluxDivergenceAtFace(face, A, B);
            }
            return new ScalarField<Face>(A.Index, results);
        }

        private double FluxDivergenceAtFace(int face, ScalarField<Face> A, ScalarField<Face> B)
        {
            var neighbours = _neighbours[face];
            var edgeLengths = _edgeLengths[face];
            var distances = _distances[face];

            var result = 0.0;
            for (int j = 0; j < neighbours.Length; j++)
            {
                var currentNeighbour = neighbours[j];
                var edgeLength = edgeLengths[j];
                var distance = distances[j];

                result += edgeLength/distance * (A[face] + A[currentNeighbour])*(B[currentNeighbour] - B[face]);
            }

            return result / (2 * _areas[face]);
        }
        #endregion

        #region Laplacian methods
        public ScalarField<Face> Laplacian(ScalarField<Face> A)
        {
            var results = new double[_numberOfFaces];
            foreach (var face in Enumerable.Range(0, _numberOfFaces))
            {
                results[face] = LaplacianAtFace(face, A);
            }
            return new ScalarField<Face>(A.Index, results);
        }

        private double LaplacianAtFace(int face, ScalarField<Face> A)
        {
            var neighbours = _neighbours[face];
            var edgeLengths = _edgeLengths[face];
            var distances = _distances[face];

            var result = 0.0;
            for (int j = 0; j < neighbours.Length; j++)
            {
                var currentNeighbour = neighbours[j];
                var edgeLength = edgeLengths[j];
                var distance = distances[j];

                result += edgeLength/distance * (A[currentNeighbour] - A[face]);
            }

            return result / _areas[face];
        }
        #endregion
    }
}
