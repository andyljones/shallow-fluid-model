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

        public FieldUpdater(Polyhedron surface, Dictionary<Face, int> index, SimulationParameters parameters)
        {
            _operators = new FieldOperators(surface, index);
            _integrator = new FieldIntegrator(surface);
            _parameters = parameters;
        }

        public PrognosticFields<Face> Update(PrognosticFields<Face> oldFields, PrognosticFields<Face> olderFields)
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

        private ScalarField<Face> DerivativeOfAbsoluteVorticity(ScalarField<Face> absoluteVorticity, ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            throw new NotImplementedException();
        }

        private ScalarField<Face> DerivativeOfDivergence(ScalarField<Face> divergence, ScalarField<Face> height, ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            throw new NotImplementedException();
        }

        private ScalarField<Face> DerivativeOfHeight(ScalarField<Face> derivativeOfHeight, ScalarField<Face> scalarField, ScalarField<Face> derivativeOfHeight1)
        {
            throw new NotImplementedException();
        }

        private ScalarField<Face> NewAbsoluteVorticity(ScalarField<Face> derivativeOfAbsoluteVorticity, ScalarField<Face> scalarField, ScalarField<Face> derivativeOfAbsoluteVorticity1)
        {
            throw new NotImplementedException();
        }

        private ScalarField<Face> NewDivergence(ScalarField<Face> derivativeOfDivergence, ScalarField<Face> scalarField, ScalarField<Face> derivativeOfDivergence1)
        {
            throw new NotImplementedException();
        }

        private ScalarField<Face> NewStreamfunction(ScalarField<Face> absoluteVorticity, double coriolisParameter)
        {
            throw new NotImplementedException();
        }

        private ScalarField<Face> NewVelocityPotential(ScalarField<Face> divergence)
        {
            throw new NotImplementedException();
        }

        private ScalarField<Face> NewHeight(ScalarField<Face> height, ScalarField<Face> streamfunction, ScalarField<Face> velocityPotential)
        {
            throw new NotImplementedException();
        }
    }
}
