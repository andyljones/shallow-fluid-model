using Engine.Geometry;

namespace Engine.Simulation
{
    /// <summary>
    /// Datatype for holding the current state of the simulation.
    /// </summary>
    public class PrognosticFields
    {
        public VectorField<Vertex> DerivativeOfVelocity;
        public ScalarField<Face> DerivativeOfHeight; 

        public VectorField<Vertex> Velocity;
        public ScalarField<Face> Height;
    }
}
