using Engine.Polyhedra;

namespace Engine.Models.VorticityDivergenceModel
{
    public class PrognosticFieldsUpdater
    {
        private readonly FieldOperators _operators;
        private readonly FieldIntegrator _integrator;
        private readonly SimulationParameters _parameters;

        private readonly ScalarField<Face> _coriolisField; 

        public PrognosticFieldsUpdater(IPolyhedron surface, SimulationParameters parameters)
        {
            _operators = new FieldOperators(surface);
            _integrator = new FieldIntegrator(surface, parameters);
            _parameters = parameters;

            _coriolisField = SimulationUtilities.CoriolisField(surface, _parameters.RotationFrequency);
        }

        public PrognosticFields<Face> Update(PrognosticFields<Face> fields, PrognosticFields<Face> oldFields = null, PrognosticFields<Face> olderFields = null)
        {
            // Instantaneous derivatives.
            var derivativeOfAbsoluteVorticity = DerivativeOfAbsoluteVorticity(fields.AbsoluteVorticity, fields.Streamfunction, fields.VelocityPotential);
            var derivativeOfDivergence = DerivativeOfDivergence(fields.AbsoluteVorticity, fields.Height, fields.Streamfunction, fields.VelocityPotential);
            var derivativeOfHeight = DerivativeOfHeight(fields.Height, fields.Streamfunction, fields.VelocityPotential);

            // New fields based on averaged derivatives.
            ScalarField<Face> absoluteVorticity;
            ScalarField<Face> divergence;
            ScalarField<Face> height;
            if (oldFields != null && olderFields != null)
            {
                absoluteVorticity = AdamsBashforthUpdate(fields.AbsoluteVorticity, derivativeOfAbsoluteVorticity, oldFields.DerivativeOfAbsoluteVorticity, olderFields.DerivativeOfAbsoluteVorticity);
                divergence = AdamsBashforthUpdate(fields.Divergence, derivativeOfDivergence, oldFields.DerivativeOfDivergence, olderFields.DerivativeOfDivergence);
                height = AdamsBashforthUpdate(fields.Height, derivativeOfHeight, oldFields.DerivativeOfHeight, olderFields.DerivativeOfHeight);
            }
            else
            {
                absoluteVorticity = EulerUpdate(fields.AbsoluteVorticity, derivativeOfAbsoluteVorticity);
                divergence = EulerUpdate(fields.Divergence, derivativeOfDivergence);
                height = EulerUpdate(fields.Height, derivativeOfHeight);
            }

            // Integral fields.
            var streamfunction = NewStreamfunction(fields.Streamfunction, absoluteVorticity);
            var velocityPotential = NewVelocityPotential(fields.VelocityPotential, divergence);

            var newFields = new PrognosticFields<Face>
            {
                DerivativeOfAbsoluteVorticity = derivativeOfAbsoluteVorticity,
                DerivativeOfDivergence = derivativeOfDivergence,
                DerivativeOfHeight = derivativeOfHeight,

                AbsoluteVorticity = absoluteVorticity,
                Divergence = divergence,
                Height = height,
                
                Streamfunction = streamfunction,
                VelocityPotential = velocityPotential
            };

            return newFields;
        }

        private ScalarField<Face> DerivativeOfAbsoluteVorticity
            (ScalarField<Face> absoluteVorticity, ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            var derivative = - _operators.FluxDivergence(absoluteVorticity, velocityPotential) + _operators.Jacobian(absoluteVorticity, streamfunction);

            return derivative;
        }

        private ScalarField<Face> DerivativeOfDivergence
            (ScalarField<Face> absoluteVorticity, ScalarField<Face> height, ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            var energy = KineticEnergy(streamfunction, velocityPotential);
            var gravity = _parameters.Gravity;

            var derivative = _operators.FluxDivergence(absoluteVorticity, streamfunction) + _operators.Jacobian(absoluteVorticity, velocityPotential) - _operators.Laplacian(energy + gravity*height);

            return derivative;
        }

        private ScalarField<Face> KineticEnergy
            (ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            var streamfunctionTerm = _operators.FluxDivergence(streamfunction, streamfunction) - _operators.Laplacian(streamfunction);
            var velocityPotentialTerm =  _operators.FluxDivergence(velocityPotential, velocityPotential) - _operators.Laplacian(velocityPotential);

            var energy = 0.5 * (streamfunctionTerm + velocityPotentialTerm) + _operators.Jacobian(streamfunction, velocityPotential); //TODO: Is this the right sign?

            return energy;
        }

        private ScalarField<Face> DerivativeOfHeight
            (ScalarField<Face> height, ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            var derivative = - _operators.FluxDivergence(height, velocityPotential) + _operators.Jacobian(height, streamfunction);

            return derivative;
        }

        private ScalarField<Face> AdamsBashforthUpdate
            (ScalarField<Face> field, ScalarField<Face> derivative, ScalarField<Face> oldDerivative, ScalarField<Face> olderDerivative)
        {
            var step = 1.0/12.0*(23*derivative - 16*oldDerivative + 5*olderDerivative);
            var newField = field + _parameters.Timestep*step;

            return newField;
        }


        private ScalarField<Face> EulerUpdate(ScalarField<Face> field, ScalarField<Face> derivative)
        {
            var step = derivative;
            var newField = field + _parameters.Timestep*step;

            return newField;
        }


        private ScalarField<Face> NewStreamfunction(ScalarField<Face> streamfunction, ScalarField<Face> absoluteVorticity)
        {
            return _integrator.Integrate(streamfunction, absoluteVorticity - _coriolisField);
        }

        private ScalarField<Face> NewVelocityPotential(ScalarField<Face> velocityPotential, ScalarField<Face> divergence)
        {
            return _integrator.Integrate(velocityPotential, divergence);
        }
    }
}
