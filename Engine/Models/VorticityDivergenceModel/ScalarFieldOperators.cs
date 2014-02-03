﻿using System.Linq;
using Engine.Polyhedra;
using Engine.Utilities;

namespace Engine.Models.VorticityDivergenceModel
{
    /// <summary>
    /// Difference functions for use in the simulation.
    /// </summary>
    public class ScalarFieldOperators
    {
        private readonly double[] _areas;
        private readonly int[][] _neighbours;
        private readonly double[][] _edgeLengths;
        private readonly double[][] _distances;

        private readonly int _numberOfFaces;

        /// <summary>
        /// Constructs a set of difference functions for use over the given surface, with faces indexed by Index. 
        /// </summary>
        public ScalarFieldOperators(IPolyhedron surface)
        {
            _areas =       FaceIndexedTableFactory.Areas(surface);
            _neighbours =  FaceIndexedTableFactory.Neighbours(surface);
            _edgeLengths = FaceIndexedTableFactory.EdgeLengths(surface);
            _distances =   FaceIndexedTableFactory.Distances(surface);
            _numberOfFaces = surface.Faces.Count;
        }

        #region Jacobian methods
        /// <summary>
        /// The discrete Jacobian, as described in Heikes & Randall 1995.
        /// </summary>
        public ScalarField<Face> Jacobian(ScalarField<Face> A, ScalarField<Face> B)
        {
            var results = new double[_numberOfFaces];
            foreach (var face in Enumerable.Range(0, _numberOfFaces))
            {
                results[face] = JacobianAtFace(face, A, B);
            }
            return new ScalarField<Face>(A.IndexOf, results);
        }

        private double JacobianAtFace(int face, ScalarField<Face> A, ScalarField<Face> B)
        {
            var neighbours = _neighbours[face];

            var result = 0.0;
            for (int j = 0; j < neighbours.Length; j++)
            {
                var previousNeighbour = neighbours.AtCyclicIndex(j - 1);
                var currentNeighbour = neighbours[j];
                var nextNeighbour = neighbours.AtCyclicIndex(j + 1);

                result += (A[face] + A[currentNeighbour]) * (B[previousNeighbour] - B[nextNeighbour]);
            }

            return result / (6 * _areas[face]);
        }
        #endregion

        #region FluxDivergence methods
        /// <summary>
        /// The discrete flux divergence, as described in Heikes & Randall 1995.
        /// </summary>
        public ScalarField<Face> FluxDivergence(ScalarField<Face> A, ScalarField<Face> B)
        {
            var results = new double[_numberOfFaces];
            foreach (var face in Enumerable.Range(0, _numberOfFaces))
            {
                results[face] = FluxDivergenceAtFace(face, A, B);
            }
            return new ScalarField<Face>(A.IndexOf, results);
        }

        private double FluxDivergenceAtFace(int face, ScalarField<Face> A, ScalarField<Face> B)
        {
            var neighbours = _neighbours[face];
            var edgeLengths = _edgeLengths[face];
            var distances = _distances[face];

            var result = 0.0;
            for (int j = 0; j < neighbours.Length; j++)
            {
                var neighbour = neighbours[j];
                var edgeLength = edgeLengths[j];
                var distance = distances[j];

                result += edgeLength/distance * (A[face] + A[neighbour])*(B[neighbour] - B[face]);
            }

            return result / (2 * _areas[face]);
        }
        #endregion

        #region Laplacian methods
        /// <summary>
        /// The discrete Laplacian, as described in Heikes & Randall 1995.
        /// </summary>
        public ScalarField<Face> Laplacian(ScalarField<Face> A)
        {
            var results = new double[_numberOfFaces];
            foreach (var face in Enumerable.Range(0, _numberOfFaces))
            {
                results[face] = LaplacianAtFace(face, A);
            }
            return new ScalarField<Face>(A.IndexOf, results);
        }

        private double LaplacianAtFace(int face, ScalarField<Face> A)
        {
            var neighbours = _neighbours[face];
            var edgeLengths = _edgeLengths[face];
            var distances = _distances[face];

            var result = 0.0;
            for (int j = 0; j < neighbours.Length; j++)
            {
                var neighbour = neighbours[j];
                var edgeLength = edgeLengths[j];
                var distance = distances[j];

                result += edgeLength/distance * (A[neighbour] - A[face]);
            }

            return result / _areas[face];
        }
        #endregion
    }
}