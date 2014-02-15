using Engine.Geometry;

namespace Engine.Simulation
{
    public class PrognosticFieldsUpdater
    {
        private readonly IModelParameters _options;
        private readonly ScalarField<Face> _coriolisField;
        private readonly VectorField<Face> _faceNormalsField;
        private readonly VectorField<Vertex> _vertexNormalsField; 

        private readonly VectorFieldOperators _operators;

        private readonly double _gravity;


        public PrognosticFieldsUpdater(IPolyhedron surface, IModelParameters options)
        {
            _options = options;
            _coriolisField = SimulationUtilities.CoriolisField(surface, _options.RotationFrequency);
            _faceNormalsField = SimulationUtilities.FaceNormalsField(surface);
            _vertexNormalsField = SimulationUtilities.VertexNormalsField(surface);

            _operators = new VectorFieldOperators(surface);

            _gravity = options.Gravity;
        }

        public PrognosticFields Update(PrognosticFields fields, PrognosticFields oldFields = null, PrognosticFields olderFields = null)
        {
            var derivativeOfHeight = DerivativeOfHeight(fields.Velocity, fields.Height);
            var derivativeOfVelocity = DerivativeOfVelocity(fields.Velocity, fields.Height);

            ScalarField<Face> height;
            VectorField<Vertex> velocity;
            if (oldFields != null && olderFields != null)
            {
                height = NumericalDerivatives.AdamsBashforth(_options.Timestep, fields.Height, derivativeOfHeight, oldFields.DerivativeOfHeight, olderFields.DerivativeOfHeight);
                velocity = NumericalDerivatives.AdamsBashforth(_options.Timestep, fields.Velocity, derivativeOfVelocity, oldFields.DerivativeOfVelocity, olderFields.DerivativeOfVelocity);
            }
            else
            {
                height = NumericalDerivatives.Euler(_options.Timestep, fields.Height, derivativeOfHeight);
                velocity = NumericalDerivatives.Euler(_options.Timestep, fields.Velocity, derivativeOfVelocity);
            }

            var newFields = new PrognosticFields
            {
                DerivativeOfHeight = derivativeOfHeight,
                DerivativeOfVelocity = derivativeOfVelocity,
                Height = height,
                Velocity = velocity
            };

            return newFields;
        }

        private VectorField<Vertex> DerivativeOfVelocity(VectorField<Vertex> velocity, ScalarField<Face> height)
        {
            var absoluteVorticity = _operators.VertexAverages(AbsoluteVorticity(velocity));
            var curlOfVelocity = VectorField<Vertex>.CrossProduct(_vertexNormalsField, velocity);

            var kineticEnergyGradient = _operators.Gradient(_operators.KineticEnergy(velocity));

            var potentialEnergyGradient = _operators.Gradient(_gravity*height);

            return (-absoluteVorticity)*curlOfVelocity - kineticEnergyGradient - potentialEnergyGradient;
        }

        private ScalarField<Face> DerivativeOfHeight(VectorField<Vertex> velocity, ScalarField<Face> height)
        {
            return -_operators.FluxDivergence(velocity, height);
        }

        private ScalarField<Face> AbsoluteVorticity(VectorField<Vertex> velocity)
        {
            var curlOfVelocity = _operators.Curl(velocity);
            var absoluteVorticity = _coriolisField + VectorField<Face>.ScalarProduct(_faceNormalsField, curlOfVelocity);

            return absoluteVorticity;
        }
    }
}
