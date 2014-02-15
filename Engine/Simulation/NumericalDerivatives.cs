namespace Engine.Simulation
{
    public static class NumericalDerivatives
    {
        public static ScalarField<T> AdamsBashforth<T>
            (double timestep, ScalarField<T> field, ScalarField<T> derivative, ScalarField<T> oldDerivative, ScalarField<T> olderDerivative)
        {
            var step = 1.0 / 12.0 * (23 * derivative - 16 * oldDerivative + 5 * olderDerivative);
            var newField = field + timestep * step;

            return newField;
        }

        public static VectorField<T> AdamsBashforth<T>
    (double timestep, VectorField<T> field, VectorField<T> derivative, VectorField<T> oldDerivative, VectorField<T> olderDerivative)
        {
            var step = 1.0 / 12.0 * (23 * derivative - 16 * oldDerivative + 5 * olderDerivative);
            var newField = field + timestep * step;

            return newField;
        }

        public static ScalarField<T> Euler<T>(double timestep, ScalarField<T> field, ScalarField<T> derivative)
        {
            var step = derivative;
            var newField = field + timestep * step;

            return newField;
        }

        public static VectorField<T> Euler<T>(double timestep, VectorField<T> field, VectorField<T> derivative)
        {
            var step = derivative;
            var newField = field + timestep * step;

            return newField;
        }
    }
}
