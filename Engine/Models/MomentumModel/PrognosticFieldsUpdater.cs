using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Models.VorticityDivergenceModel;
using Engine.Polyhedra;

namespace Engine.Models.MomentumModel
{
    public class PrognosticFieldsUpdater
    {
        private readonly SimulationParameters _parameters;
        private readonly ScalarField<Face> _coriolisField;
        private readonly VectorField<Face> _normalsField; 

        private readonly VectorFieldOperators _operators;


        public PrognosticFieldsUpdater(IPolyhedron surface, SimulationParameters parameters)
        {
            _parameters = parameters;
            _coriolisField = SimulationUtilities.CoriolisField(surface, _parameters.RotationFrequency);
            _normalsField = SimulationUtilities.NormalsField(surface);

            _operators = new VectorFieldOperators(surface);

        }

        public PrognosticFields Update(PrognosticFields fields, PrognosticFields oldFields = null, PrognosticFields olderFields = null)
        {
            var derivativeOfHeight = DerivativeOfHeight(fields.Velocity, fields.Height);
            var derivativeOfVelocity = DerivativeOfVelocity(fields.Velocity, fields.Height);

            throw new NotImplementedException();
        }

        private VectorField<Vertex> DerivativeOfVelocity(VectorField<Vertex> velocity, ScalarField<Face> height)
        {
            var absoluteVorticity = _operators.VertexAverages(AbsoluteVorticity(velocity));

            throw new NotImplementedException();
        }

        private ScalarField<Face> DerivativeOfHeight(VectorField<Vertex> velocity, ScalarField<Face> height)
        {
            return -_operators.FluxDivergence(velocity, height);
        }

        private ScalarField<Face> AbsoluteVorticity(VectorField<Vertex> velocity)
        {
            var curlOfVelocity = _operators.Curl(velocity);
            var absoluteVorticity = _coriolisField + VectorField<Face>.ScalarProduct(_normalsField, curlOfVelocity);

            return absoluteVorticity;
        }
    }
}
