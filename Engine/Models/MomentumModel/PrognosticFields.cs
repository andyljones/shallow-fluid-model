using Engine.Polyhedra;

namespace Engine.Models.MomentumModel
{
    public class PrognosticFields
    {
        public VectorField<Vertex> Velocity;
        public ScalarField<Face> Height;
    }
}
