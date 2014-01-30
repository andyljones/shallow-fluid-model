using System;
using System.Linq;
using Engine.Polyhedra;

namespace Engine.Simulation
{
    public class PrognosticFieldsFactory
    {
        public ScalarField<Face> DerivativeOfAbsoluteVorticity;
        public ScalarField<Face> DerivativeOfDivergence;
        public ScalarField<Face> DerivativeOfHeight;
        public ScalarField<Face> AbsoluteVorticity;
        public ScalarField<Face> Divergence;
        public ScalarField<Face> Height;
        public ScalarField<Face> Streamfunction;
        public ScalarField<Face> VelocityPotential;

        private readonly IPolyhedron _polyhedron;

        public PrognosticFieldsFactory(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;
            ZeroAllFields();
        }

        private void ZeroAllFields()
        {
            var allFields = typeof(PrognosticFieldsFactory).GetFields();
            var numericFields = allFields.Where(field => field.FieldType == typeof(ScalarField<Face>)).ToArray();

            foreach (var field in numericFields)
            {
                field.SetValue(this, ConstantField(0));
            }
        }

        public PrognosticFields<Face> Build()
        {
            var fields = new PrognosticFields<Face>
            {
                DerivativeOfAbsoluteVorticity = DerivativeOfAbsoluteVorticity,
                DerivativeOfDivergence = DerivativeOfDivergence,
                DerivativeOfHeight = DerivativeOfHeight,
                AbsoluteVorticity = AbsoluteVorticity,
                Divergence = Divergence,
                Height = Height,
                Streamfunction = Streamfunction,
                VelocityPotential = VelocityPotential,
            };

            return fields;
        }

        public ScalarField<Face> ConstantField(double value)
        {
            var values = Enumerable.Repeat(value, _polyhedron.Faces.Count).ToArray();

            return new ScalarField<Face>(_polyhedron.IndexOf, values);
        }

        public ScalarField<Face> ZDependentField(double average, double deviation)
        {
            var normals = SimulationUtilities.NormalsTable(_polyhedron);
            var values = _polyhedron.Faces.Select(face => average + deviation*normals[_polyhedron.IndexOf(face)][2]).ToArray();

            return new ScalarField<Face>(_polyhedron.IndexOf, values);
        }

        public ScalarField<Face> XDependentField(double average, double deviation)
        {
            var normals = SimulationUtilities.NormalsTable(_polyhedron);
            var values = _polyhedron.Faces.Select(face => average + deviation * normals[_polyhedron.IndexOf(face)][0]).ToArray();

            return new ScalarField<Face>(_polyhedron.IndexOf, values);
        }

        public ScalarField<Face> RandomField(double average, double deviation)
        {
            var prng = new Random();
            var values = _polyhedron.Faces.Select(face => average + deviation * (prng.NextDouble()-0.5)).ToArray();

            return new ScalarField<Face>(_polyhedron.IndexOf, values);
        }
    }
}
