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

        public FieldUpdater(Polyhedron surface, SimulationParameters parameters)
        {
            _operators = new FieldOperators(surface);
            _integrator = new FieldIntegrator(surface);
            _parameters = parameters;
        }

        public PrognosticFields Update(PrognosticFields oldFields, PrognosticFields olderFields)
        {
            var derivativeOfAbsoluteVorticity = DerivativeOfAbsoluteVorticity(
                oldFields.AbsoluteVorticity, 
                oldFields.Streamfunction,
                oldFields.VelocityPotential);

            var derivativeOfDivergence = DerivativeOfDivergence(
                oldFields.Divergence, 
                oldFields.Height,
                oldFields.Streamfunction,
                oldFields.VelocityPotential);

            var derivativeOfHeight = DerivativeOfHeight(
                oldFields.Height, 
                oldFields.Streamfunction, 
                oldFields.VelocityPotential);


            var absoluteVorticity = NewAbsoluteVorticity(
                derivativeOfAbsoluteVorticity,
                oldFields.DerivativeOfAbsoluteVorticity,
                olderFields.DerivativeOfAbsoluteVorticity);

            var divergence = NewDivergence(
                derivativeOfDivergence,
                oldFields.DerivativeOfDivergence,
                olderFields.DerivativeOfDivergence);

            var height = NewHeight(
                derivativeOfHeight, 
                oldFields.DerivativeOfHeight, 
                olderFields.DerivativeOfHeight);


            var streamfunction = NewStreamfunction(absoluteVorticity, _parameters.CoriolisParameter);
            var velocityPotential = NewVelocityPotential(divergence);

            var newFields = new PrognosticFields
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

        private ScalarField DerivativeOfAbsoluteVorticity(ScalarField absoluteVorticity, ScalarField streamfunction, ScalarField velocityPotential)
        {
            throw new NotImplementedException();
        }

        private ScalarField DerivativeOfDivergence(ScalarField divergence, ScalarField height, ScalarField streamfunction, ScalarField velocityPotential)
        {
            throw new NotImplementedException();
        }

        private ScalarField DerivativeOfHeight(ScalarField derivativeOfHeight, ScalarField scalarField, ScalarField derivativeOfHeight1)
        {
            throw new NotImplementedException();
        }

        private ScalarField NewAbsoluteVorticity(ScalarField derivativeOfAbsoluteVorticity, ScalarField scalarField, ScalarField derivativeOfAbsoluteVorticity1)
        {
            throw new NotImplementedException();
        }

        private ScalarField NewDivergence(ScalarField derivativeOfDivergence, ScalarField scalarField, ScalarField derivativeOfDivergence1)
        {
            throw new NotImplementedException();
        }

        private ScalarField NewStreamfunction(ScalarField absoluteVorticity, double coriolisParameter)
        {
            throw new NotImplementedException();
        }

        private ScalarField NewVelocityPotential(ScalarField divergence)
        {
            throw new NotImplementedException();
        }

        private ScalarField NewHeight(ScalarField height, object streamfunction, object velocityPotential)
        {
            throw new NotImplementedException();
        }
    }
}
