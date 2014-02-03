using Engine.Polyhedra;

namespace Engine.Models
{
    public static class NumericalDerivatives
    {
        public static ScalarField<Face> AdamsBashforth
            (double timestep, ScalarField<Face> field, ScalarField<Face> derivative, ScalarField<Face> oldDerivative, ScalarField<Face> olderDerivative)
        {
            var step = 1.0 / 12.0 * (23 * derivative - 16 * oldDerivative + 5 * olderDerivative);
            var newField = field + timestep * step;

            return newField;
        }

        public static ScalarField<Face> Euler(double timestep, ScalarField<Face> field, ScalarField<Face> derivative)
        {
            var step = derivative;
            var newField = field + timestep * step;

            return newField;
        }
    }
}
