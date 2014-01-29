using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation
{
    public class VelocityFieldFactory
    {
        private readonly int[][] _neighbours;
        private readonly Vector[][] _directions;
        private readonly double[][] _distances;

        private readonly VectorField<Face> _centers;

        public VelocityFieldFactory(IPolyhedron polyhedron)
        {
            _neighbours = SimulationUtilities.NeighboursTable(polyhedron);
            _directions = SimulationUtilities.DirectionTable(polyhedron);
            _distances = SimulationUtilities.DistancesTable(polyhedron);

            _centers = new VectorField<Face>(polyhedron.IndexOf, SimulationUtilities.NormalsTable(polyhedron));
        }

        public VectorField<Face> VelocityField(ScalarField<Face> streamfunction, ScalarField<Face> vectorPotential)
        {
            var velocity = VectorField<Face>.CrossProduct(_centers, Gradient(streamfunction)) + Gradient(vectorPotential);

            return velocity;
        }

        private VectorField<Face> Gradient(ScalarField<Face> A)
        {
            var gradients = new Vector[A.Count];
            foreach (var face in Enumerable.Range(0, A.Count))
            {
                gradients[face] = GradientAtFace(face, A);
            }

            return new VectorField<Face>(A.IndexOf, gradients);
        }

        private Vector GradientAtFace(int face, ScalarField<Face> A)
        {
            var neighbours = _neighbours[face];
            var directions = _directions[face];
            var distances = _distances[face];

            var gradient = Vector.Zeros(3);
            for (int i = 0; i < neighbours.Length; i++)
            {
                var neighbour = neighbours[i];
                gradient += (A[neighbour] - A[face])/distances[neighbour]*directions[neighbour];
            }

            return gradient/neighbours.Length;
        }
    }
}
