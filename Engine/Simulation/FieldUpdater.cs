using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;

namespace Engine.Simulation
{
    public class FieldUpdater
    {
        private readonly FieldOperators _operators;
        private readonly FieldIntegrator _integrator;
        private readonly SimulationParameters _parameters;

        public FieldUpdater(IPolyhedron surface, Dictionary<Face, int> index, SimulationParameters parameters)
        {
            _operators = new FieldOperators(surface, index);
            _integrator = new FieldIntegrator(surface, index, parameters);
            _parameters = parameters;
        }

        public PrognosticFields<Face> Update
            (PrognosticFields<Face> fields, PrognosticFields<Face> oldFields, PrognosticFields<Face> olderFields)
        {
            // Instantaneous derivatives.
            var derivativeOfAbsoluteVorticity = DerivativeOfAbsoluteVorticity(
                fields.AbsoluteVorticity, 
                fields.Streamfunction,
                fields.VelocityPotential);

            var derivativeOfDivergence = DerivativeOfDivergence(
                fields.AbsoluteVorticity, 
                fields.Height,
                fields.Streamfunction,
                fields.VelocityPotential);

            var derivativeOfHeight = DerivativeOfHeight(
                fields.Height, 
                fields.Streamfunction, 
                fields.VelocityPotential);

            // New fields based on averaged derivatives.
            var absoluteVorticity = AdamsBashforthUpdate(
                fields.AbsoluteVorticity,
                derivativeOfAbsoluteVorticity,
                oldFields.DerivativeOfAbsoluteVorticity,
                olderFields.DerivativeOfAbsoluteVorticity);

            var divergence = AdamsBashforthUpdate(
                fields.Divergence,
                derivativeOfDivergence,
                oldFields.DerivativeOfDivergence,
                olderFields.DerivativeOfDivergence);

            var height = AdamsBashforthUpdate(
                fields.Height,
                derivativeOfHeight, 
                oldFields.DerivativeOfHeight, 
                olderFields.DerivativeOfHeight);

            // Integral fields.
            var streamfunction = NewStreamfunction(fields.Streamfunction, absoluteVorticity, _parameters.Coriolis);
            var velocityPotential = NewVelocityPotential(fields.Streamfunction, divergence);

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
            var derivative = 
                - _operators.FluxDivergence(absoluteVorticity, velocityPotential)
                + _operators.Jacobian(absoluteVorticity, streamfunction);

            return derivative;
        }

        private ScalarField<Face> DerivativeOfDivergence
            (ScalarField<Face> absoluteVorticity, ScalarField<Face> height, ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            var energy = KineticEnergy(streamfunction, velocityPotential);
            var gravity = _parameters.Gravity;

            var derivative = 
                  _operators.FluxDivergence(absoluteVorticity, streamfunction)
                + _operators.Jacobian(absoluteVorticity, velocityPotential)
                - _operators.Laplacian(energy + gravity*height);

            return derivative;
        }

        private ScalarField<Face> KineticEnergy
            (ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            var energy = 0.5 * (
                  _operators.FluxDivergence(streamfunction, streamfunction) - _operators.Laplacian(streamfunction)
                + _operators.FluxDivergence(velocityPotential, velocityPotential) - _operators.Laplacian(velocityPotential)
                + _operators.Jacobian(streamfunction, velocityPotential)); //TODO: Is this the right sign?

            return energy;
        }

        private ScalarField<Face> DerivativeOfHeight
            (ScalarField<Face> height, ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            var derivative =
                - _operators.FluxDivergence(height, velocityPotential)
                + _operators.Jacobian(height, streamfunction);

            return derivative;
        }

        private ScalarField<Face> AdamsBashforthUpdate
            (ScalarField<Face> field, ScalarField<Face> derivative, ScalarField<Face> oldDerivative, ScalarField<Face> olderDerivative)
        {
            var step = 1.0/12.0*(23*derivative - 16*oldDerivative + 5*olderDerivative);
            var newField = field + _parameters.Timestep*step;

            return newField;
        }

        private ScalarField<Face> NewStreamfunction(ScalarField<Face> streamfunction, ScalarField<Face> absoluteVorticity, double coriolisParameter)
        {
            return _integrator.Integrate(streamfunction, absoluteVorticity - coriolisParameter);
        }

        private ScalarField<Face> NewVelocityPotential(ScalarField<Face> velocityPotential, ScalarField<Face> divergence)
        {
            return _integrator.Integrate(velocityPotential, divergence);
        }
    }
}
