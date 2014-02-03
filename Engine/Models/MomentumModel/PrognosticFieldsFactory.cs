using System.Linq;
using Engine.Polyhedra;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Models.MomentumModel
{
    public class PrognosticFieldsFactory
    {
        public ScalarField<Face> DerivativeOfHeight;
        public VectorField<Vertex> DerivativeOfVelocity;
        public ScalarField<Face> Height;
        public VectorField<Vertex> Velocity;

        private readonly IPolyhedron _polyhedron;

        public PrognosticFieldsFactory(IPolyhedron polyhedron)
        {
            _polyhedron = polyhedron;
            ZeroAllFields();
        }

        private void ZeroAllFields()
        {
            var allFields = typeof(VorticityDivergenceModel.PrognosticFieldsFactory).GetFields();
            var scalarFields = allFields.Where(field => field.FieldType == typeof(ScalarField<Face>)).ToArray();
            var vectorFields = allFields.Where(field => field.FieldType == typeof(VectorField<Vertex>)).ToArray();

            foreach (var field in scalarFields)
            {
                field.SetValue(this, ConstantScalarField(0));
            }
            foreach (var field in vectorFields)
            {
                field.SetValue(this, ConstantVectorField(Vector.Zeros(3)));
            }
        }

        public PrognosticFields Build()
        {
            var fields = new PrognosticFields
            {
                DerivativeOfVelocity = DerivativeOfVelocity,
                DerivativeOfHeight = DerivativeOfHeight,
                Height = Height,
                Velocity = Velocity,
            };

            return fields;
        }

        public ScalarField<Face> ConstantScalarField(double value)
        {
            var values = Enumerable.Repeat(value, _polyhedron.Faces.Count).ToArray();

            return new ScalarField<Face>(_polyhedron.IndexOf, values);
        }

        public VectorField<Vertex> ConstantVectorField(Vector value)
        {
            var values = Enumerable.Repeat(value, _polyhedron.Faces.Count).ToArray();

            return new VectorField<Vertex>(_polyhedron.IndexOf, values);
        }
    }
}
