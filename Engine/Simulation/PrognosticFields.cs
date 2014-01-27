namespace Engine.Simulation
{
    public class PrognosticFields<T>
    {
        public ScalarField<T> DerivativeOfAbsoluteVorticity;
        public ScalarField<T> DerivativeOfDivergence;
        public ScalarField<T> DerivativeOfHeight;

        public ScalarField<T> AbsoluteVorticity;
        public ScalarField<T> Divergence;
        public ScalarField<T> Height;
        public ScalarField<T> VelocityPotential;
        public ScalarField<T> Streamfunction;
    }
}
