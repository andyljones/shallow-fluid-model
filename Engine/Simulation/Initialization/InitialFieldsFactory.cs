using Engine.Geometry;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation.Initialization
{
    using Vector = Vector<double>;

    public static class InitialFieldsFactory
    {
        public static PrognosticFields Build(IPolyhedron polyhedron, IInitialFieldParameters parameters)
        {
            var height = parameters.InitialHeightFunction(polyhedron, parameters.InitialAverageHeight, parameters.InitialMaxDeviationOfHeight);
            var velocity = parameters.InitialVelocityFunction(polyhedron, parameters.InitialAverageVelocity, parameters.InitialMaxDeviationOfVelocity);

            var zeroVector = Vector.Build.Dense(3);

            var fields = new PrognosticFields
            {
                DerivativeOfHeight = ScalarFieldFactory.ConstantScalarField(polyhedron, 0, 0),
                DerivativeOfVelocity = VectorFieldFactory.ConstantVectorField(polyhedron, zeroVector, 0),
                Height = height,
                Velocity = velocity,
            };

            return fields;
        }
    }
}
