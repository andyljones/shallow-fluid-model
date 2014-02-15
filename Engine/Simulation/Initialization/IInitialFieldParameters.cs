using System;
using Engine.Geometry;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation.Initialization
{
    public interface IInitialFieldParameters
    {
        Func<IPolyhedron, double, double, ScalarField<Face>> InitialHeightFunction { get; }
        double InitialAverageHeight { get; }
        double InitialMaxDeviationOfHeight { get; }

        Func<IPolyhedron, Vector, double, VectorField<Vertex>> InitialVelocityFunction { get; }
        Vector InitialAverageVelocity { get; }
        double InitialMaxDeviationOfVelocity { get; }
    }
}
