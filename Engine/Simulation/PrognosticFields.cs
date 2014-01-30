using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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

        private static readonly FieldInfo[] _scalarFieldVariables = NumericFields();

        private static FieldInfo[] NumericFields()
        {
            var allFields = typeof(PrognosticFields<T>).GetFields();
            var numericFields = allFields.Where(field => field.FieldType == typeof (ScalarField<T>)).ToArray();

            return numericFields;
        }

        public string ToString(int index)
        {
            var result = new StringBuilder();
            foreach (var scalarFieldVariable in _scalarFieldVariables)
            {
                var name = scalarFieldVariable.Name;
                var scalarField = scalarFieldVariable.GetValue(this) as ScalarField<T>;
                var stringForField = String.Format("{0}: {1,3:N2}\n", name, scalarField[index]);
                result.Append(stringForField);
            }

            return result.ToString();
        }
    }
}
