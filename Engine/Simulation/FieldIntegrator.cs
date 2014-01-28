using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Polyhedra;
using Engine.Utilities;

namespace Engine.Simulation
{

    /// <summary>
    /// Methods for integrating equations of the form Laplacian(U) = f.
    /// </summary>
    public class FieldIntegrator
    {
        private readonly int _numberOfRelaxationIterations;
        private readonly double[] _areas;
        private readonly int[][] _neighbours;
        private readonly double[][] _edgeLengths;
        private readonly double[][] _distances;
        private readonly int _numberOfFaces;

        /// <summary>
        /// Constructs a numerical integrator over the given surface, with faces indexed by Index.
        /// </summary>
        public FieldIntegrator(IPolyhedron surface, Dictionary<Face, int> index, SimulationParameters parameters)
        {
            _numberOfRelaxationIterations = parameters.NumberOfRelaxationIterations;
            _areas = SimulationUtilities.BuildAreasTable(surface, index);
            _neighbours = SimulationUtilities.BuildNeighboursTable(surface, index);
            _edgeLengths = SimulationUtilities.BuildEdgeLengthsTable(surface, index);
            _distances = SimulationUtilities.BuildDistancesTable(surface, index);
            _numberOfFaces = surface.Faces.Count;
        }

        /// <summary>
        /// Approximate numerical integration of Laplacian(U) = f.
        /// </summary>
        public ScalarField<Face> Integrate(ScalarField<Face> U, ScalarField<Face> f)
        {
            foreach (var i in Enumerable.Range(0, _numberOfRelaxationIterations))
            {
                U = Relax(U, f);
            }

            return U;
        }

        private ScalarField<Face> Relax(ScalarField<Face> U, ScalarField<Face> f)
        {
            var results = new double[_numberOfFaces];
            foreach (var face in Enumerable.Range(0, _numberOfFaces))
            {
                results[face] = RelaxationAtFace(face, U, f);
            }
            return new ScalarField<Face>(U.Index, results);
        }

        private double RelaxationAtFace(int face, ScalarField<Face> U, ScalarField<Face> f)
        {
            var neighbours = _neighbours[face];
            var edgeLengths = _edgeLengths[face];
            var distances = _distances[face];

            var numerator = 0.0;
            var denominator = 0.0;
            for (int j = 0; j < neighbours.Length; j++)
            {
                var neighbour = neighbours[j];
                var edgeLength = edgeLengths[j];
                var distance = distances[j];

                numerator += edgeLength/distance*U[neighbour];
                denominator += edgeLength/distance;
            }
            numerator -= _areas[face]*f[face];

            return numerator / denominator;
        }
    }
}
