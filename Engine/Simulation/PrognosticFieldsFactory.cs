using System;
using System.Linq;
using Engine.Geometry;
using Engine.Utilities;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Simulation
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
            var allFields = typeof(PrognosticFieldsFactory).GetFields();
            var scalarFields = allFields.Where(field => field.FieldType == typeof(ScalarField<Face>)).ToArray();
            var vectorFields = allFields.Where(field => field.FieldType == typeof(VectorField<Vertex>)).ToArray();

            foreach (var field in scalarFields)
            {
                field.SetValue(this, ConstantScalarField(0));
            }
            foreach (var field in vectorFields)
            {
                field.SetValue(this, ConstantVectorField(0, 0));
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

        public ScalarField<Face> RandomScalarField(double average, double deviation)
        {
            var prng = new Random();
            var values = Enumerable.Repeat(deviation, _polyhedron.Faces.Count).Select(i => average + (prng.NextDouble()-0.5)*deviation).ToArray();

            return new ScalarField<Face>(_polyhedron.IndexOf, values);
        }

        public ScalarField<Face> XDependentScalarField(double average, double deviation)
        {
            var normals = FaceIndexedTableFactory.Normals(_polyhedron);
            var values = _polyhedron.Faces.Select(face => average + deviation * normals[_polyhedron.IndexOf(face)][0]).ToArray();

            return new ScalarField<Face>(_polyhedron.IndexOf, values);
        }

        public VectorField<Vertex> ConstantVectorField(double east, double north)
        {
            var values = _polyhedron.Vertices.Select(vertex => LocalVector(vertex.Position, east, north)).ToArray();

            return new VectorField<Vertex>(_polyhedron.IndexOf, values);
        }

        public VectorField<Vertex> RandomVectorField(double deviation)
        {
            var prng = new Random();
            var values = _polyhedron.Vertices.Select(vertex => deviation*LocalVector(vertex.Position, prng.NextDouble()-0.5, prng.NextDouble()-0.5)).ToArray();

            return new VectorField<Vertex>(_polyhedron.IndexOf, values);
        }

        private Vector LocalVector(Vector origin, double east, double north)
        {
            var globalNorth = VectorUtilities.NewVector(0, 0, 1);

            var localEast = Vector.CrossProduct(globalNorth, origin).Normalize();
            var localNorth = Vector.CrossProduct(origin, localEast).Normalize();

            return north * localNorth + east * localEast;

        }
    }
}
