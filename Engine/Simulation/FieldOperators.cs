using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;

namespace Engine.Simulation
{
    public class FieldOperators
    {
        public FieldOperators(Polyhedron surface)
        {
            throw new NotImplementedException();            
        }

        public ScalarField Jacobian(ScalarField a, ScalarField b)
        {
            throw new NotImplementedException();
        }
        
        public ScalarField FluxDivergence(ScalarField a, ScalarField b)
        {
            throw new NotImplementedException();
        }

        public ScalarField Laplacian(ScalarField a)
        {
            throw new NotImplementedException();
        }
    }
}
