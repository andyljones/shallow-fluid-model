using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Polyhedra;
using Engine.Simulation;

namespace Engine.Simulation2
{
    public class VectorFieldOperators
    {

        public VectorField<Vertex> Gradient(ScalarField<Face> field)
        {
            throw new NotImplementedException();
        }

        public ScalarField<Face> Divergence(VectorField<Face> field)
        {
            throw new NotImplementedException();
        }

        public VectorField<Face> Curl(VectorField<Face> field)
        {
            throw new NotImplementedException();
        }
    }
}
